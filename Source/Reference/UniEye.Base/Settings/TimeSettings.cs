using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.Settings
{
    public class TimeSettings
    {
        int lightStableTimeMs;
        public int LightStableTimeMs
        {
            get { return lightStableTimeMs; }
            set { lightStableTimeMs = value; }
        }

        int triggerDelayMs;
        public int TriggerDelayMs
        {
            get { return triggerDelayMs; }
            set { triggerDelayMs = value; }
        }

        int stepDelayS;
        public int StepDelayS
        {
            get { return stepDelayS; }
            set { stepDelayS = value; }
        }

        int inspectionDelay;
        public int InspectionDelay
        {
            get { return inspectionDelay; }
            set { inspectionDelay = value; }
        }

        int towerLampUpdataIntervalMs=100;
        public int TowerLampUpdataIntervalMs
        {
            get { return towerLampUpdataIntervalMs; }
            set { towerLampUpdataIntervalMs = value; }
        }

        int ngSignalHoldTime = 10;
        public int NgSignalHoldTime
        {
            get { return ngSignalHoldTime; }
            set { ngSignalHoldTime = value; }
        }

        int liveViewExposureTime;
        public int LiveViewExposureTime
        {
            get { return liveViewExposureTime; }
            set { liveViewExposureTime = value; }
        }

        int rejectPusherPushTimeMs;
        public int RejectPusherPushTimeMs
        {
            get { return rejectPusherPushTimeMs; }
            set { rejectPusherPushTimeMs = value; }
        }

        int rejectPusherPullTimeMs;
        public int RejectPusherPullTimeMs
        {
            get { return rejectPusherPullTimeMs; }
            set { rejectPusherPullTimeMs = value; }
        }

        int rejectWaitTimeMs;
        public int RejectWaitTimeMs
        {
            get { return rejectWaitTimeMs; }
            set { rejectWaitTimeMs = value; }
        }

        int airActionStableTimeMs;
        public int AirActionStableTimeMs
        {
            get { return airActionStableTimeMs; }
            set { airActionStableTimeMs = value; }
        }

        static TimeSettings _instance;
        public static TimeSettings Instance()
        {
            if (_instance == null)
                _instance = new TimeSettings();

            return _instance;
        }

        private TimeSettings()
        {
            liveViewExposureTime = 50;
            lightStableTimeMs = 50;
            stepDelayS = 0;
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\Time.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement timeElement = xmlDocument["Time"];
            if (timeElement == null)
                return;

            lightStableTimeMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "LightStableTime", "50"));
            stepDelayS = Convert.ToInt32(XmlHelper.GetValue(timeElement, "StepDelay", "0"));
            triggerDelayMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "TriggerDelay", "0"));
            inspectionDelay = Convert.ToInt32(XmlHelper.GetValue(timeElement, "InspectionDelay", "0"));
            towerLampUpdataIntervalMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "TowerLampUpdataInterval", "0"));
            ngSignalHoldTime = Convert.ToInt32(XmlHelper.GetValue(timeElement, "NgSignalHoldTime", "0"));
            airActionStableTimeMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "AirActionStableTimeMs", "0"));
            liveViewExposureTime = Convert.ToInt32(XmlHelper.GetValue(timeElement, "LiveViewExposureTime", "50"));
            rejectPusherPullTimeMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "RejectPusherPullTimeMs", "200"));
            rejectPusherPushTimeMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "RejectPusherPushTimeMs", "500"));
            rejectWaitTimeMs = Convert.ToInt32(XmlHelper.GetValue(timeElement, "RejectWaitTimeMs", "500"));

            if (towerLampUpdataIntervalMs < 100)
                towerLampUpdataIntervalMs = 100;
        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\Time.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement timeElement = xmlDocument.CreateElement("", "Time", "");
            xmlDocument.AppendChild(timeElement);

            XmlHelper.SetValue(timeElement, "LightStableTime", lightStableTimeMs.ToString());
            XmlHelper.SetValue(timeElement, "StepDelay", stepDelayS.ToString());
            XmlHelper.SetValue(timeElement, "TriggerDelay", triggerDelayMs.ToString());
            XmlHelper.SetValue(timeElement, "InspectionDelay", inspectionDelay.ToString());
            XmlHelper.SetValue(timeElement, "TowerLampUpdataInterval", inspectionDelay.ToString());
            XmlHelper.SetValue(timeElement, "NgSignalHoldTime", ngSignalHoldTime.ToString());
            XmlHelper.SetValue(timeElement, "LiveViewExposureTime", liveViewExposureTime.ToString());
            XmlHelper.SetValue(timeElement, "RejectPusherPushTimeMs", rejectPusherPushTimeMs.ToString());
            XmlHelper.SetValue(timeElement, "RejectPusherPullTimeMs", rejectPusherPullTimeMs.ToString());
            XmlHelper.SetValue(timeElement, "RejectWaitTimeMs", rejectWaitTimeMs.ToString());
            XmlHelper.SetValue(timeElement, "AirActionStableTimeMs", airActionStableTimeMs.ToString());

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
