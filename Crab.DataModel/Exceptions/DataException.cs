using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.DataModel.Exceptions
{
    [Serializable]
    public class DataException : SystemException
    {
        public DataException()
            : base()
        {
        }

        public DataException(string errMsg)
            :base(errMsg)
        {
        }

        protected DataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected DataException(string errMsg, Exception innerException)
            : base(errMsg, innerException)
        {
        }
    }
}
