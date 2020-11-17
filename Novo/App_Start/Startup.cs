using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using WEBAPI_VOPAK.Providers;

[assembly: OwinStartup("Startup", typeof(WEBAPI_VOPAK.App_Start.Startup))]

namespace WEBAPI_VOPAK.App_Start
{
    public class Startup
    {
     
        public static void Configuration(IAppBuilder app)
        {
            // configuracao WebApi
            var config = new HttpConfiguration();
            // configurando rotas
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                  name: "DefaultApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults: new { id = RouteParameter.Optional }
             );
            // ativando cors
            app.UseCors(CorsOptions.AllowAll);
            // ativando a geração do token
            ConfigureAuth(app);
            // ativando configuração WebApi
            app.UseWebApi(config);
        }

        internal static void ConfigureAuth(IAppBuilder app)
        {
           
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new ApplicationOAuthProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
