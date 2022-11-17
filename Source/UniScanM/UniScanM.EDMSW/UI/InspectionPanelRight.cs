using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.Devices.Light;
using UniScanM.EDMSW.Operation;
using UniScanM.EDMSW.Data;
using DynMvp.InspData;
using UniScanM.EDMSW.MachineIF;
using UniScanM.Data;
using UniScanM.Operation;
using UniScanM.EDMSW.Settings;
using System.Threading;
using ScottPlot;


namespace UniScanM.EDMSW.UI
{
    public partial class InspectionPanelRight : UserControl, IInspectionPanel, IMultiLanguageSupport, IOpStateListener
    {
        private float m_fDrawScale = 0.1f;
        private CanvasPanel canvasPanel = null;
        Data.InspectionResult m_EDMSResult = null;
        object lockobjRevison = new object();
        bool bChangedParameter = false;
        public InspectionPanelRight()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.SetPanMode();
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.SizeChanged += DrawBox_SizeChanged;
            this.canvasPanel.ReadOnly = true;
            this.canvasPanel.BackColor = Color.CornflowerBlue;

            panelImage.Controls.Add(this.canvasPanel);

            SetDoubleBuffered(this);

            SetDoubleBuffered(canvasPanel);

            SetDoubleBuffered(labelFilmValue);
            SetDoubleBuffered(labelCoatingValue);
            SetDoubleBuffered(labelPrintingValue);
            SetDoubleBuffered(labelPrintingPos);


            StringManager.AddListener(this);
            SystemState.Instance().AddOpListener(this);

            EDMSSettings.Instance().AdditionalSettingChangedDelegate += this.UpdateParamControl;
            InitComboBoxPreset();
        }
        
        private void DrawBox_SizeChanged(object sender, EventArgs e)
        {
            //UpdateModelData();
            canvasPanel.ZoomFit();
        }

