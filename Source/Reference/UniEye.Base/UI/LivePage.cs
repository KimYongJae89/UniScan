using System;
using System.Linq;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using UniEye.Base.Settings;
using UniEye.Base.Device;
using DynMvp.Authentication;

namespace UniEye.Base.UI
{
    public partial class LivePage : UserControl, IMainTabPage
    {
        private DrawBox[] cameraViewArray;

        private object drawingLock = new object();
        private bool onLiveGrab = false;
        
        public LivePage()
        {
            InitializeComponent();

            // Laguage change
            measureMode.Text = StringManager.GetString(this.GetType().FullName,measureMode.Text);
            clearMeasure.Text = StringManager.GetString(this.GetType().FullName,clearMeasure.Text);
            zoomIn.Text = StringManager.GetString(this.GetType().FullName,zoomIn.Text);
            zoomOut.Text = StringManager.GetString(this.GetType().FullName,zoomOut.Text);
            buttonZoomFit.Text = StringManager.GetString(this.GetType().FullName,buttonZoomFit.Text);
            labelStep.Text = StringManager.GetString(this.GetType().FullName,labelStep.Text);
            labelExposure.Text = StringManager.GetString(this.GetType().FullName,labelExposure.Text);
        }

        public void Initialize()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin LivePage::Initialize");

            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            ImageDeviceHandler imageDeviceHandler = deviceBox.ImageDeviceHandler;

            int numCamera = imageDeviceHandler.Count;

            cameraViewArray = new DrawBox[numCamera];

            cameraViewPanel.ColumnStyles.Clear();
            cameraViewPanel.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));
            cameraViewPanel.ColumnCount = numCount;
            cameraViewPanel.RowCount = (int)Math.Floor(Math.Sqrt(numCamera));

            cameraViewPanel.ColumnStyles.Clear();
            cameraViewPanel.RowStyles.Clear();

            for (int i = 0; i < cameraViewPanel.ColumnCount; i++)
            {
                cameraViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / cameraViewPanel.ColumnCount));
            }

            for (int i = 0; i < cameraViewPanel.RowCount; i++)
            {
                cameraViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / cameraViewPanel.RowCount));
            }

            int index = 0;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                this.cameraViewArray[index] = new DrawBox();

                this.cameraViewArray[index].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.cameraViewArray[index].Dock = System.Windows.Forms.DockStyle.Fill;
                this.cameraViewArray[index].Name = "targetImage";
                this.cameraViewArray[index].TabIndex = 8;
                this.cameraViewArray[index].TabStop = false;
                this.cameraViewArray[index].Enable = true;
                this.cameraViewArray[index].MeasureMode = true;
                this.cameraViewArray[index].SetImageDevice(imageDevice);
                this.cameraViewArray[index].Tag = imageDevice;
                this.cameraViewArray[index].MouseDoubleClicked += cameraView_DoubleClick;

                if (imageDevice is Camera && deviceBox.CameraCalibrationList.Count() > imageDevice.Index)
                {
                    this.cameraViewArray[index].MeasureScaleX = deviceBox.CameraCalibrationList[(int)imageDevice.Index].PelSize.Width;
                    this.cameraViewArray[index].MeasureScaleY = deviceBox.CameraCalibrationList[(int)imageDevice.Index].PelSize.Height;
                }
                else
                {
                    this.cameraViewArray[index].MeasureScaleX = 1.0f;
                    this.cameraViewArray[index].MeasureScaleY = 1.0f;
                }

                int rowIndex = index / numCount;
                int colIndex = index % numCount;

                cameraViewPanel.Controls.Add(cameraViewArray[index], colIndex, rowIndex);

                index++;   
            }
            
            txtExposure.Text = TimeSettings.Instance().LiveViewExposureTime.ToString();

            LogHelper.Debug(LoggerType.StartUp, "End LivePage::Initialize");
        }

        private void cameraView_DoubleClick(DrawBox senderView)
        {
            int numCamera = cameraViewArray.Count();
            if (numCamera < 1)
                return;

            if (senderView.Parent == viewContainer)
            {
                int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));
                int cameraIndex = (int)senderView.LinkedImageDevice.Index;

                int rowIndex = cameraIndex / numCount;
                int colIndex = cameraIndex % numCount;

                cameraViewPanel.Controls.Add(senderView, colIndex, rowIndex);
                cameraViewPanel.Show();
            }
            else
            {
                senderView.Parent = viewContainer;
                cameraViewPanel.Hide();
            }

            senderView.ZoomFit();
        }

        private LightValue GetLightValue()
        {
            return new LightValue(MachineSettings.Instance().NumLight);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            foreach (DrawBox cameraView in cameraViewArray)
            {
                cameraView.LockLiveUpdate = onLiveGrab;
            }

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            if (onLiveGrab == false)
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(GetLightValue());
                
                SystemManager.Instance().LiveMode = true;
                foreach (ImageDevice imageDevice in imageDeviceHandler)
                {
                    imageDevice.SetExposureTime(TimeSettings.Instance().LiveViewExposureTime);
                    imageDevice.GrabMulti();
                }

                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Stop_90x116;
                txtExposure.Enabled = false;

                onLiveGrab = true;
            }
            else
            {
                SystemManager.Instance().LiveMode = false;
                foreach (ImageDevice imageDevice in imageDeviceHandler)
                {
                    imageDevice.Stop();
                }

                buttonStart.Appearance.Image = global::UniEye.Base.Properties.Resources.Start_90x116;

                txtExposure.Enabled = true;
                onLiveGrab = false;
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            }

            SystemManager.Instance().MainForm.EnableTabs();
        }

        private void measureMode_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DrawBox cameraView in cameraViewArray)
            {
                cameraView.MeasureMode = measureMode.Checked;
            }
        }

        private void clearMeasure_Click(object sender, EventArgs e)
        {
            foreach (DrawBox cameraView in cameraViewArray)
            {
                cameraView.TempFigureGroup.Clear();
                cameraView.Invalidate(true);
            }
        }

        private void zoomIn_Click(object sender, EventArgs e)
        {
            foreach (DrawBox cameraView in cameraViewArray)
            {                
                cameraView.ZoomIn();
            }
        }

        private void zoomOut_Click(object sender, EventArgs e)
        {
            foreach (DrawBox cameraView in cameraViewArray)
            {                
                cameraView.ZoomOut();
            }
        }

        private void LivePage_VisibleChanged(object sender, EventArgs e)
        {
            CameraViewZoomfit();
        }

        private void txtExposure_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (onLiveGrab == false && String.IsNullOrEmpty(txtExposure.Text) == false)
            {
                TimeSettings.Instance().LiveViewExposureTime = Convert.ToInt32(txtExposure.Text);
                TimeSettings.Instance().Save();
            }
        }

        private void buttonZoomFit_Click(object sender, EventArgs e)
        {
            CameraViewZoomfit();
        }

        private void CameraViewZoomfit()
        {
            if (cameraViewArray == null)
                return;

            foreach (DrawBox cameraView in cameraViewArray)
            {
                cameraView.ZoomFit();
            }
        }

        public void EnableControls(UserType user)
        {

        }
        
        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }
    }
}
