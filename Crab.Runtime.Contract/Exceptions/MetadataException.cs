using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract.Exceptions
{
    [DataContract, Serializable]
    public class MetadataException : Exception
    {
        public MetadataException()
        {

        }

        public MetadataException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public MetadataException(string message)
            : base(message)
        {

        }

        protected MetadataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
