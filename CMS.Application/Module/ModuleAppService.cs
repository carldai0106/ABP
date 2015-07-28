using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using Abp.Linq.Extensions;
using Abp.UI;

using CMS.Application.Localization;
using CMS.Application.Module.Dto;

using CMS.Domain;
using CMS.Domain.Module;
using Abp.Extensions;
using CMS.Domain.ActionModule;


namespace CMS.Application.Module
{
    public class ModuleAppService : ApplicationService<Guid, Guid>, IModuleAppService
    {
        private readonly ICmsRepository<ModuleEntity, Guid> _repository;
        private readonly ICmsRepository<ActionModuleEntity, Guid> _actionModuleRepository;
        public ModuleAppService(ICmsRepository<ModuleEntity, Guid> repository, ICmsRepository<ActionModuleEntity, Guid> actionModuleRepository)
        {
            _repository = repository;
            _actionModuleRepository = actionModuleRepository;

            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        public async Task<ModuleEditDto> GetModule(IdInput<Guid> input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            return rs.MapTo<ModuleEditDto>();
        }

        public async Task<ModuleEditDto> GetModule(string moduleCode)
        {
            var rs = await _repository.GetAll().Include(x => x.ActionModules).FirstOrDefaultAsync(x => x.ModuleCode == moduleCode);
            
            return rs.MapTo<ModuleEditDto>();
        }

        public async Task<PagedResultOutput<ModuleEditDto>> GetModules(GetModulesInput input)
        {

            var query = _repository.GetAll()
                .WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.ModuleCode.Contains(input.Filter) || x.DisplayName.Contains(input.Filter));
            var count = await query.CountAsync();
            var users = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var dtos = users.MapTo<List<ModuleEditDto>>();

            return new PagedResultOutput<ModuleEditDto>(count, dtos);
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

        public async Task CreateOrUpdate(IEnumerable<ActionModuleDto> inputs)
        {
            foreach (var item in inputs)
            {
                var mapped = item.MapTo<ActionModuleEntity>();

                if (!item.Id.HasValue)
                {
                    mapped.Id = Guid.NewGuid();
                    await _actionModuleRepository.InsertAsync(mapped);
                }
                else
                    await _actionModuleRepository.UpdateAsync(mapped);
            }
        }
    }
}
