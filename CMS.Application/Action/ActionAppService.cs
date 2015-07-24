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
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using CMS.Application.Action.Dto;
using CMS.Application.IdentityFramework;
using CMS.Application.Localization;
using CMS.Application.User.Dto;
using CMS.Domain;
using CMS.Domain.Action;
using CMS.Domain.User;
using Microsoft.AspNet.Identity;

namespace CMS.Application.Action
{
    public class ActionAppService : ApplicationService<Guid,Guid>, IActionAppService
    {
        private readonly ICmsRepository<ActionEntity, Guid> _repository;
        public ActionAppService(ICmsRepository<ActionEntity, Guid> repository)
        {
            _repository = repository;
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        public async Task<ActionEditDto> GetAction(IdInput<Guid> id)
        {
            var action = await _repository.FirstOrDefaultAsync(x => x.Id == id.Id);
            return action.MapTo<ActionEditDto>();
        }

        public async Task Create(ActionCreateDto input)
        {
            if (await _repository.FirstOrDefaultAsync(x => x.ActionCode == input.ActionCode) != null)
            {
                IdentityResult.Failed(string.Format(L("Identity.DuplicateActionCode"), input.ActionCode)).CheckErrors();
            }

            var action = input.MapTo<ActionEntity>();
            action.Id = Guid.NewGuid();
            
            await _repository.InsertAsync(action);
        }

        public async Task Update(ActionEditDto input)
        {
            var rs = await _repository.FirstOrDefaultAsync(x => x.ActionCode == input.ActionCode);
            if (rs != null && rs.Id != input.Id)
            {
                IdentityResult.Failed(string.Format(L("Identity.DuplicateActionCode"), input.ActionCode)).CheckErrors();
            }

            var action = rs.MapTo<ActionEntity>();
            await _repository.UpdateAsync(action);
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
