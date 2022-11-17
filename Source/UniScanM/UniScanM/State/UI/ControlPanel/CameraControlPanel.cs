using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Devices;
using DynMvp.Vision;
using DynMvp.Devices.Light;
using DynMvp.Data.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Base;
using System.Threading;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;

namespace UniScanM.UI.ControlPanel
{
    public partial class CameraControlPanel : UserControl
    {
        ImageDeviceHandler imageDeviceHandler;
        List<Calibration> cameraCalibrationList;
        LightCtrlHandler lightCtrlHandler;
        LightParamSet lightParamSet = null;

        Task imageUpdateTask = null;
        DrawBox drawBox;

        bool onUpdate = false;

        public CameraControlPanel()
        {
            InitializeComponent();
            this.Disposed += CameraControlPanel_Disposed;

            drawBox = new DrawBox();
            drawBox.AutoFitStyle = AutoFitStyle.FitWidthOnly;
            drawBox.Dock = DockStyle.Fill;
            groupBox1.Controls.Add(drawBox);
        }

        public void Initialize(ImageDeviceHandler imageDeviceHandler, List<Calibration> cameraCalibrationList, LightCtrlHandler lightCtrlHandler)
        {
            this.imageDeviceHandler = imageDeviceHandler;
            this.cameraCalibrationList = cameraCalibrationList;
            this.lightCtrlHandler = lightCtrlHandler;
        }
        
        private void CameraControlPanel_Load(object sender, EventArgs e)
        {
            this.onUpdate = true;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                if (imageDevice is Camera)
                {
                    comboBoxCameraDevice.Items.Add(imageDevice);
                }
            }

            if (comboBoxCameraDevice.Items.Count > 0)
                comboBoxCameraDevice.SelectedIndex = 0;

            UpdateLightGrid();
            this.onUpdate = false;
        }

