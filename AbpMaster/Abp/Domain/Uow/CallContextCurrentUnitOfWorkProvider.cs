using System;
using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using Abp.Dependency;
using Castle.Core;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// CallContext implementation of <see cref="ICurrentUnitOfWorkProvider"/>. 
    /// This is the default implementation.
    /// </summary>
    public class CallContextCurrentUnitOfWorkProvider<TTenantId, TUserId> : ICurrentUnitOfWorkProvider<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private const string ContextKey = "Abp.UnitOfWork.Current";

        //TODO: Clear periodically..?
        private static readonly ConcurrentDictionary<string, IUnitOfWork<TTenantId, TUserId>> UnitOfWorkDictionary
            = new ConcurrentDictionary<string, IUnitOfWork<TTenantId, TUserId>>();

        internal static IUnitOfWork<TTenantId, TUserId> StaticUow
        {
            get
            {
                var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
                if (unitOfWorkKey == null)
                {
                    return null;
                }

                IUnitOfWork<TTenantId, TUserId> unitOfWork;
                if (!UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
                {
                    CallContext.LogicalSetData(ContextKey, null);
                    return null;
                }

                if (unitOfWork.IsDisposed)
                {
                    CallContext.LogicalSetData(ContextKey, null);
                    UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                    return null;
                }

                return unitOfWork;
            }

            set
            {
                var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
                if (unitOfWorkKey != null)
                {
                    IUnitOfWork<TTenantId, TUserId> unitOfWork;
                    if (UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
                    {
                        if (unitOfWork == value)
                        {
                            //Setting same object, no need to set again
                            return;
                        }

                        UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                    }

                    CallContext.LogicalSetData(ContextKey, null);
                }

                if (value == null)
                {
                    //It's already null (because of the logic above), no need to set
                    return;
                }

                unitOfWorkKey = Guid.NewGuid().ToString();
                if (!UnitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
                {
                    //This is almost impossible, but we're checking.
                    throw new AbpException("Can not set unit of work!");
                }

                CallContext.LogicalSetData(ContextKey, unitOfWorkKey);
            }
        }

        /// <inheritdoc />
        [DoNotWire]
        public IUnitOfWork<TTenantId, TUserId> Current
        {
            get { return StaticUow; }
            set { StaticUow = value; }
        }
    }
}