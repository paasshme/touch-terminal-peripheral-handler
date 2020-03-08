using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace ProjetS3.PeripheralCreation
{
    public class ConfigReader : IConfigReader
    {
        private XmlDocument xmldoc;
        public ConfigReader(string path)
        {
            try
            {
                string xmlFile = File.ReadAllText(path);
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
            XmlNodeList nodeList = xmldoc.GetElementsByTagName("library");
            ArrayList strList = new ArrayList();
            foreach (XmlNode xmlNode in nodeList)
                strList.Add(xmlNode.Attributes.GetNamedItem("path").Value);

            return strList;
        }

        //only object name
        public ArrayList GetAllInstancesFromOneDll(string libName)
        {
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName("library");
            ArrayList instances = new ArrayList();
            foreach (XmlNode nodes in dllNodes)
            {

                //Find the good lib in all libraries
                if (nodes.Attributes["path"].Value == libName)
                {
                    foreach (XmlNode node in nodes.ChildNodes)
                    {

                        System.Diagnostics.Debug.WriteLine("Name : " + node.Attributes["name"].Value);
                        instances.Add(node.Attributes["name"].Value);
                    }
                }
            }
            if (instances.Count == 0) throw new MissingDllException();
            return instances;
        }

        public Object[] GetParametersForOneInsance(String libName, String instanceName)
        {
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName("library");
            ArrayList instances = new ArrayList();
            foreach (XmlNode library in dllNodes)
            {

                //Find the good lib in all libraries
                if (library.Attributes["path"].Value == libName)
                {
                    foreach (XmlNode instance in library)
                    {
                        //Find the good instance in all instance
                        if (instance.Attributes["name"].Value == instanceName)
                        {
                            XmlNodeList parameters = instance.ChildNodes;
                            int nbParams = parameters.Count;
                            Object[] paramObjects = new object[nbParams];
                            for (int i = 0; i < nbParams; i++)
                            {
                                String paramType = parameters.Item(i).Attributes["type"].Value;
                                String paramValue = parameters.Item(i).InnerText;
                                switch (paramType)
                                {
                                    case "string":
                                        paramObjects[i] = paramValue;
                                        break;
                                    case "int":
                                        paramObjects[i] = int.Parse(paramValue);
                                        break;
                                    case "boolean":
                                        if (paramValue == "True")
                                        {
                                            paramObjects[i] = true;
                                        }
                                        else
                                        {
                                            paramObjects[i] = false;
                                        }
                                        break;
                                }
                            }
                            return paramObjects;
                        }
                    }
                }
            }
            return null;
        }
    }
}