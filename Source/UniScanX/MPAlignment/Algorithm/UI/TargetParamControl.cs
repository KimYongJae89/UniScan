using System;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Device.Dio;
using DynMvp.Vision;
using DynMvp.Data.Forms;
using DynMvp.Authentication;
using UniAoi.A.UI.ComponentControls;

namespace UniAoi.A.UI.InspectParamControl
{
    public delegate void SyncDoneDelegate();

    public partial class TargetParamControl : UserControl
    {
        enum AlgorithmType { PatternMatching, BinaryCounter, BrightnessChecker, WidthChecker, SimpleColorChecker,
                        BarcodeReader, ColorChecker, ColorMatchChecker, BoltChecker, BoltSegmentation, BlobChecker }

        public ValueChangedDelegate ValueChanged = null;
        public SyncDoneDelegate SyncDone = null;

        bool onValueUpdate = false;

        Target currentTarget;
        private List<Target> selectedTargets = null;
        public List<Target> SelectedTargets
        {
            set { selectedTargets = value; }
        }

        private List<Probe> selectedProbes = new List<Probe>();
        public List<Probe> SelectedProbes
        {
            get { return selectedProbes; }
            set { selectedProbes = value; }
        }

        private Bitmap targetImage = null;
        public CanvasPanel targetImageView;
        private UserControl selectedProbeParamControl = null;
        private VisionParamControl visionParamControl;
        private const string addNewTypeString = "Add New Type...";

        private Model curModel;

        AlgorithmType algorithmType;

        public TargetParamControl()
        {
            InitializeComponent();

            //this.targetImageView = new DrawBox();
            this.targetImageView = new CanvasPanel("TargetParam");
            this.visionParamControl = new VisionParamControl();

            //((System.ComponentModel.ISupportInitialize)(this.targetImageView)).BeginInit();
            this.SuspendLayout();

            this.visionParamControl.Name = "visionParamControl";
            this.visionParamControl.Location = new System.Drawing.Point(0, 0);
            this.visionParamControl.Size = new System.Drawing.Size(5, 5);
            this.visionParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.visionParamControl.TabIndex = 26;
            this.visionParamControl.Hide();
            this.visionParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);

            this.targetImageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetImageView.Location = new System.Drawing.Point(0, 0);
            this.targetImageView.Name = "targetImage";
            this.targetImageView.Size = new System.Drawing.Size(10, 10);
            this.targetImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetImageView.TabIndex = 26;
            this.targetImageView.TabStop = false;
            this.targetImageView.Enable = true;
            this.targetImageView.RotationLocked = false;

            this.targetImageView.FigureDeleted += targetImageView_FigureDeleted;
            this.targetImageView.FigureCopied += targetImageView_FigureCopied;
            this.targetImageView.FigureCreated += targetImageView_FigureCreated;
            this.targetImageView.FigureSelected += targetImageView_FigureSelected;
            this.targetImageView.FigureModified += targetImageView_FigureModified;
            this.targetImageView.FigurePasted += targetImageView_FigurePasted;
            this.targetImageView.InspectRequest += targetImageView_InspectRequest;

            this.pnlTargetImage.Controls.Add(this.targetImageView);

            //((System.ComponentModel.ISupportInitialize)(this.targetImageView)).EndInit();

            this.ResumeLayout(false);
            this.PerformLayout();

            //change language
            labelName.Text = StringManager.GetString(labelName.Text);
            useInspection.Text = StringManager.GetString(useInspection.Text);

            this.ColumnID.HeaderText = StringManager.GetString(this.ColumnID.HeaderText);
            this.ColumnType.HeaderText = StringManager.GetString(this.ColumnType.HeaderText);

            patternMatchingToolStripMenuItem.Text = StringManager.GetString(patternMatchingToolStripMenuItem.Text);
            binaryCounterToolStripMenuItem.Text = StringManager.GetString(binaryCounterToolStripMenuItem.Text);
            brightnessCheckerToolStripMenuItem.Text = StringManager.GetString(brightnessCheckerToolStripMenuItem.Text);
            simpleColorCheckerToolStripMenuItem.Text = StringManager.GetString(simpleColorCheckerToolStripMenuItem.Text);
            simpleColorCheckerToolStripMenuItem.Text = StringManager.GetString(simpleColorCheckerToolStripMenuItem.Text);

            ChangeAlgorithm();

