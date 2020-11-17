namespace WEBAPI_VOPAK
{
    #region Espaços
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    #endregion

    /// <summary>
    /// Auxiliar para a Dapper.
    /// </summary>
    internal class DbHelper : IDisposable
    {
        #region Manutenção
        /// <summary>
        /// Mantém o status das chamadas à destruição da instância.
        /// </summary>
        private bool disposedValue;
        #endregion

        #region Instância.
        /// <summary>
        /// Limpeza da memória.
        /// </summary>
        private void CleanUp()
        {
            Ambient = null;
            SqlStatement = null;
            try { Parameters.Clear(); } catch { }
            try { Parameters = null; } catch { }
        }
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        internal DbHelper() { Ambient = System.Diagnostics.Debugger.IsAttached ? "T" : "P"; }
        /// <summary>
        /// Construtor com o ambiente.
        /// </summary>
        /// <param name="ambiente">Sigla do ambiente á ser utilizado.</param>
        public DbHelper(string ambiente)
        {
            disposedValue = false;
            Ambient = ambiente;
        }
        /// <summary>
        /// Construtor com o ambiente e a instrução Sql.
        /// </summary>
        /// <param name="ambiente">Sigla do ambiente á ser utilizado.</param>
        /// <param name="sql">Instrução SQL ou procedimento armazenado.</param>
        public DbHelper(string ambiente, string sql)
        {
            disposedValue = false;
            Ambient = ambiente;
            SqlStatement = sql;
        }
        /// <summary>
        /// Construtor com o ambiente e a instrução Sql.
        /// </summary>
        /// <param name="ambiente">Sigla do ambiente á ser utilizado.</param>
        /// <param name="sql">Instrução SQL ou procedimento armazenado.</param>
        public DbHelper(string ambiente, string sql, string parameterName, dynamic parameterValue, DbType parameterDataType = DbType.Int32, ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            disposedValue = false;
            Ambient = ambiente;
            SqlStatement = sql;
            Add(parameterName, parameterValue, parameterDataType, parameterDirection);
        }
        /// <summary>
        /// Destruidor efetivo.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) { if (!disposedValue) { if (disposing) { CleanUp(); } disposedValue = true; } }
        /// <summary>
        /// Equivalente ao Finalize do VB.
        /// </summary>
        ~DbHelper() { Dispose(false); }
        /// <summary>
        /// Destruidor padrão.
        /// </summary>
        public void Dispose() { Dispose(true); GC.SuppressFinalize(this); }
        #endregion

        #region Propriedades internas.
        /// <summary>
        /// Tipo de instrução SQL (texto ou procedure).
        /// </summary>
        private CommandType CommandType
        {
            get
            {
                return (SqlStatement.Contains(" ")) ?
                    CommandType.Text :
                    CommandType.StoredProcedure;
            }
        }
        /// <summary>
        /// Lista de parâmetros.
        /// </summary>
        private List<DbParameter> Parameters { get; set; } = new List<DbParameter>();
        /// <summary>
        /// Conexão aos dados.
        /// </summary>
        private string Ambient { get; set; } = "P";
        #endregion

        #region Propriedades públicas
        /// <summary>
        /// Consulta SQL.
        /// </summary>
        public string SqlStatement { get; set; } = "SELECT GETDATE()";

        /// <summary>
        /// Tempo limite padrão para consultas.
        /// </summary>
        private int DefaultTimeout 
        {
            get
            {
                return (int)TimeSpan.FromSeconds(200).TotalSeconds;
            }
        }
        /// <summary>
        /// Retorna se a base de dados pode ou não ser conectada.
        /// </summary>
        /// <param name="ambiente">Texto, somente "P" ou "T".</param>
        /// <returns>Sucesso da conectividade com o sevidor.</returns>
        internal static bool CanConnect(string ambiente)
        {
            bool ret = false;
            try
            {
                DateTime? dt;
                using (SqlConnection db = ConfigHelper.Connection(ambiente))
                    dt = db
                        .Query<DateTime>("SELECT GETDATE();")
                        .FirstOrDefault();
                ret = dt.HasValue;
            }
            catch (Exception ex) { ex.Log(); ret = false; }
            return ret;
        }
        #endregion

        #region Métodos públicos para a lide com os parâmetros
        /// <summary>
        /// Adiciona um parâmetro á coleção.
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="type">Tipo de dados. (<see cref="DbType"/>.)</param>
        /// <param name="direction">Direção do parâmetro. (<see cref="ParameterDirection"/>.)</param>
        public void Add(string parameterName, dynamic value, DbType type = DbType.Int32, ParameterDirection direction = ParameterDirection.Input)
        {
            if (Parameters == null)
                Parameters = new List<DbParameter>();
            if (Parameters
                .Where(p => p.ParameterName
                .Equals(parameterName))
                .Count() == 0)
            {
                SqlParameter p = new SqlParameter(parameterName, value);
                p.DbType = type;
                p.Direction = direction;
                Parameters.Add(p);
            }
        }

        /// <summary>
        /// Remove, se existir, o parâmetro com o nome indicado.
        /// </summary>
        /// <param name="parameterName">Nome do parâmetro á ser removido.</param>
        public void Remove(string parameterName)
        {
            if (Parameters.Where(p => p.ParameterName
            .Equals(parameterName))
                .Count() == 0)
                Parameters.Remove(Parameters
                    .Where(p => p.ParameterName
                    .Equals(parameterName))
                    .FirstOrDefault());
        }

        /// <summary>
        /// Limpa (esvazia) a coleção de parâmetros.
        /// </summary>
        public void Clear()
        {
            Parameters.Clear();
            Parameters = new List<DbParameter>();
        }
        #endregion

        #region Ações
        /// <summary>
        /// Executa uma instrução ou um procedimento armazenado 
        /// e retorna a quantidade de registros afetados.
        /// </summary>
        /// <param name="useTransaction">Define se deve ou não utilizar transações.</param>
        /// <returns>
        /// Quantidade de registros afetados. 
        /// Em caso de erro de preparação, retorna -1. 
        /// Em caso de erro de execução, retorna -2.
        /// </returns>
        public int Execute(bool useTransaction = true)
        {
            int ret = 0;
            try
            {
                IDbTransaction transa = null;
                DynamicParameters p = new DynamicParameters();
                foreach (DbParameter par in Parameters)
                    p.Add(par.ParameterName, par.Value, par.DbType, par.Direction);
                using (SqlConnection db = ConfigHelper.Connection(Ambient))
                {
                    db.Open();
                    if (useTransaction)
                        transa = db.BeginTransaction();
                    try
                    {
                        ret = db.Execute(SqlStatement, p, transa, DefaultTimeout, CommandType);
                        if (transa != null)
                            transa.Commit();
                        if (db != null)
                            if (db.State != ConnectionState.Open)
                                db.Close();
                    }
                    catch (SqlException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (DbException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (DataException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (Exception ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                }
            }
            catch (Exception ex){ ex.Log(); ret = 0; }
            return ret;
        }

        /// Tentando com a Dapper:
        public int InsertComDapper()
        {
            int ret = 0;
            using (SqlConnection db = ConfigHelper.Connection(Ambient))
            {
                try
                {

                    DynamicParameters dp = new DynamicParameters();
                    foreach (DbParameter item in Parameters)
                        dp.Add(item.ParameterName, item.Value, item.DbType, item.Direction);
                    ret = db.ExecuteScalar<int>(SqlStatement, dp, null, DefaultTimeout, CommandType.Text);
                }
                catch (SqlException ex) { ex.Log(); ret = 0; }
                catch (DbException ex) { ex.Log(); ret = 0; }
                catch (DataException ex) { ex.Log(); ret = 0; }
                catch (Exception ex) { ex.Log(); ret = 0; }
                if (db.State != ConnectionState.Closed)
                    db.Close();
            }
            return ret;
        }
        /// <summary>
        /// Executa uma instrução ou um procedimento armazenado do tipo escalar 
        /// e retorna o valor (inteiro, decimal, tabela etc) obtido por esse processo.
        /// </summary>
        /// <param name="useTransaction">Define se deve ou não utilizar transações.</param>
        /// <returns>
        /// Valor do procedimento ou instrução escalar. 
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pendente>")]
        public int Insert(bool useTransaction = true) //where T : class
        {
            int ret = 0;
            SqlTransaction transa = null;
            try
            {
                using (SqlConnection db = ConfigHelper.Connection(Ambient))
                {
                    db.Open();
                    SqlConnection.ClearAllPools();
                    try
                    {
                        if (useTransaction)
                            transa = db.BeginTransaction();
                        using (SqlCommand cmd = (SqlCommand)db.CreateCommand())
                        {
                            cmd.CommandText = SqlStatement;
                            cmd.Transaction = transa;
                            cmd.CommandType = SqlStatement.Contains(" ") ? CommandType.Text : CommandType.StoredProcedure;
                            cmd.Parameters.AddRange(Parameters.Select(p => new SqlParameter(p.ParameterName, p.Value)
                            {
                                DbType = p.DbType,
                                Direction = p.Direction
                            }).ToArray());
                            if (cmd.CommandType == CommandType.StoredProcedure)
                                cmd.Prepare();
                            ret = (int)cmd.ExecuteScalar();
                            if (transa != null)
                                transa.Commit();
                        }
                    }
                    catch (SqlException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (DbException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (DataException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    catch (Exception ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = 0; }
                    if (db.State != ConnectionState.Open)
                        db.Close();
                }
            }
            catch { ret = 0; }
            return ret;
        }

        /// <summary>
        /// Executa uma instrução ou um procedimento armazenado do tipo escalar 
        /// e retorna o valor (inteiro, decimal, tabela etc) obtido por esse processo.
        /// </summary>
        /// <param name="useTransaction">Define se deve ou não utilizar transações.</param>
        /// <returns>
        /// Valor do procedimento ou instrução escalar. 
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pendente>")]
        public T ExecuteScalar<T>(bool useTransaction = true) //where T : class
        {
            T ret = Activator.CreateInstance<T>();
            try
            {
                SqlTransaction transa = null;
                using (SqlConnection db = ConfigHelper.Connection(Ambient))
                {
                    db.Open();
                    if (useTransaction)
                        transa = db.BeginTransaction();
                    try
                    {
                        using (SqlCommand cmd = db.CreateCommand())
                        {
                            cmd.CommandText = SqlStatement;
                            cmd.Transaction = transa;
                            cmd.CommandType = SqlStatement.Contains(" ")? CommandType.Text: CommandType.StoredProcedure;
                            foreach (DbParameter p in Parameters)
                            {
                                SqlParameter par = new SqlParameter(p.ParameterName, p.Value);
                                par.DbType = p.DbType;
                                par.Direction = p.Direction;
                                cmd.Parameters.Add(par);
                            }
                            if(cmd.CommandType== CommandType.StoredProcedure)
                                cmd.Prepare();
                            var ok = cmd.ExecuteScalar();
                            foreach (SqlParameter par in cmd.Parameters)
                            {
                                if (par.Direction == ParameterDirection.Output)
                                    ret = (T)par.Value;
                            }
                            if (transa != null)
                                transa.Commit();
                            if (db != null)
                                if (db.State != ConnectionState.Open)
                                    db.Close();
                        }
                    }
                    catch (SqlException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
                    catch (DbException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
                    catch (DataException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
                    catch (Exception ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
                }
            }
            catch { ret = Activator.CreateInstance<T>(); }
            return ret;
        }

        /// <summary>
        /// Efetua uma consulta à base de dados e retorna o Mensagens como uma lista de <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo desejado para a lista de retorno.</typeparam>
        /// <param name="useTransaction">Informa se deve ou não utilizar transações.</param>
        /// <returns>Lista de itens (<see cref="IEnumerable{T}"/>).</returns>
        public IEnumerable<T> Query<T>(bool useTransaction = true) //where T : class
        {
            List<T> ret = Activator.CreateInstance<List<T>>();
            IDbTransaction transa = null;
            try
            {
                using (SqlConnection db = ConfigHelper.Connection(Ambient))
                {
                    db.Open();
                    if (useTransaction)
                        transa = db.BeginTransaction();
                    DynamicParameters p = new DynamicParameters();
                    foreach (DbParameter par in Parameters)
                        p.Add(par.ParameterName, par.Value, par.DbType, par.Direction);
                    ret.AddRange(db.Query<T>(SqlStatement, p, transa, true, DefaultTimeout, CommandType));
                    if (transa != null)
                        transa.Commit();
                    if (db!=null)
                        if (db.State != ConnectionState.Open)
                            db.Close();
                }
            }
            catch (SqlException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<List<T>>(); }
            catch (DbException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<List<T>>(); }
            catch (DataException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<List<T>>(); }
            catch (Exception ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<List<T>>(); }
            return ret;
        }

        /// <summary>
        /// Retorna o primeiro item cadastrado localizado pela consulta aos dados.
        /// </summary>
        /// <typeparam name="T">Tipo de retorno.</typeparam>
        /// <param name="useTransaction">Informa se deve ou não utilizar transações.</param>
        /// <returns>Item de tipo indicado, ou nulo.</returns>
        public T QueryFirstOrDefault<T>(bool useTransaction = true) //where T : class
        {
            T ret = Activator.CreateInstance<T>();
            IDbTransaction transa = null;
            try
            {
                using (SqlConnection db = ConfigHelper.Connection(Ambient))
                {
                    db.Open();
                    if (useTransaction)
                        transa = db.BeginTransaction();
                    DynamicParameters p = new DynamicParameters();
                    foreach (DbParameter par in Parameters)
                        p.Add(par.ParameterName, par.Value, par.DbType, par.Direction);
                    ret = db.QueryFirstOrDefault<T>(SqlStatement, p, transa, DefaultTimeout, CommandType);
                    if (transa != null)
                        transa.Commit();
                    if (db != null)
                        if (db.State != ConnectionState.Open)
                            db.Close();
                }
            }
            catch (SqlException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
            catch (DbException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
            catch (DataException ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
            catch (Exception ex) { ex.Log(); if (transa != null) { transa.Rollback(); } ret = Activator.CreateInstance<T>(); }
            return ret;
        }
        #endregion
    }
}