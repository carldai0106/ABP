using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Core;
using Framework.Core.Caching;
using EPS.IDAL;
using EPS.Models;
using Framework.Web;
using Framework.Web.Admission;
using Framework.Web.Utils;

namespace EPS.Web.Areas.Admin.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRole _role;
        private readonly ICacheManager _cache;
        public RolesController(IRole role, ICacheManager cache)
        {
            _role = role;
            _cache = cache;
        }

        [Permission(ActionCode = "Display", ModuleCode = "Roles")]
        public ActionResult Index()
        {
            Utility.GetModelState(this);
            var list = _cache.Get(Constants.CACHE_KEY_ROLES, () => _role.GetList());

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(ActionCode = "Add", ModuleCode = "Roles")]
        public ActionResult Create(RoleEntry model)
        {
            var info = _role.GetByCode(model.RoleCode);

            if (info != null && info.RoleCode == model.RoleCode)
            {
                ModelState.AddModelError("RoleCode", string.Format("{0} has been used, please change one.", "Action Code"));
            }

            if (ModelState.IsValid)
            {
                model.CreatedBy = Utility.CurrentUserName;
                model.CreatedTime = DateTime.UtcNow;
                Utility.Operate(this, Operations.Add, () =>
                {
                    _cache.Remove(Constants.CACHE_KEY_ROLES);
                    return _role.Add(model);
                }, model.DisplayName);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/Roles/Index");
        }
        [Permission(ActionCode = "Edit", ModuleCode = "Roles")]
        public ActionResult Edit(int id)
        {
            var list = _cache.Get(Constants.CACHE_KEY_ROLES, () => _role.GetList());
            var model = list.FirstOrDefault(x => x.RoleId == id);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Permission(ActionCode = "Edit", ModuleCode = "Roles")]
        public ActionResult Edit(RoleEntry model)
        {
            var info = _role.GetById(model.RoleId);
            if (ModelState.IsValid)
            {
                model.CreatedTime = info.CreatedTime;
                model.ChangedBy = info.CreatedBy;
                model.ChangedBy = Utility.CurrentUserName;
                model.ChangedTime = DateTime.UtcNow;
                Utility.Operate(this, Operations.Update, () =>
                {
                    _cache.Remove(Constants.CACHE_KEY_ROLES);
                    return _role.Update(model);
                }, model.DisplayName);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/Roles/Index");
        }
        [Permission(ActionCode = "Delete", ModuleCode = "Roles")]
        public ActionResult Delete(string id)
        {
            var iVal = 0;
            Utility.Operate(this, Operations.Delete, () =>
            {
                iVal = _role.Delete(id);
                return iVal;
            });
            if (iVal > 0)
            {
                _cache.Remove(Constants.CACHE_KEY_ROLES);
            }

            return Redirect("~/Admin/Roles/Index");
        }
        
        public JsonResult CheckCode(string roleCode)
        {
            var flag = true;
            var info = _role.GetByCode(roleCode);
            if (info != null)
            {
                flag = false;
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }
    }
}
