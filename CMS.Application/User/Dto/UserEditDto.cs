using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using CMS.Domain.User;

namespace CMS.Application.User.Dto
{
    
    public class UserEditDto : IDoubleWayDto
    {
        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 20;

        public Guid Id { get; set; }
        [Required]
        [StringLength(UserEntity.MaxUserNameLength)]
        public virtual string UserName { get; set; }
        [StringLength(MaxPasswordLength, MinimumLength = MinPasswordLength)]
        public virtual string Password { get; set; }
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
