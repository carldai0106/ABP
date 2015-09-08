using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CMS.Domain.User;

namespace CMS.Application.User.Dto
{
    public class CreateUserDto : IInputDto, IPassivable
    {
        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 20;

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

        public bool IsActive { get; set; }
    }
}