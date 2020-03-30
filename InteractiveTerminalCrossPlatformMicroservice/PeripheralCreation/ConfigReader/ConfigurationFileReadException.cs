using System;
namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /*
     * Exception thrown when the configuration file can't be read (IO issues, incorrect path, ...)
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