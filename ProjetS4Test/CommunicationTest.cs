
using System;
using Xunit;
using ProjetS3;
using ProjetS3.PeripheralCreation;
using IDeviceLib;
using System.Reflection;
using System.Collections.Generic;
using ProjetS3.PeripheralRequestHandler;

namespace ProjetS4Test
{
    public class CommunicationTest
    {
        public static StatusObject status = new StatusObject();
     
        [Fact]
        public void TestWebsocket()
        {
           
            PeripheralFactory.Init();
            EventHandlerTestProxy proxy = EventHandlerTestProxy.GetInstance();
            PeripheralFactory.SetHandler(proxy);
           IDevice randomDevice =  PeripheralFactory.GetInstance("RandomDevice");
            List<MethodInfo> methodList = PeripheralFactory.FindMethods(randomDevice.GetType());
            MethodInfo correctMethodName = null;

            foreach (MethodInfo method in methodList)
            {
                if (method.Name.Equals("Start"))
                {
                    correctMethodName = method;
                }
            }
            correctMethodName.Invoke(randomDevice, new object[0]);

            //PeripheralTestEventHandler => event is send
            Assert.True(status.eventHandlerStatus);
            //EventHandlerTestProxy => event has been queued by the proxy
            Assert.True(status.proxyStatus);
            //SocketTestHandler => event is sent to webSocket
            Assert.True(status.socketHandlerStatus);
        }
    }
}
