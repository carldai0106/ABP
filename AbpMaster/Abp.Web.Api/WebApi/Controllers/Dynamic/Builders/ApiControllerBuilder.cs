using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using Abp.WebApi.Controllers.Dynamic.Interceptors;

namespace Abp.WebApi.Controllers.Dynamic.Builders
{
    /// <summary>
    /// Used to build <see cref="DynamicApiControllerInfo"/> object.
    /// </summary>
    /// <typeparam name="T">The of the proxied object</typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    internal class ApiControllerBuilder<T, TTenantId, TUserId> : IApiControllerBuilder<T, TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Name of the controller.
        /// </summary>
        private readonly string _serviceName;

        /// <summary>
        /// List of all action builders for this controller.
        /// </summary>
        private readonly IDictionary<string, ApiControllerActionBuilder<T, TTenantId, TUserId>> _actionBuilders;

        /// <summary>
        /// Action Filters to apply to the whole Dynamic Controller.
        /// </summary>
        private IFilter[] _filters;
        private bool _conventionalVerbs;

        /// <summary>
        /// Creates a new instance of ApiControllerInfoBuilder.
        /// </summary>
        /// <param name="serviceName">Name of the controller</param>
        public ApiControllerBuilder(string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ArgumentException("serviceName null or empty!", "serviceName");
            }

            if (!DynamicApiServiceNameHelper.IsValidServiceName(serviceName))
            {
                throw new ArgumentException("serviceName is not properly formatted! It must contain a single-depth namespace at least! For example: 'myapplication/myservice'.", "serviceName");
            }

            _serviceName = serviceName;

            _actionBuilders = new Dictionary<string, ApiControllerActionBuilder<T, TTenantId, TUserId>>();
            foreach (var methodInfo in DynamicApiControllerActionHelper.GetMethodsOfType<TTenantId, TUserId>(typeof(T)))
            {
                _actionBuilders[methodInfo.Name] = new ApiControllerActionBuilder<T, TTenantId, TUserId>(this, methodInfo);
            }
        }

        /// <summary>
        /// The adds Action filters for the whole Dynamic Controller
        /// </summary>
        /// <param name="filters"> The filters. </param>
        /// <returns>The current Controller Builder </returns>
        public IApiControllerBuilder<T, TTenantId, TUserId> WithFilters(params IFilter[] filters)
        {
            _filters = filters;
            return this;
        }

        /// <summary>
        /// Used to specify a method definition.
        /// </summary>
        /// <param name="methodName">Name of the method in proxied type</param>
        /// <returns>Action builder</returns>
        public IApiControllerActionBuilder<T, TTenantId, TUserId> ForMethod(string methodName)
        {
            if (!_actionBuilders.ContainsKey(methodName))
            {
                throw new AbpException("There is no method with name " + methodName + " in type " + typeof(T).Name);
            }

            return _actionBuilders[methodName];
        }

        public IApiControllerBuilder<T, TTenantId, TUserId> WithConventionalVerbs()
        {
            _conventionalVerbs = true;
            return this;
        }
        /// <summary>
        /// Builds the controller.
        /// This method must be called at last of the build operation.
        /// </summary>
        public void Build()
        {
            var controllerInfo = new DynamicApiControllerInfo(
                _serviceName,
                typeof(T),
                typeof(DynamicApiController<T, TTenantId, TUserId>),
                typeof(AbpDynamicApiControllerInterceptor<T>),
                _filters
                );
            foreach (var actionBuilder in _actionBuilders.Values)
            {
                if (actionBuilder.DontCreate)
                {
                    continue;
                }

                if (_conventionalVerbs && !actionBuilder.Verb.HasValue)
                {
                    actionBuilder.WithVerb(DynamicApiVerbHelper.GetConventionalVerbForMethodName(actionBuilder.ActionName));
                }
                controllerInfo.Actions[actionBuilder.ActionName] = actionBuilder.BuildActionInfo();
            }

            DynamicApiControllerManager.Register(controllerInfo);
        }
    }
}