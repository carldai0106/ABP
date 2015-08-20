using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Localized
{
    public class LocalizedRegularExpression : RegularExpressionAttribute
    {
        public LocalizedRegularExpression(string pattern)
            : base(pattern)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(Localization.GetLang(ErrorMessage), name);
        }       
    }
}
