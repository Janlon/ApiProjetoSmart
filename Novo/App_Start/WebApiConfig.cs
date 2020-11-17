using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Web.Http;
using System.Web.Http.Filters;
using WEBAPI_VOPAK.Providers;

namespace WEBAPI_VOPAK
{
    public static class WebApiConfig 
    {
        public static string PublicClientId { get; private set; }

        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web
            ((DefaultContractResolver)config
                .Formatters
                .JsonFormatter
                .SerializerSettings
                .ContractResolver)
                .IgnoreSerializableAttribute = true;

            //config.SuppressDefaultHostAuthentication();
           // config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.SuppressDefaultHostAuthentication();
           // config.Filters.Add((IFilter)new HostAuthenticationFilter("Bearer"));
            // Rotas da API da Web
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           

        }

        
    }
}
