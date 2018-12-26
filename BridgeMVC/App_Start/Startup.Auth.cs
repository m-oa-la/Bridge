using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using BridgeMVC.B2C;

namespace BridgeMVC
{

    public partial class Startup
    {
        public static string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        public static string ClientSecret = ConfigurationManager.AppSettings["ida:ClientSecret"];
        public static string AadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
        public static string Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        public static string RedirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];

        // B2C policy identifiers
        public static string SignUpSignInPolicyId = ConfigurationManager.AppSettings["ida:SignUpSignInPolicyId"];
        public static string EditProfilePolicyId = ConfigurationManager.AppSettings["ida:EditProfilePolicyId"];
        public static string ResetPasswordPolicyId = ConfigurationManager.AppSettings["ida:ResetPasswordPolicyId"];

        public static string DefaultPolicy = SignUpSignInPolicyId;

        public static string[] Scopes = new string[] { "https://dnvglb2cprod.onmicrosoft.com/83054ebf-1d7b-43f5-82ad-b2bde84d7b75/user_impersonation" };
        // OWIN auth middleware constants
        public const string ObjectIdElement = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        // Authorities
        public static string Authority = String.Format(AadInstance, Tenant, DefaultPolicy);

        public void ConfigureAuth(IAppBuilder app)
        {
            //string ClientId = ConfigurationManager.AppSettings["ida:ClientID"];
            ////fixed address for multitenant apps in the public cloud
            //string Authority = "https://login.microsoftonline.com/common/";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    // Generate the metadata address using the tenant and policy information
                    MetadataAddress = String.Format(AadInstance, Tenant, DefaultPolicy),

                    // These are standard OpenID Connect parameters, with values pulled from web.config
                    ClientId = ClientId,
                    RedirectUri = RedirectUri,
                    PostLogoutRedirectUri = RedirectUri,
                    UseTokenLifetime = true,
                    // Specify the callbacks for each type of notifications
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        RedirectToIdentityProvider = OnRedirectToIdentityProvider,
                        AuthorizationCodeReceived = OnAuthorizationCodeReceived,
                        AuthenticationFailed = OnAuthenticationFailed,
                    },

                    // Specify the claim type that specifies the Name property.
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name" //"name"
                    },

                    // Specify the scope by appending all of the scopes requested into one string (separated by a blank space)
                    Scope = $"openid profile offline_access https://dnvglb2cprod.onmicrosoft.com/83054ebf-1d7b-43f5-82ad-b2bde84d7b75/user_impersonation"
                }
            );
            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        ClientId = ClientId,
            //        Authority = Authority,
            //        TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
            //            // we inject our own multitenant validation logic
            //            ValidateIssuer = false,
            //        },
            //        Notifications = new OpenIdConnectAuthenticationNotifications()
            //        {
            //            RedirectToIdentityProvider = (context) =>
            //            {
            //                // This ensures that the address used for sign in and sign out is picked up dynamically from the request
            //                // this allows you to deploy your app (to Azure Web Sites, for example)without having to change settings
            //                // Remember that the base URL of the address used here must be provisioned in Azure AD beforehand.
            //                string appBaseUrl = context.Request.Scheme + "://" + context.Request.Host + context.Request.PathBase;
            //                context.ProtocolMessage.RedirectUri = appBaseUrl;
            //                context.ProtocolMessage.PostLogoutRedirectUri = appBaseUrl;
            //                return Task.FromResult(0);
            //            },
            //            // we use this notification for injecting our custom logic
            //            SecurityTokenValidated = (context) =>
            //            {
            //                // retriever caller data from the incoming principal
            //                string issuer = context.AuthenticationTicket.Identity.FindFirst("iss").Value;
            //                string UPN = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.Name).Value;
            //                string tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;

            //                //if (
            //                //    // the caller comes from an admin-consented, recorded issuer
            //                //    (db.Tenants.FirstOrDefault(a => ((a.IssValue == issuer) && (a.AdminConsented))) == null)
            //                //    // the caller is recorded in the db of users who went through the individual onboardoing
            //                //    && (db.Users.FirstOrDefault(b =>((b.UPN == UPN) && (b.TenantID == tenantID))) == null)
            //                //    )
            //                //    // the caller was neither from a trusted issuer or a registered user - throw to block the authentication flow
            //                //    throw new SecurityTokenValidationException();                            
            //                return Task.FromResult(0);
            //            },
            //            AuthenticationFailed = (context) =>
            //            {
            //                context.OwinContext.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
            //                context.HandleResponse(); // Suppress the exception
            //                return Task.FromResult(0);
            //            }
            //        }
            //    });

        }

        private Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            var policy = notification.OwinContext.Get<string>("Policy");

            if (!string.IsNullOrEmpty(policy) && !policy.Equals(DefaultPolicy))
            {
                notification.ProtocolMessage.Scope = OpenIdConnectScope.OpenId;
                notification.ProtocolMessage.ResponseType = OpenIdConnectResponseType.IdToken;
                notification.ProtocolMessage.IssuerAddress = notification.ProtocolMessage.IssuerAddress.ToLower().Replace(DefaultPolicy.ToLower(), policy.ToLower());
            }
            //else if (!string.IsNullOrEmpty(ApiIdentifier))
            //{
            //    notification.ProtocolMessage.Scope += $" offline_access {ApiIdentifier}";
            //    notification.ProtocolMessage.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            //}

            return Task.FromResult(0);
        }

        /*
         * Catch any failures received by the authentication middleware and handle appropriately
         */
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();

            // Handle the error code that Azure AD B2C throws when trying to reset a password from the login page 
            // because password reset is not supported by a "sign-up or sign-in policy"
            if (notification.ProtocolMessage.ErrorDescription != null && notification.ProtocolMessage.ErrorDescription.Contains("AADB2C90118"))
            {
                // If the user clicked the reset password link, redirect to the reset password route
                notification.Response.Redirect("/Account/ResetPassword");
            }
            else if (notification.Exception.Message == "access_denied")
            {
                notification.Response.Redirect("/");
            }
            else
            {
                notification.Response.Redirect("/Home/Error?message=" + notification.Exception.Message);
            }

            return Task.FromResult(0);
        }


        /*
         * Callback function when an authorization code is received 
         */
        private async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedNotification notification)
        {
            // Extract the code from the response notification
            var code = notification.Code;

            //string signedInUserID = notification.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string signedInUserID = notification.AuthenticationTicket.Identity.Claims.First(c => c.Type == "userId").Value;
            TokenCache userTokenCache = new MSALSessionCache(signedInUserID, notification.OwinContext.Environment["System.Web.HttpContextBase"] as HttpContextBase).GetMsalCacheInstance();
            ConfidentialClientApplication cca = new ConfidentialClientApplication(ClientId, Authority, RedirectUri, new ClientCredential(ClientSecret), userTokenCache, null);
            try
            {
                AuthenticationResult result = await cca.AcquireTokenByAuthorizationCodeAsync(code, Scopes);
            }
            catch (Exception ex)
            {
                //TODO: Handle
                throw;
            }
        }

    }
}