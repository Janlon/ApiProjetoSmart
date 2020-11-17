using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using WEBAPI_VOPAK.Models;
using WEBAPI_VOPAK.Models.Componentes;

namespace WEBAPI_VOPAK.Controllers
{
 
    [RoutePrefix("api/Ocr")]
    public class CapturaOcrController : SmartAPIController
    {
        [Authorize]
        [HttpPost]
        [Route("PesquisaMotivacao")]
        public IHttpActionResult PesquisaMotivacao([FromBody] Captura captura)
        {
            var mt = Dados.GetMotivacoesTemporarias(captura.Ambiente, captura.NumOs.ToString()).FirstOrDefault();
            return (IHttpActionResult)this.Ok<MotivacaoTemporaria>(mt);
        }

        [HttpGet]
        [Route("ImagemCapturada")]
        public HttpResponseMessage GetImagemCapturada(int idCapturaImagemOcr, int idOcr, string ambiente)
        {
            byte[] ret = Dados.SelecionaImagemCapturada(ambiente, idCapturaImagemOcr, idOcr);
            if (ret == null)
                ret = CameraUtil.SemImagemBytes;
            if (ret.Length == 0)
                ret = CameraUtil.SemImagemBytes;
            MemoryStream ms = new MemoryStream(ret);
            HttpResponseMessage retorno = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = (HttpContent)new StreamContent((Stream)ms) };
            retorno.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return retorno;
        }


        /// <summary>
        /// Este método grava nas TB_CapturaOCR e TB_CapturaImagemOCR as imagens solicitadas.
        /// </summary>
        /// <param name="captura"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("CapturaOCR")]
        public HttpResponseMessage CapturaOcr([FromBody] Captura captura)
        {
            Retorno ret = new Retorno();
            int framesPorCamera = 1;
            try
            {
                // Validar a requisição:
                ret.AddResult(SmartValidaRequisicao(captura));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o ambiente:
                ret.AddResult(SmartValidaAmbiente(captura.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar a motivação:
                List<MotivacaoTemporaria> tmp = Dados.GetMotivacoesTemporarias(captura.Ambiente, captura.NumOs.ToString());
                if (tmp.Count == 0)
                    ret.AddResult(99999, "Parâmetro numOs não encontrado");
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar as câmeras:
                List<CameraEquipamento> cameras = Dados.ListaDeCamerasDoLocal(captura.Ambiente, captura.Local);
                if (tmp.Count == 0)
                    ret.AddResult(ErroSmartApi.CamerasNaoCadastradas);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Imagens por câmera, padrão 1 se não encontrar nas configurações.
                if (ConfigurationManager.AppSettings["QtdImgPorCaptura"] != null)
                    if (!int.TryParse(ConfigurationManager.AppSettings["QtdImgPorCaptura"], out framesPorCamera))
                        framesPorCamera = 1;

                // Capturar as imagens: Se não capturar nada, retorna.
                List<string> stringList = CameraUtil.CapturaFrames(cameras, framesPorCamera);
                if (stringList.Count == 0)
                    ret.AddResult(99999, "Nenhuma imagem capturada");
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Carmen, ou seja, a resposta do serviço do Carmen, apenas aplicando o Leveinshtein à placa detectada.
                Carmen carmem = CameraUtil.GetCarmenLevenstein(captura.Ambiente, stringList, captura);
                if (carmem == null)
                {
                    // Não tem "carmem".
                    ret.AddResult(new
                    {
                        tmp[0].Placa,
                        Score = 0F,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = 0,
                        Imagem = ""
                    });
                }
                else
                {
                    // "captura" é a requisição em sí, e portanto não é adicionado ao retorno.
                    // É usado aqui apenas para manter informações sobre os dados gravados.
                    captura.IdOCR = Dados.GravaOcr(captura);
                    captura.IdCapturaImagemOcr = Dados.GravaImagem(captura);
                    captura.DataCadastro = Dados.RecuperaDataDaOcr(captura.Ambiente, captura.IdOCR);
                    ret.AddResult(new
                    {
                        Placa = carmem.plate,
                        Score = carmem.score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}",
                                Request.RequestUri.GetLeftPart(UriPartial.Authority),
                                captura.IdCapturaImagemOcr,
                                captura.IdOCR,
                                captura.Ambiente)
                    });
                }
            }
            catch (Exception ex) { ret.AddResult(SmartTrataErro(ex)); }
            return SmartRetornoImediato(ret);
        }


