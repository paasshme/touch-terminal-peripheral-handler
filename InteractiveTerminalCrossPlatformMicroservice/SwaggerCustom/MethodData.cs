using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace InteractiveTerminalCrossPlatformMicroservice.SwaggerCustom
{
    /// <summary>
    /// Represent every data contained in a method
    /// Thus, a route(Http) and a list of OpenApiParameter
    /// </summary>
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
