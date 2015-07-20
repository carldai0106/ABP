using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CMS.Application.IdentityFramework
{
    public class AbpIdentityResult : IdentityResult
    {
        public AbpIdentityResult()
        {

        }

        public AbpIdentityResult(IEnumerable<string> errors)
            : base(errors)
        {

        }

        public AbpIdentityResult(params string[] errors)
            : base(errors)
        {

        }

        public static AbpIdentityResult Failed(params string[] errors)
        {
            return new AbpIdentityResult(errors);
        }
    }
}
