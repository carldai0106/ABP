using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.ActionModule;
using CMS.Domain.Tenant;
using CMS.Domain.UserRole;

namespace CMS.Data.Mapping
{
    public class ActionModuleEntityMap : EntityTypeConfiguration<ActionModuleEntity>
    {
        public ActionModuleEntityMap()
        {
            ToTable("ActionModules");
            HasRequired(x => x.Action).WithMany(x => x.ActionModuels).WillCascadeOnDelete(false);
        }
    }
}
