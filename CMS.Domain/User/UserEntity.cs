using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.Tenant;
using CMS.FW;

namespace CMS.Domain.User
{
    public class UserEntity : FullAuditedAndTenantEntity
    {
        public const int MaxUserNameLength = 64;
        public const int MaxPasswordLength = 128;
        public const int MaxEmailLength = 256;
        public const int MaxFirstNameLength = 128;
        public const int MaxLastNameLength = 128;

        [Required]
        [StringLength(MaxUserNameLength)]
        public virtual string UserName { get;  set; }
        [Required]
        [StringLength(MaxPasswordLength)]
        public virtual string Password { get;  set; }
        [Required]
        [StringLength(MaxEmailLength)]
        public virtual string Email { get;  set; }
        [Required]
        [StringLength(MaxFirstNameLength)]
        public virtual string FirstName { get;  set; }
        [Required]
        [StringLength(MaxLastNameLength)]
        public virtual string LastName { get;  set; }

        public virtual TenantEntity Tenant { get; set; }
    }
}
