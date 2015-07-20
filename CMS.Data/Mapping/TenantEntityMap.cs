using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using CMS.Domain.Tenant;

namespace CMS.Data.Mapping
{
    public class TenantEntityMap : EntityTypeConfiguration<TenantEntity>
    {
        public TenantEntityMap()
        {
            ToTable("Tenants");
            HasKey(x => x.Id);
            Property(x => x.DisplayName).IsRequired().HasMaxLength(TenantEntity.MaxDisplayNameLength);
            Property(x => x.TenancyName)
                .IsRequired()
                .HasMaxLength(TenantEntity.MaxTenancyNameLength)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("UNQ_Tenants_TenancyName", 1) { IsUnique = true })
                     );
            
            
        }
    }
}