        private void InitComboBoxPreset()
        {
            UniScanM.EDMSW.Data.Model currentModel = SystemManager.Instance().CurrentModel as UniScanM.EDMSW.Data.Model;
            if(currentModel !=null)
            {
                for (int i = 0; i < currentModel.PresetArray.Length; i++)
                {
                    string name = string.Format("Preset{0}", i);
                    if (!cb_Preset.Items.Contains(name))
                    {
                        cb_Preset.Items.Add(name);
                    }
                }
                cb_Preset.SelectedIndex = currentModel.SelectedPresetNum;
            }
        }
        private void cb_Preset_SelectedIndexChanged(object sender, EventArgs e)
        {
            UniScanM.EDMSW.Data.Model currentModel = SystemManager.Instance().CurrentModel as UniScanM.EDMSW.Data.Model;
            currentModel.SelectedPresetNum = cb_Preset.SelectedIndex;// cb_Preset.SelectedItem;
            UpdateModelData();
            bChangedParameter = true;
        }
        private void timer_GUIRevison_Tick(object sender, EventArgs e)
        {
            timer_GUIRevison.Stop();
            AsyncDraw();
        }
        static int nDisplayRevisionCount = 0;

        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            //if (m_EDMSResult != null)
            //{
            //      lock(m_EDMSResult)
            //        m_EDMSResult = (Data.InspectionResult)inspectionResult;
            //    timer_GUIRevison.Start();
            //}
            //////////////////////////////////////////////////////////////////////////
            //화면 갱신중이면, 현결과는 그냥 스킵...
            Exception eLockException = null;
            bool bLockWasTaken = false;
            try
            {
                System.Threading.Monitor.TryEnter(lockobjRevison, 0, ref bLockWasTaken);
                if (bLockWasTaken)
                {
                    m_EDMSResult = (Data.InspectionResult)inspectionResult;
                    timer_GUIRevison.Start();
                }
                else return;
                    //throw new Exception("Timeout exceeded, unable to lock.");
            }
            catch (Exception ex)
            {
                // conserver l'exception
                eLockException = ex;
            }
            finally
            {
                // release le lock
                if (bLockWasTaken)
                    Monitor.Exit(lockobjRevison);
                // relancer l'exception
                //if (eLockException != null)
                   // throw new Exception("An exception occured during the lock proces.", eLockException);
            }
        }

        public void AsyncDraw()
        {
            if (m_EDMSResult == null) return;
            Data.InspectionResult myInspectionResult = m_EDMSResult;

            lock (lockobjRevison)
            {
                if (SystemState.Instance().OpState != OpState.Idle)
                {
                    if (myInspectionResult.State == State_EDMS.Waiting)
                    {
                        labelState.ForeColor = Color.Black;
                        labelState.BackColor = Color.Yellow;
                        labelState.Text = string.Format("{0} - {1}[m]", StringManager.GetString(this.GetType().FullName, "Wait"), myInspectionResult.RemainWaitDist);
                    }
                    else if (myInspectionResult.State == State_EDMS.Zeroing)
                    {
                        labelState.ForeColor = Color.Black;
                        labelState.BackColor = Color.Yellow;
                        labelState.Text = StringManager.GetString(this.GetType().FullName, "Zeroing");
                    }
                    else if (myInspectionResult.State == State_EDMS.Inspecting)
                    {
                        labelState.ForeColor = Color.White;
                        labelState.BackColor = Color.Green;
                        labelState.Text = StringManager.GetString(this.GetType().FullName, "Measure");
                        switch (myInspectionResult.Judgment)
                        {
                            case Judgment.Accept:
                            case Judgment.FalseReject:
                                break;
                            case Judgment.Reject:
                                break;
                            case Judgment.Skip:
                                break;
                            case Judgment.Warn:
                                break;
                        }
                    }
                    
                    int progressPercent = (int)(myInspectionResult.ZeroingNum / EDMSSettings.Instance().ZeroingCount * 100.0);
                    if (progressPercent < 0) progressPercent = 0;
                    else if (progressPercent > 100) progressPercent = 100;
                    progressBarZeroing.Value = progressPercent;
                }//if (SystemState.Instance().OpState != OpState.Idle)

                Bitmap bitmap = null;

                lock (myInspectionResult.DisplayBitmap)
                    bitmap = (Bitmap)myInspectionResult.DisplayBitmap.Clone();

                FigureGroup figureGroup = new FigureGroup();
                // 3mm 오프셋 기준선
                DrawReferenceLine(figureGroup, bitmap, myInspectionResult);

                //눈금자 그리기. 1mm간격
                DrawRuler(figureGroup, bitmap, myInspectionResult);

                //찾은 엣지 그리기
                DrawEdgeLine(figureGroup, bitmap, myInspectionResult);

                ///화면에 Revision 숫자 표시
                nDisplayRevisionCount++;
                if (nDisplayRevisionCount >= 100) nDisplayRevisionCount = 0;
                TextFigure text = new TextFigure(nDisplayRevisionCount.ToString(), 
                    new Point((int)(200 * m_fDrawScale), (int)(200 *m_fDrawScale)), 
                    new Font(FontFamily.GenericSerif, 300 *m_fDrawScale), Color.Blue); 

                figureGroup.AddFigure(text);
                //화면에 프레임 intensity 표시

                TextFigure text2 = new TextFigure(myInspectionResult.FrameAvgIntensityLeft.ToString("#.0"), 
                    new Point((int)(200 * m_fDrawScale), (int)(600 * m_fDrawScale)),
                    new Font(FontFamily.GenericSerif, 300 * m_fDrawScale), 
                    Color.Red);

                figureGroup.AddFigure(text2);
                //////////////////////////////////////
                canvasPanel.Clear();
                canvasPanel.WorkingFigures.AddFigure(figureGroup);
                UpdateData(myInspectionResult.TotalEdgePositionResultLeft, bitmap);
            }//lock (lockobjRevison)
        }

        // 3mm 오프셋 기준선


        private void DrawReferenceLine(FigureGroup figureGroup, Bitmap bitmap, Data.InspectionResult myInspectionResult)
        {
            if (bitmap == null) return;
            Pen rulerLinePenV = new Pen(Color.FromArgb(128, 255, 0, 0), 100 * m_fDrawScale);
            PointF verticalOriginStartPt;
            PointF verticalOriginEndPt;
            //if (EDMSSettings.Instance().IsFrontPosition)
            //{
            //    verticalOriginStartPt = new PointF(bitmap.Width - 3 * 1000 / 5, 0);
            //    verticalOriginEndPt = new PointF(bitmap.Width - 3 * 1000 / 5, bitmap.Height);
            //}
            //else
            {
                verticalOriginStartPt = new PointF(3 * 1000 / 5  * m_fDrawScale , 0 );
                verticalOriginEndPt = new PointF(3 * 1000 / 5 * m_fDrawScale, bitmap.Height );
            }
            figureGroup.AddFigure(new LineFigure(verticalOriginStartPt, verticalOriginEndPt, rulerLinePenV));


            Pen LinePenV = new Pen(Color.FromArgb(128, 0, 255, 0), 100 * m_fDrawScale);
            verticalOriginStartPt = new PointF(bitmap.Width/2, 0);
            verticalOriginEndPt = new PointF(bitmap.Width/2 , bitmap.Height);
            figureGroup.AddFigure(new LineFigure(verticalOriginStartPt, verticalOriginEndPt, LinePenV));
        }

        //눈금자 그리기. 1mm간격
        private void DrawRuler(FigureGroup figureGroup, Bitmap bitmap, Data.InspectionResult myInspectionResult)
        {
            if (bitmap == null) return;
            Pen rulerLinePenV = new Pen(Color.FromArgb(50, 0, 255, 0), 100);
            Pen rulerLinePenH = new Pen(Color.Green, 2);

            float rulerHeight = bitmap.Height != 0 ? bitmap.Height - (bitmap.Height / 4) : 100;

            PointF rulerStartPt = new PointF(0, rulerHeight );
            PointF rulerEndPt = new PointF(bitmap.Width, rulerHeight );
            figureGroup.AddFigure(new LineFigure(rulerStartPt, rulerEndPt, rulerLinePenH));

            for (int gridIndex = 0; gridIndex < bitmap.Width; gridIndex += (int)(200 *m_fDrawScale))
            {
                PointF gridStartPt = new PointF(gridIndex, rulerHeight - (400 * m_fDrawScale) );
                PointF gridEndPt = new PointF(gridIndex, rulerHeight);
                figureGroup.AddFigure(new LineFigure(gridStartPt, gridEndPt, rulerLinePenH));
            }
        }
            //찾은 엣지 그리기
        private void DrawEdgeLine(FigureGroup figureGroup, Bitmap bitmap, Data.InspectionResult myInspectionResult)
        {
            double[] pixelPositionLeft = myInspectionResult.EdgePositionResultLeft;
            double[] pixelPositionRight = myInspectionResult.EdgePositionResultRight;
            PointF verticalOriginStartPt;
            PointF verticalOriginEndPt;

            if (pixelPositionLeft != null && pixelPositionRight != null && bitmap != null)
            {
                Pen arrowPen = new Pen(Color.Red, 2);
                arrowPen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                Pen linePen = new Pen(Color.Yellow, 2);

                //Left
                PointF filmVerticalStartPt = new PointF((float)pixelPositionLeft[0] * m_fDrawScale, 0);
                PointF filmVerticalEndPt = new PointF((float)pixelPositionLeft[0] * m_fDrawScale, bitmap.Height);
                PointF coatingVerticalStartPt = new PointF((float)pixelPositionLeft[1] * m_fDrawScale, 0);
                PointF coatingVerticalEndPt = new PointF((float)pixelPositionLeft[1] * m_fDrawScale, bitmap.Height);
                PointF printingVerticalStartPt = new PointF((float)pixelPositionLeft[2] * m_fDrawScale, 0);
                PointF printingVerticalEndPt = new PointF((float)pixelPositionLeft[2] * m_fDrawScale, bitmap.Height);

                figureGroup.AddFigure(new LineFigure(filmVerticalStartPt, filmVerticalEndPt, linePen));
                figureGroup.AddFigure(new LineFigure(coatingVerticalStartPt, coatingVerticalEndPt, linePen));
                figureGroup.AddFigure(new LineFigure(printingVerticalStartPt, printingVerticalEndPt, linePen));

                //Right
                //if (EDMSSettings.Instance().IsFrontPosition)
                {
                    verticalOriginStartPt = new PointF(bitmap.Width - 3 * 1000 / 5, 0);
                    verticalOriginEndPt = new PointF(bitmap.Width - 3 * 1000 / 5, bitmap.Height);

                    filmVerticalStartPt = new PointF((float)(bitmap.Width - pixelPositionRight[0] * m_fDrawScale), 0);
                    filmVerticalEndPt = new PointF((float)(bitmap.Width - pixelPositionRight[0] * m_fDrawScale), bitmap.Height);
                    coatingVerticalStartPt = new PointF((float)(bitmap.Width - pixelPositionRight[1] * m_fDrawScale), 0);
                    coatingVerticalEndPt = new PointF((float)(bitmap.Width - pixelPositionRight[1] * m_fDrawScale), bitmap.Height);
                    printingVerticalStartPt = new PointF((float)(bitmap.Width - pixelPositionRight[2] * m_fDrawScale), 0);
                    printingVerticalEndPt = new PointF((float)(bitmap.Width - pixelPositionRight[2] * m_fDrawScale), bitmap.Height);
                }
                figureGroup.AddFigure(new LineFigure(filmVerticalStartPt, filmVerticalEndPt, linePen));
                figureGroup.AddFigure(new LineFigure(coatingVerticalStartPt, coatingVerticalEndPt, linePen));
                figureGroup.AddFigure(new LineFigure(printingVerticalStartPt, printingVerticalEndPt, linePen));

                ///////////////////////////////////
                ///Profile ////////////////////////

                if (myInspectionResult.ProfileLeftHor != null && myInspectionResult.ProfileRightHor != null)
                    if (!checkOnTune.Checked)
                    {
                        var profilelist = new List<float>();
                        profilelist.AddRange(myInspectionResult.ProfileLeftHor);
                        profilelist.AddRange(myInspectionResult.ProfileRightHor);
                        var profile = profilelist.ToArray();
                        //////////////////////////////////
                        Pen penProfile = new Pen(Color.FromArgb(90, 0, 200, 0), 5);
                        float fmax = profile.Max();
                        float profileHeight = (bitmap.Height / 4);
                        double Scale = profileHeight / fmax;

                        PointF StartPt;
                        PointF EndPt;
                        int y = 0;// bitmap.Height - (int)(profile[x] * Scale) - 1;
                        for (int x = 0; x < bitmap.Width; x += 1)
                        {
                            y = bitmap.Height - (int)(profile[ (int )(x  / m_fDrawScale) ] * Scale) - 1;
                            y = y > bitmap.Height - 1 ? bitmap.Height - 1 : y;
                            //if (EDMSSettings.Instance().IsFrontPosition)
                            //{
                            //    StartPt = new PointF(bitmap.Width - x - 1, bitmap.Height - 1);
                            //    EndPt = new PointF(bitmap.Width - x - 1, y);
                            //}
                            //else
                            {
                                StartPt = new PointF(x , bitmap.Height - 1);
                                EndPt =   new PointF(x , y);

                            }
                            figureGroup.AddFigure(new LineFigure(StartPt, EndPt, penProfile));
                        }

                        //Threshold 문턱값 그리기/////////////////////////////////////////////////////////////////////
                        //Pen penTh = new Pen(Color.FromArgb(255, 255, 0, 0), 20);
                        Pen[] pens = { Pens.Red, Pens.Green, Pens.Blue, Pens.Cyan, Pens.Magenta, Pens.Yellow };
                        Pen penTh = new Pen(Color.FromArgb(255, 255, 0, 0), 20);
                        UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
                        int[] Th = new int[3];
                        Th[0] = (int)(((EDMSParam)(currentModel).InspectParam).FilmThreshold * Scale);
                        Th[1] = (int)(((EDMSParam)(currentModel).InspectParam).CoatingThreshold * Scale);
                        Th[2] = (int)(((EDMSParam)(currentModel).InspectParam).PrintingThreshold * Scale);
                        int linewidth = (int)(200 * m_fDrawScale);
                        for (int i = 0; i < 3; i++)
                        {
                            int sX = (int)(pixelPositionLeft[i] * m_fDrawScale - linewidth);
                            int eX = (int)(pixelPositionLeft[i] * m_fDrawScale + linewidth);

                            figureGroup.AddFigure(
                                    new LineFigure(
                                    new PointF(sX, bitmap.Height - Th[i]),
                                    new PointF(eX, bitmap.Height - Th[i]),
                                    pens[i]) );


                            //if (EDMSSettings.Instance().IsFrontPosition)
                            {
                                sX = (int)(bitmap.Width - pixelPositionRight[i] * m_fDrawScale - linewidth);
                                eX = (int)(bitmap.Width - pixelPositionRight[i] * m_fDrawScale + linewidth);
                            }
                            figureGroup.AddFigure(
                                new LineFigure(
                                    new PointF(sX, bitmap.Height - Th[i]),
                                    new PointF(eX, bitmap.Height - Th[i]),
                                    pens[i])  );


                        }
                    }
            }
        }


        private delegate void UpdateDataDelegate(double[] position, Bitmap bitmap);
        private void UpdateData(double[] position, Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData), position, bitmap);
                return;
            }

            if (position != null)
            {
                double[] datas = new double[3];
                datas[0] = position[(int)Data.DataType.FilmEdge];
                datas[1] = Math.Max(0, position[(int)Data.DataType.Coating_Film]);
                datas[2] = Math.Max(0, position[(int)Data.DataType.Printing_Coating]);
                double posPrint = Array.TrueForAll(datas, f => f > 0) ? datas.Sum() : 0;

                labelFilmValue.Text = String.Format("{0:0.000}", datas[0]);
                labelCoatingValue.Text = String.Format("{0:0.000}", datas[1]);
                labelPrintingValue.Text = String.Format("{0:0.000}", datas[2]);
                labelPrintingPos.Text = String.Format("{0:0.000}", posPrint);
            }
            else
            {
                labelFilmValue.Text = String.Format("-.--");
                labelCoatingValue.Text = String.Format("-.--");
                labelPrintingValue.Text = String.Format("-.--");
                labelPrintingPos.Text = String.Format("-.--");
            }

            bool zoomFitRequred = canvasPanel.Image == null;
            canvasPanel.UpdateImage(bitmap);

            //좌우 뒤집기...
   //         if (EDMSSettings.Instance().IsFrontPosition)
   //             canvasPanel.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            if (zoomFitRequred == true)
                canvasPanel.ZoomFit();
                                                                                                                     
            canvasPanel.Invalidate();
        }                                                   

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        bool onUpdate = false;
        private void UpdateModelData()
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                onUpdate = true;
                if (currentModel.LightParamSet.NumLight != 0)
                {
                    backLightValue.Value = currentModel.LightParamSet.LightParamList[0].LightValue.Value[0];
                    frontLightValue.Value = currentModel.LightParamSet.LightParamList[0].LightValue.Value[1];
                    numericUpDownFilmThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).FilmThreshold;
                    numericUpDownCoatingThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).CoatingThreshold;
                    numericUpDownPrintingThreshold.Value = (decimal)((EDMSParam)(currentModel).InspectParam).PrintingThreshold;
                }
                onUpdate = false;
            }
        }

        private void lightValue_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;
            bChangedParameter = true;
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                int lightCount = currentModel.LightParamSet.LightParamList[0].LightValue.Value.Length;
                int[] lightvalue = new int[4];
                lightvalue[0] = (int)backLightValue.Value; //backLight Right
                lightvalue[1] = (int)frontLightValue.Value; //ToptLight Right
                lightvalue[2] = (int)backLightValue.Value; //backLight   Left
                lightvalue[3] = (int)frontLightValue.Value; //TopLight   Left

                for (int i=0; i<lightCount; i++)
                {
                    currentModel.LightParamSet.LightParamList[0].LightValue.Value[i] = (int)lightvalue[i];
                }
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                SystemManager.Instance().InspectRunner.PostEnterWaitInspection(); //여기서 조명을 켜고 있음 ^^ ㅋㅋ
            }
        }

        private void numericUpDownFilmThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                bChangedParameter = true;
                ((EDMSParam)currentModel.InspectParam).FilmThreshold = (double)numericUpDownFilmThreshold.Value;
            }
        }

        private void numericUpDownCoatingThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                bChangedParameter = true;
                ((EDMSParam)currentModel.InspectParam).CoatingThreshold = (double)numericUpDownCoatingThreshold.Value;
            }
        }

        private void numericUpDownPrintingThreshold_ValueChanged(object sender, EventArgs e)
        {
            UniScanM.Data.Model currentModel = SystemManager.Instance().CurrentModel;
            if (currentModel != null)
            {
                bChangedParameter = true;
                ((EDMSParam)currentModel.InspectParam).PrintingThreshold = (double)numericUpDownPrintingThreshold.Value;
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            UpdateParamControl();
        }

        public void Initialize() { }

        public delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if(InvokeRequired)
            {
                this.Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            canvasPanel.Clear();
            canvasPanel.UpdateImage(null);
            canvasPanel.Invalidate();

            labelCoatingValue.Text = "0";
            labelPrintingValue.Text = "0";
            labelPrintingPos.Text = "0";
        }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(DynMvp.Data.Model model = null) { }
        public void InfomationChanged(object obj = null) { }

        private void buttonPLCTeset_Click(object sender, EventArgs e)
        {

         
        }

        private void buttonStateReset_Click(object sender, EventArgs e)
        {
            SystemManager.Instance().InspectRunner.ResetState();
        }

        private void checkOnTune_CheckedChanged(object sender, EventArgs e)
        {
            OperationOption.Instance().OnTune = !checkOnTune.Checked;
            UpdateParamControl();

            ((UniScanM.UI.InspectionPage)SystemManager.Instance().MainForm.InspectPage).UpdateStatusLabel();

            if (!OperationOption.Instance().OnTune && bChangedParameter == true)
            {
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                bChangedParameter = false;
            }
        }

        void UpdateParamControl()
        {
            bool flag = !OperationOption.Instance().OnTune;
            checkOnTune.Text = flag ? StringManager.GetString("Comm is opened") : StringManager.GetString("Comm is closed");

            groupThresHold.Enabled = !flag;
            buttonSave.Enabled = (!flag);// && (EDMS.Settings.EDMSSettings.Instance().AutoLight == false);
            groupLightParameter.Enabled = (!flag) && (EDMSW.Settings.EDMSSettings.Instance().AutoLight == false);
            cb_Preset.Enabled = label_Preset.Enabled =!flag;
        }

        private void InspectionPanelRight_Load(object sender, EventArgs e)
        {
            UpdateModelData();
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                //public delegate void (OpState curOpState, OpState prevOpState);
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            if (curOpState == OpState.Idle)
            {
                labelState.ForeColor = SystemColors.ControlText;
                labelState.BackColor = SystemColors.Control;
                labelState.Text = "";
                progressBarZeroing.Value = 0;
                return;
            }
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void InspectionPanelRight_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible)
            {
                tableLayoutPanelCurrentValue.Controls.Clear();
                if (EDMSSettings.Instance().IsFrontPosition)
                {
                    tableLayoutPanelCurrentValue.Controls.Add(labelFilm, 3, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelFilmValue, 3, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label1, 2, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelCoatingValue, 2, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label2, 1, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelPrintingValue, 1, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label6, 0, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelPrintingPos, 0, 1);
                }
                else
                {
                    tableLayoutPanelCurrentValue.Controls.Add(labelFilm, 0, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelFilmValue, 0, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label1, 1, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelCoatingValue, 1, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label2, 2, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelPrintingValue, 2, 1);

                    tableLayoutPanelCurrentValue.Controls.Add(label6, 3, 0);
                    tableLayoutPanelCurrentValue.Controls.Add(labelPrintingPos, 3, 1);
                }
            }
        }
    }
}
