using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WebApiBusiness
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>)context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(keyValuePair.Key, (object)keyValuePair.Value);
            return (Task)Task.FromResult<object>((object)null);
        }

        public override async Task ValidateClientAuthentication(
          OAuthValidateClientAuthenticationContext context)
        {
            int num = await Task.FromResult<bool>(context.Validated()) ? 1 : 0;
        }

        public override async Task GrantResourceOwnerCredentials(
          OAuthGrantResourceOwnerCredentialsContext context)
        {
            ClaimsIdentity identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (context.Password == "d41d8cd98f00b204e9800998ecf8427e" && context.UserName == "vopak")
            {
                identity.AddClaim(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "user"));
                identity.AddClaim(new Claim("usuario", new JavaScriptSerializer().Serialize((object)"user")));
                identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "admn"));
                context.Validated(identity);
            }
            else
                context.SetError("Usuario e senha invalidos");
        }
    }
}
