using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class AdamUser
    {
        private byte[] _sid;
        private string _principleName;
        private string _email;

        [DataMember]
        public byte[] Sid
        {
            get { return _sid; }
            set { _sid = value; }
        }

        [DataMember]
        public string PrincipleName
        {
            get { return _principleName; }
            set { _principleName = value; }
        }

        [DataMember]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
    }
}
