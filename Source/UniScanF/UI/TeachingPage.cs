using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;
using UniEye.Base;
using DynMvp.Base;
using UniScan.Data;

using System.Diagnostics;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;

using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices;
using DynMvp.Data.UI;
using DynMvp.UI;
using System.Threading;
using DynMvp.UI.Touch;
using System.IO;
using System.Drawing.Imaging;
using UniEye.Base.Settings;

namespace UniScan.UI
{
    public enum MotionPosition
    {
        StartPosition, EndPosition, ReferencePosition, BackGroundPosiotion
    }

    public partial class TeachingPage : UserControl
    {
        float[] dustSizeList = new float[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 300 };

        DrawBox originBox;
        FigureGroup figureGroup = new FigureGroup();
        FigureGroup backgroundFigureGroup = new FigureGroup();

        List<float> dustPosList = new List<float>();
        DustPosCount[] dustPosCountList;

        List<Image2D> grabbedImageList = new List<Image2D>();

        CancellationTokenSource autoRunCancelTokenSource;

        Bitmap bitMap;

        Task autoRunTask;
        private bool started = false;

        public TeachingPage()
        {
            InitializeComponent();

            this.SuspendLayout();

            this.originBox = new DrawBox();

            this.originBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.originBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originBox.Name = "originBox";
            this.originBox.TabIndex = 0;
            this.originBox.TabStop = false;

            imageSplitContainer.Panel2.Controls.Add(this.originBox);

            this.ResumeLayout();

            //#if SHEET_DUST == false
            //            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            //            camera.ImageGrabbed += webCam_ImageGrabbed;
            //#endif
        }

        delegate void CameraImageGrabbedDelegte(ImageDevice imageDevice);

