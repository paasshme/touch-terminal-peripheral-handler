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
 * Cas d'erreurs dans l'URL => pointless étant donné qu'en navigateur l'user n'a pas accès à l'URL
 * un ? et rien après
 * plusieurs ?
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

        //private readonly PeripheralFactory _myFactory;


        public BrowserRequestsController(/*PeripheralFactory theFactory*/)        {

           // _myFactory = theFactory;

           // PeripheralEventHandler peh = new PeripheralEventHandler(HttpContext);
        }
        



        /**
         * TODO: ne pas uiquement call la facto 
         * -> la facto me renvoie un objet into j'applique un traitement sur l'objet renvoyé
         */
        [Route("api")]
        public async Task<string> testMethodAsync(string param)
        {
            PeripheralFactory.Init();

            // PeripheralEventHandler peh = new PeripheralEventHandler(HttpContext);
            return "Test app work & facto init!";
        }


        //TODO change return type to IActionResult 
        [Route("api/{ObjectName}/{Method}")]
        public string CommunicateToPeripheral(string ObjectName, string Method)
        {
            PeripheralFactory.Init();
            //Getting ?param1=4&... part of the URL
            var query = this.HttpContext.Request.QueryString;
            //If there are no parameters in GET Request
            if (String.IsNullOrEmpty(query.ToString()))
            {
                try
                {
                    System.Console.WriteLine("Going to use the " + Method + "on the " + ObjectName);
                    UseMethod(ObjectName, Method, new object[0]);
                }
                catch (UncorrectMethodNameException e)
                {
                    return "400";
                    //return StatusCode(400);
                }
                catch (UncorrectObjectNameException e2)
                {
                    return "400";
                    //return StatusCode(400);
                }
                
            }
            else
            {
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

                string test = "";
                for (int i = 0; i < strlist.Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        test += strlist[i];
                        parametersArray[counter] = strlist[i];
                        counter++;
                    }
                }
                try
                {
                    System.Console.WriteLine("GOnna call the "+Method + "on " + ObjectName);
                    UseMethod(ObjectName, Method, parametersArray);
                   // _myFactory.faitMagie(ObjectName, Method, parametersArray);
                }
                catch (UncorrectMethodNameException e)
                {
                    return "400";
                    //return StatusCode(400);
                }
                catch (UncorrectObjectNameException e2)
                {
                    return "400";
                    //return StatusCode(400);
                }
                catch (WrongParametersException e3)
                {
                    //return StatusCode(400);
                    return "400";
                }
            }
            //return StatusCode(200);
            System.Console.WriteLine("Is called "+Method);
            return "200";
        }


        private void UseMethod(string objectName, string methodName, object[] methodParams)
        {
     /*       string methodNameGotFromAPICall = "Scan"; //For instance
            string objectNameGotFromAPICall = "BarCode"; //For instance*/

            IDevice device = PeripheralFactory.GetInstance(objectName);
            List<MethodInfo> methodList = PeripheralFactory.FindMethods(device.GetType());
            MethodInfo correctOne = null;

            foreach (MethodInfo method in methodList)
                if (method.Name.Equals(methodName))
                    correctOne = method;
            //Console.WriteLine(correctOne);
            if (correctOne is null) throw new Exception("The Device does not own this function");

            correctOne.Invoke(device, methodParams);
        }



        /*
        public async Task Test()
        {
            var context = ControllerContext.HttpContext;
            var isSocketRequest = context.WebSockets.IsWebSocketRequest;

            if(isSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await sendMessage(context, webSocket);
            }
            else
            {
                context.Response.StatusCode = 400;
            }
        }

        public async Task sendMessage(HttpContext context, WebSocket socket)
        {
            string message = "Bonjour";

            var bytes = Encoding.ASCII.GetBytes(message);
            var arraySegement = new ArraySegment<byte>(bytes);
            await socket.SendAsync(arraySegement, WebSocketMessageType.Binary, false, CancellationToken.None);

            await socket.SendAsync(new ArraySegment<byte>(null), WebSocketMessageType.Binary, false, CancellationToken.None);
        }
        */
    }
}
 