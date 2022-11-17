using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.Device;
using UniEye.Base;
using DynMvp.Devices;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.InspData;
using UniScanM.Pinhole.Data;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;
using System.Reflection;
using DynMvp.Devices.Light;
using UniScanM.Pinhole.UI.MenuPage;
using System.Threading;
using UniScanM.Pinhole.Settings;
using UniScanM.Pinhole.UI.MenuPanel;

using System.Collections.Concurrent;
using System.Diagnostics;

namespace UniScanM.Pinhole.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        static int numOfResultView = 2;
        CanvasPanel[] canvasPanel;
        LastDefectPanel[] lastDefectPanel;
        ConcurrentQueue<DynMvp.InspData.InspectionResult> m_ResultQue;

        int lastUpdatedCamIndex = -1;
                
        private Judgment leftState;
        private Judgment rightState;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            InitViewPanel();
            InitLastDefectPanel();
            timer_GUIRevison.Interval = 200;

            StringManager.AddListener(this);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        
        void InitLastDefectPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanel Start.");

            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            ImageDeviceHandler imageDeviceHandler = deviceBox.ImageDeviceHandler;

            lastDefectPanel = new LastDefectPanel[numOfResultView];
            for (int i = 0; i < numOfResultView; i++)
            {
                //Task task = Task.Factory.StartNew(() => this.lastDefectPanel[i] = new LastDefectPanel(i + 1));
                //task.Wait();
                this.lastDefectPanel[i] = new LastDefectPanel(i+1);
                this.lastDefectPanel[i].Dock = DockStyle.Fill;
                this.lastDefectPanel[i].Margin = new Padding(0, 0, 0, 0);
                layoutLastImage.Controls.Add(lastDefectPanel[i], i, 0);
            }

            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanelEnd.");
        }

        private void InitViewPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init Result View Panel Start.");

            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            ImageDeviceHandler imageDeviceHandler = deviceBox.ImageDeviceHandler;

            m_ResultQue = new ConcurrentQueue<DynMvp.InspData.InspectionResult>();

            canvasPanel = new CanvasPanel[numOfResultView];
            for (int i = 0; i < numOfResultView; i++)
            {
                //Task task = Task.Factory.StartNew(() => this.canvasPanel[i] = new CanvasPanel());
                //task.Wait();
                this.canvasPanel[i] = new CanvasPanel();
                this.canvasPanel[i].ShowCenterGuide = false;
                this.canvasPanel[i].ShowToolbar = false;
                this.canvasPanel[i].Dock = DockStyle.Fill;
                this.canvasPanel[i].ReadOnly = true;
                Image2D tempImage = new Image2D(2048, 1024, 3);
                if (i == 0)
                    panelCam1.Controls.Add(canvasPanel[i]);
                else
                    panelCam2.Controls.Add(canvasPanel[i]);
            }

            canvasPanel[0].HorizontalAlignment = HorizontalAlignment.Right;
            canvasPanel[1].HorizontalAlignment = HorizontalAlignment.Left;
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init Result View Panel End.");
        }
        
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            m_ResultQue.Enqueue(inspectionResult);
            timer_GUIRevison.Start();
