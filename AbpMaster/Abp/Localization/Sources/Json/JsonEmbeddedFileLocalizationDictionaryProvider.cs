using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Abp.IO.Extensions;
using Abp.Localization.Dictionaries.Json;

namespace Abp.Localization.Sources.Json
{
    /// <summary>
    /// Provides localization dictionaries from JSON files embedded into an <see cref="Assembly"/>.
    /// </summary>
    public class JsonEmbeddedFileLocalizationDictionaryProvider : ILocalizationDictionaryProvider
    {
        private readonly Assembly _assembly;
        private readonly string _rootNamespace;

        /// <summary>
        /// Creates a new <see cref="JsonEmbeddedFileLocalizationDictionaryProvider"/> object.
        /// </summary>
        /// <param name="assembly">Assembly that contains embedded xml files</param>
        /// <param name="rootNamespace">Namespace of the embedded xml dictionary files</param>
        public JsonEmbeddedFileLocalizationDictionaryProvider(Assembly assembly, string rootNamespace)
        {
            _assembly = assembly;
            _rootNamespace = rootNamespace;
        }

        public IEnumerable<LocalizationDictionaryInfo> GetDictionaries(string sourceName)
        {
            var dictionaries = new List<LocalizationDictionaryInfo>();
            
            var resourceNames = _assembly.GetManifestResourceNames();
            foreach (var resourceName in resourceNames)
            {
                if (resourceName.StartsWith(_rootNamespace))
                {
                    using (var stream = _assembly.GetManifestResourceStream(resourceName))
                    {
                        var bytes = stream.GetAllBytes();
                        var xmlString = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3); //Skipping byte order mark
                        dictionaries.Add(
                            new LocalizationDictionaryInfo(
                                JsonLocalizationDictionary.BuildFomJsonString(xmlString),
                                isDefault: resourceName.EndsWith(sourceName + ".json")
                                )
                            );
                    }
                }
            }

            return dictionaries;
        }
    }
}