using System;
using System.Runtime.Serialization;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation
{
    /*
     * Exception thrown when the XML configuration file 
     * includes a device with parameters (constructor parameters)
     * and one of the parameter isn't primitive (int,float,double,short,long,string,char,bool)
     */
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
