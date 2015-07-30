using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using CMS.Domain;
using CMS.Domain.Action;
using CMS.Domain.ActionModule;
using CMS.Domain.Module;
using CMS.Domain.Role;
using CMS.Domain.RoleRight;
using CMS.Domain.Tenant;
using CMS.Domain.User;
using CMS.Domain.UserRole;

namespace CMS.Data.EntityFramework
{
    public class CmsDbContext : AbpDbContext<Guid, Guid>
    {
        public virtual IDbSet<TenantEntity> Tenants { get; set; }
        public virtual IDbSet<UserEntity> Users { get; set; }
        public virtual IDbSet<RoleEntity> Roles { get; set; }
        public virtual IDbSet<UserRoleEntity> UserRoles { get; set; }
        public virtual IDbSet<ActionEntity> Actions { get; set; }
        public virtual IDbSet<ModuleEntity> Modules { get; set; }
        public virtual IDbSet<ActionModuleEntity> ActionModules { get; set; }
        public virtual IDbSet<RoleRightEntity> RoleRights { get; set; }

        public CmsDbContext()
            : base("Default")
        {
            //Update-Database -ConnectionString "Server=CARL\MSSQL2012; Database=AbpCms; Trusted_Connection=Yes;" -ConnectionProviderName "System.Data.SqlClient" -Verbose -ProjectName "CMS.Data"
        }

        public CmsDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            
        }

        public CmsDbContext(DbConnection connection)
            : base(connection, true)
        {

        }

        static CmsDbContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<CmsDbContext>());
            //Database.SetInitializer<CmsDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
           .Where(type => !String.IsNullOrEmpty(type.Namespace))
           .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
               type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}
