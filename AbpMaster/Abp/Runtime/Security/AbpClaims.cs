namespace Abp.Runtime.Security
{
    /// <summary>
    /// Used to get ABP-specific claim type names.
    /// </summary>
    public static class AbpClaimTypes
    {
        /// <summary>
        /// TenantId.
        /// </summary>
        public const string TenantId = "http://www.aspnetboilerplate.com/identity/claims/tenantId";
        /// <summary>
        /// Role
        /// </summary>
        public const string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        /// <summary>
        /// User Id
        /// </summary>
        public const string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        /// <summary>
        /// User Name
        /// </summary>
        public const string UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
        /// <summary>
        /// Identity Provider ClaimValue
        /// </summary>
        public const string DefaultIdentityProviderClaimValue = "ASP.NET Identity";
        /// <summary>
        /// Identity Provider
        /// </summary>
        public const string IdentityProviderClaimType = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        
    }
}
