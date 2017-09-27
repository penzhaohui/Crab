using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract.Exceptions
{
    [DataContract, Serializable]
    public class AuthenticationException : Exception
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public AuthenticationException(string message)
            : base(message)
        {
        }

        protected AuthenticationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
