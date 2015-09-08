using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Extensions;
using Abp.Web.Authorization;
using Abp.Web.Localization;
using Abp.Web.MultiTenancy;
using Abp.Web.Navigation;
using Abp.Web.Sessions;
using Abp.Web.Settings;

namespace Abp.Web.Mvc.Controllers
{
    /// <summary>
    /// This controller is used to create client side scripts
    /// to work with ABP.
    /// </summary>
    public class AbpScriptsController<TTenantId, TUserId> : AbpController<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly IMultiTenancyScriptManager _multiTenancyScriptManager;
        private readonly ISettingScriptManager<TTenantId, TUserId> _settingScriptManager;
        private readonly INavigationScriptManager<TTenantId, TUserId> _navigationScriptManager;
        private readonly ILocalizationScriptManager _localizationScriptManager;
        private readonly IAuthorizationScriptManager<TTenantId, TUserId> _authorizationScriptManager;
        private readonly ISessionScriptManager<TTenantId, TUserId> _sessionScriptManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AbpScriptsController(
            IMultiTenancyScriptManager multiTenancyScriptManager,
            ISettingScriptManager<TTenantId, TUserId> settingScriptManager,
            INavigationScriptManager<TTenantId, TUserId> navigationScriptManager,
            ILocalizationScriptManager localizationScriptManager,
            IAuthorizationScriptManager<TTenantId, TUserId> authorizationScriptManager,
            ISessionScriptManager<TTenantId, TUserId> sessionScriptManager)
        {
            _multiTenancyScriptManager = multiTenancyScriptManager;
            _settingScriptManager = settingScriptManager;
            _navigationScriptManager = navigationScriptManager;
            _localizationScriptManager = localizationScriptManager;
            _authorizationScriptManager = authorizationScriptManager;
            _sessionScriptManager = sessionScriptManager;
        }

        /// <summary>
        /// Gets all needed scripts.
        /// </summary>
        [DisableAuditing]
        public async Task<ActionResult> GetScripts(string culture = "")
        {
            if (!culture.IsNullOrEmpty())
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);                
            }

            var sb = new StringBuilder();

            sb.AppendLine(_multiTenancyScriptManager.GetScript());
            sb.AppendLine();

            sb.AppendLine(_sessionScriptManager.GetScript());
            sb.AppendLine();
            
            sb.AppendLine(_localizationScriptManager.GetScript());
            sb.AppendLine();
            
            sb.AppendLine(await _authorizationScriptManager.GetScriptAsync());
            sb.AppendLine();
            
            sb.AppendLine(await _navigationScriptManager.GetScriptAsync());
            sb.AppendLine();
            
            sb.AppendLine(await _settingScriptManager.GetScriptAsync());

            sb.AppendLine(GetTriggerScript());

            return Content(sb.ToString(), "application/x-javascript", Encoding.UTF8);
        }

        private static string GetTriggerScript()
        {
            var script = new StringBuilder();

            script.AppendLine("(function(){");
            script.AppendLine("    abp.event.trigger('abp.dynamicScriptsInitialized');");
            script.Append("})();");

            return script.ToString();
        }
    }
}