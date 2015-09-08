using System;
using Abp.Application.Services.Dto;

namespace CMS.Application.User.Dto
{
    public class PermissionDto : IOutputDto
    {
        public virtual Guid RoleId { get; set; }
        public virtual string RoleCode { get; set; }
        public virtual Guid ActionId { get; set; }
        public virtual string ActionCode { get; set; }
        public virtual Guid ModuleId { get; set; }
        public virtual string ModuleCode { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual bool Status { get; set; }
    }
}