        private void UpdateLightGrid()
        {
            dataGridViewLight.Rows.Clear();
            lightParamSet = this.lightCtrlHandler.GetLastLightParamSet();

            for (int i = 0; i < this.lightCtrlHandler.Count; i++)
            {
                LightCtrl lightCtrl = this.lightCtrlHandler.GetLightCtrl(i);
                LightValue lightValue = lightParamSet.LightParamList[i].LightValue;
                for (int j = 0; j < lightValue.NumLight; j++)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewLight, i, j, lightValue.Value[j]);
                    dataGridViewLight.Rows.Add(row);
                }
            }
        }

        private void CameraControlPanel_Disposed(object sender, EventArgs e)
        {
            buttonCameraGrabStop_Click(sender, e);
        }

        private Camera GetSelectedCamera()
        {
            return (Camera)comboBoxCameraDevice.SelectedItem;
        }

        private void comboBoxCameraDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            float exposureMs = GetSelectedCamera().GetDeviceExposureMs();
            buttonCameraGrabExposure.Value = (Decimal)(exposureMs);
        }

        private void buttonCameraGrabExposure_ValueChanged(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();
            lock (selectedCamera)
            {
                float framesPerSec = (float)buttonCameraGrabExposure.Value;
                float linesPerSec = framesPerSec * selectedCamera.ImageSize.Height;

                bool ok;
                if (selectedCamera is CameraVirtual)
                {
                    float msecPerFrame = (1 / framesPerSec) * 1000;
                    ok = selectedCamera.SetExposureTime(msecPerFrame * 1000);
                }
                else
                {
                    float msecPerLine = (1 / linesPerSec) * 1000;
                    ok = selectedCamera.SetExposureTime(msecPerLine * 1000);
                }

                if (ok)
                    labelCameraFreq.Text = string.Format("{0:F3}", linesPerSec / 1000);
                else
                {
                    int linePerFrame = selectedCamera.ImageSize.Height;
                    float sPerLine = selectedCamera.GetDeviceExposureMs()/1000;
                    buttonCameraGrabExposure.Value = (decimal)(1 / (linePerFrame * sPerLine));
                    //labelCameraFreq.Text = "-.----";
                }

            }
        }

        private void buttonCameraGrabOnce_Click(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();

            UpdateGrabButtonState(true);
            selectedCamera.ImageGrabbed += selectedCamera_ImageGrabbed;
            selectedCamera.SetTriggerMode(TriggerMode.Software);
            float grabHz = (float)buttonCameraGrabExposure.Value * selectedCamera.ImageSize.Height;
            float lineGrabUs = (1 / grabHz) * 1E6f;
            selectedCamera.SetExposureTime(lineGrabUs);
            selectedCamera.GrabOnce();

            bool ok = selectedCamera.WaitGrabDone(5000);
            this.labelGrabDoneState.Text = string.Format("GRAB {0}", ok ? "OK" : "FAIL");

            selectedCamera.ImageGrabbed -= selectedCamera_ImageGrabbed;
            selectedCamera.Stop();
            UpdateGrabButtonState(false);

        }

        private void buttonCameraGrabMulti_Click(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();
            selectedCamera.ImageGrabbed += selectedCamera_ImageGrabbed;
            selectedCamera.SetTriggerMode(TriggerMode.Software);
            selectedCamera.GrabMulti();
            UpdateGrabButtonState(true);
        }

        private void buttonCameraGrabStop_Click(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();
            if (selectedCamera != null)
            {
                selectedCamera.ImageGrabbed -= selectedCamera_ImageGrabbed;
                selectedCamera.Stop();
            }
            UpdateGrabButtonState(false);
        }

        private void UpdateGrabButtonState(bool v)
        {
            comboBoxCameraDevice.Enabled = !v;
            buttonCameraGrabExposure.Enabled = !v;
            buttonCameraGrabOnce.Enabled = !v;
            buttonCameraGrabMulti.Enabled = !v;
            buttonCameraGrabStop.Enabled = v;
        }

        private void selectedCamera_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            //if (Monitor.TryEnter(updateLock) == false)
            //    return;
            if (imageUpdateTask == null || imageUpdateTask.IsCompleted == true)
            {
                imageUpdateTask = new Task(() =>
                {
                    Image2D image2D = (Image2D)imageDevice.GetGrabbedImage(ptr);
                    if (image2D.IsUseIntPtr())
                    {
                        Image2D image2DD = (Image2D)image2D.Clone();
                        image2DD.ConvertFromPtr();
                        image2D = image2DD;
                    }
                    
                    Bitmap bitmap = image2D.ToBitmap();
                    drawBox.AutoFitStyle = AutoFitStyle.FitWidthOnly;
                    drawBox.UpdateImage(bitmap);
                    drawBox.ZoomFit();
                    bitmap.Dispose();

                    //image2D.Dispose();
                    //image2D = null;

                    // mmmmmmmmmmmeeeeeeeeeeeeemmmmmmmmmmoooooooooooorrrrrrrrrrrryyyyyyyyyyy 181818181818
                    //GC.Collect();
                    //GC.WaitForFullGCComplete();

                    //imageUpdateTask = null;
                });
                imageUpdateTask.Start();
            }
        }

        private void buttonLightAllOn_Click(object sender, EventArgs e)
        {
            //LightParam lightParam = new LightParam()
            lightCtrlHandler.TurnOn(this.lightParamSet.LightParamList[0]);
            UpdateLightGrid();
        }

        private void buttonLightAllOff_Click(object sender, EventArgs e)
        {
            lightCtrlHandler.TurnOff();
            UpdateLightGrid();
        }

        private void buttonCameraCalibration_Click(object sender, EventArgs e)
        {
            CameraCalibrationForm form = new CameraCalibrationForm();
            form.Initialize();
            form.Show();
        }

        private void dataGridViewLight_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.onUpdate)
                return;

            if (e.RowIndex<0 || e.ColumnIndex != 2)
                return;

            DataGridViewRow row = this.dataGridViewLight.Rows[e.RowIndex];
            int ctrlNo = (int)row.Cells[0].Value;
            int chanNo = (int)row.Cells[1].Value;
            int newValue =int.Parse(row.Cells[2].Value.ToString());
            this.lightParamSet.LightParamList[ctrlNo].LightValue.Value[chanNo] = newValue;
        }
    }
}
