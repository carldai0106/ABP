using System.Linq;
using System.Reflection;
using Abp.Localization.Sources.Json;
using Abp.Localization.Sources.Xml;
using Shouldly;
using Xunit;

namespace Abp.Tests.Localization
{
    public class JsonEmbeddedFileLocalizationDictionaryProvider_Tests
    {
        private readonly JsonEmbeddedFileLocalizationDictionaryProvider _dictionaryProvider;

        public JsonEmbeddedFileLocalizationDictionaryProvider_Tests()
        {
            _dictionaryProvider = new JsonEmbeddedFileLocalizationDictionaryProvider(
                Assembly.GetExecutingAssembly(),
                "Abp.Tests.Localization.TestJsonFiles"
                );
        }

        [Fact]
        public void Should_Get_Dictionaries()
        {
            var dictionaries = _dictionaryProvider.GetDictionaries("Test-en").ToList();
            
            dictionaries.Count.ShouldBe(2);

            var enDict = dictionaries.FirstOrDefault(d => d.Dictionary.CultureInfo.Name == "en");
            enDict.ShouldNotBe(null);
            enDict.IsDefault.ShouldBe(true);
            enDict.Dictionary["SmtpHost"].ShouldBe("SMTP host");
            
            var trDict = dictionaries.FirstOrDefault(d => d.Dictionary.CultureInfo.Name == "zh-CN");
            trDict.ShouldNotBe(null);
            trDict.Dictionary["SmtpHost"].ShouldBe("SMTP主机");            
        }
    }
}
