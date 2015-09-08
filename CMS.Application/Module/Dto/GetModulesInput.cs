using Abp.Runtime.Validation;

namespace CMS.Application.Module.Dto
{
    public class GetModulesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "ModuleCode";
            }
        }
    }
}