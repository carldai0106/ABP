using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMS.Application.Module;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class ModulesController : CmsControllerBase
    {
        private IModuleAppService _moduleService;

        public ModulesController(IModuleAppService moduleService)
        {
            _moduleService = moduleService;
        }

        
        public ActionResult Index(int id = 0)
        {
            //_cache.Remove(Constants.CACHE_KEY_MODULES);
            //Utility.GetModelState(this);
            ViewBag.CurrentId = id;

            var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            var model = TreeUtils.GetTree(list, id);

            return System.Web.UI.WebControls.View(model);
        }

        [ChildActionOnly]
        public ActionResult Carte()
        {
            //var id = DataCast.Get<int>(ViewBag.CurrentMenuID);
            //var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            //var filters = list.Where(x => x.DisplayAsMenu);
            //var model = TreeUtils.GetTree(filters, id);

            //return View(model);
        }

        
    }
}
