using System.Globalization;
using Abp.Localization;
using Abp.Localization.Sources;

namespace Abp.Web.Mvc.Localized
{
    /// <summary>
    /// 
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Gets/sets name of the localization source that is used in this controller.
        /// It must be set in order to use <see cref="L(string)"/> and <see cref="L(string,CultureInfo)"/> methods.
        /// </summary>
        public string LocalizationSourceName
        {
            get;
            set;
        }

        private ILocalizationSource _localizationSource;

        /// <summary>
        /// 
        /// </summary>
        public Translation()
        {
            _localizationSource = NullLocalizationSource.Instance;
        }

        private void SetLocalizationSource()
        {
            _localizationSource = LocalizationHelper.GetSource(LocalizationSourceName);
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        public string L(string name)
        {
            SetLocalizationSource();
            return _localizationSource.GetString(name);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public string L(string name, params object[] args)
        {
            SetLocalizationSource();
            return _localizationSource.GetString(name, args);
        }

        /// <summary>
        /// Gets localized string for given key name and specified culture information.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <returns>Localized string</returns>
        public string L(string name, CultureInfo culture)
        {
            SetLocalizationSource();
            return _localizationSource.GetString(name, culture);
        }

        /// <summary>
        /// Gets localized string for given key name and current language with formatting strings.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <param name="culture">culture information</param>
        /// <param name="args">Format arguments</param>
        /// <returns>Localized string</returns>
        public string L(string name, CultureInfo culture, params object[] args)
        {
            SetLocalizationSource();
            return _localizationSource.GetString(name, culture, args);
        }
    }
}
