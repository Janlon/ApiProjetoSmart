using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{
    //[EnableCors("*", "*", "*")]
    //[AllowAnonymous]
    //public class CancelaOSController : SmartAPIController
    //{
    //    [HttpPost]
    //    [Route("api/acesso/CancelaOS")]
    //    public HttpResponseMessage CancelaOS(CancelaOSViewModel item)
    //    {
    //        try
    //        {
    //            // Valida a requisição.
    //            Retorno ret = SmartValidaRequisicao(item);
    //            if (!ret.Ok()) return SmartRetornoImediato(ret);

    //            // Valida o ambiente informado.
    //            ret.AddResult(SmartValidaAmbiente(item.ambiente));
    //            if (!ret.Ok()) return SmartRetornoImediato(ret);

    //            // Processa a requisição e devolve o retorno.
    //            return SmartRetornoImediato(CancelaOsRepositorio.Add(item, false));
    //        }
    //        catch (Exception ex) { return SmartTrataErro(ex); }
    //    }
    //}
}
