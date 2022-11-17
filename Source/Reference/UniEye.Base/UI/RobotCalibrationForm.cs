using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Device;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Device.UI;
using UniEye.Base.Device;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class RobotCalibrationForm : Form
    {
        DrawBox cameraBox;
        DrawBox robotMapBox;
        TeachBox teachBox;
        RobotAligner robotAligner = new RobotAligner();
        public RobotAligner RobotAligner
        {
            get { return robotAligner; }
            set { robotAligner = value; }
        }

        Thread workingThread;
        // Calibration cameraCalibrarion;

        bool stopFlag = false;
        bool isCenterFounded = false;
        bool onCalibrating = false;

        BlobChecker blobChecker;
        BlobCheckerParam blobCheckerParam;
        ImageD image;

        public RobotCalibrationForm()
        {
            InitializeComponent();

            cameraBox = new DrawBox();
            robotMapBox = new DrawBox();
            teachBox = new TeachBox();
            
            txtXStep.Text = Length.GetExternalValue(20 * 1000).ToString();         
        }

        public void Initialize()
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);

            this.cameraBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraBox.Location = new System.Drawing.Point(3, 3);
            this.cameraBox.Name = "targetImage";
            this.cameraBox.Size = new System.Drawing.Size(409, 523);
            this.cameraBox.TabIndex = 8;
            this.cameraBox.TabStop = false;
            this.cameraBox.Enable = false;
            this.cameraBox.MeasureMode = true;
            tabPageCamera.Controls.Add(cameraBox);

            cameraBox.Enable = false;
            cameraBox.ShowCenterGuide = true;
            cameraBox.SetImageDevice(camera);
            cameraBox.LockLiveUpdate = false;
            cameraBox.Calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];
            blobChecker = new BlobChecker();
            blobCheckerParam = new BlobCheckerParam();
            UpdateFovRect();
            //camera.GrabMulti();

            robotAligner = robotStage.RobotAligner;
            robotStage.OnEndMove += robotStage_RobotMoved;

            UpdateGrid();

            Joystick2AxisControl joystick = new Joystick2AxisControl();
            joystick.Initialize(robotStage);
            joystickPanel.Controls.Add(joystick);
        }

        private void robotStage_RobotMoved(AxisPosition axisPosition)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new RobotEventDelegate(robotStage_RobotMoved), axisPosition);
                return;
            }

            if (onCalibrating == false)
                Grab();
        }

        private void UpdateGrid()
        {
            if (robotAligner.RefPoints == null)
                return;

            for (int y = 0; y < robotAligner.RefPoints.GetLength(1);y++)
            {
                for (int x = 0; x < robotAligner.RefPoints.GetLength(0); x++)
                {
                    float len = (float) Math.Sqrt(Math.Pow(robotAligner.Offset[x, y].X, 2) + Math.Pow(robotAligner.Offset[x, y].Y, 2));
                    AddDataGrid(string.Format("P{0}-{1}", x, y),
                        string.Format("{0},{1}", robotAligner.RefPoints[x, y].X, robotAligner.RefPoints[x, y].Y),
                        string.Format("{0},{1}", robotAligner.RealPoints[x, y].X, robotAligner.RealPoints[x, y].Y),
                        robotAligner.Offset[x, y].X.ToString(), robotAligner.Offset[x, y].Y.ToString(),
                       string.Format("{0}", Math.Sqrt(Math.Pow(robotAligner.Offset[x, y].X, 2) + Math.Pow(robotAligner.Offset[x, y].X, 2))));
                }
            }
        }

        private void UpdateFovRect()
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            
            AxisPosition axisPosition = robotStage.GetActualPos();
            cameraBox.DisplayRect = DrawingHelper.FromCenterSize(axisPosition.ToPointF(), camera.FovSize);

            RectangleF circleRect = DrawingHelper.FromCenterSize(axisPosition.ToPointF(), new SizeF((float)circleDiameter.Value, (float)circleDiameter.Value));

            FigureGroup tempFigureGroup = new FigureGroup();
            tempFigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Red)));
            cameraBox.FigureGroup = tempFigureGroup;

            cameraBox.Invalidate();
        }

        private void CalibrationProc()
        {
            onCalibrating = true;

            LogHelper.Debug(LoggerType.Operation, "Start CalibrationProc.");

            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;

            Axis xAxis = robotStage.GetAxis("X-Axis");
            Axis yAxis = robotStage.GetAxis("Y-Axis");

            float xStepUm = Length.GetInternalValue(Convert.ToSingle(txtXStep.Text));
            float yStepUm = Length.GetInternalValue(Convert.ToSingle(txtYStep.Text));

            int countX = Convert.ToInt32(numPosX.Value);
            int countY = Convert.ToInt32(numPosY.Value);
            PointF[,] refPoints = new PointF[countX, countY];
            PointF[,] offsets = new PointF[countX, countY];

            float yPos =Convert.ToSingle(txtStartY.Text);
            float xPos =Convert.ToSingle(txtStartX.Text);
            PointF startPoint = new PointF(xPos, yPos);

            robotAligner.Clear();
            for (int y = 0; y < countY ; y++)
            {
                if (stopFlag)
                    break;

                for (int x = 0; x < countX; x++)
                {
                    if (stopFlag)
                        break;

                    int realX = x;
                    isCenterFounded = false;
                    //if ((y % 2) == 1)
                    //{
                    //    realX = countX - x - 1;
                    //}

                    if(realX == 0 && y == 0)
                    {
                        while (true)
                        {
                            robotStage.Move(new AxisPosition(new float[] { xPos, yPos }));
                            robotStage.WaitMoveDone();

                            AxisPosition axisPosition = robotStage.GetCommandPos();
                            Grab();

                            AxisPosition foundOffset = FindEdgeCenter(image);

                            xPos = axisPosition.Position[0] - foundOffset.Position[0];
                            yPos = axisPosition.Position[1] + foundOffset.Position[1];

                            if ((foundOffset[0] < 5 && foundOffset[0] > -5) && (foundOffset[1] < 5 && foundOffset[1] > -5))
                                break;

                            if (stopFlag)
                                return;
                        }

                        AxisPosition alignedPosition = robotStage.GetCommandPos();
                        startPoint = refPoints[0, 0] = new PointF(alignedPosition[0], alignedPosition[1]);
                        AddDataGrid(string.Format("P{0}-{1}", realX, y), string.Format("{0},{1}", startPoint.X, startPoint.Y)
                            , string.Format("{0},{1}", startPoint.X, startPoint.Y)
                            , "0", "0", "0");
                    }
                    else
                    {
                        xPos = startPoint.X + xStepUm * (-realX);
                        yPos = startPoint.Y + yStepUm * (-y);
                        AxisPosition axisPosition = new AxisPosition(new float[] { xPos, yPos });
                        robotStage.Move(axisPosition);

                        Thread.Sleep(300);

                        Grab();

                        //image.SaveImage("D:\\RobotCalib.bmp", ImageFormat.Bmp);

                        AxisPosition foundOffset = FindEdgeCenter(image);
                        
                        PointF offset = new PointF(-foundOffset.Position[0],foundOffset.Position[1]);
                        offsets[realX, y] = offset;
                        refPoints[realX, y] = new PointF(xPos, yPos);
                        AddDataGrid(string.Format("P{0}-{1}", realX, y), string.Format("{0},{1}", xPos, yPos),
                                string.Format("{0},{1}", xPos + offset.X, yPos + offset.Y), offset.X.ToString(), offset.Y.ToString(),
                                string.Format("{0}", Math.Sqrt(Math.Pow(offset.X,2)+ Math.Pow(offset.Y,2))));
                    }
                }
            }

            // 임의 정지시 저장 안 함
            if (!stopFlag)
            {
                robotAligner.Calculate(refPoints, offsets);
                robotAligner.Save(PathSettings.Instance().Config);
            }

            onCalibrating = false;
        }

        private delegate void AddDataGridDelegate(string pointIdx, string refPos, string realPos, string offsetX, string offsetY, string length);
        private void AddDataGrid(string pointIdx, string refPos, string realPos, string offsetX, string offsetY, string length)
        {
            if (InvokeRequired)
            {
                Invoke(new AddDataGridDelegate(AddDataGrid), pointIdx, refPos, realPos, offsetX, offsetY, length);
                return;
            }

            int idx = dataGridView1.Rows.Add(pointIdx, refPos, realPos, offsetX, offsetY, length);
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
        }

        private AxisPosition FindEdgeCenter(ImageD image)
        {
            AxisPosition position = new AxisPosition();
            Image2D grabImage = (Image2D)image; //Image2D.ToImage2D(image);
            AlgoImage clipImage = ImageBuilder.Build(BlobChecker.TypeName, grabImage, ImageType.Grey, ImageBandType.Luminance);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);

            //imageProcessing.Binarize(clipImage);
            imageProcessing.Binarize(clipImage, clipImage, 100);
            imageProcessing.Not(clipImage, clipImage);
            
            BlobParam blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            //blobParam.SelectBoundingRect = false;

            BlobRectList blobRectList = imageProcessing.Blob(clipImage, blobParam);

            BlobRect blobRect = blobRectList.GetMaxAreaBlob();
            //PointF imageCenterPoint = new PointF(imageCenterPoint)
            //float x = DrawingHelper.CenterPoint(grabImage.Roi).X + blobRect.CenterPt.X;
            //float y = DrawingHelper.CenterPoint(grabImage.Roi).Y + blobRect.CenterPt.Y;

            float x = (grabImage.Width / 2) - blobRect.CenterPt.X;
            float y = (grabImage.Height / 2) - blobRect.CenterPt.Y;

          PointF worldPos=  cameraBox.Calibration.PixelToWorld(new PointF(x, y));
            position.Position[0] = worldPos.X;
            position.Position[1] = worldPos.Y;
            clipImage.Dispose();
            return position;
        }

        private void UpdateMapFigure()
        {

        }

        private void Inspect()
        {
            //ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(cameraIndex);

            //teachBox.Inspect(deviceImageSet, false, null, null, null);

            //UpdateImageFigure();

            //InspectionResult lastSelectedResult = null;

            //if (lastInspectionResult != null)
            //{
            //    lastSelectedResult = new InspectionResult();
            //    foreach (ITeachObject teachObject in teachHandlerProbe.SelectedObjs)
            //    {
            //        Probe probe = teachObject as Probe;
            //        if (probe != null)
            //            lastSelectedResult.AddProbeResult(lastInspectionResult.GetProbeResult(probe));
            //    }
            //}

            //tryInspectionResultView.AddResult(teachBox.InspectionResultSelected, lastSelectedResult);
        }


        private void FindCircle()
        {

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (onCalibrating)
            {
                stopFlag = true;
                return;
            }

            if (MessageBox.Show("the old data will be removed. continue?", "UniEye", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                return;

            stopFlag = false;

            dataGridView1.Rows.Clear();

            workingThread = new Thread(new ThreadStart(CalibrationProc));
            workingThread.IsBackground = true;
            workingThread.Start();
        }

        private void UpdateImageFigure()
        {
            teachBox.UpdateFigure();
        }

        private void RobotCalibrationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (workingThread != null)
            {
                workingThread.Abort();
                workingThread = null;
            }

            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            robotStage.OnEndMove -= robotStage_RobotMoved;

            this.Close();
        }

        private void buttonTestGrab_Click(object sender, EventArgs e)
        {
            if (onCalibrating)
            {
                stopFlag = true;
                return;
            }
             
            stopFlag = false;

            dataGridView1.Rows.Clear();

            workingThread = new Thread(new ThreadStart(TestProc));
            workingThread.IsBackground = true;
            workingThread.Start();
        }

        private void TestProc()
        {
            onCalibrating = true;

            LogHelper.Debug(LoggerType.Operation, "Start Test.");

            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            Axis xAxis = robotStage.GetAxis("X-Axis");
            Axis yAxis = robotStage.GetAxis("Y-Axis");

            float xStepUm = Length.GetInternalValue(Convert.ToSingle(txtXStep.Text));
            float yStepUm = Length.GetInternalValue(Convert.ToSingle(txtYStep.Text));

            int countX = Convert.ToInt32(numPosX.Value);
            int countY = Convert.ToInt32(numPosY.Value);

            float yPos = Convert.ToSingle(txtStartY.Text);
            float xPos = Convert.ToSingle(txtStartX.Text);
            PointF startPoint = new PointF(xPos, yPos);

            for (int y = 0; y < countY; y++)
            {
                if (stopFlag)
                    break;

                for (int x = 0; x < countX; x++)
                {
                    if (stopFlag)
                        break;

                    int realX = x;
                    isCenterFounded = false;
                    //if ((y % 2) == 1)
                    //{
                    //    realX = countX - x - 1;
                    //}

                    if (realX == 0 && y == 0)
                    {
                        while (true)
                        {
                            robotStage.Move(new AxisPosition(new float[] { xPos, yPos }));
                            robotStage.WaitMoveDone();

                            AxisPosition axisPosition = robotStage.GetCommandPos();
                            Grab();

                            AxisPosition foundOffset = FindEdgeCenter(image);

                            xPos = xPos - foundOffset.Position[0];
                            yPos = yPos + foundOffset.Position[1];

                            if ((foundOffset[0] < 10 && foundOffset[0] > -10) && (foundOffset[1] < 10 && foundOffset[1] > -10))
                                break;

                            if (stopFlag)
                                return;
                        }

                        AxisPosition alignedPosition = robotStage.GetCommandPos();
                        startPoint = new PointF(alignedPosition[0], alignedPosition[1]);
                        AddDataGrid(string.Format("P{0}-{1}", realX, y),
                            string.Format("{0},{1}", startPoint.X, startPoint.Y),
                            string.Format("{0},{1}", startPoint.X, startPoint.Y), "0", "0", "0");
                    }
                    else
                    {
                        xPos = startPoint.X + xStepUm * (-realX);
                        yPos = startPoint.Y + yStepUm * (-y);
                        AxisPosition axisPosition = new AxisPosition(new float[] { xPos, yPos });
                        robotStage.Move(axisPosition);

                        Thread.Sleep(500);

                        Grab();

                        AxisPosition foundOffset = FindEdgeCenter(image);

                        PointF offset = new PointF(foundOffset.Position[0], foundOffset.Position[1]);
                        AddDataGrid(string.Format("P{0}-{1}", realX, y),
                            string.Format("{0},{1}", xPos, yPos), 
                            string.Format("{0},{1}", xPos + offset.X, yPos + offset.Y),
                            offset.X.ToString(), offset.Y.ToString(), string.Format("{0}", Math.Sqrt(Math.Pow(offset.X, 2) + Math.Pow(offset.Y, 2))));
                    }
                }
            }

            onCalibrating = false;
        }

        public void Grab()
        {
            //AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            Camera camera = (Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();

            imageAcquisition.AcquireCalibation(0, 0);

            image = camera.GetGrabbedImage(IntPtr.Zero);

            PointF centerPt = new PointF(image.Width / 2, image.Height / 2);
            RectangleF circleRect = DrawingHelper.FromCenterSize(centerPt, new SizeF((float)1000, (float)1000));

            FigureGroup tempFigureGroup = new FigureGroup();
            tempFigureGroup.AddFigure(new EllipseFigure(circleRect, new Pen(Color.Red)));
            cameraBox.FigureGroup = tempFigureGroup;

            cameraBox.ZoomFit();

            //imageAcquisition.AcquireCalibation(0, 0, machine.LightCtrlHandler);
            //ImageD image = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(0, 0).Image;
            //camera.UpdateImage(image);

        }

        private void applyLightButton_Click(object sender, EventArgs e)
        {
            Grab();
        }

        private void buttonOrigin_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            robotStage.HomeMove();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            AxisPosition act = robotStage.GetActualPos();
            AxisPosition cmd = robotStage.GetCommandPos();            
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (onCalibrating)
                return;

            if (dataGridView1.SelectedCells.Count == 0)
                return;

            int rowId = dataGridView1.SelectedCells[0].RowIndex;
            int colId = dataGridView1.SelectedCells[0].ColumnIndex;
            string refPositionStr = (string)dataGridView1.Rows[rowId].Cells[1].Value;
            string[] split = refPositionStr.Split(new char[] { ',' });
            PointF refPosition = new PointF(float.Parse(split[0]), float.Parse(split[1]));

            // 아래를 살리면 Align을 두 번 함...
            //if (colId == 2)
            //{
            //    refPosition = robotAligner.Align(refPosition);
            //}
            //AxisPosition tt = robotStage.GetActualPos();
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            robotStage.Move(new AxisPosition(refPosition.X, refPosition.Y));
            Grab();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            robotAligner.Save(PathSettings.Instance().Config);
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            stopFlag = true;
        }

        private void buttonUpdateStartPos_Click(object sender, EventArgs e)
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController?.RobotStage;
            PointF curPos = robotStage.GetCommandPos().ToPointF();

            txtStartX.Text = curPos.X.ToString();
            txtStartY.Text = curPos.Y.ToString();
        }

        private void buttonExportTestData_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                StringBuilder dataStr = new StringBuilder();
                dataStr.Append("Point, Ref.Position X, Ref.Position Y, Real Position X, Real Position Y, Offset X, Offset Y, Length");
                dataStr.AppendLine();

                for (int i=0; i<dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                        dataStr.Append(dataGridView1.Rows[i].Cells[j].Value + ", ");
                    dataStr.AppendLine();
                }

                File.WriteAllText(dlg.FileName, dataStr.ToString());
            }
        }
    }
}
