using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Microsoft.AspNet.Identity;

namespace Abp.Runtime.Session
{
    /// <summary>
    ///     Implements IAbpSession to get session informations from ASP.NET Identity framework.
    /// </summary>
    public class AbpSession<TTenatId, TUserId> : IAbpSession<TTenatId, TUserId>, ISingletonDependency
        where TTenatId : struct
        where TUserId : struct
    {
        private readonly IMultiTenancyConfig _multiTenancy;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public AbpSession(IMultiTenancyConfig multiTenancy)
        {
            _multiTenancy = multiTenancy;
        }

        public TUserId? UserId
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity == null)
                    throw new ArgumentException("Identity");

                var userId = string.Empty;
                var claimsIdentity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    userId = claimsIdentity.FindFirstValue(AbpClaimTypes.UserIdClaimType);
                }

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                var convertFromInvariantString =
                    TypeDescriptor.GetConverter(typeof (TUserId)).ConvertFromInvariantString(userId);
                if (convertFromInvariantString != null)
                    return (TUserId) convertFromInvariantString;

                throw new InvalidCastException("The type of string : " + userId + " can not convert to type of " +
                                               typeof (TUserId).Name);
            }
        }

        public TTenatId? TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    return default(TTenatId);
                }

                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal == null)
                {
                    return null;
                }

                var claim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
                if (claim == null || string.IsNullOrEmpty(claim.Value))
                {
                    return null;
                }

                var tenantId = claim.Value;
                var convertFromInvariantString =
                    TypeDescriptor.GetConverter(typeof (TTenatId)).ConvertFromInvariantString(tenantId);
                if (convertFromInvariantString != null)
                    return (TTenatId) convertFromInvariantString;

                throw new InvalidCastException("The type of string : " + tenantId + " can not convert to type of " +
                                               typeof (TTenatId).Name);
            }
        }

        public MultiTenancySides MultiTenancySide
        {
            get
            {
                return _multiTenancy.IsEnabled && !TenantId.HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }
        }
    }
}