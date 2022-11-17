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
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.UI.Touch;
using UniScanM.Algorithm;
using DynMvp.Vision;
using UniEye.Base.Settings;
using System.IO;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices;
using UniEye.Base.UI;
using UniScanM.Pinhole.Algorithm;
using DynMvp.InspData;
using UniScanM.Pinhole.Data;
using UniScanM.Pinhole.UI.ControlPanel;
using DynMvp.Devices.Light;
using System.Windows.Forms.DataVisualization.Charting;
using UniScanM.UI.MenuPage.AutoTune;
using UniScanM.Pinhole.Settings;
using DynMvp.Authentication;

namespace UniScanM.Pinhole.UI.MenuPanel
{
    public partial class TeachPanel : UserControl, ITeachPage, IMultiLanguageSupport
    {
        bool onUpdateData = false;

        enum OperationMode
        {
            Grab, Load
        }
        ProjectionData[] projectionData;
        Image2D[] curImageArray = null;
        private CanvasPanel[] canvasPanelArray;

        OperationMode mode = OperationMode.Grab;
        UniScanM.Pinhole.Data.Model model;
        PinholeChecker pinholeChecker = new PinholeChecker();

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        int numOfResultView = 2;

        public TeachPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            InitViewPanel();
            InitChartPanel();

            StringManager.AddListener(this);
        }

