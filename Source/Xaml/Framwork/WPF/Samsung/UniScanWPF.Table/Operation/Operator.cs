using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using UniEye.Base.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public enum ResultType
    {
        LightTune, Scan, Extract, Inspect, InspectLump, Train
    }

    public enum OperatorState
    {
        Run, Wait, Idle, Pause
    }

    public abstract class Operator : INotifyPropertyChanged
    {
        const float dispResizeRatio = 0.2f;
        public static float DispResizeRatio { get => dispResizeRatio; }

        public ResultKey ResultKey => this.resultKey;
        protected ResultKey resultKey;

        protected CancellationTokenSource cancellationTokenSource;

        OperatorState operatorState;

        protected const float debugImageScale = 0.5f;

        public int TotalProgressSteps { get => totalProgressSteps; }
        int totalProgressSteps = 0;

        public int CurProgressSteps
        {
            get => curProgressSteps;
            set
            {
                curProgressSteps = value;
                OnPropertyChanged("Progress");
            }
        }
        int curProgressSteps = 0;

        public int Progress { get => totalProgressSteps < 0 || operatorState != OperatorState.Run ? 100 :
                Math.Min(100, (int)Math.Round(curProgressSteps * 100.0 / totalProgressSteps)); }

        public SolidColorBrush DoneBrush { get => doneBrush; }
        SolidColorBrush doneBrush = new SolidColorBrush(Colors.Green);

        public SolidColorBrush RunBrush { get => runBrush; }
        SolidColorBrush runBrush = new SolidColorBrush(Colors.LightGreen);

        public SolidColorBrush WaitBrush { get => waitBrush;}
        SolidColorBrush waitBrush = new SolidColorBrush(Colors.CornflowerBlue);

        public SolidColorBrush IdleBrush { get => idleBrush; }
        SolidColorBrush idleBrush = new SolidColorBrush(Colors.LightGray);

        public SolidColorBrush PauseBrush { get => pauseBrush;  }
        SolidColorBrush pauseBrush = new SolidColorBrush(Colors.Gold);

        public event PropertyChangedEventHandler PropertyChanged;

        public SolidColorBrush StateBrushBg
        {
            get
            {
                switch (operatorState)
                {
                    case OperatorState.Run:
                        return runBrush;

                    case OperatorState.Wait:
                        return waitBrush;

                    case OperatorState.Idle:
                        return idleBrush;

                    case OperatorState.Pause:
                        return pauseBrush;
                }

                return null;
            }
        }

        public SolidColorBrush StateBrushFg
        {
            get
            {
                switch (operatorState)
                {
                    case OperatorState.Run:
                        return doneBrush;
                    default:
                        return StateBrushBg;
                }
            }
        }

        public OperatorState OperatorState
        {
            get => operatorState;
            set
            {
                if (operatorState != value)
                {
                    operatorState = value;
                    OnPropertyChanged("StateBrush");
                    OnPropertyChanged("StateBrushBg");
                    OnPropertyChanged("StateBrushFg");
                    OnPropertyChanged("OperatorState");
                    OnPropertyChanged("OperatorStateString");
                    OnPropertyChanged("Progress");

                    Table.Data.InfoBox.Instance.OperatorStateChanged();
                }
            }
        }

        public string OperatorStateString
        {
            get => LocalizeHelper.GetString(operatorState.ToString());

        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public Operator()
        {
            OperatorState = OperatorState.Idle;
        }

        public virtual void Release()
        {
            this.CurProgressSteps = -1;
            OperatorState = OperatorState.Idle;
        }

        public virtual bool Initialize(ResultKey resultKey, int totalProgressSteps, CancellationTokenSource cancellationTokenSource)
        {
            this.resultKey = resultKey;
            this.cancellationTokenSource = cancellationTokenSource;
            this.totalProgressSteps = totalProgressSteps;
            this.CurProgressSteps = 0;

            OperatorState = OperatorState.Wait;
           
            return true;
        }

        protected void DebugWriteLine(string v)
        {
            Debug.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss.fff"), v));
        }

        protected DebugContext GetDebugContext(string subPath = null)
        {
            string fullPath = PathSettings.Instance().Temp;
            //fullPath = @"C:\temp";
            if (string.IsNullOrEmpty(subPath) == false)
                fullPath = Path.Combine(fullPath, subPath);
            //Directory.CreateDirectory(fullPath);
            return new DebugContext(OperationSettings.Instance().SaveDebugImage, fullPath);
        }
    }

    public abstract class OperatorResult
    {
        public ResultType Type { get => this.type; }
        protected ResultType type;

        public ResultKey ResultKey { get => this.resultKey; }
        protected ResultKey resultKey;

        public Exception Exception { get => this.exception; }
        protected Exception exception;

        public DateTime DateTime { get => this.dateTime; }
        protected DateTime dateTime;

        public bool IsError => this.exception != null;

        public OperatorResult(ResultType type, ResultKey resultKey, DateTime dateTime, Exception exception = null)
        {
            this.type = type;
            this.resultKey = resultKey;
            this.dateTime = dateTime;
            this.exception = exception;
        }

        protected abstract string GetLogMessage();
        public string GetLog(string head)
        {
            return string.Format("{0}LOG:{1},{2},{3},{4}",
              head, this.type, this.dateTime.ToString("yyyy/MM/dd"), this.dateTime.ToString("HH:mm:ss"),
              GetLogMessage());
        }
    }

    public abstract class OperatorSettings : ISavableObj, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected string fileName;

        public OperatorSettings()
        {
            Initialize();
            Load();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Set<T>(string propName, ref T target, T value)
        {
            target = value;
            OnPropertyChanged(propName);
        }

        public void Set<T>(string propName, ref T target, T value, params string[] onChangedPropName)
        {
            target = value;
            OnPropertyChanged(propName);
            Array.ForEach(onChangedPropName, f => OnPropertyChanged(f));
        }

        public void Load(string fileName = "")
        {
            bool ok = false;
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = this.fileName;

                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Settings"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
                ok = true;
            }
            finally
            {
                //if (ok == false)
                //    Save();
            }
        }

        public void Save(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.fileName;

            string superDirectory = Path.GetDirectoryName(fileName);
            if (Directory.Exists(superDirectory) == false)
                Directory.CreateDirectory(superDirectory);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(fileName);
        }

        protected abstract void Initialize();
        public abstract void Load(XmlElement xmlElement);
        public abstract void Save(XmlElement xmlElement);
    }
}
