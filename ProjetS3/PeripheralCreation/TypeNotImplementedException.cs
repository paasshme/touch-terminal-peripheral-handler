using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProjetS3.PeripheralCreation
{
    public class TypeNotImplementedException : Exception
    {

        public TypeNotImplementedException(string message) : base(message)
        {
        }

        public TypeNotImplementedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TypeNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
