using System.Threading.Tasks;
using Abp.Application.Services;
using CMS.Application.Sessions.Dto;

namespace CMS.Application.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}