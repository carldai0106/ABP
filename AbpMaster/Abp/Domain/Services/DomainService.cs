namespace Abp.Domain.Services
{
    /// <summary>
    ///     This class can be used as a base class for domain services.
    /// </summary>
    public abstract class DomainService<TTenantId, TUserId> : AbpServiceBase<TTenantId, TUserId>, IDomainService
        where TTenantId : struct
        where TUserId : struct
    {
    }
}