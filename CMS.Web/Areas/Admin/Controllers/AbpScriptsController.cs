using System;
using Abp.Web.Authorization;
using Abp.Web.Localization;
using Abp.Web.MultiTenancy;
using Abp.Web.Mvc.Controllers;
using Abp.Web.Navigation;
using Abp.Web.Sessions;
using Abp.Web.Settings;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class AbpScriptsController : AbpScriptsController<Guid, Guid>
    {
        public AbpScriptsController(IMultiTenancyScriptManager
            multiTenancyScriptManager,
            ISettingScriptManager<Guid, Guid> settingScriptManager,
            INavigationScriptManager<Guid, Guid> navigationScriptManager,
            ILocalizationScriptManager localizationScriptManager,
            IAuthorizationScriptManager<Guid, Guid> authorizationScriptManager,
            ISessionScriptManager<Guid, Guid> sessionScriptManager) :
                base(multiTenancyScriptManager,
                    settingScriptManager,
                    navigationScriptManager,
                    localizationScriptManager,
                    authorizationScriptManager,
                    sessionScriptManager)
        {
        }
    }
}