//            UpdateResult4((Data.InspectionResult)inspectionResult);
        }

        IAsyncResult asyncUpdatePage;
        delegate void UpdateResultDelegate(Data.InspectionResult inspectResult);

        public void UpdateResult(Data.InspectionResult inspectResult)
        {
            if (InvokeRequired)
            {
                if (asyncUpdatePage != null)
                {
                    if (asyncUpdatePage.IsCompleted == false)
                    {
                        return;
                    }
                }

                asyncUpdatePage = BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectResult);// = BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectResult);
                return;
            }

            if (inspectResult == null)
                return;

            FigureGroup figureGroup = new FigureGroup();
            ///*
            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                Figure defectMark;
                RectangleF boundingRect = defectInfo.BoundingRect;
                if (boundingRect.Width < 10 || boundingRect.Height < 10)
                {
                    defectMark = new EllipseFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else if (boundingRect.Width > 1000 || boundingRect.Height > 1000)
                {
                    defectMark = new XRectFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else
                {
                    defectMark = new RectangleFigure(boundingRect, new Pen(Color.Red));
                }

                defectMark.Tag = defectInfo;

                figureGroup.AddFigure(defectMark);
            }
            //*/

            TextFigure textFigure;
            int point = 200;
            if (inspectResult.DeviceIndex == 1)
                point = 1848;

            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure(StringManager.GetString("OK"), new Point(point, 10), new Font("Arial", 150), Color.Green);
            else if (inspectResult.Judgment == Judgment.Skip)
                textFigure = new TextFigure(StringManager.GetString("Skip"), new Point(point, 10), new Font("Arial", 150), Color.Green);
            else
                textFigure = new TextFigure(StringManager.GetString("NG"), new Point(point, 10), new Font("Arial", 150), Color.Red);

            figureGroup.AddFigure(textFigure);
            figureGroup.Selectable = false;

            CanvasPanel curDrawBox = canvasPanel[inspectResult.DeviceIndex];
            //if (curDrawBox.Image == null)
                curDrawBox.UpdateImage(inspectResult.DisplayBitmap);

            curDrawBox.WorkingFigures.Clear();
            curDrawBox.WorkingFigures.AddFigure(figureGroup);
            curDrawBox.Invalidate();
        }


        bool[] onUpdateResult = new bool[2];

        delegate void UpdateResultListDelegate(List<Data.InspectionResult> inspectResult);

        void UpdateResult4(Data.InspectionResult inspectResult)
        {
            if (inspectResult == null)
                return;

            lastUpdatedCamIndex = inspectResult.DeviceIndex;

            FigureGroup figureGroup = new FigureGroup();

            TextFigure textFigure;
            int point = 40;
            if (inspectResult.DeviceIndex == 1)
                point = 360;
            //float zoomSize = 
            float resizeRatio = PinholeSettings.Instance().ResizeRatio;
            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure("OK", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Green);
            else if (inspectResult.Judgment == Judgment.Skip)
                textFigure = new TextFigure("Skip", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Green);
            else
                textFigure = new TextFigure("NG", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Red);

            PointF startPt;
            PointF endPt;
            LineFigure interestRegionFigure = null;
            if (inspectResult != null)
            {
                if(inspectResult.InterestRegion != null)
                {
                    if (inspectResult.DeviceIndex == 1)
                    {
                        startPt = new PointF(inspectResult.InterestRegion.Right * 0.2f, 0);
                        endPt = new PointF(inspectResult.InterestRegion.Right * 0.2f, inspectResult.InterestRegion.Height* 0.2f);
                    }
                    else
                    {
                        startPt = new PointF(inspectResult.InterestRegion.X * 0.2f, 0);
                        endPt = new PointF(inspectResult.InterestRegion.X * 0.2f, inspectResult.InterestRegion.Height*0.2f);
                    }
                    interestRegionFigure = new LineFigure(startPt, endPt, new Pen(Color.Yellow, 2));
                }
            }
            
            figureGroup.AddFigure(textFigure);
            figureGroup.Selectable = false;
            if (interestRegionFigure != null)
                figureGroup.AddFigure(interestRegionFigure);

            CanvasPanel _canvasPanel = canvasPanel[inspectResult.DeviceIndex];
            //lock(inspectResult.DisplayBitmap)
            Bitmap image = inspectResult.DisplayBitmap.Clone() as Bitmap;
            if(image !=null)
            {
                _canvasPanel.UpdateImage(image);
                _canvasPanel.WorkingFigures.Clear();
                _canvasPanel.WorkingFigures.AddFigure(figureGroup);
                _canvasPanel.ZoomFit();
            }
            lastDefectPanel[inspectResult.DeviceIndex].UpdateLastDefect(inspectResult);
        }

        void UpdateResultWithOutCanvasPanel(Data.InspectionResult inspectResult)
        {
            if (inspectResult == null)
                return;

            lastUpdatedCamIndex = inspectResult.DeviceIndex;

            lastDefectPanel[inspectResult.DeviceIndex].UpdateLastDefect(inspectResult);
        }
        
        private void process1_Exited(object sender, EventArgs e)
        {

        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if(InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            Array.ForEach(canvasPanel, f =>
            {
                f.Clear();
                f.UpdateImage(null);
            });

            Array.ForEach(lastDefectPanel, f =>
            {
                f.Clear();
            });
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

        private void timer_GUIRevison_Tick(object sender, EventArgs e)
        {
            timer_GUIRevison.Stop();

            DynMvp.InspData.InspectionResult result = null;
            int countQdata = m_ResultQue.Count();
            Debug.WriteLine("m_ResultQue.Count [{0}]", countQdata);

            if (countQdata == 0)
            {
                return;
            }
            //한개면 그냥 그리기
            else if (countQdata == 1)  
            {
               if( m_ResultQue.TryDequeue(out result) )
                {
                    UpdateResult4(result as Data.InspectionResult);
                }
            }
            //결과가 여러개 누적되었으면,  모두 OK 이면 맨마지막 OK, NG가 있으면 마지막 NG만
            else
            {
                List<DynMvp.InspData.InspectionResult> listResult = new List<DynMvp.InspData.InspectionResult>();
                while (m_ResultQue.TryDequeue(out result))
                    listResult.Add(result);

                //Left, Right Camera....
                List<DynMvp.InspData.InspectionResult>[] arrListResult = new List<DynMvp.InspData.InspectionResult>[2];
                arrListResult[0] = listResult.FindAll(f => ((f as Data.InspectionResult)).DeviceIndex == 0);
                arrListResult[1]   = listResult.FindAll(f =>( (f as Data.InspectionResult)).DeviceIndex == 1);
                //

                foreach (var list in arrListResult) //Left, Right
                {
                    var countAll = list.Count();
                    var countOK = list.Count(f => f.Judgment == Judgment.Accept  || f.Judgment == Judgment.Skip);
                    var countNG = list.Count(f => f.Judgment == Judgment.Reject);

                    if (countAll == 0) continue;

                    if (countAll == countOK) //모두다 정상이면 마지막꺼만
                    {
                        UpdateResult4 ( list.Last() as Data.InspectionResult);
                    }
                    else  //NG
                    {
                        var lastNgResult = list.FindLast(f => f.Judgment == Judgment.Reject);
                        UpdateResult4(lastNgResult as Data.InspectionResult);
                    }
                }
            }

        }// private void timer_GUIRevison_Tick(object sender, EventArgs e)
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static void DoubleBuffered(this Label label, bool setting)
        {
            Type dgvType = label.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(label, setting, null);
        }
    }
}
