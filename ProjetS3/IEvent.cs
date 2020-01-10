using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public interface IEvent
    {
         string objectName { get; set; }

         string eventName { get; set; }

         object value { get; set; }
    }
}
