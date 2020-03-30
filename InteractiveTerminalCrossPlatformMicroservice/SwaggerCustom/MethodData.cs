using Microsoft.OpenApi.Models;
using System.Collections.Generic;

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
