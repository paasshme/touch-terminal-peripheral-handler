using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ProjetS3.PeripheralCreation;
using IDeviceLib;

namespace ProjetS3.SwaggerCustom
{
    class CustomMethodIntrospection : IDocumentFilter
    {
        private const string UID = "aizjeiuazhneuiabzudbazlekzbzubnadkuz"; 
        private const string API_START_PATH = "/api/";
        private const string API_END_PATH= "/";

        private const string DEFAULT_API_TAG_NAME = "BrowserRequests";
        private const string SUCCESS_DESCRIPTION = "Success";
        private const string FAILURE_DESCRIPTION = "Failure";

        private const string SUCCESS_HTTP_CODE = "200";
        private const string FAILURE_HTTP_CODE = "400";

        void IDocumentFilter.Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {

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

        //Goal : generates all paths possible since swwagger can't do it dynamically
        //Getting all the instances of the peripherals -> then finding all the methods
        List<MethodData> generateAllMethodInfos()
        {
            List<MethodData> methodInfosList = new List<MethodData>();
            IList<string> peripheralNames = PeripheralFactory.GetAllInstanceNames();

            foreach(string currentPeripheralInstanceName in peripheralNames)
            {

                IDevice currentPeripheralInstance = PeripheralFactory.GetInstance(currentPeripheralInstanceName);

                List<MethodInfo> methodList = PeripheralFactory.FindMethods(currentPeripheralInstance.GetType());

                foreach(MethodInfo currentMethod in methodList)
                {
                    if (currentMethod.Name.StartsWith("get_") || currentMethod.Name.StartsWith("set_")) continue;
                    string currentRoute = API_START_PATH + currentPeripheralInstanceName + API_END_PATH;
                    currentRoute += currentMethod.Name;
                    

                    ParameterInfo[] currentMethodParameters = currentMethod.GetParameters();
                    List<OpenApiParameter> parametersList = new List<OpenApiParameter>();
                    foreach(ParameterInfo pi in currentMethodParameters)
                    {
                        OpenApiParameter currentParameter = new OpenApiParameter { Name = pi.Name, Description = ""+pi.ParameterType, In = ParameterLocation.Query };
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
