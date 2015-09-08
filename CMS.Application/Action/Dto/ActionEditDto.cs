using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Action;

namespace CMS.Application.Action.Dto
{
    [AutoMap(typeof (ActionEntity))]
    public class ActionEditDto : EntityDto<Guid>, IDoubleWayDto
    {
        public const int MaxActionCodeLength = 128;
        public const int MaxDisplayNameLength = 256;
        public const int MaxDescriptionLength = 512;

        [Required]
        [StringLength(MaxActionCodeLength)]
        public string ActionCode { get; set; }

        [Required]
        [StringLength(MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}