using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WEBAPI_VOPAK.Models;

namespace WEBAPI_VOPAK.Controllers
{
    [EnableCors("*", "*", "*")]
    [AllowAnonymous]
    public class ColaboradorController : SmartAPIController
    {
        [HttpPost]
        [Route("api/colaborador/ManterColaborador")]
        public HttpResponseMessage PostColaborador(ColaboradorViewModel item)
        {
            try
            {
                // Valida a requisição:
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida o ambiente:
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida os parâmetros do colaborador:
                ret.AddResult(SmartValidaParametrosDoColaborador(
                    item.Ambiente, item.tipoOperacao, item.idEmpresaIntegrador, item.idColaboradorIntegrador,
                    item.nome, item.sexo, item.dtNascimento, item.tel, item.emailColaborador, item.numeroCracha, item.foto,
                    item.tipoDocumento, item.numDocumento, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh,
                    item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf, item.cep));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Processar a requisição e enviar o Mensagens:
                int wIdColaborador = 0;
                return SmartRetornoImediato(ColaboradorRepositorio.Add(item, ref wIdColaborador));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }
    }
}
