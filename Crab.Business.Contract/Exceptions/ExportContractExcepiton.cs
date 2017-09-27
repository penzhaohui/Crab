using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Business.Contract.Exceptions
{
    [DataContract, Serializable]
    public class ExportContractExcepiton : Exception
    {
        public ExportContractExcepiton()
        {

        }

        public ExportContractExcepiton(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ExportContractExcepiton(string message)
            : base(message)
        {

        }

        protected ExportContractExcepiton(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
