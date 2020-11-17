using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class ValidaBDCCController : SmartAPIController 
    {
        [HttpPost]
        [Route("api/BDCC/ValidaBDCC")]
        public HttpResponseMessage PostColaborador(ValidaBDCCViewModel item)
        {
            Retorno ret = null;
            try
            {
                //Valida a requisição em si:
                ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida os dados do ambiente:
                ret.AddResult(SmartValidaAmbiente(item.ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida os documentos do colaborador:
                ret.AddResult(SmartValidaDocumentosDoColaborador(item.ambiente,
                    item.tipoDocumento, item.cpf, item.numDocumento));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Se chegou aqui, processa a requisição e devolve o Mensagens.
                ret.AddResult( ValidaBDCCRepositorio.Add(item) );
                if (ret.Ok())
                    ret.AddResult(new { StatusMotorista = 1 });
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
            return SmartRetornoImediato(ret);       
        }
    }
}
