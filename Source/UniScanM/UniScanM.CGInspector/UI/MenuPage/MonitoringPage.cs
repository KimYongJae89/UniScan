//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using DynMvp.InspData;
//using UniEye.Base.Data;
//using DynMvp.Data.UI;
//using DynMvp.Base;
//using UniEye.Base.UI;
//using DynMvp.Data;
//using DynMvp.Devices;
//using DynMvp.UI;
//using UniEye.Base;
//using UniScanM.StillImage.Operation;
//using DynMvp.UI.Touch;
//using System.Threading;
//using Infragistics.Win.Misc;
//using Infragistics.Win;

//namespace UniScanM.StillImage.UI.MenuPage
//{
//    public partial class MonitoringPage : UserControl, IInspectionPage, IInspectionPanel, IOpStateListener, IInspectStateListener, IMultiLanguageSupport
//    {
//        private DrawBox drawBox = null;
//        private UniScanM.Data.FigureDrawOption figureDrawOption = null;

//        Label[] positionIndicator = null;
        
//        public IInspectionPanel InspectionPanel
//        {
//            get { return this; }
//        }

//        Control showHideControl;
//        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

//        public MonitoringPage()
//        {
//            InitializeComponent();

//            this.drawBox = new DrawBox();
//            this.drawBox.Dock = DockStyle.Fill;
//            this.drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
//            this.splitContainer.Panel1.Controls.Add(this.drawBox);

//            this.positionIndicator = new Label[6] { this.labelZone1, this.labelZone2, this.labelZone3, this.labelZone4, this.labelZone5, this.labelZone6 };

//            figureDrawOption = new UniScanM.Data.FigureDrawOption()
//            {
//                useTargetCoord = false,

//                PatternConnection = false,

//                TeachResult = new UniScanM.Data.FigureDrawOptionProperty()
//                {
//                    ShowFigure = true,
//                    Good = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
//                    Ng = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
//                    Invalid = new UniScanM.Data.DrawSet(null, null),

//                    ShowText = false,
//                    FontSet = new UniScanM.Data.FontSet(new Font("Gulim", 20), Color.Yellow)
//                },

//                ProcessResult = new UniScanM.Data.FigureDrawOptionProperty()
//                {
//                    ShowFigure = true,
//                    Good = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
//                    Ng = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
//                    Invalid = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0xFF, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0xFF, 0x00))),

//                    ShowText = false,
//                    FontSet = new UniScanM.Data.FontSet(new Font("Gulim", 20), Color.Red)
//                }
//            };

//            SystemState.Instance().AddOpListener(this);
//            SystemState.Instance().AddInspectListener(this);
//            ErrorManager.Instance().OnStartAlarm += ErrorManager_OnStartAlarm;

//            StringManager.AddListener(this);
//        }

//        private void ErrorManager_OnStartAlarm()
//        {
//        }

//        private void MonitoringPage_Load(object sender, EventArgs e)
//        {
//            for (int i = 0; i < this.positionIndicator.Length; i++)
//            {
//                this.positionIndicator[i].Font = UiHelper.AutoFontSize(this.positionIndicator[i], this.positionIndicator[i].Text);
//                this.positionIndicator[i].BackColor = Color.LightSteelBlue;
//            }
//        }

//        public void OpStateChanged(OpState curOpState, OpState prevOpState)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
//                return;
//            }

//            UpdateControlText(this.labelStatus, curOpState.ToString());

