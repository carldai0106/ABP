using System;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.AutoMapper;
using CMS.Application.Sessions.Dto;
using CMS.Domain;
using CMS.Domain.Tenant;
using CMS.Domain.User;

namespace CMS.Application.Sessions
{
    public class SessionAppService : CmsAppServiceBase, ISessionAppService
    {
        private readonly ICmsRepository<TenantEntity, Guid> _tenantRepository;
        private readonly ICmsRepository<UserEntity, Guid> _userRepository;

        public SessionAppService(ICmsRepository<UserEntity, Guid> userRepository,
            ICmsRepository<TenantEntity, Guid> tenantRepository)
        {
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var userId = AbpSession.UserId ?? default(Guid);
            var userInfo = await _userRepository.FirstOrDefaultAsync(userId);

            var output = new GetCurrentLoginInformationsOutput
            {
                User = userInfo.MapTo<UserLoginInfoDto>()
            };

            if (AbpSession.TenantId.HasValue)
            {
                var tenantInfo = await _tenantRepository.FirstOrDefaultAsync(AbpSession.TenantId.Value);
                output.Tenant = tenantInfo.MapTo<TenantLoginInfoDto>();
            }

            return output;
        }
    }
}