using System.Web.Http.Filters;
using Abp.Web;

namespace Abp.WebApi.Controllers.Dynamic.Builders
{
    /// <summary>
    /// This interface is used to define a dynamic api controller.
    /// </summary>
    /// <typeparam name="T">Type of the proxied object</typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    public interface IApiControllerBuilder<T, TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// To add Action filters for the Dynamic Controller.
        /// </summary>
        /// <param name="filters"> The filters. </param>
        /// <returns>The current Controller Builder </returns>
        IApiControllerBuilder<T, TTenantId, TUserId> WithFilters(params IFilter[] filters);

        /// <summary>
        /// Used to specify a method definition.
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type</param>
        /// <returns>Action builder</returns>
        IApiControllerActionBuilder<T, TTenantId, TUserId> ForMethod(string methodName);
		
		/// <summary>
        /// Use conventional Http Verbs by method names.
        /// By default, it uses <see cref="HttpVerb.Post"/> for all actions.
        /// </summary>
        /// <returns>The current Controller Builder</returns>
        IApiControllerBuilder<T, TTenantId, TUserId> WithConventionalVerbs();
        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        void Build();
    }
}