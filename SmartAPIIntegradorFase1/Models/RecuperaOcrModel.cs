using Dapper;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class RecuperaOcrModel : BaseModel
    {
        internal Captura RecuperaOcr(Captura captura)
        {
            this.StrSql.Append("SELECT * FROM TB_CapturaOCR A INNER JOIN TB_CapturaImagemOCR B ON B.IdOCR = A.IdOCR WHERE A.IdOCR  = @ID");
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@ID", (object)captura.IdOCR, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            Captura captura1 = this.Query<Captura>((Basica)captura, this.StrSql.ToString(), (object)dynamicParameters).FirstOrDefault<Captura>();
            if (captura1 != null)
                captura.StatusCode = 0;
            if (captura1 != null)
                return captura1;
            captura.StatusCode = 1;
            return captura1;
        }
    }
}