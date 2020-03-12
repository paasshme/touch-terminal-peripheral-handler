using ProjetS3.PeripheralCreation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using IDeviceLib;
using System.Reflection;

namespace ProjetS4Test
{
    public class FactoryTest
    {
        [Fact]
        public void initTest()
        {
            PeripheralFactory.CONFIGURATION_FILE_PATH= "C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/configFileTest.xml";
            PeripheralFactory.Init();
            int expectedInstanceNumber = 3;
            int nbInstance = PeripheralFactory.GetAllInstanceNames().Count;
            IDevice dev = PeripheralFactory.GetInstance("RandomDevice");

            Assert.Equal(expectedInstanceNumber, nbInstance);

        }

        [Fact]
        public void findMethodsTest()
        {
            IDevice dev = PeripheralFactory.GetInstance("RandomDevice");
            List<MethodInfo> methodList = PeripheralFactory.FindMethods(dev.GetType());
           
            int nbMethodRandomDevice = 4;
            Assert.Equal(nbMethodRandomDevice, methodList.Count);
        }
            

        [Fact]
        public void hasEventHandlerTest()
        {
            IDevice dev = PeripheralFactory.GetInstance("RandomDevice");
            Assert.NotNull(dev.eventHandler);
        }
    }
}