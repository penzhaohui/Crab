using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class Upn
    {
        private string _tenantName;
        private string _username;
        private const char Seperator = '@';

        [DataMember]
        public String TenantName
        {
            get { return _tenantName; }
            set { _tenantName = value; }
        }

        [DataMember]
        public string Username
        {
            get { return _username; }
        }


        public Upn()
        {
        }

        public Upn(string tenantName, string username)
        {
            _tenantName = tenantName;
            _username = username;
        }

        public Upn(Upn upn)
        {
            _tenantName = upn._tenantName;
            _username = upn._username;
        }

        public Upn(string upn)
        {
            Upn.TryParse(upn, out _tenantName, out _username);
        }


        public override string ToString()
        {
            return _username + Seperator + _tenantName;
        }

        static public bool TryParse(string upn, out string tenantName, out string username)
        {
            tenantName = null;
            username = null;
            if (string.IsNullOrEmpty(upn))
                return false;
            int pos = upn.IndexOf(Seperator);
            if (pos <=0 || pos == upn.Length-1)
                return false;
            username = upn.Substring(0, pos).Trim();
            tenantName = upn.Substring(pos + 1).Trim();
            if (tenantName.Length == 0 || username.Length == 0)
                return false;
            return true;
        }
    }
}