            ShowAlgorithmParamControl(ProbeType.Vision);
        }

        private void ChangeAlgorithm()
        {

        }

        private void TargetParamControl_Load(object sender, EventArgs e)
        {

        }

        public void EnableUseInspection(bool flag)
        {
            this.useInspection.Enabled = flag;
        }

        public void Initialize()
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - Initialize");
        }

        public void UpdateData(Model model, Target selTarget)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateData");

            onValueUpdate = true;

            bool singleSelected = selectedTargets.Count() == 1;
            bool itemSelected = selectedTargets.Count() > 0;

            curModel = model;

            List<string> targetTypes = new List<string>();
            curModel.GetTargetTypes(targetTypes);
            this.cmbTargetType.Items.Clear();
            this.cmbTargetType.Items.AddRange(targetTypes.ToArray());

            this.cmbTargetType.Enabled = itemSelected;
            this.btnSyncTarget.Enabled = itemSelected;

            this.targetName.Enabled = singleSelected;
            this.useInspection.Enabled = itemSelected;
            this.comboInspectionLogicType.Enabled = itemSelected;
            this.btnAddProbe.Enabled = singleSelected;
            this.btnDeletProbe.Enabled = singleSelected;
            this.btnCopyProbe.Enabled = singleSelected;
            this.btnPasteProbe.Enabled = singleSelected;

            if (itemSelected == false)
            {
                targetImageView.ResetImage();

                targetImageView.WorkingFigures.Clear();

                probeSelector.Rows.Clear();

                ClearProbeData();

                currentTarget = null;
            }
            else
            {
                selectedProbes.Clear();

                currentTarget = selTarget;
                if (currentTarget == null)
                    currentTarget = selectedTargets[0];

                lblAngle.Text = currentTarget.Region.Angle.ToString();
                targetId.Text = currentTarget.Id.ToString();
                targetName.Text = currentTarget.Name;
                useInspection.Checked = currentTarget.UseInspection;
                cmbTargetType.Text = currentTarget.TypeName;
                comboInspectionLogicType.SelectedIndex = (int)currentTarget.InspectionLogicType;

                UpdateProbeSelector(currentTarget);
                UpdateTargetImageFigure(currentTarget);

                SelectProbe(0);
            }

            UpdateButtonState();

            onValueUpdate = false;
        }

        public void UpdateTargetImage(Bitmap targetImage)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateTargetImage(Bitmap targetImage)");

            targetImageView.ClearSelection();
            selectedProbes.Clear();
            targetImageView.TempFigures.Clear();

            this.targetImage = targetImage;

            UpdateTargetImage();
            visionParamControl.TargetImage = targetImage;
            targetImageView.ZoomFit();
        }

        public void UpdateTargetImage()
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateTargetImage");

            if (targetImage == null)
                return;

            if (selectedProbes.Count == 1 && (selectedProbeParamControl is VisionParamControl))
            {
                if (checkPreview.Checked)
                    targetImageView.UpdateImage(selectedProbes[0].PreviewFilterResult(targetImage, 0));
                else
                    targetImageView.UpdateImage(targetImage);
            }
            else
            {
                targetImageView.UpdateImage(targetImage);
            }

            targetImageView.Invalidate();
        }

        public void UpdateTargetImageFigure(Target target)
        {
            if (target == null)
            {
                targetImageView.ResetImage();
                targetImageView.ClearFigure();
                return;
            }

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateTargetImageFigure");

            target.AppendProbeFigures(targetImageView.WorkingFigures, null);

            foreach (Probe probe in target)
            {
                targetImageView.SelectFigureByTag(probe);
            }

            targetImageView.Invalidate();
        }

        public void UpdateProbeImageFigure()
        {
            if (currentTarget == null)
            {
                targetImageView.ResetImage();
                targetImageView.ClearFigure();
                return;
            }

            if (selectedProbes.Count() == 0)
                return;

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateProbeImageFigure");

            targetImageView.TempFigures.Clear();
            selectedProbes[0].AppendAdditionalFigures(targetImageView.TempFigures);

            RectangleF targetBoundRect = currentTarget.Region.GetBoundRect();
            targetImageView.TempFigures.Offset(-targetBoundRect.Left, -targetBoundRect.Top);
            targetImageView.Invalidate(true);
        }

        private void UpdateProbeSelector(Target target)
        {
            probeSelector.Rows.Clear();

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - UpdateProbeSelector");

            foreach (Probe probe in target)
            {
                int rowIndex = probeSelector.Rows.Add(probe.Id, probe.GetProbeTypeShortName());

                probeSelector.Rows[rowIndex].Height = (probeSelector.Height - probeSelector.ColumnHeadersHeight) / 3;
                if (probeSelector.Rows[rowIndex].Height > probeSelector.Height - probeSelector.ColumnHeadersHeight)
                {
                    probeSelector.Rows[rowIndex].Height = (probeSelector.Height - probeSelector.ColumnHeadersHeight);
                }

                probeSelector.Rows[rowIndex].Tag = probe;
            }
        }

        private void probeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - probeList_CellClick");

            SelectProbe(e.RowIndex);
        }

        private void SelectProbe(int rowIndex)
        {
            targetImageView.ClearSelection();

            if (rowIndex < 0 || rowIndex > (probeSelector.Rows.Count - 1))
            {
                selectedProbeParamControl.Hide();
                return;
            }

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - SelectProbe");

            Probe probe = (Probe)probeSelector.Rows[rowIndex].Tag;
            targetImageView.SelectFigureByTag(probe);

            SelectProbe(probe);
        }

        private void SelectProbe(Probe probe)
        {
            AddSelectedProbe(probe);

            UpdateTargetImage();

            targetImageView.TempFigures.Clear();
            probe.AppendAdditionalFigures(targetImageView.TempFigures);

            RectangleF targetBoundRect = currentTarget.Region.GetBoundRect();
            targetImageView.TempFigures.Offset(-targetBoundRect.Left, -targetBoundRect.Top);

            ShowAlgorithmParamControl(probe.ProbeType);

            UpdateButtonState();

            targetImageView.Invalidate(true);
        }

        private void SelectProbe(List<Probe> probeList)
        {
            UpdateTargetImage();

            selectedProbes.Clear();
            targetImageView.TempFigures.Clear();
            foreach (Probe probe in probeList)
            {
                AddSelectedProbe(probe);
                probe.AppendAdditionalFigures(targetImageView.TempFigures);
            }

            RectangleF targetBoundRect = currentTarget.Region.GetBoundRect();
            targetImageView.TempFigures.Offset(-targetBoundRect.Left, -targetBoundRect.Top);

            if (probeList.Count > 0)
            {
                Probe probe = probeList[0];
                ShowAlgorithmParamControl(probe.ProbeType);
            }

            UpdateButtonState();

            targetImageView.Invalidate(true);
        }

        public void ClearProbeData()
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - ClearProbeData");

            selectedProbes.Clear();
            targetImageView.ClearSelection();
            probeSelector.ClearSelection();

            if (selectedProbeParamControl != null)
                selectedProbeParamControl.Hide();
        }

        private void ShowAlgorithmParamControl(ProbeType probeType)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - ShowAlgorithmParamControl");

            if (selectedProbeParamControl != null)
                selectedProbeParamControl.Hide();

            switch (probeType)
            {
                case ProbeType.Vision:
                    selectedProbeParamControl = visionParamControl;
                    break;
                default:
                    throw new InvalidTypeException();
            }

            this.panelParam.Controls.Clear();
            this.panelParam.Controls.Add(selectedProbeParamControl);

            selectedProbeParamControl.Show();
            if (selectedProbes.Count == 1)
                ((ProbeContainable)selectedProbeParamControl).SetSelectedProbe(selectedProbes[0]);
            else
                ((ProbeContainable)selectedProbeParamControl).SetSelectedProbe(null);
        }

        private void addProbeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - addProbeButton_Click");

            contextMenuStripAlgorithmType.Show(btnAddProbe, new Point(0, btnAddProbe.Height));
        }

        private void deleteProbeButton_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void Delete()
        { 
            if (currentTarget == null)
                return;

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - deleteProbeButton_Click");

            targetImageView.DeleteSelection();
        }

        private void targetImageView_FigureDeleted(List<Figure> figureList)
        {
            foreach (Probe probe in SelectedProbes)
                currentTarget.RemoveProbe(probe);
            currentTarget.SetModified();

            UpdateProbeSelector(currentTarget);

            selectedProbes.Clear();

            UpdateTargetImage();

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private RotatedRect GetDefaultProbeRegion()
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - GetDefaultProbeRegion");

            //if (IsTargetSingleSelected() == false)
            //{
            //    LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
            //    return new RotatedRect();
            //}

            float centerX = currentTarget.Region.Left + currentTarget.Region.Width / 2;
            float centerY = currentTarget.Region.Top + currentTarget.Region.Height / 2;

            float width = currentTarget.Region.Width / 4;
            float height = currentTarget.Region.Height / 4;

            float left = centerX - width / 2;
            float top = centerY - height / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        private Bitmap GetClipImage(RotatedRect clipRegion)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - GetClipImage");

            return ImageHelper.ClipImage((Bitmap)targetImage, clipRegion);
        }

        private void AddVisionProbe(VisionProbe visionProbe)
        {
            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            visionProbe.InspAlgorithm.SourceImageType = ImageType.Color;
            if (currentTarget.Image.PixelFormat == PixelFormat.Format8bppIndexed)
                visionProbe.InspAlgorithm.SourceImageType = ImageType.Grey;

            AddProbe(visionProbe);
        }

        private void AddProbe(Probe probe)
        {
            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            currentTarget.AddProbe(probe);

            targetImageView.Invalidate();
        }

        private int GetProbeIndex(Probe probe)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - GetProbeIndex");

            for (int index = 0; index < probeSelector.RowCount; index++)
            {
                Probe queryProbe = (Probe)probeSelector.Rows[index].Tag;
                if (queryProbe == probe)
                    return index;
            }

            return -1;
        }

        private void AddSelectedProbe(Probe probe)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - AddSelectedProbe(Probe probe)");

            selectedProbes.Add(probe);

            int index = GetProbeIndex(probe);
            if (index > -1)
            {
                probeSelector.Rows[index].Selected = true;
            }
        }

        private void ClearSelectedProbe()
        {
            selectedProbes.Clear();
            probeSelector.ClearSelection();
            targetImageView.ClearSelection();
        }

        private void ProbeAdded(List<Probe> probeList)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - ProbeAdded");

            if (currentTarget == null)
                return;

            ClearSelectedProbe();
            UpdateProbeSelector(currentTarget);

            foreach (Probe probe in probeList)
            {
                AddSelectedProbe(probe);
                targetImageView.SelectFigureByTag(probe);

                if (probeList.Count == 1)
                    ShowAlgorithmParamControl(probe.ProbeType);
            }

            currentTarget.SetModified();

            ParamControl_ValueChanged(ValueChangedType.None, true);

            UpdateButtonState();
        }

        private VisionProbe CreateVisionProbe(RotatedRect inspRegion)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - CreateVisionProbe");

            VisionProbe visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);
            visionProbe.Region = inspRegion;

            return visionProbe;
        }

        private Algorithm CreateAlgorithm(RotatedRect imageRegion)
        {
            Algorithm algorithm = null;

            switch(algorithmType)
            {
                case AlgorithmType.PatternMatching:
                    PatternMatching patternMatching = new PatternMatching();
                    algorithm = patternMatching;

                    AlgoImage algoImage = ImageBuilder.Build(patternMatching.GetAlgorithmType(),
                                            GetClipImage(imageRegion), ImageType.Grey, ImageBandType.Luminance);

                    patternMatching.AddPattern(algoImage);
                    break;
                case AlgorithmType.ColorChecker:
                    ColorChecker colorChecker = new ColorChecker();
                    algorithm = colorChecker;
                    break;
                case AlgorithmType.BinaryCounter:
                    BinaryCounter binaryCounter = new BinaryCounter();
                    algorithm = binaryCounter;
                    break;
                case AlgorithmType.BrightnessChecker:
                    BrightnessChecker brightnessChecker = new BrightnessChecker();
                    algorithm = brightnessChecker;
                    break;
                case AlgorithmType.SimpleColorChecker:
                    SimpleColorChecker simpleColorChecker = new SimpleColorChecker();
                    algorithm = simpleColorChecker;
                    break;
                case AlgorithmType.BoltChecker:
                    NNBoltChecker boltChecker = new NNBoltChecker();
                    algorithm = boltChecker;
                    break;
                case AlgorithmType.BoltSegmentation:
                    NNBoltSegmentation boltSegmentation = new NNBoltSegmentation();
                    algorithm = boltSegmentation;
                    break;
                case AlgorithmType.BlobChecker:
                    BlobChecker blobChecker = new BlobChecker();
                    algorithm = blobChecker;
                    break;
            }

            return algorithm;
        }

        private void targetImageView_InspectRequest()
        {
            //throw new NotImplementedException();
        }

        private Probe CreateProbe(RotatedRect figureRect)
        {
            RotatedRect probeRegion = figureRect;
            RectangleF targetBoundRect = currentTarget.Region.GetBoundRect();
            probeRegion.X += targetBoundRect.Left;
            probeRegion.Y += targetBoundRect.Top;

            VisionProbe probe = CreateVisionProbe(probeRegion);
            if (probe == null)
                return null;

            probe.InspAlgorithm = CreateAlgorithm(figureRect);

            AddVisionProbe(probe);

            return probe;
        }

        private void patternMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - patternMatchingToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.PatternMatching;
        }

        private void colorCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - colorCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.ColorChecker;
        }

        private void colorMatchCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - colorMatchCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.ColorMatchChecker;
        }

        private void binaryCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - binaryCounterToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BinaryCounter;
        }

        private void iOProbeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - iOProbeToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return;

            Probe probe = ProbeFactory.Create(ProbeType.Io);
            probe.Region = rect;

            AddProbe(probe);
        }

        private void serialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - serialToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return;

            Probe probe = ProbeFactory.Create(ProbeType.Serial);
            probe.Region = rect;

            AddProbe(probe);
        }


        public void ShowInspResult(InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - ShowInspResult");

            if (selectedTargets.Count == 0)
                return;

            targetImageView.TempFigures.Clear();
            foreach (ProbeResult probeResult in inspectionResult)
            {
                if(currentTarget.Id == probeResult.Probe.Target.Id)
                    probeResult.AppendResultFigures(targetImageView.TempFigures, false);
            }
            targetImageView.Invalidate();
        }

        private void targetImageView_FigureModified(List<Figure> figureList)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - targetImageView_FigureMoved");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            foreach (Figure figure in figureList)
            {
                Probe probe = figure.Tag as Probe;
                if (probe != null)
                {
                    RotatedRect rectangle = figure.GetRectangle();

                    RectangleF targetBoundRect = probe.Target.Region.GetBoundRect();

                    Rectangle figureRect = rectangle.ToRectangle();
                    Rectangle imageRect = new Rectangle(0, 0, targetImageView.ImageWidth, targetImageView.ImageHeight);
                    if (Rectangle.Intersect(imageRect, figureRect) != figureRect)
                    {
                        RotatedRect probeRect = probe.Region;
                        probeRect.Offset(-targetBoundRect.Left, -targetBoundRect.Top);

                        figure.SetRectangle(probeRect);
                        continue;
                    }

                    rectangle.Offset(targetBoundRect.Left, targetBoundRect.Top);

                    probe.Region = rectangle;

                    VisionProbe visionProbe = probe as VisionProbe;
                    if (visionProbe != null)
                    {
                        if (visionProbe.InspAlgorithm is PatternMatching)
                        {
                            AlertForm frm = new AlertForm();

                            frm.TopMost = true;
                            frm.TopLevel = true;
                            frm.ShowAlert("Please, update pattern image", AlertForm.FormType.Info);
                        }
                    }
                }
            }

            currentTarget.SetModified();

            UpdateTargetImage();
            ((ProbeContainable)selectedProbeParamControl).UpdateProbeImage();

            ParamControl_ValueChanged(ValueChangedType.Position, true);
        }

        private void targetImageView_FigureCopied(List<Figure> figureList, CoordTransformer coordTransformer, FigureGroup workingFigures, FigureGroup backgroundFigures)
        {
            if (currentTarget == null)
                return;

            List<Probe> newProbeList = new List<Probe>();

            foreach (Figure figure in figureList)
            {
                Probe probe = figure.Tag as Probe;
                if (probe != null)
                {
                    Probe newProbe = (Probe)probe.Clone();

                    RotatedRect rectangle = figure.GetRectangle();
                    rectangle.Offset(currentTarget.Region.Left, currentTarget.Region.Top);
                    newProbe.Region = rectangle;

                    Figure newFigure = newProbe.AppendFigures(workingFigures, null);
                    newFigure.Offset(-currentTarget.Region.Left, -currentTarget.Region.Top);

                    currentTarget.AddProbe(newProbe);

                    newProbeList.Add(newProbe);
                }
            }

            ProbeAdded(newProbeList);
        }


        private void UpdateButtonState()
        {
            bool enable = selectedProbes.Count() > 0;
            //btnCopyProbe.Enabled = enable;
            //btnDeletProbe.Enabled = enable;
            //btnAddProbe.Enabled = CopyBuffer.IsTypeValid(typeof(Probe));
        }

        private void targetImageView_FigureSelected(List<Figure> figureList, bool appendMode = true)
        {
            if (currentTarget == null)
                return;

            if (figureList.Count == 0)
            {
                selectedProbes.Clear();
                if (selectedProbeParamControl != null)
                    selectedProbeParamControl.Hide();
                UpdateTargetImage();
            }
            else
            {
                List<Probe> probes = new List<Probe>();
                foreach (Figure figure in figureList)
                {
                    Probe probe = figure.Tag as Probe;
                    if (probe != null)
                    {
                        probes.Add(probe);
                    }
                }
                SelectProbe(probes);
            }

            UpdateButtonState();
        }

        private Probe AddProbe(Figure figure, FigureGroup workingFigures)
        {
            RotatedRect targetRegion = currentTarget.Region;
            RotatedRect figureRect = figure.GetRectangle();

            if (targetRegion.Angle != 0)
            {
                PointF centerPt = DrawingHelper.CenterPoint(figureRect);

                RotatedRect newFigureRect;
                if (targetRegion.Angle == 90 || targetRegion.Angle == 270)
                {
                    newFigureRect = DrawingHelper.FromCenterSizeAngle(centerPt, new SizeF(figureRect.Height, figureRect.Width), targetRegion.Angle);
                }
                else
                {
                    newFigureRect = DrawingHelper.FromCenterSizeAngle(centerPt, new SizeF(figureRect.Width, figureRect.Height), targetRegion.Angle);
                }

                figure.SetRectangle(newFigureRect);
                figureRect = newFigureRect;
            }

            RectangleF boundRect = figureRect.GetBoundRect();
            
            if (boundRect.Right >= targetImageView.ImageWidth)
                boundRect.Width = targetImageView.ImageWidth - boundRect.Left - 1;

            if ((boundRect.Bottom + 10) >= targetImageView.ImageHeight)
                boundRect.Height = targetImageView.ImageHeight - boundRect.Top - 1;

            Probe probe = CreateProbe(figureRect);
            figure.Tag = probe;

            figure.Pen = Probe.ProbePen;

            workingFigures.AddFigure(figure);

            return probe;
        }

        private void targetImageView_FigurePasted(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset)
        {
            if (currentTarget == null)
                return;

            List<Probe> probeList = new List<Probe>();
            foreach (Figure figure in figureList)
            {
                Probe probe = AddProbe(figure, workingFigures);
                probeList.Add(probe);
            }

            ProbeAdded(probeList);
        }

        private void targetImageView_FigureCreated(Figure figure, CoordTransformer coordTransformer, FigureGroup workingFigures, FigureGroup backgroundFigures)
        {
            if (currentTarget == null)
                return;

            Probe probe = AddProbe(figure, workingFigures);

            ProbeAdded(new List<Probe>() { probe });
        }

        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - ParamControl_ValueChanged");

            if (onValueUpdate == false)
            {
                if (valueChangedType == ValueChangedType.Position)
                {
                    UpdateProbeImageFigure();
                }
                else if (valueChangedType == ValueChangedType.ImageProcessing)
                {
                    UpdateTargetImage();
                }

                if (ValueChanged != null)
                    ValueChanged(valueChangedType, modified);

                currentTarget.SetModified();
            }
        }

        private void brightnessCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - brightnessCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BrightnessChecker;
        }

        private void widthCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - widthCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.WidthChecker;
        }

        private void targetName_TextChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - targetName_TextChanged");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            currentTarget.Name = targetName.Text;

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void SetTargetTypeName(string targetTypeName)
        {
            foreach (Target target in selectedTargets)
            {
                target.TypeName = targetTypeName;
                if (String.IsNullOrEmpty(target.Name))
                {
                    target.Name = targetTypeName;
                }
            }

            if (String.IsNullOrEmpty(targetName.Text))
                targetName.Text = targetTypeName;

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void targetImage_FigureMoved(Figure figure)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - targetImage_FigureMoved");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            Probe probe = figure.Tag as Probe;
            if (probe != null)
            {
                RotatedRect rectangle = figure.GetRectangle();
                rectangle.Offset(currentTarget.Region.Left, currentTarget.Region.Top);

                probe.Region = rectangle;

                UpdateTargetImageFigure(currentTarget);

                currentTarget.SetModified();
            }
        }

        private void targetImage_FigureSelected(Figure figure)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - targetImage_FigureSelected");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }


            if (figure == null)
            {
                ClearProbeData();
                UpdateTargetImage();
                return;
            }

            Probe probe = figure.Tag as Probe;
            if (probe != null)
            {
                foreach (DataGridViewRow row in probeSelector.Rows)
                {
                    if (row.Tag == probe)
                    {
                        row.Selected = true;
                        SelectProbe(row.Index);
                    }
                }
            }
        }

        private void daqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - daqToolStripMenuItem_Click");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return;

            Probe probe = ProbeFactory.Create(ProbeType.Daq);
            probe.Region = rect;

            AddProbe(probe);
        }

        private void copyProbeButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - copyProbeButton_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not single selected.");
                return;
            }

            Copy();
        }

        private void Copy()
        {
            targetImageView.Copy();

            UpdateButtonState();
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            UpDownControl.ShowControl(labelName.Text, (Control)sender);
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideControl((Control)sender);
        }

        private void useInspection_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - useInspection_CheckedChanged");

            foreach (Target target in selectedTargets)
            {
                target.UseInspection = useInspection.Checked;
            }

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void pasteProbeButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void Paste()
        {
            targetImageView.Paste();
        }

        private void comboLogic_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl -  Logic selected");
            foreach (Target target in selectedTargets)
            {
                if (comboInspectionLogicType.SelectedIndex <= 0 || comboInspectionLogicType.SelectedItem.ToString() == "AND")
                {
                    target.InspectionLogicType = InspectionLogicType.And;

                }
                else if (comboInspectionLogicType.SelectedItem.ToString() == "OR")
                {
                    target.InspectionLogicType = InspectionLogicType.Or;
                }

                target.SetModified();
            }
        }

        private void barcodeReadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "ModellerPage - barcodeReadToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BarcodeReader;
        }

        private void serialBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - serialBarcodeToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return;

            Probe probe = ProbeFactory.Create(ProbeType.Serial);
            probe.Region = rect;

            AddProbe(probe);
        }

        private void simpleColorCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - colorCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.SimpleColorChecker;
        }

        private void boltCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - boltCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BoltChecker;
        }

        private void boltSegmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - boltSegmentationToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BoltSegmentation;
        }

        private void serialDaqToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - daqToolStripMenuItem_Click");

            RotatedRect rect = GetDefaultProbeRegion();
            if (rect.IsEmpty)
                return;

            Probe probe = ProbeFactory.Create(ProbeType.SerialDaq);
            probe.Region = rect;

            AddProbe(probe);
        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            targetImageView.ZoomIn();
            Invalidate();
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            targetImageView.ZoomOut();

            Invalidate();
        }

        private void buttonZoomFit_Click(object sender, EventArgs e)
        {
            targetImageView.ZoomFit();

            Invalidate();
        }

        private void blobCheckerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - blobCheckerToolStripMenuItem_Click");

            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            targetImageView.SetAddMode(FigureType.Rectangle);
            algorithmType = AlgorithmType.BlobChecker;
        }

        private void btnSyncTarget_Click(object sender, EventArgs e)
        {
            if (currentTarget == null)
            {
                LogHelper.Error("TargetParamControl - Target is not selected or multi-selected");
                return;
            }

            curModel.SyncTarget(currentTarget);

            SyncDone?.Invoke();
        }

        private void cmbTargetType_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbTargetType_TextChanged(object sender, EventArgs e)
        {
            if (onValueUpdate == true)
                return;

            LogHelper.Debug(LoggerType.OpDebug, "TargetParamControl - cmbTargetType_TextChanged");

            foreach (Target target in selectedTargets)
                target.TypeName = cmbTargetType.Text;

            ParamControl_ValueChanged(ValueChangedType.None, true);
        }

        private void checkPreview_CheckedChanged(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.OpDebug, "VisionParamControl - checkPreview_CheckedChanged");

            if (currentTarget == null)
            {
                LogHelper.Error("VisionParamControl - selectedProbe instance is null.");
                return;
            }

            ParamControl_ValueChanged(ValueChangedType.ImageProcessing, false);
        }
    }
}