//            //if (curOpState == OpState.Idle)
//            //{
//            //}
//            //else if (curOpState == OpState.Wait)
//            //{
//            //    if (prevOpState == OpState.Idle)
//            //    {
//            //        // 정지->대기. 검사결과창 제거
//            //        UpdateControlText(this.labelStatus, SystemState.Instance().OpState.ToString());
//            //        Color color = Color.Black;
//            //        UpdateControlText(this.labelResult, "------", color);
//            //        UpdateControlText(this.labelMarginW, "---.-", color);
//            //        UpdateControlText(this.labelMarginH, "---.-", color);
//            //        UpdateControlText(this.labelBlotW, "--.-", color);
//            //        UpdateControlText(this.labelBlotH, "--.-", color);
//            //        UpdateControlText(this.labelDefectW, "--.-", color);
//            //        UpdateControlText(this.labelDefectH, "--.-", color);
//            //        UpdateControlText(this.labelDefectC, "--.-", color);
//            //    }
//            //    else if (prevOpState == OpState.Inspect)
//            //    {
//            //        // 검사->대기.
//            //        //UpdateControlText(this.labelStatus, SystemState.Instance().OpState.ToString());
//            //    }
//            //}
//            //else if (curOpState == OpState.Inspect)
//            //{
//            //    UpdateControlText(this.labelStatus, SystemState.Instance().InspectState.ToString());
//            //}
//        }

//        public void InspectStateChanged(InspectState curInspectState)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new InspectStateChangedDelegate(InspectStateChanged), curInspectState);
//                return;
//            }
//            if (curInspectState == InspectState.Wait)
//                return;

//            if(SystemState.Instance().OpState == OpState.Inspect)
//                UpdateControlText(this.labelStatus, curInspectState.ToString());
//        }

//        public void ProductInspected(InspectionResult inspectionResult)
//        {
//            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

//            int inspectZone = -1;
//            if (inspectionResult.ExtraResult.ContainsKey("InspectSequence"))
//                inspectZone = (int)inspectionResult.ExtraResult["InspectSequence"];
            
//            string inspectState = (string)myInspectionResult.ExtraResult["InspectState"];

//            if (inspectState == "Monitoring")
//            // In Monitoring State
//            {
//                UpdateZoneLabel(-1, Color.Empty);
//                UpdateControlText(this.labelResult, "Monitoring", Color.Black);
//                UpdateResultValue(Color.Black);
//            }
//            else if (inspectState == "Inspection")//myInspectionResult.ProcessResultList != null)
//            // In Inspection State
//            {
//                if(inspectionResult.Judgment == Judgment.Skip)
//                // sheet not founded 
//                {
//                    UpdateControlText(this.labelResult, "SKIP", Color.Yellow);
//                    UpdateResultValue(Color.Black);
//                    UpdateZoneLabel(inspectZone, Color.Yellow);
//                    return;
//                }

//                Rectangle roiInFov = myInspectionResult.RoiRectInFov;
//                Data.ProcessResult interestProcessResult = myInspectionResult.ProcessResultList.InterestProcessResult;
//                int defectCount = myInspectionResult.ProcessResultList.DefectRectList.Count;
//                Rectangle defectRect = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect();

//                bool isGood = (interestProcessResult == null ? false : interestProcessResult.IsGood) && defectRect.IsEmpty;
//                Color color = isGood ? Color.Green : Color.Red;

//                UpdateZoneLabel(inspectZone, color);

//                if (interestProcessResult == null)
//                // Inspection Fail
//                {
//                    UpdateControlText(this.labelResult, "NG", color);
//                    UpdateResultValue(Color.Black);
//                }
//                else
//                {
//                    // Update Result Text
//                    string message = string.Format("{0}", isGood ? "OK" : "NG", color);
//                    UpdateControlText(this.labelResult, message, color);

//                    // Draw Figure
//                    FigureGroup figureGroup = new FigureGroup();

//                    Color blotFigColor = interestProcessResult.IsBlotGood ? Color.LightGreen : Color.Red;
//                    //RectangleF figRect = interestProcessResult.InspPatternInfo.ShapeInfo.BaseRect;
//                    RectangleF blotFigRect = myInspectionResult.BlotInspRect;
//                    figureGroup.AddFigure(new RectangleFigure(blotFigRect, new Pen(blotFigColor, 3)));

