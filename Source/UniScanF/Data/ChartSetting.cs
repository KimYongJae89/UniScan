using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScan.Data
{
    public class ChartSetting
    {
        float maxValue;
        float minValue;
        float standardValue;
        float startPos;
        float endPos;
        float validStartPos;
        float validEndPos;
        float valueUpperError;
        float valueUpperWarning;
        float valueLowerWarning;
        float valueLowerError;
        float xPitch;
        float yPitch;

        public float MaxValue { get { return maxValue; } set { maxValue = value; } }
        public float MinValue { get { return minValue; }
            set { minValue = value; }
        }
        public float StandardValue { get { return standardValue; }
            set { standardValue = value; }
        }
        public float StartPos { get { return startPos; }
            set { startPos = value; }
        }
        public float EndPos { get { return endPos; }
            set { endPos = value; }
        }
        public float ValidStartPos { get { return validStartPos; }
            set { validStartPos = value; }
        }
        public float ValidEndPos { get { return validEndPos; }
            set { validEndPos = value; }
        }
        public float ValueUpperError { get { return valueUpperError; }
            set { valueUpperError = value; }
        }
        public float ValueUpperWarning { get { return valueUpperWarning; }
            set { valueUpperWarning = value; }
        }
        public float ValueLowerWarning { get { return valueLowerWarning; }
            set { valueLowerWarning = value; }
        }
        public float ValueLowerError { get { return valueLowerError; }
            set { valueLowerError = value; }
        }
        public float XPitch { get { return xPitch; }
            set { xPitch = value; }
        }
        public float YPitch { get { return yPitch; }
            set { yPitch = value; }
        }

        public ChartSetting()
        {
            maxValue = 0;
            minValue = 0;
            standardValue = 0;
            startPos = 0;
            endPos = 0;
            validStartPos = 0;
            validEndPos = 0;
            valueUpperError = 0;
            valueUpperWarning = 0;
            valueLowerWarning = 0;
            valueLowerError = 0;
            xPitch = 0;
            yPitch = 0;
        }

        public ChartSetting Clone()
        {
            ChartSetting chartSetting = new ChartSetting();

            chartSetting.maxValue = this.maxValue;
            chartSetting.minValue = this.minValue;
            chartSetting.standardValue = this.standardValue;
            chartSetting.startPos = this.startPos;
            chartSetting.endPos = this.endPos;
            chartSetting.validStartPos = this.validStartPos;
            chartSetting.validEndPos = this.validEndPos;
            chartSetting.valueUpperError = this.valueUpperError;
            chartSetting.valueUpperWarning = this.valueUpperWarning;
            chartSetting.valueLowerWarning = this.valueLowerWarning;
            chartSetting.valueLowerError = this.valueLowerError;
            chartSetting.xPitch = this.xPitch;
            chartSetting.yPitch = this.yPitch;

            return chartSetting;
        }

        public void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("MaxValue", maxValue.ToString());
            xmlElement.SetAttribute("MinValue", minValue.ToString());
            xmlElement.SetAttribute("StandardValue", standardValue.ToString());
            xmlElement.SetAttribute("StartPos", startPos.ToString());
            xmlElement.SetAttribute("EndPos", endPos.ToString());
            xmlElement.SetAttribute("ValidStartPos", validStartPos.ToString());
            xmlElement.SetAttribute("ValidEndPos", validEndPos.ToString());
            xmlElement.SetAttribute("ValueUpperError", valueUpperError.ToString());
            xmlElement.SetAttribute("ValueUpperWarning", valueUpperWarning.ToString());
            xmlElement.SetAttribute("ValueLowerWarning", valueLowerWarning.ToString());
            xmlElement.SetAttribute("ValueLowerError", valueLowerError.ToString());
            xmlElement.SetAttribute("XPitch", xPitch.ToString());
            xmlElement.SetAttribute("YPitch", yPitch.ToString());
        }

        public void Load(XmlElement xmlElement)
        {
            maxValue = Convert.ToSingle(xmlElement.GetAttribute("MaxValue"));
            minValue = Convert.ToSingle(xmlElement.GetAttribute("MinValue"));
            standardValue = Convert.ToSingle(xmlElement.GetAttribute("StandardValue"));
            startPos = Convert.ToSingle(xmlElement.GetAttribute("StartPos"));
            endPos = Convert.ToSingle(xmlElement.GetAttribute("EndPos"));
            validStartPos = Convert.ToSingle(xmlElement.GetAttribute("ValidStartPos"));
            validEndPos = Convert.ToSingle(xmlElement.GetAttribute("ValidEndPos"));
            valueUpperError = Convert.ToSingle(xmlElement.GetAttribute("ValueUpperError"));
            valueUpperWarning = Convert.ToSingle(xmlElement.GetAttribute("ValueUpperWarning"));
            valueLowerWarning = Convert.ToSingle(xmlElement.GetAttribute("ValueLowerWarning"));
            valueLowerError = Convert.ToSingle(xmlElement.GetAttribute("ValueLowerError"));
            XPitch = Convert.ToSingle(xmlElement.GetAttribute("XPitch"));
            YPitch = Convert.ToSingle(xmlElement.GetAttribute("YPitch"));
        }
    }
}
