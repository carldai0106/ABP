using System.Collections.Generic;
using System.IO;
using Abp.Localization.Dictionaries.Xml;

namespace Abp.Localization.Sources.Json
{
    /// <summary>
    ///     Provides localization dictionaries from XML files in a directory.
    /// </summary>
    public class JsonFileLocalizationDictionaryProvider : ILocalizationDictionaryProvider
    {
        private readonly string _directoryPath;

        /// <summary>
        ///     Creates a new <see cref="JsonFileLocalizationDictionaryProvider" />.
        /// </summary>
        /// <param name="directoryPath">Path of the dictionary that contains all related XML files</param>
        public JsonFileLocalizationDictionaryProvider(string directoryPath)
        {
            if (!Path.IsPathRooted(directoryPath))
            {
                directoryPath = Path.Combine(JsonLocalizationSource.RootDirectoryOfApplication, directoryPath);
            }

            _directoryPath = directoryPath;
        }

        public IEnumerable<LocalizationDictionaryInfo> GetDictionaries(string sourceName)
        {
            var fileNames = Directory.GetFiles(_directoryPath, "*.json", SearchOption.TopDirectoryOnly);

            var dictionaries = new List<LocalizationDictionaryInfo>();

            foreach (var fileName in fileNames)
            {
                dictionaries.Add(
                    new LocalizationDictionaryInfo(
                        XmlLocalizationDictionary.BuildFomFile(fileName), fileName.EndsWith(sourceName + ".json")
                        )
                    );
            }

            return dictionaries;
        }
    }
}