using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveTerminalCrossPlatformMicroservice
{
    /*
     * Exception thrown when the controller tries to use a unimplemented method on a peripheral
     */
    public class UncorrectMethodNameException : Exception
    {
        public UncorrectMethodNameException()
        {
            Console.WriteLine("This method doesnt exist");
        }
    }
}
