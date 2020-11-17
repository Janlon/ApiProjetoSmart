using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace WebApiBusiness.Util
{
    public class ApiControllerBase : ApiController
    {
        public IHttpActionResult CustomBadRequest<T>(T value)
        {
            ContentNegotiationResult negotiationResult = this.Configuration.Services.GetContentNegotiator().Negotiate(typeof(T), this.Request, (IEnumerable<MediaTypeFormatter>)this.Configuration.Formatters);
            return (IHttpActionResult)this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = (HttpContent)new ObjectContent<T>(value, negotiationResult.Formatter, negotiationResult.MediaType)
            });
        }
    }
}
