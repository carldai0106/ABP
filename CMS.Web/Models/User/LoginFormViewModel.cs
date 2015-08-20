using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Models.User
{
    public class LoginFormViewModel
    {
        public string TenancyName { get; set; }

        public string SuccessMessage { get; set; }

        public string UserNameOrEmailAddress { get; set; }

        public bool IsSelfRegistrationEnabled { get; set; }
    }
}
