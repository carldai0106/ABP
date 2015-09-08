using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.Role.Dto;

namespace CMS.Application.Role
{
    public interface IRoleAppService
    {
        Task Create(RoleCreateDto input);
        Task Update(RoleEditDto input);
        Task Delete(IdInput<Guid> input);
        Task<RoleEditDto> GetRole(string roleCode);
        Task<RoleEditDto> GetRole(IdInput<Guid> input);
        Task<PagedResultOutput<RoleEditDto>> GetRoles(GetRolesInput input);
        Task CreateOrUpdate(IEnumerable<RoleRightDto> inputs);
    }
}