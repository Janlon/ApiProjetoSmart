namespace WEBAPI_VOPAK
{

    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Security;

    internal static class ConfigHelper
    {

        /// <summary>
        /// Retorna uma conexão com a base de dados indicada pelo ambiente.
        /// </summary>
        /// <param name="ambiente">Texto, 'T' o 'P', indicando o ambiente de Teste ou Produção.</param>
        /// <returns>Objeto do tipo <see cref="SqlConnection"/>.</returns>
        public static SqlConnection Connection(string ambiente)
        {
            SqlConnection ret = null;
            try
            {
                SqlConnectionStringBuilder sb = null;
                sb = new SqlConnectionStringBuilder();
                if (!string.IsNullOrEmpty(ambiente))
                {
                    sb.DataSource = GetConnection(ambiente);
                    sb.InitialCatalog = GetCatalog(ambiente);
                }
                else
                {
                    sb.DataSource = ".";
                    sb.InitialCatalog = "LOCKTEK";
                }                
                sb.ApplicationName = "WEBAPI_VOPAK";
                sb.CurrentLanguage = "brazilian";
                sb.PersistSecurityInfo = true;
                sb.Pooling = true;
                sb.WorkstationID = Environment.MachineName;
                sb.MultipleActiveResultSets = true;
                sb.Enlist = true;
                sb.ConnectRetryCount = 0;
                sb.ConnectTimeout = 2;
                //sb.Password = "LockTek@2017";
                //sb.UserID = "LOCKTEK";
                ret = new SqlConnection(sb.ConnectionString)
                { Credential = Credentials() };
            }
            catch (Exception ex){ var pp = ex; }
            return (ret);
        }

        /// <summary>
        /// Preara um <see cref="SqlCredential"/> para conexão aos dados.
        /// </summary>
        /// <param name="userName">Conta de acesso.</param>
        /// <param name="password">Senha de acesso.</param>
        /// <returns>Objeto do tipo <see cref="SqlCredential"/>.</returns>
        private static SqlCredential Credentials(string userName="sa", string password= "Senha@123")
        {
            SecureString ret = new SecureString();
            foreach (char c in password)
                ret.AppendChar(c);
            ret.MakeReadOnly();
            return (new SqlCredential("sa", ret));
        }

        /// <summary>
        /// Retorna, se configurado, o comando de conexão aos dados, conforme o ambiente solicitado.
        /// </summary>
        /// <param name="ambiente">Ambiente solicitado. Só aceita "T" ou "P".</param>
        /// <returns>Comando de conexão aos dados.</returns>
        private static string GetConnection(string ambiente)
        {
            string temp;
            ambiente =(""+ ambiente+"").Trim().ToUpper();
            if (string.IsNullOrEmpty(ambiente))
                ambiente = "T";
            if (string.IsNullOrWhiteSpace(ambiente))
                ambiente = "T";
            if (!"TP".Contains(ambiente))
                ambiente = "T";
            temp = (ambiente == "T") ?
                ConfigurationManager.AppSettings["db_t"] :
                ConfigurationManager.AppSettings["db_p"];
            return temp.Trim();
        }

        /// <summary>
        /// Retorna, se configurado, o comando de conexão aos dados, conforme o ambiente solicitado.
        /// </summary>
        /// <param name="ambiente">Ambiente solicitado. Só aceita "T" ou "P".</param>
        /// <returns>Comando de conexão aos dados.</returns>
        private static string GetCatalog(string ambiente)
        {
            string temp;
            ambiente = ("" + ambiente + "").Trim().ToUpper();
            if (string.IsNullOrEmpty(ambiente))
                ambiente = "T";
            if (string.IsNullOrWhiteSpace(ambiente))
                ambiente = "T";
            if (!"TP".Contains(ambiente))
                ambiente = "T";
            temp = (ambiente == "T") ?
                ConfigurationManager.AppSettings["ct_t"] :
                ConfigurationManager.AppSettings["ct_p"];
            return temp.Trim();
        }
    }
}