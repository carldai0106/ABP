using System.Linq;
using System.Web.Mvc;
using Abp.Authorization;
using Abp.Authorization.Interceptors;
using Abp.Dependency;
using Abp.Logging;

namespace Abp.Web.Mvc.Authorization
{
    /// <summary>
    /// This attribute is used on an action of an MVC <see cref="Controller"/>
    /// to make that action usable only by authorized users. 
    /// </summary>
    public class AbpMvcAuthorizeAttribute : AuthorizeAttribute, IAbpAuthorizeAttribute
    {
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
                //modify by carl
                var container = IocManager.Instance.IocContainer;
                var handlers = container.Kernel.GetAssignableHandlers(typeof(object));
                var handler = handlers.FirstOrDefault(
                    x => x.ComponentModel.Implementation.IsGenericType && x.ComponentModel.Implementation.GetGenericTypeDefinition() == typeof(AuthorizationInterceptor<,>));

                if (handler != null)
                {
                    var impl = handler.ComponentModel.Implementation;
                    var type1 = impl.GetGenericArguments()[0];
                    var type2 = impl.GetGenericArguments()[1];

                    //TODO: Use Async..?

                    using (dynamic authorizationAttributeHelper = IocManager.Instance.ResolveAsDisposable(typeof(IAuthorizeAttributeHelper<,>).MakeGenericType(type1, type2)))
                    {
                        authorizationAttributeHelper.Object.Authorize(this);
                    }
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
