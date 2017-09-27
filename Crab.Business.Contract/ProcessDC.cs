using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Business.Contract
{
    [DataContract, Serializable]
    public class ProcessDC
    {
        private Guid _processId;
        private Guid _objectId;
        private ProcessStatus _status;

        [DataMember]
        public Guid ProcessId
        {
            get { return _processId; }
            set { _processId = value; }
        }

        [DataMember]
        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        [DataMember]
        public ProcessStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }
    }
}
