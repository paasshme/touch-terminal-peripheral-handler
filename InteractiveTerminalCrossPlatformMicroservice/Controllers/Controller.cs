using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PeripheralTools;
using System.Reflection;
using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation;



namespace InteractiveTerminalCrossPlatformMicroservice.Controllers
{
    /// <summary>
    /// An ASP.CORE Controller which takes over the reception of the different HTTP requests.
    /// </summary>
    public class Controller : Microsoft.AspNetCore.Mvc.Controller
    {
        /// <summary>
        /// http code sent to the browser in case of success
        /// </summary>
        public const int HTTP_CODE_SUCCESS = 200;

        /// <summary>
        /// http code sent to the browser in case of failure
        /// </summary>
        public const int HTTP_CODE_FAILURE = 400;

        /// <summary>
        /// When browser sends a get request with the format : api/{ObjectName}/{Method}
        /// The controller tries to call the method on the peripheral instance and returns an adapted answer
        /// </summary>
        /// <param name="ObjectName"> Name of the type of the object on which the method will be call</param>
        /// <param name="Method"> Name of the method to invoke</param>
        /// <returns>A StatusCode code indicating whether the invocation is a success or not with a description</returns>
        [HttpGet]
        [Route("api/{ObjectName}/{Method}")]
        public IActionResult CommunicateToPeripheral(string ObjectName, string Method)
        {
            //Getting ?param1=x&param2=y... part of the URL
            var query = this.HttpContext.Request.QueryString;

            //No parameters in GET request
            if (string.IsNullOrEmpty(query.ToString()))
            {
                try
                {
                    //Calling the method without any parameter 
                    UseMethod(ObjectName, Method, new object[0]);
                }
                catch (InexistantObjectException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object " + ObjectName + " doesn't exists");
                }
                catch (UncorrectMethodNameException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object " + ObjectName + " doesn't implements the method " + Method);
                }
            }

            else
            {
                //Parameters string parsing
                char[] separators = {'=', '&'};
                int counter = 0;

                //Removing the ? from the param string
                string param = query.ToString().Substring(1);

                string[] parametersListWithSeparators = param.Split(separators);    

                object[] parametersArray = new object[parametersListWithSeparators.Length/2];

                //Filling the parameters array with the values only
                for (int i = 1; i < parametersListWithSeparators.Length; i+=2)
                {
                    parametersArray[counter++] = parametersListWithSeparators[i];
                }

                try
                {
                    UseMethod(ObjectName, Method, parametersArray);
                }
                catch (InexistantObjectException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object " + ObjectName +
                        " doesn't exists");
                }
                catch (UncorrectMethodNameException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object "+ObjectName + 
                        " doesn't implements the method " + Method);
                }
               
                catch (ArgumentException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The method " + Method + 
                        " is used with wrong parameters types!");
                }
                catch(TargetParameterCountException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The method " + Method + 
                        " isn't used with the good number of parameters!");
                }
                
            }
            return StatusCode(HTTP_CODE_SUCCESS, "Calling the method " + Method + " on " + ObjectName);

        }

        /// <summary>
        /// Method that calls the method on the right peripheral instance
        /// </summary>
        /// <param name="objectName"> Type of the peripheral</param>
        /// <param name="methodName"> Name of the method to call on the peripheral </param>
        /// <param name="methodParams"> Array that contains all the parameters value (can be empty) </param>
        private void UseMethod(string objectName, string methodName, object[] methodParams) 
        {           
            // Getting the peripheral instance based on its name 
            IDevice device = PeripheralFactory.GetInstance(objectName);
            
            // Getting all the methods of the device
            HashSet<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());

            MethodInfo correctMethodName = null;

            // Retreiving the methodInfo which correspond to the "methodName" parameter
            foreach (MethodInfo method in methodList)
            {
                if (method.Name.Equals(methodName))
                {
                    correctMethodName = method;
                }
            }

            // Handling a non existing methodName
            if (correctMethodName is null)
            {
                throw new UncorrectMethodNameException();
            }

            // Calling the method with the parameters
            correctMethodName.Invoke(device, methodParams);
        }

    }
}
 