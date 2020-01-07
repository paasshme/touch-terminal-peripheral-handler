using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public interface IEventHandler
    {
        public void send(string objectName, string eventName, string value);
    }
}
