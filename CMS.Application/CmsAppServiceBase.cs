using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using CMS.Application.Localization;

namespace CMS.Application
{
    public abstract class CmsAppServiceBase : ApplicationService<Guid, Guid>
    {
        protected CmsAppServiceBase()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }
    }
}
