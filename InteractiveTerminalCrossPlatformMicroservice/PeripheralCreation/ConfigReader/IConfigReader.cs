using System.Collections;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /// <summary>
    /// Represents an object that can read a configuration file. It can :
    ///  - Get all the libraries(.dll) in the config file
    ///  - Get all the peripheral types from a library in the config file
    /// </summary>
    interface IConfigReader
    {

        /// <summary>
        /// Reads the config file and get all the libraries in the XML file 
        /// </summary>
        /// <returns>An array list that contains the path of every .dll file in the configuration file (without the .dll)</returns>
        ArrayList GetAllDllName();

        /// <summary>
        ///Get all the peripheral types from a library in the XML config file 
        /// </summary>
        /// <param name="libName">libName Name of the library that will be searched in the config file (without the '.dll')</param>
        /// <returns>An a

        ArrayList GetAllInstancesFromOneDll(string libName);


        /// <summary>
        ///Getting all the constructor parameters values from a peripheral instance 
        /// </summary>
        /// <param name="libName"> Name of the library that will be searched in the config file (without the .dll) </param>
        /// <param name="instanceName"> name of the peripheral instance (node instance, attribute name in the XML) </param>
        /// <returns> An object array that contains the values of each constructor parameter </returns>
        object[] GetParametersForOneInstance(string libName, string instanceName);
    }
}
