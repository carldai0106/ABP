using System;
using System.IO;
using System.Reflection;
using Abp.Dependency;

namespace Abp.Localization.Sources.Json
{
    /// <summary>
    ///     XML based localization source.
    ///     It uses XML files to read localized strings.
    /// </summary>
    [Obsolete(
        "Directly use DictionaryBasedLocalizationSource with JsonFileLocalizationDictionaryProvider instead of this class"
        )]
    public class JsonLocalizationSource : DictionaryBasedLocalizationSource, ISingletonDependency
    {
        static JsonLocalizationSource()
        {
            RootDirectoryOfApplication = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        ///     Creates an Xml based localization source.
        /// </summary>
        /// <param name="name">Unique Name of the source</param>
        /// <param name="directoryPath">Directory path of the localization XML files</param>
        public JsonLocalizationSource(string name, string directoryPath)
            : base(name, new JsonFileLocalizationDictionaryProvider(directoryPath))
        {
        }

        internal static string RootDirectoryOfApplication { get; set; }
        //TODO: Find a better way of passing root directory
    }
}