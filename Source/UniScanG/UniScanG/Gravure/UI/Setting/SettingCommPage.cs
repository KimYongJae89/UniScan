using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using UniScanG.Gravure.Settings;
using UniScanG.Settings.UI;
using DynMvp.Devices.UI;
using DynMvp.Device.Serial;
using System.Diagnostics;
using DynMvp.UI.Touch;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingCommPage : UserControl, ISettingSubPage, IMultiLanguageSupport
    {
        private bool onUpdate;

        Form ioPortViewer = null;
        Form encoderSettingForm = null;

        public bool ShowEncoderButton { get; set; }
        public bool ShowImsPowControlButton { get; set; }

        public SettingCommPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            
            StringManager.AddListener(this);
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void SettingCommPage_Load(object sender, EventArgs e)
        {
            launchImsArgs.Text = "-Restart -CameraConfiguration FastMode";

            launchImsArgs.Visible = false;
#if DEBUG
            launchImsArgs.Visible = true;
#endif
        }

        public void Initialize()
        {
            SerialEncoder serialEncoder = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo is SerialEncoderInfo) as SerialEncoder;
            openEncoderSetting.Enabled = ShowEncoderButton && (serialEncoder != null);

            startIms.Visible = ShowImsPowControlButton;

            onUpdate = true;
            onUpdate = false;
            UpdateData();
        }

        public void UpdateData()
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            onUpdate = true;

            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();
            this.checkBoxAutoOperation.Checked = settings.AutoOperation;
            
            onUpdate = false;
        }

        private void ApplyData()
        {
            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();
            settings.AutoOperation = this.checkBoxAutoOperation.Checked;

            //settings.Save();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            ApplyData();
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            ApplyData();
        }

        private void openIoViewer_Click(object sender, EventArgs e)
        {
            if (this.ioPortViewer == null)
            {
                this.ioPortViewer = new DynMvp.Devices.UI.IoPortViewer(SystemManager.Instance().DeviceBox.DigitalIoHandler, SystemManager.Instance().DeviceBox.PortMap);
                this.ioPortViewer.SizeGripStyle = SizeGripStyle.Hide;
                this.ioPortViewer.FormClosed += new FormClosedEventHandler((s, a) => this.ioPortViewer = null);
                //this.ioPortViewer.Show(this);
            }

            if (!this.ioPortViewer.Visible)
                this.ioPortViewer.Show(this);

            this.ioPortViewer.Focus();
        }

        private void openEncoderSetting_Click(object sender, EventArgs e)
        {
            SerialEncoder serialEncoder = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo is SerialEncoderInfo) as SerialEncoder;
            if (serialEncoder == null)
                return;

            if (this.encoderSettingForm == null)
            {
                SerialEncoderPanel serialEncoderPanel = new SerialEncoderPanel();
                serialEncoderPanel.Initialize(serialEncoder);

                this.encoderSettingForm = new Form();
                this.encoderSettingForm.SizeGripStyle = SizeGripStyle.Hide;
                this.encoderSettingForm.Controls.Add(serialEncoderPanel);
                this.encoderSettingForm.AutoSize = true;
                this.encoderSettingForm.FormClosed += new FormClosedEventHandler((s, a) => 
                {
                    UniEye.Base.Settings.MachineSettings.Instance().Save();
                    this.encoderSettingForm = null;
                });
                this.encoderSettingForm.Show(this);
            }
            this.encoderSettingForm.Focus();
        }

        private void startIms_Click(object sender, EventArgs e)
        {
            new DynMvp.UI.Touch.SimpleProgressForm().Show(() =>
            {
                Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<Common.Data.InspectorObj> objList = server.GetInspectorList();
                objList.ForEach(f => SystemManager.Instance().DeviceController.Startup(f.Info.GetName()));
            });
           
        }

        private void shutdownIms_Click(object sender, EventArgs e)
        {
            new DynMvp.UI.Touch.SimpleProgressForm().Show(this, () =>
            {
                //if (ShowImsPowControlButton)
                {
                    if (MessageForm.Show(this, "Turn all IM's Power Off?", MessageFormType.YesNo) == DialogResult.No)
                        return;
                }

                Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<Common.Data.InspectorObj> objList = server.GetInspectorList();
                objList.AsParallel().ForAll(f => SystemManager.Instance().DeviceController.Shutdown(f.Info.GetName(), false));
            });
        }

        private void resetIMs_Click(object sender, EventArgs e)
        {
            new DynMvp.UI.Touch.SimpleProgressForm().Show(this, () =>
            {
                Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<Common.Data.InspectorObj> objList = server.GetInspectorList();
                objList.AsParallel().ForAll(f => SystemManager.Instance().DeviceController.Shutdown(f.Info.GetName(), true));
            });
        }

        private void launchImsArgs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string arg = this.launchImsArgs.Text;
                new DynMvp.UI.Touch.SimpleProgressForm().Show(this, () =>
                {
                    Common.IServerExchangeOperator server = (Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                    List<Common.Data.InspectorObj> objList = server.GetInspectorList();
                    objList.AsParallel().ForAll(f => SystemManager.Instance().DeviceController.Launch(f.Info.GetName(), arg.Split(' ')));
                });
            }
        }
    }
}
