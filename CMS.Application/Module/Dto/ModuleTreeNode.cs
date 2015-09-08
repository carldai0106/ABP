using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Module;

namespace CMS.Application.Module.Dto
{
    [AutoMap(typeof (ModuleEntity))]
    public class ModuleTreeNode : IOutputDto
    {
        private readonly List<ModuleTreeNode> _list = new List<ModuleTreeNode>();
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }
        public string ModuleCode { get; set; }
        public string DisplayName { get; set; }
        public int Order { get; set; }
        public bool IsActived { get; set; }
        public string AppUrl { get; set; }
        public string ClassName { get; set; }

        public List<ModuleTreeNode> Children
        {
            get { return _list; }
        }
    }
}