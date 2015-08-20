using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Web;

namespace CMS.Web
{
    public class MvcApplication : AbpWebApplication<Guid,Guid>
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

            
        }
    }
}
