using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Web.Configuration;
using WebApiBusiness.App_Data;
using WebApiBusiness.Business;
using WebApiBusiness.Models;
using WebApiBusiness.Unisolution;
using WebApiBusiness.Util;

namespace WebApiBusiness.BO
{
    public class MotivarMotoristaAreaBO
    {
        private IntervencaoGuarda intervencaoGuarda = new IntervencaoGuarda();
        private Colaborador colaborador;

        public async Task<object> MotivarMotoristaArea(
          string sessao,
          int idCracha,
          string local,
          string Ambiente,
          char sentido)
        {
            Return retorno = new Return();
            LogBusiness.InserirLogApi(new LogApi()
            {
                idSessao = sessao,
                texto = "Inicio."
            });
            Return retValidaBDCC = this.MMA_ValidaBDCC("P", idCracha);
            if (retValidaBDCC.StatusCode == 1)
                this.intervencaoGuarda.CD_VCO_PESSOA += "907,";
            Colaborador Col = new ColaboradorModel().GetColaborador(idCracha, Ambiente);
            Captura captura1 = new Captura();
            captura1.Local = local;
            captura1.Ambiente = Ambiente;
            Captura captura = captura1;
            ReturnOcr retCapturaOcr = await this.MMA_CapturaORC(captura);
            this.intervencaoGuarda.ID_EQUIPAMENTO = captura.IdLocal;
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "MMA_CapturaORC",
                idSessao = sessao
            });
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "Status Code : " + retCapturaOcr.StatusCode.ToString(),
                idSessao = sessao
            });
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "IsPlate : " + retCapturaOcr.Placa.IsPlate().ToString(),
                idSessao = sessao
            });
            if (retCapturaOcr.StatusCode == 1 || !retCapturaOcr.Placa.IsPlate())
            {
                retorno.StatusCode = retorno.StatusCode == 1 ? retorno.StatusCode : 1;
                retorno.StatusMessage = "BLOQUEIO DE LAP";
                retorno.Result = (object)new
                {
                    ResultBdcc = retValidaBDCC.Result,
                    ResultOcr = retCapturaOcr.Result
                };
                if (Col != null)
                {
                    this.intervencaoGuarda.CD_VCO_PESSOA = "909";
                    this.intervencaoGuarda.Ambiente = Ambiente;
                    this.intervencaoGuarda.CD_CREDENCIAL_PESSOA = Col.CdCracha;
                    this.intervencaoGuarda.CD_SECAO = sessao;
                    this.intervencaoGuarda.CD_SENTIDO = sentido;
                    new IntervencaoGuardaBusiness().InserirIntervencaoGuarda(this.intervencaoGuarda);
                }
                if (retCapturaOcr.Image != null)
                    new AcessoBusiness().InserirLapRequisicaoScore(int.Parse(sessao), retCapturaOcr.Image, Ambiente);
                return (object)retorno;
            }
            UnisolutionMotorista unisolutionMotorista = await this.MMA_ValidaEntradaPatio(retCapturaOcr.Placa, Local.Area, sessao);
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "Status Code MMA_ValidaEntradaPatio : " + unisolutionMotorista.statusCode.ToString(),
                idSessao = sessao
            });
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "Status mESSAGE MMA_ValidaEntradaPatio : " + unisolutionMotorista.statusMessage.ToString(),
                idSessao = sessao
            });
            if (unisolutionMotorista.statusCode == 1)
            {
                retorno.StatusCode = retorno.StatusCode == 1 ? retorno.StatusCode : unisolutionMotorista.statusCode;
                retorno.StatusMessage = unisolutionMotorista.statusMessage.ToString();
                this.intervencaoGuarda.CD_VCO_PESSOA += "910,";
            }
            this.intervencaoGuarda.CD_PLACA_VEICULO = unisolutionMotorista.statusCode == 1 ? retCapturaOcr.Placa : this.intervencaoGuarda.CD_PLACA_VEICULO;
            retorno.Result = (object)new
            {
                ResultBdcc = retValidaBDCC.Result,
                ResultOcr = retCapturaOcr.Result,
                ResultEntradaPatio = new
                {
                    entradaPatio = (unisolutionMotorista.statusCode == 0),
                    message = (unisolutionMotorista.statusCode == 0 ? "ACESSO PATIO LIBERADO" : "ACESSO PATIO BLOQUEADO"),
                    ordemServico = unisolutionMotorista?.result?.numOs
                }
            };
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "retorno.StatusCode : " + (object)retorno.StatusCode,
                idSessao = sessao
            });
            if (retorno.StatusCode == 0)
            {
                try
                {
                    MotivacaoTemporaria motivacaoTemporaria1 = new MotivacaoTemporaria();
                    motivacaoTemporaria1.IdColaborador = Col.IdColaborador;
                    motivacaoTemporaria1.IdCracha = idCracha;
                    motivacaoTemporaria1.OrdemServico = unisolutionMotorista.result.numOs.ToString();
                    motivacaoTemporaria1.Placa = retCapturaOcr.Placa;
                    motivacaoTemporaria1.Ambiente = Ambiente;
                    MotivacaoTemporaria motivacaoTemporaria2 = motivacaoTemporaria1;
                    LogBusiness.InserirLogApi(new LogApi()
                    {
                        texto = "Col.IdColaborador : " + (object)Col.IdColaborador,
                        idSessao = sessao
                    });
                    MotivacaoTemporaria motivacaoTemporaria3 = new MotivacaoModel().ListarMotivacaoTemporaria(motivacaoTemporaria2);
                    if (motivacaoTemporaria3 == null)
                    {
                        new MotivacaoBusiness().InserirMotivacaoTemporaria(motivacaoTemporaria2);
                        retorno.IdMotivaCaoTemporaria = motivacaoTemporaria2.Id;
                    }
                    else
                        retorno.IdMotivaCaoTemporaria = motivacaoTemporaria3.Id;
                }
                catch (Exception ex)
                {
                    LogBusiness.InserirLogApi(new LogApi()
                    {
                        texto = "ex : " + ex.Message,
                        idSessao = sessao
                    });
                }
            }
            else if (this.colaborador != null && this.intervencaoGuarda.ID_EQUIPAMENTO > 0)
            {
                this.intervencaoGuarda.CD_VCO_PESSOA = this.intervencaoGuarda.CD_VCO_PESSOA.Remove(this.intervencaoGuarda.CD_VCO_PESSOA.Length - 1);
                this.intervencaoGuarda.Ambiente = Ambiente;
                this.intervencaoGuarda.CD_CREDENCIAL_PESSOA = this.colaborador.CdCracha;
                this.intervencaoGuarda.CD_SECAO = sessao;
                this.intervencaoGuarda.CD_SENTIDO = sentido;
                this.intervencaoGuarda.OrdemServico = unisolutionMotorista?.result?.numOs.ToString();
                new IntervencaoGuardaBusiness().InserirIntervencaoGuarda(this.intervencaoGuarda);
            }
            if (retCapturaOcr.Image != null && this.intervencaoGuarda.ID_EQUIPAMENTO > 0)
                new AcessoBusiness().InserirLapRequisicaoScore(int.Parse(sessao), retCapturaOcr.Image, Ambiente);
            return (object)retorno;
        }

        public Return MMA_ValidaBDCC(string ambiente, int idCracha)
        {
            try
            {
                this.colaborador = new ColaboradorBusiness().GetColaborador(idCracha, ambiente);
                if (this.colaborador == null)
                    return new Return()
                    {
                        StatusCode = 1,
                        StatusMessage = "ERRO NA REQUISIÇÃO",
                        Result = (object)new
                        {
                            cadastroMotorista = false,
                            bdcc = false,
                            message = "MOTORISTA/CPF SEM CADASTRO VALIDO"
                        }
                    };
                if (JsonConvert.DeserializeObject<RetornoApiIntegrada>(new AcessoBusiness().ValidaBDCC(new Acesso()
                {
                    Ambiente = ambiente,
                    cpf = this.colaborador.CdCpf
                })).StatusCode == 1)
                    return new Return()
                    {
                        StatusCode = 1,
                        StatusMessage = "BLOQUEADO NO BDCC",
                        Result = (object)new
                        {
                            cadastroMotorista = true,
                            bdcc = false,
                            message = "CPF BLOQUEADO NO BDCC"
                        }
                    };
                return new Return()
                {
                    StatusCode = 0,
                    StatusMessage = "LIBERADO NO BDCC",
                    Result = (object)new
                    {
                        cadastroMotorista = true,
                        bdcc = true,
                        message = "CPF LIBERADO NO BDCC"
                    }
                };
            }
            catch (Exception ex)
            {
                SimpleLog.Error(string.Format("MMA_ValidaBDCC() ERRO NÃO MAPEADO: {0}.", (object)ex.ToString()), true);
                return new Return()
                {
                    StatusCode = 1,
                    StatusMessage = "ERRO NA REQUISIÇÃO",
                    Result = (object)new
                    {
                        cadastroMotorista = false,
                        bdcc = false,
                        message = string.Format("ERRO NÃO MAPEADO {0}", (object)ex.ToString())
                    }
                };
            }
        }

        public async Task<ReturnOcr> MMA_CapturaORC(Captura captura)
        {
            try
            {
                Captura captura1 = new CapturaBusiness().CapturaOcr(captura);
                if (captura1.StatusCode == 0)
                {
                    this.intervencaoGuarda.IdCapturaOcr = captura1.IdOCR;
                    return new ReturnOcr()
                    {
                        StatusCode = captura.StatusCode,
                        StatusMessage = "Processo executado com sucesso",
                        IdCapturaImagemOcr = captura1.IdCapturaImagemOcr,
                        Placa = captura.TextoPlacaCarmen,
                        Image = captura.Image,
                        Result = (object)new
                        {
                            placa = captura.TextoPlacaCarmen,
                            Score = captura.Score,
                            IdOCR = captura.IdOCR,
                            ocr = captura.TextoPlacaCarmen.IsPlate(),
                            message = (captura.TextoPlacaCarmen.IsPlate() ? "PLACA ENCONTRADA" : "PLACA NÃO ENCONTRADA"),
                            Imagem = string.Format("/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={0}&idOcr={1}&ambiente={2}", (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                        }
                    };
                }
                if (captura1.Code.Equals("0009"))
                {
                    this.intervencaoGuarda.IdCapturaOcr = captura1.IdOCR;
                    this.intervencaoGuarda.CD_VCO_PESSOA += "909,";
                    return new ReturnOcr()
                    {
                        StatusCode = captura.StatusCode,
                        StatusMessage = "Erro(s) encontrado(s)",
                        IdCapturaImagemOcr = captura1.IdCapturaImagemOcr,
                        Placa = captura.TextoPlacaCarmen,
                        Image = captura.Image,
                        Result = (object)new
                        {
                            placa = captura.TextoPlacaCarmen,
                            Score = captura.Score,
                            IdOCR = captura.IdOCR,
                            ocr = false,
                            message = "PLACA NÃO ENCONTRADA",
                            Imagem = string.Format("/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={0}&idOcr={1}&ambiente={2}", (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                        }
                    };
                }
                ReturnOcr returnOcr;
                if (captura != null)
                {
                    this.intervencaoGuarda.CD_VCO_PESSOA += "908,";
                    returnOcr = new ReturnOcr()
                    {
                        StatusCode = captura.StatusCode,
                        IdCapturaImagemOcr = captura1.IdCapturaImagemOcr,
                        Placa = captura.TextoPlacaCarmen,
                        StatusMessage = "Erro(s) encontrado(s)",
                        Result = (object)new
                        {
                            ocr = false,
                            message = captura.Message
                        }
                    };
                }
                else
                {
                    this.intervencaoGuarda.CD_VCO_PESSOA += "908,";
                    returnOcr = new ReturnOcr()
                    {
                        StatusCode = captura1.StatusCode,
                        IdCapturaImagemOcr = captura1.IdCapturaImagemOcr,
                        Placa = captura.TextoPlacaCarmen,
                        StatusMessage = "Erro(s) encontrado(s)",
                        Result = (object)new
                        {
                            ocr = false,
                            message = captura.Message
                        }
                    };
                }
                return returnOcr;
            }
            catch (Exception ex)
            {
                SimpleLog.Error(string.Format("MMA_CapturaORC() ERRO NÃO MAPEADO: {0}.", (object)ex.ToString()), true);
                return new ReturnOcr()
                {
                    StatusCode = 1,
                    StatusMessage = "Erro(s) encontrado(s)",
                    IdCapturaImagemOcr = 0,
                    Placa = ""
                };
            }
        }

        public async Task<UnisolutionMotorista> MMA_ValidaEntradaPatio(
          string placa,
          Local local,
          string sessao = "0")
        {
            SimpleLog.Info("CHAMOU MMA_ValidaEntradaPatio", true);
            string appSetting1 = WebConfigurationManager.AppSettings["UnisolutionLogin"];
            string appSetting2 = WebConfigurationManager.AppSettings["UnisolutionPassword"];
            string appSetting3 = WebConfigurationManager.AppSettings["UnisolutionUrlToken"];
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "urlToken : " + appSetting3,
                idSessao = sessao
            });
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "userName : " + appSetting1,
                idSessao = sessao
            });
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "password : " + appSetting2,
                idSessao = sessao
            });
            string urlValidaEntradaPatio = WebConfigurationManager.AppSettings["UnisolutionUrlValidaEntradaPatio"];
            string filial = WebConfigurationManager.AppSettings["UnisolutionFilial"];
            string token = UnisolutionService.GetToken(appSetting1, appSetting2, appSetting3);
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "token : " + token,
                idSessao = sessao
            });
            if (token != null)
            {
                string appSetting4 = WebConfigurationManager.AppSettings["UnisolutionVersion"];
                UnisolutionMotorista unisolutionMotorista1 = new UnisolutionMotorista();
                LogBusiness.InserirLogApi(new LogApi()
                {
                    texto = "versaoUnisolution : " + appSetting4,
                    idSessao = sessao
                });
                LogBusiness.InserirLogApi(new LogApi()
                {
                    texto = "url : " + urlValidaEntradaPatio + " - " + (object)local + " - " + filial,
                    idSessao = sessao
                });
                UnisolutionMotorista unisolutionMotorista2;
                if (appSetting4 == "1")
                    unisolutionMotorista2 = await UnisolutionService.ValidaEntradaPatioV1(placa, token, urlValidaEntradaPatio, local, filial);
                else
                    unisolutionMotorista2 = UnisolutionService.ValidaEntradaPatio(placa, token, urlValidaEntradaPatio, local, filial, sessao);
                return unisolutionMotorista2;
            }
            return new UnisolutionMotorista()
            {
                statusCode = 1,
                statusMessage = "Erro de autenticação no webService"
            };
        }
    }
}