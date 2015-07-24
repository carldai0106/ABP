using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.Action.Dto;

namespace CMS.Application.Action
{
    public interface IActionAppService
    {
        Task<ActionEditDto> GetAction(IdInput<Guid> id);
        Task Create(ActionCreateDto input);
        Task Update(ActionEditDto input);
        Task Delete(IdInput<Guid> id);
        Task<PagedResultOutput<ActionEditDto>> GetActions(GetActionsInput input);
    }
}
