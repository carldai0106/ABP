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
