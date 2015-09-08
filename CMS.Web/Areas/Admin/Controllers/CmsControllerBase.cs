using System;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Abp.Web.Mvc.Controllers;
using CMS.Application.Localization;

namespace CMS.Web.Areas.Admin.Controllers
{
    [AbpMvcAuthorize]
    public abstract class CmsControllerBase : AbpController<Guid, Guid>
    {
        protected CmsControllerBase()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
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