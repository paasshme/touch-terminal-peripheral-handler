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
            ConfigReader reader = new ConfigReader("configFileTest.xml");
            ArrayList dl = reader.GetAllDllName();
            int numberOfDll = 1;
            Assert.Equal(numberOfDll, dl.Count);

            string dllName = ".. / PeripheralLibraries / TestDevices";
            Assert.True(dl.Contains(dllName));
        }
    }
}
