using System;
using Xunit;

namespace ProjetS4Test
{
    public class UnitTest1
    {
        [Fact]
        public void TestConfigReader()
        {
            ProjetS3.ConfigReader reader = new ProjetS3.ConfigReader("testConfig.xml");

        }
    }
}
