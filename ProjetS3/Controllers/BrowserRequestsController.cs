using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Web;

/*
 * Pour tester le controlleur :
 * envoyer des requêtes à localhost:5001/browserRequests/objet/method[?parameters]
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


        [Route("BrowserRequests/{ObjectName}/{parameters?}")]
        public IActionResult CommunicateToPeripheral(string ObjectName, string parameters)
        { 
            string[] strlist1 = parameters.Split('?');

            var context = this.HttpContext;
            var query = context.Request.QueryString;

            if (String.IsNullOrEmpty(query.ToString()))
            {
                _myFactory.faitMagie(ObjectName, strlist1[0], new object[0]);
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
                _myFactory.faitMagie(ObjectName, strlist1[0], parametersArray);
               
            }
            /*
             * TODO: refaire la réponse, si l'objet ou la méthode n'existe pas => renvoyer un code d'erreur (300)
             * si l'objet / methode n'existe pas, on catch l'exception d    ans faitMagie, on la recatch ici into code d'erreur
             */
            return StatusCode(200);
        }
    }
}
 