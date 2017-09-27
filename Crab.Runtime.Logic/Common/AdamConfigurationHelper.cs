using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Logic.Common
{
    public static class AdamConfigurationHelper
    {
        static public AdamTenantManager AdamManager
        {
            get
            {
                return new AdamTenantManager(ConnectionString, ConnectionUsername, ConnectionPassword, SecureConnection);
            }
        }

        static public string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[Constants.AdamSettings.ConnectionStringName].ConnectionString;
            }
        }

        static public string ConnectionUsername
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AdamSettings.ConnectionUsername];
            }
        }

        static public string ConnectionPassword
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AdamSettings.ConnectionPassword];
            }
        }

        static public bool SecureConnection
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AdamSettings.SecureConnection]);
            }
        }
    }
}
