using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.User;

namespace CMS.Application.Sessions.Dto
{
    [AutoMapFrom(typeof (UserEntity))]
    public class UserLoginInfoDto : EntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}