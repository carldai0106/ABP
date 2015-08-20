using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Web.Mvc.Authorization;
using CMS.Web.Areas.Admin.Controllers;


namespace CMS.Web.Areas.Admin.Controllers
{
    
    public class DashboardController : CmsControllerBase
    {
        [AbpAuthorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}