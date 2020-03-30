namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation
{
    /*
     * Exception thrown when the facotry tries to load a .dll file but it is not present in the folder
     */
    public class MissingDllException : System.Exception
    {
        public MissingDllException() { }
        public MissingDllException(string message) : base(message) { }
        public MissingDllException(string message, System.Exception inner) : base(message, inner) { }
        protected MissingDllException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}