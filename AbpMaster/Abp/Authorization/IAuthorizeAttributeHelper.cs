using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Authorization
{
    internal interface IAuthorizeAttributeHelper<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes);
        Task AuthorizeAsync(IAbpAuthorizeAttribute authorizeAttribute);
        void Authorize(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes);
        void Authorize(IAbpAuthorizeAttribute authorizeAttribute);
    }
}