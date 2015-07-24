using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Domain.Role;
using CMS.Domain.Tenant;
using CMS.Domain.User;


namespace CMS.Data.Mapping
{
    public class RoleEntityMap : EntityTypeConfiguration<RoleEntity>
    {
        public RoleEntityMap()
        {
            ToTable("Roles");
            HasKey(x => x.Id);
            Property(x => x.RoleCode).HasMaxLength(RoleEntity.MaxRoleCodeLength).IsRequired();
            Property(x => x.DisplayName).HasMaxLength(RoleEntity.MaxDisplayNameLength).IsRequired();
            HasRequired(x => x.Tenant).WithRequiredDependent().WillCascadeOnDelete(false);
        }
    }
}
