using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IDeviceLib;
using System.Reflection;
using ProjetS3.PeripheralCreation;


/*
 * Pour tester le controlleur :
 * envoyer des requêtes à localhost:5001/browserRequests/objet/method[?parameters]
 */

/*
 * plusieurs & à la suite sans =
 * plusieurs = à la suite sans &
 * le mec met des ?,&,= dans le nom de l'objet
 * le mec met pas de méthode -> appeler to String ?
 * 
 */

namespace ProjetS3.Controllers
{
    public class BrowserRequestsController : Controller
    {
        public BrowserRequestsController()
        {
            PeripheralFactory.Init();
        }

        [HttpGet]
        [Route("api")]
        public async Task<string> testMethodAsync(string param)
        { 
            // PeripheralEventHandler peh = new PeripheralEventHandler(HttpContext);
            return "Test app work & facto init!";
        }



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
                    System.Console.WriteLine("Going to use the " + Method + "on the " + ObjectName);

                    //Calling the method without any parameter 
                    UseMethod(ObjectName, Method, new object[0]);
                }
                catch (InexistantObjectException e)
                {
                    return StatusCode(400, "The object " + ObjectName + " doesn't exists");
                }
                catch (UncorrectMethodNameException e2)
                {
                    return StatusCode(400, "The object " + ObjectName + " doesn't implements the method " + Method);
                }
            }

            
            else
            {
                //Counting the numbet of parameters
                char[] separators = {'=', '&'};
                int counter = 0;

                string param = query.ToString().Substring(1);

                string[] strlist = param.Split(separators);    


                for(int i = 0; i < strlist.Length; i++)
                {
                    if (i % 2 == 1)
                    {
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
                    System.Console.WriteLine("Gonna call the "+Method + "on " + ObjectName);
                    UseMethod(ObjectName, Method, parametersArray);
                }
                catch (InexistantObjectException e)
                {
                    return StatusCode(400,"The object " + ObjectName + " doesn't exists");
                }
                catch (UncorrectMethodNameException e2)
                {
                    return StatusCode(400,"The object "+ObjectName+" doesn't implements the method " + Method);
                }
               
                catch (WrongParametersException e3)
                {
                    return StatusCode(400,"The method " + Method + " is used with wrong parameters !");
                }
                
            }
            System.Console.WriteLine("Is called "+Method);
            return StatusCode(200, "Calling the method " + Method + " on " + ObjectName);

        }

        private void UseMethod(string objectName, string methodName, object[] methodParams) 
        {
            /*       string methodNameGotFromAPICall = "Scan"; //For instance
                   string objectNameGotFromAPICall = "BarCode"; //For instance*/

            IDevice device;

            try
            {
                device = PeripheralFactory.GetInstance(objectName);
            }
            catch (InexistantObjectException e)
            {
                throw new InexistantObjectException();
            }

            List<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());
            MethodInfo correctOne = null;

            //finding the good method
            foreach (MethodInfo method in methodList)
                if (method.Name.Equals(methodName))
                    correctOne = method;

            //handling wrong url
            if (correctOne is null) throw new UncorrectMethodNameException();
            try
            {
                correctOne.Invoke(device, methodParams);
            }
            catch (Exception e)
            {
                throw new WrongParametersException();
            }
        }

    }
}
 