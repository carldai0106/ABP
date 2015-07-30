using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using CMS.Domain.RoleRight;
using CMS.Domain.Tenant;
using CMS.Domain.UserRole;
using CMS.FW;

namespace CMS.Domain.Role
{
    [Table("Roles")]
    public class RoleEntity : FullAuditedAndTenantEntity, IPassivable
    {
        public const int MaxRoleCodeLength = 128;
        public const int MaxDisplayNameLength = 256;
        public const int MaxDescriptionLength = 512;

        [Required]
        [StringLength(MaxRoleCodeLength)]
        public virtual string RoleCode { get; set; }
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }
        public virtual bool IsActive { get; set; }
        [ForeignKey("TenantId")]
        public virtual TenantEntity Tenant { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }

        [ForeignKey("RoleId")]
        public virtual ICollection<RoleRightEntity> RoleRights { get; set; }

        protected RoleEntity()
        {

        }

        public RoleEntity(Guid tenantId, string roleCode, string displayName, string description)
        {
            Id = Guid.NewGuid();
            TenantId = tenantId;
            RoleCode = roleCode;
            DisplayName = displayName;
            Description = description;
        }
    }
}
