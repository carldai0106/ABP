using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPS.IDAL;
using EPS.Models;
using Framework;
using Framework.Core;
using Framework.Core.Caching;
using Framework.Data;
using Framework.Web;
using Framework.Web.Admission;
using Framework.Web.Utils;

namespace EPS.Web.Areas.Admin.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly INews _news;
        private readonly IModule _module;
        private readonly ICacheManager _cache;
        private readonly IPhoto _photo;

        public ProductsController(INews news, IModule module, IPhoto photo, ICacheManager cache)
        {
            _news = news;
            _module = module;
            _cache = cache;
            _photo = photo;
        }

        [Permission(ActionCode = "Display", ModuleCode = "Products")]
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
        [Permission(ActionCode = "Add", ModuleCode = "Products")]
        public ActionResult Create(int id)
        {
            ViewBag.ParentId = id;
            var list = _cache.Get(Constants.CACHE_KEY_MODULES, () => _module.GetList());
            list = list.Where(x => x.ParentId == id);
            ViewBag.Types = list;

            return View();
        }


        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        [Permission(ActionCode = "Add", ModuleCode = "Products")]
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

            return Redirect("~/Admin/Products/Show/" + model.NewsId);
        }

        [HttpGet]
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
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
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
        public ActionResult Edit(NewsEntry model)
        {
            if (ModelState.IsValid)
            {
                var info = _news.GetById(model.NewsId);
                info.ChangedBy = Utility.CurrentUserName;
                info.ChangedTime = DateTime.UtcNow;
                info.Content = model.Content;
                info.ModuleId = model.ModuleId;
                info.Title = model.Title;

                Utility.Operate(this, Operations.Update, () => _news.Update(model), model.Title);
            }
            else
            {
                Utility.SetErrorModelState(this);
            }

            return Redirect("~/Admin/Products/Show/" + model.NewsId);
        }
        [Permission(ActionCode = "Delete", ModuleCode = "Products")]
        public ActionResult Delete(string id)
        {
            var iVal = 0;
            Utility.Operate(this, Operations.Delete, () =>
            {
                iVal = _news.Delete(id);
                return iVal;
            });

            var page = Pagination.CheckPageIndexWhenDeleted(this, iVal);
            return Redirect(string.Format("~/Admin/Products/Index?MenuID={0}&page={1}", TempData["_menuid"], page));
        }

        [HttpGet]
        [Permission(ActionCode = "Display", ModuleCode = "Products")]
        public ActionResult Show(int id = 0, int page = 1)
        {
            ViewBag.MenuId = Utility.GetMenuId(0);
            ViewBag.NewsId = id;
            Utility.GetModelState(this);
            IEnumerable<PhotoEntry> list = new List<PhotoEntry>();
            if (id > 0)
            {
                var model = new PageModel
                {
                    Filter = "Where 1=1 AND NewsId = " + id,
                    PageIndex = page,
                    PageSize = 8
                };

                list = _photo.GetList(model);
                Pagination.NewPager(this, page, (int)model.Records, 8);
            }

            return View(list);
        }

        [HttpGet]
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
        public ActionResult Upload(int id = 0)
        {
            ViewBag.NewsId = id;
            return View();
        }

        [HttpPost]
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
        public JsonResult Upload(FormCollection collection)
        {
            var model = new PhotoEntry();

            //获取上传文件队列  
            var oFile = Request.Files["Filedata"];
            if (oFile != null)
            {
                string topDir = collection["folder"];  // 获取uploadify的folder配置，在此示例中，客户端配置了上传到 Files/ 文件夹
                // 检测并创建目录:当月上传的文件放到以当月命名的文件夹中，例如2011年11月的文件放到网站根目录下的 /Files/201111 里面
                string dateFolder = Utility.GetUploadBasePath(topDir) + "/" + DateTime.Now.Date.ToString("yyyyMM");
                string thumbnailFolder = dateFolder + "/thumbnail";
                if (!Directory.Exists(dateFolder))  // 检测是否存在磁盘目录
                {
                    Directory.CreateDirectory(dateFolder);  // 不存在的情况下，创建这个文件目录 例如 C:/wwwroot/Files/201111/
                }
                if (!Directory.Exists(thumbnailFolder))
                {
                    Directory.CreateDirectory(thumbnailFolder);
                }

                // 使用Guid命名文件，确保每次文件名不会重复
                string guidFileName = Guid.NewGuid() + Path.GetExtension(oFile.FileName).ToLower();
                // 保存文件，注意这个可是完整路径，例如C:/wwwroot/Files/201111/92b2ce5b-88af-405e-8262-d04b552f48cf.jpg
                var originalPath = dateFolder + "/" + guidFileName;
                oFile.SaveAs(originalPath);

                var original = new DirectoryInfo(originalPath).FullName.Replace(AppDomain.CurrentDomain.BaseDirectory, "");

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////// TODO 在此，您可以添加自己的业务逻辑，比如保存这个文件信息到数据库
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                var thumbnailPath = thumbnailFolder + "/" + guidFileName;

                using (var file = System.IO.File.OpenRead(originalPath))
                {
                    PhotoUtils.CutForCustom(file, thumbnailPath, 380, 252, 50);
                }

                string thumbnail = new DirectoryInfo(thumbnailPath).FullName.Replace(AppDomain.CurrentDomain.BaseDirectory, "");
                var newsId = DataCast.Get<int>(collection["NewsId"]);


                model.Description = "请添加描述";
                model.NewsId = newsId;
                model.Original = @"~\" + original;
                model.Thumbnail = @"~\" + thumbnail;
                _photo.Add(model);
            }

            return Json(model);
        }

        [HttpGet]
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
        public ActionResult EditPhoto(int id)
        {
            var info = _photo.GetById(id);
            return View(info);
        }

        [HttpPost]
        [Permission(ActionCode = "Edit", ModuleCode = "Products")]
        public JsonResult EditPhoto(PhotoEntry model)
        {
            var i = _photo.Update(model);
            if (i > 0)
            {
                return Json(model);
            }
            else
            {
                model = _photo.GetById(model.PhotoId);
                return Json(model);
            }
        }

        [HttpGet]
        [Permission(ActionCode = "Delete", ModuleCode = "Products")]
        public ActionResult DeletePhoto(string id, int NewsId = 0)
        {
            var iVal = 0;
            Utility.Operate(this, Operations.Delete, () =>
            {
                iVal = _photo.Delete(id);
                return iVal;
            });

            var page = Pagination.CheckPageIndexWhenDeleted(this, iVal);
            return Redirect(string.Format("~/Admin/Products/Show/{0}?page={1}", NewsId, page));
        }
    }
}
