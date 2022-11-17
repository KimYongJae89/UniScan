using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynMvp.Data;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;

namespace DynMvp.InspData
{
    public enum Judgment
    {
        Accept, Reject, FalseReject, Skip, Warn
    }

    public class JudgementString
    {
        public static string ToLocaleString(Judgment judgment)
        {
            switch (judgment)
            {
                default:
                case Judgment.Reject:
                    return StringManager.GetString("JudgementString", "Reject");
                case Judgment.FalseReject:
                    return StringManager.GetString("JudgementString", "FalseReject");
                case Judgment.Accept:
                    return StringManager.GetString("JudgementString", "Accept");
            }
        }

        public static string GetString(Judgment judgment)
        {
            string result = "";
            switch (judgment)
            {
                case Judgment.Accept:
                case Judgment.FalseReject:
                    result = "GOOD";
                    break;
                case Judgment.Reject:
                    result = "NG";
                    break;
                case Judgment.Skip:
                    result = "Skip";
                    break;
                case Judgment.Warn:
                    result = "Warn";
                    break;
            }
            return result;
        }
    }

    public enum ResultImageType
    {
        TargetGroup, Target, Probe
    }

    public class ProbeResultValue
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private List<string> resultMessageList = new List<string>();
        public List<string> ResultMessageList
        {
            get { return resultMessageList; }
            set { resultMessageList = value; }
        }

        private object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private float ucl;
        public float Ucl
        {
            get { return ucl; }
            set { ucl = value; }
        }

        private float lcl;
        public float Lcl
        {
            get { return lcl; }
            set { lcl = value; }
        }

        private bool good;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        private string desiredString;
        public string DesiredString
        {
            get { return desiredString; }
            set { desiredString = value; }
        }


        public ProbeResultValue(string name, bool value)
        {
            this.name = name;
            this.value = this.good = value;
        }

        public ProbeResultValue(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public ProbeResultValue(string name, float ucl = 0, float lcl = 0, float value = 0, bool good = true)
        {
            this.name = name;
            this.ucl = ucl;
            this.lcl = lcl;
            this.value = value;
            this.good = good;
        }

        public ProbeResultValue(string name, string desiredString, string value = "", bool good = true)
        {
            this.name = name;
            this.desiredString = desiredString;
            this.value = value;
            this.good = good;
        }

        public ProbeResultValue(AlgorithmResultValue algorithmResultValue)
        {
            name = algorithmResultValue.Name;
            ucl = algorithmResultValue.Ucl;
            lcl = algorithmResultValue.Lcl;
            value = algorithmResultValue.Value;
            good = algorithmResultValue.Good;
            DesiredString = algorithmResultValue.DesiredString;
        }
    }

    public class ProbeResultValueList : List<ProbeResultValue>
    {
        public ProbeResultValue GetResultValue(string name)
        {
            return Find(x => x.Name == name);
        }
    }

    public abstract class ProbeResult
    {
        public const int DefaultSequence = -1;

        private Probe probe;
        public Probe Probe
        {
            get { return probe; }
            set { probe = value; }
        }

        int stepNo;
        public int StepNo
        {
            get { return (probe != null ? probe.Target.TargetGroup.InspectionStep.StepNo : stepNo); }
            set { stepNo = value; }
        }

        int groupId;
        public int GroupId
        {
            get { return (probe != null ? probe.Target.TargetGroup.GroupId : groupId); }
            set { groupId = value; }
        }

        int targetId;
        public int TargetId
        {
            get { return (probe != null ? probe.Target.Id : targetId); }
            set { targetId = value; }
        }

        string targetName;
        public string TargetName
        {
            get { return (probe != null ? probe.Target.Name : targetName); }
            set { targetName = value; }
        }

        string targetType;
        public string TargetType
        {
            get { return (probe != null ? probe.Target.TypeName : targetType); }
            set { targetType = value; }
        }

        int probeId;
        public int ProbeId
        {
            get { return (probe != null ? probe.Id : probeId); }
            set { probeId = value; }
        }

        string probeName;
        public string ProbeName
        {
            get { return (probe != null ? probe.Name : probeName); }
            set { probeName = value; }
        }

        ProbeType probeType;
        public ProbeType ProbeType
        {
            get { return (probe != null ? probe.ProbeType : probeType); }
            set { probeType = value; }
        }

        ProbeResultValueList resultValueList = new ProbeResultValueList();
        public ProbeResultValueList ResultValueList
        {
            get { return resultValueList; }
            set { resultValueList = value; }
        }

        protected MessageBuilder resultMessage = new MessageBuilder();
        public MessageBuilder ResultMessage
        {
            get { return resultMessage; }
            set { resultMessage = value; }
        }

        protected string shortResultMessage;
        public string ShortResultMessage
        {
            get { return shortResultMessage; }
            set { shortResultMessage = value; }
        }

        private int sequenceNo;
        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }

