using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using System.Drawing;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.InspData;

namespace DynMvp.Data
{
    public enum ComputeType
    {
        Add, Subtract, Multiply, Divide, Equal, NotEqual, Greater, GreaterEqual, Less, LessEq, Not, And, Or, Distance
    }

    public class ComputeItem
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        ComputeType type;
        public ComputeType Type
        {
            get { return type; }
            set { type = value; }
        }

        string operand1;
        public string Operand1
        {
            get { return operand1; }
            set { operand1 = value; }
        }

        string operand2;
        public string Operand2
        {
            get { return operand2; }
            set { operand2 = value; }
        }

        public bool Compute(ComputeResultList computeResultList, out object value)
        {
            object value1;
            object value2;

            value = 0;

            bool result = false;
            if (computeResultList.GetValue(operand1, out value1) == true &&
                    computeResultList.GetValue(operand2, out value2) == true)
            {
                if (MathHelper.IsBoolean(value1) && MathHelper.IsBoolean(value2))
                {
                    result = ComputeBoolean(value1, value2, out value);
                }
                else if (MathHelper.IsNumeric(value1) && MathHelper.IsNumeric(value2))
                {
                    result = ComputeNumeric(value1, value2, out value);
                }
                else if (value1 is string && value2 is string)
                {
                    result = ComputeString(value1, value2, out value);
                }
                else if(value1 is PointF  && value2 is PointF)
                {
                    result = ComputeDistance(value1, value2, out value);
                }
            }

            return result;
        }

        private bool ComputeString(object value1, object value2, out object value)
        {
            float fValue1 = Convert.ToSingle(value1);
            float fValue2 = Convert.ToSingle(value2);

            value = 0;

            if (type == ComputeType.Add)
            {
                string resultString = value1.ToString() + value2.ToString();
                value = resultString;

                return true;
            }
            else if (type == ComputeType.Equal)
            {
                bool bResult = (value1.ToString() == value2.ToString());
                value = bResult;

                return true;
            }

            return false;
        }

        private bool ComputeDistance(object value1, object value2, out object value)
        {
            PointF fValue1 = (PointF)value1;
            PointF fValue2 = (PointF)value2;

            bool bResult = true;

            value = 0;
            if (type == ComputeType.Distance)
            {
                value = CalculateDistance(fValue1, fValue2);
            }

            return bResult;
        }
        private double CalculateDistance(PointF point1, PointF point2)
        { 
            double resultX = Math.Pow((double)point2.X - (double)point1.X, 2);
            double resultY = Math.Pow((double)point2.Y - (double)point1.Y, 2);
            double length = Math.Sqrt(resultX + resultY);
          
            return length;
        }

        private bool ComputeBoolean(object value1, object value2, out object value)
        {
            bool bValue1 = Convert.ToBoolean(value1);
            bool bValue2 = Convert.ToBoolean(value2);

            value = 0;

            bool bResult = true;
            if (type == ComputeType.And)
            {
                value = bValue1 && bValue2;
            }
            else if (type == ComputeType.Or)
            {
                value = bValue1 || bValue2;
            }
            else if (type == ComputeType.Not)
            {
                value = !bValue1;
            }
            else if (type == ComputeType.Equal)
            {
                value = (bValue1 == bValue2);
            }
            else if (type == ComputeType.NotEqual)
            {
                value = (bValue1 != bValue2);
            }
            else
            {
                bResult = false;
            }

            return bResult;
        }

