using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace Framework.Core.Localized
{
    public class LocalizedRemote : RemoteAttribute
    {
        public LocalizedRemote(string routeName)
            : base(routeName)
        {
        }

        public LocalizedRemote(string action, string controller)
            : base(action, controller)
        {
        }

        

        public LocalizedRemote(string action, string controller, string areaName)
            : base(action, controller, areaName)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(Localization.GetLang(base.ErrorMessage), name);
        }
    }
}
