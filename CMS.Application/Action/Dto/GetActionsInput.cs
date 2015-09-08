using Abp.Runtime.Validation;

namespace CMS.Application.Action.Dto
{
    public class GetActionsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "ActionCode";
            }
        }
    }
}