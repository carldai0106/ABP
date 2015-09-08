using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CMS.Application.User.Dto;

namespace CMS.Application.User
{
    public interface IUserAppService : IApplicationService
    {
        Task<Guid> CreateUser(CreateUserDto input);
        Task<UserEditDto> GetUser(string userNameOrEmail);
        Task<UserEditDto> GetUser(NullableIdInput<Guid> input);
        Task UpdateUser(UserEditDto input);
        Task DeleteUser(IdInput<Guid> input);
        Task<PagedResultOutput<UserEditDto>> GetUsers(GetUsersInput input);
        Task CreateOrUpdate(IEnumerable<UserRoleDto> inputs);
        Task<List<PermissionDto>> GetPermission(NullableIdInput<Guid> userId, string moduleCode, string actionCode);
        Task<LoginResultDto> Login(string userNameOrEmailAddress, string plainPassword, string tenancyName = null);
    }
}