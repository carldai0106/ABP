using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Core.Caching;
using EPS.IDAL;
using EPS.Models;
using Framework.Web;
using Framework.Web.Admission;
using Framework.Web.Utils;

namespace EPS.Web.Areas.Admin.Controllers
{
    public class ActionsController : BaseController
    {
        private readonly IAction _action;
        private readonly ICacheManager _cache;
        public ActionsController(IAction action, ICacheManager cache)
        {
            _action = action;
            _cache = cache;
        }

        [Permission(ActionCode = "Display", ModuleCode = "Actions")]
        public ActionResult Index()
        {
            Utility.GetModelState(this);
            var list = _cache.Get(Constants.CACHE_KEY_ACTIONS, () => _action.GetList());

            return View(list);
        }
        [Permission(ActionCode = "Add", ModuleCode = "Actions")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(ActionCode = "Add", ModuleCode = "Actions")]
        public ActionResult Create(ActionEntry model)
        {
            var info = _action.GetByCode(model.ActionCode);

            if (info != null && info.ActionCode == model.ActionCode)
            {
                ModelState.AddModelError("ActionCode", string.Format("{0} has been used, please change one.", "Action Code"));
            }

            if (ModelState.IsValid)
            {
                model.CreatedBy = Utility.CurrentUserName;
                model.CreatedTime = DateTime.UtcNow;
                Utility.Operate(this, Operations.Add, () =>
                {
                    _cache.Remove(Constants.CACHE_KEY_ACTIONS);
                    return _action.Add(model);
                }, model.DisplayName);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/Actions/Index");
        }
        [Permission(ActionCode = "Edit", ModuleCode = "Actions")]
        public ActionResult Edit(int id)
        {
            var list = _cache.Get(Constants.CACHE_KEY_ACTIONS, () => _action.GetList());
            return View(list.FirstOrDefault(x => x.ActionId == id));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(ActionCode = "Edit", ModuleCode = "Actions")]
        public ActionResult Edit(ActionEntry model)
        {
            var info = _action.GetById(model.ActionId);
            if (ModelState.IsValid)
            {
                model.CreatedTime = info.CreatedTime;
                model.ChangedBy = info.CreatedBy;
                model.ChangedBy = Utility.CurrentUserName;
                model.ChangedTime = DateTime.UtcNow;
                Utility.Operate(this, Operations.Update, () =>
                {
                    _cache.Remove(Constants.CACHE_KEY_ACTIONS);
                    return _action.Update(model);
                }, model.DisplayName);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/Actions/Index");
        }
        [Permission(ActionCode = "Delete", ModuleCode = "Actions")]
        public ActionResult Delete(string id)
        {
            var iVal = 0;
            Utility.Operate(this, Operations.Delete, () =>
            {
                iVal = _action.Delete(id);
                return iVal;
            });
            if (iVal > 0)
            {
                _cache.Remove(Constants.CACHE_KEY_ACTIONS);
            }

            return Redirect("~/Admin/Actions/Index");
        }

        public JsonResult CheckCode(string actionCode)
        {
            var flag = true;
            var info = _action.GetByCode(actionCode);
            if (info != null)
            {
                flag = false;
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
