using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader
{
    /// <summary>
    ///An object that can read an XML configuration files (see convention in documentation) 
    /// </summary>
    public class XMLConfigReader : IConfigReader
    {
        // Conventions used in the Config.xml file 

        /// <summary>
        /// Node for the library in the xml file
        /// </summary>
        private const string LIBRARY_NODE = "library";


        /// <summary>
        /// Node for the library path in the xml file
        /// </summary>
        private const string PATH_TO_LIBRARY = "path";


        /// <summary>
        /// Node for the name of the instance in the xml file
        /// </summary>
        private const string INSTANCE_NAME = "name";

        /// <summary>
        /// Node for the type of the instance in the xml file
        /// </summary>
        private const string INSTANCE_ATTRIBUTE_TYPE = "type";

        /// <summary>
        /// The used Config.xml file
        /// </summary>
        private XmlDocument xmldoc;

        
        /// <summary>
        /// Constructor for the xml configuration reader
        /// </summary>
        /// <param name="pathToXmlFile"> pathToXmlFile the path to the XML file(relative to this file) </param>
        public XMLConfigReader(string pathToXmlFile)
        {
            try
            {
                string xmlFile = File.ReadAllText(pathToXmlFile);
                this.xmldoc = new XmlDocument();
                //Loading the content in a variable so as to have less IO connections that are very time-expensive
                this.xmldoc.LoadXml(xmlFile);
            }
            // Catching Exception since their handling doesn't change regardless of the type of the exception
            catch (Exception e)
            {
                throw new ConfigurationFileReadException("Can't read configuration file", e);
            }
            
        }


        /// <summary>
        /// Reads the config file and get all the libraries in the XML file 
        /// </summary>
        /// <returns>An array list that contains the path of every .dll file in the configuration file (without the .dll)</returns>
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

        /// <summary>
        ///Get all the peripheral types from a library in the XML config file 
        /// </summary>
        /// <param name="libName">libName Name of the library that will be searched in the config file (without the '.dll')</param>
        /// <returns>An array list that contains the name of every peripheral in this library </returns>
       
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


        /// <summary>
        ///Getting all the constructor parameters values from a peripheral instance 
        /// </summary>
        /// <param name="libName"> Name of the library that will be searched in the config file (without the .dll) </param>
        /// <param name="instanceName"> name of the peripheral instance (node instance, attribute name in the XML) </param>
        /// <returns> An object array that contains the values of each constructor parameter </returns>
        public object[] GetParametersForOneInstance(string libName, string instanceName)
        {
            //Geting all the library paths
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList instances = new ArrayList();

            foreach (XmlNode library in dllNodes)
            {
                //Find the good library in every libraries
                if (library.Attributes[PATH_TO_LIBRARY].Value == libName)
                {
                    foreach (XmlNode instance in library)
                    {
                        //Find the good instance in every instance
                        if (instance.Attributes[INSTANCE_NAME].Value == instanceName)
                        {
                            //Getting all the parameters
                            XmlNodeList parametersNodeList = instance.ChildNodes;
                            //Getting the number of parameters
                            int nbParams = parametersNodeList.Count;
                            object[] parameters= new object[nbParams];

                            
                            for (int parameterIndex = 0; parameterIndex < nbParams; ++parameterIndex)
                            {
                                //Getyting the type of the curretn parameter
                                string paramType = parametersNodeList.Item(parameterIndex).Attributes[INSTANCE_ATTRIBUTE_TYPE].Value;
                                //Getting the value of the current parameter
                                string paramValue = parametersNodeList.Item(parameterIndex).InnerText;

                                //Applying a different treatement according to the type of the current parameter
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