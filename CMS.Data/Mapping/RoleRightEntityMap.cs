using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.ActionModule;
using CMS.Domain.RoleRight;
using CMS.Domain.Tenant;
using CMS.Domain.UserRole;

namespace CMS.Data.Mapping
{
    public class RoleRightEntityMap : EntityTypeConfiguration<RoleRightEntity>
    {
        public RoleRightEntityMap()
        {
            ToTable("RoleRights");
            HasRequired(x => x.ActionModule).WithMany(x => x.RoleRights).WillCascadeOnDelete(false);
        }
    }
}
