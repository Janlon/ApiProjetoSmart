using Dapper;
using System.Data;
using System.Linq;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class ColaboradorModel : BaseModel
    {
        public Colaborador GetColaborador(int idCracha, string ambiente)
        {
            this.StrSql.Append("  SELECT A.IdColaborador, A.CdCpf, B.CdCracha FROM TB_Colaborador A ");
            this.StrSql.Append("  JOIN TB_Cracha B on A.IdColaborador =  B.IdColaborador ");
            this.StrSql.Append("  WHERE ");
            this.StrSql.Append("  A.CdAtivo = 1 and ");
            this.StrSql.Append("  A.cdExcluido = 0 and ");
            this.StrSql.Append("  B.CdAtivo = 1 and ");
            this.StrSql.Append("  B.cdExcluido = 0  and ");
            this.StrSql.Append("  B.IdCracha = @IdCracha ");
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@IdCracha", (object)idCracha, new DbType?(), new ParameterDirection?(), new int?(), new byte?(), new byte?());
            return this.Query<Colaborador>(new Basica()
            {
                Ambiente = ambiente
            }, this.StrSql.ToString(), (object)dynamicParameters).FirstOrDefault<Colaborador>();
        }
    }
}