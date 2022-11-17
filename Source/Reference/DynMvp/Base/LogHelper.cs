using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace DynMvp.Base
{
    public enum LoggerType
    {
        // Program Initialize
        StartUp,    
        Shutdown,

        // Communicate message. ex) timeout, disconnect, ...
        Network,
        Serial,
        IO,

        // H/W message. ex) motion control, light command, ...
        Machine,
        Device,         // 장비 운영 로그 ( Motion / Grab 제외 )

        // S/W message
        UserNotify, // user notify. ex) messageBox
        Operation,  // S/W operation. ex) model load, result save, update control, ...
        Grab,   // grab step
        Inspection, // inspection algorithm.
        ImageProcessing,
        Algorithm,

        DataRemover,    // old data removed

        ValueChanged,   // user parametor changed

        //Debug,  // any developer log
        Error,  // dummy
        Function,       // 함수 호출. Start / End
    }

    public enum LogLevel
    {
        Fatal, Error, Warn, Info, Debug, Trace
    }

    public static class ILogExtentions
    {
        public static void Trace(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Trace, message, exception);
        }

        public static void Trace(this ILog log, string message)
        {
            log.Trace(message, null);
        }

        public static void Verbose(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Verbose, message, exception);
        }

        public static void Verbose(this ILog log, string message)
        {
            log.Verbose(message, null);
        }
    }

    public delegate void LogDelegate(string message);

    public interface LoggingTarget
    {
        void Log(string message);
    }

    public class LogHelper
    {
        public static string BackupPathForamt = "yyyyMMddHHmmss";

        static LoggingTarget loggingTarget = null;
        public static LoggingTarget LoggingTarget
        {
            get { return loggingTarget;  }
            set { loggingTarget = value; }
        }

        public static bool InitializeLogSystem(string logConfigFile)
        {
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(logConfigFile);
            if(!fileInfo.Exists)
                return false;

            log4net.Config.XmlConfigurator.Configure(fileInfo);

            string initString;
            string assName = System.Reflection.Assembly.GetEntryAssembly().FullName;
            string[] toekns = assName.Split(',');
            if (toekns.Length == 4)
                initString = $"Assembly: {toekns[0]}, Version: {VersionHelper.Instance().VersionString}, Build: {VersionHelper.Instance().BuildString}";
            else
                initString = $"Version: {VersionHelper.Instance().VersionString}, Build: {VersionHelper.Instance().BuildString}";
            Info(LoggerType.Operation, $"InitializeLogSystem - {initString}");
            return true;
        }

        public static void Fatal(LoggerType loggerType,string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Fatal(message);
            if(writeOutputWindow)
                WriteOutputWindow(LogLevel.Fatal, loggerType, message);
            loggingTarget?.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }

        public static void Error(LoggerType loggerType, string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Error(message);
            if(writeOutputWindow)
                WriteOutputWindow(LogLevel.Error, loggerType, message);
            loggingTarget?.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }

        public static void Error(LoggerType loggerType, Exception ex, bool writeOutputWindow = false)
        {
            StackFrame stackFrame = new System.Diagnostics.StackFrame(1, true);
            MethodBase mb = stackFrame.GetMethod();
            string funcName = mb.Name;
            string className = mb.ReflectedType.Name;
            string message = $"{className}::{funcName} - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}";

            Error(loggerType, message, writeOutputWindow);

            if (ex is AggregateException)
            {
                AggregateException aggregateException = (AggregateException)ex;
                aggregateException.InnerExceptions.ToList().ForEach(f => Error(loggerType, f));
            }
            else
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    Error(loggerType, ex);
                }
            }
        }

        public static void Warn(LoggerType loggerType, string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Warn(message);
            if(writeOutputWindow)
                WriteOutputWindow(LogLevel.Warn, loggerType, message);
            loggingTarget?.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }

        public static void Info(LoggerType loggerType, string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Info(message);
            if(writeOutputWindow)
                WriteOutputWindow(LogLevel.Info, loggerType, message);
            loggingTarget?.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }

        public static void Debug(LoggerType loggerType, string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Debug(message);
            if (writeOutputWindow)
                WriteOutputWindow(LogLevel.Debug, loggerType, message);
            loggingTarget?.Log(string.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }
        
        public static void Trace(LoggerType loggerType, string message, bool writeOutputWindow = false)
        {
            LogManager.GetLogger(loggerType.ToString()).Trace(message);
            if(writeOutputWindow)
                WriteOutputWindow(LogLevel.Error, loggerType, message);
            loggingTarget?.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
        }

        private static void WriteOutputWindow(LogLevel logLevel, LoggerType loggerType, string message)
        {
            DynMvp.ConsoleEx.WriteLine(string.Format("[{0}] {1} [{2}] {3}", logLevel.ToString(), DateTime.Now.ToString("HH:mm:ss.fff"), loggerType.ToString(), message));
        }

        public static void ChangeLevel(string logLevel)
        {
            return;

            log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            //Configure all loggers to be at the debug level.
            foreach (log4net.Repository.ILoggerRepository repository in repositories)
            {
                //repository.Threshold = repository.LevelMap[logLevel];
                log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
                log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
                foreach (log4net.Core.ILogger logger in loggers)
                {
                    ((log4net.Repository.Hierarchy.Logger)logger).Level = hier.LevelMap[logLevel];
                }
            }

            //Configure the root logger.
            log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            //rootLogger.Level = h.LevelMap[logLevel];

            //TestLog();
        }
    }
}
