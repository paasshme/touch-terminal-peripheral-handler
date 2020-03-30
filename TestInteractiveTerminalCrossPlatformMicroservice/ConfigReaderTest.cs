using System;
using Xunit;
using System.Collections;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader;

namespace TestInteractiveTerminalCrossPlatformMicroservice
{

    public class ConfigReaderTest
    {
        String PROJECT_ROOT_PATH = "C:/Users/adres/source/repos/ProjetS4/";
        String DLL_FOLDER_PATH = "../PeripheralLibraries/";
        String configFilePath = "../../../configFileTest.xml";

        [Fact]
        public void GetAllDllNameTest()
        {
            XMLConfigReader reader = new XMLConfigReader(configFilePath);
            ArrayList dl = reader.GetAllDllName();
            int numberOfDll = 1;
            Assert.Equal(numberOfDll, dl.Count);

           string dllName = DLL_FOLDER_PATH + "TestDevices";
           Assert.True(dl.Contains(dllName));
        }

        [Fact]
        public void GetAllInstanceFromOneDllTest()
        {
            XMLConfigReader reader = new XMLConfigReader(configFilePath);
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
            XMLConfigReader reader = new XMLConfigReader(configFilePath);
            string dllName =(DLL_FOLDER_PATH + "TestDevices");
            string instanceName = "RandomDeviceWithParameters";
            Object[] resParams = reader.GetParametersForOneInstance(dllName, instanceName);

            Assert.NotNull(resParams);

            String resString = (String) resParams[0];
            int resInt = (int)resParams[1];
            bool resBool = (bool)resParams[2];

            String expectedString = "stringTest";
            int expectedInt = 7357;
            bool expectedBool = true;

            Assert.Equal(expectedString, resString);
            Assert.Equal(expectedInt, resInt);
            Assert.Equal(expectedBool, resBool);
        }
    }
}
