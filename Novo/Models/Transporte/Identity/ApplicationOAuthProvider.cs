namespace WEBAPI_VOPAK.Providers
{

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Script.Serialization;
    using WEBAPI_VOPAK.Models;

    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
  
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Valida as requisições posteriores ao login 
        /// Fonte: https://msdn.microsoft.com/en-us/library/microsoft.owin.security.oauth.oauthauthorizationserverprovider.validateclientauthentication(v=vs.113).aspx
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.FromResult(context.Validated());
        }


#pragma warning disable CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
#pragma warning restore CS1998 // O método assíncrono não possui operadores 'await' e será executado de forma síncrona
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //  if (context.Password == "d41d8cd98f00b204e9800998ecf8427e" && context.UserName == "vopak")
            if (context.Password == context.Password && context.UserName == context.UserName)
            {
                
            // identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            // identity.AddClaim(new Claim("usuario", new JavaScriptSerializer().Serialize("user")));
            // identity.AddClaim(new Claim(ClaimTypes.Name, "admn"));

                context.Validated(identity);

            }
            else
            {
                context.SetError("Usuário e senha inválidos");
                return;
            }
        }
    }
}