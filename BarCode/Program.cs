using System;
using IDeviceLib;
using System.IO.Ports;

namespace BarCode
{
    class Barcode : IDevice

    {
        public IPeripheralEventHandler eventHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        static void Main(string[] args)
        {
  
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
