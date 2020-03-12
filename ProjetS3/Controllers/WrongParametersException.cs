using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class WrongParametersException : Exception
    {
        public WrongParametersException()
        {
            Console.WriteLine("Wrong Parameters for this method !");
        }
    }
}
