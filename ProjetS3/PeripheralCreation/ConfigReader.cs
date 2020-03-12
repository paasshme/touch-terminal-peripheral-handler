using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace ProjetS3.PeripheralCreation
{
    public class ConfigReader : IConfigReader
    {
        private const string LIBRARY_NODE = "library";

        private const string PATH_TO_LIBRARY = "path";

        private const string INSTANCE_NAME = "name";

        private const string INSTANCE_ATTRIBUTE_TYPE = "type";

        private XmlDocument xmldoc;

        public ConfigReader(string pathToXmlFile)
        {
            try
            {
                string xmlFile = File.ReadAllText(pathToXmlFile);
                this.xmldoc = new XmlDocument();
                this.xmldoc.LoadXml(xmlFile);
            }
            catch (Exception e)
            {
                throw new ConfigurationFileReadException("Can't read configuration file", e);

            }
        }

        //dll name: example (without the .dll) but with the path
        public ArrayList GetAllDllName()
        {
            XmlNodeList nodeList = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList listOfAllDllName = new ArrayList();
            foreach (XmlNode xmlNode in nodeList)
                listOfAllDllName.Add(xmlNode.Attributes.GetNamedItem(PATH_TO_LIBRARY).Value);

            return listOfAllDllName;
        }

        //only object name
        public ArrayList GetAllInstancesFromOneDll(string libName)
        {
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList instances = new ArrayList();
            foreach (XmlNode nodes in dllNodes)
            {

                //Find the good lib in all libraries
                if (nodes.Attributes[PATH_TO_LIBRARY].Value == libName)
                {
                    foreach (XmlNode node in nodes.ChildNodes)
                    {
                        instances.Add(node.Attributes[INSTANCE_NAME].Value);
                    }
                }
            }
            if (instances.Count == 0) throw new MissingDllException();

            return instances;
        }

        public Object[] GetParametersForOneInsance(String libName, String instanceName)
        {
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName(LIBRARY_NODE);
            ArrayList instances = new ArrayList();
            foreach (XmlNode library in dllNodes)
            {

                //Find the good lib in all libraries
                if (library.Attributes[PATH_TO_LIBRARY].Value == libName)
                {
                    foreach (XmlNode instance in library)
                    {
                        //Find the good instance in all instance
                        if (instance.Attributes[INSTANCE_NAME].Value == instanceName)
                        {
                            XmlNodeList parametersNodeList = instance.ChildNodes;
                            int nbParams = parametersNodeList.Count;
                            Object[] parameters= new object[nbParams];

                            for (int parameterIndex = 0; parameterIndex < nbParams; ++parameterIndex)
                            {
                                String paramType = parametersNodeList.Item(parameterIndex).Attributes[INSTANCE_ATTRIBUTE_TYPE].Value;
                                String paramValue = parametersNodeList.Item(parameterIndex).InnerText;

                                switch (paramType)
                                {
                                    case "string":
                                        parameters[parameterIndex] = paramValue;
                                        break;

                                    case "int":
                                        parameters[parameterIndex] = int.Parse(paramValue);
                                        break;

                                    case "bool":
                                        if (paramValue == "True")
                                        {
                                            parameters[parameterIndex] = true;
                                        }
                                        else
                                        {
                                            parameters[parameterIndex] = false;
                                        }
                                        break;
                                    case "float":
                                        parameters[parameterIndex] = float.Parse(paramValue);
                                        break;
                                    case "double":
                                        parameters[parameterIndex] = double.Parse(paramValue);
                                        break;
                                    case "short":
                                        parameters[parameterIndex] = short.Parse(paramValue);
                                        break;
                                    case "long":
                                        parameters[parameterIndex] = long.Parse(paramValue);
                                        break;
                                    case "char":
                                        parameters[parameterIndex] = char.Parse(paramValue);
                                        break;
                                    default:
                                        throw new TypeNotImplementedException(paramType);

                                }
                            }
                            return parameters;
                        }
                    }
                }
            }
            return null;
        }
    }
}