using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using CMS.Application.Module;
using CMS.Application.Module.Dto;
using Microsoft.AspNet.Identity;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class ModulesController : CmsControllerBase
    {
        private IModuleAppService _moduleService;

        public ModulesController(IModuleAppService moduleService)
        {
            _moduleService = moduleService;
        }


        public async Task<ActionResult> Index(Guid id)
        {
            ViewBag.CurrentId = id;
            var model = await _moduleService.GetModuleTree(new NullableIdInput<Guid> { Id = id });
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Carte()
        {
            var result = Task.Run(async () => await _moduleService.GetModuleTree(new NullableIdInput<Guid> { Id = null })).Result;
            //var model = await _moduleService.GetModuleTree(new NullableIdInput<Guid> { Id = null });

            return View(result);
        }
    }
}
