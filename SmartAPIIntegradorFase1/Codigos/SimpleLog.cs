using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebApiBusiness.Util
{
    internal static class SimpleLog
    {
        private static DirectoryInfo _logDir = new DirectoryInfo(Directory.GetCurrentDirectory());
        private static SimpleLog.Severity _logLevel = SimpleLog.Severity.Info;
        private static readonly Queue<XElement> _logEntryQueue = new Queue<XElement>();
        private static readonly object _backgroundTaskSyncRoot = new object();
        private static readonly object _logFileSyncRoot = new object();
        private static string _textSeparator = " | ";
        private static int _daysToDeleteFromPath;
        private static string _prefix;
        private static string _dateFormat;
        private static string _suffix;
        private static string _extension;
        private static Task _backgroundTask;

        public static string LogDir
        {
            get
            {
                return SimpleLog._logDir.FullName;
            }
        }

        public static string Prefix
        {
            get
            {
                return SimpleLog._prefix ?? string.Empty;
            }
            set
            {
                SimpleLog._prefix = value;
            }
        }

        public static string Suffix
        {
            get
            {
                return SimpleLog._suffix ?? string.Empty;
            }
            set
            {
                SimpleLog._suffix = value;
            }
        }

        public static string Extension
        {
            get
            {
                return SimpleLog._extension ?? "log";
            }
            set
            {
                SimpleLog._extension = value;
            }
        }

        public static string DateFormat
        {
            get
            {
                return SimpleLog._dateFormat ?? "yyyy_MM_dd";
            }
            set
            {
                SimpleLog._dateFormat = value;
            }
        }

        public static SimpleLog.Severity LogLevel
        {
            get
            {
                return SimpleLog._logLevel;
            }
            set
            {
                SimpleLog._logLevel = value;
            }
        }

        public static bool StartExplicitly { get; set; }

        public static bool WriteText { get; set; }

        public static string TextSeparator
        {
            get
            {
                return SimpleLog._textSeparator;
            }
            set
            {
                SimpleLog._textSeparator = value ?? string.Empty;
            }
        }

        public static string FileName
        {
            get
            {
                return SimpleLog.GetFileName(DateTime.Now);
            }
        }

        public static bool StopEnqueingNewEntries { get; private set; }

        public static bool StopLoggingRequested { get; private set; }

        public static Exception LastExceptionInBackgroundTask { get; private set; }

        public static int NumberOfLogEntriesWaitingToBeWrittenToFile
        {
            get
            {
                return SimpleLog._logEntryQueue.Count;
            }
        }

        public static bool LoggingStarted
        {
            get
            {
                return SimpleLog._backgroundTask != null;
            }
        }

        static SimpleLog()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(SimpleLog.CurrentDomainProcessExit);
        }

        private static void CurrentDomainProcessExit(object sender, EventArgs e)
        {
            SimpleLog.StopLogging(true);
        }

        public static Exception SetLogFile(
          string logDir = null,
          string prefix = null,
          string suffix = null,
          string extension = null,
          string dateFormat = null,
          SimpleLog.Severity? logLevel = null,
          bool? startExplicitly = null,
          bool check = true,
          bool? writeText = null,
          string textSeparator = null,
          int daysToDeleteFromPath = 7)
        {
            Exception exception = (Exception)null;
            try
            {
                SimpleLog._daysToDeleteFromPath = daysToDeleteFromPath;
                if (writeText.HasValue)
                    SimpleLog.WriteText = writeText.Value;
                if (textSeparator != null)
                    SimpleLog.TextSeparator = textSeparator;
                if (logLevel.HasValue)
                    SimpleLog.LogLevel = logLevel.Value;
                if (extension != null)
                    SimpleLog.Extension = extension;
                if (suffix != null)
                    SimpleLog.Suffix = suffix;
                if (dateFormat != null)
                    SimpleLog.DateFormat = dateFormat;
                if (prefix != null)
                    SimpleLog.Prefix = prefix;
                if (startExplicitly.HasValue)
                    SimpleLog.StartExplicitly = startExplicitly.Value;
                if (logDir != null)
                    exception = SimpleLog.SetLogDir(logDir, true);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            return exception;
        }

        public static Exception SetLogDir(string logDir, bool createIfNotExisting = false)
        {
            if (string.IsNullOrEmpty(logDir))
                logDir = Directory.GetCurrentDirectory();
            try
            {
                SimpleLog._logDir = new DirectoryInfo(logDir);
                if (!SimpleLog._logDir.Exists)
                {
                    if (!createIfNotExisting)
                        throw new DirectoryNotFoundException(string.Format("Directory '{0}' does not exist!", (object)SimpleLog._logDir.FullName));
                    SimpleLog._logDir.Create();
                }
            }
            catch (Exception ex)
            {
                return ex;
            }
            return (Exception)null;
        }

        public static Exception Info(string message, bool useBackgroundTask = true)
        {
            SimpleLog.cleanLogFolder();
            return SimpleLog.Log(message, SimpleLog.Severity.Info, true, 0);
        }

        public static Exception Warning(string message, bool useBackgroundTask = true)
        {
            SimpleLog.cleanLogFolder();
            return SimpleLog.Log(message, SimpleLog.Severity.Warning, true, 0);
        }

        public static Exception Error(string message, bool useBackgroundTask = true)
        {
            SimpleLog.cleanLogFolder();
            return SimpleLog.Log(message, SimpleLog.Severity.Error, true, 0);
        }

        public static Exception Log(Exception ex, bool useBackgroundTask = true, int framesToSkip = 0)
        {
            if (ex != null)
                return SimpleLog.Log(SimpleLog.GetExceptionXElement(ex), SimpleLog.Severity.Exception, useBackgroundTask, framesToSkip);
            return (Exception)null;
        }

        public static string GetExceptionAsXmlString(Exception ex)
        {
            XElement exceptionXelement = SimpleLog.GetExceptionXElement(ex);
            if (exceptionXelement != null)
                return exceptionXelement.ToString();
            return string.Empty;
        }

        public static XElement GetExceptionXElement(Exception ex)
        {
            if (ex == null)
                return (XElement)null;
            XElement xelement1 = new XElement((XName)"Exception");
            xelement1.Add((object)new XAttribute((XName)"Type", (object)ex.GetType().FullName));
            xelement1.Add((object)new XAttribute((XName)"Source", ex.TargetSite == (MethodBase)null || ex.TargetSite.DeclaringType == (Type)null ? (object)ex.Source : (object)string.Format("{0}.{1}", (object)ex.TargetSite.DeclaringType.FullName, (object)ex.TargetSite.Name)));
            xelement1.Add((object)new XElement((XName)"Message", (object)ex.Message));
            if (ex.Data.Count > 0)
            {
                XElement xelement2 = new XElement((XName)"Data");
                foreach (DictionaryEntry dictionaryEntry in ex.Data)
                    xelement2.Add((object)new XElement((XName)"Entry", new object[2]
                    {
            (object) new XAttribute((XName) "Key", dictionaryEntry.Key),
            (object) new XAttribute((XName) "Value", dictionaryEntry.Value ?? (object) string.Empty)
                    }));
                xelement1.Add((object)xelement2);
            }
            if (ex is SqlException)
            {
                SqlException sqlException = (SqlException)ex;
                XElement xelement2 = new XElement((XName)"SqlException");
                xelement2.Add((object)new XAttribute((XName)"ErrorNumber", (object)sqlException.Number));
                if (!string.IsNullOrEmpty(sqlException.Server))
                    xelement2.Add((object)new XAttribute((XName)"ServerName", (object)sqlException.Server));
                if (!string.IsNullOrEmpty(sqlException.Procedure))
                    xelement2.Add((object)new XAttribute((XName)"Procedure", (object)sqlException.Procedure));
                xelement1.Add((object)xelement2);
            }
            if (ex is COMException)
            {
                COMException comException = (COMException)ex;
                XElement xelement2 = new XElement((XName)"ComException");
                xelement2.Add((object)new XAttribute((XName)"ErrorCode", (object)string.Format("0x{0:X8}", (object)(uint)comException.ErrorCode)));
                xelement1.Add((object)xelement2);
            }
            if (ex is AggregateException)
            {
                XElement xelement2 = new XElement((XName)"AggregateException");
                foreach (Exception innerException in ((AggregateException)ex).InnerExceptions)
                    xelement2.Add((object)SimpleLog.GetExceptionXElement(innerException));
                xelement1.Add((object)xelement2);
            }
            xelement1.Add(ex.InnerException == null ? (object)new XElement((XName)"StackTrace", (object)ex.StackTrace) : (object)SimpleLog.GetExceptionXElement(ex.InnerException));
            return xelement1;
        }

        public static Exception Log(
          string message,
          SimpleLog.Severity severity = SimpleLog.Severity.Info,
          bool useBackgroundTask = true,
          int framesToSkip = 0)
        {
            if (!string.IsNullOrEmpty(message))
                return SimpleLog.Log(new XElement((XName)"Message", (object)message), severity, useBackgroundTask, framesToSkip);
            return (Exception)null;
        }

        public static Exception Log(
          XElement xElement,
          SimpleLog.Severity severity = SimpleLog.Severity.Info,
          bool useBackgroundTask = true,
          int framesToSkip = 0)
        {
            if (xElement != null)
            {
                if (severity >= SimpleLog.LogLevel)
                {
                    try
                    {
                        XElement xelement = new XElement((XName)"LogEntry");
                        xelement.Add((object)new XAttribute((XName)"Date", (object)DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        xelement.Add((object)new XAttribute((XName)"Severity", (object)severity));
                        xelement.Add((object)new XAttribute((XName)"Source", (object)SimpleLog.GetCaller(framesToSkip)));
                        xelement.Add((object)new XAttribute((XName)"ThreadId", (object)Thread.CurrentThread.ManagedThreadId));
                        xelement.Add((object)xElement);
                        if (!useBackgroundTask)
                            return SimpleLog.WriteLogEntryToFile(xelement);
                        SimpleLog.Enqueue(xelement);
                    }
                    catch (Exception ex)
                    {
                        return ex;
                    }
                    return (Exception)null;
                }
            }
            return (Exception)null;
        }

        public static string GetFileName(DateTime dateTime)
        {
            return string.Format("{0}\\{1}{2}{3}.{4}", (object)SimpleLog.LogDir, (object)SimpleLog.Prefix, (object)dateTime.ToString(SimpleLog.DateFormat), (object)SimpleLog.Suffix, (object)SimpleLog.Extension);
        }

        public static bool LogFileExists(DateTime dateTime)
        {
            return File.Exists(SimpleLog.GetFileName(dateTime));
        }

        public static XDocument GetLogFileAsXml()
        {
            return SimpleLog.GetLogFileAsXml(DateTime.Now);
        }

        public static XDocument GetLogFileAsXml(DateTime dateTime)
        {
            string fileName = SimpleLog.GetFileName(dateTime);
            if (!File.Exists(fileName))
                return (XDocument)null;
            SimpleLog.Flush();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            stringBuilder.AppendLine("<LogEntries>");
            stringBuilder.AppendLine(File.ReadAllText(fileName));
            stringBuilder.AppendLine("</LogEntries>");
            return XDocument.Parse(stringBuilder.ToString());
        }

        public static string GetLogFileAsText()
        {
            return SimpleLog.GetLogFileAsText(DateTime.Now);
        }

        public static string GetLogFileAsText(DateTime dateTime)
        {
            string fileName = SimpleLog.GetFileName(dateTime);
            if (!File.Exists(fileName))
                return (string)null;
            SimpleLog.Flush();
            return File.ReadAllText(fileName);
        }

        public static void ShowLogFile()
        {
            SimpleLog.ShowLogFile(DateTime.Now);
        }

        public static void ShowLogFile(DateTime dateTime)
        {
            string str;
            if (SimpleLog.WriteText)
            {
                SimpleLog.Flush();
                str = SimpleLog.GetFileName(dateTime);
            }
            else
            {
                str = string.Format("{0}Log_{1}.xml", (object)Path.GetTempPath(), (object)DateTime.Now.ToString("yyyyMMddHHmmssffff"));
                SimpleLog.GetLogFileAsXml(dateTime)?.Save(str);
            }
            if (!File.Exists(str))
                return;
            Process.Start(str);
            Thread.Sleep(2000);
        }

        public static void StartLogging()
        {
            if (SimpleLog._backgroundTask != null || SimpleLog.StopEnqueingNewEntries || SimpleLog.StopLoggingRequested)
                return;
            SimpleLog.StopEnqueingNewEntries = false;
            SimpleLog.StopLoggingRequested = false;
            lock (SimpleLog._backgroundTaskSyncRoot)
            {
                if (SimpleLog._backgroundTask != null)
                    return;
                SimpleLog.LastExceptionInBackgroundTask = (Exception)null;
                SimpleLog._backgroundTask = new Task(new Action(SimpleLog.WriteLogEntriesToFile), TaskCreationOptions.LongRunning);
                SimpleLog._backgroundTask.Start();
            }
        }

        public static void StopLogging(bool flush = true)
        {
            SimpleLog.StopEnqueingNewEntries = true;
            if (SimpleLog._backgroundTask == null)
                return;
            if (flush)
                SimpleLog.Flush();
            SimpleLog.StopLoggingRequested = true;
            lock (SimpleLog._backgroundTaskSyncRoot)
            {
                if (SimpleLog._backgroundTask == null)
                    return;
                SimpleLog._backgroundTask.Wait(1000);
                SimpleLog._backgroundTask = (Task)null;
            }
        }

        public static void Flush()
        {
            if (!SimpleLog.LoggingStarted)
                return;
            while (SimpleLog.NumberOfLogEntriesWaitingToBeWrittenToFile > 0)
            {
                int toBeWrittenToFile1 = SimpleLog.NumberOfLogEntriesWaitingToBeWrittenToFile;
                Thread.Sleep(222);
                int toBeWrittenToFile2 = SimpleLog.NumberOfLogEntriesWaitingToBeWrittenToFile;
                if (toBeWrittenToFile1 == toBeWrittenToFile2)
                    break;
            }
        }

        public static void ClearQueue()
        {
            lock (SimpleLog._logEntryQueue)
                SimpleLog._logEntryQueue.Clear();
        }

        public static void cleanLogFolder()
        {
            foreach (string file in Directory.GetFiles(SimpleLog.LogDir))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.LastAccessTime < DateTime.Now.AddDays((double)SimpleLog._daysToDeleteFromPath))
                    fileInfo.Delete();
            }
        }

        private static void Enqueue(XElement logEntry)
        {
            if (SimpleLog.StopEnqueingNewEntries)
                return;
            if (!SimpleLog.StartExplicitly)
                SimpleLog.StartLogging();
            lock (SimpleLog._logEntryQueue)
            {
                if (SimpleLog._logEntryQueue.Count >= 10000)
                    return;
                SimpleLog._logEntryQueue.Enqueue(logEntry);
            }
        }

        private static XElement Peek()
        {
            lock (SimpleLog._logEntryQueue)
                return SimpleLog._logEntryQueue.Count == 0 ? (XElement)null : SimpleLog._logEntryQueue.Peek();
        }

        private static void Dequeue()
        {
            lock (SimpleLog._logEntryQueue)
            {
                if (SimpleLog._logEntryQueue.Count <= 0)
                    return;
                SimpleLog._logEntryQueue.Dequeue();
            }
        }

        private static void WriteLogEntriesToFile()
        {
            while (!SimpleLog.StopLoggingRequested)
            {
                XElement xmlEntry = SimpleLog.Peek();
                if (xmlEntry == null)
                {
                    Thread.Sleep(100);
                }
                else
                {
                    for (int index = 0; index < 10; ++index)
                    {
                        Exception file = SimpleLog.WriteLogEntryToFile(xmlEntry);
                        SimpleLog.WriteOwnExceptionToEventLog(file);
                        SimpleLog.LastExceptionInBackgroundTask = file;
                        if (SimpleLog.LastExceptionInBackgroundTask != null && SimpleLog.NumberOfLogEntriesWaitingToBeWrittenToFile <= 1000)
                            Thread.Sleep(100);
                        else
                            break;
                    }
                    SimpleLog.Dequeue();
                }
            }
        }

        private static void WriteOwnExceptionToEventLog(Exception ex)
        {
            if (ex == null || SimpleLog.LastExceptionInBackgroundTask != null && ex.Message == SimpleLog.LastExceptionInBackgroundTask.Message)
                return;
            try
            {
                string message;
                try
                {
                    message = SimpleLog.GetExceptionXElement(ex).ToString();
                }
                catch
                {
                    message = ex.Message;
                }
                if (!EventLog.SourceExists(nameof(SimpleLog)))
                    EventLog.CreateEventSource(nameof(SimpleLog), "Application");
                EventLog.WriteEntry(nameof(SimpleLog), message, EventLogEntryType.Error, 0);
            }
            catch
            {
            }
        }

        private static Exception WriteLogEntryToFile(XElement xmlEntry)
        {
            if (xmlEntry == null)
                return (Exception)null;
            if (Monitor.TryEnter(SimpleLog._logFileSyncRoot, new TimeSpan(0, 0, 0, 5)))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(SimpleLog.FileName, FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                        {
                            if (SimpleLog.WriteText)
                                streamWriter.WriteLine(SimpleLog.ConvertXmlToPlainText(xmlEntry));
                            else
                                streamWriter.WriteLine((object)xmlEntry);
                        }
                    }
                    return (Exception)null;
                }
                catch (Exception ex)
                {
                    try
                    {
                        ex.Data[(object)"Filename"] = (object)SimpleLog.FileName;
                    }
                    catch
                    {
                    }
                    try
                    {
                        WindowsIdentity current = WindowsIdentity.GetCurrent();
                        ex.Data[(object)"Username"] = current == null ? (object)"unknown" : (object)current.Name;
                    }
                    catch
                    {
                    }
                    return ex;
                }
                finally
                {
                    Monitor.Exit(SimpleLog._logFileSyncRoot);
                }
            }
            else
            {
                try
                {
                    return new Exception(string.Format("Could not write to file '{0}', because it was blocked by another thread for more than {1} seconds.", (object)SimpleLog.FileName, (object)5));
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }

        private static string ConvertXmlToPlainText(XElement xmlEntry)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (XElement xelement in xmlEntry.DescendantsAndSelf())
            {
                if (xelement.HasAttributes)
                {
                    foreach (XAttribute attribute in xelement.Attributes())
                    {
                        if (stringBuilder.Length > 0)
                            stringBuilder.Append(SimpleLog.TextSeparator);
                        stringBuilder.Append((object)attribute.Name).Append(" = ").Append(attribute.Value);
                    }
                }
                else
                {
                    if (stringBuilder.Length > 0)
                        stringBuilder.Append(SimpleLog.TextSeparator);
                    string str = xelement.Value.Replace("\r\n", " ");
                    stringBuilder.Append((object)xelement.Name).Append(" = ").Append(str);
                }
            }
            return stringBuilder.ToString();
        }

        private static string GetCaller(int framesToSkip = 0)
        {
            string str = string.Empty;
            int num = 1;
            Type declaringType;
            do
            {
                MethodBase method = new StackFrame(num++).GetMethod();
                if (!(method == (MethodBase)null))
                {
                    declaringType = method.DeclaringType;
                    if (!(declaringType == (Type)null))
                        str = string.Format("{0}.{1}", (object)declaringType.FullName, (object)method.Name);
                    else
                        break;
                }
                else
                    break;
            }
            while (!(declaringType != typeof(SimpleLog)) || --framesToSkip >= 0);
            return str;
        }

        public enum Severity
        {
            Info,
            Warning,
            Error,
            Exception,
        }
    }
}