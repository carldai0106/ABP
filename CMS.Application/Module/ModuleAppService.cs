using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using CMS.Application.Module.Dto;
using CMS.Domain;
using CMS.Domain.ActionModule;
using CMS.Domain.Module;
using Newtonsoft.Json;

namespace CMS.Application.Module
{
    public class ModuleAppService : CmsAppServiceBase, IModuleAppService
    {
        private readonly ICmsRepository<ActionModuleEntity, Guid> _actionModuleRepository;
        private readonly ICmsRepository<ModuleEntity, Guid> _repository;

        public ModuleAppService(ICmsRepository<ModuleEntity, Guid> repository,
            ICmsRepository<ActionModuleEntity, Guid> actionModuleRepository)
        {
            _repository = repository;
            _actionModuleRepository = actionModuleRepository;
        }

        public async Task<ModuleEditDto> GetModule(IdInput<Guid> input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            return rs.MapTo<ModuleEditDto>();
        }

        public async Task<ModuleEditDto> GetModule(string moduleCode)
        {
            var rs =
                await
                    _repository.GetAll()
                        .Include(x => x.ActionModules)
                        .FirstOrDefaultAsync(x => x.ModuleCode == moduleCode);

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

            var result = await _repository.FirstOrDefaultAsync(x => x.ModuleCode == input.ModuleCode);
            if (result != null && result.Id != input.Id)
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
                    if (
                        await
                            _actionModuleRepository.FirstOrDefaultAsync(
                                x => x.ActionId == item.ActionId && x.ModuleId == item.ModuleId) == null)
                    {
                        mapped.Id = Guid.NewGuid();
                        await _actionModuleRepository.InsertAsync(mapped);
                    }
                }
                else
                    await _actionModuleRepository.UpdateAsync(mapped);
            }
        }

        public async Task<ModuleTreeNode> GetModuleTree(NullableIdInput<Guid> input)
        {
            var query = _repository.GetAll();
            var list = await query.ToListAsync();
            var mappedList = list.MapTo<List<ModuleTreeNode>>();
            var root = new ModuleTreeNode
            {
                ModuleCode = "Root",
                DisplayName = "Root",
                ParentId = null,
                Id = null
            };

            return GetSubItem(mappedList, root, input.Id);
        }

        private static ModuleTreeNode GetSubItem(IEnumerable<ModuleTreeNode> source, ModuleTreeNode parentNode,
            Guid? currentId)
        {
            if (source == null || parentNode == null)
            {
                return null;
            }
            var list = source.Where(x => x.ParentId == parentNode.Id).OrderBy(x => x.Order);

            foreach (var item in list)
            {
                var json = JsonConvert.SerializeObject(item);
                var child = JsonConvert.DeserializeObject<ModuleTreeNode>(json);
                child.IsActived = child.Id == currentId;

                parentNode.Children.Add(GetSubItem(source, child, currentId));
                if (child.Children.Any(x => x.IsActived))
                {
                    child.IsActived = true;
                }
            }

            return parentNode;
        }
    }
}