        private bool ComputeNumeric(object value1, object value2, out object value)
        {
            float fValue1 = Convert.ToSingle(value1);
            float fValue2 = Convert.ToSingle(value2);

            value = 0;

            bool bResult = true;
            switch (type)
            {
                case ComputeType.Add:
                    value = (fValue1 + fValue2);
                    break;
                case ComputeType.Subtract:
                    value = (fValue1 - fValue2);
                    break;
                case ComputeType.Multiply:
                    value = (fValue1 * fValue2);
                    break;
                case ComputeType.Divide:
                    if (fValue2 == 0)
                    {
                        LogHelper.Error(LoggerType.Error, "The operand2 must have none zero value when computeType is Divide.");
                        bResult = false;
                    }
                    else
                    {
                        value = fValue1 / fValue2;
                    }
                    break;
                case ComputeType.Equal:
                    value = (fValue1 == fValue2);
                    break;
                case ComputeType.NotEqual:
                    value = (fValue1 != fValue2);
                    break;
                case ComputeType.Greater:
                    value = (fValue1 > fValue2);
                    break;
                case ComputeType.GreaterEqual:
                    value = (fValue1 >= fValue2);
                    break;
                case ComputeType.Less:
                    value = (fValue1 < fValue2);
                    break;
                case ComputeType.LessEq:
                    value = (fValue1 <= fValue2);
                    break;
                default:
                    bResult = false;
                    break;
            }

            return bResult;
        }
    }

    public class ComputeResult
    {
        string name;
        public string Name
        {
            get { return name; }
        }

        object value;
        public object Value
        {
            get { return this.value; }
        }

        public ComputeResult(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public class ComputeResultList
    {
        List<ComputeResult> computeResultList = new List<ComputeResult>();

        public void Clear()
        {
            computeResultList.Clear();
        }

        public void Add(ComputeResult computeResult)
        {
            computeResultList.Add(computeResult);
        }

        public bool GetValue(string name, out object value)
        {
            value = 0;
            ComputeResult computeResult = computeResultList.Find(x => { return x.Name == name; } );
            if (computeResult != null)
            {
                value = computeResult.Value;
            }

            return computeResult != null;
        }
    }

    public class ComputeProbe : Probe
    {
        List<ComputeItem> computeItemList = new List<ComputeItem>();
        public List<ComputeItem> ComputeItemList
        {
            get { return computeItemList; }
            set { computeItemList = value; }
        }

        string resultComputeItemName;
        public string ResultComputeItemName
        {
            get { return resultComputeItemName; }
            set { resultComputeItemName = value; }
        }

        public override Object Clone()
        {
            ComputeProbe computeProbe = new ComputeProbe();
            computeProbe.Copy(this);

            return computeProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            ComputeProbe computeProbe = (ComputeProbe)probe;

        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return false;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<ProbeResultValue> resultValues = new List<ProbeResultValue>();
            resultValues.Add(new ProbeResultValue("Result"));

            return resultValues;
        }

        private ComputeItem GetComputeItem(string name)
        {
            foreach (ComputeItem computeItem in computeItemList)
            {
                if (computeItem.Name == name)
                    return computeItem;
            }

            return null;
        }

        public List<string> GetOperandList()
        {
            List<string> operandList = new List<string>();
            foreach (ComputeItem computeItem in computeItemList)
            {
                operandList.Add(computeItem.Operand1);
                operandList.Add(computeItem.Operand2);
            }

            return operandList;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            ComputeResultList computeResultList = new ComputeResultList();
            computeResultList.Clear();

            List<string> operandList = GetOperandList();

            object value = null;
            InspectionResult inspectionResult = inspParam.InspectionResult;
            foreach (string operandName in operandList)
            {
                bool bResult = inspectionResult.GetResult(operandName, out value);
                if (bResult == true)
                    computeResultList.Add(new ComputeResult(operandName, value));
            }

            bool result;
            foreach(ComputeItem computeItem in computeItemList)
            {
                result = computeItem.Compute(computeResultList, out value);
                if (result == true && value != null)
                {
                    computeResultList.Add(new ComputeResult(computeItem.Name, value));
                }
            }

            result = computeResultList.GetValue(resultComputeItemName, out value);

            ComputeProbeResult computeProbeResult = new ComputeProbeResult(this, resultComputeItemName, (result == true ? value : 0));
            if (result == true)
            {
                if (MathHelper.IsBoolean(value) == true)
                    computeProbeResult.ResultValueList.Add(new ProbeResultValue("Result", Convert.ToBoolean(value)));
                else if (value is string)
                    computeProbeResult.ResultValueList.Add(new ProbeResultValue("Result", value.ToString()));
                else
                    computeProbeResult.ResultValueList.Add(new ProbeResultValue("Result", 0, 0, Convert.ToSingle(value)));
            }

            return computeProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new ComputeProbeResult(this, "Error", false);
        }
    }

    public class ComputeProbeResult : ProbeResult
    {
        public ComputeProbeResult(Probe probe, string resultName, object value)
        {
            this.Judgment = Judgment.Accept;
            if (value is Boolean && Convert.ToBoolean(value) == false)
                this.Judgment = Judgment.Reject;
            this.Probe = probe;
        }

        public override void BuildResultMessage(DynMvp.UI.MessageBuilder totalResultMessage)
        {

        }

        public override void AppendResultFigures(FigureGroup figureGroup, bool useTargetCoord)
        {

        }

        public override string ToString()
        {
            return String.Format("result value = {0}", ResultValueList[0].Value);
        }
    }
}
