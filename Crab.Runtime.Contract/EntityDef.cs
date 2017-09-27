using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class EntityDef
    {
        #region private members
        private Guid _id;
        private string _name;
        private string _caption;
        #endregion

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }
    }
}
