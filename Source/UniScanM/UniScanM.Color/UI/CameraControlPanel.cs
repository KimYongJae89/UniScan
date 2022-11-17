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
using UniEye.Base.Settings;

using UniScanM.ColorSens.Algorithm;

using DynMvp.UI.Touch;
using UniScanM.ColorSens.Algorithm;


namespace UniScanM.ColorSens.UI
{
    public partial class CameraControlPanel : UserControl
    {
        InspectSheetBrightness inspector=null;
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

        internal void Initialize(ImageDeviceHandler imageDeviceHandler, List<Calibration> cameraCalibrationList, LightCtrlHandler lightCtrlHandler)
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
        private void CameraControlPanel_Leave(object sender, EventArgs e)
        {
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                if (imageDevice is Camera)
                {
                    imageDevice.ImageGrabbed = null;
                }
            }
        }

        private delegate void RevisionHistogramDelegate(ImageD image);
        private void RevisionHistogram(ImageD image)
        {
            if (chart_Histogram.InvokeRequired)
            {
                BeginInvoke(new RevisionHistogramDelegate(RevisionHistogram), image);
                return;
            }
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraControlPanel::RevisionHistogram"));

            Image2D image2D = image as Image2D;
            AlgoImage fullImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);

            //long[] histo = imageProcessing.Histogram(fullImage);
            long[] histo =  autoLightTune.Histogram(fullImage);
            for (int i = 0; i < histo.Length; i++)
                if (histo[i] < 1) histo[i] = 1;
            chart_Histogram.Series[0].Points.Clear();//logarithm은 0또는 음수를 허용하지 않음
            chart_Histogram.Series[1].Points.Clear();

