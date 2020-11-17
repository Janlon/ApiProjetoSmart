using Dapper;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class Ethernet3Model : BaseModel
    {
        public LiberarBalancaDTO GetEquipamentoEth03ByBalanca(
          LiberarBalancaDTO ethernet3)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@DsEquipamento", (object)ethernet3.Balanca, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = " SELECT ECE.*  FROM TB_RelacBalancaEthernet3 RLE " + " JOIN TB_Equipamento E ON RLE.Idbalanca =  E.IdEquipamento " + " JOIN TB_EquipamentoConfiguracaoEth03 ECE ON ECE.IdEquipamento =  RLE.IdEthernet3 " + " WHERE E.DsEquipamento = @DsEquipamento ";
            return this.Query<LiberarBalancaDTO>((Basica)ethernet3, sql, (object)dynamicParameters).FirstOrDefault<LiberarBalancaDTO>();
        }
    }
}
