using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class CapturaOcrModel : BaseModel
    {
        internal List<CameraEquipamento> GetCamerasEquipamento(Captura captura)
        {
            this.StrSql.Append("  select  A.IdEquipamento as IdEquipamentoCamera,");
            this.StrSql.Append("\t\t  (select IdEquipamento from TB_Equipamento where DsEquipamento = @LOCAL) as IdEquipamento,");
            this.StrSql.Append("\t\t  A.Ip, A.Conta,");
            this.StrSql.Append("\t\t  A.Senha,");
            this.StrSql.Append("\t\t  A.DsLinkCameraVideo");
            this.StrSql.Append("\t\t  from TB_EquipamentoConfiguracao A");
            this.StrSql.Append("  JOIN TB_Equipamento B ON A.IdEquipamento = B.IdEquipamento ");
            this.StrSql.Append("  where A.IdEquipamentoPai =  (select IdEquipamento from TB_Equipamento where DsEquipamento = @LOCAL)");
            this.StrSql.Append("  and B.IdEquipamentoTipo IN (4, 8)");
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@LOCAL", (object)captura.Local, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            return this.Query<CameraEquipamento>((Basica)captura, this.StrSql.ToString(), (object)dynamicParameters);
        }

        internal bool GravaOcr(Captura captura)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@IdLocal", (object)captura.IdLocal, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Pesagem", (object)captura.Pesagem, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@TextoPlacaCarmen", (object)captura.TextoPlacaCarmen, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Placa", (object)captura.Placa, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Score", (object)captura.Score, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "INSERT INTO TB_CapturaOCR\n                                        ([IdLocal]\n                                        ,[Pesagem]\n                                        ,[TextoPlacaCarmen]\n                                        ,[TextoConfirmado]\n                                        ,[Score])\n                                    VALUES\n                                        (@IdLocal\n                                        ,@Pesagem\n                                        ,@TextoPlacaCarmen\n                                        ,@Placa\n                                        ,@Score); SELECT CAST(SCOPE_IDENTITY() as int)";
            captura.IdOCR = this.Query<int>((Basica)captura, sql, (object)dynamicParameters).Single<int>();
            return true;
        }

        internal bool GravaImagem(Captura captura)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@IdOCR", (object)captura.IdOCR, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Image", (object)captura.Image, new DbType?(DbType.Binary), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "INSERT INTO TB_CapturaImagemOCR\n                                        ([IdOCR]\n                                        ,[Image])\n                                    VALUES\n                                        (@IdOCR\n                                        ,@Image) SELECT CAST(SCOPE_IDENTITY() as int)";
            captura.IdCapturaImagemOcr = this.Query<int>((Basica)captura, sql, (object)dynamicParameters).Single<int>();
            return true;
        }

        internal MotivacaoTemporaria SelecionarMotivacao(Captura captura)
        {
            string str = "SELECT * FROM [TB_MotivacaoTemporaria] WHERE OrdemServico = @OrdemServico";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@OrdemServico", (object)captura.NumOs.ToString(), new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            return this.Query<MotivacaoTemporaria>((Basica)captura, str.ToString(), (object)dynamicParameters).FirstOrDefault<MotivacaoTemporaria>();
        }

        internal Equipamento SelecionarEquipamento(Captura captura)
        {
            string str = $"SELECT * FROM [TB_Equipamento] WHERE DsEquipamento = '{captura.Local}'";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@DsEquipamento", captura.Local.ToUpper(), DbType.String, ParameterDirection.Input);
            // Aqui, se retornar nulo porque não encontrou nada, 
            return this.Query<Equipamento>( (Basica)captura, str, dynamicParameters).FirstOrDefault<Equipamento>();
        }

        internal Captura SelecionarImageCapturaOcr(
          int idCapturaImagemOcr,
          int idOcr,
          string ambiente)
        {
            string str = "SELECT * FROM [TB_CapturaImagemOCR] WHERE IdCapturaImagemOcr = @IdCapturaImagemOcr and @IdOcr =  idOcr";
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@IdCapturaImagemOcr", (object)idCapturaImagemOcr, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@IdOcr", (object)idOcr, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            Captura captura = new Captura();
            captura.Ambiente = ambiente;
            return this.Query<Captura>((Basica)captura, str.ToString(), (object)dynamicParameters).FirstOrDefault<Captura>();
        }
    }
}