using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Few line of comments
namespace ProjetS3
{
    public class PeripheralFactory : IFactory
    {
        public PeripheralFactory()
        {

        }
        void IFactory.faitMagie(string ObjectName, string MethodName, object[] parameters)
        {
            return;
        }
    }
}
