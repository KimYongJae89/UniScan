using System;

using System.Linq;

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.Devices;
using DynMvp.Base;
using DynMvp.Devices.Light;
using UniEye.Base.Settings;
using DynMvp.Data.UI;
using DynMvp.UI;
using Infragistics.Win.DataVisualization;
using System.Threading;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DynMvp.UI.Touch;
using System.Diagnostics;

namespace UniEye.Base.UI.CameraCalibration
{
    public partial class CameraCalibrationForm : Form, IMultiLanguageSupport
    {
        public class ComboBoxItem
        {
            public string text = "";
            public LightParam lightParam = null;
            public int ctrlId = -1;
            public int ctrlCh = -1;
            public ComboBoxItem(string text, int ctrlId, int ctrlCh)
            {
                this.text = text;
                this.ctrlId = ctrlId;
                this.ctrlCh = ctrlCh;
            }
            public ComboBoxItem(string text, LightParam lightParam)
            {
                this.text = text;
                this.lightParam = lightParam;
            }
            public override string ToString()
            {
                return text;
            }
        }

        LightCtrlHandler lightCtrlHandler;
        LightParam lightParam;
        LightParamSet lightParamSet;
        ImageDeviceHandler imageDeviceHandler;
        Calibration curCameraCalibration;
        List<Calibration> cameraCalibrationList;
        DrawBox drawBox = null;
        ToolTip drawBoxToolTip = null;

        CalibrationResult lastCalibrationResult = null;

        ManualResetEvent imageGrabbedDone = null;

        CameraCalibrationPanel curCameraCalibrationPanel = null;

        Task asyncProcessTask = null;
        object lockObject = new object();

        public CameraCalibrationForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            drawBox = new DrawBox();
            drawBox.Dock = DockStyle.Fill;
            drawBox.Enable = false;
            drawBox.BackColor = Control.DefaultBackColor;
            drawBox.AutoFitStyle = AutoFitStyle.FitAll;
            drawBox.pictureBox.MouseMove += DrawBox_MouseMove;
            drawBox.pictureBox.MouseLeave += DrawBox_MouseLeave;
            panelDrawBox.Controls.Add(drawBox);

            lightParamSet = LightSettings.Instance().LightParamSet.Clone();

            drawBoxToolTip = new ToolTip();
            drawBoxToolTip.ShowAlways = true;

            this.imageGrabbedDone = new ManualResetEvent(true);

            CalibrationConstant calibrationConstant = new CalibrationConstant();
            //calibrationConstant.ChangeLanguage();
            calibrationConstant.Dock = DockStyle.Fill;

            CalibrationGrid calibrationGrid = new CalibrationGrid();
            //calibrationGrid.ChangeLanguage();
            calibrationGrid.Dock = DockStyle.Fill;

            CalibrationRuler calibrationRuler = new CalibrationRuler();
            //calibrationRuler.ChangeLanguage();
            calibrationRuler.Dock = DockStyle.Fill;

            tabPageContant.Controls.Add(calibrationConstant);
            tabPageGrid.Controls.Add(calibrationGrid);
            tabPageRuler.Controls.Add(calibrationRuler);
        }

        public void Initialize()
        {
            this.lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            this.imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            this.cameraCalibrationList = SystemManager.Instance().DeviceBox.CameraCalibrationList;
        }

