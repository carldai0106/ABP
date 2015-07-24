using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Tenant;
using CMS.Domain.UserRole;

namespace CMS.Data.Mapping
{
    public class UserRoleEntityMap : EntityTypeConfiguration<UserRoleEntity>
    {
        public UserRoleEntityMap()
        {
            ToTable("UserRoles");
            Property(x => x.Status).IsRequired();
            HasRequired(x => x.Role).WithMany(x => x.UserRoles).WillCascadeOnDelete(false);
        }
    }
}
