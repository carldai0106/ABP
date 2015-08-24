using System.ComponentModel;

namespace Abp.Web.Mvc.Localized
{
    public class LocalizedDisplay : DisplayNameAttribute
    {
        private string _nameOrKey = string.Empty;

        public LocalizedDisplay(string nameOrKey)
        {
            _nameOrKey = nameOrKey;
        }

        public override string DisplayName
        {
            get
            {
                return TranslationHelper.L(_nameOrKey);
            }
        }
    }
}
