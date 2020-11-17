using Dapper;
using System.Data;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class LogModel : BaseModel
    {
        public bool InserirLog(LogSmartApi log)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@texto", (object)log.texto, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@metodo", (object)log.metodo, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@ip", (object)log.ip, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "INSERT INTO tb_RequisicaoApi\n                                        ([texto]\n                                        ,[metodo]\n                                        ,[ip])\n                                    VALUES\n                                        (@texto\n                                        ,@metodo\n                                        ,@ip)";
            this.Execute((Basica)log, sql, (object)dynamicParameters);
            return true;
        }

        public bool InserirLogApi(LogApi log)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@texto", (object)log.texto, new DbType?(DbType.String), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            dynamicParameters.Add("@id_sessao", (object)log.idSessao, new DbType?(DbType.Int32), new ParameterDirection?(ParameterDirection.Input), new int?(), new byte?(), new byte?());
            string sql = "INSERT INTO tb_Log_Api\n                                        ([texto], [id_sessao])\n                                    VALUES\n                                        (@texto,@id_sessao)";
            this.Execute((Basica)log, sql, (object)dynamicParameters);
            return true;
        }
    }
}