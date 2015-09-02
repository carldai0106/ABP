using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using Abp.Web;
using CMS.Web.Filters;

namespace CMS.Web
{
    public class MvcApplication : AbpWebApplication<Guid,Guid>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            base.Application_Start(sender, e);
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            //ModelBinders.Binders.Add(typeof(Guid), new GuidModelBinder());

            GlobalFilters.Filters.Add(new GlobalFilterAttribute());
        }
    }
}
