using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace CMS.Application.User.Dto
{
    public class LoginResultDto : IOutputDto
    {
        public LoginResultType Result { get; private set; }

        public UserEditDto User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public LoginResultDto(LoginResultType result)
        {
            Result = result;
        }

        public LoginResultDto(UserEditDto user, ClaimsIdentity identity)
            : this(LoginResultType.Success)
        {
            User = user;
            Identity = identity;
        }
    }
}
