using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Business.Contract
{
    [DataContract, Serializable]
    public class ExportProcessDC: ProcessDC
    {
        private string _number;
        private string _creator;
        private decimal _amount;

        [DataMember]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [DataMember]
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        [DataMember]
        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
