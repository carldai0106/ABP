using Abp.Dependency;
using Abp.Domain.Uow;
using NHibernate;

namespace Abp.NHibernate.Uow
{
    public class UnitOfWorkSessionProvider<TTenantId, TUserId> : ISessionProvider<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public ISession Session
        {
            get { return _unitOfWorkProvider.Current.GetSession<TTenantId, TUserId>(); }
        }

        private readonly ICurrentUnitOfWorkProvider<TTenantId, TUserId> _unitOfWorkProvider;

        public UnitOfWorkSessionProvider(ICurrentUnitOfWorkProvider<TTenantId, TUserId> unitOfWorkProvider)
        {
            _unitOfWorkProvider = unitOfWorkProvider;
        }
    }
}