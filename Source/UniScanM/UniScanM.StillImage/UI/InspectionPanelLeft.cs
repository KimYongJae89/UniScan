using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using UniScanM.StillImage.Operation;
using DynMvp.UI.Touch;
using System.Threading;
using Infragistics.Win.Misc;
using Infragistics.Win;
using System.Diagnostics;


namespace UniScanM.StillImage.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        private DrawBox drawBox = null;
        private UniScanM.Data.FigureDrawOption figureDrawOption = null;

        Label[] positionIndicator = null;
        
        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        public Size GetImageViewSize()
        {
            return  drawBox.Size;
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.drawBox = new DrawBox();
            this.drawBox.Dock = DockStyle.Fill;
            this.drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
            this.imagePanel.Controls.Add(this.drawBox);

            this.positionIndicator = new Label[6] { this.labelZone1, this.labelZone2, this.labelZone3, this.labelZone4, this.labelZone5, this.labelZone6 };

            figureDrawOption = new UniScanM.Data.FigureDrawOption()
            {
                useTargetCoord = false,

                PatternConnection = false,

                TeachResult = new UniScanM.Data.FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
                    Invalid = new UniScanM.Data.DrawSet(null, null),

                    ShowText = false,
                    FontSet = new UniScanM.Data.FontSet(new Font("Gulim", 20), Color.Yellow)
                },

                ProcessResult = new UniScanM.Data.FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
                    Invalid = new UniScanM.Data.DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0xFF, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0xFF, 0x00))),

                    ShowText = false,
                    FontSet = new UniScanM.Data.FontSet(new Font("Gulim", 20), Color.Red)
                }
            };
            
            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;

            StringManager.AddListener(this);
        }

        private void ErrorManager_OnStartAlarm()
        {
        }

        private void MonitoringPage_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < this.positionIndicator.Length; i++)
            {
                this.positionIndicator[i].Font = UiHelper.AutoFontSize(this.positionIndicator[i], this.positionIndicator[i].Text);
                this.positionIndicator[i].BackColor = Color.LightSteelBlue;
            }
        }
        int nDisplayRevisionCount=0;
        public void ProductInspected(InspectionResult inspectionResult)
        {
            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            drawBox.FigureGroup.Clear();
            Bitmap displayBitmap = null;

            int inspectZone = myInspectionResult.InspZone;
            string inspectState = myInspectionResult.InspectState;
            if (myInspectionResult.DisplayBitmap != null)
            {
                try //어디선가 사용중인경우 에러메세지 표시 됨 -> 일단 에러창을 보이지않게 처리.
                {
                    //Bitmap.Clone() 메서드는 새 비트맵 개체를 생성하지만 픽셀 데이터는 원래 비트맵 개체와 공유
                    displayBitmap = myInspectionResult.DisplayBitmap.Clone(
                        myInspectionResult.DisplayImageRect,
                        myInspectionResult.DisplayBitmap.PixelFormat);
                }
                catch(Exception e)
                {
                    LogHelper.Debug(LoggerType.Error, $"UniScanM.StillImage.UI.InspectionPanelLeft.ProductInspected():{e}");
                }

            }

            if (inspectState == "Monitoring")// In Monitoring State
            {
                UpdateZoneLabel(-1, Color.Empty);
            }
            else if (inspectState == "Inspection")// In Inspection State
            {
                if(inspectionResult.Judgment == Judgment.Skip)
                // sheet not founded 
                {
                    UpdateZoneLabel(inspectZone, Color.Yellow);
                    return;
                }

                Data.ProcessResult interestProcessResult = myInspectionResult.ProcessResultList.InterestProcessResult;
                if (interestProcessResult != null)
                {
                    // Draw Figure
                    FigureGroup figureGroup = new FigureGroup();

                    Color blotFigColor = interestProcessResult.IsBlotGood ? Color.LightGreen : Color.Red;
                    //RectangleF figRect = interestProcessResult.InspPatternInfo.ShapeInfo.BaseRect;
                    RectangleF blotFigRect = myInspectionResult.BlotRectInInsp;
                    figureGroup.AddFigure(new RectangleFigure(blotFigRect, new Pen(blotFigColor, 3)));

                    Color marginFigColor = interestProcessResult.IsMarginGood ? Color.Green : Color.Red;
                    //figRect.Inflate(interestProcessResult.InspPatternInfo.TeachInfo.Feature.Margin);
                    RectangleF marginFigRect = myInspectionResult.MarginRectInInsp;
                    figureGroup.AddFigure(new RectangleFigure(marginFigRect, new Pen(marginFigColor, 3)));

                 
                    foreach (Rectangle defRect in myInspectionResult.ProcessResultList.DefectRectList)
                    {
                        defRect.Inflate(10, 10);
                        figureGroup.AddFigure(new RectangleFigure(defRect, new Pen(Color.Red, 3)));
                    }
                    
                    figureGroup.Offset(-myInspectionResult.DisplayImageRect.X, -myInspectionResult.DisplayImageRect.Y);
                    //myInspectionResult.UpdateJudgement();

                    /*
                    ///화면에 Revision 숫자 표시
                    nDisplayRevisionCount++;
                    if (nDisplayRevisionCount >= 100) nDisplayRevisionCount = 0;
                    TextFigure text = new TextFigure(nDisplayRevisionCount.ToString(), new Point(100, 100), new Font(FontFamily.GenericSerif, 100), Color.Yellow); ;
                    figureGroup.AddFigure(text);
                    //화면에 프레임 intensity 표시 */


                    drawBox.FigureGroup.AddFigure(figureGroup);
                }

                Rectangle defectRect = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect();
                bool isGood = (interestProcessResult == null ? false : interestProcessResult.IsGood) && defectRect.IsEmpty;
                Color color = isGood ? Color.Green : Color.Red;
                UpdateZoneLabel(inspectZone, color);
            }
            else if(inspectState == "Teaching")
            {
                UpdateZoneLabel(-1, Color.Empty);
            }
            else if(inspectState == "LightTune")
            {
                UpdateZoneLabel(-1, Color.Empty);
            }

            drawBox.UpdateImage(displayBitmap);
            drawBox.ZoomFit();
        }

        private Size GetClipSize(Size imageSize)
        {
            Size displaySize = drawBox.Size;
            float rX = imageSize.Width * 1.0f / displaySize.Width * 1.0f;
            float rY = imageSize.Height * 1.0f / displaySize.Height * 1.0f;
            float ratio = Math.Min(rX, rY);
            Size clipImageSize = Size.Round(new SizeF(displaySize.Width * ratio, displaySize.Height * ratio));
            return clipImageSize;
        }

        delegate void UpdateImageDelegate(Bitmap bitmap);
        private void UpdateImage(Bitmap bitmap)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateImageDelegate(UpdateImage), bitmap);
                return;
            }
         
        }

        private void UpdateZoneLabel(int zoneNo, Color color)
        {
            if (zoneNo >= 0)
                zoneNo %= positionIndicator.Length;

            for (int i = 0; i < positionIndicator.Length; i++)
            {
                if (i == zoneNo)
                    positionIndicator[i].BackColor = color;
                else
                    positionIndicator[i].BackColor = Color.LightSteelBlue;
            }
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }

        public void ClearPanel() {
            this.drawBox.ClearFigure();
            this.drawBox.UpdateImage(null);
            nDisplayRevisionCount = 0;
        }

        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
    }
}
