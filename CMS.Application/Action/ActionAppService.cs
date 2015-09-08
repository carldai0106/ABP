using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using CMS.Application.Action.Dto;
using CMS.Domain;
using CMS.Domain.Action;

namespace CMS.Application.Action
{
    public class ActionAppService : CmsAppServiceBase, IActionAppService
    {
        private readonly ICmsRepository<ActionEntity, Guid> _repository;

        public ActionAppService(ICmsRepository<ActionEntity, Guid> repository)
        {
            _repository = repository;
        }

        public async Task<ActionEditDto> GetAction(IdInput<Guid> id)
        {
            var action = await _repository.FirstOrDefaultAsync(x => x.Id == id.Id);
            return action.MapTo<ActionEditDto>();
        }

        public async Task<ActionEditDto> GetAction(string actionCode)
        {
            var action = await _repository.FirstOrDefaultAsync(x => x.ActionCode == actionCode);
            return action.MapTo<ActionEditDto>();
        }

        public async Task Create(ActionCreateDto input)
        {
            if (await _repository.FirstOrDefaultAsync(x => x.ActionCode == input.ActionCode) != null)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateActionCode"), input.ActionCode));
            }

            var action = input.MapTo<ActionEntity>();
            action.Id = Guid.NewGuid();

            await _repository.InsertAsync(action);
        }

        public async Task Update(ActionEditDto input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (rs == null)
            {
                throw new UserFriendlyException(string.Format("There is no actin with id : {0}", input.Id));
            }

            input.MapTo(rs);

            var result = await _repository.FirstOrDefaultAsync(x => x.ActionCode == input.ActionCode);

            if (result != null && result.Id != input.Id)
            {
                throw new UserFriendlyException(string.Format(L("DuplicateActionCode"), input.ActionCode));
            }

            await _repository.UpdateAsync(rs);
        }

        public async Task Delete(IdInput<Guid> id)
        {
            await _repository.DeleteAsync(id.Id);
        }

        public async Task<PagedResultOutput<ActionEditDto>> GetActions(GetActionsInput input)
        {
            var query = _repository.GetAll();
            var count = await query.CountAsync();
            var users = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var dtos = users.MapTo<List<ActionEditDto>>();

            return new PagedResultOutput<ActionEditDto>(count, dtos);
        }
    }
}