using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.UI;
using CMS.Application.Module.Dto;
using CMS.Domain;
using CMS.Domain.Module;

namespace CMS.Application.Module
{
    public class ModuleAppService : ApplicationService<Guid, Guid>, IModuleAppService
    {
        private readonly ICmsRepository<ModuleEntity, Guid> _repository;
        public ModuleAppService(ICmsRepository<ModuleEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ModuleEditDto> GetModule(IdInput<Guid> input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            return rs.MapTo<ModuleEditDto>();
        }

        public async Task Update(ModuleEditDto input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (rs == null)
            {
                throw new UserFriendlyException(string.Format("There is no module with id : {0}", input.Id));
            }

            input.MapTo(rs);

            rs = await _repository.FirstOrDefaultAsync(x => x.ModuleCode == input.ModuleCode);
            if (rs != null && rs.Id != input.Id)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateModuleCode"), input.ModuleCode));
            }

            await _repository.UpdateAsync(rs);
        }

        public async Task Delete(IdInput<Guid> input)
        {
            await _repository.DeleteAsync(input.Id);
        }

        public async Task Create(ModuleCreateDto input)
        {
            var module = await _repository.FirstOrDefaultAsync(x => x.ModuleCode == input.ModuleCode);
            if (module != null)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateModuleCode"), input.ModuleCode));
            }

            var mapped = input.MapTo<ModuleEntity>();
            mapped.Id = Guid.NewGuid();

            await _repository.InsertAsync(mapped);
        }
    }
}
