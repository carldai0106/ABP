using System;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.Filters
{
    public class GlobalFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var cookie = context.Request.Cookies["CMS.CurrentMenuId"];
            var menuId = context.Request.QueryString["mid"];

            if (!string.IsNullOrEmpty(menuId))
            {
                if (cookie != null && cookie.Value != menuId)
                {
                    var httpCookie = context.Response.Cookies["CMS.CurrentMenuId"];
                    if (httpCookie != null) 
                        httpCookie.Value = menuId;
                }
                else
                {
                    context.Response.Cookies.Add(new HttpCookie("CMS.CurrentMenuId", menuId));
                }
            }
            else
            {
                menuId = cookie != null ? cookie.Value : "";
            }

            if (!string.IsNullOrEmpty(menuId))
                filterContext.Controller.ViewBag.CurrentMenuId = Guid.Parse(menuId);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}
