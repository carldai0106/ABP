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
    public class RoleRightsController : BaseController
    {
        private readonly IRole _role;
        private readonly IRoleRight _roleRight;
        private readonly IActionModule _actionModule;
        private readonly IModule _module;
        private readonly IAction _action;
        private readonly ICacheManager _cache;
        private readonly IUserRole _userRole;

        public RoleRightsController(IRole role, IUserRole userRole, IModule module, 
            IAction action, IActionModule actionModule, 
            IRoleRight roleRight, ICacheManager cache)
        {
            _role = role;
            _module = module;
            _actionModule = actionModule;
            _roleRight = roleRight;
            _action = action;
            _cache = cache;
            _userRole = userRole;
        }
        [Permission(ActionCode = "Display", ModuleCode = "RoleRights")]
        public ActionResult Index(int id = 0, int roleId = 0)
        {
            Utility.GetModelState(this);

            var modules =  _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            var actionModules = _cache.Get(Constants.CACHE_KEY_ACTIONMODULE, () => _actionModule.GetList());
            var roles = _cache.Get(Constants.CACHE_KEY_ROLES, () => _role.GetList());
            var actions = _cache.Get(Constants.CACHE_KEY_ACTIONS, () => _action.GetList());
            var userRoles = _userRole.GetList(Utility.CurrentLoginModel.UserId);

            IEnumerable<RoleEntry> roleList = null;
            if (roles != null && roles.Any())
            {
                var list = roles.Where(x => userRoles.Any(m => m.RoleId == x.RoleId && m.Status)).ToList();

                var maxGrade = roles.Max(x => x.SeqNo);
                var currentGrade = list.Max(x => x.SeqNo);

                //当前等级 等于 系统中 最大权限等级 那么当前用户是超级管理员
                if (currentGrade == maxGrade)
                {
                    roleList = roles;
                }
                else
                {
                    roleList = roles.Where(x => x.SeqNo < currentGrade);
                }

                roleId = roleId == 0 ? roleList.FirstOrDefault(x => x.SeqNo == roleList.Min(m => m.SeqNo)).RoleId : roleId;
            }

            var roleRights = _roleRight.GetList(roleId);
            var filters = modules.Where(x => x.DisplayAsMenu);
            var tree = TreeUtils.GetTree(filters, id);

            ViewBag.Module = tree;
            ViewBag.Actions = actions;
            ViewBag.ModuleList = modules;
            ViewBag.CurrentId = id;
            ViewBag.RoleId = roleId;
            ViewBag.RoleRights = roleRights;

            ViewBag.Roles = new SelectList(roleList, "RoleId", "DisplayName", roleId);

            return View(actionModules);
        }
        [Permission(ActionCode = "Add|Edit", ModuleCode = "RoleRights")]
        public ActionResult Handle(FormCollection collection)
        {
            var actionModules = _cache.Get(Constants.CACHE_KEY_ACTIONMODULE, () => _actionModule.GetList());

            var selectedModuleId = DataCast.Get<int>(collection["SelectedModuleId"]);
            var moduleIDs = collection["ModuleId"];
            var roleId = DataCast.Get<int>(collection["RoleId"]);
            var arrayModuleId = moduleIDs.Split(',');

            var list = new List<RoleRightEntry>();
            foreach (var item in arrayModuleId)
            {
                var moduleId = int.Parse(item);
                var amList = actionModules.Where(x => x.ModuleId == moduleId);
                foreach (var model in amList)
                {
                    var chkName = "ActionModule_" + model.ActionModuleId;
                    var ramIdName = "RightId_" + model.ActionModuleId;

                    var chkVal = collection[chkName];
                    var rightId = DataCast.Get<int>(collection[ramIdName]);

                    var info = new RoleRightEntry
                    {
                        ActionModuleId = model.ActionModuleId,
                        RoleId = roleId,
                        RightId = rightId,
                        Status = chkVal == "on"
                    };

                    list.Add(info);
                }
            }

            var addList = list.Where(x => x.RightId == 0);
            var updateList = list.Where(x => x.RightId != 0);

            Utility.Operate(this, Operations.Save, () =>
            {
                if (addList.Any())
                    _roleRight.Add(addList);
                
                if (updateList.Any())
                    _roleRight.Update(updateList);

                return 1;
            });
           

            return Redirect(string.Format("~/Admin/RoleRights/Index/{0}?roleId={1}", selectedModuleId, roleId));
        }
    }
}
