using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice.Interfaces
{
    public interface ICalculator : IDevice
    {
        void add(int a, int b);
    }
}
