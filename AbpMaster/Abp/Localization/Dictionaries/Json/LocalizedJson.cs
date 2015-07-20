using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizedJson
    {
        /// <summary>
        /// 
        /// </summary>
        public LocalizedJson()
        {
            KeyValuePairs = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> KeyValuePairs { get; private set; }
    }
}
