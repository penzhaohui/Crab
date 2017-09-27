using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Crab.DataModel.Configuration
{
    /// <summary>
    /// Represents the "<datamodel/>" section within a configuration file.
    /// </summary>
    public sealed class DataModelSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty _propDefaultProvider;
        private static readonly ConfigurationProperty _propProviders;
        private static ConfigurationPropertyCollection _properties;
        private static ConfigurationProperty _propTypeMappings;

        [ConfigurationProperty("defaultProvider", DefaultValue = "SqlDataModelProvider"), StringValidator(MinLength = 1)]
        public string DefaultProvider
        {
            get
            {
                return (string)base[DataModelSection._propDefaultProvider];
            }
            set
            {
                base[DataModelSection._propDefaultProvider] = value;
            }
        }

        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base[DataModelSection._propProviders];
            }
        }

        [ConfigurationProperty("typemappings")]
        public KeyTypeMappingSettingsCollection TypeMappings
        {
            get
            {
                return (KeyTypeMappingSettingsCollection)base[DataModelSection._propTypeMappings];
            }
        }
 
        public DataModelSection()
        {
        }

        static DataModelSection()
        {
            _propDefaultProvider = new ConfigurationProperty("defaultProvider", typeof(string), "SqlDataModelProvider");
            _propProviders = new ConfigurationProperty("providers", typeof(ProviderSettingsCollection), null, ConfigurationPropertyOptions.None);
            _propTypeMappings = new ConfigurationProperty("typemappings", typeof(KeyTypeMappingSettingsCollection), null, ConfigurationPropertyOptions.None);
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_propDefaultProvider);
        }
    }
}
