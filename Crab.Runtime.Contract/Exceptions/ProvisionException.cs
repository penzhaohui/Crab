using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract.Exceptions
{
    [DataContract, Serializable]
    public class ProvisioningException : Exception
    {
        public ProvisioningException()
        {

        }

        public ProvisioningException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public ProvisioningException(string message)
            : base(message)
        {

        }

        protected ProvisioningException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
