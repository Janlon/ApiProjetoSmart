namespace WEBAPI_VOPAK
{
    #region Espaços
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Data.Common;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    #endregion

    /// <summary>
    /// Classe para manutenção de erros.
    /// </summary>
    public class ErrorBlock
    {
        [JsonProperty("DetectionDate")]
        [Display(Name = "Data da detecção")]
        public DateTime DetectionDate { get; set; } = DateTime.Now;

        [JsonProperty("Caller")]
        [Display(Name = "Método chamador")]
        public string Caller { get; set; } = "";

        [JsonProperty("FileName")]
        [Display(Name = "Arquivo de código")]
        public string FileName { get; set; } = "";

        [JsonProperty("LineNumber")]
        [Display(Name = "Linha")]
        public int LineNumer { get; set; } = 0;

        [JsonProperty("ErrorMessage")]
        [Display(Name = "Mensagem de erro")]
        public string ErrorMessage { get; set; } = "";

        [JsonProperty("JSonError")]
        [Display(Name = "Erro como JSon")]
        [ScaffoldColumn(false)]
        [Browsable(false)]
        [JsonIgnore()]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string JSonError { get; set; } = "";
    }

    public static class ExceptionExtensions
    {
        #region Variáveis de manutenção
        /// <summary>
        /// Mantém o bloqueador de arquivos.
        /// </summary>
        private static ReaderWriterLockSlim _readWriteLock { get; set; } = new ReaderWriterLockSlim();

        /// <summary>
        /// Mantém a pasta de escrita dos erros.
        /// </summary>
        private static string LogFolder
        {
            get
            {

                var temp = HttpContext.Current.Server.MapPath("~");
                string ret = Path.Combine(temp, "ErrorLog");
                if (!Directory.Exists(ret))
                    Directory.CreateDirectory(ret);
                return ret;
            }
        }
        /// <summary>
        /// Mantém o nome do arquivo de erros em uso no dia corrente.
        /// </summary>
        private static string LogFile
        {
            get
            {
                return Path.Combine(LogFolder,
                    string.Format("erros_{0}.log", DateTime.Now.ToFilePrefix()));
            }
        }

        private static long LogFileSize
        {
            get
            {
                long ret = 0;
                try { ret = (new FileInfo(LogFile)).Length; }
                catch { ret = 0; }
                return ret;
            }
        }

        #endregion

        #region Suporte
        /// <summary>
        /// Blooquear o arquivo de log, escrever a mensagem e em seguida, desbloquear.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        private static void Out(string text)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                if (LogFileSize > 0)
                    text = string.Format(",{0}", text);
                // Incluír o texto...
                using (StreamWriter sw = File.AppendText(LogFile))
                {
                    sw.Write(text);
                    sw.Close();
                }
            }
            finally
            { _readWriteLock.ExitWriteLock(); }
        }
        #endregion

        #region Métodos públicos de extensão

        /// <summary>
        /// Blooquear o arquivo de log, escrever a mensagem e em seguida, desbloquear.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        public static List<ErrorBlock> LoadAll(this string text)
        {
            List<ErrorBlock> ret = new List<ErrorBlock>();
            string json = "";
            _readWriteLock.EnterWriteLock();
            try { json = File.ReadAllText(LogFile); }
            finally { _readWriteLock.ExitWriteLock(); }
            if(!string.IsNullOrEmpty(json))
                ret = JsonConvert.DeserializeObject<List<ErrorBlock>>(json);
            return ret;
        }
        /// <summary>
        /// Blooquear o arquivo de log, escrever a mensagem e em seguida, desbloquear.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        public static List<ErrorBlock> LoadAll(this Exception ex)
        {
            List<ErrorBlock> ret = new List<ErrorBlock>();
            string json = "";
            _readWriteLock.EnterWriteLock();
            try { json = string.Format("[{0}] ", File.ReadAllText(LogFile)); }
            finally { _readWriteLock.ExitWriteLock(); }
            if (!string.IsNullOrEmpty(json))
                ret = JsonConvert.DeserializeObject<List<ErrorBlock>>(json);
            return ret;
        }


        /// <summary>
        /// Método síncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="DataException"/>.</param>
        public static void Log(this DataException ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {

            ErrorBlock b = new ErrorBlock()
            {
                Caller = memberName,
                DetectionDate = DateTime.Now,
                ErrorMessage = ex.Message,
                FileName = sourceFilePath,
                LineNumer = sourceLineNumber,
                JSonError = JsonConvert.SerializeObject(ex)
            };
            Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
        }
        /// <summary>
        /// Método síncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="DbException"/>.</param>
        public static void Log(this DbException ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            ErrorBlock b = new ErrorBlock()
            {
                Caller = memberName,
                DetectionDate = DateTime.Now,
                ErrorMessage = ex.Message,
                FileName = sourceFilePath,
                LineNumer = sourceLineNumber,
                JSonError = JsonConvert.SerializeObject(ex)
            };
            Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
        }
        /// <summary>
        /// Método síncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="Exception"/>.</param>
        public static void Log(this Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            ErrorBlock b = new ErrorBlock()
            {
                Caller = memberName,
                DetectionDate = DateTime.Now,
                ErrorMessage = ex.Message,
                FileName = sourceFilePath,
                LineNumer = sourceLineNumber,
                JSonError = JsonConvert.SerializeObject(ex)
            };
            Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
        }        
        /// <summary>
        /// Método assíncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="DataException"/>.</param>
        public async static Task LogAsync(this DataException ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Action a = new Action(() =>
            {
                ErrorBlock b = new ErrorBlock()
                {
                    Caller = memberName,
                    DetectionDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    FileName = sourceFilePath,
                    LineNumer = sourceLineNumber,
                    JSonError = JsonConvert.SerializeObject(ex)
                };
                Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
            });
            await Task.Run(a);
        }
        /// <summary>
        /// Método assíncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="DbException"/>.</param>
        public async static Task LogAsync(this DbException ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Action a = new Action(() =>
            {
                ErrorBlock b = new ErrorBlock()
                {
                    Caller = memberName,
                    DetectionDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    FileName = sourceFilePath,
                    LineNumer = sourceLineNumber,
                    JSonError = JsonConvert.SerializeObject(ex)
                };
                Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
            });
            await Task.Run(a);
        }
        /// <summary>
        /// Método assíncrono de extensão para gravação de erros.
        /// </summary>
        /// <param name="ex">Objeto do tipo <see cref="Exception"/>.</param>
        public async static Task LogAsync(this Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Action a = new Action(() =>
            {
                ErrorBlock b = new ErrorBlock()
                {
                    Caller = memberName,
                    DetectionDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    FileName = sourceFilePath,
                    LineNumer = sourceLineNumber,
                    JSonError = JsonConvert.SerializeObject(ex)
                };
                Out(string.Format("\n{0}", JsonConvert.SerializeObject(b, Formatting.Indented)));
            });
            await Task.Run(a);
        }
        #endregion
    }

}