//                    Color marginFigColor = interestProcessResult.IsMarginGood ? Color.Green : Color.Red;
//                    //figRect.Inflate(interestProcessResult.InspPatternInfo.TeachInfo.Feature.Margin);
//                    RectangleF marginFigRect = myInspectionResult.MarginInspRect;
//                    figureGroup.AddFigure(new RectangleFigure(marginFigRect, new Pen(marginFigColor, 3)));

//                    foreach (Rectangle defRect in myInspectionResult.ProcessResultList.DefectRectList)
//                    {
//                        defRect.Inflate(10, 10);
//                        figureGroup.AddFigure(new RectangleFigure(defRect, new Pen(Color.Red, 3)));
//                    }

//                    figureGroup.Offset(-myInspectionResult.RoiRectInFov.X, -myInspectionResult.RoiRectInFov.Y);
//                    figureGroup.Offset(-myInspectionResult.DisplayImageRect.Left, -myInspectionResult.DisplayImageRect.Top);
//                    drawBox.FigureGroup.Clear();
//                    drawBox.FigureGroup.AddFigure(figureGroup);
//                    //drawBoxBig.FigureGroup.Offset(this.displayedImageRect.Left, this.displayedImageRect.Top);

//                    // Update Result Value
//                    SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

//                    // 검출된 크기
//                    Data.Feature feature = interestProcessResult.InspPatternInfo.TeachInfo.Feature.Mul(pelSize);

//                    // 오프셋 크기
//                    Data.Feature offset = interestProcessResult.OffsetValue.Mul(pelSize);

//                    // Defect 크기
//                    SizeF defectSizeF = new SizeF((float)(defectRect.Width * pelSize.Width), (float)(defectRect.Height * pelSize.Height));

//                    Color colorMargin = interestProcessResult.IsMarginGood ? Color.Green : Color.Red;
//                    Color colorBlot = interestProcessResult.IsBlotGood ? Color.Green : Color.Red;
//                    Color colorDef = defectCount == 0 ? Color.Green : Color.Red;
//                    UpdateResultValue(feature, offset, defectCount, defectSizeF, colorMargin, colorBlot, colorDef);
//                }
//            }
//            else if(inspectState == "Teaching")
//            {
//                UpdateZoneLabel(-1, Color.Empty);
//                bool isGood = myInspectionResult.IsGood();
//                Color color = isGood ? Color.LightGreen : Color.Red;
//                string message = string.Format("{0}", isGood ? "Teached" : "NOT Teached");
//                if (myInspectionResult.ExtraResult.ContainsKey("Sequnece"))
//                // In Teach State
//                {
//                    message += string.Format(" - {0}", myInspectionResult.ExtraResult["Sequnece"]);
//                }

//                UpdateControlText(this.labelResult, message, color);
//                UpdateResultValue(Color.Black);
//                //UpdateControlText(this.labelResult, "Teached", Color.Green);
//                //SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
//                //Data.Feature feature = myInspectionResult.TeachData.PatternInfoGroupList.Find(f => f.TeachInfo.Use).TeachInfo.Feature.Mul(pelSize);
//                //UpdateResultValue(feature, Color.Green);
//            }
//            else if(inspectState == "LightTune")
//            {
//                UpdateZoneLabel(-1, Color.Empty);
//                int offsetLevel = myInspectionResult.LightTuneResult.OffsetLevel;
//                bool isGood = myInspectionResult.IsGood();
//                int tryCount = myInspectionResult.LightTuneResult.TryCount;
//                Color color = isGood ? Color.LightGreen : Color.Red;
//                string message = string.Format("{0} ({1})", isGood ? "GOOD" : offsetLevel > 0 ? "Too Dark" : offsetLevel < 0 ? "Too Bright" : "No Sheet", tryCount);

//                UpdateControlText(this.labelResult, message, color);
//                UpdateResultValue(Color.Black);
//            }

//            drawBox.Invalidate();
//        }

