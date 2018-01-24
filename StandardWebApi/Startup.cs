
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Microsoft.Owin;
using System;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(StandardWebApi.Startup))]
namespace StandardWebApi
{
    public partial class Startup
    {

        public static Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the application for OAuth based flow
            PublicClientId = "self";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath =
                new PathString("api/Products"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(10),
                // In production mode set AllowInsecureHttp = false
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}