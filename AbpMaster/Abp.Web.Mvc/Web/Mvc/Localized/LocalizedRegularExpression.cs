using System.ComponentModel.DataAnnotations;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedRegularExpression : RegularExpressionAttribute
    {
        public LocalizedRegularExpression(string pattern)
            : base(pattern)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(TranslationHelper.L(ErrorMessage), name);
        }       
    }
}
