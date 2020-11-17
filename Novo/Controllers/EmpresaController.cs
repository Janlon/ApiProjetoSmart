using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class EmpresaController : SmartAPIController
    {
        [HttpPost]
        [Route("api/empresa/ManterEmpresa")]
        public HttpResponseMessage PostEmpresa(EmpresaViewModel item)
        {
            try
            {
                // Validar a requisição:
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o ambiente:
                ret.AddResult(SmartValidaAmbiente(item.ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o tipo de operação:
                ret.AddResult(SmartValidaTipoDeOperacao(item.tipoOperacao, true));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar os parâmetros da empresa:
                ret.AddResult(SmartValidaParametrosDaEmpresa(
                    item.ambiente, item.idEmpresaIntegrador.Value, 
                    item.cnpj, item.razaoSocial, item.nomeFantasia, item.representante, item.emailRepresentante,
                    item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf, item.cep));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Se chegou até aqui, processar e enviar o Mensagens.
                Int32 wIdEmpresa = int.MinValue;
                return SmartRetornoImediato(EmpresaRepositorio.Add(item, ref wIdEmpresa));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }

        private int SmartValidaTipoDeOperacao(string tipoOperacao)
        {
            throw new NotImplementedException();
        }
    }
}
