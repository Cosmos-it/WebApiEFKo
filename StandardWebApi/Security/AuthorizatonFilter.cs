using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace StandardWebApi.Security
{
    public class AuthorizatonFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            if (httpActionContext.Request.Headers.Authorization == null)
            {
                httpActionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = httpActionContext.Request.Headers.Authorization.Parameter;
                string decodeToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                string userName = decodeToken.Substring(0, decodeToken.IndexOf(":")); //gets userName or email
                string password = decodeToken.Substring(0, decodeToken.IndexOf(":") + 1); // gets the password


                // Login can be implement here from the server with 
                // the user credentials.
                if (userName == "taban" && password == "1234")
                {
                     // don't do anything
                }
                else
                {
                    httpActionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}