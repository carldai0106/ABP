using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedStringLength : StringLengthAttribute
    {
        public LocalizedStringLength(int maximumLength)
            : base(maximumLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(TranslationHelper.L(ErrorMessage), name, MinimumLength, MaximumLength);
        }
    }
}
