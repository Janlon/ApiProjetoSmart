using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WebApiBusiness.App_Data;
using WebApiBusiness.BO;
using WebApiBusiness.Business;
using WebApiBusiness.Models;
using WebApiBusiness.Unisolution;
using WebApiBusiness.Util;

namespace SmartAPIIntegradorFase1.Controllers
{
    [RoutePrefix("api/Ocr")]
    public class CapturaOcrController : ApiControllerBase
    {
        private CapturaBusiness capturaB = new CapturaBusiness();

        [Authorize]
        [HttpPost]
        [Route("CapturaOCR")]
        public async Task<IHttpActionResult> CapturaOcr([FromBody] Captura captura)
        {
            CapturaOcrController capturaOcrController = this;

            if (captura == null) 
            {
                // Se a requisição chegou sem nada, 
                // tenta agora, que ao menos tem um dynamic
                var ret = new 
                {
                    StatusCode = 1,
                    StatusMessage = "Erro(s) encontrado(s)",
                    Result = new
                    {
                        Code = 36,
                        Message = "Requisição sem dados"
                    }
                };
                return (IHttpActionResult)Ok<dynamic>(ret);
            }
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), nameof(CapturaOcr));
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = ">Captura OCR : " + (object)captura,
                idSessao = captura.IdSecao.ToString()
            });
            Captura captura1 = capturaOcrController.capturaB.CapturaOcr(captura);
            if (captura1.StatusCode == 0)
            {
                var content = new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Processo executado com sucesso",
                    Result = new
                    {
                        Placa = captura.TextoPlacaCarmen,
                        Score = captura.Score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}", (object)capturaOcrController.Request.RequestUri.GetLeftPart(UriPartial.Authority), (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                    }
                };
                LogBusiness.InserirLogApi(new LogApi()
                {
                    texto = "> Retorno Ok: " + captura.TextoPlacaCarmen + " " + (object)captura.Score + " " + (object)captura.IdOCR,
                    idSessao = captura.IdSecao.ToString()
                });
                return (IHttpActionResult)capturaOcrController.Ok(content);
            }
            if (captura1.Code.Equals("0009"))
            {
                var content = new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Erro(s) encontrado(s)",
                    Result = new
                    {
                        Placa = captura.TextoPlacaCarmen,
                        Score = captura.Score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}", (object)capturaOcrController.Request.RequestUri.GetLeftPart(UriPartial.Authority), (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                    }
                };
                LogBusiness.InserirLogApi(new LogApi()
                {
                    texto = "> Retorno Erro : " + captura.TextoPlacaCarmen + " " + (object)captura.Score + " " + (object)captura.IdOCR,
                    idSessao = captura.IdSecao.ToString()
                });
                return (IHttpActionResult)capturaOcrController.Ok(content);
            }
            object obj = captura == null ? (object)new
            {
                StatusCode = captura1.StatusCode,
                StatusMessage = "Erro(s) encontrado(s)",
                Result = new
                {
                    Code = captura1.Code,
                    Message = captura1.Message
                }
            } : (object)new
            {
                StatusCode = captura.StatusCode,
                StatusMessage = "Erro(s) encontrado(s)",
                Result = new
                {
                    Code = captura.Code,
                    Message = captura.Message
                }
            };
            LogBusiness.InserirLogApi(new LogApi()
            {
                texto = "> Retorno Bad Request  " + (object)captura,
                idSessao = captura.IdSecao.ToString()
            });
            return capturaOcrController.CustomBadRequest<object>(obj);
        }

        [HttpPost]
        [Route("CapturaOcrEM")]
        public async Task<IHttpActionResult> CapturaOcrEventManager([FromBody] Captura captura)
        {
            CapturaOcrController capturaOcrController = this;
            int IdSecao = captura.IdSecao;
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), "CapturaOcrEM");
            Captura captura1 = capturaOcrController.capturaB.CapturaOcr(captura);
            if (captura1.StatusCode == 0)
            {
                var content = new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Processo executado com sucesso",
                    Result = new
                    {
                        Placa = captura.TextoPlacaCarmen,
                        Score = captura.Score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}", (object)capturaOcrController.Request.RequestUri.GetLeftPart(UriPartial.Authority), (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                    }
                };
                if (IdSecao > 0)
                    new AcessoBusiness().InserirLapRequisicaoScore(IdSecao, captura.Image, captura.Ambiente);
                return (IHttpActionResult)capturaOcrController.Ok(content);
            }
            if (captura1.Code.Equals("0009"))
            {
                var content = new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Erro(s) encontrado(s)",
                    Result = new
                    {
                        Placa = captura.TextoPlacaCarmen,
                        Score = captura.Score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}", (object)capturaOcrController.Request.RequestUri.GetLeftPart(UriPartial.Authority), (object)captura.IdCapturaImagemOcr, (object)captura.IdOCR, (object)captura.Ambiente)
                    }
                };
                return (IHttpActionResult)capturaOcrController.Ok(content);
            }
            object obj = captura == null ? (object)new
            {
                StatusCode = captura1.StatusCode,
                StatusMessage = "Erro(s) encontrado(s)",
                Result = new
                {
                    Code = captura1.Code,
                    Message = captura1.Message
                }
            } : (object)new
            {
                StatusCode = captura.StatusCode,
                StatusMessage = "Erro(s) encontrado(s)",
                Result = new
                {
                    Code = captura.Code,
                    Message = captura.Message
                }
            };
            return capturaOcrController.CustomBadRequest<object>(obj);
        }

        [Authorize]
        [HttpPost]
        [Route("RecuperaOCR")]
        public IHttpActionResult RecuperaOCR([FromBody] Captura captura)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), nameof(RecuperaOCR));
            Captura captura1 = new Captura();
            object obj;
            try
            {
                Captura captura2 = this.capturaB.RecuperaOcr(captura);
                if (captura2 == null && captura.StatusCode == 1)
                {
                    obj = (object)new
                    {
                        StatusCode = captura.StatusCode,
                        StatusMessage = "Erro encontrado",
                        Result = new
                        {
                            Code = "0006",
                            Message = "IdOcr não encontrado"
                        }
                    };
                }
                else
                {
                    if (captura2.StatusCode == 0)
                        return (IHttpActionResult)this.Ok<object>((object)new
                        {
                            StatusCode = 0,
                            StatusMessage = "Processo executado com sucesso",
                            Result = new
                            {
                                Placa = captura2.TextoPlacaCarmen,
                                Score = captura2.Score,
                                DataHora = captura2.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                                IdOCR = captura2.IdOCR,
                                Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}", (object)this.Request.RequestUri.GetLeftPart(UriPartial.Authority), (object)captura2.IdCapturaImagemOcr, (object)captura2.IdOCR, (object)captura.Ambiente)
                            }
                        });
                    obj = (object)new
                    {
                        StatusCode = captura.StatusCode,
                        StatusMessage = "Erro(s) encontrado(s)",
                        Result = new
                        {
                            Code = captura.Code,
                            Message = captura.Message
                        }
                    };
                }
            }
            catch (SqlException ex)
            {
                return (IHttpActionResult)this.BadRequest(JsonConvert.SerializeObject((object)new
                {
                    StatusCode = 1,
                    StatusMessage = "Erro(s) encontrado(s)",
                    Result = new
                    {
                        Code = "0098",
                        Message = "Falha de conexão com base de dados"
                    }
                }).Replace("\\", "").Replace("\"", ""));
            }
            return this.CustomBadRequest<object>(obj);
        }

        [Authorize]
        [HttpPost]
        [Route("ConfirmaOCR")]
        public IHttpActionResult ConfirmaOCR([FromBody] Captura captura)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), nameof(ConfirmaOCR));
            Captura captura1;
            try
            {
                captura1 = this.capturaB.ConfirmaOcr(captura);
            }
            catch (SqlException ex)
            {
                return (IHttpActionResult)this.BadRequest(JsonConvert.SerializeObject((object)new
                {
                    StatusCode = 1,
                    StatusMessage = "Erro(s) encontrado(s)",
                    Result = new
                    {
                        Code = "0098",
                        Message = "Falha de conexão com base de dados"
                    }
                }).Replace("\\", "").Replace("\"", ""));
            }
            object content;
            if (captura != null)
            {
                if (captura1.StatusCode == 1)
                    return this.CustomBadRequest<object>((object)new
                    {
                        StatusCode = captura1.StatusCode,
                        StatusMessage = "Erro(s) encontrado(s)",
                        Result = new
                        {
                            Code = captura1.Code,
                            Message = captura1.Message
                        }
                    });
                content = (object)new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Processo executado com sucesso",
                    Result = new { Placa = captura.Placa }
                };
            }
            else
            {
                if (captura1.StatusCode == 1)
                    return this.CustomBadRequest<object>((object)new
                    {
                        StatusCode = captura1.StatusCode,
                        StatusMessage = "Erro(s) encontrado(s)",
                        Result = new
                        {
                            Code = captura1.Code,
                            Message = captura1.Message
                        }
                    });
                content = (object)new
                {
                    StatusCode = captura.StatusCode,
                    StatusMessage = "Processo executado com sucesso",
                    Result = new { Placa = captura.Placa }
                };
            }
            return (IHttpActionResult)this.Ok<object>(content);
        }

        [Authorize]
        [HttpPost]
        [Route("PesquisaMotivacao")]
        public IHttpActionResult PesquisaMotivacao([FromBody] Captura captura)
        {
            return (IHttpActionResult)this.Ok<MotivacaoTemporaria>(this.capturaB.PesquisarMotivacaoBusiness(captura));
        }

        [HttpGet]
        [Route("ImagemCapturada")]
        public HttpResponseMessage GetImagemCapturada(
          int idCapturaImagemOcr,
          int idOcr,
          string ambiente)
        {
            MemoryStream memoryStream = new MemoryStream(this.capturaB.SelecionarImageCapturaOcrBusiness(idCapturaImagemOcr, idOcr, ambiente).Image);
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = (HttpContent)new StreamContent((Stream)memoryStream)
            };
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return httpResponseMessage;
        }
    }
}
