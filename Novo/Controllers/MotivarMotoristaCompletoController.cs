using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{

    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class MotivarMotoristaCompletoController : SmartAPIController
    {
        [HttpPost]
        [Route("api/motivar/MotivarMotorista")]
        public HttpResponseMessage PostMotivarMotorista(MotivarMotoristaCompleto item)
        {
            try
            {
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) SmartRetornoImediato(ret);

                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) SmartRetornoImediato(ret);

                return SmartRetornoImediato(MotivarMotoristaCompletoRepositorio.Add(item));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }
    }
}
