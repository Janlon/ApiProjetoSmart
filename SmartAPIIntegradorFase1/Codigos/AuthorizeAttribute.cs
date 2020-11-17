using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace WebApiBusiness
{
    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            if (this.IsAuthorized(actionContext))
                return;
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            this.HandleUnauthorizedRequest(actionContext);
        }
    }
}
