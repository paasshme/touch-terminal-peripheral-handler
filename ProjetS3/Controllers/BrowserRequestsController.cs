using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using IDeviceLib;
using System.Reflection;
using ProjetS3.PeripheralCreation;



namespace ProjetS3.Controllers
{
    /*
     * Object which takes over the reception of the brower requests
     */
    public class BrowserRequestsController : Controller
    {

        public const int HTTP_CODE_SUCCESS = 200; 
        public const int HTTP_CODE_FAILURE = 400;

        //When browser sends a get request with the format : api/{ObjectName}/{Method}
        //The controller tries to call the method on the peripheral instance and returns an adapted answer
        [HttpGet]
        [Route("api/{ObjectName}/{Method}")]
        public IActionResult CommunicateToPeripheral(string ObjectName, string Method)
        {
            //Getting ?param1=x&param2=y... part of the URL
            var query = this.HttpContext.Request.QueryString;

            //No parameters in GET request
            if (String.IsNullOrEmpty(query.ToString()))
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

                string[] strlist = param.Split(separators);    

                //Counting the number of parameters
                for(int i = 0; i < strlist.Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        //Increasing when arriving on parameter value
                        counter++;
                    }
                }

                object[] parametersArray = new object[counter];
                counter = 0;

                //Filling the parameters array with the values only
                for (int i = 0; i < strlist.Length; i++)
                {
                    // % 2 since we are getting what is between = and &
                    if (i % 2 == 1)
                    {
                        parametersArray[counter] = strlist[i];
                        counter++;
                    }
                }
                try
                {
                    //Calling the method
                    UseMethod(ObjectName, Method, parametersArray);
                }
                catch (InexistantObjectException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object " + ObjectName + " doesn't exists");
                }
                catch (UncorrectMethodNameException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The object "+ObjectName+" doesn't implements the method " + Method);
                }
               
                catch (ArgumentException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The method " + Method + " is used with wrong parameters types!");
                }
                catch(TargetParameterCountException)
                {
                    return StatusCode(HTTP_CODE_FAILURE, "The method " + Method + " isn't used with the good number of parameters!");
                }
                
            }
            return StatusCode(HTTP_CODE_SUCCESS, "Calling the method " + Method + " on " + ObjectName);

        }

        /*
         * Method that calls the method on the right peripheral instance
         * @param objectName Type of the peripheral
         * @param methodName Name of the method to call on the peripheral
         * @param Array that contains all the parameters value (can be null)
         */
        private void UseMethod(string objectName, string methodName, object[] methodParams) 
        {           
            //Getting the peripheral instance based on the name 
            IDevice device = PeripheralFactory.GetInstance(objectName);
            
            //Getting all the methods of the device
            List<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());

            MethodInfo correctMethodName = null;

            //finding the good method
            foreach (MethodInfo method in methodList)
            {
                if (method.Name.Equals(methodName))
                {
                    correctMethodName = method;
                }
            }

            //handling wrong url
            if (correctMethodName is null)
            {
                throw new UncorrectMethodNameException();
            }

            //Calling the method with the parameters
            correctMethodName.Invoke(device, methodParams);
        }

    }
}
 