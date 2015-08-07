using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPS.IDAL;
using EPS.Models;
using Framework;
using Framework.Core.Caching;
using Framework.Data;
using Framework.Web;
using Framework.Web.Admission;
using Framework.Web.Utils;

namespace EPS.Web.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INews _news;
        private readonly IModule _module;
        private readonly ICacheManager _cache;

        public NewsController(INews news, IModule module, ICacheManager cache)
        {
            _news = news;
            _module = module;
            _cache = cache;
        }

        [Permission(ActionCode = "Display", ModuleCode = "News")]
        public ActionResult Index(int page = 1, int MenuID = 0, int ModuleId = 0, string Content = "")
        {
            Utility.GetModelState(this);

            var types = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            var moduleTypes = types.Where(x => x.ParentId == MenuID).ToList();
            ViewBag.ModuleTypes = moduleTypes;

            ViewBag.MenuID = MenuID;
            ViewBag.ModuleId = ModuleId;
            ViewBag.Content = Content;

            TempData["_menuid"] = Utility.GetMenuId(MenuID);

            var sql =
                Sql.Builder.Append(
                    "SELECT News.*, Modules.DisplayName FROM News JOIN Modules ON News.ModuleId = Modules.ModuleId");
            sql.Append("WHERE News.ParentId = @0", MenuID);

            if (ModuleId != 0)
            {
                sql.Append(" AND News.ModuleId = @0", ModuleId);
            }

            if (!string.IsNullOrEmpty(Content))
            {
                sql.Append(" AND Title LIKE @0", "%" + Content + "%");
            }

            var model = new PageSqlModel
            {
                PageIndex = page,
                PageSize = Utility.PageSize,
                Sql = sql
            };

            var list = _news.GetList(model);
            Pagination.NewPager(this, page, (int)model.Records);

            return View(list);
        }

        [HttpGet]
        [Permission(ActionCode = "Add", ModuleCode = "News")]
        public ActionResult Create(int id)
        {
            ViewBag.ParentId = id;
            var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            list = list.Where(x => x.ParentId == id);
            ViewBag.Types = list;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Permission(ActionCode = "Add", ModuleCode = "News")]
        public ActionResult Create(NewsEntry model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = Utility.CurrentUserName;
                model.CreatedTime = DateTime.UtcNow;

                Utility.Operate(this, Operations.Add, () => _news.Add(model), model.Title);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/News/Index?MenuID=" + model.ParentId);
        }

        [HttpGet]
        [Permission(ActionCode = "Edit", ModuleCode = "News")]
        public ActionResult Edit(int id)
        {
            var info = _news.GetById(id);
            var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            list = list.Where(x => x.ParentId == info.ParentId);
            ViewBag.ParentId = info.ParentId;
            ViewBag.Types = list;

            return View(info);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Permission(ActionCode = "Edit", ModuleCode = "News")]
        public ActionResult Edit(NewsEntry model)
        {
            if (ModelState.IsValid)
            {
                var info = _news.GetById(model.NewsId);
                info.ChangedBy = Utility.CurrentUserName;
                info.ChangedTime = DateTime.UtcNow;
                info.Author = model.Author;
                info.Brief = model.Brief;
                info.Content = model.Content;
                info.Count = model.Count;
                info.ModuleId = model.ModuleId;
                info.Source = model.Source;
                info.Title = model.Title;

                Utility.Operate(this, Operations.Update, () => _news.Update(model), model.Title);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/News/Index?MenuID=" + model.ParentId);
        }

        [Permission(ActionCode = "Delete", ModuleCode = "News")]
        public ActionResult Delete(string id)
        {
            var iVal = 0;
            Utility.Operate(this, Operations.Delete, () =>
            {
                iVal = _news.Delete(id);
                return iVal;
            });

            var page = Pagination.CheckPageIndexWhenDeleted(this, iVal);
            return Redirect(string.Format("~/Admin/News/Index?MenuID={0}&page={1}", TempData["_menuid"], page));
        }
    }
}
