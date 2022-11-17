using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision
{
    public class AlgorithmResultValue : IDisposable
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string valueType;
        public string ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }

        private object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        private bool good;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        private object desiredValue;
        public object DesiredValue
        {
            get { return desiredValue; }
            set { this.desiredValue = value; }
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

        private string desiredString;
        public string DesiredString
        {
            get { return desiredString; }
            set { desiredString = value; }
        }

        public AlgorithmResultValue(string name, object value)
        {
            this.name = name;
            this.value = value;
            this.good = true;
            valueType = value.GetType().Name;
        }

        public AlgorithmResultValue(string name, object desiredValue, object value)
        {
            this.name = name;
            this.desiredValue = desiredValue;
            this.value = value;
            this.good = true;
            valueType = value.GetType().Name;
        }

        public AlgorithmResultValue(string name, float ucl, float lcl, float value, bool good = true)
        {
            this.name = name;
            this.ucl = ucl;
            this.lcl = lcl;
            this.value = value;
            this.good = good;
            valueType = value.GetType().Name;
        }

        public AlgorithmResultValue(string name, string desiredString, string value)
        {
            this.name = name;
            this.desiredString = desiredString;
            this.value = value;
            this.good = true;
            valueType = value.GetType().Name;
        }

        public AlgorithmResultValue(string name, string desiredString, List<string> value)
        {
            this.name = name;
            this.desiredString = desiredString;
            this.value = value;
            this.good = true;
            valueType = value[0].GetType().Name;
        }

        public AlgorithmResultValue(string name, string value = "")
        {
            this.name = name;
            this.value = value;
            valueType = value.GetType().Name;
        }

        public void Dispose()
        {
            if (this.value is IDisposable)
                ((IDisposable)value).Dispose();
        }
    }

    public class SubResult : AlgorithmResult
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private float value;
        public float Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public SubResult(string algorithmName) : base(algorithmName)
        {

        }
    }

    public class AlgorithmResult : IDisposable
    {
        protected string algorithmName = "";
        public string AlgorithmName
        {
            get { return algorithmName; }
        }

        protected bool good = true;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        protected List<AlgorithmResultValue> resultValueList = new List<AlgorithmResultValue>();
        public List<AlgorithmResultValue> ResultValueList
        {
            get { return resultValueList; }
            set { resultValueList = value; }
        }

        protected SizeF offsetFound;
        public SizeF OffsetFound
        {
            get { return offsetFound; }
            set { offsetFound = value; }
        }

        protected SizeF realOffsetFound;
        public SizeF RealOffsetFound
        {
            get { return realOffsetFound; }
            set { realOffsetFound = value; }
        }

        protected bool calibrated = false;
        public bool Calibrated
        {
            get { return calibrated; }
            set { calibrated = value; }
        }

        protected float angleFound;
        public float AngleFound
        {
            get { return angleFound; }
            set { angleFound = value; }
        }

        protected RotatedRect resultRect;
        public RotatedRect ResultRect
        {
            get { return resultRect; }
            set { resultRect = value; }
        }

        protected FigureGroup resultFigureGroup = new FigureGroup();
        public FigureGroup ResultFigureGroup
        {
            get { return resultFigureGroup; }
            set { resultFigureGroup = value; }
        }

        protected MessageBuilder messageBuilder = new MessageBuilder();
        public MessageBuilder MessageBuilder
        {
            get { return messageBuilder; }
            set { messageBuilder = value; }
        }

        protected string message = "";
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        protected string shortResultMessage = "";
        public string ShortResultMessage
        {
            get
            {
                if (string.IsNullOrEmpty(shortResultMessage))
                    return good ? "GOOD" : "NG";
                return shortResultMessage;
            }
            set { shortResultMessage = value; }
        }

        protected List<AlgorithmResult> subResultList = new List<AlgorithmResult>();
        public List<AlgorithmResult> SubResultList
        {
            get { return subResultList; }
            set { subResultList = value; }
        }

        protected TimeSpan spandTime = new TimeSpan(0);
        public TimeSpan SpandTime
        {
            get { return spandTime; }
            set { spandTime = value; }
        }

        public AlgorithmResult(string algorithmName)
        {
            this.algorithmName = algorithmName;
        }

        public void Offset(float x, float y)
        {
            resultFigureGroup.Offset(x, y);
            this.resultRect.Offset(x, y);
            subResultList.ForEach(f => f.Offset(x, y));
        }

        public void AddSubResult(AlgorithmResult subResult)
        {
            subResultList.Add(subResult);
        }

        public virtual void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            Figure figure = (Figure)resultFigureGroup.Clone();

            figure.Offset(offset.X, offset.Y);
            figureGroup.AddFigure(figure);

            foreach (SubResult subResult in subResultList)
            {
                if (subResult == null)
                    continue;
                Figure subFigure = (Figure)subResult.ResultFigureGroup.Clone();
                subFigure.Offset(offset.X, offset.Y);
                figureGroup.AddFigure(subFigure);

                /*if (String.IsNullOrEmpty(subResult.ShortMessage) == false)
                {
                    Font font = new Font("Arial", 5);
                    TextFigure textFigure = new TextFigure(subResult.ShortMessage, new Point((int)subFigure.GetRectangle().Right, (int)subFigure.GetRectangle().Top), font, Color.Red);
                    textFigure.Alignment = StringAlignment.Far;
                    figureGroup.AddFigure(textFigure);
                }*/
            }
        }

        public AlgorithmResultValue GetResultValue(string name)
        {
            return resultValueList.Find(x => x.Name == name);
        }

        public List<AlgorithmResultValue> GetResultValues(string name)
        {
            return resultValueList.FindAll(x => x.Name == name);
        }

        /*
                public virtual String GetResultString()
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (string resultMessage in resultMessageList)
                    {
                        stringBuilder.AppendLine(resultMessage);
                    }

                    foreach (AlgorithmResultValue resultValue in resultValueList)
                    {
                        if (MathHelper.IsNumeric(resultValue.Value) == true)
                            stringBuilder.AppendLine(String.Format("{0} : {1} ( {2} ~ {3} )", resultValue.Name, resultValue.Value, resultValue.Lcl, resultValue.Ucl));
                        else
                        {
                            if (resultValue.DesiredValue == null)
                                stringBuilder.AppendLine(String.Format("{0} : {1} ", resultValue.Name, resultValue.Value.ToString()));
                            else
                                stringBuilder.AppendLine(String.Format("{0} : {1} ({2}) ", resultValue.Name, resultValue.Value.ToString(), resultValue.DesiredValue.ToString()));
                        }
                    }

                    if (subResultList.Count > 0)
                    {
                        stringBuilder.AppendLine("< Sub Result >");

                        int index = 1;
                        foreach (SubResult subResult in subResultList)
                        {
                            stringBuilder.AppendLine(String.Format("[{0}] {1} - {2} : {3}", index++, subResult.Name, subResult.Value, subResult.Message));
                        }
                    }

                    return stringBuilder.ToString();
                }
        */

        public override string ToString()
        {
            return string.Format("{0} - {1}", algorithmName, ShortResultMessage);
        }

        public void Dispose()
        {
            resultValueList.ForEach(f => f.Dispose());
            resultValueList.Clear();
        }
    }
}
