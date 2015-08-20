using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Localized;
using CMS.Domain.User;

namespace CMS.Application.User.Dto
{
    
    public class UserEditDto : IDoubleWayDto
    {
        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 20;

        public Guid Id { get; set; }

        [LocalizedDisplay("UserName")]
        [Required]
        [StringLength(UserEntity.MaxUserNameLength)]
        public virtual string UserName { get; set; }
        [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
        public virtual string Password { get; set; }

        [LocalizedDisplay("ReenterPassword")]
        //[LocalizedStringLength(20, MinimumLength = 6, ErrorMessage = "Please enter {0} between {1} and {2} characters long.")]
        //[LocalizedCompare("Password", ErrorMessage = "{0} and {1} does not match.")]
        public virtual string ConfimPassword { get; set; }
        
        [Required]
        [StringLength(UserEntity.MaxEmailLength)]
        public virtual string Email { get; set; }
        [Required]
        [StringLength(UserEntity.MaxFirstNameLength)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(UserEntity.MaxLastNameLength)]
        public virtual string LastName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual ICollection<UserRoleDto> UserRoles { get; set; }
    }
}
