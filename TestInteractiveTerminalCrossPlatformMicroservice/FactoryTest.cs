using System;
using System.Collections.Generic;
using Xunit;
using System.Reflection;
using System.Linq;
using TestDevices;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation;
using InteractiveTerminalCrossPlatformMicroservice;
using PeripheralTools;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation.ConfigReader;

namespace TestInteractiveTerminalCrossPlatformMicroservice
{
    public class FactoryTest
    {
        private readonly XMLConfigReader reader;

        public FactoryTest()
        {
            InitFactoryForTest();
            this.reader = new XMLConfigReader(ConfigReaderTest.configFilePath);
        }

        [Fact]
        public void InstanceNumber()
        {
            int counter = 0;
            foreach (string libraryName in reader.GetAllDllName())
            {
                foreach (string instanceName in reader.GetAllInstancesFromOneDll(libraryName))
                {
                    ++counter;
                }
            }
            int nbInstance = PeripheralFactory.GetAllInstanceNames().Count;
            Assert.Equal(counter, nbInstance);
        }

        [Fact]
        public void CreateObject()
        {
            foreach (string libraryName in reader.GetAllDllName())
            {
                foreach (string instanceName in reader.GetAllInstancesFromOneDll(libraryName))
                {
                    Assert.NotNull(PeripheralFactory.GetInstance(instanceName));
                }
            }
            Assert.Throws<InexistantObjectException>(() => PeripheralFactory.GetInstance("DeviceWithAnErrorInHisName"));
        }

        [Fact]
        public void FindMethodsTest()
        {
            IDevice device = PeripheralFactory.GetInstance("RandomDevice");
            int nbMethodRandomDevice = 4;

            HashSet<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());
     

            Assert.All(methodList, x => Assert.NotNull(x));

            Assert.Equal(nbMethodRandomDevice, methodList.Count);
            Assert.All(methodList, x => Assert.NotNull(x));

            Assert.Collection(methodList,
                x => Assert.Equal("PeripheralTools.IPeripheralEventHandler get_eventHandler()", x.ToString()),
                x => Assert.Equal("Void set_eventHandler(PeripheralTools.IPeripheralEventHandler)", x.ToString()),
                x => Assert.Equal("Void Start()", x.ToString()),
                x => Assert.Equal("Void Stop()", x.ToString()));


            device = PeripheralFactory.GetInstance("DeviceWithMethodParameter");
            methodList = PeripheralFactory.FindMethods(device.GetType());
            nbMethodRandomDevice = 6;
            Assert.Equal(nbMethodRandomDevice, methodList.Count);
            Assert.All(methodList, x => Assert.NotNull(x));

            List<string> list = methodList.Select(s => s.ToString()).ToList();

            Assert.Contains("Void MethodWithParameters(System.String)", list);
            Assert.Contains("Void MethodWithTwoParameters(System.String, System.String)", list);
        }

        [Fact]
        public void HasConstructorParametersWellSet()
        {
            RandomDeviceWithParameters dev =(RandomDeviceWithParameters) PeripheralFactory.GetInstance("RandomDeviceWithParameters");
            Assert.Equal("stringTest", dev.stringTest);
            Assert.Equal(7357, dev.intTest);
            Assert.True(dev.boolTest);
        }


        [Fact]
        public void HasEventHandlerTest()
        {
            Assert.All(PeripheralFactory.GetAllInstanceNames(), 
                x => Assert.NotNull(PeripheralFactory.GetInstance(x).eventHandler));
        }

        [Fact]
        public void TestEveryInstanceName()
        {
            Assert.All(PeripheralFactory.GetAllInstanceNames(), x => Assert.NotNull(x));
            Assert.Collection(PeripheralFactory.GetAllInstanceNames(), item => Assert.Equal("RandomDevice",item.ToString()),
                                                                       item => Assert.Equal("RandomDeviceWithParameters", item.ToString()),
                                                                       item => Assert.Equal("DeviceWithMethodParameter", item.ToString()));
            foreach (string libraryName in reader.GetAllDllName())
            {
                foreach (string instanceName in reader.GetAllInstancesFromOneDll(libraryName))
                {
                    Assert.Contains<string>(instanceName,PeripheralFactory.GetAllInstanceNames());
                }
            }
        }

        //Init factory for test (with the specified config file)
        private void InitFactoryForTest()
        {
            PeripheralFactory.CONFIGURATION_FILE_PATH = "../../../configFileTest.xml";
            PeripheralFactory.Init();
        }
    }
}