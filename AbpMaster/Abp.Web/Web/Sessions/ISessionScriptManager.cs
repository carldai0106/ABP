namespace Abp.Web.Sessions
{
    /// <summary>
    /// Used to create client scripts for session.
    /// </summary>
    public interface ISessionScriptManager<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        string GetScript();
    }
}
