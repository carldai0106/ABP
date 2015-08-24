using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedRequired : RequiredAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return string.Format(TranslationHelper.L(ErrorMessageString), name);
        }
    }
}
