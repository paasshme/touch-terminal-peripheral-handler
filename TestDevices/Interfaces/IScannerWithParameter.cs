using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevice.Interfaces
{
    interface IScannerWithParameter : IScanner
    {
        void printTest(string parameterTest);
    }
}
