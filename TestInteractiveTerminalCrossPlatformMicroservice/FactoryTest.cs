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
        private XMLConfigReader reader;



        public FactoryTest()
        {
            InitFactoryForTest();
            this.reader = new XMLConfigReader(ConfigReaderTest.configFilePath);
        }


        [Fact]
        public void InitTest()
        {
            //this.reade
            int expectedInstanceNumber = 3; //Number of instance in the fake config file
            int nbInstance = PeripheralFactory.GetAllInstanceNames().Count;

            Console.WriteLine(nbInstance);
            Assert.Equal(expectedInstanceNumber, nbInstance);
        }

        [Fact]
        public void CreateObject()
        {
            Assert.NotNull(PeripheralFactory.GetInstance("RandomDevice"));
            Assert.NotNull(PeripheralFactory.GetInstance("RandomDeviceWithParameters"));
            Assert.NotNull(PeripheralFactory.GetInstance("DeviceWithMethodParameter"));
            Assert.Throws<InexistantObjectException>(() => PeripheralFactory.GetInstance("DeviceWithAnErrorInHisName"));
        }

        [Fact]
        public void FindMethodsTest()
        {
            IDevice device = PeripheralFactory.GetInstance("RandomDevice");
            List<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());
            int nbMethodRandomDevice = 4;
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

            Assert.Contains<string>("Void MethodWithParameters(System.String)", list);
            Assert.Contains<string>("Void MethodWithTwoParameters(System.String, System.String)", list);
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
            Assert.NotNull(PeripheralFactory.GetInstance("RandomDevice").eventHandler);
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
        }

        //Init factory for test (with the specified config file)
        private void InitFactoryForTest()
        {
            PeripheralFactory.CONFIGURATION_FILE_PATH = "../../../configFileTest.xml";
            PeripheralFactory.Init();
        }
    }
}