        public void InitViewPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "TeachPanel Result View Panel");

            
            canvasPanelArray = new CanvasPanel[numOfResultView];

            for (int i = 0; i < numOfResultView; i++)
            {
                this.canvasPanelArray[i] = new CanvasPanel();
                this.canvasPanelArray[i].ShowCenterGuide = false;
                this.canvasPanelArray[i].ShowToolbar = true;
                this.canvasPanelArray[i].ShowModeToolBar = false;
                this.canvasPanelArray[i].ShowShapeToolBar = false;
                this.canvasPanelArray[i].ShowZoomToolBar = false;

                this.canvasPanelArray[i].SetPanMode();
                this.canvasPanelArray[i].ShowZoomToolBar = true;

                this.canvasPanelArray[i].BackColor = Color.Black;
                this.canvasPanelArray[i].Dock = DockStyle.Fill;

                layoutLeft.Controls.Add(canvasPanelArray[i], i, 0);
            }

            curImageArray = new Image2D[numOfResultView];
        }

        public void InitChartPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "TeachPanel 2 Result View Panel");

            int numOfResultView = 2;

            projectionData = new ProjectionData[numOfResultView];

            for (int i = 0; i < numOfResultView; i++)
            {
                this.projectionData[i] = new ProjectionData(i);
                this.projectionData[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
                this.projectionData[i].Dock = DockStyle.Fill;
                this.projectionData[i].Location = new System.Drawing.Point(3, 3);
                this.projectionData[i].Name = "targetImage";
                this.projectionData[i].Size = new System.Drawing.Size(409, 523);
                this.projectionData[i].TabIndex = 8;
                this.projectionData[i].TabStop = false;
            }            
        }
        
        private void numLightValue_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void buttonInspect_Click(object sender, EventArgs e)
        {
            defectList.Rows.Clear();

            for(int i = 0; i < numOfResultView; i++)
            {
                Inspect(i);
            }
        }

        private void Inspect(int deviceIndex)
        {
            Image2D image = curImageArray[deviceIndex];

            if (image == null)
            {
                LogHelper.Debug(LoggerType.Operation, String.Format("TeachPane::Inspect - {0}, CurrentAlgoImage is null ", deviceIndex));                
                return;
            }
            
            LogHelper.Debug(LoggerType.Operation, String.Format("TeachPane::Inspect - {0}, START ", deviceIndex));
            
            Data.InspectionResult inspecitonResult = new Data.InspectionResult();
            inspecitonResult.DeviceIndex = deviceIndex;
            
            inspecitonResult.DisplayBitmap = image.ToBitmap();
            inspecitonResult.SaveBitmap = image.ToBitmap();
            inspecitonResult = pinholeChecker.Inspect(image, inspecitonResult);

            UpdateDefectList(inspecitonResult);
            UpdateFigure(inspecitonResult);
            LogHelper.Debug(LoggerType.Operation, String.Format("TeachPane::Inspect - {0}, END ", deviceIndex));
        }

        void UpdateDefectList(Data.InspectionResult inspectResult)
        {
            int rowHeight = (defectList.Height - defectList.ColumnHeadersHeight) / 3;

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewTextBoxCell camIndexCell = new DataGridViewTextBoxCell() { Value = inspectResult.DeviceIndex + 1 };
                DataGridViewTextBoxCell typeCell = new DataGridViewTextBoxCell() { Value = defectInfo.DefectType };
                DataGridViewTextBoxCell infoCell = new DataGridViewTextBoxCell() { Value = defectInfo.ToString() };
                DataGridViewImageCell imageCell = new DataGridViewImageCell() { Value = defectInfo.ClipImage };
                imageCell.ImageLayout = DataGridViewImageCellLayout.Zoom;

                row.Cells.Add(camIndexCell);
                row.Cells.Add(typeCell);
                row.Cells.Add(infoCell);
                row.Cells.Add(imageCell);
                
                row.Tag = defectInfo;

                row.Height = rowHeight;

                rowList.Add(row);
            }

            defectList.Rows.AddRange(rowList.ToArray());
        }

        void UpdateFigure(Data.InspectionResult inspectResult)
        {
            FigureGroup figureGroup = new FigureGroup();

            Color skipColor = Color.FromArgb(50, Color.Yellow);
            figureGroup.AddFigure(new RectangleFigure(inspectResult.SkipRegion, new Pen(skipColor, 1), new SolidBrush(skipColor)));

            Color interestColor =  Color.FromArgb(50, Color.LightGreen);
            figureGroup.AddFigure(new RectangleFigure(inspectResult.InterestRegion, new Pen(interestColor, 1), new SolidBrush(interestColor)));

            Pen pen = new Pen(Color.Red, 1.0F);
            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                //if (defectInfo.DefectType == Data.DefectType.Dust)
                //    defectMark = new XRectFigure(DrawingHelper.CenterPoint(defectInfo.BoundingRect), 40, new Pen(Color.Red));
                //else
                //    defectMark = new EllipseFigure(DrawingHelper.CenterPoint(defectInfo.BoundingRect), 40, new Pen(Color.Red));
                
                figureGroup.AddFigure(new RectangleFigure(defectInfo.BoundingRect, pen));
            }

            TextFigure textFigure;
            int point = 200;
            if (inspectResult.DeviceIndex == 1)
                point = 1848;

            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure("OK", new Point(point, 10), new Font("Arial", 150), Color.Green);
            else
                textFigure = new TextFigure("NG", new Point(point, 10), new Font("Arial", 150), Color.Red);

            //figureGroup.AddFigure(textFigure);
            canvasPanelArray[inspectResult.DeviceIndex].TempFigures.Clear();
            canvasPanelArray[inspectResult.DeviceIndex].TempFigures.AddFigure(figureGroup);
            canvasPanelArray[inspectResult.DeviceIndex].Invalidate();
        }

        void ClearDrawBox()
        {
            for(int i = 0; i < numOfResultView; i++)
            {
                canvasPanelArray[i].BackgroundFigures.Clear();
                canvasPanelArray[i].TempFigures.Clear();
                canvasPanelArray[i].Invalidate();
            }
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            mode = OperationMode.Grab;
            //조명값을 설정

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);
            //그랩
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.Stop();
            imageDeviceHandler.Reset();
            foreach (ImageDevice imageDevce in imageDeviceHandler)
            {
                imageDevce.ImageGrabbed += ImageGrabbed;
            }
            imageDeviceHandler.GrabOnce();
        }

        private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            imageDevice.ImageGrabbed -= ImageGrabbed;

            Task.Run(() => WaitGrabDone(imageDevice));

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            Image2D image = (Image2D)imageDevice.GetGrabbedImage(ptr);

            if (image.IsUseIntPtr() == true)
                image.ConvertFromPtr();

            Image2D cloneImage = (Image2D)image.Clone();
            UpdateImage(imageDevice.Index, cloneImage);
            
            if (SystemManager.Instance().CurrentModel == null)
                return;

            string imagePath = SystemManager.Instance().CurrentModel.GetImagePathName(imageDevice.Index, 0, 0);
            image.SaveImage(imagePath);

            ShowHisto(imageDevice.Index);
        }

        void UpdateImage(int index, Image2D image)
        {
            if (curImageArray[index] != null)
            {
                curImageArray[index].Dispose();
                curImageArray[index] = null;
            }

            curImageArray[index] = image;
                
            canvasPanelArray[index].UpdateImage(image.ToBitmap());
            canvasPanelArray[index].ZoomFit();
        }

        private void buttonLoadImage_VisibleChanged(object sender, EventArgs e)
        {
            model = (UniScanM.Pinhole.Data.Model)SystemManager.Instance().CurrentModel;
        }
        
        void UpdateData(List<ImageD> imageList)
        {
            onUpdateData = true;

            model = (UniScanM.Pinhole.Data.Model)SystemManager.Instance().CurrentModel;
            
            skipRange.Value = (int)PinholeSettings.Instance().SkipLength;

            lightValue.Value = model.LightParamSet.LightParamList[0].LightValue.Value[0];

            binarize1.Value = model.InspectParam.PinholeParamValue1.EdgeThreshold;
            defectTh1.Value = model.InspectParam.PinholeParamValue1.DefectThreshold;

            binarize2.Value = model.InspectParam.PinholeParamValue2.EdgeThreshold;
            defectTh2.Value = model.InspectParam.PinholeParamValue2.DefectThreshold;

            ShowHisto(0);
            ShowHisto(1);

            onUpdateData = false;
        }
        

        void UpdateModel()
        {
            if (onUpdateData == true)
                return;

            model = (UniScanM.Pinhole.Data.Model)SystemManager.Instance().CurrentModel;
            model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)lightValue.Value;

            model.InspectParam.PinholeParamValue1.EdgeThreshold = (int)binarize1.Value;
            model.InspectParam.PinholeParamValue1.DefectThreshold = (int)defectTh1.Value;

            model.InspectParam.PinholeParamValue2.EdgeThreshold = (int)binarize2.Value;
            model.InspectParam.PinholeParamValue2.DefectThreshold = (int)defectTh2.Value;
            
            PinholeSettings.Instance().SkipLength = (int)skipRange.Value;
            
            SystemManager.Instance().ModelManager.SaveModel(model);

            ShowHisto(0);
            ShowHisto(1);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            UpdateModel();
        }

        private void TeachPanel_Load(object sender, EventArgs e)
        {
            UpdateData(null);
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)lightValue.Value;

            SystemManager.Instance().ModelManager.SaveModel(model);
        }

        public void UpdateControl(string item, object value)
        {

        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            UpdateData(null);
        }

        public void EnableControls(UserType userType)
        {
            
        }

        private void TeachPanel_VisibleChanged(object sender, EventArgs e)
        {
            if(this.Visible == true)
                UpdateData(null);
        }
        
        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        private void AutoSet(int deviceIndex)
        {
            Image2D image = curImageArray[deviceIndex];

            if (image == null)
                return;

            LogHelper.Debug(LoggerType.Operation, String.Format("TeachPanel::Teach - Cam {0}", deviceIndex));

            //Teach 결과 리턴
            //bool result = pinholeChecker.AutoSet(deviceIndex, image);

            //SystemManager.Instance().ModelManager.SaveModel(model);

            //UpdateData(null);
        }

        delegate void ShowHistoDelegate(int deviceIndex);
        private void ShowHisto(int deviceIndex)
        {
            
            if (curImageArray[deviceIndex] == null)
                return;

            if (InvokeRequired)
            {
                Invoke(new ShowHistoDelegate(ShowHisto), deviceIndex);
                return;
            }

            Chart chart = null;
            if (deviceIndex == 0)
                chart = histo1;
            else
                chart = histo2;

            long[] histo = pinholeChecker.GetHisto(curImageArray[deviceIndex]);
            long maxValue = 0;
            
            chart.Series[0].Points.Clear();
            
            for (int i = 0; i < 255; i++)
            {
                chart.Series[0].Points.AddXY(i, histo[i] + 1); //빨리 안하냐? 죄송합니다ㅜㅜ

                if (maxValue < histo[i])
                    maxValue = histo[i];
            }
        }

        private void buttonAutoSet1_Click(object sender, EventArgs e)
        {
            AutoSet(0);
            AutoSet(1);
        }
        
        private void ValueChanged(object sender, EventArgs e)
        {
            UpdateModel();
            Preview();
        }

        private void PreviewCheckChanged(object sender, EventArgs e)
        {
            Preview();
        }

        private void Preview()
        {
            //for (int i = 0; i < numOfResultView; i++)
            //{
            //    if (curImageArray[i] == null)
            //        continue;
                
            //    ImageD prevImage = null;
            //    if (i == 0)
            //        prevImage = pinholeChecker.GetPrevImage(i, curImageArray[i], previewBinarize1.Checked, previewDefectTh1.Checked);
            //    else if (i == 1)
            //        prevImage = pinholeChecker.GetPrevImage(i, curImageArray[i], previewBinarize2.Checked, previewDefectTh2.Checked);

            //    canvasPanelArray[i].UpdateImage(prevImage.ToBitmap());
            //}
        }
        
        private void buttonAutoTune_Click(object sender, EventArgs e)
        {
            AutoTuneForm autoTuneForm = new AutoTuneForm(UpdateData, AutoTuneType.Otsu, null);
            autoTuneForm.ShowDialog();
            //stdList[0] = new List<float>();
            //stdList[1] = new List<float>();

            //mode = OperationMode.Grab;
            ////조명값을 설정

            //LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;

            //model.LightParamSet.LightParamList[0].LightValue.Value[0] = 1;
            //lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);
            ////그랩
            //ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            //imageDeviceHandler.Stop();
            //imageDeviceHandler.Reset();
            //foreach (ImageDevice imageDevce in imageDeviceHandler)
            //    imageDevce.ImageGrabbed += AutoTuneGrabbed;

            //imageDeviceHandler.GrabOnce();
            //bool gdone = imageDeviceHandler.WaitGrabDone(5000);
        }

        private void AutoTuneGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            //ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            //if (model.LightParamSet.LightParamList[0].LightValue.Value[0] >= 255)
            //{
            //    foreach (ImageDevice imageDevce in imageDeviceHandler)
            //        imageDevce.ImageGrabbed -= AutoTuneGrabbed;

            //    imageDeviceHandler.Stop();
            //    imageDeviceHandler.Reset();

            //    AdjustLightParam(imageDevice.Index);
            //    return;
            //}

            //Image2D image = (Image2D)imageDevice.GetGrabbedImage(ptr);
            //stdList[imageDevice.Index].Add(pinholeChecker.GetSubStd(image));
            
            //LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            //model.LightParamSet.LightParamList[0].LightValue.Value[0]++;
            //lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);

            //Task.Run(() => WaitGrabDone(imageDevice));
        }

        public void WaitGrabDone(ImageDevice imageDevice)
        {
            //SimpleProgressForm simpleProgressForm = new SimpleProgressForm(string.Format("Wait Cam[{0}]", imageDevice.Index + 1));
            //simpleProgressForm.Show(() =>
            //{
            //    while (SystemManager.Instance().DeviceBox.ImageDeviceHandler.IsGrabDone(imageDevice.Index) == false)
            //        System.Threading.Thread.Sleep(0);
                
            //    SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabOnce(imageDevice.Index);
            //});
        }

        private void AdjustLightParam(int index)
        {
        }

        private void layoutCam2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
