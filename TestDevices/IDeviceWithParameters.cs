using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevices
{
    interface IDeviceWithParameters : IDevice
    {
        void MethodWithParameters(string aString);
    }
}
