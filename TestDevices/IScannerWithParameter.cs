using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestDevices
{
    interface IScannerWithParameter : IScanner
    {
        void printTest(string parameterTest);
    }
}
