using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation;
using PeripheralTools;

namespace InteractiveTerminalCrossPlatformMicroservice.SwaggerCustom
{
    class CustomInstrospectionFilter : IDocumentFilter
    {
        // Unique ID used to distinguish Swagger Operations

        private const string UID = "19fa61d75522a4669b44e39c1d2e1726c530232130d407"; 

        // Path used in the API
        private const string API_START_PATH = "/api/";
        private const string API_END_PATH= "/";

        private const string DEFAULT_API_TAG_NAME = "BrowserRequests";
        private const string SUCCESS_DESCRIPTION = "Success";
        private const string FAILURE_DESCRIPTION = "Failure";

        // Possible HTTP codes
        private const string SUCCESS_HTTP_CODE = "200";
        private const string FAILURE_HTTP_CODE = "400";

        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            //Removing path that are generated automatically since it's irrelevant
            swaggerDoc.Paths.Clear();


            List<MethodData> allMethodInfos = generateAllMethodInfos();

            int counter = 0;

            //Tag Handling
            OpenApiTag tag = new OpenApiTag { Name = DEFAULT_API_TAG_NAME};
            List<OpenApiTag> tagList = new List<OpenApiTag>();
            tagList.Add(tag);
            

            //Response handling

            //Creating the responses indidually
            OpenApiResponse positiveAnswer = new OpenApiResponse { Description = SUCCESS_DESCRIPTION};
            OpenApiResponse negativeAnswer = new OpenApiResponse { Description = FAILURE_DESCRIPTION};

            //Generating the list of answers
            OpenApiResponses allPossibleAnswers = new OpenApiResponses();
            allPossibleAnswers.Add(SUCCESS_HTTP_CODE,positiveAnswer);
            allPossibleAnswers.Add(FAILURE_HTTP_CODE, negativeAnswer);


            foreach (MethodData methodInfo in allMethodInfos)
            {
                Dictionary<OperationType,OpenApiOperation> operationDictionnary = new Dictionary<OperationType,OpenApiOperation>();
                
                OpenApiOperation currentOperation = new OpenApiOperation {OperationId = UID+counter, Tags = tagList, Responses = allPossibleAnswers, Parameters=methodInfo.parameters};
                ++counter;

                operationDictionnary.Add(OperationType.Get, currentOperation);
                swaggerDoc.Paths.Add(methodInfo.route, new OpenApiPathItem{Operations = operationDictionnary});
            }
        }

        // Generate every possible path since swagger can't do it dynamically
        //Getting every instances of the peripherals -> then finding every methods
        private List<MethodData> generateAllMethodInfos()
        {
            List<MethodData> methodInfosList = new List<MethodData>();
            IList<string> peripheralNames = PeripheralFactory.GetAllInstanceNames();

            foreach(string currentPeripheralInstanceName in peripheralNames)
            {

                IDevice currentPeripheralInstance = PeripheralFactory.GetInstance(currentPeripheralInstanceName);

                List<MethodInfo> methodList = PeripheralFactory.FindMethods(currentPeripheralInstance.GetType());

                foreach(MethodInfo currentMethod in methodList)
                {
                    if (currentMethod.Name.StartsWith("get_") || currentMethod.Name.StartsWith("set_"))
                    {
                        continue;
                    }
                    string currentRoute = API_START_PATH + currentPeripheralInstanceName + API_END_PATH;
                    currentRoute += currentMethod.Name;
                    

                    ParameterInfo[] currentMethodParameters = currentMethod.GetParameters();
                    List<OpenApiParameter> parametersList = new List<OpenApiParameter>();
                    foreach(ParameterInfo currentParameterInfo in currentMethodParameters)
                    {
                        OpenApiParameter currentParameter = new OpenApiParameter { Name = currentParameterInfo.Name, Description = ""+currentParameterInfo.ParameterType, In = ParameterLocation.Query };
                        parametersList.Add(currentParameter);
                    }

                    MethodData currentMethodData = new MethodData(currentRoute, parametersList);
                    methodInfosList.Add(currentMethodData);
                    
                }
            }
            return methodInfosList;
        }
    }
}
