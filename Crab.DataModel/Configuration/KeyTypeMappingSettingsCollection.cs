using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Crab.DataModel.Configuration
{
    [ConfigurationCollection(typeof(KeyTypeMappingSettings))]
    public sealed class KeyTypeMappingSettingsCollection : ConfigurationElementCollection
    {
        // Methods
        static KeyTypeMappingSettingsCollection()
        {
            _properties = new ConfigurationPropertyCollection();
        }

        public KeyTypeMappingSettingsCollection()
        {
        }

        public void Add(KeyTypeMappingSettings keyTypeMapping)
        {
            if (keyTypeMapping != null)
            {
                this.BaseAdd(keyTypeMapping);
            }

        }

        public void Clear()
        {
            base.BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyTypeMappingSettings();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KeyTypeMappingSettings)element).Key;
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public KeyTypeMappingSettings this[object key]
        {
            get
            {
                return (KeyTypeMappingSettings)base.BaseGet(key);
            }
        }

        protected override ConfigurationPropertyCollection Properties 
        {
            get
            {
                return _properties;
            }
        }

        // Fields
        private static ConfigurationPropertyCollection _properties;
    }
}
