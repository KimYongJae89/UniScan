using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace MIL_vs_CUDA.Processer
{
    public delegate void AddLogDelegate(LogItemType logItemType, string message, DateTime dateTime, bool isStartLog);

    public class ProcessInput
    {
        public AddLogDelegate OnLogAdded = null;

        public ProcesserType ProcesserType { get => processerType; }
        ProcesserType processerType;

        public string Name { get => name; }
        string name;

        public Image2D Image2D { get => this.image2D; }
        Image2D image2D;

        public ProcessBufferSetG ProcessBufferSet { get => this.processBufferSet; }
        ProcessBufferSetG processBufferSet;

        public CheckState UseParallel { get => this.useParallel; }
        CheckState useParallel;

        public ProcessInput(Image2D image2D, ProcessBufferSetG processBufferSet, string name, ProcesserType processerType, CheckState useParallel)
        {
            this.image2D = image2D;
            this.processBufferSet = processBufferSet;

            this.name = name;
            this.processerType = processerType;

            this.useParallel = useParallel;
        }
    }

    public enum LogItemType { Start, Build, Transfer, Process_Calc, Process_Detect, Save, Dispose, End, Error }
    public class ProcessOutput
    {
        public string Name { get => this.name; }
        string name;

        public DateTime StartDateTime { get => this.dtartDateTime; }
        DateTime dtartDateTime;

        public InspectionResult InspectionResult { get => this.inspectionResult; set => this.inspectionResult = value; }
        InspectionResult inspectionResult;

        public int DetectsCount { get => this.detectsCount; set => this.detectsCount = value; }
        int detectsCount = 0;

        public bool IsSuccess { get => !this.processLog.Exists(f=>f.Item1 == LogItemType.Error); }

        public double TotalTimeS
        {
            get => processLog.Count == 0 ? 0 : (float)((processLog.LastOrDefault().Item3 - processLog.FirstOrDefault().Item3).TotalSeconds);
        }
         
        public double BuildTimeS
        {
            get => processLog.FindAll(f => f.Item1 == LogItemType.Build).Sum(f => f.Item4) / 1000;
        }

        public double TransferTimeS
        {
            get => processLog.FindAll(f => f.Item1 == LogItemType.Transfer).Sum(f => f.Item4) / 1000;
        }

        public double ProcessTimeS
        {
            get => processLog.FindAll(f =>
            f.Item1 == LogItemType.Process_Calc ||
            f.Item1 == LogItemType.Process_Detect ||
            f.Item1 == LogItemType.Transfer).Sum(f => f.Item4) / 1000;
        }

        public double ProcessCalcTimeS
        {
            get => processLog.FindAll(f => f.Item1 == LogItemType.Process_Calc).Sum(f => f.Item4) / 1000;
        }

        public double ProcessDetectTimeS
        {
            get => processLog.FindAll(f => f.Item1 == LogItemType.Process_Detect).Sum(f => f.Item4) / 1000;
        }

        public double SaveTimeS
        {
            get => processLog.FindAll(f => f.Item1 == LogItemType.Save).Sum(f => f.Item4) / 1000;
        }

        List<Tuple<LogItemType, string, DateTime, double>> processLog = new List<Tuple<LogItemType, string, DateTime, double>>();

        public ProcessOutput(string name)
        {
            this.name = name;
            this.dtartDateTime = DateTime.Now;
        }

        public void AddLog(LogItemType logItemType, string message)
        {
            AddLog(logItemType, message, DateTime.Now);
        }

        public void AddLog(LogItemType logItemType,  string message, DateTime dateTime)
        {
            Tuple<LogItemType, string, DateTime, double> lastItem = processLog.LastOrDefault();
            double timeSpan = 0;
            if (lastItem != null)
                timeSpan = (dateTime - lastItem.Item3).TotalMilliseconds;
            processLog.Add(new Tuple<LogItemType, string, DateTime, double>(logItemType, message, dateTime, timeSpan));
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}",
                this.dtartDateTime.ToString("yyyy.MM.dd HH:mm:ss"),
                this.name,
                this.BuildTimeS,
                this.ProcessTimeS,
                this.SaveTimeS,
                this.TotalTimeS
                );
        }
    }
}
