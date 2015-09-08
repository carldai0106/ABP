using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace CMS.Application
{
    public class PagedAndSortedInputDto : IInputDto, IPagedResultRequest, ISortedResultRequest
    {
        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

        [Range(1, AppConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public string Sorting { get; set; }
    }
}