using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using WebApiBusiness.App_Data;

namespace WebApiBusiness.Models
{
    public class BaseModel
    {
        protected StringBuilder StrSql = new StringBuilder();

        protected T QueryFirstOrDefault<T>(Basica basica, string sql, object parameters = null)
        {
            using (IDbConnection connection = this.CreateConnection(basica))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<T>(sql, parameters, (IDbTransaction)null, new int?(), new CommandType?());
            }
        }

        protected List<T> Query<T>(Basica basica, string sql, object parameters = null)
        {
            using (IDbConnection connection = this.CreateConnection(basica))
            {
                connection.Open();
                return connection.Query<T>(sql, parameters, (IDbTransaction)null, true, new int?(), new CommandType?()).ToList<T>();
            }
        }

        protected int Execute(Basica basica, string sql, object parameters = null)
        {
            using (IDbConnection connection = this.CreateConnection(basica))
            {
                connection.Open();
                return connection.Execute(sql, parameters, (IDbTransaction)null, new int?(), new CommandType?());
            }
        }

        protected int Execute(Basica basica, string sql, CommandType tipoComando, object parameters = null)
        {
            using (IDbConnection connection = this.CreateConnection(basica))
            {
                connection.Open();
                return connection.Execute(sql, parameters, (IDbTransaction)null, new int?(), new CommandType?(tipoComando));
            }
        }

        private IDbConnection CreateConnection(Basica basica)
        {
            string connectionString1 = ConfigurationManager.ConnectionStrings["BANCO_P"].ConnectionString;
            string connectionString2 = ConfigurationManager.ConnectionStrings["BANCO_T"].ConnectionString;
            return (IDbConnection)new SqlConnection(basica.Ambiente.Equals("P") ? connectionString1 : connectionString2);
        }
    }
}