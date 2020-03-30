using System;
using Xunit;
using System.Collections;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader;

namespace TestInteractiveTerminalCrossPlatformMicroservice
{
    /**
     * 
     *  Test for the XML reader:
     *  it uses the configFileTest.xml
     */

    public class ConfigReaderTest
    {
        private string DLL_FOLDER_PATH = "../InteractiveTerminalCrossPlatformMicroservice/PeripheralLibraries/";
        public static string configFilePath = "../../../configFileTest.xml";
        private XMLConfigReader reader;

        public ConfigReaderTest()
        {
            this.reader = new XMLConfigReader(configFilePath);
        }

        [Fact]
        public void GetAllDllNameTest()
        {
            ArrayList dl = reader.GetAllDllName();
            int numberOfDll = 1;
            Assert.Equal(numberOfDll, dl.Count);

           string dllName = DLL_FOLDER_PATH + "TestDevices";
           Assert.True(dl.Contains(dllName));
        }

        [Fact]
        public void GetAllInstanceFromOneDllTest()
        {
            ArrayList list = reader.GetAllInstancesFromOneDll(DLL_FOLDER_PATH + "TestDevices");
            string res = "RandomDevice";
            string res2 = "RandomDeviceWithParameters";
            string res3 = "DeviceWithMethodParameter";
            int nbInstance = 3;

            Assert.True(list.Contains(res));
            Assert.True(list.Contains(res2));
            Assert.True(list.Contains(res3));

            Assert.Equal(nbInstance, list.Count);
        }
        [Fact]
        public void GetParametersForOneInstanceTest()
        {
            string dllName =(DLL_FOLDER_PATH + "TestDevices");
            string instanceName = "RandomDeviceWithParameters";
            Object[] resParams = reader.GetParametersForOneInstance(dllName, instanceName);

            Assert.NotNull(resParams);

            string resString = (string) resParams[0];
            int resInt = (int)resParams[1];
            bool resBool = (bool)resParams[2];

            string expectedString = "stringTest";
            int expectedInt = 7357;
            bool expectedBool = true;

            Assert.Equal(expectedString, resString);
            Assert.Equal(expectedInt, resInt);
            Assert.Equal(expectedBool, resBool);
        }
    }
}
