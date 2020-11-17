using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WEBAPI_VOPAK.Models;
using WEBAPI_VOPAK.Models.Componentes;

namespace WEBAPI_VOPAK.Controllers
{
    [RoutePrefix("api/Acesso")]
    public class AcessoController : SmartAPIController
    {
        //private AcessoBusiness acessoBusiness = new AcessoBusiness();
        [Authorize]
        [HttpPost]
        [Route("MotivarMotorista")]
        public async Task<HttpResponseMessage> MotivarMotoristaAsync([FromBody] MotivarMotoristaCompleto item)
        {
            try
            {
                // Validação da requisição:
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                ///Validação do ambiente:
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validação dos parâmetros da empresa:
                ret.AddResult(SmartValidaParametrosDaEmpresa(item.Ambiente, item.idEmpresaIntegrador, item.cnpj, item.razaoSocial, item.nomeFantasia, item.representante, item.emailRepresentante, item.endereco, item.numEndereco, item.bairroEndereco, item.cidade, item.uf, item.cep));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validação dos parâmetros do motorista:
                ret.AddResult(SmartValidaParametrosDoColaborador(
                    item.Ambiente, item.tipoOperacao,
                    item.idEmpresaIntegrador,
                    item.idColaboradorIntegrador,
                    item.nome,
                    item.sexo,
                    item.dtNascimento,
                    item.tel,
                    item.emailColaborador,
                    item.numeroCracha,
                    item.foto,
                    item.tipoDocumento, item.numDocumento, item.cpf, item.cnh, item.orgaoEmissorCnh, item.emissorCnh, item.enderecoMotorista, item.numEnderecoMotorista, item.bairroEnderecoMotorista, item.cidadeMotorista, item.ufMotorista, item.cepMotorista));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                try
                {
                    //---------- parte da nova api e base----------
                    int? numordemservico = Convert.ToInt32(item.numOs);
                    int idcolaboradorretorno = await NovaApiInserir(item.nome, item.cpf, item.numDocumento, item.tipoDocumento, item.cnpj, item.cnh, item.razaoSocial, item.nomeFantasia, false, item.placa, numordemservico, item.area, item.numeroCracha);
                    //----------------------------------------------
                }
                catch (Exception ex)
                {

                }

                // Se chegou aqui, processar:
                return SmartRetornoImediato(MotivarMotoristaCompletoRepositorio.Add(item));
                

            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }

        private static async Task<int> NovaApiInserirAgenda(int area, int? numOs, int? empresaid, int? pessoaid, int? veiculoid, string placa = null, string credencial = null)
        {
            int id = 0;
            try
            {
                List<int> ids = new List<int>();
                List<EquipamentoViewModel2> evm = await ApiQuery.GetListaEquipamentosPorSetorAsync(area);
                foreach (var item in evm)
                {
                    ids.Add(item.Id);
                }
                int? crachaId = null;
                var cracha = await ApiQuery.GetCrachaPeloNumeroCredencialAsync(credencial);
                if (cracha != null)
                {
                    crachaId = cracha.Id;
                }
                else
                {
                    crachaId = null;
                }
                if(empresaid == 0)
                {
                    empresaid = null;
                }
                if(veiculoid == 0)
                {
                    veiculoid = null;
                }
                AgendaViewModel2 agenda = new AgendaViewModel2()
                {
                    Cadastro = DateTime.Now,
                    Chegada = DateTime.Now,
                    Saida = DateTime.Now.AddHours(12),
                    Credencial = credencial,
                    OrdemServico = numOs,
                    EquipamentosId = ids,
                    Placa = placa,
                    EmpresaId = empresaid,
                    PessoaId = pessoaid,
                    VeiculoId = veiculoid,
                    CrachaId = crachaId,
                    IsDeleted = false
                };
                var ret = Dados.InsereRequisicao("P", JsonConvert.SerializeObject(agenda), "172.100.0.2");
                ApiClient Api = new ApiClient();
                var result = await Api.Use(HttpMethod.Post, agenda, "api/agenda");
            }
            catch (Exception)
            {

            }
            return id;
        }
        private static async Task<int> NovaApiInserir(string nome, string cpf, string numDocumento, string tipoDocumento, string cnpj, string cnh, string razaosocial, string nomefantasia, bool excluido, string placa, int? numos, int area, string numeroCracha)
        {
            int idPessoa = 0;
            int idagendaretorno = 0;
            EmpresaViewModel2 empresaretorno = new EmpresaViewModel2();
            VeiculoViewModel2 veiculoretorno = new VeiculoViewModel2();

            //gambiarra da unisolution
            if (area == 0)
            {
                area = 6;
            }

            try
            {
                var pp = await ApiQuery.GetPessoaPeloCPF(cpf);
                if (pp != null)
                {
                    if (pp.Id != 0)
                    {
                        idPessoa = pp.Id;
                    }
                }

                if (idPessoa == 0)
                {
                    //inserir documentos na nova base
                    List<DocumentoViewModel2> documentos = new List<DocumentoViewModel2>();
                    if (!string.IsNullOrEmpty(cpf))
                    {
                        DocumentoViewModel2 documentoViewCPF = new DocumentoViewModel2()
                        {
                            Numero = cpf.Trim(),
                            Tipo = TipoDocumentoPessoa.CPF
                        };
                        documentos.Add(documentoViewCPF);
                    }
                    if (!string.IsNullOrEmpty(cnh))
                    {
                        DocumentoViewModel2 docCNH = new DocumentoViewModel2()
                        {
                            Numero = cnh.Trim(),
                            Tipo = TipoDocumentoPessoa.CNH
                        };
                        documentos.Add(docCNH);
                    }
                    if (!string.IsNullOrEmpty(numDocumento))
                    {
                        if (numDocumento != cpf && !string.IsNullOrEmpty(tipoDocumento))
                        {
                            Enum.TryParse(tipoDocumento.Trim().ToUpper(), out TipoDocumentoPessoa td);
                            DocumentoViewModel2 documentoView = new DocumentoViewModel2()
                            {
                                Numero = numDocumento.Trim(),
                                Tipo = td
                            };
                            documentos.Add(documentoView);
                        }
                    }

                    //inserir empresa na nova base
                    List<EmpresaViewModel2> empresas = new List<EmpresaViewModel2>();
                    //existe um cnpj para pesquisar e inserir?
                    if (!string.IsNullOrEmpty(cnpj) && !string.IsNullOrWhiteSpace(cnpj))
                    {
                        //verificar se existe empresa cadastrado na base
                        empresaretorno = await ApiQuery.GetEmpresaPeloCNPJ(cnpj);
                        //trouxe um objeto que nao esta nulo
                        if (empresaretorno != null)
                        {
                            // não existe empresa na base
                            if (empresaretorno.Id == 0)
                            {
                                //adicionar para ser incluido e sai
                                empresas.Add(empresaretorno);
                            }
                            else // existe a empresa na base
                            {
                                empresaretorno = new EmpresaViewModel2()
                                {
                                    Id = empresaretorno.Id,
                                    CNPJ = cnpj.Trim(),
                                    NomeFantasia = nomefantasia.ToUpper().Trim(),
                                    RazaoSocial = razaosocial.ToUpper().Trim(),
                                    IsDeleted = excluido,
                                    Cadastro = DateTime.Now
                                };
                                empresas.Add(empresaretorno);
                            }
                        }
                        else
                        {
                            empresaretorno = new EmpresaViewModel2()
                            {
                                CNPJ = cnpj.Trim(),
                                NomeFantasia = nomefantasia.ToUpper().Trim(),
                                RazaoSocial = razaosocial.ToUpper().Trim(),
                                IsDeleted = excluido,
                                Cadastro = DateTime.Now
                            };
                            empresas.Add(empresaretorno);
                        }
                    }

                    //inserir veiculo na nova base
                    List<VeiculoViewModel2> veiculos = new List<VeiculoViewModel2>();
                    //existe uma placa para pesquisar e inserir?
                    if (!string.IsNullOrEmpty(placa) && !string.IsNullOrWhiteSpace(placa))
                    {
                        //verificar se existe o veiculo cadastrado na base
                        veiculoretorno = await ApiQuery.GetVeiculoPelaPLaca(placa);
                        // trouxe um objeto que nao esta nulo
                        if (veiculoretorno != null)
                        {
                            // não existe o veiculo na base
                            if (veiculoretorno.Id == 0)
                            {
                                //adicionar para ser incluido e sai
                                veiculos.Add(veiculoretorno);
                            }
                            else // existe o veiculo na base
                            {
                                veiculoretorno = new VeiculoViewModel2
                                {
                                    Id = veiculoretorno.Id,
                                    Placa = placa,
                                    Cadastro = DateTime.Now,
                                    IsDeleted = false
                                };
                                veiculos.Add(veiculoretorno);
                            }
                        }
                        else
                        {
                            veiculoretorno = new VeiculoViewModel2
                            {
                                Placa = placa,
                                Cadastro = DateTime.Now,
                                IsDeleted = false
                            };
                            veiculos.Add(veiculoretorno);
                        }
                    }

                    PessoaViewModel2 pessoa = new PessoaViewModel2()
                    {
                        Cadastro = DateTime.Now,
                        IsDeleted = false,
                        Nome = nome.ToUpper().Trim(),
                        Documentos = documentos,
                        Empresas = empresas,
                        Veiculos = veiculos,
                    };

                    ApiClient Api = new ApiClient();
                    var result = await Api.Use(HttpMethod.Post, pessoa, "api/pessoa");
                    if (result.Sucess)
                    {
                        PessoaViewModel2 pvm = new PessoaViewModel2();
                        foreach (var doc in documentos)
                        {
                            if (doc.Tipo.Equals(TipoDocumentoPessoa.CPF))
                            {
                                pvm = await ApiQuery.GetPessoaPeloCPF(cpf);
                                break;
                            }
                            else if (doc.Tipo.Equals(TipoDocumentoPessoa.CNH))
                            {
                                pvm = await ApiQuery.GetPessoaPeloCNH(cnh);
                                break;
                            }
                            else
                            {
                                pvm = await ApiQuery.GetPessoaPeloDOC(numDocumento);
                                break;
                            }
                        }

                        if (pvm != null)
                        {
                            idPessoa = pvm.Id;
                        }

                        if (!string.IsNullOrEmpty(cnpj) && !string.IsNullOrWhiteSpace(cnpj))
                        {
                            if (empresaretorno != null)
                            {
                                if (empresaretorno.Id == 0)
                                {
                                    empresaretorno = await ApiQuery.GetEmpresaPeloCNPJ(cnpj);
                                }
                            }
                        }
                        else
                        {
                            empresaretorno.Id = 0;   
                        }

                        if (!string.IsNullOrEmpty(placa) && !string.IsNullOrWhiteSpace(placa))
                        {
                            if (veiculoretorno != null)
                            {
                                if (veiculoretorno.Id == 0)
                                {
                                    veiculoretorno = await ApiQuery.GetVeiculoPelaPLaca(placa);
                                }
                            }
                        }
                        else
                        {
                            veiculoretorno.Id = 0;
                        }

                        idagendaretorno = await NovaApiInserirAgenda(area, numos, empresaretorno.Id, idPessoa, veiculoretorno.Id, placa, numeroCracha);

                    }
                }
                else
                {
                    //existe um cnpj para pesquisar e inserir?
                    if (!string.IsNullOrEmpty(cnpj) && !string.IsNullOrWhiteSpace(cnpj))
                    {
                        //verificar se existe empresa cadastrado na base
                        empresaretorno = await ApiQuery.GetEmpresaPeloCNPJ(cnpj);
                    }
                    else
                    {
                        empresaretorno.Id = 0;
                    }

                    //existe uma placa para pesquisar e inserir?
                    if (!string.IsNullOrEmpty(placa) && !string.IsNullOrWhiteSpace(placa))
                    {
                        //verificar se existe o veiculo cadastrado na base
                        veiculoretorno = await ApiQuery.GetVeiculoPelaPLaca(placa);
                    }
                    else
                    {
                        veiculoretorno.Id = 0;
                    }

                    idagendaretorno = await NovaApiInserirAgenda(area, numos, empresaretorno.Id, idPessoa, veiculoretorno.Id, placa, numeroCracha);
                }
            }
            catch (Exception)
            {

            }
            return idagendaretorno;
        }


        [Authorize]
        [HttpPost]
        [Route("FinalizaOs")]
        public HttpResponseMessage FinalizaOs([FromBody] CancelaOSViewModel item)
        {
            try
            {
                // Valida a existência da requisição:.
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida o ambiente informado.
                ret.AddResult(SmartValidaAmbiente(item.ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Processa a requisição e devolve o retorno.
                return SmartRetornoImediato(CancelaOsRepositorio.Add(item, true));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }

        [Authorize]
        [HttpPost]
        [Route("CancelaOs")]// esse está ok, certo?
        public HttpResponseMessage CancelaOs([FromBody] CancelaOSViewModel item)
        {
            try
            {
                // Valida a existência da requisição:.
                Retorno ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida o ambiente informado.
                ret.AddResult(SmartValidaAmbiente(item.ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Processa a requisição e devolve o retorno.
                return SmartRetornoImediato(CancelaOsRepositorio.Add(item, false));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
        }

        [Authorize]
        [HttpPost]
        [Route("ConsultaAcesso")]
        public HttpResponseMessage ConsultarAcesso([FromBody] PontoDeControleViewModel item)
        {

            Retorno ret;
            try
            {
                // Valida a existência da requisição:
                ret = SmartValidaRequisicao(item);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida ambiente:
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar outros parâmetros da requisição:
                ret.AddResult(SmartValidaConsultaAcesso(item));

                // Finalmente, processar a requisição:
                if (ret.Ok())
                    // Adicionar a coleção que venha desde a consulta aos dados:
                    ret.AddResult((IEnumerable<dynamic>)Dados.ConsultaAcessos(item));
                
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
            return SmartRetornoImediato(ret);
            //return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.ConsultarAcesso(equipamento));
        }

        [Authorize]
        [HttpPost]
        [Route("EntradaNaoAutorizada")]
        public HttpResponseMessage EntradaNaoAutorizada([FromBody] EntradaNaoAutorizadaDTO item)
        {
            Retorno ret = new Retorno();
            try
            {
                // Valida a existência da requisição:
                ret.AddResult(SmartValidaRequisicao(item));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida ambiente:
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);


                // Valida o CPF, se informado.
                var temp = item.CPF.HasValidLenght(11,11,"");
                switch (temp)
                {
                    case ValidLenghtResult.IsEmpty:
                    case ValidLenghtResult.IsNull:
                        ret.AddResult(ErroSmartApi.CPFObrigatorio);
                        break;
                    case ValidLenghtResult.IsGreater:
                    case ValidLenghtResult.IsLower:
                    case ValidLenghtResult.IsInvalid:
                        ret.AddResult(ErroSmartApi.CPFTamanhoExcedido);
                        break;
                }
                if (!ret.Ok()) return SmartRetornoImediato(ret);                
               // if (!item.CPF.IsBrCpf())
                 //   ret.AddResult(ErroSmartApi.CpfInvalido);
                //if (!ret.Ok()) return SmartRetornoImediato(ret);
               // if (!(Dados.ChecaCpfDuplicado(item.Ambiente, item.CPF)))
                //    ret.AddResult(ErroSmartApi.CpfNaoLocalizado);
               // if (!ret.Ok()) return SmartRetornoImediato(ret);


                bool rettemp = false;
                rettemp = Dados.EntradaNaoAutorizadaDTO(item);
                if (rettemp)
                    return SmartRetorno(new RetornoMensagem() { Result = "Registro atualizado com sucesso" });
                else
                    ret.AddResult(ErroSmartApi.NenhumaAcaoRealizada);

            }
            catch (Exception ex) { return SmartTrataErro(ex); }
            return SmartRetornoImediato(ret);
            //return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.EntradaNaoAutorizada(entradaNaoAutorizadaDTO));
        }

        [Authorize]
        [HttpPost]
        [Route("LiberarVeiculo")]
        public HttpResponseMessage LiberarBalanca([FromBody] LiberarBalancaDTO item)
        {
            Retorno ret = new Retorno();
            try
            {
                // Valida a existência da requisição:
                ret.AddResult(SmartValidaRequisicao(item));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida ambiente:
                ret.AddResult(SmartValidaAmbiente(item.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Valida parâmetros:
                ret.AddResult(SmartValidaParametrosLiberarBalanca(item));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar registro(s) para processar.
                List<LiberarBalancaDTO> Equiptos = new List<LiberarBalancaDTO>();
                Equiptos = Dados.GetEquipamentoEth03ByBalanca(item);
                if (Equiptos.Count == 0) ret.AddResult(ErroSmartApi.BalancaInvalido);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar registro(s) de motivação à processar:
                List<MotivacaoTemporaria> Motivacoes = new List<MotivacaoTemporaria>();
                Motivacoes = Dados.GetMotivacoesTemporarias(item.Ambiente, item.NumOs);
                if (Motivacoes.Count == 0) ret.AddResult(ErroSmartApi.NumOsNaoLocalizado);
                if (!ret.Ok()) return SmartRetornoImediato(ret);


                // Processar a requisição:
                List<dynamic> retornos = new List<dynamic>();
                LiberarBalancaDTO equipto = Equiptos[0];
                MotivacaoTemporaria motivacao = Motivacoes[0];                   
                using (HttpClient httpClient = new HttpClient())
                {
                    string requestUri = string.Format("{0}/api/Eth03/LiberarBalanca?sentido={1}&NumOs={2}&placa={3}&token=5a9ba14e-0aca-446c-987c-8697620d9755", 
                        equipto.UrlBaseService, 
                        item.Balanca.Substring(item.Balanca.Length - 1), 
                        item.NumOs, 
                        motivacao.Placa);
                    try
                    {
                        string result = httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex) 
                    {
                        string msg = ex.Message;
                        if (ex.InnerException != null)
                            msg += " " + ex.InnerException.Message;
                        ret.AddResult(9999, msg); return SmartRetornoImediato(ret);
                    }
                }
                retornos.Add((object)new
                {
                    StatusCode = 0,
                    StatusMessage = "Executado com sucesso",
                    Result = "Processo Iniciado com sucesso."
                });                   
                ret.Result.AddRange(retornos);
                //return (IHttpActionResult)this.Ok<object>(this.acessoBusiness.LiberarBalanca(liberarBalancaDTO));
            }
            catch (Exception ex) { return SmartTrataErro(ex); }
            return SmartRetornoImediato(ret);
        }

        //[HttpPost]
        //[Route("MotivarMotoristaArea")]
        //public HttpResponseMessage MotivarMotoristaArea(string sessao, int idCracha, string local, string ambiente, char sentido)
        //{
        //    Retorno ret = new Retorno();
        //    if (ConfigurationManager.AppSettings["QtdImgPorCaptura"] != null)
        //        ret.AddResult(99999, "Quantidade de imagens por captura não foi configurado.");
        //    else
        //    {
        //        int framesPorCamera = 0;
        //        if (int.TryParse(ConfigurationManager.AppSettings["QtdImgPorCaptura"], out framesPorCamera))
        //        {
        //            // Validar o ambiente.
        //            ret.AddResult(SmartValidaAmbiente(ambiente));
        //            if (!ret.Ok()) SmartRetornoImediato(ret);
        //            // Validar as cÂmeras do local:
        //            List<CameraEquipamento> cameras = Dados.ListaDeCamerasDoLocal(ambiente, local);
        //            if (cameras.Count == 0) ret.AddResult(ErroSmartApi.CamerasNaoCadastradas);
        //            if (!ret.Ok()) SmartRetornoImediato(ret);
        //            // Validar o crachá informado:
        //            ColaboradorViewModel colaborador = Dados.ColaboradorByIdCracha(ambiente, idCracha);
        //            if (colaborador == null) ret.AddResult(ErroSmartApi.ColaboradorNaoLocalizado);
        //            if (!ret.Ok()) SmartRetornoImediato(ret);
        //            // Valida o colaborador no BDCC:
        //            ret.AddResult(ValidaBDCCRepositorio.Add(new ValidaBDCCViewModel { ambiente = ambiente, cpf = colaborador.cpf, numDocumento = colaborador.cpf, tipoDocumento = "CPF" }));
        //            if (!ret.Ok()) return SmartRetornoImediato(ret);
        //            // Captura das imagens das câmeras do local:
        //            List<string> capturadas = CameraUtil.CapturaFrames(cameras, framesPorCamera);
        //            //Captura captura1 = new Captura();
        //            //captura1.Local = local;
        //            //captura1.Ambiente = ambiente;
        //            //Captura captura = captura1;
        //            //ReturnOcr retCapturaOcr = await this.MMA_CapturaORC(captura);
        //            int idSessao = 0;
        //            byte[] imagem = { 0 };
        //            int idLapTipoRequisicao = 0, nrPosicaoCameraTotem = 0, id = 0;
        //            float score = 0F;
        //            bool temp = Dados.InserirLapRequisicaoScore(ambiente, idSessao, imagem, idLapTipoRequisicao, nrPosicaoCameraTotem, score, id);
        //        }
        //        else
        //            ret.AddResult(99999, "Quantidade de capturas por câmera configurado erroneamente");
        //    }
        //    return SmartRetornoImediato(ret);
        //    //return (IHttpActionResult)this.Ok<object>(AcessoBusiness.MotivarMotoristaArea(sessao, idCracha, local, ambiente, sentido));
        //}

        //[Route("ValidaEntradapatioEM")]
        //public async Task<IHttpActionResult> ValidaEntradapatio(
        //  string placa,
        //  string sessao = "")
        //{
        //    return (IHttpActionResult)this.Ok<UnisolutionMotorista>(await new MotivarMotoristaAreaBO().MMA_ValidaEntradaPatio(placa, Local.Patio, sessao));
        //}
    }
}
