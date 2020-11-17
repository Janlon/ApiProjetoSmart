using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WEBAPI_VOPAK.Controllers
{
    [RoutePrefix("api/Errors")]
    public class ErrorsController : ApiController
    {
        [HttpPost]
        [Route("List")]
        public IEnumerable<ErrorBlock> ListAll()
        {
            try
            {
                Exception ex = new Exception();
                return ex.LoadAll();
            }
            catch (Exception ex) { ex.Log(); return null; }
        }
    }
}