        private void DrawBox_MouseLeave(object sender, EventArgs e)
        {
            drawBoxToolTip.Hide(this.drawBox);
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (drawBox.Image == null)
                return;

            CoordTransformer coordTransformer = drawBox.GetCoordTransformer();
            System.Drawing.Point imagePoint = coordTransformer.InverseTransform(e.Location);
            float fx = imagePoint.X * 100.0f / drawBox.Image.Width;
            float fy = imagePoint.Y * 100.0f / drawBox.Image.Height;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("X: {0}px ({1}%)", imagePoint.X, fx));
            sb.AppendLine(string.Format("Y: {0}px ({1}%)", imagePoint.Y, fy));
            drawBoxToolTip.Show(sb.ToString(), this.drawBox, e.Location.X + 10, e.Location.Y + 10);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //labelCamera.Text = StringManager.GetString(this.GetType().FullName,labelCamera.Text);
            //calibrationTypeGrid.Text = StringManager.GetString(this.GetType().FullName,calibrationTypeGrid.Text);
            //calibrationTypeChessboard.Text = StringManager.GetString(this.GetType().FullName,calibrationTypeChessboard.Text);
            //labelNumRow.Text = StringManager.GetString(this.GetType().FullName,labelNumRow.Text);
            //labelRowSpace.Text = StringManager.GetString(this.GetType().FullName,labelRowSpace.Text);
            //labelNumCol.Text = StringManager.GetString(this.GetType().FullName,labelNumCol.Text);
            //labelColSpace.Text = StringManager.GetString(this.GetType().FullName,labelColSpace.Text);
            //buttonGrab.Text = StringManager.GetString(this.GetType().FullName,buttonGrab.Text);
            //buttonCalibrate.Text = StringManager.GetString(this.GetType().FullName,buttonCalibrate.Text);
            //buttonSaveCalibration.Text = StringManager.GetString(this.GetType().FullName,buttonSaveCalibration.Text);
            //buttonLoadCalibration.Text = StringManager.GetString(this.GetType().FullName,buttonLoadCalibration.Text);
        }

        private void InitChartMenu()
        {
         
        }

        private void CameraCalibrationForm_Load(object sender, EventArgs e)
        {
            // Init Camera Combo
            InitCameraMenu();

            // Init Lights
            InitLightMenu();

            // Init Chart
            InitChartMenu();

            // init Calibrate Menu
            UpdateCalibrateMenu();


        }

        private void UpdateCalibrateMenu()
        {
            if (curCameraCalibration == null)
                return;
                  

            if (curCameraCalibration.CalibrationType == CalibrationType.SingleScale)
            {
                tabControlCalibrationType.SelectedIndex = 0;
            }
            else if (curCameraCalibration.CalibrationType == CalibrationType.Grid)
            {
                tabControlCalibrationType.SelectedIndex = 1;
            }
            else if (curCameraCalibration.CalibrationType == CalibrationType.Ruler)
            {
                tabControlCalibrationType.SelectedIndex = 2;
            }

            this.curCameraCalibrationPanel = (CameraCalibrationPanel)tabControlCalibrationType.SelectedTab.Controls[0];
            //curCameraCalibrationPanel.ChangeLanguage();
            curCameraCalibrationPanel.UpdateData(curCameraCalibration);
            return;
        }

        private void InitLightMenu()
        {
            int numLightType = MachineSettings.Instance().NumLightType;
            if (numLightType == 0)
            {
                ChangeLightValue();
                radioDeviceLightType.Enabled = false;
            }
            else
            {
                ChangeLightType();
            }
        }

        private void radioDeviceLight_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioDeviceLightType.Checked)
                ChangeLightType();

            if (radioDeviceLightValue.Checked)
                ChangeLightValue();
        }
        
        private void ChangeLightValue()
        {
            radioDeviceLightValue.Checked = true;
            comboLightType.Items.Clear();

            for (int i = 0; i < lightCtrlHandler.Count; i++)
            {
                LightCtrl lightCtrl = lightCtrlHandler.GetLightCtrl(i);
                string contorllorName = lightCtrl.Name;
                if (string.IsNullOrEmpty(contorllorName))
                    contorllorName = string.Format("Ctrl. {0}", i);

                for (int j = 0; j < lightCtrl.NumChannel; j++)
                {
                    string channelName = string.Format("Ch. {0}", j);
                    string text = string.Format("{0} - {1}", contorllorName, channelName);
                    comboLightType.Items.Add(new ComboBoxItem(text, i, j));
                }
            }

            if (comboLightType.Items.Count > 0)
                comboLightType.SelectedIndex = 0;
        }

        private void ChangeLightType()
        {
            radioDeviceLightType.Checked = true;
            comboLightType.Items.Clear();

            for (int i = 0; i < this.lightParamSet.LightParamList.Count; i++)
            {
                string name = lightParamSet.LightParamList[i].Name;
                if (string.IsNullOrEmpty(name))
                    name = string.Format("Type {0}", i);

                comboLightType.Items.Add(new ComboBoxItem(name, lightParamSet.LightParamList[i]));
            }

            if (comboLightType.Items.Count > 0)
                comboLightType.SelectedIndex = 0;
        }

        private void InitCameraMenu()
        {
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                Camera camera = imageDevice as Camera;
                if (camera != null)
                {
                    string name = camera.Name;
                    if (string.IsNullOrEmpty(name))
                        name = String.Format("Camera {0}", camera.Index);

                    comboBoxCamera.Items.Add(name);

                    SetCalibrationGrab(camera);
                }
            }

            if (comboBoxCamera.Items.Count > 0)
                comboBoxCamera.SelectedIndex = 0;
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
                return;

            Camera camera = imageDeviceHandler.GetImageDevice(comboBoxCamera.SelectedIndex) as Camera;
            if (camera == null)
                return;

            if (camera.IsOnLive() == true)
                return;

            Grab(camera);
        }

        private void buttonGrabMulti_Click(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
                return;

            Camera camera = imageDeviceHandler.GetImageDevice(comboBoxCamera.SelectedIndex) as Camera;

            if (camera == null)
                return;

            if (camera.IsOnLive() == false)
            {
                StartGrabMulti(camera);
            }
            else
            {
                StopGrab(camera);                
            }
        }

        private void Grab(Camera camera)
        {
            LockButtons();

            camera.ImageGrabbed += ImageGrabbed;
            imageGrabbedDone.Reset();

            camera.SetTriggerMode(TriggerMode.Software, TriggerType.RisingEdge);

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn();
            Thread.Sleep(10);

            camera.GrabOnce();
            camera.WaitGrabDone();
            new SimpleProgressForm("Processing").Show(() => imageGrabbedDone.WaitOne(5000));

            camera.Stop();
            camera.ImageGrabbed -= ImageGrabbed;

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            ReleaseButtons();
        }

        private void StartGrabMulti(Camera camera)
        {
            camera.SetTriggerMode(TriggerMode.Software);

            LockButtons();
            buttonGrabMulti.Text = "STOP";
            buttonGrabMulti.Enabled = true;

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn();
            this.drawBox.DisplayRect = RectangleF.Empty;
            camera.ImageGrabbed += ImageGrabbed;
            camera.GrabMulti();
        }

        private void StopGrab(Camera camera)
        {
            camera.ImageGrabbed -= ImageGrabbed;
            camera.Stop();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            if (asyncProcessTask != null && (asyncProcessTask.IsCompleted == false))
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    asyncProcessTask.Wait();
                });
            }

            buttonGrabMulti.Text = "Grab Multi";
            ReleaseButtons();
        }

        private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (asyncProcessTask == null || asyncProcessTask.IsCompleted)
            {
                asyncProcessTask?.Dispose();
                asyncProcessTask = new Task(new Action(() =>
                {
                    try
                    {
                        LogHelper.Debug(LoggerType.Function, "CameraCalibrationForm::asyncProcessTask Start");

                        // Get Grabbed Image
                        Image2D grabbedImage = (Image2D)imageDevice.GetGrabbedImage(ptr);
                        if (grabbedImage.IsUseIntPtr())
                        {
                            Image2D image2DD = (Image2D)grabbedImage.Clone();
                            image2DD.ConvertFromPtr();
                            grabbedImage = image2DD;
                        }
                        //grabbedImage.SaveImage(@"D:\temp\tt.bmp");

                        if (imageDevice is CameraVirtual)
                        {
                            grabbedImage = (Image2D)grabbedImage.ClipImage(new System.Drawing.Rectangle(0, 0, grabbedImage.Width, 256));
                        }
                        // Draw Chart
                        //draw_Chart_ms(image2D);

                        // Calibration Func
                        this.lastCalibrationResult = CalibrateFunc(grabbedImage);

                        //// Draw Image and Overlay Figure
                        //if (this.lastCalibrationResult != null && this.lastCalibrationResult.image != null)
                        //{
                        //    Image2D binalImage = this.lastCalibrationResult.binalImage;
                        //    image2D.CopyFrom(binalImage, new System.Drawing.Rectangle(System.Drawing.Point.Empty, binalImage.Size), binalImage.Pitch, this.lastCalibrationResult.calibRect.Location);
                        //}

                        //this.drawBox.FigureGroup.Clear();
                        //this.lastCalibrationResult.AppendFigure(this.drawBox.FigureGroup);

                        //Image image = image2D.ToBitmap();
                        //this.drawBox.UpdateImage(image);
                        //image.Dispose();


                        UpdateResult(grabbedImage);
                        imageGrabbedDone.Set();

                        LogHelper.Debug(LoggerType.Function, "CameraCalibrationForm::asyncProcessTask End");
                    }
                    finally
                    {
                        Application.DoEvents();
                        Thread.Sleep(50);
                    }
                }));
                asyncProcessTask.Start();
            }
        }

        private CalibrationResult CalibrateFunc(Image2D image2D)
        {
            CalibrationResult result = curCameraCalibrationPanel.Calibrate(curCameraCalibration, image2D);
            //switch (curCameraCalibration.CalibrationType)
            //{
            //    case CalibrationType.SingleScale:
            
            //        break;
            //    case CalibrationType.Grid:
            //        
            //        break;
            //    case CalibrationType.Ruler:

            //        break;
            //}

            return result;
        }

        private void ReleaseButtons()
        {
            UpdateBottons(true);

            int numLightType = MachineSettings.Instance().NumLightType;
            if (numLightType == 0)
                radioDeviceLightType.Enabled = false;
        }

        private void UpdateBottons(bool v)
        {
            comboBoxCamera.Enabled = v;
            numericUpDownExternalExpose.Enabled = v;
            radioDeviceLightType.Enabled = v;
            radioDeviceLightValue.Enabled = v;
            comboLightType.Enabled = v;
            trackBar1.Enabled = v;
            buttonGrab.Enabled = v;
            buttonGrabMulti.Enabled = v;
            buttonSave.Enabled = v;
            buttonLoad.Enabled = v;
        }

        private void LockButtons()
        {
            UpdateBottons(false);
        }

        private void TurnOnLight()
        {
            if (lightCtrlHandler == null)
                return;

            lightCtrlHandler.TurnOn(this.lightParam);
            Thread.Sleep(100);
        }

        private void TurnOffLight()
        {
            if (lightCtrlHandler == null)
                return;

            lightCtrlHandler.TurnOff();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            Camera camera = imageDeviceHandler.GetImageDevice(comboBoxCamera.SelectedIndex) as Camera;
            if (camera == null)
                return;

            Image2D grabbedImage = camera.GetGrabbedImage(IntPtr.Zero) as Image2D;
            if (grabbedImage != null && grabbedImage.IsUseIntPtr())
            {
                Image2D image2DD = (Image2D)grabbedImage.Clone();
                image2DD.ConvertFromPtr();
                grabbedImage = image2DD;
            }

            if (grabbedImage!=null && camera is CameraVirtual)
            {
                grabbedImage = (Image2D)grabbedImage.ClipImage(new System.Drawing.Rectangle(0, 0, grabbedImage.Width, Math.Min(grabbedImage.Height, 256)));
            }

            if (curCameraCalibration == null)
                return;

            this.lastCalibrationResult = curCameraCalibrationPanel.Calibrate(curCameraCalibration, grabbedImage);
            UpdateResult(grabbedImage);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (Apply())
                MessageForm.Show(null, "Calibrate OK");
            else
                MessageForm.Show(null, "Calibrate Fail");
        }

        private bool Apply()
        {
            if (curCameraCalibration == null)
                return false;

            if (comboBoxCamera.SelectedIndex < 0)
                return false;

            if (lastCalibrationResult == null)
                return false;

            curCameraCalibration.PelSize = lastCalibrationResult.pelSize;
            return true;
        }


        private void buttonSaveCalibration_Click(object sender, EventArgs e)
        {
            if (Save())
                MessageForm.Show(null, "Save OK");
            else
                MessageForm.Show(null, "Save Fail");
        }

        private bool Save()
        {
            if (curCameraCalibration == null)
                return false;

            if (comboBoxCamera.SelectedIndex < 0)
                return false;

            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(comboBoxCamera.SelectedIndex);
            camera.UpdateFovSize(curCameraCalibration.PelSize);

            curCameraCalibration.Save();
            return true;
        }

        private void buttonLoadCalibration_Click(object sender, EventArgs e)
        {
            if (curCameraCalibration != null)
            {
                curCameraCalibration.Load();
                UpdateCalibrateMenu();
            }
        }

        private void comboBoxCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraSelectedChanged(comboBoxCamera.SelectedIndex);

            label10.Text = curCameraCalibration.PelSize.Width.ToString("0.000");
            label3.Text = curCameraCalibration.PelSize.Height.ToString("0.000");
        }

        private void CameraSelectedChanged(int selIndex)
        {
            if (selIndex < 0)
                return;

            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(selIndex);

            float exposureTimeMs = camera.GetDeviceExposureMs();
            bool enable = !float.IsNaN(exposureTimeMs) && (exposureTimeMs >= 0);
            numericUpDownExternalExpose.Enabled = enable;
            if (enable)
                numericUpDownExternalExpose.Value = (decimal)Math.Max(0, exposureTimeMs);

            Calibration cameraCalibration = cameraCalibrationList.Find(f => f.CameraIndex == camera.Index);
            if (cameraCalibration == null)
            {
                return;
            }
            curCameraCalibration = cameraCalibration;


            UpdateCalibrateMenu();
        }

        private void SetCalibrationGrab(Camera camera)
        {
            /*
            if (camera is CameraGenTL)
            {
                CameraGenTL cameraGenTL = (CameraGenTL)camera;
                cameraGenTL.SetAreaMode();
                cameraGenTL.SetDeviceExposure((float)numericUpDownExternalExpose.Value);
            }*/


            //camera.SetScanMode(ScanMode.Area);
            camera.SetExposureTime((float)numericUpDownExternalExpose.Value * 1000);
        }

        private void ResetCalibrationGrab(Camera camera)
        {
            /*
            if (camera is CameraGenTL)
            {
                CameraGenTL cameraGenTL = (CameraGenTL)camera;
                cameraGenTL.SetLineScanMode();
                //cameraGenTL.SetDeviceExposure((float)numericUpDownExternalExpose.Value);
            }*/
            //camera.SetScanMode(ScanMode.Line);
        }

        private void draw_Histogram(ImageD image)//ms
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraCalibrationForm::draw_Histogram"));

            Image2D image2D = image as Image2D;
            AlgoImage fullImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);

            //1. Revision Histogram
            long[] histo = imageProcessing.Histogram(fullImage);
            RevisionHistogram(histo);


            //2. Revision MTF-Chart 
            int range = 100;
            int count = fullImage.Width / 2 - range / 2;
            float[] fullMFT = new float[fullImage.Width];
            byte[] linedata = new byte[fullImage.Width];
            float[] left = new float[count];
            float[] right = new float[count];
            byte[] temp = new byte[range];

            byte[] imageData = fullImage.GetByte();
            int offset = fullImage.Height / 2 * fullImage.Width;
            for (int i = 0; i < fullImage.Width; i++)
            {
                linedata[i] = imageData[offset + i];
            }

            Array.Clear(fullMFT, 0, fullMFT.Length);
            for (int i = range / 2; i < fullImage.Width - range / 2; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    int index = i - range / 2 + j;
                    temp[j] = linedata[index];
                }
                fullMFT[i] = getMTF(temp);
            }
            //left
            for (int i = 0; i < count; i++)
            {
                left[i] = fullMFT[fullImage.Width / 2 - i - 1];
            }
            //right
            for (int i = 0; i < count; i++)
            {
                right[i] = fullMFT[fullImage.Width / 2 + i];
            }

            RevisionMTF(left, right);

            fullImage.Dispose();
        }

        private float getMTF(byte[] data)
        {
            float fmax = data.Max();
            float fmin = data.Min();
            if (fmax == 0) return 0;
            return (fmax - fmin) / (fmax + fmin);
        }

        private delegate void RevisionHistogramDelegate(long[] histo);
        private void RevisionHistogram(long[] histo)
        {
            if (chart_Histogram.InvokeRequired)
            {
                BeginInvoke(new RevisionHistogramDelegate(RevisionHistogram), histo);
                return;
            }

            chart_Histogram.Series[0].Points.Clear();//logarithm은 0또는 음수를 허용하지 않음
            chart_Histogram.Series[1].Points.Clear();
            foreach (var data in histo)
            {
                chart_Histogram.Series[0].Points.Add(data + 1);//logarithm은 0또는 음수를 허용하지 않음
                chart_Histogram.Series[1].Points.Add(data);
            }
        }

        private delegate void RevisionMTFDelegate(float[] left, float[] right);
        private void RevisionMTF(float[] left, float[] right)
        {
            if (chart_MTF.InvokeRequired)
            {
                BeginInvoke(new RevisionMTFDelegate(RevisionMTF), left, right);
                return;
            }
            chart_MTF.Series[0].Points.Clear();
            chart_MTF.Series[1].Points.Clear();
            foreach (var data in left)
            {
                chart_MTF.Series[0].Points.Add(data);
            }
            foreach (var data in right)
            {
                chart_MTF.Series[1].Points.Add(data);
            }
        }
        
        private void draw_Chart_ms(ImageD imageD)//ms
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraCalibrationForm::draw_Chart_ms"));
            //Image2D image2D = Image2D.ToImage2D(drawBox.Image);
            Image2D image2D = imageD as Image2D;

            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(Calibration.TypeName);
            AlgoImage fullImage = ImageBuilder.Build(algorithmStrategy.LibraryType, image2D, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algorithmStrategy.LibraryType);

            //1. Revision Histogram
            long[] histo = imageProcessing.Histogram(fullImage);
            RevisionHistogram(histo);

            //2. Revision MTF-Chart 
            int range = 100;
            int count = fullImage.Width / 2 - range / 2;
            float[] fullMFT = new float[fullImage.Width];
            byte[] linedata = new byte[fullImage.Width];
            float[] left = new float[count];
            float[] right = new float[count];
            byte[] temp = new byte[range];

            byte[] imageData = fullImage.GetByte();
            int offset = fullImage.Height / 2 * fullImage.Width;
            for (int i = 0; i < fullImage.Width; i++)
            {
                linedata[i] = imageData[offset + i];
            }

            Array.Clear(fullMFT, 0, fullMFT.Length);
            for (int i = range / 2; i < fullImage.Width - range / 2; i++)
            {
                for (int j = 0; j < range; j++)
                {
                    int index = i - range / 2 + j;
                    temp[j] = linedata[index];
                }
                fullMFT[i] = getMTF(temp);
            }
            //left
            for (int i = 0; i < count; i++)
            {
                left[i] = fullMFT[fullImage.Width / 2 - i - 1];
            }
            //right
            for (int i = 0; i < count; i++)
            {
                right[i] = fullMFT[fullImage.Width / 2 + i];
            }

            RevisionMTF(left, right);

            fullImage.Dispose();
        }

        // 흔적... 일단 살려주세요
        //private AlgoImage CreateMaskImage(Size referenceSize, System.Drawing.Rectangle measureReagion)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(OperationSettings.Instance().ImagingLibrary);
        //    AlgoImage maskImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, measureReagion.Width, measureReagion.Height);
        //    imageProcessing.Clear(maskImage, 255);
        //    foreach (PointF skip in this.propRulerSkipRegionList)
        //    {
        //        int ss = (int)Math.Round(referenceSize.Width * skip.X);
        //        int ee = (int)Math.Round(referenceSize.Width * skip.Y);
        //        System.Drawing.Rectangle skipRect = System.Drawing.Rectangle.FromLTRB(ss, measureReagion.Top, ee, measureReagion.Bottom);
        //        skipRect.Intersect(measureReagion);
        //        if (skipRect.Width <= 0)
        //            continue;
        //        skipRect.Offset(-measureReagion.Location.X, -measureReagion.Location.Y);
        //        imageProcessing.Clear(maskImage, skipRect, Color.Black);
        //    }

        //    return maskImage;
        //}
        
        private void UpdateResult(ImageD grabbedImage)
        {
            UpdateResult(grabbedImage, this.lastCalibrationResult);
            UpdateChart(grabbedImage);

            curCameraCalibrationPanel.UpdateResult(this.lastCalibrationResult, -1);

            for (int i = 0; i < this.lastCalibrationResult.subCalibrationResult.Count; i++)
            {
                CalibrationResult calibrationResult = this.lastCalibrationResult.subCalibrationResult[i];
                curCameraCalibrationPanel.UpdateResult(calibrationResult, i);
            }

        }

        private void UpdateResult(ImageD grabbedImage, CalibrationResult result)
        {
            // Draw Image and Overlay Figure
            if (grabbedImage != null)
            {
                Image2D image2D = (Image2D)grabbedImage.Clone();
                if (this.lastCalibrationResult != null && this.lastCalibrationResult.clipImage != null)
                {
                    Image2D binalImage = this.lastCalibrationResult.binalImage;
                    try
                    {
                        image2D.CopyFrom(binalImage, new System.Drawing.Rectangle(System.Drawing.Point.Empty, binalImage.Size), binalImage.Pitch, this.lastCalibrationResult.calibRect.Location);
                    }
                    catch (Exception)
                    {

                    }
                }
                Image image = image2D.ToBitmap();
                this.drawBox.UpdateImage(image);
                image.Dispose();
                image2D.Dispose();
            }
            this.drawBox.FigureGroup.Clear();
            this.lastCalibrationResult.AppendFigure(this.drawBox.FigureGroup);

            Label[] labelList = new Label[] { label7, label8, label11, label10, label3, label52 };
            SetLabelText(labelList[0], result.avgBrightness < 0 ? "---.-" : result.avgBrightness.ToString("0.0"));
            SetLabelText(labelList[1], result.minBrightness < 0 ? "---.-" : result.minBrightness.ToString("0.0"));
            SetLabelText(labelList[2], result.maxBrightness < 0 ? "---.-" : result.maxBrightness.ToString("0.0"));
            SetLabelText(labelList[3], result.pelSize.Width < 0 ? "---.---" : result.pelSize.Width.ToString("0.000"));
            SetLabelText(labelList[4], result.pelSize.Height < 0 ? "---.---" : result.pelSize.Height.ToString("0.000"));
            SetLabelText(labelList[5], result.focusValue < 0 ? "---.---" : result.focusValue.ToString("0.000"));
        }

        private delegate void SetRLabelTextDelegate(Label label, string message);
        private void SetLabelText(Label label, string message)
        {
            if (InvokeRequired)
            {
                Invoke(new SetRLabelTextDelegate(SetLabelText), label, message);
                return;
            }

            label.Text = message;
        }

        private void UpdateChart(ImageD grabbedImage)
        {
            if (grabbedImage != null)
                draw_Chart_ms(grabbedImage);

            // Histogram

            // MTF
        }

        private void CameraCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
                return;

            int cameraIndex = Convert.ToInt32(comboBoxCamera.SelectedIndex);
            Camera camera = imageDeviceHandler.GetImageDevice(cameraIndex) as Camera;

            if (camera == null)
                return;

            if (camera.IsOnLive() == true)
            {
                camera.Stop();
                camera.ImageGrabbed -= ImageGrabbed;
            }

            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() => ResetCalibrationGrab(camera));
        }

        private void numericUpDownExternalExpose_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxCamera.SelectedIndex < 0)
                return;

            int cameraIndex = Convert.ToInt32(comboBoxCamera.SelectedIndex);

            Camera camera = imageDeviceHandler.GetImageDevice(cameraIndex) as Camera;
            camera.SetExposureTime((float)numericUpDownExternalExpose.Value*1000);
            
        }

        private void tabControlCalibrationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (curCameraCalibration == null)
                return;

            this.curCameraCalibrationPanel = (CameraCalibrationPanel)tabControlCalibrationType.SelectedTab.Controls[0];
            if (tabControlCalibrationType.SelectedIndex == 0)
            {
                curCameraCalibration.CalibrationType = CalibrationType.SingleScale;
            }
            else if (tabControlCalibrationType.SelectedIndex == 1)
            {
                curCameraCalibration.CalibrationType = CalibrationType.Grid;
            }
            else if (tabControlCalibrationType.SelectedIndex == 2)
            {
                curCameraCalibration.CalibrationType = CalibrationType.Ruler;
            }
        }

        private void comboLightType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = comboLightType.SelectedIndex;
            if (idx < 0)
                return;

            ComboBoxItem item = (ComboBoxItem)comboLightType.SelectedItem;
            if (radioDeviceLightType.Checked)
            {
                this.lightParam = item.lightParam;
            }
            else
            {
                this.lightParam = this.lightParamSet.LightParamList[item.ctrlId];
                int value = this.lightParam.LightValue.Value[item.ctrlCh];

                int maxValue = this.lightCtrlHandler.GetLightCtrl(item.ctrlId).GetMaxLightLevel();

                trackBar1.Value = (int)Math.Round(value * 100.0f / maxValue);
                label13.Text = trackBar1.Value.ToString() + "%";

            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (radioDeviceLightType.Checked)
                return;

            int selIdx = comboLightType.SelectedIndex;
            if (selIdx < 0)
                return;

            ComboBoxItem item = (ComboBoxItem)comboLightType.SelectedItem;
            int maxValue = this.lightCtrlHandler.GetLightCtrl(item.ctrlId).GetMaxLightLevel();
            this.lightParam.LightValue.Value[item.ctrlCh] = (int)Math.Round(trackBar1.Value / 100f * maxValue);
            label13.Text = trackBar1.Value.ToString() + "%";
        }
        
        //private void UpdateValue()
        //{
        //    this.propRulerWidth = (float)propertyWidth.Value / 100f;
        //    this.propRulerHeight = (float)propertyHeight.Value / 100f;
        //    this.propRulerScale = (float)propertyScale.Value;
        //    this.propRulerReagionNum = (int)regionNum.Value;
        //    this.propRulerThreshold = (int)propertyThreshold.Value;
        //    this.propRulerPartial = (float)propertyPartial.Value / 100f;
        //}

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (drawBox.Image == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".bmp";
            dlg.FileName = string.Format("Calibration_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "Bitmap(BMP)|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    drawBox.Image.Save(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageForm.Show(null, ex.Message);
                }
            }
        }

        private void imageContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            drawBox.ZoomFit();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "bmp";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            Bitmap bitmap = new Bitmap(dlg.FileName);
            Image2D image2D = Image2D.FromBitmap(bitmap);

            this.lastCalibrationResult = CalibrateFunc(image2D);

            this.drawBox.UpdateImage(bitmap);
            this.drawBox.ZoomFit();
            if (this.lastCalibrationResult != null)
                this.lastCalibrationResult.AppendFigure(this.drawBox.FigureGroup);
        }

        private void buttonLightOn_Click(object sender, EventArgs e)
        {
            TurnOnLight();
        }

        private void buttonLightOff_Click(object sender, EventArgs e)
        {
            TurnOffLight();
        }

        private void CameraCalibrationForm_Resize(object sender, EventArgs e)
        {
            this.drawBox?.ZoomFit();
        }
    }
}

