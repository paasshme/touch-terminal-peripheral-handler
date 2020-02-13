using System;
using System.Collections.Generic;
using System.Text;

namespace IDeviceLib
{
    public interface ICalculator : IDevice
    {
        void add(int a, int b);
    }
}
