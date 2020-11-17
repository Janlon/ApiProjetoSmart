using Dapper;
using System;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class MotivacaoModel : BaseModel
    {
        public bool InserirMotivacaoTemporaria(MotivacaoTemporaria motivacaoTemporaria)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@IdColaborador", (object)motivacaoTemporaria.IdColaborador, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@OrdemServico", (object)motivacaoTemporaria.OrdemServico, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Placa", (object)motivacaoTemporaria.Placa, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ValidadeDtInicio", (object)DateTime.Now, new DbType?(DbType.DateTime), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ValidadeDtTermino", (object)DateTime.Now.AddMonths(6), new DbType?(DbType.DateTime), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@idMotivaCaoTemporaria", (object)null, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Output), new int?(), new byte?(), new byte?());
            int num = this.Execute((Basica)motivacaoTemporaria, "SP_MotivacaoTemporariaInserir", CommandType.StoredProcedure, (object)dynamicParameters);
            motivacaoTemporaria.Id = dynamicParameters.Get<int>("@idMotivaCaoTemporaria");
            return num > 0;
        }

        public MotivacaoTemporaria ListarMotivacaoTemporariaAtiva(
          MotivacaoTemporaria motivacaoTemporaria)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@numOs", (object)motivacaoTemporaria.OrdemServico, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "SELECT * FROM TB_MotivacaoTemporaria where OrdemServico = @numOs and FlSaida = 0 and DtSaida is null and dtCancelamento is null";
            return this.Query<MotivacaoTemporaria>((Basica)motivacaoTemporaria, sql, (object)dynamicParameters).FirstOrDefault<MotivacaoTemporaria>();
        }

        public MotivacaoTemporaria ListarMotivacaoTemporaria(
          MotivacaoTemporaria motivacaoTemporaria)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@numOs", (object)motivacaoTemporaria.OrdemServico, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "SELECT * FROM TB_MotivacaoTemporaria where OrdemServico = @numOs and FlSaida = 0 and DtSaida is null and dtCancelamento is null";
            return this.Query<MotivacaoTemporaria>((Basica)motivacaoTemporaria, sql, (object)dynamicParameters).FirstOrDefault<MotivacaoTemporaria>();
        }
    }
}