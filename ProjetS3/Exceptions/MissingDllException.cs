namespace ProjetS3
{
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