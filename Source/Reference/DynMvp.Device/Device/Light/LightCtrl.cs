using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using System.Xml;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Comm;
using DynMvp.Device.Device.Light;

namespace DynMvp.Devices.Light
{
    public enum LightCtrlType
    {
        None, IO, Serial
    }

    public enum LightControllerVender
    {
        Unknown = -1, Iovis, Movis, AltSystem, LFine, Lvs, PSCC, VIT, UniSensor, iCore
    }


    public class InvalidLightSizeException : ApplicationException
    {

    }

    public abstract class LightCtrlInfo
    {
        string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected LightCtrlType controllerType;
        public LightCtrlType ControllerType => this.controllerType; 


        protected LightControllerVender controllerVender;
        public LightControllerVender ControllerVender => this.controllerVender;

        int numChannel;
        public int NumChannel
        {
            get { return numChannel; }
            set { numChannel = value; }
        }

        public LightCtrlInfo(LightControllerVender controllerVender)
        {
            this.controllerVender = controllerVender;
        }

        public virtual void SaveXml(XmlElement lightElement)
        {
            XmlHelper.SetValue(lightElement, "Name", name.ToString());
            XmlHelper.SetValue(lightElement, "LightCtrlType", this.controllerType);
            XmlHelper.SetValue(lightElement, "LightControllerVender", this.controllerVender);
            XmlHelper.SetValue(lightElement, "NumChannel", numChannel.ToString());
        }

        public virtual void LoadXml(XmlElement lightInfoElement)
        {
            this.name = XmlHelper.GetValue(lightInfoElement, "Name", "");
            this.controllerType = XmlHelper.GetValue(lightInfoElement, "LightCtrlType", LightCtrlType.IO);
            this.controllerVender = XmlHelper.GetValue(lightInfoElement, "LightControllerVender", LightControllerVender.Unknown);
            this.numChannel = Convert.ToInt32(XmlHelper.GetValue(lightInfoElement, "NumChannel", ""));
        }

        public abstract System.Windows.Forms.Form GetAdvancedConfigForm();

        public abstract LightCtrlInfo Clone();

        public virtual void CopyFrom(LightCtrlInfo srcInfo)
        {
            name = srcInfo.name;
            controllerType = srcInfo.controllerType;
            controllerVender = srcInfo.controllerVender;
            numChannel = srcInfo.numChannel;
        }
    }

    public class LightCtrlInfoList : List<LightCtrlInfo>
    {
        public LightCtrlInfoList Clone()
        {
            LightCtrlInfoList newLightCtrlInfoList = new LightCtrlInfoList();

            foreach (LightCtrlInfo lightCtrlInfo in this)
            {
                newLightCtrlInfoList.Add(lightCtrlInfo.Clone());
            }

            return newLightCtrlInfoList;
        }
    }

    public class LightCtrlInfoFactory
    {
        public static LightCtrlInfo Create(LightCtrlType lightCtrlType, LightControllerVender vender)
        {
            LightCtrlInfo lightCtrlInfo = null;
            try
            {
                switch (lightCtrlType)
                {
                    case LightCtrlType.IO:
                        lightCtrlInfo = IoLightCtrlInfo.Create(vender);
                        break;

                    case LightCtrlType.Serial:
                        lightCtrlInfo = SerialLightCtrlInfo.Create(vender);
                        break;
                }
                return lightCtrlInfo;
            }
            catch
            {
                return null;
            }
        }

        public static LightCtrlInfo Create(XmlElement xmlElement)
        {
            LightCtrlType lightCtrlType = XmlHelper.GetValue(xmlElement, "LightCtrlType", LightCtrlType.None);
            LightControllerVender lightCtrlVender = XmlHelper.GetValue(xmlElement, "LightControllerVender", LightControllerVender.Unknown);

            LightCtrlInfo lightCtrlInfo = LightCtrlInfoFactory.Create(lightCtrlType, lightCtrlVender);
            if (lightCtrlInfo != null)
                lightCtrlInfo.LoadXml(xmlElement);
            return lightCtrlInfo;
        }
    }

