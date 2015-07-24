﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CMS.Application.User.Dto;

namespace CMS.Application.User
{
    public interface IUserAppService : IApplicationService
    {
        Task CreateUser(CreateUserDto input);

        Task<UserEditDto> GetUser(string userName);

        Task<UserEditDto> GetUser(NullableIdInput<Guid> input);

        Task UpdateUser(UserEditDto input);

        Task DeleteUser(IdInput<Guid> input);

        Task<PagedResultOutput<UserEditDto>> GetUsers(GetUsersInput input);

        int Add(int a, int b);
    }
}
