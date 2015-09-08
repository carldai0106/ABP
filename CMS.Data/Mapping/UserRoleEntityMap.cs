using System.Data.Entity.ModelConfiguration;
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