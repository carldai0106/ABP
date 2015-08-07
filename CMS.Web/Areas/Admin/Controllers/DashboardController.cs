using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EPS.IDAL;
using EPS.Models;
using Framework.Core.Caching;
using Framework.Core.Localized;
using Framework.Web;
using Framework.Web.Utils;


namespace EPS.Web.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IBasic _basic;
        private readonly ICacheManager _cache;

        public DashboardController(IBasic basic, ICacheManager cache) 
        {
            _basic = basic;
            _cache = cache;
        }

        public ActionResult Index()
        {
            var table = _basic.GetRecords(15, 18, 21);
            ViewBag.Table = table;
            
            return View();
        }


        public ActionResult Configuration()
        {
            Utility.GetModelState(this);
            var list = _cache.Get(Constants.CACHE_KEY_BASIC, () => _basic.GetList());
            var table = new Hashtable();
            foreach (var item in list)
            {
                table[item.Key] = item.Value;
            }

            return View(table);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult Configuration(FormCollection collection)
        {
            var fileKeys = new string[] {"logo", "banner1", "banner2", "banner3", "banner4", "banner5"};
            var language = Localization.GetLang("Banner Image");
            var dic = new Dictionary<string, string>()
            {
                {"logo","Logo"},
                {"banner1",language + 1},
                {"banner2",language + 2},
                {"banner3",language + 3},
                {"banner4",language + 4},
                {"banner5",language + 5}
            };

            var hasError = false;
            var list = _basic.GetList();

            foreach (var item in fileKeys)
            {
                var info = list.FirstOrDefault(x => x.Key == item);
                if (info != null && Request.Files[item] != null && Request.Files[item].FileName != string.Empty)
                {
                    var up = new Uploads();
                    var ht = new Hashtable();
                    var filetype = new string[] {".png", ".jpg", ".jpeg"};

                    ht = up.upFile("/Content/Uploads/banner/", filetype, 16, item);
                    if (ht["state"].ToString() == "SUCCESS")
                    {
                        info.Value = "~/Content/Uploads/banner/" + ht["filename"].ToString();
                    }
                    else
                    {
                        var lang = string.Empty;
                        if (ht["state"].ToString() == "2")
                        {
                            lang = string.Format(Localization.GetLang("Please select a valid photo for {0}."), dic[item]);
                        }
                        else
                        {
                            if (ht["state"].ToString() == "3")
                            {
                                lang = string.Format(Localization.GetLang("{0} size is too large."), dic[item]);
                            }
                        }
                        ModelState.AddModelError(item, lang);
                        hasError = true;
                    }
                }
            }

            if (hasError)
            {
                Utility.SetErrorModelState(this);
            }

            foreach (var item in list)
            {
                if (!fileKeys.Contains(item.Key))
                {
                    var info = list.FirstOrDefault(x => x.Key == item.Key);
                    if (info != null && !string.IsNullOrEmpty(info.Key))
                    {
                        info.Value = collection[item.Key];
                    }
                }
            }

            Utility.Operate(this, Operations.Save, () =>
            {
                _cache.Remove(Constants.CACHE_KEY_BASIC);
                return _basic.Update(list);
            });

            return Redirect("~/Admin/Dashboard/Configuration");
        }
    }
}