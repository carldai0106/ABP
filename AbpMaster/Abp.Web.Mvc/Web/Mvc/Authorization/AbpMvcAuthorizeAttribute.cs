﻿using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Interceptors;
using Abp.Dependency;
using Abp.Logging;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching;
using Abp.Web.Authorization;

namespace Abp.Web.Mvc.Authorization
{
    /// <summary>
    /// This attribute is used on an action of an MVC <see cref="Controller"/>
    /// to make that action usable only by authorized users. 
    /// </summary>
    public class AbpMvcAuthorizeAttribute : AuthorizeAttribute, IAbpAuthorizeAttribute
    {
        /// <inheritdoc/>
        public string ModuleCode { get; set; }

        /// <inheritdoc/>
        public string[] Permissions { get; set; }

        /// <inheritdoc/>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="AbpMvcAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public AbpMvcAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <inheritdoc/>
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
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