    public class LightCtrlFactory
    {
        public static LightCtrl Create(LightCtrlInfo lightCtrlInfo, DigitalIoHandler digitalIoHandler, bool isVirtualMode)
        {
            LightCtrl lightCtrl = null;

            if (isVirtualMode)
            {
                lightCtrl = new LightCtrlVirtual(lightCtrlInfo);
            }
            else
            {
                switch (lightCtrlInfo.ControllerType)
                {
                    case LightCtrlType.IO:
                        lightCtrl = new IoLightCtrl((IoLightCtrlInfo)lightCtrlInfo, digitalIoHandler);
                        break;

                    case LightCtrlType.Serial:
                        SerialLightCtrlInfo serialLightCtrlInfo = lightCtrlInfo as SerialLightCtrlInfo;
                        switch (serialLightCtrlInfo.ControllerVender)
                        {
                            case LightControllerVender.Iovis:
                            case LightControllerVender.Movis:
                            case LightControllerVender.AltSystem:
                            case LightControllerVender.LFine:
                            case LightControllerVender.Lvs:
                            case LightControllerVender.PSCC:
                            case LightControllerVender.VIT:
                                lightCtrl = new SerialLightCtrl(serialLightCtrlInfo);
                                break;

                            case LightControllerVender.UniSensor:
                                lightCtrl = new SerialLightUniSensor(serialLightCtrlInfo);
                                break;

                            case LightControllerVender.iCore:
                                lightCtrl = new SerialLightIPulse(serialLightCtrlInfo);
                                break;

                            case LightControllerVender.Unknown:
                            default:
                                break;
                        }
                        break;
                }
            }

            if (lightCtrl == null)
            {
                throw new AlarmException(ErrorCodeLight.Instance.InvalidType, ErrorLevel.Fatal,
                    lightCtrlInfo.Name, "Invalid Light Type", null, "");
                //ErrorManager.Instance().Report(ErrorCodeLight.Instance.InvalidType, ErrorLevel.Fatal,
                //    lightCtrlInfo.Name, "Can't create light controller.");
                return null;
            }

            if (lightCtrl.Initialize() == false)
            {
                throw new AlarmException(ErrorCodeLight.Instance.InvalidType, ErrorLevel.Fatal,
                    lightCtrlInfo.Name, "Fail to Initialize", null, "");
                //ErrorManager.Instance().Report(ErrorCodeLight.Instance.FailToInitialize, ErrorLevel.Fatal,
                //    lightCtrlInfo.Name, "Can't initialize light controller.");

                lightCtrl = new LightCtrlVirtual(lightCtrlInfo);
                lightCtrl.UpdateState(DeviceState.Error, "Light controller is invalid.");
            }
            else
            {
                lightCtrl.UpdateState(DeviceState.Ready, "Light controller initialization succeeded.");
            }

            DeviceManager.Instance().AddDevice(lightCtrl);

            return lightCtrl;
        }
    }

    public delegate void OnLigthValueChangedDelegate();
    public abstract class LightCtrl : Device
    {
        //LightCtrlType lightCtrlType;
        //public LightCtrlType LightCtrlType => lightCtrlInfo.ControllerType;

        //protected LightControllerVender lightControllerVender;
        //public LightControllerVender LightControllerVender => lightCtrlInfo.ControllerVender;

        protected LightCtrlInfo lightCtrlInfo;
        public int NumChannel => this.lightCtrlInfo.NumChannel;


        protected int lightStableTimeMs;
        public int LightStableTimeMs
        {
            get { return lightStableTimeMs; }
            set { lightStableTimeMs = value; }
        }

        int startChannelIndex;
        public int StartChannelIndex
        {
            get { return startChannelIndex; }
            set { startChannelIndex = value; }
        }

        public int EndChannelIndex
        {
            get { return startChannelIndex + NumChannel; }
        }

        //public LightValue CurLightValue => this.curLightValue;
        protected LightValue curLightValue;

        protected LightValue lastLightValue;
        public LightValue LastLightValue
        {
            get
            {
                if (lastLightValue == null)
                    lastLightValue = new LightValue(NumChannel);
                return lastLightValue;
            }
        }

        public LightCtrl(LightCtrlInfo lightCtrlInfo)
        {
            this.name = lightCtrlInfo.Name;
            if (string.IsNullOrEmpty(this.name))
                this.name = lightCtrlInfo.ControllerType.ToString();

            DeviceType = DeviceType.LightController;
            this.lightCtrlInfo = lightCtrlInfo;

            UpdateState(DeviceState.Idle);
        }
        
        public event OnLigthValueChangedDelegate OnLigthValueChanged;

        public abstract int GetMaxLightLevel();
        public abstract bool Initialize();
        public void TurnOn()
        {
            if (lastLightValue == null)
            {
                lastLightValue = new LightValue(NumChannel);
                for (int i = 0; i < NumChannel; i++)
                    lastLightValue.Value[i] = GetMaxLightLevel();
            }
            TurnOn(lastLightValue);
        }

        public void TurnOn(LightValue lightValue)
        {
            int maxVal = GetMaxLightLevel();
            for (int i = 0; i < lightValue.NumLight; i++)
            {
                lightValue.Value[i] = Math.Min(lightValue.Value[i], maxVal);
                lightValue.Value[i] = Math.Max(lightValue.Value[i], 0);
            }

            bool good;
            LogHelper.Debug(LoggerType.Grab, String.Format("Set light value: {0}", lightValue.KeyValue));

            try
            {
                good = SetLightValue(lightValue);
            }
            catch
            {
                good = false;
            }

            if (good)
            {
                this.curLightValue = lightValue.Clone();
                if (this.lastLightValue == null || lightValue.Value.All(f => f == 0) == false)
                    this.lastLightValue = lightValue.Clone();

                this.OnLigthValueChanged?.Invoke();
            }
        }

        protected abstract bool SetLightValue(LightValue lightValue);
        public abstract LightValue GetLightValue();

        public void TurnOff()
        {
            //LogHelper.Debug(LoggerType.Grab, "Turn off light");

            TurnOn(new LightValue(this.NumChannel));
        }
    }
}
