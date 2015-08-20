using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedCompare : CompareAttribute
    {
        public LocalizedCompare(string otherProperty)
            : base(otherProperty)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(Localization.L(base.ErrorMessage), base.OtherPropertyDisplayName, name);
        }
    }
}
