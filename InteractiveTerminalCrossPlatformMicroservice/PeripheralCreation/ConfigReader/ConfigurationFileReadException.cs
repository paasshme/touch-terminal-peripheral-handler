using System;
namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /// <summary>
    /// Exception thrown when the configuration file can't be read (IO issues, incorrect path, ...)
    /// </summary>
    public class ConfigurationFileReadException : Exception
    {
        public ConfigurationFileReadException() { }
        public ConfigurationFileReadException(string message)
        : base(message){}
    
     public ConfigurationFileReadException(string message, Exception inner)
        : base(message, inner){}
    }
}