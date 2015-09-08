using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.Action.Dto;

namespace CMS.Application.Action
{
    public interface IActionAppService
    {
        Task<ActionEditDto> GetAction(IdInput<Guid> id);
        Task<ActionEditDto> GetAction(string actionCode);
        Task Create(ActionCreateDto input);
        Task Update(ActionEditDto input);
        Task Delete(IdInput<Guid> id);
        Task<PagedResultOutput<ActionEditDto>> GetActions(GetActionsInput input);
    }
}