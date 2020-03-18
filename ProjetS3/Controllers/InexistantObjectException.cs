using System;
namespace ProjetS3
{
    /*
     * Exception thrown when the controller tries to use a peripheral that doesn't exists (not in the config file)
     */
    public class InexistantObjectException: System.Exception
    {
        public InexistantObjectException() { }
        public InexistantObjectException(string message)
        : base(message){}
        public InexistantObjectException(string message, Exception inner)
        : base(message, inner){}
    }
}