        private ImageD image;
        public ImageD Image
        {
            get { return image; }
            set { image = value; }
        }

        private Judgment judgment;
        public Judgment Judgment
        {
            get { return judgment; }
            set { judgment = value; }
        }

        private RotatedRect targetRegion;
        public RotatedRect TargetRegion
        {
            get { return targetRegion; }
            set { targetRegion = value; }
        }

        private RotatedRect regionInFov;
        public RotatedRect RegionInFov
        {
            get { return regionInFov; }
            set { regionInFov = value; }
        }

        private RotatedRect regionInTarget;
        public RotatedRect RegionInTarget
        {
            get { return regionInTarget; }
            set { regionInTarget = value; }
        }

        private RotatedRect regionInProbe;
        public RotatedRect RegionInProbe
        {
            get { return regionInProbe; }
            set { regionInProbe = value; }
        }

        private bool differentProductDetected;
        public bool DifferentProductDetected
        {
            get { return differentProductDetected; }
            set { differentProductDetected = value; }
        }

        public float GetResultValueUcl(string valueName)
        {
            ProbeResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
                return resultValue.Ucl;

            return 0;
        }

        public float GetResultValueLcl(string valueName)
        {
            ProbeResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
                return resultValue.Lcl;

            return 0;
        }

        public float GetResultValueFloat(string valueName)
        {
            float floatValue = 0;
            ProbeResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
                floatValue = Convert.ToSingle(resultValue.Value);

            return floatValue;
        }

        public string GetResultValueString(string valueName)
        {
            ProbeResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
                return resultValue.Value.ToString();

            return "";
        }

        public bool GetResultValueGood(string valueName)
        {
            ProbeResultValue resultValue = GetResultValue(valueName);
            if (resultValue != null)
                return resultValue.Good;

            return true;
        }

        public virtual void InvertJudgment()
        {
            if (judgment == Judgment.Accept)
            {
                judgment = Judgment.Reject;
            }
            else
            {
                judgment = Judgment.Accept;
            }
        }

        public static ProbeResult CreateProbeResult(ProbeType probeType)
        {
            ProbeResult probeResult = null;
            switch (probeType)
            {
                case ProbeType.Vision:
                    probeResult = new VisionProbeResult();
                    break;
                case ProbeType.Serial:
                    probeResult = new SerialProbeResult();
                    break;
                case ProbeType.Io:
                    probeResult = new IoProbeResult();
                    break;
                case ProbeType.Daq:
                    probeResult = new DaqProbeResult();
                    break;
                case ProbeType.Tension:
                    probeResult = new TensionSerialProbeResult();
                    break;
            }

            if (probeResult != null)
                probeResult.ProbeType = probeType;

            return null;
        }

        public abstract void BuildResultMessage(MessageBuilder totalResultMessage);
        public virtual void AppendResultFigures(FigureGroup figureGroup, bool useTargetCoord = true)
        {
            Pen pen;
            if (judgment == Judgment.Accept)
                pen = new Pen(Color.LightGreen, 2.0F);
            else if (judgment == Judgment.FalseReject)
                pen = new Pen(Color.Yellow, 2.0F);
            else
                pen = new Pen(Color.Red, 2.0F);

            RectangleF resultRect = Probe.BaseRegion.GetBoundRect();
            //if (useTargetCoord == true)
            //    resultRect.Offset(-Probe.Target.Region.X, -Probe.Target.Region.Y);

            RectangleFigure figure = new RectangleFigure(resultRect, pen);
            figure.Selectable = false;
            figure.Tag = this;

            figureGroup.AddFigure(figure);
        }

        public virtual void AppendResultFigures(FigureGroup figureGroup, ResultImageType resultImageType)
        {
            Pen pen;
            if (judgment == Judgment.Accept)
                pen = new Pen(Color.LightGreen, 2.0F);
            else if (judgment == Judgment.FalseReject)
                pen = new Pen(Color.Yellow, 2.0F);
            else
                pen = new Pen(Color.Red, 2.0F);

            //RectangleF resultRect = Probe.Region.GetBoundRect();
            //if (useTargetCoord == true)
            //    resultRect.Offset(-Probe.Target.Region.X, -Probe.Target.Region.Y);

            RectangleFigure figure;
            if (resultImageType == ResultImageType.TargetGroup)
                figure = new RectangleFigure(regionInFov, pen);
            else if (resultImageType == ResultImageType.Target)
                figure = new RectangleFigure(regionInTarget, pen);
            else
                figure = new RectangleFigure(regionInProbe, pen);

            figure.Tag = this;

            figureGroup.AddFigure(figure);
        }

        public void AddResultValue(ProbeResultValue probeResultValue)
        {
            resultValueList.Add(probeResultValue);
        }

        public ProbeResultValue GetResultValue(string valueName)
        {
            return resultValueList.Find(x => { return x.Name == valueName; });
        }
    }
}
