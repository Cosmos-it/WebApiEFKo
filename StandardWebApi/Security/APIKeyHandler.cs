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
    public class APIKeyHandler : DelegatingHandler
    {
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
                return request.CreateResponse(HttpStatusCode.Forbidden, "Bad API key");

            //process the request
            var response = await base.SendAsync(request, cancellationToken);

            return response; // return response to the chain

        }
    }
}