        private void webCam_ImageGrabbed(ImageDevice imageDevice)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new CameraImageGrabbedDelegte(webCam_ImageGrabbed), imageDevice);
                return;
            }

            if (originBox.Image != null)
                originBox.Image.Dispose();

            originBox.UpdateImage(imageDevice.GetGrabbedImage(IntPtr.Zero).ToBitmap());
        }

        private void tdi_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            Image2D grabbedImage = (Image2D)imageDevice.GetGrabbedImage(ptr);
            grabbedImageList.Add(grabbedImage);
        }

        delegate void ImageGrabDoneDelegte();
        private void ImageGrabDone()
        {
            if (InvokeRequired)
            {
                Invoke(new ImageGrabDoneDelegte(ImageGrabDone));
                return;
            }

            if (originBox.Image != null)
                originBox.Image.Dispose();

            if (grabbedImageList.Count == 0)
                return;

            int width = grabbedImageList.Min(f => f.Width);
            int height = grabbedImageList.Sum(f => f.Height);

            Image2D image2D = new Image2D(width, height, 1);
            Rectangle subRect = new Rectangle(0, 0, width, grabbedImageList[0].Height);
            foreach (Image2D grabbedImage in grabbedImageList)
            {
                grabbedImage.ConvertFromDataPtr();
                image2D.CopyFrom(grabbedImage, new Rectangle(0, 0, grabbedImage.Width, grabbedImage.Height), grabbedImage.Pitch, subRect.Location);
                subRect.Offset(0, grabbedImage.Height);
            }

            string fileName = ImageBuffer.GetImage2dFileName(0, 0, 0,ImageFormat.Bmp);
            string filePath = SystemManager.Instance().CurrentModel.GetImagePath();
            if (Directory.Exists(filePath) == false)
                Directory.CreateDirectory(filePath);
            //image2D.SaveImage(Path.Combine(filePath, fileName), ImageFormat.Bmp);

            originBox.UpdateImage(image2D.ToBitmap());

            grabbedImageList.Clear();
        }

        private void TeachingPage_VisibleChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

        }

        class DustPosCount
        {
            int[] countArr;
            public int[] CountArr
            {
                get { return countArr; }
                set { countArr = value; }
            }

            public DustPosCount(int numSize)
            {
                countArr = new int[numSize];
            }
        }

        delegate void SetupUiDelegte();
        void SetupUi()
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new SetupUiDelegte(SetupUi));
            //    return;
            //}
            dustList.Rows.Clear();

            sectionComboBox.Items.Clear();
            sectionComboBox.Items.Add("All");

            // Summary Table
            summaryTable.Columns.Clear();
            summaryTable.Columns.Add("Size", "Size");
            summaryTable.Columns.Add("Sum", "Sum");

            // 위치 그래프 초기화
            List<Series> seriesList = new List<Series>();
            if (dustPosChart.Series.Count > 0)
            {
                seriesList.AddRange(dustPosChart.Series);

                foreach (Series series in seriesList)
                {
                    dustPosChart.Series.Remove(series);
                }
            }

            dustPosChart.Series.Clear();
            dustPosChart.ResetAutoValues();

            float width = originBox.Image.Width;

            float scanStart = Math.Max((float)nudSectionStart.Value, 0);
            float scanEnd = Math.Min((float)nudSectionEnd.Value, width - 1);

            if(comboLRselector.SelectedIndex == 1)
            {
                scanStart = (float)nudSectionStart.Value;
            }
            

            if (nudSectionStart.Value != 0 || nudSectionEnd.Value != 0)
            {
                width = (float)(nudSectionEnd.Value - nudSectionStart.Value);
            }
                

            float step = width / (float)nudNumSection.Value;

            dustPosChart.ChartAreas[0].AxisX.Maximum = (int)nudNumSection.Value;

            dustPosList.Clear();
            dustPosList.Add(scanStart);

            dustPosCountList = new DustPosCount[(int)nudNumSection.Value];
            for (int i = 0; i < nudNumSection.Value; i++)
            {
                dustPosList.Add((i + 1) * step + scanStart);
                dustPosCountList[i] = new DustPosCount(dustSizeList.Length);
                sectionComboBox.Items.Add(i.ToString());
            }

            // 크기 그래프 초기화
            if (dustSizeChart.Series.Count > 0)
            {
                seriesList.Clear();
                seriesList.AddRange(dustSizeChart.Series);

                foreach (Series series in seriesList)
                {
                    dustSizeChart.Series.Remove(series);
                }
            }

            dustSizeChart.Series.Clear();
            dustSizeChart.ResetAutoValues();
        } 

        public void AddBlobRect(AlgoImage grayImage, AlgoImage binImage, PointF offset, BlobRectList blobRectList, bool lastSection = false)
        {
            float lastSectionLeft = dustPosList[dustPosList.Count - 2];
            float sectionWidth = dustPosList[1] - dustPosList[0];
            if (comboLRselector.SelectedIndex == 1)
            {
                lastSectionLeft = dustPosList[0];
                sectionWidth = dustPosList[1] - dustPosList[0];

            }

            RectangleF lastSectionRect = new RectangleF(lastSectionLeft, 0, sectionWidth, grayImage.Height);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectSigmaValue = true;
            blobParam.SelectMaxValue = true;
            blobParam.SelectMinValue = true;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grayImage);
            float resolution = (float)nudResolution.Value;

            BlobRectList subBlobRectList = imageProcessing.Blob(binImage,  blobParam);
            foreach (BlobRect subBlobRect in subBlobRectList)
            {
                // 경계 Section 일 경우, 영역을 세부 분리해서 이물을 검출한다.
                if ((lastSection == true || lastSectionRect.Contains(DrawingHelper.CenterPoint(subBlobRect.BoundingRect)) == true) && subBlobRect.Area > 50)
                {
                    // 이물 후보 영역을 저장할 목록
                    BlobRectList subBlobRectList2 = new BlobRectList();

                    // 블랍내의 밝기 변화가 주어진 변화보다 클 경우, 중간 밝기값으로 영역을 다시 분할한다.
                    if ((subBlobRect.MaxValue - subBlobRect.MinValue) > (int)nudMinSizeThreshold.Value)
                    {
                        Rectangle rect = new Rectangle((int)subBlobRect.BoundingRect.X, (int)subBlobRect.BoundingRect.Y,
                            (int)subBlobRect.BoundingRect.Width, (int)subBlobRect.BoundingRect.Height);

                        AlgoImage grayClip = grayImage.Clip(rect);
                        AlgoImage binClip = grayClip.Clone();

                        // 새로운  Threshold 값
                        int threshold = (int)(subBlobRect.MaxValue + subBlobRect.MinValue) / 2;
                        imageProcessing.Binarize(grayClip, binClip, threshold, checkBoxDetectWhite.Checked == false);

                        //binClip.Save("binClip.bmp", new DebugContext(true, PathSettings.Instance().Temp));

                        AddBlobRect(grayClip, binClip, new PointF(rect.X, rect.Y), subBlobRectList2, true);
                    }
                    else
                    {
                        subBlobRectList2.Append(subBlobRect);
                    }

                    // 이물 후보 영역에 대해 이물 분류를 다시 한다.
                    BlobRectList finalBlobRectList = new BlobRectList();

                    foreach (BlobRect subBlobRect2 in subBlobRectList2)
                    {
                        // 크기가 큰 블랍일 경우, 세로 방향 Projection을 구하고,
                        // 세로 방향으로 Scan 하면서 폭 변화가 큰 부분을 추출하고, 이를 이물 영역으로 판정한다.
                        if (subBlobRect2.BoundingRect.Height > (int)nudLongHeight.Value)
                        {
                            Rectangle rect2 = new Rectangle((int)subBlobRect2.BoundingRect.X, (int)subBlobRect2.BoundingRect.Y,
                                (int)subBlobRect2.BoundingRect.Width, (int)subBlobRect2.BoundingRect.Height);

                            AlgoImage grayClip2 = grayImage.Clip(rect2);

                            float[] proj = imageProcessing.Projection(grayClip2, grayClip2, DynMvp.Vision.Direction.Vertical, ProjectionType.Mean);
                            float avgWidth = proj.Average();
                            float maxWidth = proj.Max();

                            if ((maxWidth - avgWidth) > 1)
                            {
                                int startIndex = -1;

                                // 폭 계산
                                for (int i = 0; i < proj.Length; i++)
                                {
                                    if (startIndex == -1)
                                    {
                                        if ((proj[i] - avgWidth) > 2)
                                        {
                                            startIndex = i;
                                        }
                                    }
                                    else if (startIndex != -1)
                                    {
                                        if ((proj[i] - avgWidth) <= 0)
                                        {
                                            BlobRect blobRect = new BlobRect();
                                            blobRect.BoundingRect = new RectangleF(rect2.X + offset.X, rect2.Y + offset.Y + startIndex, rect2.Width, i - startIndex);
                                            blobRect.Area = blobRect.BoundingRect.Width * blobRect.BoundingRect.Height;
                                            blobRect.MaxValue = subBlobRect2.MaxValue;
                                            finalBlobRectList.Append(blobRect);
                                            startIndex = -1;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 크기가 작을 경우, 이물 불량으로 판정
                            RectangleF rectangle = subBlobRect2.BoundingRect;
                            rectangle.Offset(offset.X, offset.Y);
                            subBlobRect2.BoundingRect = rectangle;

                            finalBlobRectList.Append(subBlobRect2);
                        }
                    }

                    foreach (BlobRect blobRect in finalBlobRectList)
                        blobRectList.Append(blobRect);
                }
                else
                {
                    // 크기가 작고 밝기 변화가 작을 경우, 이물 판정에서 제외한다.
                    if (subBlobRect.Area < (int)nudMinSize.Value && ((subBlobRect.MaxValue - (int)nudThreshold.Value) < (int)nudMinSizeThreshold.Value))
                        continue;

                    RectangleF rectangle = subBlobRect.BoundingRect;
                    rectangle.Offset(offset.X, offset.Y);
                    subBlobRect.BoundingRect = rectangle;

                    blobRectList.Append(subBlobRect);
                }
            }
        }

        public void SplitBlob(AlgoImage grayImage, BlobRect blobRect, BlobRectList blobRectList)
        {
        }

        public void UpdateChart()
        {
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            CameraGenTL cameraGenTL = camera as CameraGenTL;
            if (cameraGenTL == null)
            {
                camera.GrabOnce();
            }
            else
            {
                GrabTdi(cameraGenTL);
            }

        }

        private void GrabTdi(CameraGenTL cameraGenTL)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            AxisPosition scanStartPos = axisHandler.CreatePosition();
            AxisPosition scanEndPos = axisHandler.CreatePosition();
            scanStartPos[0] = (float)nudScanStart.Value;
            scanEndPos[0] = (float)nudScanEnd.Value;

            axisHandler.AxisList[0].AxisParam.MovingParam.MaxVelocity = 50 * 1000;
            axisHandler.AxisList[0].AxisParam.MovingParam.StartVelocity = 50 * 1000 / 2;
            axisHandler.Move(scanStartPos);

            double spd = (double)nudMotionSpeed.Value * 1000;
            double cy = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)(spd / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height);

            //if (cameraGenTL.SetFreeMode(grabHz) == false)
            //    return;

            cameraGenTL.ImageGrabbed += tdi_ImageGrabbed;
            cameraGenTL.GrabMulti(-1);

            axisHandler.AxisList[0].AxisParam.MovingParam.MaxVelocity = (float)spd;
            axisHandler.AxisList[0].AxisParam.MovingParam.StartVelocity = (float)spd / 2;
            axisHandler.Move(scanEndPos);

            cameraGenTL.Stop();
            cameraGenTL.ImageGrabbed -= tdi_ImageGrabbed;

            ImageGrabDone();
        }

        private void GrabTdi2(CameraGenTL cameraGenTL)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return;            

            AxisPosition scanStartPos = axisHandler.CreatePosition();
            AxisPosition scanEndPos = axisHandler.CreatePosition();
            scanStartPos[0] = (float)nudScanStart.Value;
            scanEndPos[0] = (float)nudScanEnd.Value;

            axisHandler.AxisList[0].AxisParam.MovingParam.MaxVelocity = 50 * 1000;
            axisHandler.AxisList[0].AxisParam.MovingParam.StartVelocity = 50 * 1000 / 2;
            axisHandler.Move(scanStartPos);

            double spd = (double)nudMotionSpeed.Value * 1000;
            double cy = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float grabHz = (float)(spd / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height);

            //if (cameraGenTL.SetFreeMode(grabHz) == false)
            //    return;

            cameraGenTL.ImageGrabbed += tdi_ImageGrabbed;
            cameraGenTL.GrabMulti(-1);

            axisHandler.AxisList[0].AxisParam.MovingParam.MaxVelocity = (float)spd;
            axisHandler.AxisList[0].AxisParam.MovingParam.StartVelocity = (float)spd / 2;
            axisHandler.Move(scanEndPos);

            cameraGenTL.Stop();
            cameraGenTL.ImageGrabbed -= tdi_ImageGrabbed;

            ImageGrabDone();
        }

        private void dustList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            backgroundFigureGroup = new FigureGroup();
            BlobRect blobRect = (BlobRect)dustList.Rows[e.RowIndex].Tag;

            PointF centerPt = DrawingHelper.CenterPoint(blobRect.BoundingRect);
            figureGroup.AddFigure(new LineFigure(new PointF(0, centerPt.Y), new PointF(originBox.Image.Width, centerPt.Y), new Pen(Color.Yellow, 1)));
            figureGroup.AddFigure(new LineFigure(new PointF(centerPt.X, 0), new PointF(centerPt.X, originBox.Image.Height), new Pen(Color.Yellow, 1)));
            figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Green, 2)));

            originBox.BackgroundFigures = backgroundFigureGroup;
            originBox.Invalidate(true);
        }

        private void moveHome_Click(object sender, EventArgs e)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler != null)
            {
                axisHandler.AxisList[0].AxisParam.HomeSpeed.HighSpeed.MaxVelocity = 40000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.HighSpeed.StartVelocity = 20000;

                axisHandler.AxisList[0].AxisParam.HomeSpeed.MediumSpeed.MaxVelocity = 20000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.MediumSpeed.StartVelocity = 10000;

                axisHandler.AxisList[0].AxisParam.HomeSpeed.FineSpeed.MaxVelocity = 10000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.FineSpeed.StartVelocity = 5000;

                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(new Action(() => axisHandler.HomeMove(cancellationTokenSource)), cancellationTokenSource);
            }
        }

        
        private void TeachingPage_Load(object sender, EventArgs e)
        {
            //AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            //if (axisHandler != null)
            //{
            //    nudMotionSpeed.Value = (decimal)axisHandler.AxisList[0].AxisParam.MovingParam.MaxVelocity/1000;
            //}
            comboLRselector.SelectedIndex = 0;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                originBox.UpdateImage((Bitmap)ImageHelper.LoadImage(dlg.FileName));
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (originBox.Image == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ImageHelper.SaveImage(originBox.Image, dlg.FileName);
            }
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string rawDataFileName = Path.Combine(dlg.SelectedPath, "raw_data.csv");
                string summaryDataFileName = Path.Combine(dlg.SelectedPath, "summary_data.csv");
                string positionChartFileName = Path.Combine(dlg.SelectedPath, "position_chart.bmp");
                string sizeChartFileName = Path.Combine(dlg.SelectedPath, "size_chart.bmp");
                string resultImageFileName = Path.Combine(dlg.SelectedPath, "result_image.bmp");

                UiHelper.ExportCsv(dustList, rawDataFileName);
                UiHelper.ExportCsv(summaryTable, summaryDataFileName);
                dustPosChart.SaveImage(positionChartFileName, ChartImageFormat.Bmp);
                dustSizeChart.SaveImage(sizeChartFileName, ChartImageFormat.Bmp);
                originBox.SaveImage(resultImageFileName);
            }
        }

        private void toolStripZoomInButton_Click(object sender, EventArgs e)
        {
            originBox.ZoomIn();
        }

        private void toolStripZoomOutButton_Click(object sender, EventArgs e)
        {
            originBox.ZoomOut();
        }

        private void toolStripZoomFitButton_Click(object sender, EventArgs e)
        {
            originBox.ZoomFit();
        }

        private void sectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = sectionComboBox.SelectedIndex;

            List<Series> seriesList = new List<Series>();

            if (dustSizeChart.Series.Count > 0)
            {
                seriesList.Clear();
                seriesList.AddRange(dustSizeChart.Series);

                foreach (Series series in seriesList)
                {
                    dustSizeChart.Series.Remove(series);
                }
            }

            Series seriesDust2 = new Series();

            seriesDust2.ChartType = SeriesChartType.Column;
            seriesDust2.ChartArea = "ChartArea";

            if (index == 0) //all
            {
                float[] dustSizeList = new float[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 300 };

                int[] sizeCountSum = new int[dustSizeList.Length];

                for (int idxpos = 0; idxpos < dustPosCountList.Length; idxpos++)
                {
                    for (int i = 0; i < dustSizeList.Length; i++)
                    {
                        int count = dustPosCountList[idxpos].CountArr[i];

                        sizeCountSum[i] += count;
                    }
                }

                for (int idxSize = 0; idxSize < dustSizeList.Length; idxSize++)
                {
                    int ptIndex = seriesDust2.Points.AddXY(idxSize, sizeCountSum[idxSize]);
                    seriesDust2.Points[ptIndex].AxisLabel = dustSizeList[idxSize].ToString() + "um";
                }

                dustSizeChart.Series.Add(seriesDust2);

            }
            else //selected section
            {
                float[] dustSizeList = new float[] { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 300 };

                int[] sizeCountSum = new int[dustSizeList.Length];

                for (int i = 0; i < dustSizeList.Length; i++)
                {
                    int count = dustPosCountList[index - 1].CountArr[i];

                    sizeCountSum[i] = count;
                }


                for (int idxSize = 0; idxSize < dustSizeList.Length; idxSize++)
                {
                    int ptIndex = seriesDust2.Points.AddXY(idxSize, sizeCountSum[idxSize]);
                    seriesDust2.Points[ptIndex].AxisLabel = dustSizeList[idxSize].ToString() + "um";
                }

                dustSizeChart.Series.Add(seriesDust2);
            }
        }

        private void checkBoxShowResult_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowResult.Checked == true)
            {
                originBox.FigureGroup = figureGroup;
                originBox.BackgroundFigures = backgroundFigureGroup;
            }
            else
            {
                originBox.FigureGroup = new FigureGroup();
                originBox.BackgroundFigures = new FigureGroup();
            }

            originBox.Invalidate(true);
        }

        private void toolStripInspection_Click(object sender, EventArgs e)
        {
            InspectDust();
        }

        private void toolStripGrab_Click(object sender, EventArgs e)
        {
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            CameraGenTL cameraGenTL = camera as CameraGenTL;
            if (cameraGenTL == null)
            {
                camera.GrabOnce();
            }
            else
            {
                GrabTdi(cameraGenTL);
            }
        }

        private void toolStripLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                originBox.UpdateImage((Bitmap)ImageHelper.LoadImage(dlg.FileName));

                bitMap = (Bitmap)originBox.Image.Clone();
            }
        }

        private void toolStripSaveImage_Click(object sender, EventArgs e)
        {
            if (originBox.Image == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ImageHelper.SaveImage(originBox.Image, dlg.FileName);
            }
        }

        private void pictureStart_Click(object sender, EventArgs e)
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (started)
            {
                started = false;
                autoRunCancelTokenSource.Cancel();
                buttonStart.Image = Properties.Resources.Start;
                buttonStart.Text = StringManager.GetString(this.GetType().FullName, "Start");
                EnableControls(true);
            }
            else
            {
                started = true;
                EnableControls(false);
                AutoRun();
                buttonStart.Image = Properties.Resources.Stop;
                buttonStart.Text = StringManager.GetString(this.GetType().FullName, "Stop");
            }
        }
        private void AutoRun()
        {
            autoRunCancelTokenSource = new CancellationTokenSource();
            autoRunTask = new Task(new Action(AutoRunProc), autoRunCancelTokenSource.Token);
            autoRunTask.Start();
        }
        private void AutoRunProc()
        {
            while(started)
            {
                if (autoRunCancelTokenSource.IsCancellationRequested)
                {
                    return;  
                }
                HomeMove();
                GrabProcess();
                HomeMove();
                InspectDust();
                if (started == false)
                    return;
            }
        }


        private void HomeMove()
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler != null)
            {
                axisHandler.AxisList[0].AxisParam.HomeSpeed.HighSpeed.MaxVelocity = 40000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.HighSpeed.StartVelocity = 20000;

                axisHandler.AxisList[0].AxisParam.HomeSpeed.MediumSpeed.MaxVelocity = 20000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.MediumSpeed.StartVelocity = 10000;

                axisHandler.AxisList[0].AxisParam.HomeSpeed.FineSpeed.MaxVelocity = 10000;
                axisHandler.AxisList[0].AxisParam.HomeSpeed.FineSpeed.StartVelocity = 5000;

                axisHandler.HomeMove(autoRunCancelTokenSource);
            }
        }

        private void GrabProcess()
        {
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            CameraGenTL cameraGenTL = camera as CameraGenTL;
            if (cameraGenTL == null)
            {
                camera.GrabOnce();
            }
            else
            {
                GrabTdi2(cameraGenTL);
            }
        }

        delegate void InspectDustDelegte();
        private void InspectDust()
        {
            if (InvokeRequired)
            {
                Invoke(new InspectDustDelegte(InspectDust));
                return;
            }
            LogHelper.Debug(LoggerType.Inspection, "Start Insepction.");
            if (originBox.Image == null)
            {
                LogHelper.Debug(LoggerType.Inspection, "originBox.Image null return");
                return;
            }
            try
            {
                SetupUi();
            }
            catch(Exception e)
            {
                LogHelper.Debug(LoggerType.Inspection, "Setup UI Fail");
                return;
            }
                       

            ImageD image = Image2D.FromBitmap((Bitmap)originBox.Image);
            AlgoImage grayImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey, ImageBandType.Luminance);
            AlgoImage binImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey, ImageBandType.Luminance);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grayImage);
            for(int i = 0; i < 3; i++)
                imageProcessing.Average(grayImage, grayImage);
            

            int threshold = (int)nudThreshold.Value;

            if (threshold == 0)
                threshold = (int)imageProcessing.Binarize(grayImage);

            imageProcessing.Binarize(grayImage, binImage, (int)nudThreshold.Value, checkBoxDetectWhite.Checked == false);

            //binImage.Save("binImage.bmp", new DebugContext(true, PathSettings.Instance().Temp)); // 이미지 저장에서 1.5 초 이상 잡아 먹음

            BlobRectList blobRectList = new BlobRectList();
            AddBlobRect(grayImage, binImage, new PointF(0, 0), blobRectList);

            float resolution = (float)nudResolution.Value;

            figureGroup.Clear();

            int index = 1;
            foreach (BlobRect blobRect in blobRectList)
            {
                RectangleF rectangle = blobRect.BoundingRect;
                PointF centerPt = DrawingHelper.CenterPoint(rectangle);

                int rowIndex = dustList.Rows.Add(index++, rectangle.X * resolution, rectangle.Y * resolution,
                            rectangle.Width * resolution, rectangle.Height * resolution, blobRect.Area * resolution * resolution,
                            rectangle.Width, rectangle.Height, blobRect.MaxValue);
                dustList.Rows[rowIndex].Tag = blobRect;

                figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Red)));

                float maxLength = Math.Max(blobRect.BoundingRect.Width, blobRect.BoundingRect.Height) * resolution;

                for (int i = 0; i < dustPosList.Count - 1; i++)
                {
                    if ((dustPosList[i] <= centerPt.X) && (centerPt.X < dustPosList[i + 1]))
                    {
                        DustPosCount dustPosCount = dustPosCountList[i];

                        for (int idx = 0; idx < dustSizeList.Length; idx++)
                        {
                            if (idx == (dustSizeList.Length - 1))
                            {
                                if (dustSizeList[idx] <= maxLength)
                                {
                                    dustPosCount.CountArr[idx]++;
                                    break;
                                }
                            }
                            else
                            {
                                if ((dustSizeList[idx] <= maxLength) && (maxLength < dustSizeList[idx + 1]))
                                {
                                    dustPosCount.CountArr[idx]++;
                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }

            float sectionWidth = dustPosList[1] - dustPosList[0];

            for (int idxPos = 0; idxPos < dustPosList.Count - 1; idxPos++)
            {
                string title = idxPos.ToString();
                summaryTable.Columns.Add(title, title);

                if (checkBoxShowSection.Checked)
                {
                    RectangleF sectionRect = new RectangleF(dustPosList[idxPos], 0, sectionWidth, image.Height);
                    figureGroup.AddFigure(new RectangleFigure(sectionRect, new Pen(Color.LightCyan)));
                }
            }

            for (int idxSize = 0; idxSize < dustSizeList.Length; idxSize++)
            {
                Series seriesDust = new Series(dustSizeList[idxSize].ToString());

                seriesDust.ChartType = SeriesChartType.StackedColumn;
                seriesDust.ChartArea = "ChartArea";

                dustPosChart.Series.Add(seriesDust);

                summaryTable.Rows.Add(dustSizeList[idxSize].ToString() + "um");
            }

            int sumRowIndex = summaryTable.Rows.Add("Sum");

            if (dustPosChart.Legends.Count > 0)
                dustPosChart.Legends.Clear();

            dustPosChart.Legends.Add("Main");

            int[] sizeCountSum = new int[dustSizeList.Length];

            for (int idxPos = 0; idxPos < dustPosCountList.Length; idxPos++)
            {
                for (int idxSize = 0; idxSize < dustSizeList.Length; idxSize++)
                {
                    int count = dustPosCountList[idxPos].CountArr[idxSize];
                    int ptIndex = dustPosChart.Series[idxSize].Points.AddXY(idxPos, count);
                    dustPosChart.Series[idxSize].Points[ptIndex].AxisLabel = idxPos.ToString();
                    if (count > 0)
                    {
                        dustPosChart.Series[idxSize].Points[ptIndex].Label = dustSizeList[idxSize].ToString();
                    }

                    sizeCountSum[idxSize] += count;

                    summaryTable.Rows[idxSize].Cells[idxPos + 2].Value = count;
                }

                summaryTable.Rows[sumRowIndex].Cells[idxPos + 2].Value = dustPosCountList[idxPos].CountArr.Sum();
            }
            summaryTable.Rows[sumRowIndex].Cells[1].Value = sizeCountSum.Sum();

            Series seriesDust2 = new Series();

            seriesDust2.ChartType = SeriesChartType.Column;
            seriesDust2.ChartArea = "ChartArea";

            for (int idxSize = 0; idxSize < dustSizeList.Length; idxSize++)
            {
                int ptIndex = seriesDust2.Points.AddXY(idxSize, sizeCountSum[idxSize]);
                summaryTable.Rows[idxSize].Cells[1].Value = sizeCountSum[idxSize];
                seriesDust2.Points[ptIndex].AxisLabel = dustSizeList[idxSize].ToString() + "um";
            }

            dustSizeChart.Series.Add(seriesDust2);
            sectionComboBox.SelectedIndex = 0;

            originBox.FigureGroup = figureGroup;
            originBox.Invalidate(true);

            if (nudThreshold.Value == 0)
                nudThreshold.Value = threshold;

            LogHelper.Debug(LoggerType.Inspection, "Finish Insepction."); 
        }

        private void EnableControls(bool enable)
        {
            toolStripInspection.Enabled = enable;
            toolStripGrab.Enabled = enable;
            toolStripLoadImage.Enabled = enable;
            toolStripSaveImage.Enabled = enable;
        }
    }
}