//        private void UpdateZoneLabel(int zoneNo, Color color)
//        {
//            for (int i = 0; i < positionIndicator.Length; i++)
//            {
//                if (i == zoneNo)
//                    positionIndicator[i].BackColor = color;
//                else
//                    positionIndicator[i].BackColor = Color.LightSteelBlue;
//            }
//        }

//        private void UpdateResultValue(Color color)
//        {
//            //UpdateControlText(this.labelResult, "-----", color);
//            UpdateControlText(this.labelMarginW, "---.-", color);
//            UpdateControlText(this.labelMarginH, "---.-", color);
//            UpdateControlText(this.labelBlotW, "--.-", color);
//            UpdateControlText(this.labelBlotH, "--.-", color);
//            UpdateControlText(this.labelDefectW, "--.-", color);
//            UpdateControlText(this.labelDefectH, "--.-", color);
//            UpdateControlText(this.labelDefectC, "--.-", color);
//        }

//        private void UpdateResultValue(Data.Feature measure, Color color)
//        {
//            string format = "{0:000.0}";
//            UpdateControlText(this.labelMarginW, string.Format(format, measure.Margin.Width), color);
//            UpdateControlText(this.labelMarginH, string.Format(format, measure.Margin.Height), color);
//            UpdateControlText(this.labelBlotW, string.Format(format, measure.Blot.Width), color);
//            UpdateControlText(this.labelBlotH, string.Format(format, measure.Blot.Height), color);

//            UpdateControlText(this.labelDefectW, "--.-", color);
//            UpdateControlText(this.labelDefectH, "--.-", color);
//            UpdateControlText(this.labelDefectC, "--.-", color);
//        }

//        private void UpdateResultValue(Data.Feature measure, Data.Feature offset, int defectCount, SizeF defectSize, Color colorMargin, Color colorBlot, Color colorDef)
//        {
//            //string format = "{0:000.0} \r\n ({1:0.0})";
//            //UpdateControlText(this.label11, string.Format(format, measure.Margin.Width, offset.Margin.Width), color);
//            //UpdateControlText(this.label14, string.Format(format, measure.Margin.Height, offset.Margin.Height), color);
//            //UpdateControlText(this.label17, string.Format(format, measure.Blot.Width, offset.Blot.Width), color);
//            //UpdateControlText(this.label20, string.Format(format, measure.Blot.Height, offset.Blot.Height), color);

//            string format1 = "{0:000.0}";
//            // Margin은 측정값
//            UpdateControlText(this.labelMarginW, string.Format(format1, measure.Margin.Width), colorMargin);
//            UpdateControlText(this.labelMarginH, string.Format(format1, measure.Margin.Height), colorMargin);

//            // Blot은 차이값
//            string format2 = "{0:00.0}";
//            UpdateControlText(this.labelBlotW, string.Format(format2, Math.Max(0, offset.Blot.Width)), colorBlot);
//            UpdateControlText(this.labelBlotH, string.Format(format2, Math.Max(0, offset.Blot.Height)), colorBlot);

//            // DefectSize
//            string format3 = "{0:000}";
//            UpdateControlText(this.labelDefectW, string.Format(format3, defectSize.Width), colorDef);
//            UpdateControlText(this.labelDefectH, string.Format(format3, defectSize.Height), colorDef);
//            string format4 = "{0:0}";
//            UpdateControlText(this.labelDefectC, string.Format(format4, defectCount), colorDef);
//        }

//        public void UpdateControl(string item, object value)
//        {
//            throw new NotImplementedException();
//        }

//        private delegate void UpdateControlTextDelegate(Label labelResult, string text, Color? foreColor);
//        private void UpdateControlText(Control control, string text, Color? foreColor = null)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateControlTextDelegate(UpdateControlText), control, text, foreColor);
//                return;
//            }

//            //if (control is Label)
//            //    control.Font = DynMvp.UI.UiHelper.AutoFontSize((Label)control, text);

//            if (foreColor != null)
//                control.ForeColor = foreColor.Value;

