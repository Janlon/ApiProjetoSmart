namespace WEBAPI_VOPAK
{ 
    using System;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using WEBAPI_VOPAK.Models;

    public static class ControllerExtensions
    {

        /// <summary>
        /// Extensão para facilitar o retorno de erros padrão.
        /// </summary>
        /// <param name="api">Este <see cref="ApiController"/>.</param>
        /// <returns>Objeto do tipo <see cref="HttpResponseMessage"/>.</returns>
        public static HttpResponseMessage CreateUnnespecifiedReturn(this ApiController api)
        {
            Retorno ret = new Retorno();
            ret.AddResult(ErrosMotivarMotorista.ErroNaoEspecificado);
            return api.Request.CreateResponse<Retorno>(ret);
        }

        /// <summary>
        /// Extensão para facilitar o retorno de erros padrão.
        /// </summary>
        /// <param name="api">Este <see cref="ApiController"/>.</param>
        /// <param name="ex">Objeto do tipo <see cref="Exception"/>.</param>
        /// <returns>Objeto do tipo <see cref="HttpResponseMessage"/>.</returns>
        public static HttpResponseMessage CreateBadResponse(this ApiController api, Exception ex)
        { return api.Request.CreateResponse<string>(ex.Message.ToString()); }

        /// <summary>
        /// Extensão para facilitar a captura do IP do processo de requisição.
        /// </summary>
        /// <param name="value">Este <see cref="ApiController"/>.</param>
        /// <param name="request">Requisição recebida.</param>
        /// <returns>Endereço de IP (v4) na forma de texto.</returns>
        public static string RemoteIP(this ApiController value, HttpRequestMessage request)
        {
            string str = "";
            try
            {
                HttpContextWrapper property = null;
                if (request != null)
                    property = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                if (property != null)
                    str = property.Request.UserHostAddress;
            }
            catch { str = ""; }
            return str;
        }
    }
}