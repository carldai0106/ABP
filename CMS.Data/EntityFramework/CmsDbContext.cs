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

namespace CMS.Data.EntityFramework
{
    public class CmsDbContext : AbpDbContext<Guid, Guid>
      
    {
        //public virtual IDbSet<UserEntity> Users { get; set; }

        public CmsDbContext()
            : base("AbpCmsConn")
        {
            //Console.WriteLine(this.Database.Connection.ConnectionString);
        }

        //public CmsDbContext(string nameOrConnectionString)
        //    : base(nameOrConnectionString)
        //{

        //}

        //public CmsDbContext(DbConnection connection)
        //    : base(connection, true)
        //{

        //}

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
