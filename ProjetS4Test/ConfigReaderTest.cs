using System;
using Xunit;
using ProjetS3.PeripheralCreation;
using System.Collections;

namespace ProjetS4Test
{
    public class ConfigReaderTest
    {
        [Fact]
        public void GetAllDllNameTest()
        {
            ConfigReader reader = new ConfigReader("../../../configFileTest.xml");
            ArrayList dl = reader.GetAllDllName();
            int numberOfDll = 1;
            Assert.Equal(numberOfDll, dl.Count);

            string dllName = "C:/Users/adres/source/repos/ProjetS4/PeripheralLibraries/TestDevices";
            Assert.True(dl.Contains(dllName));
        }

        [Fact]
        public void GetAllInstanceFromOneDllTest()
        {
            ConfigReader reader = new ConfigReader("../../../configFileTest.xml");
            ArrayList list = reader.GetAllInstancesFromOneDll("C:/Users/adres/source/repos/ProjetS4/PeripheralLibraries/TestDevices");
            string res = "RandomDevice";
            Assert.True(list.Contains(res));

            Assert.Equal(1, list.Count);
        }
    }
}
