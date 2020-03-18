using System;
namespace ProjetS3
{
    /*
     * Exception thrown when the configuration file can't be read (IO problems, incorrect path, ...)
     */
    public class ConfigurationFileReadException : System.Exception
    {
        public ConfigurationFileReadException() { }
        public ConfigurationFileReadException(string message)
        : base(message){}
    
     public ConfigurationFileReadException(string message, Exception inner)
        : base(message, inner){}
    }
}