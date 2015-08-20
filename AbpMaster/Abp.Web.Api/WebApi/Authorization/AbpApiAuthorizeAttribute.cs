using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Abp.Authorization;
using Abp.Authorization.Interceptors;
using Abp.Dependency;
using Abp.Logging;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching;
using Abp.Web;
using Abp.Web.Authorization;

namespace Abp.WebApi.Authorization
{
    /// <summary>
    /// This attribute is used on a method of an <see cref="ApiController"/>
    /// to make that method usable only by authorized users.
    /// </summary>
    public class AbpApiAuthorizeAttribute : AuthorizeAttribute, IAbpAuthorizeAttribute
    {
        /// <inheritdoc/>
        public string ModuleCode { get; set; }

        /// <inheritdoc/>
        public string[] Permissions { get; set; }

        /// <inheritdoc/>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="AbpApiAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public AbpApiAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <inheritdoc/>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }

            try
            {
                var authorizeAttributeHelperType = AbpAuthorizationHelper.GetIAuthorizeAttributeHelper();
                //TODO: Carl
                //TODO: Use Async..?
                using (dynamic authorizationAttributeHelper = IocManager.Instance.ResolveAsDisposable(authorizeAttributeHelperType))
                {
                    authorizationAttributeHelper.Object.Authorize(this);
                }

                return true;
            }
            catch (AbpAuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                return false;
            }
        }
    }
}
