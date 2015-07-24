using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.Module.Dto;

namespace CMS.Application.Module
{
    public interface IModuleAppService
    {
        Task<ModuleEditDto> GetModule(IdInput<Guid> input);
        Task Update(ModuleEditDto input);
        Task Delete(IdInput<Guid> inpput);
        Task Create(ModuleCreateDto input);
    }
}
