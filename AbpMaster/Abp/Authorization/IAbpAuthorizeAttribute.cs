namespace Abp.Authorization
{
    /// <summary>
    /// Defines standard interface for authorization attributes.
    /// </summary>
    public interface IAbpAuthorizeAttribute
    {
        //modify by carl
        /// <summary>
        /// The code of module or menu
        /// </summary>
        string ModuleCode { get; }

        /// <summary>
        /// A list of permissions to authorize.
        /// </summary>
        string[] Permissions { get; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// Default: false.
        /// </summary>
        bool RequireAllPermissions { get; set; }
    }
  
}