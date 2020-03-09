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
        private const string UID = "aizjeiuazhneuiabzudbazlekzbzubnadkuz"; 
        private const string API_START_PATH = "/api/";
        private const string API_END_PATH= "/";

        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

            List<string> allRoutes = generateAllRoutes();
            int counter = 0;
            foreach(string route in allRoutes)
            {
                Dictionary<OperationType,OpenApiOperation> dic = new Dictionary<OperationType,OpenApiOperation>();
                
                OpenApiOperation ope = new OpenApiOperation {OperationId = UID+counter};
                ++counter;
                dic.Add(OperationType.Get, ope);

                swaggerDoc.Paths.Add(route, new OpenApiPathItem{Operations = dic/*(OperationType.Get, new OpenApiOperation())*/});
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
                    string current = API_START_PATH + peripheralName + API_END_PATH;
                    current += currentMethod.Name;
                    routesItemsList.Add(current);
                }
            }
            return routesItemsList;
        }
    }
}
