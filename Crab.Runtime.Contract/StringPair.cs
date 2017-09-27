using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class StringPair
    {
        private string _key;
        private string _value;

        public StringPair()
        {
        }

        public StringPair(string key, string value)
        {
            _key = key;
            _value = value;
        }

        [DataMember]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        [DataMember]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
