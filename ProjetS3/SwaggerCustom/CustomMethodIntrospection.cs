using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Reflection;
using IDeviceLib;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using ProjetS3.PeripheralCreation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProjetS3.SwaggerCustom
{
    class CustomMethodIntrospection : IDocumentFilter
    {
        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {


            System.Console.WriteLine("AAAAAAAAAAAAAAAAAa");
            System.Console.WriteLine(swaggerDoc.Paths);

            swaggerDoc.Tags = new List<OpenApiTag> {
                new OpenApiTag {Name = "BrowserRequest", Description = "ALED2"}
            };

                       List<string> allRoutes = generateAllRoutes();

            foreach(string route in allRoutes)
            {
                swaggerDoc.Paths.Add(route, new OpenApiPathItem());
            }

            foreach (var o in swaggerDoc.Paths)
            {
                System.Console.WriteLine(o);
            }

        }

                //Goal : generates all paths possible since swwagger can't do it dynamically
        //Getting all the instances of the peripherals -> then finding all the methods
        List<string> generateAllRoutes()
        {
            List<string> routesItemsList = new List<string>();
            IList<string> peripheralNames = PeripheralFactory.GetAllInstanceNames();

            foreach(string peripheralName in peripheralNames)
            {

                IDevice currentPeripheralInstance = PeripheralFactory.GetInstance(peripheralName);

                List<MethodInfo> methodList = PeripheralFactory.FindMethods(currentPeripheralInstance.GetType());

                foreach(MethodInfo currentMethod in methodList)
                {
                    if (currentMethod.Name.StartsWith("get_") || currentMethod.Name.StartsWith("set_")) continue;
                    string current = "/api/" + peripheralName + "/";
                    current += currentMethod.Name;
                    routesItemsList.Add(current);
                }
            }
            return routesItemsList;
        }
    }
}
