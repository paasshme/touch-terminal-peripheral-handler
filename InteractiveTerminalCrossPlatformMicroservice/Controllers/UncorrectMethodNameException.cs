using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveTerminalCrossPlatformMicroservice
{
    /// <summary>
    /// Exception thrown when the controller tries to use a unimplemented or inexistant method on a peripheral
    /// </summary>
    public class UncorrectMethodNameException : Exception
    {
        public UncorrectMethodNameException()
        {
            Console.WriteLine("This method doesnt exist");
        }
    }
}
