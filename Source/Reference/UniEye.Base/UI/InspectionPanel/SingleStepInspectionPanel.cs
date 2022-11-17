using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspData;
using UniEye.Base.Settings;

namespace UniEye.Base.UI.InspectionPanel
{
    public partial class SingleStepInspectionPanel : UserControl, IInspectionPanel
    {
        List<InspectionResult> inspectionResultList = new List<InspectionResult>();

        int fontSize = 40;

        private DrawBox[] resultView;

        public SingleStepInspectionPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor Inspection Page");

            InitializeComponent();
            InitResultViewPanel();

            LogHelper.Debug(LoggerType.StartUp, "End Constructor Inspection Page");
        }

        public void InitResultViewPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Single Step Inspection Init Result View Panel");

            int numOfResultView = CustomizeSettings.Instance().NumOfResultView;

            resultView = new DrawBox[numOfResultView];

            resultViewPanel.ColumnStyles.Clear();
            resultViewPanel.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));
            resultViewPanel.ColumnCount = numCount;
            resultViewPanel.RowCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));

            for (int i = 0; i < resultViewPanel.ColumnCount; i++)
            {
                resultViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / resultViewPanel.ColumnCount));
            }

            for (int i = 0; i < resultViewPanel.RowCount; i++)
            {
                resultViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / resultViewPanel.RowCount));
            }

            for (int i = 0; i < numOfResultView; i++)
            {
                this.resultView[i] = new DrawBox();

                this.resultView[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.resultView[i].Dock = System.Windows.Forms.DockStyle.Fill;
                this.resultView[i].Location = new System.Drawing.Point(3, 3);
                this.resultView[i].Name = "targetImage";
                this.resultView[i].Size = new System.Drawing.Size(409, 523);
                this.resultView[i].TabIndex = 8;
                this.resultView[i].TabStop = false;
                this.resultView[i].Enable = false;
                this.resultView[i].MeasureMode = true;
                this.resultView[i].ShowTooltip = true;

                int rowIndex = i / numCount;
                int colIndex = i % numCount;

                resultViewPanel.Controls.Add(resultView[i], colIndex, rowIndex);
            }
        }

        public void Initialize()
        {

        }

        public void ClearPanel()
        {
            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPanel - ClearPanel");

            for (int i = 0; i < CustomizeSettings.Instance().NumOfResultView; i++)
            {
                resultView[i].TempFigureGroup.Clear();
                ImageHelper.Clear(resultView[i].Image as Bitmap, 0);
            }
        }

        delegate void PrepareInspectionDelegate();

        public void PrepareInspection()
        {
            if (InvokeRequired)
            {
                Invoke(new PrepareInspectionDelegate(PrepareInspection));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - PrepareInspection");

            for (int i = 0; i < resultView.Count(); i++)
            {
                ImageHelper.Clear((Bitmap)resultView[i].Image, 0);

                FigureGroup tempFigureGroup = new FigureGroup();

                TextFigure inspectionTextFigure = new TextFigure("Inspecting...", new Point(Parent.Width - fontSize, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Orange);

                inspectionTextFigure.FigureProperty.Alignment = StringAlignment.Near;

                //tempFigureGroup.AddFigure(inspectionTextFigure);

                resultView[i].ShowCenterGuide = OperationSettings.Instance().ShowCenterGuide;
                resultView[i].TempFigureGroup = tempFigureGroup;

                resultView[i].Invalidate();
            }
        }

        public void InspectionFinished(object hintObj)
        {

        }

        private void ShowCount()
        {

        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int inspectCount, InspectionResult inspectionResult)
        {

        }

        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {

        }

        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
        {

        }

        delegate void ProductInspectedDelegate(InspectionResult inspectionResult);

        public void ProductInspected(InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "Add InspectionResultList");

                inspectionResultList.Add(inspectionResult);

                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke ProductInspected");
                BeginInvoke(new ProductInspectedDelegate(ProductInspected), inspectionResult);

                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ProductInspected");

            ShowResult(inspectionResult);

            if (OperationSettings.Instance().UseDefectReview == true)
            {
                if (inspectionResult.IsGood() == false)
                {
                    DefectReviewForm form = new DefectReviewForm();
                    form.Initialize(inspectionResult);
                    form.Location = PointToScreen(Location);
                    form.Size = Size;

                    form.ShowDialog(this);

                    ShowResult(inspectionResult);
                }
            }
        }

        private void ShowResult(InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ShowResult");

            int numOfResultView = CustomizeSettings.Instance().NumOfResultView;

            for (int groupIndex = 0; groupIndex < inspectionResult.GrabImageList.Count; groupIndex++)
            {
                int viewIndex = Math.Min(numOfResultView - 1, groupIndex);

                InspectionResult targetInspectionResult = new InspectionResult();
                inspectionResult.GetTargetGroupResult("00", groupIndex, targetInspectionResult, true);

                ShowImageResult(targetInspectionResult, viewIndex, inspectionResult.GrabImageList[groupIndex]);
            }
        }

        private void ShowImageResult(InspectionResult inspectionResult, int viewIndex, ImageD grabImage)
        {
            if (grabImage == null)
                return;

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - ShowImageResult");

            Bitmap preBitmap = (Bitmap)resultView[viewIndex].Image;

            resultView[viewIndex].UpdateImage(grabImage.ToBitmap());

            if (preBitmap != null)
                preBitmap.Dispose();

            FigureGroup tempFigureGroup = new FigureGroup();
            inspectionResult.AppendResultFigures(tempFigureGroup);

            TextFigure overallResultTextFigure = null;

            if (inspectionResult.ProbeResultList.Count == 0)
            {
                overallResultTextFigure = new TextFigure("Not Trained", new Point(Parent.Width - fontSize, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Orange);
            }
            else if (inspectionResult.Judgment == Judgment.Accept)
            {
                overallResultTextFigure = new TextFigure("Good", new Point(Parent.Width - fontSize + 800, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.LightGreen);
            }
            else if (inspectionResult.Judgment == Judgment.FalseReject)
            {
                overallResultTextFigure = new TextFigure("Overkill", new Point(Parent.Width - fontSize + 450, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Yellow);
            }
            else
            {
                overallResultTextFigure = new TextFigure("NG", new Point(Parent.Width - fontSize + 800, 10), new Font(FontFamily.GenericSansSerif, fontSize), Color.Red);
            }
            //tempFigureGroup.AddFigure(overallResultTextFigure);

            LogHelper.Debug(LoggerType.Inspection, string.Format("Result[{0}] image Text is \"{1}\"", viewIndex, overallResultTextFigure.Text));

            if (OperationSettings.Instance().ShowScore)
            {
                foreach (ProbeResult probeResult in inspectionResult.ProbeResultList)
                {
                    Probe probe = probeResult.Probe;
                    ProbeResultValue probeResultValue = probeResult.ResultValueList.GetResultValue("Value");
                    string valueStr = probeResultValue.Value.ToString();

                    Color penColor = new Color();
                    if (probeResult.Judgment == Judgment.Accept)
                        penColor = Color.LightGreen;
                    else if (probeResult.Judgment == Judgment.FalseReject)
                        penColor = Color.Yellow;
                    else
                        penColor = Color.Red;

                    TextFigure tempTextFigure = new TextFigure(valueStr, new Point((int)probe.BaseRegion.X, (int)probe.BaseRegion.Y - 70),
                                                                    new Font(FontFamily.GenericSansSerif, 40, FontStyle.Bold), penColor);
                    tempFigureGroup.AddFigure(tempTextFigure);
                }
            }

            resultView[viewIndex].ShowCenterGuide = OperationSettings.Instance().ShowCenterGuide;
            resultView[viewIndex].TempFigureGroup = tempFigureGroup;

            resultView[viewIndex].Invalidate();
            resultView[viewIndex].Update();

            if (OperationSettings.Instance().SaveResultFigure)
            {
                string fileName = String.Format("{0}\\Image_C{1:00}_S{2:000}_L{3:00}R.jpeg", inspectionResult.ResultPath, 0, 0, 0);
                resultView[viewIndex].SaveImage(fileName);
            }
        }

        delegate void UpdateImageDelegate(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult);
        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                LogHelper.Debug(LoggerType.Inspection, "UpdateImage InvokeRequired");

                Invoke(new UpdateImageDelegate(UpdateImage), deviceImageSet, groupId, inspectionResult);
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "SingleStepInspectionPage - UpdateImage2");

            int numOfCameraImage = Math.Min(CustomizeSettings.Instance().NumOfResultView, MachineSettings.Instance().NumCamera);
            for (int i = 0; i < numOfCameraImage; i++)
            {
                //ImageD cameraImage = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(i, 0).Image;
                ImageD cameraImage = deviceImageSet.ImageList2D[i];

                if (cameraImage != null)
                {
                    Bitmap preBitmap = (Bitmap)resultView[i].Image;

                    resultView[i].ZoomScale = new SizeF(-1, -1);
                    resultView[i].UpdateImage(cameraImage.ToBitmap());
                    resultView[i].Invalidate(true);
                    resultView[i].Update();

                    if (preBitmap != null)
                        preBitmap.Dispose();
                }
            }
        }

        public void EnterWaitInspection()
        {
            throw new NotImplementedException();
        }

        public void ExitWaitInspection()
        {
            throw new NotImplementedException();
        }

        public void OnPreInspection()
        {
            throw new NotImplementedException();
        }

        public void OnPostInspection()
        {
            throw new NotImplementedException();
        }

        public void ModelChanged(Model model = null)
        {
            throw new NotImplementedException();
        }

        public void InfomationChanged(object obj = null)
        {
            throw new NotImplementedException();
        }
    }
}
