using System;
using System.Runtime.Serialization;

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
