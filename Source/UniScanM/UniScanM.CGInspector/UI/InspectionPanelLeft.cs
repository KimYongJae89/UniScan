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
using UniScanM.CGInspector.Operation;
using DynMvp.UI.Touch;
using System.Threading;
using Infragistics.Win.Misc;
using Infragistics.Win;
using System.Diagnostics;


namespace UniScanM.CGInspector.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        private CanvasPanel canvaspanel = null;
        private UniScanM.Data.FigureDrawOption figureDrawOption = null;

        Label[] positionIndicator = null;
        public UniScanM.CGInspector.Operation.InspectRunner InspectRunner { get => InspectRunner; set => InspectRunner = value; }

        public IInspectionPanel InspectionPanel
        {
            get { return this; }
        }

        public Size GetImageViewSize()
        {
            return canvaspanel.Size;
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.canvaspanel = new CanvasPanel();
            this.canvaspanel.Dock = DockStyle.Fill;
            this.canvaspanel.SetPanMode();
            //this.canvaspanel.AutoSize = AutoFitStyle.KeepRatio;

            this.imagePanel.Controls.Add(this.canvaspanel);

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
            canvaspanel.BackColor = canvaspanel.Image == null ? Color.Gray : Color.DarkViolet;
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            canvaspanel.Clear();
            Bitmap displayBitmap = null;


            canvaspanel.UpdateImage(displayBitmap);
            canvaspanel.ZoomFit();
            
        }

        private Size GetClipSize(Size imageSize)
        {
            Size displaySize = canvaspanel.Size;
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

        public void ClearPanel()
        {
            this.canvaspanel.Clear();
            this.canvaspanel.UpdateImage(null);
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