//            control.Text = StringManager.GetString(this.GetType().FullName, text);
//        }

//        public void EnableControls()
//        {
//            throw new NotImplementedException();
//        }

//        public void PageVisibleChanged(bool visibleFlag)
//        {
//            this.Visible = visibleFlag;

//            Color buttonBackColor = visibleFlag ? Color.CornflowerBlue : Color.Transparent;
//            if (this.showHideControl != null)
//                ((UltraButton)this.showHideControl).Appearance.BackColor = buttonBackColor;
//        }

//        public Production GetCurrentProduction()
//        {
//            throw new NotImplementedException();
//        }

//        public void Initialize()
//        {
//            throw new NotImplementedException();
//        }

//        public void ClearPanel()
//        {
//            throw new NotImplementedException();
//        }

//        public void EnterWaitInspection()
//        {
//            throw new NotImplementedException();
//        }

//        public void ExitWaitInspection()
//        {
            
//        }

//        public void OnPreInspection()
//        {
//        }

//        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
//        {
//            throw new NotImplementedException();
//        }

//        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
//        {
//            throw new NotImplementedException();
//        }

//        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
//        {
//            throw new NotImplementedException();
//        }

//        delegate void UpdateImageDeleagte(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult);
//        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateImageDeleagte(UpdateImage), deviceImageSet, groupId, inspectionResult);
//                return;
//            }

//            LogHelper.Debug(LoggerType.Debug, "MonitoringPage::UpdateImage");
//            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

//            int inspectSequence = -1;
//            if (inspectionResult.ExtraResult.ContainsKey("InspectSequence"))
//                inspectSequence = (int)inspectionResult.ExtraResult["InspectSequence"];

//            //if (myInspectionResult.ProcessResultList != null&& myInspectionResult.ProcessResultList.InterestProcessResult!=null)
//            //{
//            //    Data.ProcessResult processResult = myInspectionResult.ProcessResultList.InterestProcessResult;
//            //    Rectangle defectRect = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect();
//            //    Color color = processResult == null ? Color.Red : (processResult.IsGood && defectRect.IsEmpty ? Color.LightGreen : Color.Red);

//            //    figureGroup.AddFigure(new RectangleFigure(processResult.InspPatternInfo.ShapeInfo.BaseRect, new Pen(color, 3)));

//            //    foreach (Rectangle defRect in myInspectionResult.ProcessResultList.DefectRectList)
//            //    {
//            //        defRect.Inflate(10, 10);
//            //        figureGroup.AddFigure(new RectangleFigure(defRect, new Pen(Color.Red, 3)));
//            //    }
//            //}

//            //Bitmap bitmap = deviceImageSet.ImageList2D[0]?.ToBitmap();
//            ////if(bitmap!=null)
//            //drawBoxBig.UpdateImage(bitmap);

//            //drawBoxBig.FigureGroup.Clear();
//            //figureGroup.Offset(-myInspectionResult.RoiRectInFov.X, -myInspectionResult.RoiRectInFov.Y);
//            //drawBoxBig.FigureGroup.AddFigure(figureGroup);
//            //bitmap?.Dispose();

//            if (deviceImageSet.ImageList2D.Count > 0)
//            // 화면면 비율에 맞게 적당히 잘라냄
//            {
//                LogHelper.Debug(LoggerType.Debug, "MonitoringPage::UpdateImage - Image Founded");

