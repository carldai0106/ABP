using System.Web.Mvc;
using Abp.Authorization;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class DashboardController : CmsControllerBase
    {
        [AbpAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}