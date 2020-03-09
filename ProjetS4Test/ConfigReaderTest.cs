using System;
using Xunit;
using ProjetS3.PeripheralCreation;
using System.Collections;

namespace ProjetS4Test
{
    public class ConfigReaderTest
    {
        String configFilePath = "C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/configFileTest.xml";
        [Fact]
        public void GetAllDllNameTest()
        {
            ConfigReader reader = new ConfigReader(configFilePath);
            ArrayList dl = reader.GetAllDllName();
            int numberOfDll = 1;
            Assert.Equal(numberOfDll, dl.Count);

           string dllName = "C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/bin/Debug/netcoreapp3.1/TestDevices";
           Assert.True(dl.Contains(dllName));
        }

        [Fact]
        public void GetAllInstanceFromOneDllTest()
        {
            ConfigReader reader = new ConfigReader(configFilePath);
            ArrayList list = reader.GetAllInstancesFromOneDll("C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/bin/Debug/netcoreapp3.1/TestDevices");
            string res = "RandomDevice";
            string res2 = "RandomDeviceWithParameters";

            Assert.True(list.Contains(res));
            Assert.True(list.Contains(res2));

            Assert.Equal(2, list.Count);
        }
        [Fact]
        public void GetParametersForOneInstanceTest()
        {
            ConfigReader reader = new ConfigReader(configFilePath);
            string dllName =("C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/bin/Debug/netcoreapp3.1/TestDevices");
            string instanceName = "RandomDeviceWithParameters";
            Object[] resParams = reader.GetParametersForOneInsance(dllName, instanceName);

            Assert.NotNull(resParams);

            String resString = (String) resParams[0];
            int resInt = (int)resParams[1];
            bool resBool = (bool)resParams[2];

            String expectedString = "StringTest";
            int expectedInt = 7357;
            bool expectedBool = true;

            Assert.Equal(expectedString, resString);
            Assert.Equal(expectedInt, resInt);
            Assert.Equal(expectedBool, resBool);
        }
    }
}
