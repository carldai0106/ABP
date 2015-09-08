using Abp.Runtime.Validation;

namespace CMS.Application.Role.Dto
{
    public class GetRolesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "RoleCode";
            }
        }
    }
}