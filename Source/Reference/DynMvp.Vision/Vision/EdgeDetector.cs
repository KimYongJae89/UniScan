using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision
{
    public class EdgeDetectorParam
    {
        private EdgeType edgeType;
        public EdgeType EdgeType
        {
            get { return edgeType; }
            set { edgeType = value; }
        }

        private int filterSize;
        public int FilterSize
        {
            get { return filterSize; }
            set { filterSize = value; }
        }

        private float edgeThreshold;
        public float EdgeThreshold
        {
            get { return edgeThreshold; }
            set { edgeThreshold = value; }
        }

        public EdgeDetectorParam()
        {
            edgeType = EdgeType.LightToDark;
            filterSize = 2;
            edgeThreshold = 5;
        }

        public virtual EdgeDetectorParam Clone()
        {
            EdgeDetectorParam param = new EdgeDetectorParam();

            param.Copy(this);

            return param;
        }

        public virtual void Copy(EdgeDetectorParam srcParam)
        {
            edgeType = srcParam.edgeType;
            filterSize = srcParam.filterSize;
            edgeThreshold = srcParam.edgeThreshold;
        }

        public virtual void LoadParam(XmlElement paramElement)
        {
            edgeType = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "LightToDark"));
            filterSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FilterSize", "2"));
            edgeThreshold = Convert.ToSingle(XmlHelper.GetValue(paramElement, "EdgeThreshold", "5"));
        }

        public virtual void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "EdgeType", edgeType.ToString());
            XmlHelper.SetValue(paramElement, "FilterSize", filterSize.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThreshold", edgeThreshold.ToString());
        }
    }

    public abstract class EdgeDetector
    {
        EdgeDetectorParam param;
        public EdgeDetectorParam Param
        {
            get { return param; }
            set { param = value; }
        }


        public static string TypeName
        {
            get { return "EdgeDetector"; }
        }

        public abstract EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext);
    }

    public class EdgeDetectionResult
    {
        PointF edgePosition = new PointF();
        public PointF EdgePosition
        {
            get { return edgePosition; }
            set { edgePosition = value; }
        }

        bool result = false;
        public bool Result
        {
            get { return result; }
            set { result = value; }
        }
    }
}
