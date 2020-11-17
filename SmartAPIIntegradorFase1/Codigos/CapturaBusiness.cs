using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebApiBusiness.App_Data;
using WebApiBusiness.Models;

namespace WebApiBusiness.Business
{
    public class CapturaBusiness
    {
        public CapturaOcrModel CapturaOcrModel { get; set; }

        public RecuperaOcrModel RecuperaOcrModel { get; set; }

        public CapturaBusiness()
        {
            this.CapturaOcrModel = new CapturaOcrModel();
            this.RecuperaOcrModel = new RecuperaOcrModel();
        }

        public MotivacaoTemporaria PesquisarMotivacaoBusiness(Captura captura)
        {
            try
            {
                if (string.IsNullOrEmpty(captura.NumOs.ToString()))
                    return (MotivacaoTemporaria)null;
                return this.CapturaOcrModel.SelecionarMotivacao(captura);
            }
            catch (Exception)
            {
                return (MotivacaoTemporaria)null;
            }
        }

        public Captura CapturaOcr(Captura captura)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), nameof(CapturaOcr));
            List<CameraEquipamento> cameraEquipamentoList = new List<CameraEquipamento>();
            List<CameraEquipamento> camerasEquipamento;
            try
            {
                if (captura == null)
                {
                    captura = new Captura();
                    captura.StatusCode = 1;
                    captura.Message = "Erro não especificado";
                    captura.Code = "0099";
                    return captura;
                }
                if (string.IsNullOrEmpty(captura.Ambiente) || !captura.Ambiente.ToUpper().Equals("P") && !captura.Ambiente.ToUpper().Equals("T"))
                {
                    captura.StatusCode = 1;
                    captura.Message = "Parâmetro “ambiente” deve ser preenchido com T ou P";
                    captura.Code = "0002";
                    return captura;
                }
                Equipamento equipamento = this.CapturaOcrModel.SelecionarEquipamento(captura);
                if (equipamento == null)
                {
                    captura.StatusCode = 1;
                    captura.Message = "Parâmetro local não encontrado";
                    captura.Code = "0006";
                    return captura;
                }
                captura.IdLocal = equipamento.IdEquipamento;
                if (captura.NumOs != "")
                {
                    MotivacaoTemporaria motivacaoTemporaria = this.CapturaOcrModel.SelecionarMotivacao(captura);
                    if (motivacaoTemporaria == null)
                    {
                        captura.StatusCode = 1;
                        captura.Message = "Parâmetro numOs não encontrado";
                        captura.Code = "0010";
                        return captura;
                    }
                }
                camerasEquipamento = this.CapturaOcrModel.GetCamerasEquipamento(captura);
                if (!camerasEquipamento.Any<CameraEquipamento>())
                {
                    captura.StatusCode = 1;
                    captura.Message = "Câmeras não cadastradas para o local informado";
                    captura.Code = "0007";
                    return captura;
                }
            }
            catch (SqlException ex)
            {
                captura.StatusCode = 1;
                captura.Message = "Falha de conexão com base de dados";
                captura.Code = "0098";
                return captura;
            }
            catch (Exception ex)
            {
                ex.InnerException?.ToString();
                captura.StatusCode = 1;
                captura.Message = "Erro não especificado";
                captura.Code = "0099";
                return captura;
            }
            List<string> stringList = new List<string>();
            int framesPorCamera = int.Parse(ConfigurationManager.AppSettings["QtdImgPorCaptura"]);
            CameraUtil.CapturaFrames(camerasEquipamento, framesPorCamera, stringList);
            if (!stringList.Any<string>())
            {
                captura.StatusCode = 1;
                captura.Message = "Falha de conexão com a(s) câmera(s)";
                captura.Code = "0008";
                return captura;
            }
            Carmen carmenLevenstein = CameraUtil.GetCarmenLevenstein(stringList, captura);
            if (carmenLevenstein == null)
            {
                captura.TextoPlacaCarmen = "";
                captura.Placa = "";
                captura.Score = 0;
                captura.Image = Convert.FromBase64String(stringList[stringList.Count - 1]);
                this.CapturaOcrModel.GravaOcr(captura);
                this.CapturaOcrModel.GravaImagem(captura);
                captura.DataCadastro = this.RecuperaOcrModel.RecuperaOcr(captura).DataCadastro;
                captura.Message = "Placa não encontrada";
                captura.Code = "0009";
                return captura;
            }
            captura.TextoPlacaCarmen = carmenLevenstein.plate;
            captura.Placa = carmenLevenstein.ConfirmedPlate;
            captura.Score = (int)carmenLevenstein.score;
            captura.Image = Convert.FromBase64String(carmenLevenstein.base64);
            this.CapturaOcrModel.GravaOcr(captura);
            this.CapturaOcrModel.GravaImagem(captura);
            captura.DataCadastro = this.RecuperaOcrModel.RecuperaOcr(captura).DataCadastro;
            return captura;
        }

        public Captura SelecionarImageCapturaOcrBusiness(
          int idCapturaImagemOcr,
          int idCapturaOcr,
          string ambiente)
        {
            return this.CapturaOcrModel.SelecionarImageCapturaOcr(idCapturaImagemOcr, idCapturaOcr, ambiente);
        }

        public Captura RecuperaOcr(Captura captura)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), MethodBase.GetCurrentMethod().Name);
            RecuperaOcrModel recuperaOcrModel = new RecuperaOcrModel();
            try
            {
                if (captura == null)
                {
                    captura = new Captura();
                    captura.StatusCode = 1;
                    captura.Message = "Erro não especificado";
                    captura.Code = "0099";
                    return captura;
                }
                if (!captura.Ambiente.ToUpper().Equals("P") && !captura.Ambiente.ToUpper().Equals("T"))
                {
                    captura.StatusCode = 1;
                    captura.Message = "Parâmetro “ambiente” deve ser preenchido com T ou P";
                    captura.Code = "0002";
                    return captura;
                }
                int idOcr = captura.IdOCR;
                if (captura.IdOCR != 0)
                    return recuperaOcrModel.RecuperaOcr(captura);
                captura.StatusCode = 1;
                captura.Message = "Parâmetro IdOcr obrigatório";
                captura.Code = "0002";
                return captura;
            }
            catch (Exception ex)
            {
                captura.StatusCode = 1;
                captura.Code = "0099";
                captura.Message = "Erro não especificado";
                return captura;
            }
        }

        public Captura ConfirmaOcr(Captura captura)
        {
            LogBusiness.InserirLog(JsonConvert.SerializeObject((object)captura), MethodBase.GetCurrentMethod().Name);
            ConfirmaOcrModel confirmaOcrModel = new ConfirmaOcrModel();
            try
            {
                if (captura == null)
                {
                    captura = new Captura();
                    captura.StatusCode = 1;
                    captura.Message = "Erro não especificado";
                    captura.Code = "0099";
                    return captura;
                }
                if (!captura.Ambiente.ToUpper().Equals("P") && !captura.Ambiente.ToUpper().Equals("T"))
                {
                    captura.StatusCode = 1;
                    captura.Message = "Parâmetro “ambiente” deve ser preenchido com T ou P";
                    captura.Code = "0002";
                    return captura;
                }
                Captura captura1 = this.RecuperaOcrModel.RecuperaOcr(captura);
                if (captura1 == null)
                {
                    captura.StatusCode = 1;
                    captura.Code = "0006";
                    captura.Message = "Parâmetro IdOcr não encontrado";
                    return captura;
                }
                if (captura1.Score >= 100)
                {
                    captura.StatusCode = 1;
                    captura.Code = "0011";
                    captura.Message = "Placa já está com 100% de confirmação";
                    return captura;
                }
                if (string.IsNullOrEmpty(captura.Placa))
                {
                    captura.StatusCode = 1;
                    captura.Code = "0007";
                    captura.Message = "Parâmetro placa obrigatório";
                    return captura;
                }
                if (captura.IdOCR == 0)
                {
                    captura.StatusCode = 1;
                    captura.Code = "0005";
                    captura.Message = "Parâmetro IdOCR obrigatório";
                    return captura;
                }
                if (captura.StatusCode != 1)
                    return confirmaOcrModel.ConfirmaOcr(captura);
                captura.StatusCode = 1;
                captura.Code = "0010";
                captura.Message = "Registro não pode ser atualizado";
                return captura;
            }
            catch (Exception ex)
            {
                captura.StatusCode = 1;
                captura.Code = "0010";
                captura.Message = "Registro não pode ser atualizado";
                return captura;
            }
        }

        //public bool EhPlacaValida(string value)
        //{
        //    value = value.Replace("-", "").Trim().ToUpper();
        //    return new Regex("^[a-zA-Z]{3}\\d{4}$").IsMatch(value);
        //}

        private byte[] getImg(string strBase64)
        {
            return Convert.FromBase64String(strBase64);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public bool EhPlacaValida(string placa)
        {
            placa = placa.Replace("-", "").Trim().ToUpper();
            bool ret = ValidarPlacaPadrao(placa);
            if (!ret)
                ret = ValidarPlacaMercosul(placa);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public static bool ValidarPlacaPadrao(string placa)
        {
            return new Regex("^[a-zA-Z]{3}\\d{4}$").IsMatch(placa);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="placa"></param>
        /// <returns></returns>
        public static bool ValidarPlacaMercosul(string placa)
        {
            return new Regex("^[a-zA-Z]{3}[0-9][a-zA-Z]\\d{2}$").IsMatch(placa);
        }

    }
}
