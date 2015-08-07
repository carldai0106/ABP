using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPS.IDAL;
using EPS.Models;
using Framework.Core;
using Framework.Core.Caching;
using Framework.Core.Localized;
using Framework.Web;
using Framework.Web.Admission;
using Framework.Web.Utils;

namespace EPS.Web.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        private readonly ICacheManager _cache;
        private readonly INews _news;
        private readonly IModule _module;

        public AboutController(INews news, IModule module, ICacheManager cache)
        {
            _news = news;
            _module = module;
            _cache = cache;
        }

        [Permission(ActionCode = "Display", ModuleCode = "aboutus")]
        public ActionResult Index(int MenuID = 0)
        {
            Utility.GetModelState(this);
            ViewBag.ParentId = MenuID;

            var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());

            var filters = list.Where(x => x.ParentId == MenuID).ToList();
            filters.Insert(0, new ModuleEntry
            {
                ModuleId = 0,
                DisplayName = Localization.GetLang("Please select one")
            });

            ViewBag.Types = new SelectList(filters, "ModuleId", "DisplayName");

            return View();
        }

        [HttpGet]
        public JsonResult Get(int id)
        {
            var info = _news.GetAboutUs(id);
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateInput(false)]
        [Permission(ActionCode = "Add|Edit", ModuleCode = "aboutus")]
        public ActionResult Index(NewsEntry entry)
        {
            if (ModelState.IsValid)
            {
                if (entry.NewsId == 0)
                {
                    entry.CreatedBy = Utility.CurrentUserName;
                    entry.CreatedTime = DateTime.UtcNow;
                    Utility.Operate(this, Operations.Add, () => _news.Add(entry), entry.Title);
                }
                else
                {
                    var info = _news.GetById(entry.NewsId);
                    info.ChangedBy = Utility.CurrentUserName;
                    info.ChangedTime = DateTime.UtcNow;
                    info.Content = entry.Content;
                    info.ModuleId = entry.ModuleId;

                    Utility.Operate(this, Operations.Update, () => _news.Update(info), info.Title);
                }
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/About/Index?MenuID=" + entry.ParentId);
        }
    }
}
