﻿using System.Web.Mvc;

namespace CMS.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Login", id = UrlParameter.Optional },
                new [] { "CMS.Web.Areas.Admin.Controllers" }
            );
        }
    }
}