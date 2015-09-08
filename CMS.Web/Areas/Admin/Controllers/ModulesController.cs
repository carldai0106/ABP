using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Application.Services.Dto;
using CMS.Application.Module;

namespace CMS.Web.Areas.Admin.Controllers
{
    public class ModulesController : CmsControllerBase
    {
        private readonly IModuleAppService _moduleService;

        public ModulesController(IModuleAppService moduleService)
        {
            _moduleService = moduleService;
        }

        public async Task<ActionResult> Index(Guid id)
        {
            ViewBag.CurrentId = id;
            var model = await _moduleService.GetModuleTree(new NullableIdInput<Guid> {Id = id});
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Carte()
        {
            var id = ViewBag.CurrentMenuID;
            var result =
                Task.Run(async () => await _moduleService.GetModuleTree(new NullableIdInput<Guid> {Id = id})).Result;
            return View(result);
        }
    }
}