using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /*
     * An object that can read an XML configuration files (see convention in documentation)
     */
    public class XMLConfigReader : IConfigReader
    {
        private const string LIBRARY_NODE = "library";

        private const string PATH_TO_LIBRARY = "path";

        private const string INSTANCE_NAME = "name";

        private const string INSTANCE_ATTRIBUTE_TYPE = "type";

        private XmlDocument xmldoc;

        /*
         * Build an XML configuration file reader
         * @param pathToXmlFile the path to the XML file (relative to this file)
         */
        public XMLConfigReader(string pathToXmlFile)
        {
            try
            {
                string xmlFile = File.ReadAllText(pathToXmlFile);
                this.xmldoc = new XmlDocument();
                //Loading the content in a variable so as to have less IO connections that are very time-expensive
                this.xmldoc.LoadXml(xmlFile);
            }
            //Catching Exception since there are a lot of ones to catch and handling doesn't change regardless of the type of the exception
            catch (Exception e)
            {
                throw new ConfigurationFileReadException("Can't read configuration file", e);

            }
            
        }

        /*
         * Reads the config file and get all the libraries in the XML file
         * @return An array list that contains the path of every .dll file in the configuration file (without the .dll)
         */
        public ArrayList GetAllDllName()
        {
            //Getting all the paths in xml
            XmlNodeList nodeList = xmldoc.GetElementsByTagName(LIBRARY_NODE);

            ArrayList listOfAllDllName = new ArrayList();

            foreach (XmlNode xmlNode in nodeList)
                //adding each path to the returned list
                listOfAllDllName.Add(xmlNode.Attributes.GetNamedItem(PATH_TO_LIBRARY).Value);

            return listOfAllDllName;
        }

        /*
         * Get all the peripheral types from a library in the XML config file
         * @param libName Name of the library that will be searched in the config file (without the .dll)
         * @return  An array list that contains the name of every peripheral in this library 
         */
        public ArrayList GetAllInstancesFromOneDll(string libName)
        {
            //Getting all the libray paths
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList instances = new ArrayList();

            //Trying to find the library in parameters among all libraries
            foreach (XmlNode nodes in dllNodes)
            {

                if (nodes.Attributes[PATH_TO_LIBRARY].Value == libName)
                {
                    foreach (XmlNode node in nodes.ChildNodes)
                    {
                        //For each peripheral instance node in the library, adding it to the returned list
                        instances.Add(node.Attributes[INSTANCE_NAME].Value);
                    }
                }
            }
            //Throwing an expcetion if the library wasn't found
            if (instances.Count == 0) throw new MissingDllException();

            return instances;
        }

        /*
         * Getting all the constructor parameters values from a peripheral instance
         * @param libName Name of the library that will be searched in the config file (without the .dll)
         * @param instanceName name of the pperipheral instance (node instance, attribute name in the XML)
         * @return An object array that contains the values of each constructor parameter
         * */
        public Object[] GetParametersForOneInsance(String libName, String instanceName)
        {
            //Geting all the library paths
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList instances = new ArrayList();
            foreach (XmlNode library in dllNodes)
            {

                //Find the good library in all libraries
                if (library.Attributes[PATH_TO_LIBRARY].Value == libName)
                {
                    foreach (XmlNode instance in library)
                    {
                        //Find the good instance in all instance
                        if (instance.Attributes[INSTANCE_NAME].Value == instanceName)
                        {
                            //Getting all the parameters
                            XmlNodeList parametersNodeList = instance.ChildNodes;
                            //Getting the number of parameters
                            int nbParams = parametersNodeList.Count;
                            Object[] parameters= new object[nbParams];

                            
                            for (int parameterIndex = 0; parameterIndex < nbParams; ++parameterIndex)
                            {
                                //Getyting the type of the curretn parameter
                                String paramType = parametersNodeList.Item(parameterIndex).Attributes[INSTANCE_ATTRIBUTE_TYPE].Value;
                                //Getting the value of the current parameter
                                String paramValue = parametersNodeList.Item(parameterIndex).InnerText;

                                //Applying a diffrenet treatement according to the type of the current parameter
                                switch (paramType)
                                {
                                    case "string":
                                        parameters[parameterIndex] = paramValue;
                                        break;

                                    case "int":
                                        //casting value to int
                                        parameters[parameterIndex] = int.Parse(paramValue);
                                        break;

                                    case "bool":
                                        //putting the right boolean value
                                        if (paramValue == "true")
                                        {
                                            parameters[parameterIndex] = true;
                                        }
                                        else
                                        {
                                            parameters[parameterIndex] = false;
                                        }
                                        break;
                                    case "float":
                                        //casting value to float
                                        parameters[parameterIndex] = float.Parse(paramValue);
                                        break;
                                    case "double":
                                        //casting value to double
                                        parameters[parameterIndex] = double.Parse(paramValue);
                                        break;
                                    case "short":
                                        //casting value to short
                                        parameters[parameterIndex] = short.Parse(paramValue);
                                        break;
                                    case "long":
                                        //casting value to long
                                        parameters[parameterIndex] = long.Parse(paramValue);
                                        break;
                                    case "char":
                                        //casting value to character
                                        parameters[parameterIndex] = char.Parse(paramValue);
                                        break;
                                    default:
                                        //If the type isn't primitive, throw an exception
                                        throw new TypeNotImplementedException(paramType);

                                }
                            }
                            return parameters;
                        }
                    }
                }
            }

            //If the library or the peripheral instance wasn't found, return null
            return null;
        }
    }
}