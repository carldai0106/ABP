using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Application.User.Dto
{
    public enum LoginResultType
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,
        InvalidPassword,
        UserIsNotActive,

        InvalidTenancyName,
        TenantIsNotActive,
        UserEmailIsNotConfirmed
    }
}
