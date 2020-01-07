using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3
{
    public interface IEvent
    {
        public string objectName { get; set; }

        public string eventName { get; set; }

        public object value { get; set; }
    }
}
