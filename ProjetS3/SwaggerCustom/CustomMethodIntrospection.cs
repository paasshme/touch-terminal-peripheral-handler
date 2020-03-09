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
using System;

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

            //Tag Handling
            OpenApiTag tag = new OpenApiTag { Name = "BrowserRequests"};
            List<OpenApiTag> tagList = new List<OpenApiTag>();
            tagList.Add(tag);

            //Response handling

            //Creating the responses indidually
            OpenApiResponse positiveAnswer = new OpenApiResponse { Description = "Succes"};
            OpenApiResponse negativeAnswer = new OpenApiResponse { Description = "Failure" };

            //Generating the list of answers
            OpenApiResponses allAnswerPossibles = new OpenApiResponses();
            allAnswerPossibles.Add("200",positiveAnswer);
            allAnswerPossibles.Add("400", negativeAnswer);


            foreach (string route in allRoutes)
            {
                Dictionary<OperationType,OpenApiOperation> dic = new Dictionary<OperationType,OpenApiOperation>();
                
                OpenApiOperation ope = new OpenApiOperation {OperationId = UID+counter, Tags=tagList,Responses=allAnswerPossibles};
                ++counter;
                dic.Add(OperationType.Get, ope);

                swaggerDoc.Paths.Add(route, new OpenApiPathItem{Operations = dic});
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


                    /** Tessting purpose -> will work when facto fixed (maybe)
                    ParameterInfo[] currentMethodParameters = currentMethod.GetParameters();
                    Console.WriteLine("Je passe ??????????????????????????????????????????????");
                    foreach(ParameterInfo pi in currentMethodParameters)
                    {
                        Console.WriteLine("SWAAGGGGGGGGGGGGGER");
                        Console.WriteLine("" + currentMethod.Name + " Param : " + pi);
                        Console.WriteLine("SWAGGGER MIEUX");

                    }
                    */
                }
            }
            return routesItemsList;
        }
    }
}
