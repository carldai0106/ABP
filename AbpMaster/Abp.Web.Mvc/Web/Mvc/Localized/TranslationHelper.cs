using System;
using System.Globalization;
using Abp.Dependency;

namespace Abp.Web.Mvc.Localized
{
    /// <summary>
    /// 
    /// </summary>
    public class TranslationHelper
    {
        private static readonly Lazy<Translation> Translation;

        static TranslationHelper()
        {
            Translation = new Lazy<Translation>(
                () => IocManager.Instance.IsRegistered<Translation>()
                    ? IocManager.Instance.Resolve<Translation>()
                    : new Translation());
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        public static string L(string name)
        {
            return Translation.Value.L(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public static string L(string name, params object[] args)
        {
            return Translation.Value.L(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <returns>Localized string</returns>
        public static string L(string name, CultureInfo culture)
        {
            return Translation.Value.L(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public static string L(string name, CultureInfo culture, params object[] args)
        {
            return Translation.Value.L(name, culture, args);
        }
    }
}
