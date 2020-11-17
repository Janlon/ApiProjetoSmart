using Dapper;
using System.Data;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    internal class ConfirmaOcrModel : BaseModel
    {
        internal Captura ConfirmaOcr(Captura captura)
        {
            this.StrSql.Append("UPDATE TB_CapturaOCR SET TextoConfirmado = @Placa WHERE IdOCR = @ID");
            string upper = captura.Placa.Replace("-", "").Trim().ToUpper();
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@ID", (object)captura.IdOCR, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@Placa", (object)upper, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            int num = this.Execute((Basica)captura, this.StrSql.ToString(), (object)dynamicParameters);
            if (num == 1)
                captura.StatusCode = 0;
            if (num == 0)
                captura.StatusCode = 1;
            return captura;
        }
    }
}
