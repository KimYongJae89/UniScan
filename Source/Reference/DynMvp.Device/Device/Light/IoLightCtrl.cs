using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System.Xml;
using System.Windows.Forms;

namespace DynMvp.Devices.Light
{
    public class IoLightCtrlInfo : LightCtrlInfo
    {
        List<IoPort> lightCtrlIoPortList = new List<IoPort>();
        public List<IoPort> LightCtrlIoPortList
        {
            get { return lightCtrlIoPortList; }
            set { lightCtrlIoPortList = value; }
        }

        public static LightCtrlInfo Create(LightControllerVender controllerVender)
        {
            return new IoLightCtrlInfo(controllerVender);
        }

        protected IoLightCtrlInfo(LightControllerVender controllerVender) : base(controllerVender)
        {
            this.controllerType = LightCtrlType.IO;
        }

        public override Form GetAdvancedConfigForm()
        {
            return null;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            XmlElement ioLightCtrlElement = lightInfoElement.OwnerDocument.CreateElement("", "IoLightController", "");
            lightInfoElement.AppendChild(ioLightCtrlElement);

            foreach (IoPort ioLightPort in LightCtrlIoPortList)
            {
                XmlElement ioLightPortElement = lightInfoElement.OwnerDocument.CreateElement("", "IoLightPort", "");
                ioLightCtrlElement.AppendChild(ioLightPortElement);

                ioLightPort.SaveXml(ioLightPortElement);
            }
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            XmlElement ioLightCtrlElement = lightInfoElement["IoLightController"];
            if (ioLightCtrlElement != null)
            {
                int lightIndex = 0;
                foreach (XmlElement ioLightPortElement in ioLightCtrlElement)
                {
                    if (ioLightPortElement.Name == "IoLightPort")
                    {
                        IoPort ioLightPort = new IoPort(String.Format("Light{0}", lightIndex));
                        ioLightPort.LoadXml(ioLightPortElement);
                    }
                }
            }
        }

        public override LightCtrlInfo Clone()
        {
            IoLightCtrlInfo IoLightCtrlInfo = new IoLightCtrlInfo(this.controllerVender);
            IoLightCtrlInfo.CopyFrom(this);

            return IoLightCtrlInfo;
        }
    }

    public class IoLightCtrl : LightCtrl
    {
        private DigitalIoHandler digitalIoHandler;
        private IoPort[] lightPorts;

        public IoLightCtrl(IoLightCtrlInfo ioLightCtrlInfo, DigitalIoHandler digitalIoHandler)
            : base(ioLightCtrlInfo)
        {
            this.digitalIoHandler = digitalIoHandler;
        }

        public override bool Initialize()
        {
            IoLightCtrlInfo ioLightCtrlInfo = (IoLightCtrlInfo)lightCtrlInfo;

            lightPorts = ioLightCtrlInfo.LightCtrlIoPortList.ToArray();

            return true;
        }

        public override int GetMaxLightLevel()
        {
            return 1;
        }

        //public override void TurnOn()
        //{
        //    LogHelper.Debug(LoggerType.Grab, "Turn on light");
        //    TurnOn(new LightValue(lightPorts.Count(), 256));
        //}

        //public override void TurnOff()
        //{
        //    LogHelper.Debug(LoggerType.Grab, "Turn off light");
        //    TurnOn(new LightValue(lightPorts.Count(), 0));
        //}

        public override void Release()
        {
            base.Release();
        }

        public int GetNumLight()
        {
            return lightPorts.Count();
        }

        protected override bool SetLightValue(LightValue lightValue)
        {
            DioValue outputValue = digitalIoHandler.ReadOutput(true);
            for (int i = 0; i < outputValue.Count; i++)
            {
                outputValue.UpdateBitFlag(lightPorts[i], (lightValue.Value[StartChannelIndex + i] > 0));
            }

            digitalIoHandler.WriteOutput(outputValue, true);

            Thread.Sleep(lightStableTimeMs);
            return true;
        }

        public override LightValue GetLightValue()
        {
            DioValue outputValue = digitalIoHandler.ReadOutput(true);
            int maxLigthLevel = GetMaxLightLevel();

            LightValue curLightValue = new LightValue(this.NumChannel);
            for(int i=0; i< this.NumChannel; i++)
                curLightValue.Value[i] = outputValue.GetValue(lightPorts[i]) ? maxLigthLevel : 0;
            return curLightValue;
        }
    }
}
