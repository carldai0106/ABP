using System.ComponentModel;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedDescription : DescriptionAttribute
    {
        private string _description = string.Empty;
        public LocalizedDescription(string description)
        {
            _description = TranslationHelper.L(description);
            DescriptionValue = _description;
        }

        public override string Description
        {
            get
            {
                return _description;
            }
        }
    }
}
