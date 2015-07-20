using NHibernate;

namespace Abp.NHibernate
{
    public interface ISessionProvider<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        ISession Session { get; }
    }
}