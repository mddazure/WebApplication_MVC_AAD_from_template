using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin;

namespace WebApplication_MVC_AAD_from_template
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string authority = aadInstance + tenantId;
       
        


        public void ConfigureAuth(IAppBuilder app)
        {

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            /*{CookieDomain = "https://webapplicationmvcaadfed.azurewebsites.net" });
            new OpenIdConnectProtocolValidator { RequireNonce = false };*/                      

            
            app.UseOpenIdConnectAuthentication(
               new OpenIdConnectAuthenticationOptions
               {
                    //See https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-webapp#owin-startup-class

                    // Sets the ClientId, authority, RedirectUri as obtained from web.config
                   ClientId = clientId,
                   Authority = authority,
                    // PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
                   PostLogoutRedirectUri = postLogoutRedirectUri,
                   //redir,
                   RedirectUri = redirectUri,                   
                   Scope = OpenIdConnectScope.OpenIdProfile,
                    // ResponseType is set to request the id_token - which contains basic information about the signed-in user
                    ResponseType = OpenIdConnectResponseType.IdToken,
                    // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                    // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
                    // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter 
                    TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = false
                   }
                   

               });

            app.Use((context, next) =>
            {

                if (context.Request.Headers.TryGetValue("x-forwarded-proto", out String[] proto))
                {
                    context.Request.Protocol = proto[0];
                }

                if (context.Request.Headers.TryGetValue("x-forwarded-host", out String[] host))
                {
                    context.Request.Host = new Microsoft.Owin.HostString(host[0]);
                }
                return next();
            });

        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