            foreach (var data in histo)
            {
                chart_Histogram.Series[0].Points.Add(data);//logarithm은 0또는 음수를 허용하지 않음
                chart_Histogram.Series[1].Points.Add(data);
            }
        }

        private delegate void RevisionProjectionDelegate(ImageD image);
        private void RevisionProjection(ImageD image)
        {
            if (chart_Histogram.InvokeRequired)
            {
                BeginInvoke(new RevisionProjectionDelegate(RevisionProjection), image);
                return;
            }
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraControlPanel::RevisionProjection"));

            Image2D image2D = image as Image2D;
            AlgoImage fullImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);

            // filter
            imageProcessing.Average(fullImage);
            AlgoImage resizeImage = ImageBuilder.Build(fullImage.LibraryType, fullImage.ImageType, fullImage.Width / 16, fullImage.Height / 16);
            imageProcessing.Resize(fullImage, resizeImage);
            // Projection
            float[] projection = imageProcessing.Projection(resizeImage, Direction.Vertical, ProjectionType.Mean);


            //1. Revision projection
            chart_Projection.Series[0].Points.Clear();
            //chart_Projection.Series[0].set
            foreach (var data in projection)
            {
                chart_Projection.Series[0].Points.Add(data);
            }
        }

        private delegate void RevisionResultGraphDelegate(ImageD image);
        private void RevisionResultGraph(ImageD image)
        {
            if (chart_Result.InvokeRequired)
            {
                BeginInvoke(new RevisionResultGraphDelegate(RevisionResultGraph), image);
                return;
            }
            if (inspector == null)
            {
                int SeperateStep = 16;
                double SheetLength_mm = 1500;
                double ResolutionLine_um = 1 * 1000;
                inspector = new InspectSheetBrightness(SeperateStep, SheetLength_mm, ResolutionLine_um);
            }
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraControlPanel::RevisionResultGraph"));
            Image2D image2D = image as Image2D;
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image2D, ImageType.Grey);

            inspector.AddImage(algoImage);
            float []Result = inspector.Inspect();
            //1. Revision projection
            chart_Result.Series[0].Points.Clear();
            //chart_Projection.Series[0].set
            foreach (var data in Result)
            {
                chart_Result.Series[0].Points.Add(data);
            }
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
            buttonCameraGrabExposure.Value = (Decimal)(exposureMs*1000.0);
        }

        private void buttonCameraGrabExposure_ValueChanged(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();
            lock (selectedCamera)
            {
                float exp_us = (float)buttonCameraGrabExposure.Value;// us 단위 노출시간

                bool ok;
                ok = selectedCamera.SetExposureTime(exp_us);//us 단위
  
                double LineRateHz = selectedCamera.GetAcquisitionLineRate();
                labelCameraFreq.Text = string.Format("{0:F4}", LineRateHz);
            }
        }

        private void buttonCameraGrabOnce_Click(object sender, EventArgs e)
        {
            Camera selectedCamera = GetSelectedCamera();
            selectedCamera.ImageGrabbed += selectedCamera_ImageGrabbed;
            selectedCamera.SetTriggerMode(TriggerMode.Software);
            selectedCamera.GrabOnce();
            UpdateGrabButtonState(true);

            selectedCamera.WaitGrabDone(1000);

            //selectedCamera.ImageGrabbed -= selectedCamera_ImageGrabbed;
            selectedCamera.ImageGrabbed -= null;
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

                    Image2D cloneImage = (Image2D)image2D.Clone();
                    Bitmap bitmap = image2D.ToBitmap();
                    drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
                    drawBox.UpdateImage(bitmap);
                    bitmap.Dispose();

                    RevisionHistogram(cloneImage);
                    RevisionProjection(cloneImage);
                    RevisionResultGraph(cloneImage);


                    //image2D.Dispose();
                    //image2D = null;

                    // mmmmmmmmmmmeeeeeeeeeeeeemmmmmmmmmmoooooooooooorrrrrrrrrrrryyyyyyyyyyy 181818181818
                    GC.Collect();
                    GC.WaitForFullGCComplete();

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


        FinderSheetPeriod findsheetperiodalg = new FinderSheetPeriod();
        private void Calibration_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {

            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            Image2D imgBuf = (Image2D)imageD.Clone();
            imgBuf.Tag = imageD.Tag;
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imgBuf, ImageType.Grey);

            findsheetperiodalg.AddImage(algoImage);

            Bitmap bitmap = imgBuf.ToBitmap();
            drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
            drawBox.UpdateImage(bitmap);
        }

        private void buttonCameraCalibration_Click(object sender, EventArgs e)
        {
            //CameraCalibrationForm form = new CameraCalibrationForm();
            //form.Initialize();
            //form.Show();
            ////////////////////////////////////////////////////////////////////

            //findsheetperiodalg.Reset();
            //Camera selectedCamera = GetSelectedCamera();
            //selectedCamera.ImageGrabbed += Calibration_ImageGrabbed;
            //selectedCamera.SetTriggerMode(TriggerMode.Software);
            //selectedCamera.GrabMulti(30);
            //UpdateGrabButtonState(true);

        }

        private void button_Calculate_Click(object sender, EventArgs e)
        {
            double umPerPixel = 0;
            double sheetpatternperion_mm = 471.238;
            int onesheetline = findsheetperiodalg.CalculatePixelResolution(sheetpatternperion_mm, ref umPerPixel);
            label_Result.Text = string.Format("{0:0}, {1:0.000}", onesheetline, umPerPixel);

        }
        private void dataGridViewLight_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this.onUpdate)
                return;

            if (e.RowIndex < 0 || e.ColumnIndex != 2)
                return;

            DataGridViewRow row = this.dataGridViewLight.Rows[e.RowIndex];
            int ctrlNo = (int)row.Cells[0].Value;
            int chanNo = (int)row.Cells[1].Value;
            int newValue = int.Parse(row.Cells[2].Value.ToString());
            this.lightParamSet.LightParamList[ctrlNo].LightValue.Value[chanNo] = newValue;
        }


        private bool requestStop = false;
        private void button_LightTune_Click(object sender, EventArgs e)
        {
            //requestStop = true;
            autoLightTune autotune = new autoLightTune(0.03f);
            int lightvalue= autotune.tune();
            //if (runningThreadHandler != null)
            //{
            // SimpleProgressForm form = new SimpleProgressForm("Light Tune");

            // form.Show(() => );
            //}

        }
        void stop_tune() //stop camera..
        {

        }
        private void button_GetBrightness_Click(object sender, EventArgs e)
        {

        }
    }
}
