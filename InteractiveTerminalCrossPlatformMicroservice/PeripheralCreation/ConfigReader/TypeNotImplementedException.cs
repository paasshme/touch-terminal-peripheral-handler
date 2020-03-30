using System;
using System.Runtime.Serialization;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /// <summary>
    ///  Exception thrown when a device used parameters in its constructor that isn't recognized as a primitive type
    /// </summary>
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
