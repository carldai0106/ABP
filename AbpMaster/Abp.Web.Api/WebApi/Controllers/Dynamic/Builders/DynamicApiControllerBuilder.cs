using System.Reflection;

namespace Abp.WebApi.Controllers.Dynamic.Builders
{
    /// <summary>
    /// Used to generate dynamic api controllers for arbitrary types.
    /// </summary>
    public static class DynamicApiControllerBuilder
    {
        /// <summary>
        /// Generates a new dynamic api controller for given type.
        /// </summary>
        /// <param name="serviceName">Name of the Api controller service. For example: 'myapplication/myservice'.</param>
        /// <typeparam name="T">Type of the proxied object</typeparam>
        /// <typeparam name="TTenantId"></typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static IApiControllerBuilder<T, TTenantId, TUserId> For<T, TTenantId, TUserId>(string serviceName)
            where TTenantId : struct
            where TUserId : struct
        {
            return new ApiControllerBuilder<T, TTenantId, TUserId>(serviceName);
        }

        /// <summary>
        /// Generates multiple dynamic api controllers.
        /// </summary>
        /// <typeparam name="T">Base type (class or interface) for services</typeparam>
        /// <param name="assembly">Assembly contains types</param>
        /// <param name="servicePrefix">Service prefix</param>
        public static IBatchApiControllerBuilder<T> ForAll<T>(Assembly assembly, string servicePrefix)
        {
            return new BatchApiControllerBuilder<T>(assembly, servicePrefix);
        }
    }
}
