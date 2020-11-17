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
    [RoutePrefix("api/Acesso")]
    public class AcessoController : ApiController
    {

        private AcessoBusiness acessoBusiness = new AcessoBusiness();

        [HttpPost]
        [Route("MotivarMotoristaArea")]
        public IHttpActionResult MotivarMotoristaArea(string sessao, int idCracha, string local, string ambiente, char sentido)
        {
            return (IHttpActionResult)this.Ok<object>(AcessoBusiness.MotivarMotoristaArea(sessao, idCracha, local, ambiente, sentido));
        }

        [Route("ValidaEntradapatioEM")]
        public async Task<IHttpActionResult> ValidaEntradapatio(string placa, string sessao = "")
        {
            return (IHttpActionResult)this.Ok<UnisolutionMotorista>(await new MotivarMotoristaAreaBO().MMA_ValidaEntradaPatio(placa, Local.Patio, sessao));
        }

        [Authorize]
        [HttpPost]
        [Route("MotivarMotorista")]
        public IHttpActionResult MotivarMotorista([FromBody] Acesso acesso)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)acesso), nameof(MotivarMotorista));
            if (acesso != null)
            {
                string json = this.acessoBusiness.MotivarMotorista(acesso);
                return (IHttpActionResult)this.Ok<dynamic>(JsonConvert.DeserializeObject<dynamic>(json));
            }
            return (IHttpActionResult)this.Ok<RetornoApiIntegrada>(new RetornoApiIntegrada()
            {
                StatusMessage = "Erro na requisição.",
                StatusCode = 1,
                Result = new List<RetornoApiIntegrada.Resultado>()
        {
          new RetornoApiIntegrada.Resultado(96, "Formatação do Objeto está invalida.")
        }
            });
        }

        [Authorize]
        [HttpPost]
        [Route("FinalizaOs")]
        public IHttpActionResult FinalizaOs([FromBody] Acesso acesso)
        {
            string json = this.acessoBusiness.FinalizaOS(acesso);

            return (IHttpActionResult)this.Ok<RetornoApiIntegrada>(JsonConvert.DeserializeObject<RetornoApiIntegrada>(json));
        }

        [Authorize]
        [HttpPost]
        [Route("CancelaOs")]
        public IHttpActionResult CancelaOs([FromBody] Acesso acesso)
        {
            return (IHttpActionResult)this.Ok<RetornoApiIntegrada>(JsonConvert.DeserializeObject<RetornoApiIntegrada>(this.acessoBusiness.CancelaOS(acesso)));
        }

        [Authorize]
        [HttpPost]
        [Route("ConsultaAcesso")]
        public IHttpActionResult ConsultarAcesso([FromBody] PontoDeControle equipamento)
        {
            return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.ConsultarAcesso(equipamento));
        }

        [Authorize]
        [HttpPost]
        [Route("EntradaNaoAutorizada")]
        public IHttpActionResult EntradaNaoAutorizada([FromBody] EntradaNaoAutorizadaDTO entradaNaoAutorizadaDTO)
        {
            return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.EntradaNaoAutorizada(entradaNaoAutorizadaDTO));
        }

        [Authorize]
        [HttpPost]
        [Route("LiberarVeiculo")]
        public IHttpActionResult LiberarBalanca([FromBody] LiberarBalancaDTO liberarBalancaDTO)
        {
            return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.LiberarBalanca(liberarBalancaDTO));
        }
    }
}
