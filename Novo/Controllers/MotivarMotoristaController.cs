using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class MotivarMotoristaController : SmartAPIController
    {
        [HttpPost]
        [Route("api/motivar/MotivarMotoristaSimples")]
        public HttpResponseMessage PostMotivarMotoristaSimples(MotivarMotorista item)
        {
            try
            {
                int wIdContratada = 0;
                int wIdColaborador = 0;
                Retorno ret = SmartValidaRequisicao(item);
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (item.idContratadaIntegradora > 0)
                    wIdContratada = Dados.GetIdContratadaIntegrada(item.Ambiente, 1, item.idContratadaIntegradora, true);
                else
                    ret.AddResult(ErroSmartApi.IdEmpresaIntegradorObrigatorio);
                if(wIdContratada>0)
                    wIdColaborador = Dados.GetIdColaboradorIntegrado(item.Ambiente, 1, wIdContratada, item.idColaboradorIntegrador, true);
                else
                    ret.AddResult(ErroSmartApi.IdColaboradorIntegradorNaoEncontrado);
                if (ret.Ok())
                {
                    item.idColaboradorIntegrador = wIdColaborador;
                    item.idContratadaIntegradora = wIdContratada;
                    ret.AddResult(MotivarMotoristaRepositorio.Add(item, ref wIdColaborador));
                }
                return SmartRetornoImediato(ret);
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }
    }
}
