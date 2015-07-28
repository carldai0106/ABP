using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.UserRole;

namespace CMS.Application.User.Dto
{
    [AutoMap(typeof(UserRoleEntity))]
    public class UserRoleDto : EntityDto<Guid?>, IDoubleWayDto
    {
        [Required]
        public virtual Guid UserId { get; set; }
        [Required]
        public virtual Guid RoleId { get; set; }
        public virtual bool Status { get; set; }
    }
}
