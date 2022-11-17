using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;
using UniScanM.CEDMS.Settings;
using UniScanM.CEDMS.UI.Chart;
using DynMvp.UI.Touch;
using UniScanM.CEDMS.Data;
using UniEye.Base.MachineInterface;
using System.Threading;
using UniScanM.CEDMS.MachineIF;
using UniScanM.MachineIF;
using UniScanM.CEDMS.Operation;
using DynMvp.Device.Device.Serial.Sensor.SC_HG1_485;
using UniEye.Base.Settings;
using System.Xml;
using DynMvp.Device.Serial.Sensor;

namespace UniScanM.CEDMS.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        SerialSensor serialSensor = null;
        private bool EnableFlag { get; set; } = false;

        public InspectionPanelRight()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            SerialDeviceHandler serialDeviceHandler = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialDevice serialDevice = serialDeviceHandler.Find(x => x.DeviceInfo.DeviceType == ESerialDeviceType.SerialSensor);

            if (serialDevice is SerialSensor)
            {
                SerialSensor serialSensor = (SerialSensor)serialDevice;

                if (serialSensor != null && serialSensor.SerialPortEx != null)
                {
                    this.serialSensor = serialSensor;
                    Load();
                }
            }

            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);
        }

        private void Clear()
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void ProductInspectedDelegate(DynMvp.InspData.InspectionResult inspectionResult);
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                Invoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);
                return;
            }

            CEDMS.Data.InspectionResult cedmsSInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;
            this.SuspendLayout();

            UiHelper.SuspendDrawing(labelState);

            if (SystemState.Instance().OpState != OpState.Idle)
            {
                if (cedmsSInspectionResult.ZeroingComplete == false)
                {
                    CEDMSSettings cedmsSetting = (CEDMSSettings)CEDMSSettings.Instance();
                    if (cedmsSInspectionResult.ZeroingStable)
                    {
                        labelState.Text = string.Format("{0} ({1}/{2})", StringManager.GetString("Zeroing"), cedmsSInspectionResult.ZeroingNum, cedmsSetting.DataCountForZeroSetting);
                        labelState.BackColor = Color.Gold;
                        labelState.ForeColor = Color.Black;
                    }
                    else
                    {
                        //double threshold = Math.Abs(cedmsSetting.LineStopUpper + cedmsSetting.LineStopLower);
                        //labelState.Text = string.Format("{0} ({1:F2}/{2:F2})", StringManager.GetString("ORG. Unstable"), cedmsSInspectionResult.ZeroingVariance, threshold);
                        labelState.BackColor = Color.Red;
                        labelState.ForeColor = Color.White;
                    }

                    progressBarZeroing.Value = (int)(((float)cedmsSInspectionResult.ZeroingNum / (float)CEDMSSettings.Instance().DataCountForZeroSetting) * 100.0f);
                }
                else
                {
                    labelState.Text = StringManager.GetString("Measure");
                    labelState.BackColor = Color.Green;
                    labelState.ForeColor = Color.White;
                }
            }
            UiHelper.ResumeDrawing(labelState);
            this.ResumeLayout();
        }

        public void Initialize() { }
        delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if (InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            labelState.Text = StringManager.GetString("State");
            labelState.BackColor = Color.Black;

        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                this.Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            if (curOpState == OpState.Idle)
            {
                labelState.ForeColor = SystemColors.ControlText;
                labelState.BackColor = SystemColors.Control;
                labelState.Text = "";
                progressBarZeroing.Value = 0;
                return;
            }
        }


        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        private void buttonStateReset_Click(object sender, EventArgs e)
        {
            // TODO:[Song] 센서 자체 ZeroSet 기능 사용
            SystemManager.Instance().InspectRunner.ResetState();

            //SerialSensorSC_HG1_485 sensor = serialSensor as SerialSensorSC_HG1_485;

            //if (sensor != null)
            //{
            //    if (sensor.GetPresetInfo(1))
            //    {
            //        Thread.Sleep(10);
            //        sensor.Preset(true, false);
            //        Thread.Sleep(10);
            //        sensor.Preset(false, false);
            //    }
            //    Thread.Sleep(10);
            //    sensor.Preset(true, true);
            //    Thread.Sleep(10);
            //    sensor.Preset(false, true);
            //}
        }

        private void infeedValue_ValueChanged(object sender, EventArgs e)
        {
            if (infeedValue.Value < 10 || infeedValue.Value > 90)
            {
                MessageBox.Show("감도설정 범위는 10 ~ 90입니다. (Default 값 : 25)");
                infeedValue.Value = 25;
                return;
            }
            else
            {
                SerialSensorSC_HG1_485 sensor = serialSensor as SerialSensorSC_HG1_485;

                var percent = Convert.ToInt32(infeedValue.Value);
                if (sensor != null)
                {
                    //if (sensor.GetSensitivityUseInfo(1))
                    //{
                    //    //Thread.Sleep(10);
                    //    sensor.UserSensitivitySet(true, percent);
                    //}
                    //else
                    //{
                        sensor.UseSensitivity(true, true);
                        //Thread.Sleep(10);
                        sensor.UserSensitivitySet(true, percent);
                    //}

                    //var test = sensor.GetSensitivityValueInfo(1);
                }
                Save();
            }
        }

        private void outfeedValue_ValueChanged(object sender, EventArgs e)
        {
            if (outfeedValue.Value < 10 || outfeedValue.Value > 90)
            {
                MessageBox.Show("감도설정 범위는 10 ~ 90입니다. (Default 값 : 25)");
                outfeedValue.Value = 25;
                return;
            }
            else
            {
                SerialSensorSC_HG1_485 sensor = serialSensor as SerialSensorSC_HG1_485;

                var percent = Convert.ToInt32(outfeedValue.Value);
                if (sensor != null)
                {
                    //if (sensor.GetSensitivityUseInfo(1))
                    //{
                    //    Thread.Sleep(10);
                    //    sensor.UserSensitivitySet(false, percent);
                    //}
                    //else
                    //{
                        sensor.UseSensitivity(false, true);
                        //Thread.Sleep(10);
                        sensor.UserSensitivitySet(false, percent);
                    //}
                }
                Save();
            }
        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\sensitivitySetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement sensitivityElement = xmlDocument.CreateElement("", "Sensitivity", "");
            xmlDocument.AppendChild(sensitivityElement);

            XmlHelper.SetValue(sensitivityElement, "InfeedValue", infeedValue.Value.ToString());
            XmlHelper.SetValue(sensitivityElement, "OutfeedValue", outfeedValue.Value.ToString());

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\sensitivitySetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement sensitivityElement = xmlDocument["Sensitivity"];
            if (sensitivityElement == null)
                return;

            infeedValue.Value = Convert.ToInt32(XmlHelper.GetValue(sensitivityElement, "InfeedValue", "25"));
            outfeedValue.Value = Convert.ToInt32(XmlHelper.GetValue(sensitivityElement, "OutfeedValue", "25"));
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            EnableFlag = !checkOnTune.Checked;
            checkOnTune.Text = !EnableFlag ? StringManager.GetString("The sensitivity settings are available.") : StringManager.GetString("The sensitivity setting is not possible.");
            groupJudegementLevel.Enabled = !EnableFlag;
        }
    }
}
