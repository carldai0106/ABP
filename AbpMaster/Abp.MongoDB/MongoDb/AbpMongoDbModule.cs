using System.Reflection;
using Abp.Modules;
using Abp.MongoDb.Configuration;

namespace Abp.MongoDb
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in MongoDB.
    /// </summary>
    public class AbpMongoDbModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            IocManager.Register<IAbpMongoDbModuleConfiguration, AbpMongoDbModuleConfiguration>();
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
