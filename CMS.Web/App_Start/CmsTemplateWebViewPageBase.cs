using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Web.Mvc.Views;
using CMS.Application.Localization;

namespace CMS.Web
{
    public abstract class CmsTemplateWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CmsTemplateWebViewPageBase()
        {
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
           
        }
    }

    public abstract class CmsTemplateWebViewPageBase : CmsTemplateWebViewPageBase<dynamic>
    {

    }
}
