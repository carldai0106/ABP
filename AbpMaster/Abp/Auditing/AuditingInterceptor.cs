using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.Json;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Timing;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Abp.Auditing
{
    internal class AuditingInterceptor<TTenantId, TUserId> : IInterceptor
        where TTenantId : struct
        where TUserId : struct
    {
	    public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }
        public ILogger Logger { get; set; }
        public IAuditingStore<TTenantId, TUserId> AuditingStore { get; set; }
       
        private readonly IAuditingConfiguration _configuration; 		
		private readonly IAuditInfoProvider _auditInfoProvider;
        private readonly IUnitOfWorkManager<TTenantId, TUserId> _unitOfWorkManager;

        public AuditingInterceptor(IAuditingConfiguration configuration, IAuditInfoProvider auditInfoProvider,
            IUnitOfWorkManager<TTenantId, TUserId> unitOfWorkManager)
        {
            _configuration = configuration;
            _auditInfoProvider = auditInfoProvider;
            _unitOfWorkManager = unitOfWorkManager;

            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            Logger = NullLogger.Instance;
            AuditingStore = SimpleLogAuditingStore<TTenantId, TUserId>.Instance;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!AuditingHelper.ShouldSaveAudit(invocation.MethodInvocationTarget, _configuration, AbpSession))
            {
                invocation.Proceed();
                return;
            }

            var auditInfo = CreateAuditInfo(invocation);

            if (AsyncHelper.IsAsyncMethod(invocation.Method))
            {
                PerformAsyncAuditing(invocation, auditInfo);
            }
            else
            {
                PerformSyncAuditing(invocation, auditInfo);
            }
        }

        private AuditInfo<TTenantId, TUserId> CreateAuditInfo(IInvocation invocation)
        {
            var auditInfo = new AuditInfo<TTenantId, TUserId>
            {
                TenantId = AbpSession.TenantId,
                UserId = AbpSession.UserId,
                ServiceName = invocation.MethodInvocationTarget.DeclaringType != null
                    ? invocation.MethodInvocationTarget.DeclaringType.FullName
                    : "",
                MethodName = invocation.MethodInvocationTarget.Name,
                Parameters = ConvertArgumentsToJson(invocation),
                ExecutionTime = Clock.Now
            };

            _auditInfoProvider.Fill(auditInfo);

            return auditInfo;
        }

        private void PerformSyncAuditing(IInvocation invocation, AuditInfo<TTenantId, TUserId> auditInfo)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                AuditingStore.Save(auditInfo);
            }
        }

        private void PerformAsyncAuditing(IInvocation invocation, AuditInfo<TTenantId, TUserId> auditInfo)
        {
            var stopwatch = Stopwatch.StartNew();

            invocation.Proceed();

            if (invocation.Method.ReturnType == typeof (Task))
            {
                invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithFinally(
                    (Task) invocation.ReturnValue,
                    exception => SaveAuditInfo(auditInfo, stopwatch, exception)
                    );
            }
            else //Task<TResult>
            {
                invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
                    invocation.Method.ReturnType.GenericTypeArguments[0],
                    invocation.ReturnValue,
                    exception => SaveAuditInfo(auditInfo, stopwatch, exception)
                    );
            }
        }

        private string ConvertArgumentsToJson(IInvocation invocation)
        {
            try
            {
                var parameters = invocation.MethodInvocationTarget.GetParameters();
                if (parameters.IsNullOrEmpty())
                {
                    return "{}";
                }

                var dictionary = new Dictionary<string, object>();
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var argument = invocation.Arguments[i];
                    dictionary[parameter.Name] = argument;
                }

                return JsonHelper.ConvertToJson(dictionary, true);
            }
            catch (Exception ex)
            {
                Logger.Warn("Could not serialize arguments for method: " + invocation.MethodInvocationTarget.Name);
                Logger.Warn(ex.ToString(), ex);
                return "{}";
            }
        }

        private void SaveAuditInfo(AuditInfo<TTenantId, TUserId> auditInfo, Stopwatch stopwatch, Exception exception)
        {
            stopwatch.Stop();
            auditInfo.Exception = exception;
            auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);

            using (var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                AuditingStore.Save(auditInfo);
                uow.Complete();
            }
        }
    }
}