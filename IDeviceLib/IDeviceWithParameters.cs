using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDeviceLib
{
   public  interface IDeviceWithParameters : IDevice
    {
        void MethodWithParameters(string aString);
    }
}
