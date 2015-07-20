using Abp.Dependency;

namespace Abp.Domain.Uow
{
    /// <summary>
    /// Unit of work manager.
    /// </summary>
    internal class UnitOfWorkManager<TTenantId, TUserId> : IUnitOfWorkManager<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly IIocResolver _iocResolver;
        private readonly ICurrentUnitOfWorkProvider<TTenantId, TUserId> _currentUnitOfWorkProvider;
        private readonly IUnitOfWorkDefaultOptions _defaultOptions;

        public IActiveUnitOfWork Current
        {
            get { return _currentUnitOfWorkProvider.Current; }
        }

        public UnitOfWorkManager(
            IIocResolver iocResolver,
            ICurrentUnitOfWorkProvider<TTenantId, TUserId> currentUnitOfWorkProvider,
            IUnitOfWorkDefaultOptions defaultOptions)
        {
            _iocResolver = iocResolver;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _defaultOptions = defaultOptions;
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            if (_currentUnitOfWorkProvider.Current != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }

            options.FillDefaultsForNonProvidedOptions(_defaultOptions);

            var uow = _iocResolver.Resolve<IUnitOfWork<TTenantId, TUserId>>();

            uow.Completed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                _iocResolver.Release(uow);
            };

            uow.Begin(options);

            _currentUnitOfWorkProvider.Current = uow;

            return uow;
        }
    }
}