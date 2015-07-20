using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration;
using CMS.Domain.User;

namespace CMS.Data.Mapping
{
    public class UserEntityMap : EntityTypeConfiguration<UserEntity>
    {
        public UserEntityMap()
        {
            ToTable("Users");

            HasKey(x => x.Id);
            Property(x => x.UserName).HasMaxLength(UserEntity.MaxUserNameLength).IsRequired();
            Property(x => x.Password).HasMaxLength(UserEntity.MaxPasswordLength).IsRequired();
            Property(x => x.Email).HasMaxLength(UserEntity.MaxEmailLength).IsRequired();
            Property(x => x.FirstName).HasMaxLength(UserEntity.MaxFirstNameLength);
            Property(x => x.LastName).HasMaxLength(UserEntity.MaxLastNameLength);
            HasRequired(x => x.Tenant).WithMany(x => x.Users).HasForeignKey(x => x.TenantId).WillCascadeOnDelete(false);
        }
    }
}
