using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniEye.Base.Device
{
    public enum TowerLampStateType
    {
        Unknown = -1, Idle, Wait, Working, Defect, Alarm
    }

    public class Lamp
    {
        bool value;
        public bool Value
        {
            get { return value; }
            set { this.value = value; }
        }

        bool blink;
        public bool Blink
        {
            get { return blink; }
            set { blink = value; }
        }

        public Lamp(bool value, bool blink = false)
        {
            this.value = value;
            this.blink = blink;
        }
    }

    public class TowerLampState
    {
        TowerLampStateType type;
        public TowerLampStateType Type
        {
            get { return type; }
        }

        Lamp redLamp;
        public Lamp RedLamp
        {
            get { return redLamp;  }
        }

        Lamp yellowLamp;
        public Lamp YellowLamp
        {
            get { return yellowLamp; }
        }

        Lamp greenLamp;
        public Lamp GreenLamp
        {
            get { return greenLamp; }
        }

        Lamp buzzer;
        public Lamp Buzzer
        {
            get { return buzzer; }
        }

        public TowerLampState()
        {
            this.type = TowerLampStateType.Unknown;
            this.redLamp = new Lamp(false);
            this.yellowLamp = new Lamp(false);
            this.greenLamp = new Lamp(false);
            this.buzzer = new Lamp(false);
        }

        public TowerLampState(TowerLampStateType type, Lamp redLamp, Lamp yellowLamp, Lamp greenLamp, Lamp buzzer)
        {
            this.type = type;
            this.redLamp = redLamp;
            this.yellowLamp = yellowLamp;
            this.greenLamp = greenLamp;
            this.buzzer = buzzer;
        }

        private  void updateTimer_Tick(object state)
        {
            UpdateValue();
        }

        public override string ToString()
        {
            return type.ToString();
        }

        public void ResetValue()
        {
            redLamp.Value = redLamp.Value || redLamp.Blink;
            yellowLamp.Value = yellowLamp.Value || yellowLamp.Blink;
            greenLamp.Value = greenLamp.Value || greenLamp.Blink;
            buzzer.Value = buzzer.Value || buzzer.Blink;
        }

        public void UpdateValue()
        {
            if (redLamp.Blink)
                redLamp.Value = !redLamp.Value;
            if (yellowLamp.Blink)
                yellowLamp.Value = !yellowLamp.Value;
            if (greenLamp.Blink)
                greenLamp.Value = !greenLamp.Value;
            if (buzzer.Blink)
                buzzer.Value = !buzzer.Value;
        }

        public void LoadXml(XmlElement xmlElement)
        {
            type = (TowerLampStateType)Enum.Parse(typeof(TowerLampStateType), XmlHelper.GetValue(xmlElement, "Type", "Idle"));
            redLamp.Value = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "RedLampValue", "False"));
            redLamp.Blink = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "RedLampBlink", "False"));
            yellowLamp.Value = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "YellowLampValue", "False"));
            yellowLamp.Blink = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "YellowLampBlink", "False"));
            greenLamp.Value = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "GreenLampValue", "False"));
            greenLamp.Blink = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "GreenLampBlink", "False"));
            buzzer.Value = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "BuzzerValue", "False"));
            buzzer.Blink = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "BuzzerBlink", "False"));
        }

        public void SaveXml(XmlElement xmlElement)
        {
            ResetValue();

            XmlHelper.SetValue(xmlElement, "Type", type.ToString());
            XmlHelper.SetValue(xmlElement, "RedLampValue", redLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "RedLampBlink", redLamp.Blink.ToString());
            XmlHelper.SetValue(xmlElement, "YellowLampValue", yellowLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "YellowLampBlink", yellowLamp.Blink.ToString());
            XmlHelper.SetValue(xmlElement, "GreenLampValue", greenLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "GreenLampBlink", greenLamp.Blink.ToString());
            XmlHelper.SetValue(xmlElement, "BuzzerValue", buzzer.Value.ToString());
            XmlHelper.SetValue(xmlElement, "BuzzerBlink", buzzer.Blink.ToString());
        }
    }

    public delegate TowerLampState GetDynamicStateDelegate();

    public class TowerLamp
    {
        public GetDynamicStateDelegate GetDynamicState;

        DigitalIoHandler digitalIoHandler;

        IoPort towerLampRed;
        IoPort towerLampYellow;
        IoPort towerLampGreen;
        IoPort towerBuzzer;

        Task workingTask;
        List<TowerLampState> towerLampStateList = new List<TowerLampState>();

        bool useBuzzerPlayer;
        public bool UseBuzzerPlayer
        {
            get { return useBuzzerPlayer; }
            set { useBuzzerPlayer = value; }
        }

        bool buzzerPlayerOn = false;
        private SoundPlayer buzzerPlayer = new SoundPlayer(DynMvp.Properties.Resources.BUZZER_1);

        public List<TowerLampState> TowerLampStateList
        {
            get { return towerLampStateList; }
        }
        
        public void Stop()
        {
            stopThreadFlag = true;

            if (workingTask != null)
                workingTask.Wait();

            TurnOffTowerLamp();
        }

        bool stopThreadFlag = false;
        TowerLampStateType towerLampStateType;
        public TowerLampStateType TowerLampStateType
        {
            set { towerLampStateType = value; }
        }

        int updateIntervalMs;

        public void Setup(DigitalIoHandler digitalIoHandler, int updateIntervalMs)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.updateIntervalMs = updateIntervalMs;

            AddTowerLampSateList();
        }

        public void Release()
        {
            TurnOffTowerLamp();
        }

        public void SetupPort(IoPort[] towerLampIoPort)
        {
            if (towerLampIoPort.Length == 4)
            {
                this.towerLampRed = towerLampIoPort[0];
                this.towerLampYellow = towerLampIoPort[1];
                this.towerLampGreen = towerLampIoPort[2];
                this.towerBuzzer = towerLampIoPort[3];
            }
        }

        private void AddTowerLampSateList()
        {
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Idle, new Lamp(false), new Lamp(true), new Lamp(false), new Lamp(false)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Wait, new Lamp(false), new Lamp(false), new Lamp(true), new Lamp(false)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Working, new Lamp(false), new Lamp(false), new Lamp(true, true), new Lamp(false)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Alarm, new Lamp(true), new Lamp(false), new Lamp(false), new Lamp(true)));
        }

        public void Save()
        {
            string filePath = String.Format(@"{0}\TowerLamp.xml", PathSettings.Instance().Config);
            Save(filePath);
        }

        public void Save(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement element = xmlDocument.CreateElement("TowerLamp");
            xmlDocument.AppendChild(element);
            foreach (TowerLampState state in towerLampStateList)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(state.Type.ToString());
                element.AppendChild(subElement);
                state.SaveXml(subElement);
            }

            XmlHelper.Save(xmlDocument, path);
        }

        public void Load()
        {
            string filePath = String.Format(@"{0}\TowerLamp.xml", PathSettings.Instance().Config);
            Load(filePath);
            Save(filePath);
        }

        public void Load(string path)
        {
            XmlDocument xmlDocument = XmlHelper.Load(path);
            if (xmlDocument == null)
                return;

            XmlElement element = xmlDocument.DocumentElement;
            string[] types = Enum.GetNames(typeof(TowerLampStateType));
            foreach (string type in types)
            {
                XmlElement subElement = element[type];
                if (subElement == null)
                {
                    continue;
                }

                // 중복된 상태 제거
                List<TowerLampState> findList = towerLampStateList.FindAll(f => f.Type.ToString() == type);
                foreach (TowerLampState find in findList)
                {
                    towerLampStateList.Remove(find);
                }

                TowerLampState state = new TowerLampState();
                state.LoadXml(subElement);
                towerLampStateList.Add(state);
            }
        }

        public TowerLampState GetState()
        {
            return towerLampStateList.Find(x => x.Type == towerLampStateType);
        }

        public TowerLampState GetState(TowerLampStateType type)
        {
            return towerLampStateList.Find(x => x.Type == type);
        }

        public void SetState(TowerLampStateType type)
        {
            towerLampStateType = type;
            TowerLampState state = GetState();
            state?.ResetValue();
        }

        private void TurnOnTowerLamp(TowerLampState towerLampState)
        {
            digitalIoHandler.WriteOutput(towerLampRed, towerLampState.RedLamp.Value);
            digitalIoHandler.WriteOutput(towerLampYellow, towerLampState.YellowLamp.Value);
            digitalIoHandler.WriteOutput(towerLampGreen, towerLampState.GreenLamp.Value);
            digitalIoHandler.WriteOutput(towerBuzzer, towerLampState.Buzzer.Value);
        }

        public void TurnOnTowerLamp()
        {
            TowerLampState state = GetState();

            TurnOnTowerLamp(state);
            state.UpdateValue();
        }

        public void TurnOffTowerLamp()
        {
            digitalIoHandler.WriteOutput(towerLampRed, false);
            digitalIoHandler.WriteOutput(towerLampYellow, false);
            digitalIoHandler.WriteOutput(towerLampGreen, false);
            digitalIoHandler.WriteOutput(towerBuzzer, false);
        }

        public void Start()
        {
            towerLampStateType = TowerLampStateType.Idle;
            workingTask = new Task(new Action(WorkingProc));
            workingTask.Start();
        }

        public void WorkingProc()
        {
            TowerLampState oldState = null;
            int saveStateCount = 0;

            while (stopThreadFlag == false)
            {
                TowerLampState state;

                if (!ErrorManager.Instance().IsCleared())
                {
                    state = GetState(TowerLampStateType.Alarm);
                    state.Buzzer.Value = ErrorManager.Instance().BuzzerOn;

                    if (ErrorManager.Instance().BuzzerOn)
                    {
                        if (buzzerPlayerOn == false)
                        {
                            buzzerPlayerOn = true;
                            buzzerPlayer.Play();
                        }
                    }
                    else
                    {
                        if (buzzerPlayerOn == true)
                        {
                            buzzerPlayerOn = false;
                            buzzerPlayer.Stop();
                        }
                    }
                }
                else
                {
                    if (buzzerPlayerOn == true)
                    {
                        buzzerPlayerOn = false;
                        buzzerPlayer.Stop();
                    }

                    if (GetDynamicState != null)
                        state = GetDynamicState();
                    else
                        state = GetState();
                }

                if (oldState?.Type == state?.Type)
                    saveStateCount++;
                else
                    saveStateCount = int.MaxValue;

                if (saveStateCount > 5)
                {
                    TurnOnTowerLamp(state);
                    state.UpdateValue();
                    saveStateCount = 0;
                }

                oldState = state;

                //Thread.Sleep(updateIntervalMs);
                //TurnOffTowerLamp();
                Thread.Sleep(updateIntervalMs);
            }
        }
    }
}
