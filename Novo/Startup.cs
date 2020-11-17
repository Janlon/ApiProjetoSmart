using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;
using WEBAPI_VOPAK.Models;
using WEBAPI_VOPAK.Providers;

[assembly: OwinStartup(typeof(WEBAPI_VOPAK.Startup))]
namespace WEBAPI_VOPAK
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void Configuration(IAppBuilder app)
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

        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new ApplicationOAuthProvider(PublicClientId)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    app.CreatePerOwinContext<ApplicationDbContext>(new Func<ApplicationDbContext>(ApplicationDbContext.Create));
        //    // AppBuilderExtensions.CreatePerOwinContext<ApplicationUserManager>(app, (Func<IdentityFactoryOptions<M0>, IOwinContext, M0>) new Func<IdentityFactoryOptions<ApplicationUserManager>, IOwinContext, ApplicationUserManager>(ApplicationUserManager.Create));
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions());
        //    app.UseExternalSignInCookie("ExternalCookie");
        //    Startup.PublicClientId = "self";
        //    Startup.OAuthOptions = new OAuthAuthorizationServerOptions()
        //    {
        //        TokenEndpointPath = new PathString("/Token"),
        //        Provider = (IOAuthAuthorizationServerProvider)new ApplicationOAuthProvider(Startup.PublicClientId),
        //        AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(14.0),
        //        AllowInsecureHttp = true
        //    };
        //    AppBuilderExtensions.UseOAuthBearerTokens(app, Startup.OAuthOptions);
        //}

        //public void Configuration(IAppBuilder app)
        //{
        //    this.ConfigureAuth(app);
        //}
    }
}
