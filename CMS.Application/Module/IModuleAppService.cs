using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.Module.Dto;

namespace CMS.Application.Module
{
    public interface IModuleAppService
    {
        Task<ModuleEditDto> GetModule(IdInput<Guid> input);
        Task<ModuleEditDto> GetModule(string moduleCode);
        Task<ModuleTreeNode> GetModuleTree(NullableIdInput<Guid> input);
        Task<PagedResultOutput<ModuleEditDto>> GetModules(GetModulesInput input);
        Task Update(ModuleEditDto input);
        Task Delete(IdInput<Guid> inpput);
        Task Create(ModuleCreateDto input);
        Task CreateOrUpdate(IEnumerable<ActionModuleDto> inputs);
    }
}