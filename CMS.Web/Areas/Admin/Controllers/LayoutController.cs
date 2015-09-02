using System.Web.Mvc;
using Abp.Configuration.Startup;
using Abp.Threading;
using CMS.Application.Sessions;
using CMS.Web.Areas.Admin.Models;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class LayoutController : CmsControllerBase
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ISessionAppService _sessionAppService;

        public LayoutController(ISessionAppService sessionAppService, IMultiTenancyConfig multiTenancyConfig)
        {
            _sessionAppService = sessionAppService;
            _multiTenancyConfig = multiTenancyConfig;
        }

        [ChildActionOnly]
        public PartialViewResult Header(string currentPageName = "")
        {
            var headerModel = new HeaderViewModel();

            if (AbpSession.UserId.HasValue)
            {
                headerModel.LoginInformations =
                    AsyncHelper.RunSync(() => _sessionAppService.GetCurrentLoginInformations());
            }

            headerModel.Languages = LocalizationManager.GetAllLanguages();
            headerModel.CurrentLanguage = LocalizationManager.CurrentLanguage;
            headerModel.CurrentPageName = currentPageName;

            headerModel.IsMultiTenancyEnabled = _multiTenancyConfig.IsEnabled;

            return PartialView("~/Areas/Admin/Views/Layout/_Header.cshtml", headerModel);
        }
    }
}