using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UniEye.Base.Settings;

namespace SimpleCameraCalibration
{
    public partial class MainForm : Form
    {
        Grabber grabber;
        Camera camera;
        ConcurrentQueue<ImageD> queue;
        ThreadHandler threadHandler;
        CanvasPanel canvasPanel;

        DateTime dateTime = DateTime.MinValue;
        bool showBinImage = false;

        public MainForm()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.DoubleClick += new EventHandler((s, e) => ((CanvasPanel)s).ZoomFit());
            this.canvasPanel.SetPanMode();
            this.panel1.Controls.Add(canvasPanel);

            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;

            this.queue = new ConcurrentQueue<ImageD>();
            this.threadHandler = new ThreadHandler("ThreadProc", new Thread(ThreadProc));

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                MachineSettings machineSettings = MachineSettings.Instance();

                GrabberInfo grabberInfo = machineSettings.GrabberInfoList.FirstOrDefault();
                if (grabberInfo == null)
                    throw new Exception("Grabber is not Set.");
                this.grabber = GrabberFactory.Create(grabberInfo);

                string camConfig = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabberInfo.Name);
                CameraConfiguration cameraConfiguration = new CameraConfiguration(1);
                cameraConfiguration.LoadCameraConfiguration(camConfig);
                if (cameraConfiguration.CameraInfos.Length == 0)
                    throw new Exception("Camera is not Set.");

                this.camera = grabber.CreateCamera(cameraConfiguration.CameraInfos[0]);
                this.camera.Initialize(true);
                this.camera.ImageGrabbed += Camera_ImageGrabbed;

                this.threadHandler.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Close();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.camera?.Stop();

            this.threadHandler?.Stop();
            this.camera?.Release();
            this.grabber?.Release();
        }

        private void Camera_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            CameraBufferTag tag = (CameraBufferTag)imageD.Tag;
            this.queue.Enqueue(imageD);
        }

        private void ThreadProc()
        {
            AlgoImage buffer = null;
            while(!this.threadHandler.RequestStop)
            {
                ImageD imageD;
                while (this.queue.TryDequeue(out imageD))
                {
                    if (this.queue.Count > 0)
                        continue;

                    CameraBufferTag tag = (CameraBufferTag)imageD.Tag;

                    byte binValue;
                    double focus = 0;
                    Tuple<double, double> sDeg = new Tuple<double, double>(double.NaN, double.NaN);
                    List<PointF> pointList;

                    using (AlgoImage algoImage = OpenCvImageBuilder.Build(ImagingLibrary.OpenCv, imageD, DynMvp.Vision.ImageType.Grey))
                    {
                        if (buffer == null || buffer.Size != algoImage.Size)
                        {
                            buffer?.Dispose();
                            buffer = OpenCvImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
                        }
                        buffer.Clear();
                        focus = Vision.GetFocus(algoImage, buffer);

                        sDeg = Vision.GetSlope(algoImage, buffer, out pointList, out binValue);
                    }

                    ImageD imageD2 = buffer.ToImageD();
                    UpdateData(imageD, imageD2, focus, sDeg, binValue, pointList);
                    
                }
                Thread.Sleep(100);
            }
            buffer?.Dispose();
        }

        private delegate void UpdateDataDelegate(ImageD imageD, ImageD imageD2, double focus, Tuple<double, double> sDeg, byte binValue, List<PointF> slplePointList);
        private void UpdateData(ImageD imageD, ImageD imageD2, double focus, Tuple<double, double> sDeg, byte binValue, List<PointF> slplePointList)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), imageD, imageD2, focus, sDeg, binValue, slplePointList);
                return;
            }

            UpdateImage(imageD, imageD2, sDeg, slplePointList);

            if (!this.checkBox1.Checked)
            {
                this.trackBar1.Value = binValue;
                this.numericUpDown1.Value = binValue;
            }

            AddChartData(new DataPoint((DateTime.Now - this.dateTime).TotalSeconds, focus));

            UiHelper.SetControlText(this.valueFocus, focus.ToString("F2"));
            UiHelper.SetControlText(this.valueSlope, MathHelper.RadToDeg(sDeg.Item1).ToString("F2"));
        }

        private void AddChartData(DataPoint dataPoint)
        {
            this.chart1.Series[0].Points.Add(dataPoint);

            int xMin = ((int)Math.Floor(this.chart1.Series[0].Points.Min(f => f.XValue)) + 4) / 5 * 5;
            int xMax = ((int)Math.Ceiling(this.chart1.Series[0].Points.Max(f => f.XValue))+4)/5 * 5;
            int xInt = (int)Math.Round(this.chart1.ChartAreas[0].AxisX.Interval)*12;
            if (xMax - xMin < xInt)
            {
                this.chart1.ChartAreas[0].AxisX.Minimum = 0;
                this.chart1.ChartAreas[0].AxisX.Maximum = xInt;
            }
            else
            {
                this.chart1.ChartAreas[0].AxisX.Maximum = xMax;
                this.chart1.ChartAreas[0].AxisX.Minimum = xMax - xInt;
            }
            this.chart1.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;

        }

        private void UpdateImage(ImageD imageD, ImageD imageD2, Tuple<double, double> slope, List<PointF> slplePointList)
        {
            bool zoom = (this.canvasPanel.Image == null);

            if (showBinImage)
            this.canvasPanel.UpdateImage(imageD2.ToBitmap());
            else
            this.canvasPanel.UpdateImage(imageD.ToBitmap());

            this.canvasPanel.WorkingFigures.Clear();

            if (!double.IsNaN(slope.Item1))
            {
                double s = slope.Item1;
                //PointF ptC = new PointF(imageD.Width / 2, imageD.Height / 2);
                //PointF ptL = new PointF(0, (float)(ptC.Y + (s * imageD.Width / 2)));
                //PointF PtR = new PointF(imageD.Width, (float)(ptC.Y - (s * imageD.Width / 2)));

                PointF ptL = new PointF(0, (float)slope.Item2);
                PointF PtR = new PointF(imageD.Width, (float)(slope.Item1 * imageD.Width + slope.Item2));

                RotatedRect rotatedRect = new RotatedRect(new Rectangle(Point.Empty, imageD.Size), 0);
                ptL = DrawingHelper.ClipToFov(rotatedRect, ptL);
                PtR = DrawingHelper.ClipToFov(rotatedRect, PtR);

                this.canvasPanel.WorkingFigures.AddFigure(new LineFigure(ptL, PtR, new Pen(Color.Red, 2)));

            //if (showBinImage)
                slplePointList.ForEach(f => this.canvasPanel.WorkingFigures.AddFigure(new RectangleFigure(DrawingHelper.FromCenterSize(f, new Size(10, 10)), new Pen(Color.Red, 2))));
            }

            if (zoom)
            {
                this.canvasPanel.ZoomFit();
            }
            else
            {
                this.canvasPanel.Invalidate(true);
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.chart1.Series[0].Points.Clear();
            this.dateTime = DateTime.Now;
            //this.camera.GrabOnce();
            this.camera.GrabMulti();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.camera.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Vision.UseManualThreshold = ((CheckBox)sender).Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.numericUpDown1.Value = ((TrackBar)sender).Value;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Vision.ManualThresholdValue = (byte)((NumericUpDown)sender).Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.showBinImage = ((CheckBox)sender).Checked;
        }
    }

}
