using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapixoGrabTest
{
    public partial class Form1 : Form
    {
        public CameraMilCXP CameraMilCXP { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        private void buttonInitMaster_Click(object sender, EventArgs e)
        {
            try
            {
                // MilSystemType systemType, uint systemNum, uint digitizerNum, CameraType cameraType, string dcfFileName
                CameraInfoMil cameraInfoMil = new CameraInfoMil()
                {
                    SystemType = MilSystemType.Rapixo,
                    SystemNum = 0,
                    DigitizerNum = 0,
                    ClientType = EClientType.Master,
                    CameraType = CameraType.CXP,
                    DcfFilePath = @"D:\UniScan\VIEWORKS-VT-18K3.5X-H140 - Master.dcf",
                };
                Initialize(cameraInfoMil);

                AddLog($"Init Done (SystemId: {this.CameraMilCXP.MilSystem.System}, DigitizerId: {this.CameraMilCXP.Digitizer})");
                UpdateProperties();
            }
            catch (Exception ex)
            {
                Release();
                AddLog($"{ex.GetType()}: {ex.Message}");
            }
        }

        private void buttonInitSlave_Click(object sender, EventArgs e)
        {
            try
            {
                CameraInfoMil cameraInfoMil = new CameraInfoMil()
                {
                    SystemType = MilSystemType.Rapixo,
                    SystemNum = 0,
                    DigitizerNum = 0,
                    ClientType = EClientType.Slave,
                    CameraType = CameraType.CXP,
                    DcfFilePath = @"D:\UniScan\VIEWORKS-VT-18K3.5X-H140 - Slave.dcf"
                };
                Initialize(cameraInfoMil);

                AddLog($"Init Done (SystemId: {this.CameraMilCXP.MilSystem.System}, DigitizerId: {this.CameraMilCXP.Digitizer})");
            }
            catch (Exception ex)
            {
                Release();
                AddLog($"{ex.GetType()}: {ex.Message}");
            }
            UpdateProperties();
        }

        private void Initialize(CameraInfoMil cameraInfoMil)
        {
            if (!MatroxHelper.IsAppInitialized)
                MatroxHelper.InitApplication();

            if (this.CameraMilCXP == null)
            {
                this.CameraMilCXP = new CameraMilCXP(cameraInfoMil);
                this.CameraMilCXP.ImageGrabbed += CameraMil_ImageGrabbed;
                this.CameraMilCXP.Initialize(false);
            }
        }

        private void UpdateProperties()
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)this.CameraMilCXP.CameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return;

            CameraMilCxpControlProperties properties = new CameraMilCxpControlProperties()
            {
                AcquisitionLineRate = this.CameraMilCXP.GetAcquisitionLineRate(),
                ExposureMs = this.CameraMilCXP.GetDeviceExposureMs(),
                TriggerMode = this.CameraMilCXP.GetTriggerMode(),
                ScanMode = this.CameraMilCXP.GetOperationMode(),
                TDIStages = this.CameraMilCXP.GetTdiStage(),
                AnalogGain = this.CameraMilCXP.GetAnalogGain(),
                ScanDirectionType = this.CameraMilCXP.GetScanDirectionType(),
                TriggerRescalerMode = this.CameraMilCXP.GetTriggerRescalerMode(),
                TriggerRescalerRate = this.CameraMilCXP.GetTriggerRescalerRate(),
                ReverseX = this.CameraMilCXP.GetReverseX(),
                OffsetX = this.CameraMilCXP.GetOffsetX(),
            };

            properties.SetImageSize(this.CameraMilCXP.ImageSize.Width, this.CameraMilCXP.ImageSize.Height);
            this.propertyGrid1.SelectedObject = properties;
        }

        private void ApplyProperties()
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)this.CameraMilCXP.CameraInfo;
            if (cameraInfoMil.ClientType == EClientType.Slave)
                return;

            CameraMilCxpControlProperties properties = this.propertyGrid1.SelectedObject as CameraMilCxpControlProperties;
            if (properties == null)
                return;

            this.CameraMilCXP.SetAcquisitionLineRate(properties.AcquisitionLineRate);
            this.CameraMilCXP.SetDeviceExposure(properties.ExposureMs);
            this.CameraMilCXP.SetTriggerMode(properties.TriggerMode, TriggerType.RisingEdge);
            this.CameraMilCXP.SetOperationMode(properties.ScanMode);
            this.CameraMilCXP.SetTdiStage(properties.TDIStages);

            this.CameraMilCXP.SetAnalogGain(properties.AnalogGain);
            this.CameraMilCXP.SetScanDirectionType(properties.ScanDirectionType);
            this.CameraMilCXP.SetTriggerRescalerMode(properties.TriggerRescalerMode);
            this.CameraMilCXP.SetTriggerRescalerRate(properties.TriggerRescalerRate);
            this.CameraMilCXP.SetReverseX(properties.ReverseX);
            this.CameraMilCXP.SetOffsetX(properties.OffsetX);

            UpdateProperties();
        }

        private void CameraMil_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            Task.Run(() =>
            {
                ImageD imageD = imageDevice.GetGrabbedImage(ptr);
                CameraBufferTag tag = (CameraBufferTag)imageD.Tag;

                AddLog($"[{imageDevice.Name}] Grabbed on [{tag.FrameId}], Size {tag.FrameSize.Width}x{tag.FrameSize.Height}, GrabPerSec: {imageDevice.GrabPerSec:F02}");
                imageD.SaveImage(@"C:\temp\temp.bmp");
                AddLog($"Saved");
            });
        }

        private void buttonRelease_Click(object sender, EventArgs e)
        {
            Release();
            AddLog($"Release Done");
        }

        private void Release()
        {
            this.CameraMilCXP?.Release();
            this.CameraMilCXP = null;
            MatroxHelper.FreeApplication();
        }

        private void buttonGrabSingle_Click(object sender, EventArgs e)
        {
            if (this.CameraMilCXP == null)
                return;

            AddLog($"[{this.CameraMilCXP.Name}] GrabOnce");
            this.CameraMilCXP?.GrabOnce();
        }

        private void buttonGrabMulti_Click(object sender, EventArgs e)
        {
            if (this.CameraMilCXP == null)
                return;

            AddLog($"[{this.CameraMilCXP.Name}] GrabMulti");
            this.CameraMilCXP?.GrabMulti(-1);
        }

        private void buttonGrabStop_Click(object sender, EventArgs e)
        {
            if (this.CameraMilCXP == null)
                return;

            AddLog($"[{this.CameraMilCXP.Name}] GrabStop");
            this.CameraMilCXP?.Stop();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ApplyProperties();
        }

        private delegate void AddLogDel(string text);
        private void AddLog(string text)
        {
            if (this.listBox1.InvokeRequired)
            {
                this.listBox1.Invoke(new AddLogDel(AddLog), text);
                return;
            }

            this.listBox1.Items.Insert(0, text);
        }
    }
}
