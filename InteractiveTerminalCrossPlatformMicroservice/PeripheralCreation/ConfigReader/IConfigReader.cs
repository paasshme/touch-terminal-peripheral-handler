using System.Collections;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /*
     *  Represents an object that can read a configuration file. It can :
     *      - Get all the libraries(.dll) in the config file 
     *      - Get all the peripheral types from a library in the config file
     */
    interface IConfigReader
    {
        /*
         * Reads the config file and get all the libraries in the file
         * @return An array list that contains the path of every .dll file in the configuration file (without the .dll)
         */
        ArrayList GetAllDllName();

        /*
         * Get all the peripheral types from a library in the config file
         * @param libName Name of the library that will be searched -> must be in the config file
         * @return  An array list that contains the name of every peripheral in this library 
         */
        ArrayList GetAllInstancesFromOneDll(string libName);

        /**
         * Get all the parameter (as an object array) of an object in its library
         * @param libName Name of the library that will be searched -> must be in the config file
         * @param instanceName Name of the instance that will be searched -> must be in the config file
         * @return  An array list that contains the name of every peripheral in this library 
         */
        object[] GetParametersForOneInstance(string libName, string instanceName);
    }
}
