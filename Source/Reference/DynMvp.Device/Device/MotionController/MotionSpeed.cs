using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;

namespace DynMvp.Devices.MotionController
{
    public class MovingParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(MovingParam))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is MovingParam)
            {

                MovingParam movingParam = (MovingParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(MovingParamConverter))]
    public class MovingParam
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        float startVelocity;
        public float StartVelocity
        {
            get { return startVelocity; }
            set { startVelocity = value; }
        }

        float accelerationTimeMs;
        public float AccelerationTimeMs
        {
            get { return accelerationTimeMs; }
            set { accelerationTimeMs = value; }
        }

        float decelerationTimeMs;
        public float DecelerationTimeMs
        {
            get { return decelerationTimeMs; }
            set { decelerationTimeMs = value; }
        }

        float maxVelocity;
        public float MaxVelocity
        {
            get { return maxVelocity; }
            set { maxVelocity = value; }
        }

        float sCurveTimeMs;
        public float SCurveTimeMs
        {
            get { return sCurveTimeMs; }
            set { sCurveTimeMs = value; }
        }

        public MovingParam()
        {
            name = "";
            startVelocity = 0.0f;
            accelerationTimeMs = 100;
            decelerationTimeMs = 100;
            maxVelocity = 1000.0f;
            sCurveTimeMs = 0;
        }

        public MovingParam(string name, float startVelocity, float accelerationTimeMs, float decelerationTimeMs, float maxVelocity, float sCurveTimeMs)
        {
            this.name = name;
            this.startVelocity = startVelocity;
            this.accelerationTimeMs = accelerationTimeMs;
            this.decelerationTimeMs = decelerationTimeMs;
            this.maxVelocity = maxVelocity;
            this.sCurveTimeMs = sCurveTimeMs;
        }

        public MovingParam(MovingParam srcMovingParam)
        {
            name = srcMovingParam.name;
            startVelocity = srcMovingParam.startVelocity;
            accelerationTimeMs = srcMovingParam.accelerationTimeMs;
            decelerationTimeMs = srcMovingParam.decelerationTimeMs;
            maxVelocity = srcMovingParam.maxVelocity;
            sCurveTimeMs = srcMovingParam.sCurveTimeMs;
        }

        public void Load(XmlElement movingParamElement)
        {
            if (movingParamElement == null)
                return;

            name = XmlHelper.GetValue(movingParamElement, "Name", name);
            startVelocity = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "StartVelocity", startVelocity.ToString()));
            accelerationTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "AccelerationTimeMs", accelerationTimeMs.ToString()));
            decelerationTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "DecelerationTimeMs", decelerationTimeMs.ToString()));
            maxVelocity = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "MaxVelocity", maxVelocity.ToString()));
            sCurveTimeMs = Convert.ToSingle(XmlHelper.GetValue(movingParamElement, "SCurveTimeMs", sCurveTimeMs.ToString()));
        }

        public MovingParam Clone()
        {
            return new MovingParam(this);
        }

        public void Save(XmlElement movingParamElement)
        {
            if (movingParamElement == null)
                return;

            XmlHelper.SetValue(movingParamElement, "Name", name);
            XmlHelper.SetValue(movingParamElement, "StartVelocity", startVelocity.ToString());
            XmlHelper.SetValue(movingParamElement, "AccelerationTimeMs", accelerationTimeMs.ToString());
            XmlHelper.SetValue(movingParamElement, "DecelerationTimeMs", decelerationTimeMs.ToString());
            XmlHelper.SetValue(movingParamElement, "MaxVelocity", maxVelocity.ToString());
            XmlHelper.SetValue(movingParamElement, "SCurveTimeMs", sCurveTimeMs.ToString());
        }
    }

    public class HomeParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(HomeParam))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is HomeParam)
            {

                HomeParam homeSpeed = (HomeParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public enum HomeMode
    {
        PosEndLimit, NegEndLimit, HomeSensor
    }

    public enum MoveDirection
    {
        CCW, CW
    }

    public enum HomeSignal
    {
        Low, High
    }

    [TypeConverterAttribute(typeof(HomeParamConverter))]
    public class HomeParam
    {
        int homeMethod;
        public int HomeMethod
        {
            get { return homeMethod; }
            set { homeMethod = value; }
        }

        HomeMode homeMode;
        public HomeMode HomeMode
        {
            get { return homeMode; }
            set { homeMode = value; }
        }

        MoveDirection homeDirection;
        public MoveDirection HomeDirection
        {
            get { return homeDirection; }
            set { homeDirection = value; }
        }

        HomeSignal homeSignal;
        public HomeSignal HomeSignal
        {
            get { return homeSignal; }
            set { homeSignal = value; }
        }

        MovingParam highSpeed = new MovingParam();
        public MovingParam HighSpeed
        {
            get { return highSpeed; }
            set { highSpeed = value; }
        }

        MovingParam mediumSpeed = new MovingParam();
        public MovingParam MediumSpeed
        {
            get { return mediumSpeed; }
            set { mediumSpeed = value; }
        }

        MovingParam fineSpeed = new MovingParam();
        public MovingParam FineSpeed
        {
            get { return fineSpeed; }
            set { fineSpeed = value; }
        }

        public void Load(XmlElement homeMovingSpeedElement)
        {
            if (homeMovingSpeedElement == null)
                return;

            homeMethod = Convert.ToInt32(XmlHelper.GetValue(homeMovingSpeedElement, "HomeMethod", "4"));
            homeMode = (HomeMode)Enum.Parse(typeof(HomeMode), XmlHelper.GetValue(homeMovingSpeedElement, "HomeMode", "NegEndLimit"));
            homeDirection = (MoveDirection)Enum.Parse(typeof(MoveDirection), XmlHelper.GetValue(homeMovingSpeedElement, "MoveDirection", "CW"));
            homeSignal = (HomeSignal)Enum.Parse(typeof(HomeSignal), XmlHelper.GetValue(homeMovingSpeedElement, "HomeSignal", "High"));

            XmlElement highSpeedElement = homeMovingSpeedElement["HighSpeed"];
            if (highSpeedElement != null)
                highSpeed.Load(highSpeedElement);

            XmlElement mediumSpeedElement = homeMovingSpeedElement["MediumSpeed"];
            if (mediumSpeedElement != null)
                mediumSpeed.Load(mediumSpeedElement);

            XmlElement fineSpeedElement = homeMovingSpeedElement["FineSpeed"];
            if (fineSpeedElement != null)
                fineSpeed.Load(fineSpeedElement);
        }

        public void Save(XmlElement homeMovingSpeedElement)
        {
            if (homeMovingSpeedElement == null)
                return;

            XmlHelper.SetValue(homeMovingSpeedElement, "HomeMethod", homeMethod.ToString());
            XmlHelper.SetValue(homeMovingSpeedElement, "HomeMode", homeMode.ToString());
            XmlHelper.SetValue(homeMovingSpeedElement, "MoveDirection", homeDirection.ToString());
            XmlHelper.SetValue(homeMovingSpeedElement, "HomeSignal", homeSignal.ToString());

            XmlElement highSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "HighSpeed", "");
            homeMovingSpeedElement.AppendChild(highSpeedElement);

            highSpeed.Save(highSpeedElement);

            XmlElement mediumSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "MediumSpeed", "");
            homeMovingSpeedElement.AppendChild(mediumSpeedElement);

            mediumSpeed.Save(mediumSpeedElement);

            XmlElement fineSpeedElement = homeMovingSpeedElement.OwnerDocument.CreateElement("", "FineSpeed", "");
            homeMovingSpeedElement.AppendChild(fineSpeedElement);

            fineSpeed.Save(fineSpeedElement);
        }
    }

    // 축별로 세부적인 속도 제어가 필요한 상황에서 사용할 클래스
    public class AxisSpeedConfig
    {
        List<MovingParam> movingParamList = new List<MovingParam>();
        public List<MovingParam> MovingParamList
        {
            get { return movingParamList; }
            set { movingParamList = value; }
        }

        public void AddMovingParam(MovingParam movingParam)
        {
            movingParamList.Add(movingParam);
        }

        public MovingParam GetMovingParam(string name)
        {
            return movingParamList.Find(delegate(MovingParam movingParam) { return movingParam.Name == name; });
        }

        public void Load(XmlElement axisSpeedConfigElement)
        {
            foreach (XmlElement movingParamElement in axisSpeedConfigElement)
            {
                if (movingParamElement.Name == "MovingParam")
                {
                    MovingParam movingParam = new MovingParam();
                    movingParam.Load(movingParamElement);

                    movingParamList.Add(movingParam);
                }
            }
        }

        public void Save(XmlElement axisSpeedConfigElement)
        {
            foreach (MovingParam movingParam in movingParamList)
            {
                XmlElement movingParamElement = axisSpeedConfigElement.OwnerDocument.CreateElement("", "MovingParam", "");
                axisSpeedConfigElement.AppendChild(movingParamElement);

                movingParam.Save(movingParamElement);
            }
        }
    }

    public class MotionSpeedConfig
    {
        string fileName = String.Empty;
        AxisSpeedConfig[] axisSpeedConfig = null;

        protected void Initialize(int numAxis)
        {
            axisSpeedConfig = new AxisSpeedConfig[numAxis];

            fileName = String.Format(@"{0}\..\Config\MotionSpeedConfig.xml", Environment.CurrentDirectory);
            if (File.Exists(fileName))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);

                int axisId = 0;

                XmlElement motionSpeedConfigElement = xmlDocument.DocumentElement;
                foreach (XmlElement axisSpeedConfigElement in motionSpeedConfigElement)
                {
                    if (axisSpeedConfigElement.Name == "AxisSpeedConfig")
                    {
                        axisSpeedConfig[axisId].Load(axisSpeedConfigElement);
                        axisId++;
                    }
                }
            }
        }

        public AxisSpeedConfig GetAxisSpeedConfig(int axisNo)
        {
            return axisSpeedConfig[axisNo];
        }

        public void SetAxisSpeedConfig(int axisNo, AxisSpeedConfig axisSpeedConfig)
        {
            this.axisSpeedConfig[axisNo] = axisSpeedConfig;
        }

        public void Save()
        {
            if (fileName != String.Empty)
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement motionSpeedConfigElement = xmlDocument.CreateElement("", "MotionSpeedConfig", "");
                xmlDocument.AppendChild(motionSpeedConfigElement);

                foreach (AxisSpeedConfig axisSpeedConfig in this.axisSpeedConfig)
                {
                    XmlElement axisSpeedConfigElement = xmlDocument.CreateElement("", "AxisSpeedConfig", "");
                    motionSpeedConfigElement.AppendChild(axisSpeedConfigElement);

                    axisSpeedConfig.Save(axisSpeedConfigElement);
                }
            }
        }
    }
}
