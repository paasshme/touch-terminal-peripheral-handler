using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public interface IFactory
    {
        public string faitMagie(string ObjectName, string MethodName, object[] parameters);
    }
}
