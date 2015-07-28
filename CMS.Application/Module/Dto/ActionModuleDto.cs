using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.ActionModule;

namespace CMS.Application.Module.Dto
{
    [AutoMap(typeof(ActionModuleEntity))]
    public class ActionModuleDto : EntityDto<Guid?>, IDoubleWayDto
    {
        [Required]
        public virtual Guid ModuleId { get; set; }
        [Required]
        public virtual Guid ActionId { get; set; }
        public virtual bool Status { get; set; }
    }
}
