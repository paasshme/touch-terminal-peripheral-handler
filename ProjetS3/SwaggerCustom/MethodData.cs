using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InteractiveTerminalCrossPlatformMicroservice.SwaggerCustom
{
    public class MethodData
    {
        public string route { get; set; }

        public List<OpenApiParameter> parameters { get; set; }

        public MethodData(string route, List<OpenApiParameter> parametersList)
        {
            this.route = route;
            this.parameters = parametersList;
        }
    }
}
