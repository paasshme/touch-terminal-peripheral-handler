using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Web;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading;

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

        private readonly IFactory _myFactory;

        public BrowserRequestsController(IFactory theFactory)
        {
            _myFactory = theFactory;
        }
        



        /**
         * TODO: ne pas uiquement call la facto 
         * -> la facto me renvoie un objet into j'applique un traitement sur l'objet renvoyé
         */

        [Route("BrowserRequests/{ObjectName}/{parameters?}")]
        public IActionResult CommunicateToPeripheral(string ObjectName, string parameters)
        {
            string[] strlist1 = parameters.Split('?');

            //Getting ?param1=4&... part of the URL
            var context = this.HttpContext;
            var query = context.Request.QueryString;

            //If there are no parameters in GET Request
            if (String.IsNullOrEmpty(query.ToString()))
            {
                try
                {
                    _myFactory.faitMagie(ObjectName, strlist1[0], new object[0]);
                }
                catch (UncorrectMethodNameException e)
                {
                    return StatusCode(400);
                }
                catch (UncorrectObjectNameException e2)
                {
                    return StatusCode(400);
                }
                
            }
            else
            {
                char[] separators = {'=', '&'};
                int counter = 0;

                string param = query.ToString();

                string[] strlist2 = param.Split("?");
                string[] strlist3 = strlist2[1].Split(separators);

                for(int i = 0; i < strlist3.Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        counter++;
                    }
                }

                object[] parametersArray = new object[counter];
                counter = 0;

                for (int i = 0; i < strlist3.Length; i++)
                {
                    if (i % 2 == 1)
                    {
                        parametersArray[counter] = strlist3[i];
                        counter++;
                    }
                }

                try
                {
                    _myFactory.faitMagie(ObjectName, strlist1[0], parametersArray);
                }
                catch (UncorrectMethodNameException e)
                {
                    return StatusCode(400);
                }
                catch (UncorrectObjectNameException e2)
                {
                    return StatusCode(400);
                }
                catch (WrongParametersException e3)
                {
                    return StatusCode(400);
                }
            }
            return StatusCode(200);
        }


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
                context.Response.StatusCode =400;
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
    }
}
 