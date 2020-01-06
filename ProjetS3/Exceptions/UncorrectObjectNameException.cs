using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class UncorrectObjectNameException : Exception
    {
        public UncorrectObjectNameException()
        {
            Console.WriteLine("This object doesnt exist");
        }
    }
}
