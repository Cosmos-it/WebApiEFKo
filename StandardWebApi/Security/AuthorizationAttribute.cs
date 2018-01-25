using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace StandardWebApi.Security
{
    public class AuthorizationAttribute : System.Web.Http.AuthorizeAttribute
    {
        private const string _securityToken = "token";

        // This can be used only when the user has the right identity after log in
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
                base.HandleUnauthorizedRequest(actionContext);
            else
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }

        private bool Authorize(HttpRequestContext actionContext)
        {
            try
            {
                HttpRequestBase request = actionContext.RequestContext.HttpContext.Request;
                string token = request.Params[_securityToken];

                return SecurityManager.IsTokenValid(token, CommonManager.GetIP(request), request.UserAgent);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}