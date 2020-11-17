using Dapper;
using System.Data;
using System.Text;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class IntervencaoGuardaModel : BaseModel
    {
        public bool InserirIntervencaoGuarda(IntervencaoGuarda intervencaoGuarda)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@CD_CREDENCIAL_PESSOA", (object)intervencaoGuarda.CD_CREDENCIAL_PESSOA, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@CD_PLACA_VEICULO", (object)intervencaoGuarda.CD_PLACA_VEICULO, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@CD_SECAO", (object)intervencaoGuarda.CD_SECAO, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ID_EQUIPAMENTO", (object)intervencaoGuarda.ID_EQUIPAMENTO, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@CD_SENTIDO", (object)intervencaoGuarda.CD_SENTIDO, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@CD_VCO_PESSOA", (object)intervencaoGuarda.CD_VCO_PESSOA, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@CD_ATIVO", (object)"S", new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Requisitante", (object)1, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@IdCapturaOcr", (object)intervencaoGuarda.IdCapturaOcr, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" insert into TB_INTERVENCAO( ");
            stringBuilder.Append(" \t\tDT_INTERVENCAO_GUARDA,  ");
            stringBuilder.Append(" \t\tCD_CREDENCIAL_PESSOA, ");
            stringBuilder.Append(" \t\tCD_PLACA_VEICULO,  ");
            stringBuilder.Append(" \t\tCD_SECAO, ");
            stringBuilder.Append(" \t\tID_EQUIPAMENTO, ");
            stringBuilder.Append(" \t\tCD_SENTIDO, ");
            stringBuilder.Append(" \t\tCD_ATIVO, ");
            stringBuilder.Append(intervencaoGuarda.IdCapturaOcr > 0 ? " IdCapturaOcr, " : "");
            stringBuilder.Append(" \t\tRequisitante, ");
            stringBuilder.Append(" \t\tCD_VCO_PESSOA ");
            stringBuilder.Append(" \t\t) ");
            stringBuilder.Append(" \tVALUES( ");
            stringBuilder.Append(" \t\tgetdate(), ");
            stringBuilder.Append(" \t\t@CD_CREDENCIAL_PESSOA, ");
            stringBuilder.Append(" \t\t@CD_PLACA_VEICULO, ");
            stringBuilder.Append(" \t\t@CD_SECAO, ");
            stringBuilder.Append(" \t\t@ID_EQUIPAMENTO, ");
            stringBuilder.Append(" \t\t@CD_SENTIDO, ");
            stringBuilder.Append(" \t\t@CD_ATIVO, ");
            stringBuilder.Append(intervencaoGuarda.IdCapturaOcr > 0 ? " @IdCapturaOcr," : "");
            stringBuilder.Append(" \t\t@Requisitante, ");
            stringBuilder.Append(" \t\t@CD_VCO_PESSOA) ");
            return this.Execute((Basica)intervencaoGuarda, stringBuilder.ToString(), (object)dynamicParameters) > 0;
        }
    }
}