using IDeviceLib;
using System;

namespace TestDevice.Interfaces
{
    public interface IScanner : IDevice
    {

        void Scan();

        void Foo();
    }
}