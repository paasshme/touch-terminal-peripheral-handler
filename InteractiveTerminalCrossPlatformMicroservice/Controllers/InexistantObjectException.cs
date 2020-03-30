using System;
namespace InteractiveTerminalCrossPlatformMicroservice
{
    /// <summary>
    /// Exception thrown when the controller tries to use a peripheral that doesn't exists
    /// </summary>
    public class InexistantObjectException: System.Exception
    {
        public InexistantObjectException() { }
        public InexistantObjectException(string message)
        : base(message){}
        public InexistantObjectException(string message, Exception inner)
        : base(message, inner){}
    }
}