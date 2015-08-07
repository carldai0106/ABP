using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.UI;
using Abp.Web.Mvc.Controllers;

namespace CMS.Web.Areas.Admin.Controllers
{
    public abstract class CmsControllerBase : AbpController<Guid, Guid>
    {
        protected CmsControllerBase()
        {
            //LocalizationSourceName = AbpZeroTemplateConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }
    }
}
