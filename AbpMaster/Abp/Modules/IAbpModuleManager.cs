namespace Abp.Modules
{
    internal interface IAbpModuleManager
    {
        void InitializeModules<TTenantId, TUserId>()
            where TTenantId : struct
            where TUserId : struct;

        void ShutdownModules();
    }
}