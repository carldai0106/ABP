using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;


namespace Framework.Core.Localized
{
    public class LocalizedDescription : DescriptionAttribute
    {
        private string _description = string.Empty;
        public LocalizedDescription(string description)
        {
            _description = Localization.GetLang(description);
            base.DescriptionValue = _description;
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
