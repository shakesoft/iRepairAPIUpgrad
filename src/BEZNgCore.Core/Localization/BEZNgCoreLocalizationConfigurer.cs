using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace BEZNgCore.Localization;

public static class BEZNgCoreLocalizationConfigurer
{
    public static void Configure(ILocalizationConfiguration localizationConfiguration)
    {
        localizationConfiguration.Sources.Add(
            new DictionaryBasedLocalizationSource(
                BEZNgCoreConsts.LocalizationSourceName,
                new XmlEmbeddedFileLocalizationDictionaryProvider(
                    typeof(BEZNgCoreLocalizationConfigurer).GetAssembly(),
                    "BEZNgCore.Localization.BEZNgCore"
                )
            )
        );
    }
}

