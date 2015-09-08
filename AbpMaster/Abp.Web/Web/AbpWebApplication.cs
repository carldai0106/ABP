using System;
using System.Globalization;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using Abp.Dependency;
using Abp.Localization;
using Abp.Reflection;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Memory;
using Abp.Runtime.Security;
using Castle.MicroKernel.Registration;

namespace Abp.Web
{
    /// <summary>
    /// This class is used to simplify starting of ABP system using <see cref="AbpBootstrapper"/> class..
    /// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start ABP system.
    /// </summary>
    public abstract class AbpWebApplication<TTenantId, TUserId> : HttpApplication
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets a reference to the <see cref="AbpBootstrapper"/> instance.
        /// </summary>
        protected AbpBootstrapper<TTenantId, TUserId> AbpBootstrapper { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected AbpWebApplication()
        {
            AbpBootstrapper = new AbpBootstrapper<TTenantId, TUserId>();
        }

        /// <summary>
        /// This method is called by ASP.NET system on web application's startup.
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            AbpBootstrapper.IocManager.RegisterIfNot<IAssemblyFinder, WebAssemblyFinder>();

            //var cache = new ThreadSafeObjectCache<object>(new MemoryCache("_AbpWebApplicationCache"), TimeSpan.FromHours(3));

            var cache = new AbpMemoryCache("_AbpWebApplicationCache") { DefaultSlidingExpireTime = TimeSpan.FromDays(1) };

            //AbpBootstrapper.IocManager.IocContainer.Register(
            //    Component.For<ThreadSafeObjectCache<object>>().Named("_AbpBootstrapper").Instance(cache).LifestyleTransient());

            AbpBootstrapper.IocManager.IocContainer.Register(
                Component.For<AbpMemoryCache>().Named("_AbpBootstrapper").Instance(cache).LifestyleTransient()
                );

            AbpBootstrapper.Initialize();

            //为了能够使用AntiForgeryToken，该处配置是必须的
            AntiForgeryConfig.UniqueClaimTypeIdentifier = AbpClaimTypes.UserIdClaimType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_End(object sender, EventArgs e)
        {
            AbpBootstrapper.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// This method is called by ASP.NET system when a request starts.
        /// </summary>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            var langCookie = Request.Cookies["Abp.Localization.CultureName"];
            if (langCookie != null && GlobalizationHelper.IsValidCultureCode(langCookie.Value))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(langCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCookie.Value);
            }
        }

        /// <summary>
        /// This method is called by ASP.NET system when a request ends.
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }
    }
}
