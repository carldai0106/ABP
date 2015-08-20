using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Localized
{
    public class LocalizedStringLength : StringLengthAttribute
    {
        public LocalizedStringLength(int maximumLength)
            : base(maximumLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(Localization.GetLang(ErrorMessage), name, base.MinimumLength, base.MaximumLength);
        }
    }
}