//                Image2D image2D = deviceImageSet.ImageList2D[0];
//                Bitmap bitmap;
//                if (image2D == null)
//                {
//                    bitmap = null;
//                }
//                else
//                {
//                    Size imageSize = image2D.Size;
//                    Size displatSize = drawBox.Size;
//                    float rX = imageSize.Width * 1.0f / displatSize.Width * 1.0f;
//                    float rY = imageSize.Height * 1.0f / displatSize.Height * 1.0f;
//                    float ratio = Math.Min(rX, rY);
//                    Size clipImageSize = Size.Round(new SizeF(displatSize.Width * ratio, displatSize.Height * ratio));
//                    Size sizeDiff = new Size(imageSize.Width - clipImageSize.Width, imageSize.Height - clipImageSize.Height);
//                    Rectangle clipRect = new Rectangle(Point.Empty, imageSize);
//                    clipRect.Inflate(-sizeDiff.Width / 2, -sizeDiff.Height / 2);
//                    if (clipRect.Width > 0 && clipRect.Height > 0)
//                    {
//                        myInspectionResult.DisplayImageRect = clipRect;
//                        ImageD clipImageD = image2D.ClipImage(clipRect);
//                        bitmap = clipImageD.ToBitmap();
//                        clipImageD.Dispose();
//                    }
//                    else
//                    {
//                        bitmap = image2D.ToBitmap();
//                    }
//                }

//                drawBox.UpdateImage(bitmap);
//                drawBox.FigureGroup.Clear();
//                drawBox.ZoomFit();

//                bitmap?.Dispose();
//            }
//        }

//        public void OnPostInspection()
//        {

//        }

//        public void ModelChanged(Model model = null)
//        {
            
//        }

//        public void InfomationChanged(object obj = null)
//        {
//            throw new NotImplementedException();
//        }

//        public void UpdateLanguage()
//        {
//            StringManager.UpdateString(this);
//        }

//        //private void buttonRoller_Click(object sender, EventArgs e)
//        //{
//        //    DynMvp.Devices.MotionController.AxisHandler convayor = SystemManager.Instance().DeviceController.Convayor;
//        //    if (convayor == null)
//        //        return;

//        //    SerialEncoderV105 serialEncoder = ((SerialEncoderV105)SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder));

//        //    if (convayor.CheckValidState() == false)
//        //        return;

//        //    if (convayor.IsMoveDone())
//        //    {
//        //        double distPerRound = 0.1 * Math.PI;    //  [m/r2]
//        //        double pulsePerRound = 10000;   //[pls/r1]
//        //        double grarRate = 12.0 / 44.0;  //[r2/r1]
//        //        double speedPls = convayor.GetAxis(0).AxisParam.JogParam.MaxVelocity;   // [pls]

//        //        double speedMetor = speedPls / pulsePerRound * grarRate * distPerRound * 60;
//        //        //double speedMetor = (speedPls * 3 * Math.PI) / (1100);  // x m/m => x*3pi/1100
//        //        InputForm inputForm = new InputForm("Speed? [m/min]", speedMetor.ToString("F1"));
//        //        inputForm.StartPosition = FormStartPosition.CenterParent;
//        //        if (inputForm.ShowDialog() == DialogResult.OK)
//        //        {
//        //            speedMetor = double.Parse(inputForm.InputText);
//        //            speedPls = speedMetor * pulsePerRound / grarRate / distPerRound / 60;
//        //            convayor.GetAxis(0).AxisParam.JogParam.MaxVelocity = (float)speedPls;
//        //            if (convayor.ContinuousMove())
//        //            {
//        //                float calib = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width;
//        //                double grabHz = speedMetor / 60 * 1000000 / calib;  // 1/s
//        //                ((Operation.InspectRunner)SystemManager.Instance().InspectRunner).AsyncGrabExpUs = (float)(1 / grabHz * 1000000);   // s -> us
//        //            }

//        //            if (serialEncoder != null)
//        //            {
//        //                serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.FQ, (speedMetor / 60.0 / 0.000007).ToString());
//        //                serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "1");
//        //            }
//        //            buttonRoller.BackColor = Color.LightSalmon;
//        //        }
//        //    }
//        //    else
//        //    {
//        //        convayor.StopMove();
//        //        buttonRoller.BackColor = Color.Transparent; ;

//        //        if (serialEncoder != null)
//        //        {
//        //            serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "0");
//        //        }
//        //    }
//        //}
//    }
//}
