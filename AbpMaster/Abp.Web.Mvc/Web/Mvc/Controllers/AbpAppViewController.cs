using System.Web.Mvc;
using Abp.Auditing;

namespace Abp.Web.Mvc.Controllers
{
    //TODO: Maybe it's better to write an HTTP handler for that instead of controller (since it's more light)
    public class AbpAppViewController<TTenantId, TUserId> : AbpController<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        [DisableAuditing]
        public ActionResult Load(string viewUrl)
        {
            if (!viewUrl.StartsWith("~"))
            {
                viewUrl = "~" + viewUrl;
            }

            return View(viewUrl);
        }
    }
}
