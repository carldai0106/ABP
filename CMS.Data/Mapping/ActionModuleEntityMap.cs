using System.Data.Entity.ModelConfiguration;
using CMS.Domain.ActionModule;

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