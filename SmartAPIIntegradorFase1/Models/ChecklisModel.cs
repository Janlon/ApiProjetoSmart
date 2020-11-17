using Dapper;
using System;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class ChecklisModel : BaseModel
    {
        public Checklist GetCheckList(Checklist checklist)
        {
            try
            {
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@placa", (object)checklist.NrPlaca, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
                string sql = "SELECT * FROM TB_Checklist WHERE NrPlaca =  @placa";
                return this.Query<Checklist>((Basica)checklist, sql, (object)dynamicParameters).FirstOrDefault<Checklist>();
            }
            catch (Exception ex)
            {
                return (Checklist)null;
            }
        }
    }
}
