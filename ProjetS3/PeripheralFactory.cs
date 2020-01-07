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
        string IFactory.faitMagie(string ObjectName, string MethodName, object[] parameters)
        {
            if (ObjectName.Equals("bonjour"))
            {
                throw new UncorrectObjectNameException();
            }
            if (MethodName.Equals("cava"))
            {
                throw new UncorrectMethodNameException();
            }
            if (parameters.Length > 0 && parameters[0].Equals("12"));
            {
                throw new WrongParametersException();
            }
            string str = "Objet :" + ObjectName + "\n Methode :" + MethodName + "\n";
            for (int i=0; i<parameters.Length; i++)
            {
                str = str + "Paramètre " + i + " : " + parameters[i];
            }
            return str;
        }
    }
}
