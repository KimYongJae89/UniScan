using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Pinhole.UI.ControlPanel;
using DynMvp.Device.Serial;
using System.IO;
using UniEye.Base.Settings;
using DynMvp.UI;
using UniEye.Base.UI;
using UniEye.Base.Settings.UI;
using System.Threading;
using UniEye.Base;
using UniScanM.Pinhole.Operation;
using UniEye.Base.UI.CameraCalibration;
using UniScanM.Pinhole.Settings;
using DynMvp.Base;
using DynMvp.Authentication;
using UniScanM.UI.MenuPage.AutoTune;
using DynMvp.Devices.Light;

namespace UniScanM.Pinhole.UI.MenuPanel
{
    public partial class SettingPage : UserControl, IMainTabPage, ISettingPage, IMultiLanguageSupport
    {
        //Timer speedTimer = null;
        
        int oldSelectedTabIndex = -1;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            StringManager.AddListener(this);
        }

        private void SettingPanel_Load(object sender, EventArgs e)
        {
            oldSelectedTabIndex = 0;
            propertyGrid.SelectedObject = PinholeSettings.Instance();
        }

        private void buttonEncoder_Click(object sender, EventArgs e)
        {
            Form encoderForm = new Form();
            SerialEncoderPanel virtualControlPanel = new SerialEncoderPanel((SerialEncoderV105)SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder));
            encoderForm.Size = virtualControlPanel.Size;
            virtualControlPanel.Dock = DockStyle.Fill;
            encoderForm.Controls.Add(virtualControlPanel);
            encoderForm.Text = "Encoder Setting Form";
            encoderForm.TopMost = true;
            encoderForm.Show();
        }

        private void buttonMotion_Click(object sender, EventArgs e)
        {
            DynMvp.Devices.UI.MotionControlForm motionControlForm = new DynMvp.Devices.UI.MotionControlForm();
            motionControlForm.Intialize(SystemManager.Instance().DeviceBox.AxisConfiguration);
            motionControlForm.Show();
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            CameraCalibrationForm form = new CameraCalibrationForm();
            form.Initialize();
            form.Show();
        }
        
        public void EnableControls(UserType userType)
        {
            groupDeveloper.Visible = (userType == UserType.Admin);
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            TimeSettings.Instance().Load();
            PinholeSettings.Instance().Load();
            OperationSettings.Instance().Load();
            UISettings.Instance().Load();
            LoadSettings();
            this.Visible = visibleFlag;
        }
   
        private void LoadSettings()
        {
            numNGSignalHoldTime.Value = TimeSettings.Instance().NgSignalHoldTime;

            numMaxDefectCount.Value = PinholeSettings.Instance().MaxDefect;
            numDefectPenSize.Value = PinholeSettings.Instance().DefectPenSize;
            checkUseReject.Checked = PinholeSettings.Instance().UseReject;
            numDefectSmallSizeWidth.Value = (decimal)PinholeSettings.Instance().SmallSize.Width;
            numDefectSmallSizeHeight.Value = (decimal)PinholeSettings.Instance().SmallSize.Height;
            //numSkipRange.Value = (int)PinholeSettings.Instance().SkipLength;
            numSkipRange.Value  = (decimal)(PinholeSettings.Instance().SkipLength * PinholeSettings.Instance().PixelResolution / 1000.0f);
            numLightBrightness.Value = LightValueHelper.Get_LightValueToPercentage(PinholeSettings.Instance().DefalutLightValue);

            numResolution.Value = (decimal)PinholeSettings.Instance().PixelResolution;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SaveSettings()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        public void UpdateControl(string item, object value)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), item, value);
                return;
            }
        }
        
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if (oldSelectedTabIndex >= 0)
            //{                
            //    oldPage.PageVisibleChanged(false);
            //}
            //int newSelectedTabIndex = e.TabPageIndex;
            
            ////newPage.PageVisibleChanged(true);

            //oldSelectedTabIndex = newSelectedTabIndex;

        }

        private void buttonMachineIF_Click(object sender, EventArgs e)
        {
            SettingPageMachineIfPanel machineIfPanel = new SettingPageMachineIfPanel();
            machineIfPanel.Initialize(MachineSettings.Instance().MachineIfSetting);
            //cameraControlPanel.Dock = DockStyle.Fill;

            Form machineIfForm = new Form();
            machineIfForm.Controls.Add(machineIfPanel);
            machineIfForm.Size = machineIfPanel.Size;
            machineIfForm.AutoSize = true;
            machineIfForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            machineIfForm.TopMost = true;
            machineIfForm.Show();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            TimeSettings.Instance().NgSignalHoldTime = (int)numNGSignalHoldTime.Value;
            PinholeSettings.Instance().MaxDefect = (int)numMaxDefectCount.Value;
            PinholeSettings.Instance().DefectPenSize = (int)numDefectPenSize.Value;

            SizeF smallSize = new SizeF((float)numDefectSmallSizeWidth.Value, (float)numDefectSmallSizeHeight.Value);
            
           
            PinholeSettings.Instance().UseReject = checkUseReject.Checked;
            PinholeSettings.Instance().PixelResolution = (int)numResolution.Value;
            PinholeSettings.Instance().SmallSize = smallSize;
            PinholeSettings.Instance().DefalutLightValue = LightValueHelper.Get_PercentageToLightValue((int)numLightBrightness.Value);

            PinholeSettings.Instance().SkipLength  = Convert.ToInt32(numSkipRange.Value  * 1000 / PinholeSettings.Instance().PixelResolution);

            float test = Convert.ToInt32(numSkipRange.Value * 1000 / PinholeSettings.Instance().PixelResolution);

            UISettings.Instance().Save();

            PinholeSettings.Instance().Save();
            OperationSettings.Instance().Save();
            AdditionalSettings.Instance().Save();
            TimeSettings.Instance().Save();
            Data.Model curModel = (Data.Model)SystemManager.Instance().CurrentModel;
            curModel.LightParamSet.LightParamList[0].LightValue.Value[0] = PinholeSettings.Instance().DefalutLightValue;
            Data.ModelManager modelManager = (Data.ModelManager)SystemManager.Instance().ModelManager;
            modelManager.SaveModel(curModel);

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);
        }

        private void ApplySettings()
        {
            AdditionalSettingsForm form = new AdditionalSettingsForm();
            form.SetInstance(AdditionalSettings.Instance());
            form.ShowDialog();

            TimeSettings.Instance().NgSignalHoldTime = (int)numNGSignalHoldTime.Value;

            //SizeF cam1pel = new SizeF((float)numCam1PelWidth.Value, (float)numCam1PelHeight.Value);
            //SizeF cam2pel = new SizeF((float)numCam2PelWidth.Value, (float)numCam2PelHeight.Value);
            

            AdditionalSettings.Instance().Save();
            TimeSettings.Instance().Save();
        }

     



        private void buttonSave_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {

        }

        private void numDefectBigSizeWidth_ValueChanged(object sender, EventArgs e)
        {

        }

        void IMultiLanguageSupport.UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
