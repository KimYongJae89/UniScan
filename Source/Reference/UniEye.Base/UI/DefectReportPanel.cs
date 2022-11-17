using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System.Media;
using System.Threading;
using DynMvp.InspData;
using System.IO;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class DefectReportPanel : Form, IDefectReportPanel
    {
        private InspectionResult inspectionResult;
        private TargetView targetResultImage;
        private TargetView targetGoodImage;
        private TargetView targetCameraImage;

        private int curIndex = 0;

        private SchemaViewer schemaView;
        public SchemaViewer SchemaView
        {
            get { return schemaView; }
        }

        ReportMode reportMode;
        bool onUpdateData;

        public DefectReportPanel()
        {
            InitializeComponent();

            this.schemaView = new SchemaViewer();

            // 
            // schemaView
            // 
            this.schemaView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaView.Location = new System.Drawing.Point(620, 47);
            this.schemaView.Name = "schemaView";
            this.schemaView.Size = new System.Drawing.Size(683, 489);
            this.schemaView.TabIndex = 177;
            this.schemaView.TabStop = false;

            this.panelSchemaView.Controls.Add(this.schemaView);

            this.targetGoodImage = new TargetView();
            this.panelProbeGoodImage.Controls.Add(this.targetGoodImage);

            this.targetResultImage = new TargetView();
            this.panelProbeNgImage.Controls.Add(this.targetResultImage);

            this.targetCameraImage = new TargetView();
            this.panelTargetGroupImage.Controls.Add(this.targetCameraImage);

            targetResultImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetResultImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetResultImage.Location = new System.Drawing.Point(3, 3);
            targetResultImage.Name = "targetViewNg";
            targetResultImage.Size = new System.Drawing.Size(409, 523);
            targetResultImage.TabStop = false;
            targetResultImage.Enable = false;

            targetGoodImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetGoodImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetGoodImage.Location = new System.Drawing.Point(3, 3);
            targetGoodImage.Name = "targetViewGood";
            targetGoodImage.Size = new System.Drawing.Size(409, 523);
            targetGoodImage.TabStop = false;
            targetGoodImage.Enable = false;

            targetCameraImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetCameraImage.Dock = System.Windows.Forms.DockStyle.Fill;
            targetCameraImage.Location = new System.Drawing.Point(3, 3);
            targetCameraImage.Name = "targetGroupView";
            targetCameraImage.Size = new System.Drawing.Size(409, 523);
            targetCameraImage.TabStop = false;
            targetCameraImage.Enable = false;
        }

        public void Initialize(ReportMode reportMode, Model model, InspectionResult inspectionResult)
        {
            if (model != null && model.ModelSchema != null)
            {
                schemaView.Schema = model.ModelSchema.Clone();
                schemaView.Schema.AutoFit = true;
            }

            this.inspectionResult = inspectionResult;

            onUpdateData = true;

            showNgPad.Checked = true;
            showGoodPad.Checked = true;

            onUpdateData = false;
        }

        private void DefectReportPanel_Load(object sender, EventArgs e)
        {
            if (inspectionResult != null)
            {
                UpdateData(inspectionResult, null);
            }
        }

        public void UpdateData(InspectionResult inspectionResult, Schema schema)
        {
            if (schema != null)
            {
                schemaView.Schema = schema.Clone();
                schemaView.Schema.AutoFit = true;
            }

            UpdateProbeList(inspectionResult);
        }

        public void UpdateProbeList(InspectionResult inspectionResult)
        {
            this.inspectionResult = inspectionResult;

            UpdateProbeList();
            if (probeResultList.Rows.Count > 0)
                probeResultList.Rows[0].Selected = true;
        }

        private void UpdateProbeList()
        {
            onUpdateData = true;

            if (inspectionResult != null)
            {
                probeResultList.Rows.Clear();

                foreach (ProbeResult probeResult in inspectionResult)
                {
                    if ((showNgPad.Checked == true && probeResult.Judgment != Judgment.Accept) ||
                        (showGoodPad.Checked == true && probeResult.Judgment == Judgment.Accept))
                    {
                        AddProbeResult(probeResult);
                    }
                }
            }

            onUpdateData = false;
        }

        private void AddProbeResult(ProbeResult padResult)
        {
            int noId = probeResultList.Rows.Count + 1;
            string probeName;
            if (padResult.Probe != null)
                probeName = padResult.Probe.Name;
            else
                probeName = padResult.ProbeName;

            int rowId = probeResultList.Rows.Add(noId, probeName);
            probeResultList.Rows[rowId].Tag = padResult;

            Color cellColor = Color.White;
            if (padResult.Judgment == Judgment.FalseReject)
            {
                cellColor = Color.LightYellow;
            }
            else
            {
                cellColor = Color.LightPink;
            }

            probeResultList.Rows[rowId].Cells[1].Style.BackColor = cellColor;

            if (padResult.Probe != null)
            {
                List<Figure> figureList = schemaView.Schema.GetFigureByTag(padResult.Probe.FullId);
                foreach (Figure figure in figureList)
                {
                    if (figure is FigureGroup)
                    {
                        FigureGroup probeFigure = (FigureGroup)figure;
                        Figure resultFigure = probeFigure.GetFigure("rectangle") as RectangleFigure;
                        if (resultFigure != null)
                            resultFigure.FigureProperty.Brush = new SolidBrush(cellColor);
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Info(LoggerType.Operation, "Defect Process - Set Good");

            CloseForm();
        }

        private void CloseForm()
        {
            if (inspectionResult.IsDefected())
            {
                DialogResult = DialogResult.No;
            }
            else
            {
                bool rejectFlag = false;
                foreach(ProbeResult probeResult in inspectionResult.ProbeResultList)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        rejectFlag = true;
                }
                if (rejectFlag == false)
                    inspectionResult.Judgment = Judgment.FalseReject;
                DialogResult = DialogResult.OK;
            }

            Close();
        }

        private void reviewButton_Click(object sender, EventArgs e)
        {
            LogHelper.Info(LoggerType.Operation, "Defect Process - Enter Modeller");

            //ioHandler.TowerLampWaiting();

            //Modeller modeller = ModellerBuilder.Create();
            //modeller.Initialize(modelManager, machine, model, inspectionResult);
            //modeller.ShowModeller(this);

            LogHelper.Info(LoggerType.Operation, "Defect Process - Exit Modeller");
        }

        public void MoveNext()
        {
            int rowIndex = 0;

            if (probeResultList.SelectedRows.Count != 0)
            {
                rowIndex = probeResultList.SelectedRows[0].Index;
            }

            if (rowIndex < (probeResultList.Rows.Count - 1))
                probeResultList.Rows[rowIndex + 1].Selected = true;
        }

        public void MovePrev()
        {
            int rowIndex = 0;

            if (probeResultList.SelectedRows.Count != 0)
            {
                rowIndex = probeResultList.SelectedRows[0].Index;
            }

            if (rowIndex > 0)
                probeResultList.Rows[rowIndex - 1].Selected = true;
        }

        public void SetGood()
        {
            if (probeResultList.SelectedRows.Count == 0)
                return;

            int rowIndex = probeResultList.SelectedRows[0].Index;
            ProbeResult padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;
            padResult.Judgment = Judgment.FalseReject;

            probeResultList.SelectedRows[0].DefaultCellStyle.BackColor = Color.LightYellow;
        }

        public void SetDefect()
        {
            if (probeResultList.SelectedRows.Count == 0)
                return;

            int rowIndex = probeResultList.SelectedRows[0].Index;
            ProbeResult padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;
            padResult.Judgment = Judgment.Reject;

            probeResultList.SelectedRows[0].DefaultCellStyle.BackColor = Color.LightPink;
        }

        private void probeResultList_SelectionChanged(object sender, EventArgs e)
        {
            if (onUpdateData == true)
                return;

            if (probeResultList.SelectedRows.Count == 0)
                return;

            int rowIndex = probeResultList.SelectedRows[0].Index;
            ProbeResult padResult = (ProbeResult)probeResultList.SelectedRows[0].Tag;

            SelectDefect(padResult);
        }

        private void SelectDefect(ProbeResult probeResult)
        {
            string probeFullId;
            if (probeResult.Probe != null)
            {
                probeFullId = probeResult.Probe.FullId;
            }
            else
            {
                probeFullId = String.Format("{0:00}.{1:00}.{2:000}.{3:000}", probeResult.StepNo, probeResult.GroupId, probeResult.TargetId, 1);
            }

            string probeImageFileName = String.Format("{0}\\{1}_{2}.jpg",
                    inspectionResult.ResultPath, probeFullId, probeResult.Judgment == Judgment.Reject ? "N" : "G");

            if (File.Exists(probeImageFileName) == true)
            {
                targetResultImage.UpdateImage((Bitmap)ImageHelper.LoadImage(probeImageFileName));

                targetResultImage.TempFigureGroup.Clear();
                probeResult.AppendResultFigures(targetResultImage.TempFigureGroup, ResultImageType.Probe);

                targetResultImage.ZoomFit();
            }

            UpdateCameraImage(probeResultList);
            schemaView.ResetSelection();

            List<Figure> figureList = schemaView.Schema.GetFigureByTag(probeFullId);
            if (figureList != null)
            {
                schemaView.SelectFigure(figureList);
                schemaView.SelectFigureByCrosshair(figureList);
            }
        }

        private void UpdateCameraImage(DataGridView resultList)
        {
            if (resultList.SelectedRows.Count == 0)
                return;

            int rowIndex = resultList.SelectedRows[0].Index;
            ProbeResult padResult = (ProbeResult)resultList.SelectedRows[0].Tag;

            int lightTypeIndex = 0;

            string imgExt = OperationSettings.Instance().TargetGroupImageFormat.ToString();
            string cameraImgFileName;

            if (padResult.Probe != null)
            {
                Target target = padResult.Probe.Target;
                if (padResult.Probe is VisionProbe)
                {
                    VisionProbe visionProbe = (VisionProbe)padResult.Probe;
                    lightTypeIndex = visionProbe.LightTypeIndex;
                }

                cameraImgFileName = String.Format("{0}\\Image_C{1:00}_S{2:000}_L{3:00}.{4}",
                        inspectionResult.ResultPath, target.TargetGroup.GroupId,
                        target.TargetGroup.InspectionStep.StepNo, lightTypeIndex, imgExt);
            }
            else
            {
                cameraImgFileName = String.Format("{0}\\Image_C{1:00}_S{2:000}_L{3:00}.{4}",
                        inspectionResult.ResultPath, padResult.GroupId,
                        padResult.StepNo, 0, imgExt);
            }

            Image2D targetGroupImage;
            if (File.Exists(cameraImgFileName) == true)
            {
                targetGroupImage = new Image2D(cameraImgFileName);

                targetCameraImage.UpdateImage( targetGroupImage.ToBitmap());

                targetCameraImage.TempFigureGroup.Clear();

                padResult.AppendResultFigures(targetCameraImage.TempFigureGroup, ResultImageType.TargetGroup);

                targetCameraImage.ZoomFit();
            }
        }
    }
}
