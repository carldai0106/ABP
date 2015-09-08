using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;
using Abp.Dependency;
using Castle.Core;
using Castle.Core.Logging;

namespace Abp.Domain.Uow
{
    /// <summary>
    ///     CallContext implementation of <see cref="ICurrentUnitOfWorkProvider{TTenantId,TUserId}" />.
    ///     This is the default implementation.
    /// </summary>
    public class CallContextCurrentUnitOfWorkProvider<TTenantId, TUserId> :
        ICurrentUnitOfWorkProvider<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private const string ContextKey = "Abp.UnitOfWork.Current";
        //TODO: Clear periodically..?
        private static readonly ConcurrentDictionary<string, IUnitOfWork<TTenantId, TUserId>> UnitOfWorkDictionary
            = new ConcurrentDictionary<string, IUnitOfWork<TTenantId, TUserId>>();

        public CallContextCurrentUnitOfWorkProvider()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        /// <inheritdoc />
        [DoNotWire]
        public IUnitOfWork<TTenantId, TUserId> Current
        {
            get { return GetCurrentUow(Logger); }
            set { SetCurrentUow(value, Logger); }
        }

        private static IUnitOfWork<TTenantId, TUserId> GetCurrentUow(ILogger logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
            if (unitOfWorkKey == null)
            {
                return null;
            }

            IUnitOfWork<TTenantId, TUserId> unitOfWork;
            if (!UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary!");
                CallContext.FreeNamedDataSlot(ContextKey);
                return null;
            }

            if (unitOfWork.IsDisposed)
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but the UOW was disposed!");
                UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                CallContext.FreeNamedDataSlot(ContextKey);
                return null;
            }

            return unitOfWork;
        }

        private static void SetCurrentUow(IUnitOfWork<TTenantId, TUserId> value, ILogger logger)
        {
            if (value == null)
            {
                ExitFromCurrentUowScope(logger);
                return;
            }

            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;
            if (unitOfWorkKey != null)
            {
                IUnitOfWork<TTenantId, TUserId> outer;
                if (UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out outer))
                {
                    if (outer == value)
                    {
                        logger.Warn("Setting the same UOW to the CallContext, no need to set again!");
                        return;
                    }

                    value.Outer = outer;
                }
            }

            unitOfWorkKey = value.Id;
            if (!UnitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
            {
                throw new AbpException("Can not set unit of work! UnitOfWorkDictionary.TryAdd returns false!");
            }

            CallContext.LogicalSetData(ContextKey, unitOfWorkKey);
        }

        private static void ExitFromCurrentUowScope(ILogger logger)
        {
            var unitOfWorkKey = CallContext.LogicalGetData(ContextKey) as string;

            if (unitOfWorkKey == null)
            {
                logger.Warn("There is no current UOW to exit!");
                return;
            }

            IUnitOfWork<TTenantId, TUserId> unitOfWork;
            if (!UnitOfWorkDictionary.TryGetValue(unitOfWorkKey, out unitOfWork))
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but not in UnitOfWorkDictionary!");
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            UnitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
            if (unitOfWork.Outer == null)
            {
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            //Restore outer UOW

            var outerUnitOfWorkKey = unitOfWork.Outer.Id;
            if (!UnitOfWorkDictionary.TryGetValue(outerUnitOfWorkKey, out unitOfWork))
            {
                //No outer UOW
                logger.Warn("Outer UOW key could not found in UnitOfWorkDictionary!");
                CallContext.FreeNamedDataSlot(ContextKey);
                return;
            }

            CallContext.LogicalSetData(ContextKey, outerUnitOfWorkKey);
        }
    }
}