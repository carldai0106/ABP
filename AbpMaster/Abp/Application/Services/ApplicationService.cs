using System;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;

namespace Abp.Application.Services
{
    /// <summary>
    /// This class can be used as a base class for application services. 
    /// </summary>
    public abstract class ApplicationService<TTenantId, TUserId> : AbpServiceBase<TTenantId, TUserId>, IApplicationService
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets current session information.
        /// </summary>
        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }
        
        /// <summary>
        /// Reference to the permission manager.
        /// </summary>
        public IPermissionManager<TTenantId, TUserId> PermissionManager { protected get; set; }

        /// <summary>
        /// Reference to the permission checker.
        /// </summary>
        public IPermissionChecker<TTenantId, TUserId> PermissionChecker { protected get; set; }

        /// <summary>
        /// Gets current session information.
        /// </summary>
        [Obsolete("Use AbpSession property instead. CurrentSetting will be removed in future releases.")]
        protected IAbpSession<TTenantId, TUserId> CurrentSession { get { return AbpSession; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ApplicationService()
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            PermissionChecker = NullPermissionChecker<TTenantId, TUserId>.Instance;
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        protected Task<bool> IsGrantedAsync(string permissionName)
        {
            return PermissionChecker.IsGrantedAsync(permissionName);
        }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        protected bool IsGranted(string permissionName)
        {
            return PermissionChecker.IsGranted(permissionName);
        }
    }
}
