using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;

namespace StandardWebApi.Security
{

    /// <summary>
    /// This class uses delegatehandler to varify the endpoint being accessed.
    /// if there is no token to access the data, it will throw an error
    /// This will work with endpoints we want to expose to uses.
    /// Another layer of security is possible. Something like client id.
    /// etc.
    /// 
    /// Users will have to login into the website to get generate the token
    /// 
    /// </summary>
    public class APIKeyHandler : DelegatingHandler
    {
        // This _token is generated and saved in the database.
        // this will allow 
        private const string _token = "jsGLWw89nv"; 

        protected  override async Task<HttpResponseMessage> SendAsync
            (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isValidAPIKey = false;
            IEnumerable<string> isHeaders;

            //Validate api key existence
            var checkAPI = request.Headers.TryGetValues("key", out isHeaders);
            
            if(checkAPI)
            {
                if (isHeaders.FirstOrDefault().Equals(_token))
                {
                    isValidAPIKey = true;
                }
            }

            //check for api key validity
            if (!isValidAPIKey)
                return request.CreateResponse(HttpStatusCode.Forbidden, "Bad Token");

            //process the request
            var response = await base.SendAsync(request, cancellationToken);

            return response; // return response to the chain

        }
    }
}