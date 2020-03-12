using System;
namespace ProjetS3
{
    public class ConfigurationFileReadException : System.Exception
    {
        public ConfigurationFileReadException() { }
        public ConfigurationFileReadException(string message)
        : base(message){}
    
     public ConfigurationFileReadException(string message, Exception inner)
        : base(message, inner){}
    }
}