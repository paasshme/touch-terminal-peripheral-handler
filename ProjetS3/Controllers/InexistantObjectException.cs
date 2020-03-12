using System;
namespace ProjetS3
{
    public class InexistantObjectException: System.Exception
    {
        public InexistantObjectException() { }
        public InexistantObjectException(string message)
        : base(message){}
        public InexistantObjectException(string message, Exception inner)
        : base(message, inner){}
    }
}