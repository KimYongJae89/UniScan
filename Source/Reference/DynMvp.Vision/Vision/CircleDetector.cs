using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class CircleDetectorParam
    {
        private PointF centerPosition;
        public PointF CenterPosition
        {
            get { return centerPosition; }
            set { centerPosition = value; }
        }

        private float innerRadius;
        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        private float outterRadius;
        public float OutterRadius
        {
            get { return outterRadius; }
            set { outterRadius = value; }
        }

        private EdgeType edgeType = EdgeType.Any;
        public EdgeType EdgeType
        {
            get { return edgeType; }
            set { edgeType = value; }
        }

        private float maxAssociationDistance;
        public float MaxAssociationDistance
        {
            get { return maxAssociationDistance; }
            set { maxAssociationDistance = value; }
        }

        private float minEdgeValue;
        public float MinEdgeValue
        {
            get { return minEdgeValue; }
            set { minEdgeValue = value; }
        }

        private bool smallest;
        public bool Smallest
        {
            get { return smallest; }
            set { smallest = value; }
        }

        public CircleDetectorParam()
        {
            centerPosition.X = 0;
            centerPosition.Y = 0;
            innerRadius = 0;
            outterRadius = 0;
            edgeType = EdgeType.Any;
        }

        public CircleDetectorParam Clone()
        {
            CircleDetectorParam param = new CircleDetectorParam();
            param.Copy(this);

            return param;
        }

        public void Copy(CircleDetectorParam srcParam)
        {
            centerPosition = srcParam.centerPosition;
            innerRadius = srcParam.innerRadius;
            outterRadius = srcParam.outterRadius;
            edgeType = srcParam.edgeType;
        }

        public void LoadParam(XmlElement paramElement)
        {
            centerPosition.X = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterPosition.X", "0"));
            centerPosition.Y = Convert.ToSingle(XmlHelper.GetValue(paramElement, "CenterPosition.Y", "0"));
            innerRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "InnerRadius", "0"));
            outterRadius = Convert.ToSingle(XmlHelper.GetValue(paramElement, "OutterRadius", "0"));
            edgeType = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "Any"));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "CenterPosition.X", centerPosition.X.ToString());
            XmlHelper.SetValue(paramElement, "CenterPosition.Y", centerPosition.Y.ToString());
            XmlHelper.SetValue(paramElement, "InnerRadius", innerRadius.ToString());
            XmlHelper.SetValue(paramElement, "OutterRadius", outterRadius.ToString());
            XmlHelper.SetValue(paramElement, "EdgeType", edgeType.ToString());
        }
    }

    public abstract class CircleDetector
    {
        CircleDetectorParam param;
        public CircleDetectorParam Param
        {
            get { return param; }
            set { param = value; }
        }

        public static string TypeName
        {
            get { return "CircleDetector"; }
        }

        public abstract CircleEq Detect(AlgoImage algoImage, DebugContext debugContext);
    }
}