using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiBusiness.App_Data;
using WebApiBusiness.BO;
using WebApiBusiness.Business;
using WebApiBusiness.Models;
using WebApiBusiness.Unisolution;

namespace SmartAPIIntegradorFase1.Controllers
{
    [RoutePrefix("api/BDCC")]
    public class BDCCController : ApiController
    {
        private AcessoBusiness acessoBusiness = new AcessoBusiness();

        [Authorize]
        [HttpPost]
        [Route("ValidaBDCC")]
        public IHttpActionResult ValidaBDCC([FromBody] Acesso acesso)
        {
            string str = this.acessoBusiness.ValidaBDCC(acesso);
            if (str.ToUpper().Contains("ERROR"))
                return (IHttpActionResult)this.Ok<RetornoApiIntegrada>(JsonConvert.DeserializeObject<RetornoApiIntegrada>(str));
            return (IHttpActionResult)this.Ok<RetornoBDCCIntegrada>(JsonConvert.DeserializeObject<RetornoBDCCIntegrada>(str));
        }
    }
}
