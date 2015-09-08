using System;
using System.Linq;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Memory;
using Abp.Runtime.Session;

namespace Abp.Web.Authorization
{
    /// <summary>
    /// AbpAuthorizationHelper
    /// </summary>
    public class AbpAuthorizationHelper
    {
        /// <summary>
        /// Get the type of IAbpSession{TTenantId,TUserId} from ThreadSafeObjectCache{object}("_AbpBootstrapper")
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Type GetIAbpSession()
        {
            Type tenantId;
            Type userId;
            OutTypeOfTenantIdAndUserId(out tenantId, out userId);

            var abpSessionType = typeof(IAbpSession<,>).MakeGenericType(tenantId, userId);
            return abpSessionType;
        }

        /// <summary>
        /// Get the type of IAuthorizeAttributeHelper{TTenantId,TUserId} from ThreadSafeObjectCache{object}("_AbpBootstrapper")
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Type GetIAuthorizeAttributeHelper()
        {
            Type tenantId;
            Type userId;
            OutTypeOfTenantIdAndUserId(out tenantId, out userId);
            var authorizeAttributeHelperType = typeof(IAuthorizeAttributeHelper<,>).MakeGenericType(tenantId, userId);
            return authorizeAttributeHelperType;
        }

        /// <summary>
        /// out the type of tentnat id and the type of user id
        /// </summary>
        /// <param name="tenantId">the type of tenant id</param>
        /// <param name="userId">the type of user id</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void OutTypeOfTenantIdAndUserId(out Type tenantId, out Type userId)
        {
            var container = IocManager.Instance.IocContainer;

            //var cache = container.Resolve<ThreadSafeObjectCache<object>>("_AbpBootstrapper");
            var cache = container.Resolve<AbpMemoryCache>("_AbpBootstrapper");
            var obj = cache.GetOrDefault("_AbpWebApplicationSubClass");

            Type mvcAppType;

            if (obj == null)
            {
                var finder = container.Resolve<WebAssemblyFinder>();

                var typeApp =
                    finder.GetAllAssemblies()
                        .SelectMany(
                            assembly =>
                                assembly.GetTypes()
                                    .Where(
                                        type =>
                                            type.IsInheritsOrImplements(typeof(AbpWebApplication<,>))))
                        .FirstOrDefault();

                cache.Set("_AbpWebApplicationSubClass", typeApp);
                mvcAppType = typeApp;
            }
            else
            {
                mvcAppType = obj as Type;
            }

            if (mvcAppType == null || mvcAppType.BaseType == null)
            {
                throw new InvalidOperationException("Can not get the type of MvcApplication.");
            }

            var args = mvcAppType.BaseType.GetGenericArguments();
            var tenantIdType = args[0];
            var userIdType = args[1];

            tenantId = tenantIdType;
            userId = userIdType;
        }
    }
}