        /// <summary>
        /// Este método grava na TB_LAP_REQUISICAO_SCORE as imagens solicitada.
        /// </summary>
        /// <param name="captura"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CapturaOcrEM")]
        public HttpResponseMessage CapturaOcrEventManager([FromBody] Captura captura)
        {
            Retorno ret = new Retorno();
            int framesPorCamera = 0;
            try
            {
                // Validar a requisição:
                ret.AddResult(SmartValidaRequisicao(captura));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o ambiente:
                ret.AddResult(SmartValidaAmbiente(captura.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar a motivação:
                List<MotivacaoTemporaria> tmp = Dados.GetMotivacoesTemporarias(captura.Ambiente, captura.NumOs.ToString());
                if (tmp.Count == 0)
                    ret.AddResult(99999, "Parâmetro numOs não encontrado");
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar as câmeras:
                List<CameraEquipamento> cameras = Dados.ListaDeCamerasDoLocal(captura.Ambiente, captura.Local);
                if (tmp.Count == 0)
                    ret.AddResult(ErroSmartApi.CamerasNaoCadastradas);
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Imagens por câmera, padrão 1 se não encontrar nas configurações.
                if (ConfigurationManager.AppSettings["QtdImgPorCaptura"] != null)
                    if (!int.TryParse(ConfigurationManager.AppSettings["QtdImgPorCaptura"], out framesPorCamera))
                        framesPorCamera = 1;

                // Capturar as imagens:
                List<string> stringList = CameraUtil.CapturaFrames(cameras, framesPorCamera);
                if (stringList.Count == 0)
                    ret.AddResult(99999, "Nenhuma imagem capturada");
                if (!ret.Ok()) return SmartRetornoImediato(ret);
                Carmen carmen = CameraUtil.GetCarmenLevenstein(captura.Ambiente, stringList, captura);
                if (carmen == null)
                {
                    ret.AddResult(new
                    {
                        tmp[0].Placa,
                        Score = 0F,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = 0,
                        Imagem = ""
                    });
                }
                else
                {
                    captura.IdOCR = Dados.GravaOcr(captura);
                    captura.IdCapturaImagemOcr = Dados.GravaImagem(captura);
                    captura.DataCadastro = Dados.RecuperaDataDaOcr(captura.Ambiente, captura.IdOCR);
                    if (captura.IdSecao > 0)
                        Dados.InserirLapRequisicaoScore(captura.Ambiente, captura.IdSecao, captura.Image, (int)TipoDeRequisicao.LAP, (int)PosicaoDaCamera.Top, carmen.score, captura.IdOCR);
                    ret.AddResult(new
                    {
                        Placa = carmen.plate,
                        Score = carmen.score,
                        DataHora = captura.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        captura.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}",
                                            Request.RequestUri.GetLeftPart(UriPartial.Authority),
                                            captura.IdCapturaImagemOcr,
                                            captura.IdOCR,
                                            captura.Ambiente),

                    });
                }
            }
            catch (Exception ex) { ret.AddResult(SmartTrataErro(ex)); }
            return SmartRetornoImediato(ret);
        }


        [Authorize]
        [HttpPost]
        [Route("RecuperaOCR")]
        public HttpResponseMessage RecuperaOCR([FromBody] Captura captura)
        {
            Retorno ret = new Retorno();
            try
            {
                // Validar a requisição:
                ret.AddResult(SmartValidaRequisicao(captura));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o ambiente:
                ret.AddResult(SmartValidaAmbiente(captura.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                Captura cap = Dados.RecuperaOcr(captura.Ambiente, captura.IdOCR);
                if (cap == null && cap.StatusCode == 1)
                    ret.AddResult(9999, "IdOcr não encontrado");
                else
                {
                    ret.AddResult(new
                    {
                        Placa = cap.TextoPlacaCarmen,
                        Score = cap.Score,
                        DataHora = cap.DataCadastro.ToString("dd/MM/yyyy HH:mm:ss"),
                        IdOCR = cap.IdOCR,
                        Imagem = string.Format("{0}/api/Ocr/ImagemCapturada?IdCapturaImagemOcr={1}&idOcr={2}&ambiente={3}",
                                            Request.RequestUri.GetLeftPart(UriPartial.Authority),
                                            cap.IdCapturaImagemOcr,
                                            cap.IdOCR,
                                            cap.Ambiente),
                    });
                }
            }
            catch (Exception ex) { ret.AddResult(SmartTrataErro(ex)); }
            return SmartRetornoImediato(ret);
        }


        [Authorize]
        [HttpPost]
        [Route("ConfirmaOCR")]
        public HttpResponseMessage ConfirmaOCR([FromBody] Captura captura)
        {
            Retorno ret = new Retorno();
            try
            {
                // Validar a requisição:
                ret.AddResult(SmartValidaRequisicao(captura));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                // Validar o ambiente:
                ret.AddResult(SmartValidaAmbiente(captura.Ambiente));
                if (!ret.Ok()) return SmartRetornoImediato(ret);

                if (captura == null)
                {
                    ret.AddResult(999, "Requisição sem dados");
                    return SmartRetornoImediato(ret);
                }


                if (captura.IdOCR == 0)
                {
                    ret.AddResult(999, "Parâmetro IdOCR obrigatório");
                    return SmartRetornoImediato(ret);
                }

                if (captura.Placa == "")
                {
                    ret.AddResult(999, "Parâmetro placa obrigatório");
                    return SmartRetornoImediato(ret);
                }

                Captura cap = Dados.ConfirmaOcr(captura.Ambiente, captura.IdOCR);

                if (cap.Score >= 100)
                    ret.AddResult(999, "Placa já está com 100% de confirmação");

                if (cap.IdOCR == 0)
                    ret.AddResult(999, "Parâmetro IdOcr não encontrado");

                if (cap.StatusCode == 0)
                {
                    cap = Dados.ConfirmaOcr(captura.Ambiente, captura.IdOCR, captura.Placa, 100);
                    if (cap.StatusCode != 0)
                        ret.AddResult(999, "Registro não pode ser atualizado");
                }

                if (cap.StatusCode == 1)
                    ret.AddResult(new { Placa = cap.Placa });

            }
            catch (Exception ex) { ret.AddResult(SmartTrataErro(ex)); }
            return SmartRetornoImediato(ret);
        }
    }
}
