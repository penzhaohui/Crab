using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Configuration.Provider;
using System.Collections.Specialized;
using Crab.DataModel.Configuration;
using Crab.DataModel.Provider;

namespace Crab.DataModel
{
    /// <summary>
    /// The class to manages meta datas. This class cannot be    
    /// inherited.
    /// </summary>
    public static class MetadataManager
    {
        #region private fileds
        private static bool _initialized;
        private static object _lock;
        private static Exception _initializeException;
        private static MetadataProvider _provider;
        private static ProviderCollection _providers;
        private static KeyTypeMappingSettingsCollection _typeMappings;
        private const string SectionName = "datamodel";
        #endregion

        static MetadataManager()
        {
            _lock = new object();
            _initialized = false;
            _initializeException = null;
        }

        //properties
        public static MetadataProvider Provider
        {
            get
            {
                Initialize();
                return _provider;
            }
        }

        public static ProviderCollection Providers
        {
            get
            {
                Initialize();
                return _providers;
            }
        }

        public static KeyTypeMappingSettingsCollection TypeMappings
        {
            get
            {
                Initialize();
                return _typeMappings;
            }
        }
        //methods
        private static void Initialize()
        {
            if (_initialized)
            {
                if (_initializeException != null)
                {
                    throw _initializeException;
                }
            }
            else
            {
                if (_initializeException != null)
                {
                    throw _initializeException;
                }
                lock (_lock)
                {
                    if (_initialized)
                    {
                        if (_initializeException != null)
                        {
                            throw _initializeException;
                        }
                    }
                    else
                    {
                        try
                        {
                            DataModelSection section = ConfigurationManager.GetSection(SectionName) as DataModelSection;
                            _providers = new ProviderCollection();
                            //fill providers collection
                            foreach (ProviderSettings settings in section.Providers)
                            {
                                _providers.Add(InstantiateProvider(settings, typeof(MetadataProvider)));
                            }
                            _provider = (MetadataProvider)_providers[section.DefaultProvider];

                            _typeMappings = section.TypeMappings;
                        }
                        catch (Exception exception)
                        {
                            _initializeException = exception;
                            throw;
                        }
                        _initialized = true;
                    }
                }
            }
        }

        private static ProviderBase InstantiateProvider(ProviderSettings providerSettings, Type providerType)
        {
            ProviderBase providerBase = null;
            try
            {
                string typeName = (providerSettings.Type == null) ? null : providerSettings.Type.Trim();
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new ArgumentException("No provider type name found.");
                }
                Type newType = Type.GetType(typeName);
                if (!providerType.IsAssignableFrom(newType))
                {
                    throw new ArgumentException(string.Format("The provider must implement {0}", providerType.ToString()));
                }
                providerBase = (ProviderBase)Activator.CreateInstance(newType);
                //clone a name value collection
                NameValueCollection collection1 = providerSettings.Parameters;
                NameValueCollection collection2 = new NameValueCollection(collection1.Count);
                foreach (string textName in collection1)
                {
                    collection2[textName] = collection1[textName];
                }
                providerBase.Initialize(providerSettings.Name, collection2);
            }
            catch (Exception exception)
            {
                if (exception is ConfigurationException)
                {
                    throw;
                }
                throw new ConfigurationErrorsException(exception.Message, providerSettings.ElementInformation.Properties["type"].Source, providerSettings.ElementInformation.Properties["type"].LineNumber);
            }
            return providerBase;
        }

        //for provider
        public static DataNode GetDataNode(Guid id)
        {
            return Provider.GetDataNode(id);
        }

        public static DataNode[] GetChildNodes(Guid parentId)
        {
            return Provider.GetChildNodes(parentId);
        }

        public static DataNode[] GetAllNodes(int nodeType)
        {
            return Provider.GetAllNodes(nodeType);
        }

        public static DataNode NewDataNode(int nodeType)
        {
            return Provider.NewDataNode(nodeType);
        }

        public static DataNode CreateDataNode(DataNode node)
        {
            return Provider.CreateDataNode(node);
        }

        public static void UpdateDataNode(DataNode node)
        {
            Provider.UpdateDataNode(node);
        }

        public static void DeleteDataNode(DataNode node)
        {
            Provider.DeleteDataNode(node);
        }
    }
}
