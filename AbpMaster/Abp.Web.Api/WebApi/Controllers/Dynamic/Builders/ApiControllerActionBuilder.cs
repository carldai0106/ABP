using System.Reflection;
using Abp.Web;
using System.Web.Http.Filters;

namespace Abp.WebApi.Controllers.Dynamic.Builders
{
    /// <summary>
    /// Used to build <see cref="DynamicApiActionInfo"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the proxied object</typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    internal class ApiControllerActionBuilder<T, TTenantId, TUserId> : IApiControllerActionBuilder<T, TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Selected action name.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Selected Http verb.
        /// </summary>
        public HttpVerb? Verb { get; private set; }

        ///<summary>
        /// Reference to the <see cref="ApiControllerBuilder{T, TTenantId, TUserId}"/> which created this object.
        /// </summary>
        private readonly ApiControllerBuilder<T, TTenantId, TUserId> _controllerBuilder;

        /// <summary>
        /// Underlying proxying method.
        /// </summary>
        private readonly MethodInfo _methodInfo;

        /// <summary>
        /// Action Filters for dynamic controller method.
        /// </summary>
        private IFilter[] _filters;

        /// <summary>
        /// A flag to set if no action will be created for this method.
        /// </summary>
        public bool DontCreate { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ApiControllerActionBuilder{T,TTenantId, TUserId}"/> object.
        /// </summary>
        /// <param name="apiControllerBuilder">Reference to the <see cref="ApiControllerBuilder{T,TTenantId, TUserId}"/> which created this object</param>
        /// <param name="methodInfo"> </param>
        public ApiControllerActionBuilder(ApiControllerBuilder<T, TTenantId, TUserId> apiControllerBuilder, MethodInfo methodInfo)
        {
            _controllerBuilder = apiControllerBuilder;
            _methodInfo = methodInfo;
            ActionName = _methodInfo.Name;
        }

        /// <summary>
        /// Used to specify Http verb of the action.
        /// </summary>
        /// <param name="verb">Http very</param>
        /// <returns>Action builder</returns>
        public IApiControllerActionBuilder<T, TTenantId, TUserId> WithVerb(HttpVerb verb)
        {
            Verb = verb;
            return this;
        }

        /// <summary>
        /// Used to specify another method definition.
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type</param>
        /// <returns>Action builder</returns>
        public IApiControllerActionBuilder<T, TTenantId, TUserId> ForMethod(string methodName)
        {
            return _controllerBuilder.ForMethod(methodName);
        }

        /// <summary>
        /// Used to add action filters to apply to this method.
        /// </summary>
        /// <param name="filters"> Action Filters to apply.</param>
        public IApiControllerActionBuilder<T, TTenantId, TUserId> WithFilters(params IFilter[] filters)
        {
            _filters = filters;
            return this;
        }

        /// <summary>
        /// Tells builder to not create action for this method.
        /// </summary>
        /// <returns>Controller builder</returns>
        public IApiControllerBuilder<T, TTenantId, TUserId> DontCreateAction()
        {
            DontCreate = true;
            return _controllerBuilder;
        }

        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        public void Build()
        {
            _controllerBuilder.Build();
        }

        /// <summary>
        /// Builds <see cref="DynamicApiActionInfo"/> object for this configuration.
        /// </summary>
        /// <returns></returns>
        public DynamicApiActionInfo BuildActionInfo()
        {
            if (Verb == null)
            {
                Verb = DynamicApiVerbHelper.GetDefaultHttpVerb();
            }

            return new DynamicApiActionInfo(ActionName, Verb.Value, _methodInfo, _filters);
        }
    }
}