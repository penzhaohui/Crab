using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Crab.DataModel.Configuration
{
    /// <summary>
    /// Represents the configuration elements associated with a provider.
    /// </summary>
    public sealed class KeyTypeMappingSettings : ConfigurationElement
    {
        private readonly ConfigurationProperty _propKey;
        private readonly ConfigurationProperty _propType;
        private ConfigurationPropertyCollection _properties;

        //properties
        [ConfigurationProperty("key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)base[this._propKey];
            }
            set
            {
                base[this._propKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the provider configured by this class.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base[this._propType];
            }
            set
            {
                base[this._propType] = value;
            }

        }

        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                //this.UpdatePropertyCollection();
                return this._properties;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Crab.DataModel.Configuration.KeyTypeMappingSettings class.
        /// </summary>
        public KeyTypeMappingSettings()
        {
            this._propKey = new ConfigurationProperty("key", typeof(string), null, ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
            this._propType = new ConfigurationProperty("type", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
            this._properties = new ConfigurationPropertyCollection();
            this._properties.Add(this._propKey);
            this._properties.Add(this._propType);
        }
        
        /// <summary>
        /// Initializes a new instance of the Crab.DataModel.Configuration.KeyTypeMappingSettings class.
        /// </summary>
        /// <param name="key">The key of the provider to configure settings for.</param>
        /// <param name="type">The type of the provider to configure settings for.</param>
        public KeyTypeMappingSettings(string key, string type)
        {
            this.Key = key;
            this.Type = type;
        }
    }
}
