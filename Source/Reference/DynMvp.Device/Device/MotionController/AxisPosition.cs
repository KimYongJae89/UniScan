using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.MotionController
{
    public class AxisPosition
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        float[] position = new float[3];
        public float[] Position
        {
            get { return position; }
        }

        public AxisPosition()
        {
        }

        public AxisPosition Clone()
        {
            AxisPosition position = new AxisPosition(GetPosition());
            position.Name = name;

            return position;
        }

        public AxisPosition(int numAxis)
        {
            position = new float[numAxis];
        }

        public AxisPosition(params float[] positions)
        {
            SetPosition(positions);
        }

        public int NumAxis
        {
            get { return position.Count(); }
        }

        public float this[int key]
        {
            get { return position[key]; }
            set { position[key] = value; }
        }

        public bool IsEmpty
        {
            get { return position == null; }
        }

        public PointF ToPointF()
        {
            return new PointF(position[0], position[1]);
        }

        public void SetPosition(params float[] positions)
        {
            List<float> posList = new List<float>();

            foreach (float pos in positions)
                posList.Add(pos);

            position = posList.ToArray();
        }

        public float[] GetPosition()
        {
            return position;
        }

        public void ResetPosition()
        {
            for (int i = 0; i < position.Count(); i++)
            {
                position[i] = 0;
            }
        }

        public void GetValue(XmlElement xmlElement, string keyName)
        {
            String posStr = XmlHelper.GetValue(xmlElement, keyName, "");
            if (posStr != "")
            {
                if (posStr.IndexOf(':') > -1)
                {
                    String[] strArray = posStr.Split(':');
                    name = strArray[0].Trim();
                    posStr = strArray[1];
                }

                String[] posStrArray = posStr.Split(',');
                position = new float[posStrArray.Count()];

                for (int i = 0; i < posStrArray.Count(); i++)
                {
                    if (posStrArray.Length - 1 >= i)
                        position[i] = Convert.ToSingle(posStrArray[i]);
                    else
                        position[i] = 0;
                }
            }
        }

        public void SetValue(XmlElement xmlElement, string keyName)
        {
            XmlHelper.SetValue(xmlElement, keyName, ToString());
        }

        public static AxisPosition ToAxisPosition(params float[] positions)
        {
            AxisPosition axisPosition = new AxisPosition();
            axisPosition.SetPosition(positions);

            return axisPosition;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(name) == false)
                sb.AppendFormat("{0} : ", name);

            Array.ForEach(position, f => sb.AppendFormat("{0},", f));           

            return sb.ToString().Trim(',');
        }

        public static AxisPosition GetMidPos(params AxisPosition[] axisPosition)
        {
            if (axisPosition == null)
                return null;

            if(axisPosition.Count()==0)
                throw new ArgumentException("AxisNum is missmatch");

            int maxAxisNum = axisPosition.Max(f => f.NumAxis);
            int minAxisNum = axisPosition.Min(f => f.NumAxis);
            if (maxAxisNum != minAxisNum)
                throw new ArgumentException("AxisNum is missmatch");

            int axisNum = axisPosition[0].position.Count();
            float[] midPosValue = new float[axisNum];
            foreach (AxisPosition axis in axisPosition)
            {
                for (int i = 0; i < axisNum; i++)
                {
                    midPosValue[i] += axis.Position[i];
                }
            }

            for (int i = 0; i < axisNum; i++)
            {
                midPosValue[i] /= axisPosition.Count();
            }

            return new AxisPosition(midPosValue);
        }

        public void Add(params float[] value)
        {
            if (value.Count() != this.NumAxis)
                throw new ArgumentException("AxisPosition::Add (count missmatch)");

            for (int i = 0; i < this.NumAxis; i++)
                this.position[i] += value[i];
        }

        public void Add(float value)
        {
            for (int i = 0; i < this.NumAxis; i++)
                this.position[i] += value;
        }
    }
}
