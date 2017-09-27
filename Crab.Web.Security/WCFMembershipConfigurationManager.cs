using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Security;
using System.Web.Configuration;

namespace Crab.Web.Security
{
    // Summary:
    //     Provides access to the settings of ADAM from the configuration file
    static class WCFMembershipConfigurationManager
    {
        private static bool _enablePasswordReset;
        private static int _minRequiredNonAlphanumericCharacters;
        private static int _minRequiredPasswordLength;
        private static string _passwordStrengthRegularExpression;

        public static bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public static int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        public static int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public static string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }


        static WCFMembershipConfigurationManager()
        {
            MembershipSection membershipSection =
            (MembershipSection)ConfigurationManager.GetSection("system.web/membership");
            if (membershipSection == null)
                return;
            string providerName = membershipSection.DefaultProvider;
            ProviderSettings providerItem = membershipSection.Providers[providerName];

            if (providerItem == null)
                return;

            Type type = Type.GetType(providerItem.Type);
            if (type == null || !(Activator.CreateInstance(type) is WCFTenantMembershipProvider))
                return;

            _enablePasswordReset = string.IsNullOrEmpty(providerItem.Parameters["enablePasswordReset"]) ? true :
                Convert.ToBoolean(providerItem.Parameters["enablePasswordReset"]);
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(providerItem.Parameters["minRequiredNonAlphanumericCharacters"]);
            _minRequiredPasswordLength = Convert.ToInt32(providerItem.Parameters["minRequiredPasswordLength"]);
            _passwordStrengthRegularExpression = providerItem.Parameters["passwordStrengthRegularExpression"];
            if (_passwordStrengthRegularExpression == null)
                _passwordStrengthRegularExpression = string.Empty;
        }
    }
}
