using System;
using System.Web.Mvc;

namespace CMS.Web
{
    public class GuidModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue("id");
            bindingContext.ModelState.SetModelValue("id", valueResult);

            var oVal = valueResult.ConvertTo(typeof (Guid));
            return oVal;
        }
    }
}
