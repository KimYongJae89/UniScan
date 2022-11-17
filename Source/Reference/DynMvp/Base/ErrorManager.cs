using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class ErrorItem
    {
        public int ErrorCode => this.errorCode;
        int errorCode;

        public string SectionStr => this.sectionStr;
        string sectionStr;

        public string ErrorStr => this.errorStr;
        string errorStr;

        public object[] Argument => argument;
        object[] argument;

        public string TargetName => targetName;
        string targetName;

        public string Message => this.message;
        string message;

        public string FormattedMessage
        {
            get
            {
                string message = (string)this.message.Clone();
                if (argument != null)
                    message = string.Format(this.message, argument);
                return message;
            }
        }

        public string LocaledMessage
        {
            get
            {
                string localedMessage = StringManager.GetString(this.GetType().FullName, message);
                if (argument != null)
                    localedMessage = string.Format(localedMessage, argument);
                return localedMessage;
            }
        }

        public ErrorLevel ErrorLevel => this.errorLevel;
        ErrorLevel errorLevel;

        public bool IsAlarm => errorLevel < ErrorLevel.Info;

        public bool IsShowen { get => this.isShowen; private set => this.isShowen = value; }
        bool isShowen;

        public bool IsCleared => isCleared;
        bool isCleared;

        public DateTime ErrorTime => this.errorTime;
        DateTime errorTime;

        public ErrorItem(ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string message, object[] argument)
        {
            this.errorTime = DateTime.Now;
            this.errorCode = errorCode.Code;
            this.errorLevel = errorLevel;
            this.sectionStr = errorCode.ErrorSubSection.Message;
            this.errorStr = errorCode.Message;
            this.targetName = targetName;
            this.message = message;
            this.argument = argument;

            this.isShowen = errorLevel > ErrorLevel.Warning;
            this.isCleared = errorLevel > ErrorLevel.Warning;
        }

        public ErrorItem(bool isCleared, string message)
        {
            this.isCleared = isCleared;
            this.message = message;
        }

        public ErrorItem()
        {
        }

        public ErrorItem Clone()
        {
            ErrorItem errorItem = new ErrorItem();
            errorItem.SetData(this.ToString());
            errorItem.errorTime = DateTime.Now;
            return errorItem;
        }

        public bool SetData(string valueStr)
        {
            string[] tokens = valueStr.Replace("\t", Environment.NewLine).Split(';');
            try
            {
                errorTime = DateTime.ParseExact(tokens[0], "yyyy/MM/dd HH:mm:ss", null);
                errorCode = Convert.ToInt32(tokens[1]);
                errorLevel = (ErrorLevel)Enum.Parse(typeof(ErrorLevel), tokens[2]);
                sectionStr = tokens[3];
                errorStr = tokens[4];
                if (tokens.Length > 6)
                {
                    this.targetName = tokens[5];
                    this.message = tokens[6];
                    if (tokens.Length > 7)
                        this.argument = tokens[7].Split(',');
                }
                else
                {
                    this.message = tokens[5];
                    this.argument = tokens.Skip(6).ToArray();
                }
                this.isShowen = true;
                this.isCleared = true;

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("Exception in ErrorItem::SetData({0}) - {1}", valueStr, ex.Message));
                return false;
            }
        }

        public void SetClear()
        {
            if (!this.isCleared)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("ErrorItem::SetClear - {0}", this.ToString()));
                this.isShowen = true;
                this.isCleared = true;
            }
        }

        public void SetShowen(bool value)
        {
            if (!this.isShowen)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("ErrorItem::SetShowen {0} - {1}", value, this.ToString()));
                this.isShowen = value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                this.errorTime.ToString("yyyy/MM/dd HH:mm:ss"),
                this.errorCode,
                this.errorLevel,
                this.sectionStr,
                this.errorStr,
                this.targetName,
                this.message,
                this.argument == null ? "" : string.Join(",", this.argument)).Replace(Environment.NewLine, "\t");
        }

        public override bool Equals(object obj)
        {
            ErrorItem errorItem = obj as ErrorItem;
            if (errorItem == null)
                return base.Equals(obj);

            return errorItem.errorCode == this.errorCode && errorItem.errorLevel == this.errorLevel;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AlarmException : ApplicationException
    {
        public override string Message => this.argument == null ? this.messageFormat : string.Format(this.messageFormat, this.argument);

        public ErrorCode ErrorCode => errorCode;
        ErrorCode errorCode;

        public ErrorLevel ErrorLevel => errorLevel;
        ErrorLevel errorLevel;

        public string TargetName => targetName;
        string targetName;

        public object[] Argument => argument;
        object[] argument;

        public string Solution => solution;
        string solution;

        string messageFormat;

        public AlarmException(ErrorCode errorCode, ErrorLevel errorLevel) : base("")
        {
            Init(false, errorCode, errorLevel, errorCode.ErrorSubSection.Message, "", null, "");
        }

        public AlarmException(ErrorCode errorCode, ErrorLevel errorLevel, string targetName) : base("")
        {
            Init(false, errorCode, errorLevel, targetName, "", null, "");
        }

        public AlarmException(ErrorCode errorCode, ErrorLevel errorLevel, string messageFormat, object[] argument, string solution) : base(messageFormat)
        {
            Init(true, errorCode, errorLevel, errorCode.ErrorSubSection.Message, messageFormat, argument, solution);
        }

        public AlarmException(ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string messageFormat, object[] argument, string solution) : base(messageFormat)
        {
            Init(true, errorCode, errorLevel, targetName, messageFormat, argument, solution);
        }

        private void Init(bool writeLog, ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string messageFormat, object[] argument, string solution)
        {
            this.errorCode = errorCode;
            this.errorLevel = errorLevel;
            this.targetName = targetName;
            this.messageFormat = messageFormat;
            this.argument = argument;
            this.solution = solution;

            if (writeLog)
            {
                string mm = string.Format("{0}: {1} in {2}, {3}", errorLevel, errorCode.ToString(), targetName, Message);
                DynMvp.ConsoleEx.WriteLine(mm);

                LoggerType loggerType = errorLevel.IsAlarm() ? LoggerType.Error : LoggerType.Operation;
                string logMessage = string.Format("{0};{1};{2};{3};{4};{5};{6}", errorCode.Code, errorLevel, errorCode.ErrorSubSection.ErrorSection.Message, errorCode.ErrorSubSection.Message, targetName, Message, solution);
                switch (loggerType)
                {
                    case LoggerType.Error:
                        LogHelper.Error(LoggerType.Error, logMessage);
                        break;

                    default:
                        LogHelper.Debug(loggerType, logMessage);
                        break;
                }
            }
        }

        public void ShowMessageBox()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("!! ERROR !!");
            sb.AppendLine(string.Format("Code   : {0:D04}", this.errorCode.Code));
            sb.AppendLine(string.Format("Section: {0} / {1}", this.errorCode.ErrorSubSection.ErrorSection.Message, this.errorCode.ErrorSubSection.Message));
            sb.AppendLine(string.Format("Type   : {0}", this.errorCode.Message));
            sb.AppendLine(string.Format("Device : {0}", this.targetName));
            sb.AppendLine();
            sb.AppendLine(this.Message);

            MessageBoxIcon icon;
            switch (this.errorLevel)
            {
                default:
                case ErrorLevel.Info:
                    icon = MessageBoxIcon.Information;
                    break;
                case ErrorLevel.Warning:
                    icon = MessageBoxIcon.Warning;
                    break;
                case ErrorLevel.Error:
                case ErrorLevel.Fatal:
                    icon = MessageBoxIcon.Error;
                    break;
            }

            MessageBox.Show(sb.ToString(), "UniScan", MessageBoxButtons.OK, icon);
        }

        public ErrorItem GetErrorItem()
        {
            return new ErrorItem(this.errorCode, this.errorLevel, this.targetName, this.messageFormat, this.argument);
        }
    }

    public delegate void AlarmEventDelegate();

    public class ErrorManager : INotifyCollectionChanged
    {
        static ErrorManager instance = null;

        List<Task> taskList;
        public event AlarmEventDelegate OnStartAlarmState;
        public event AlarmEventDelegate OnResetAlarmState;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        internal void StopProcess()
        {

        }

        string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        bool buzzerOn;
        public bool BuzzerOn
        {
            get { return buzzerOn; }
            set { buzzerOn = value; }
        }

        List<ErrorItem> errorItemList = new List<ErrorItem>();
        public List<ErrorItem> ErrorItemList
        {
            get { return errorItemList; }
        }

        public object LockObject => lockObject;
        object lockObject = new object();

        private ErrorManager()
        {
            this.taskList = new List<Task>();
        }

        public static ErrorManager Instance()
        {
            if (instance == null)
                instance = new ErrorManager();

            return instance;
        }

        public void ResetAllAlarm()
        {
            lock (lockObject)
            {
                errorItemList.ForEach(f => f.SetClear());
            }

            OnResetAlarmState?.Invoke();
            this.buzzerOn = false;
        }

        public void StopAllAlarm()
        {
            lock (lockObject)
            {
                errorItemList.ForEach(f => f.SetShowen(true));
                this.buzzerOn = false;
            }
        }

        public void ResetAlarm(ErrorItem errorItem)
        {
            errorItem.SetClear();

            if (IsCleared())
            {
                OnResetAlarmState?.Invoke();
                this.buzzerOn = true;
            }
        }

        public bool IsCleared()
        {
            lock (lockObject)
                return !errorItemList.Select(f => f.ErrorLevel.IsAlarm() ? f.IsCleared : true).Contains(false);
        }

        public bool IsAlarmed()
        {
            return !IsCleared();
        }

        public ErrorItem GetLastAlarmItem()
        {
            //return errorItemList.FirstOrDefault(f => f.ErrorLevel.IsAlarm() && !f.IsCleared);
            return errorItemList.FirstOrDefault(f => f.ErrorLevel.IsAlarm());
        }

        public bool IsShowen()
        {
            lock (lockObject)
                return errorItemList.TrueForAll(item => item.IsShowen);
        }

        public void ThrowIfAlarm()
        {
            if (!IsCleared())
                throw new Exception();
        }

        public bool IsError()
        {
            lock (lockObject)
                return errorItemList.Any(item => item.IsCleared == false && item.ErrorLevel == ErrorLevel.Error);
        }

        public bool IsWarning()
        {
            lock (lockObject)
                return errorItemList.Any(item => item.IsCleared == false && item.ErrorLevel == ErrorLevel.Warning);
        }

        public void Report(ErrorCode errorCode, ErrorLevel errorLevel, string message, object[] argument)
        {
            Report(new AlarmException(errorCode, errorLevel, errorCode.ErrorSubSection.Message, message, argument, ""));
        }

        public void Report(ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string message, object[] argument)
        {
            Report(new AlarmException(errorCode, errorLevel, targetName, message, argument, ""));
        }

        public void Report(ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string message, object[] argument, string solution)
        {
            Report(new AlarmException(errorCode, errorLevel, targetName, message, argument, solution));
        }

        public void Report(AlarmException exception)
        {
            ErrorItem errorItem = exception.GetErrorItem();
            Report(errorItem);
        }

        public void Report(ErrorItem errorItem)
        {
            lock (lockObject)
                errorItemList.Insert(0, errorItem);
            this.buzzerOn = true;

            Task task = new Task(new Action<object>(TaskProc), errorItem);
            task.ContinueWith((t) =>
            {
                lock (this.taskList)
                    this.taskList.Remove(t);
            });

            lock (this.taskList)
                this.taskList.Add(task);

            task.Start();
        }

        private void TaskProc(object arg)
        {
            ErrorItem errorItem = (ErrorItem)arg;
            if (errorItem.ErrorLevel <= ErrorLevel.Error)
                Task.Run(() => OnStartAlarmState?.Invoke());

            lock (lockObject)
                SaveErrorList();

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));            
        }

        public void WaitReportDone()
        {
            while (this.taskList.Count > 0)
                System.Threading.Thread.Sleep(100);
        }

        public void LoadErrorList(string configPath)
        {
            fileName = configPath + "\\ErrorList.txt";
            if (File.Exists(fileName) == false)
                return;

            try
            {
                string[] stringLines = File.ReadAllLines(fileName);
                lock (errorItemList)
                {
                    Array.ForEach(stringLines, f =>
                    {
                        ErrorItem errorItem = new ErrorItem();
                        if (errorItem.SetData(f))
                            errorItemList.Add(errorItem);
                    });

                    errorItemList.Sort((f, g) => g.ErrorTime.CompareTo(f.ErrorTime));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("Exception in ErrorManager::LoadErrorList - {0}", ex.Message));
            }
        }

        public void SaveErrorList()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ErrorItem errorItem in errorItemList)
            {
                stringBuilder.AppendLine(errorItem.ToString());
            }

            if (string.IsNullOrEmpty(fileName))
                return;

            File.WriteAllText(fileName, stringBuilder.ToString());
        }
    }
}
