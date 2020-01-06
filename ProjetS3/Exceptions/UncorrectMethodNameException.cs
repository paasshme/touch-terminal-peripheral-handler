using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class UncorrectMethodNameException : Exception
    {
        public UncorrectMethodNameException()
        {
            Console.WriteLine("This method doesnt exist");
        }
    }
}
