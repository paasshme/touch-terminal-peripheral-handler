using ProjetS3.PeripheralCreation;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using IDeviceLib;

namespace ProjetS4Test
{
    public class FactoryTest
    {
        [Fact]
        public void initTest()
        {
            PeripheralFactory.CONFIGURATION_FILE_PATH= "C:/Users/adres/source/repos/ProjetS4/ProjetS4Test/configFileTest.xml";
            PeripheralFactory.Init();

            IDevice dev = PeripheralFactory.GetInstance("RandomDevice");
        }
    }
}