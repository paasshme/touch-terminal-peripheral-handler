using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace ProjetS3
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
                strList.Add(xmlNode.Attributes.GetNamedItem("name").Value);

            return strList;
        }

        //only object name
        public ArrayList GetAllInstancesFromOneDll(string libName)
        {
            XmlNodeList dllNodes = xmldoc.GetElementsByTagName("library");
            ArrayList instances = new ArrayList();
            foreach (XmlNode nodes in dllNodes)
            {
                if (nodes.Attributes["name"].Value == libName)
                {
                    foreach (XmlNode node in nodes.ChildNodes)
                    {
                        instances.Add(node.InnerText);
                    }
                }
            }
            if (instances.Count == 0) throw new MissingDllException();
            return instances;
        }
    }
}
