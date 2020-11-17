namespace WEBAPI_VOPAK
{
    using System.Configuration;
    using System.Data.SqlClient;

    public static class SqlConnectionExtensions
    {

        public static bool GetConnectionConfig(this SqlConnection value, string ambiente)
        {
            string ret = "";
            ret = ambiente == "T" ?
                ConfigurationManager.AppSettings["db_t"] :
                ConfigurationManager.AppSettings["db_p"];
            //ConfigurationManager.AppSettings["BANCO_TESTE"] :
            //ConfigurationManager.AppSettings["BANCO_PRODUCAO"];
            if (!string.IsNullOrEmpty(ret))
            {
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
                sb.DataSource = ret;
                sb.InitialCatalog = "LOCKTEK";
                sb.ApplicationName = "WEBAPI_VOPAK";
                sb.CurrentLanguage = "brazilian";
                sb.Password = "Senha@123";
                sb.PersistSecurityInfo = true;
                sb.Pooling = true;
                sb.UserID = "sa";
                sb.WorkstationID = System.Environment.MachineName;
                ret = sb.ConnectionString;
                value.ConnectionString = ret; 
            }
            return (string.IsNullOrEmpty(ret));
        }
    }
}