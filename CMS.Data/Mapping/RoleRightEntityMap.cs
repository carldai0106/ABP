using System.Data.Entity.ModelConfiguration;
using CMS.Domain.RoleRight;

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