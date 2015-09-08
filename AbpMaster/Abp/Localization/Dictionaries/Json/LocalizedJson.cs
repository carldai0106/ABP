using System.Collections.Generic;

namespace Abp.Localization.Dictionaries.Json
{
    /// <summary>
    /// </summary>
    public class LocalizedJson
    {
        /// <summary>
        /// </summary>
        public LocalizedJson()
        {
            KeyValuePairs = new Dictionary<string, string>();
        }

        /// <summary>
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// </summary>
        public Dictionary<string, string> KeyValuePairs { get; private set; }
    }
}