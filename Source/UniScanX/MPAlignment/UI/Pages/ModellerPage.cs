using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Device;
using DynMvp.Device.Dio;
using DynMvp.Device.FrameGrabber;
using DynMvp.Device.UI;
using DynMvp.Authentication;
using DynMvp.Data.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
//using UniScanX.MPAlignment.Data;
using UniScanX.MPAlignment.Algo.UI;
using DynMvp.InspData;

namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class ModellerPage : UserControl
    {
        private UniScanX.MPAlignment.Data.MPModel currentModel= null;

        // Review시에만 사용됨
        private MPAlignment.Data.InspectionResult lastInspectionResult = null;

        private TryInspectionResultView tryInspectionResultView;
        private TargetParamControl targetParamControl;
        private CanvasPanel cameraImagePanel;

 
        private DynMvp.Data.Target curTarget = null;
        private List<DynMvp.Data.Target> selectedTargets = new List<DynMvp.Data.Target>();
        private const int padding = 3;

        private const int targetColumnWidth = 70;

        private List<Bitmap> targetGroupImageList;
        private List<Target> seletedTargetList;

        Bitmap modelWholeImage = null;


        private bool modified;
        private bool lockUpdateImage = false;
        private bool initialized = false;

        bool reviewMoode = false;
       
        int lightTypeIndex = 0;
        //WaitLoading loading = new WaitLoading();
        Size defaultImageSize;

        Bitmap curImage = null;
   //     FiducialMgrForm fiducialMgrForm;

        Color GoodColor = Color.LightGreen;
        Color NgColor = Color.Red;
        Color UnknownColor = Color.LightGray;

        // 선택 검사 결과
        MPAlignment.Data.InspectionResult tryInspectionResult = new MPAlignment.Data.InspectionResult();

        UniScanX.MPAlignment.Devices.DeviceController DevCtroller
        {
            get => (UniScanX.MPAlignment.Devices.DeviceController)SystemManager.Instance().DeviceController;
        }

        public ModellerPage()
        {
            InitializeComponent();
            InitializeControl();
        }

        void InitializeControl()
        {
            this.tryInspectionResultView = new TryInspectionResultView();
            this.cameraImagePanel = new CanvasPanel();// "Modeller");
            this.targetParamControl = new TargetParamControl();

            this.panelParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraImage
            // 
            this.cameraImagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cameraImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraImagePanel.Location = new System.Drawing.Point(3, 3);
            this.cameraImagePanel.Name = "cameraImage";
            this.cameraImagePanel.Size = new System.Drawing.Size(409, 523);
            this.cameraImagePanel.TabIndex = 8;
            this.cameraImagePanel.TabStop = false;
 //           this.cameraImagePanel.Enable = true;
            this.cameraImagePanel.RotationLocked = true;

            this.cameraImagePanel.FigureCreated += cameraImagePanel_FigureCreated;
            this.cameraImagePanel.FigureModified += cameraImagePanel_FigureModified;
 //           this.cameraImagePanel.FigureSelected += cameraImagePanel_FigureSelected;
//            this.cameraImagePanel.FigureCopied += cameraImagePanel_FigureCopied;
            this.cameraImagePanel.FigurePasted += cameraImagePanel_FigurePasted;
//            this.cameraImagePanel.FigureDeleted += cameraImagePanel_FigureDeleted;
            this.cameraImagePanel.MouseDoubleClick += cameraImagePanel_MouseDoubleClick;
 //           this.cameraImagePanel.InspectRequest += cameraImagePanel_InspectRequest;

            this.pnlFov.Controls.Add(this.cameraImagePanel);

            this.pnlInspectionResult.Controls.Add(this.tryInspectionResultView);

            this.tryInspectionResultView.Location = new System.Drawing.Point(96, 95);
            this.tryInspectionResultView.Name = "tryInspectionResultView";
            this.tryInspectionResultView.Size = new System.Drawing.Size(74, 101);
            this.tryInspectionResultView.TabIndex = 0;
            this.tryInspectionResultView.Dock = System.Windows.Forms.DockStyle.Fill;

            // 
            // targetParamControl
            // 
            this.targetParamControl.Location = new System.Drawing.Point(96, 95);
            this.targetParamControl.Name = "targetParamControl";
            this.targetParamControl.Size = new System.Drawing.Size(74, 101);
            this.targetParamControl.TabIndex = 0;
            this.targetParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetParamControl.SelectedTargets = selectedTargets;
            this.targetParamControl.ValueChanged = new ValueChangedDelegate(TargetParamControl_ValueChagned);
            this.targetParamControl.SyncDone = targetParamControl_SyncDone;

            this.panelParameter.Controls.Clear();
            this.panelParameter.Controls.Add(this.targetParamControl);

            this.panelParameter.ResumeLayout(false);
            this.ResumeLayout(false);

  //          fiducialMgrForm = new FiducialMgrForm(this);

            string lightStr = StringManager.GetString("Light");

            DataGridViewImageColumn column = new DataGridViewImageColumn();
            column.DefaultCellStyle.Padding = new Padding(padding);

            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            column.ImageLayout = DataGridViewImageCellLayout.Zoom;

            // change language
            labelExposure.Text = StringManager.GetString(labelExposure.Text);

            targetGroupImageList = new List<Bitmap>();

            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            targetSelector.DefaultCellStyle.SelectionBackColor = Color.Transparent;
            targetSelector.DefaultCellStyle.SelectionForeColor = Color.Red;

            timer1.Interval = 333;
            //timer1.Start();
        }

        MPAlignment.Data.ModelManager GetModelManager()
        {
            return SystemManager.Instance().ModelManager as MPAlignment.Data.ModelManager;
        }

        private void targetParamControl_SyncDone()
        {
            UpdateCameraImageFigure();
        }

        private void cameraImagePanel_InspectRequest()
        {
            Inspect();
        }

        private void cameraImagePanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            cameraImagePanel.BackgroundFigures.Clear();
            cameraImagePanel.Invalidate();
        }

        public ModellerPage(MPAlignment.Data.InspectionResult lastInspectionResult)
        { 
            if (GetModelManager().CurrentModel == null)
                return;
            InitializeComponent();
            InitializeControl();
            this.lastInspectionResult = lastInspectionResult;
            Initialize(GetModelManager().CurrentModel);

            pnlShowAllResult.Visible = true;
        }

        #region Double buffers 
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        #endregion


        public void Initialize(Model model)
        {
            if (model == null)
                return;
            LogHelper.Debug(LoggerType.Error, "Modeller - Initialize");

            this.currentModel = model as MPAlignment.Data.MPModel;

            modelWholeImage = currentModel.LoadDefaultImage();

           // this.imageAcquisition = DeviceManager.Instance.ImageAcquisition;

            LogHelper.Debug(LoggerType.Error, "Target Parameter control - Initialize");
            this.targetParamControl.Initialize();

            LogHelper.Debug(LoggerType.Error, "Model get target types - Initialize");
            List<string> modelTargetTypes = new List<string>();
            
            model.GetTargetTypes(modelTargetTypes);

            LogHelper.Debug(LoggerType.Error, "Index of target name - Initialize");

            initialized = true;

            UpdateModelInfo();
        }

        private void Modeller_Load(object sender, EventArgs e)
        {
            cameraImagePanel.ZoomFit();
        }

        public void ShowReview(MPAlignment.Data.InspectionResult lastInspectionResult)
        {
            this.lastInspectionResult = lastInspectionResult;

            UpdateTargetSelector();
        }

        private void UpdateModelInfo()
        {
            LogHelper.Debug(LoggerType.Algorithm,"Modeller - Load");
   //         selectedTargetGroup = currentModel.InspectionStepList[0].TargetGroupList[0];
        //    nudExposureTime.Value = (decimal)selectedTargetGroup.LightParamList[lightTypeIndex].ExposureTime;

            LoadLastImage();

            UpdateTargetTypeCmb();
            UpdateTargetSelector();
        }

        private void LoadLastImage()
        {
            if (GetModelManager().CurrentModel.ModelPath == null)
                return;
            //string path = Path.Combine(GetModelManager().CurrentModel.ModelPath, "DefaultImage");
            //string imagePath = Path.Combine(path, "DefaultImage.bmp");
            //if(File.Exists(imagePath) == false)
            //{
            //    LogHelper.Debug(LoggerType.Error, "Modeller page LoadLastImage file is not exist");
            //    GrabImage(); 
            //    return;
            //}
            //Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(imagePath);

            Bitmap bitmap = currentModel.LoadDefaultImage();
            if(bitmap != null)
                UpdateCameraImage(bitmap);
        }

        private void GrabImage(int inspectionStep = 0)
        {
    //        currentModel.GetInspectionStep(inspectionStep).UpdateImageBuffer(imageAcquisition.ImageBuffer);
      //      imageAcquisition.AcquireImage(inspectionStep, lightCtrl, 300);

            UpdateCameraImage();
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        private void SelectTargetGroup(int camId)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - SelectTargetGroup");

            ClearSelectedTarget();
        }

        private void AlignCameraImage()
        {
            FiducialSet fiducialSet = currentModel.FiducialSet;
            if (fiducialSet.Count > 0)
            {
                List<Bitmap> grabImageList = new List<Bitmap>();
                grabImageList.Add((Bitmap)curImage);

                //   InspParam inspParam = new InspParam(grabImageList, true, false, false);
                MPAlignment.Data.InspectionResult inspectionResult = new MPAlignment.Data.InspectionResult();

           //     fiducialSet.Inspect(inspParam, inspectionResult);

          //      PositionAligner positionAligner = fiducialSet.Calculate(inspectionResult);

          //      cameraImagePanel.UpdateImage((Bitmap)ImageHelper.TransformImage(curImage, positionAligner.Offset, positionAligner.RotationCenter, positionAligner.Angle, true));
            }
            else
            {
                cameraImagePanel.UpdateImage((Bitmap)ImageHelper.CloneImage(curImage));
            }
        }

        private void UpdateCameraImage(Bitmap virtualImage = null)
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Algorithm, "Modeller - UpdateCameraImage");

            Bitmap bitmap = null;

            if (virtualImage != null)
            {
                bitmap = (Bitmap)virtualImage.Clone();
                virtualImage.Dispose();
                virtualImage = null;
            }
            else
            {
//                bitmap = imageAcquisition.GetImage((uint)selectedTargetGroup.GroupId);
            }

            defaultImageSize = new Size(bitmap.Width, bitmap.Height);

            //Image preImage = cameraImage.Image;
            //cameraImage.Image = (Image)bitmap.Clone();
            //preImage?.Dispose();

            Image preImage = curImage;
            curImage = ImageHelper.CloneImage(bitmap);
            preImage?.Dispose();

            AlignCameraImage();
            SaveDefaultImage(bitmap);

            UpdateCameraImageFigure();
            if (selectedTargets.Count == 1)
                ShowTargetParam(selectedTargets[0]);

            cameraImagePanel.Invalidate();

            async void SaveDefaultImage(Bitmap image)
            {
                try
                {
                    if (GetModelManager().CurrentModel == null)
                        return;
                    if (image == null)
                        return;
                    string path = Path.Combine(GetModelManager().CurrentModel.ModelPath, "DefaultImage");
                    string grabbedImagePath = Path.Combine(GetModelManager().CurrentModel.ModelPath, "GrabbedImage");
                    if (Directory.Exists(path) == false)
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (Directory.Exists(grabbedImagePath) == false)
                    {
                        Directory.CreateDirectory(grabbedImagePath);
                    }

                    string imagePath = Path.Combine(path, "DefaultImage.bmp");
                    string lastImage = Path.Combine(grabbedImagePath, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.jpg");

                    await Task.Run(() => {
                        using (var newBitmap = new Bitmap(image))
                        {
                            newBitmap.Save(imagePath, ImageFormat.Bmp);
                            newBitmap.Save(lastImage, ImageFormat.Jpeg);
                        }
                        //image.Save(imagePath, ImageFormat.Bmp)
                    });
                    image?.Dispose();
                    image = null;
                }
                catch
                {
                    if (image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                }
            }
        }

        public void UpdateTargetTypeCmb()
        {
            List<string> targetTypeNames = new List<string>();
            foreach (Target target in currentModel.TargetList)
            {
                if (String.IsNullOrEmpty(target.TypeName) == false)
                    targetTypeNames.Add(target.TypeName);
            }
            cmbTargetType.Items.Clear();
            cmbTargetType.Items.AddRange(targetTypeNames.Distinct().ToArray());
        }

        public void UpdateTargetSelector()
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - UpdateTargetSelector");

            targetSelector.Rows.Clear();

            List<Target> targetList = new List<Target>();
            if (selectedTargets.Count > 0)
                targetList.AddRange(selectedTargets);
            else
                targetList.AddRange(currentModel.TargetList);

            string targetType = "Unknown";
            foreach (Target target in targetList)
            {
                int index = targetSelector.Rows.Add(target.Name);
                targetSelector.Rows[index].Tag = target;
                if (targetType != "")
                {
                    if (targetType == "Unknown")
                        targetType = target.TypeName;
                    else if (targetType != target.TypeName)
                        targetType = "";
                }
            }

          //  onUpdate = true;
            cmbTargetType.Text = targetType;
         //   onUpdate = false;

            int selIndex = targetSelector.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (selIndex > -1)
                targetSelector.Rows[selIndex].Selected = false;

            UpdateTargetResult();

            curTarget = null;
        }

        private RotatedRect GetDefaultTargetRegion()
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - GetDefaultTargetRegion");

            int width = 200;
            int height = 200;
            int left = 0;// (cameraImagePanel.Image.  - width) / 2;
            int top = 0;// (cameraImagePanel.ImageHeight - height) / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        private Target AddTarget(RotatedRect targetRegion, bool select = true)
        {
            Target target = new Target();

            //target.Region = targetRegion;

            //target.UpdateRegion(targetRegion);

            //target.Image = cameraImagePanel.ClipImage(Rectangle.Truncate(targetRegion.GetBoundRect()));

            //selectedTargetGroup.AddTarget(target);

            if (select)
            {
                List<Target> targetList = new List<Target>();
                targetList.Add(target);

                TargetAdded(targetList);
            }

            return target;
        }

        private void TargetAdded(List<Target> targetList)
        {
            LogHelper.Debug(LoggerType.Algorithm,"Modeller - TargetAdded");

            ClearSelectedTarget();

            foreach (Target target in targetList)
            {
                cameraImagePanel.SelectFigureByTag(target);
            }

            SelectTarget(targetList, false);

            SetModified();

            UpdateButtonState();

            UpdateTargetTypeCmb();
            UpdateTargetSelector();
        }

        private void UpdateCameraImageFigure()
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - UpdateCameraImageFigure");

            cameraImagePanel.WorkingFigures.Clear();
            cameraImagePanel.BackgroundFigures.Clear();

            currentModel.AppendFigures(cameraImagePanel.WorkingFigures, null, false);

            currentModel.FiducialSet.AppendFigures(cameraImagePanel.WorkingFigures);

            cameraImagePanel.TempFigures.Clear();
            if (lastInspectionResult != null)
            {
                Pen redPen = new Pen(Color.Red);
                Pen yellowPen = new Pen(Color.Yellow);
                foreach (Target target in currentModel.TargetList)
                {
                    if (lastInspectionResult.IsDefected(target))
                    {
                        target.AppendFigures(cameraImagePanel.BackgroundFigures, redPen, false);
                    }
                    else if (lastInspectionResult.IsPass(target))
                    {
                        target.AppendFigures(cameraImagePanel.BackgroundFigures, yellowPen, false);
                    }
                }
            }

            cameraImagePanel.Invalidate();
        }

        private void targetSelector_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - targetSelector_CellClick");

            if (e.RowIndex == -1)
                return;

            curTarget = (Target)targetSelector.Rows[e.RowIndex].Tag;
            if (selectedTargets.Count == 0)
            {
                SelectTarget(new List<Target>() { curTarget }, false);
            }
            else
            {
                ShowTargetParam(curTarget);
                ShowTargetResult(curTarget);
            }


          //  cameraImagePanel.AddCrossFigure(DrawingHelper.CenterPoint(curTarget.Region));

            UpdateButtonState();
        }

        private void ShowTargetResult(Target target)
        {
            MPAlignment.Data.InspectionResult lastTargetResult = null;
            MPAlignment.Data.InspectionResult tryTargetResult = tryInspectionResult.GetTargetResult(target) as MPAlignment.Data.InspectionResult;

            if (lastInspectionResult != null)
            {
                lastTargetResult = lastInspectionResult.GetTargetResult(target) as MPAlignment.Data.InspectionResult;
            }

            tryInspectionResultView.UpdateResult(tryTargetResult, lastTargetResult);
        }

        private bool GetTargetIndex(Target target, out int targetRowIndex)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - GetTargetIndex");

            targetRowIndex = 0;

            for (int rowIndex = 0; rowIndex < targetSelector.RowCount; rowIndex++)
            {
                Target queryTarget = (Target)targetSelector.Rows[rowIndex].Tag;
                if (queryTarget == target)
                {
                    targetRowIndex = rowIndex;
                    return true;
                }
            }

            return false;
        }

        public void SelectTarget(List<Target> targetList, bool appendMode)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - SelectTarget(Target target)");

            if (appendMode == false)
                selectedTargets.Clear();

            selectedTargets.AddRange(targetList);

            if (selectedTargets.Count == 0)
                return;

            Target firstTarget = targetList[0];
            ShowTargetParam(targetList[0]);
            ShowTargetResult(targetList[0]);
        }

        private void ShowTargetParam(Target target)
        {
            targetParamControl.UpdateTargetImageFigure(target);

    //        Bitmap tempTargetImage = cameraImagePanel.ClipImage(Rectangle.Truncate(target.Region.GetBoundRect()));
    //        targetParamControl.UpdateTargetImage(tempTargetImage);

            targetParamControl.UpdateData(currentModel, target);
        }

        private void ClearSelectedTarget()
        {
            selectedTargets.Clear();
            targetSelector.ClearSelection();
            cameraImagePanel.ClearSelection();
        }

        private void Inspect()
        {
            if (currentModel == null)
                return;

            tryInspectionResultView.ClearResult();
            tryInspectionResult.Clear();

            List<Target> inspTargetList = new List<Target>();

            int selIndex = targetSelector.Rows.GetFirstRow(DataGridViewElementStates.Selected);

            if (selectedTargets.Count > 0)
                inspTargetList.AddRange(selectedTargets);
            else if (selIndex > -1)
                inspTargetList.Add((Target)targetSelector.Rows[selIndex].Tag);
            else
                inspTargetList.AddRange(currentModel.TargetList);

            if (inspTargetList.Count == 0)
                return;

            int groupId = 0;
            bool saveDebugImage = true;

            Bitmap image = cameraImagePanel.Image.Clone() as Bitmap; //zmsong

            List<Bitmap> grabImageList = new List<Bitmap>();
            grabImageList.Add(image);

            //InspParam inspParam = new InspParam(grabImageList, saveDebugImage, false, false); //zmsong

            foreach (Target target in inspTargetList)
            {
                target.OnPreInspection();

                LogHelper.Debug(LoggerType.Operation, "Modeller - testTargetButton_Click");

            //    target.Inspect(inspParam, tryInspectionResult); zmsong
            }

            image.Dispose();

            cameraImagePanel.BackgroundFigures.Clear();
//            tryInspectionResult.AppendResultTargetFigures(cameraImagePanel.BackgroundFigures);

            Pen redPen = new Pen(Color.Red);
            Pen yellowPen = new Pen(Color.Yellow);
            Pen dashRedPen = new Pen(Color.Red);
            dashRedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            Pen dashYellowPen = new Pen(Color.Yellow);
            dashYellowPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            foreach (Target target in currentModel.TargetList)
            {
                Pen defectPen = null;

                if (tryInspectionResult.IsDefected(target))
                    defectPen = redPen;
                else if (lastInspectionResult != null)
                {
                    if (lastInspectionResult.IsDefected(target))
                        defectPen = dashRedPen;
                    else if (lastInspectionResult.IsPass(target))
                        defectPen = dashYellowPen;
                }

                if (defectPen != null)
                    target.AppendFigures(cameraImagePanel.BackgroundFigures, defectPen, false);
            }

            cameraImagePanel.Invalidate();

            targetParamControl.ShowInspResult(tryInspectionResult);

            UpdateTargetResult();

            if (curTarget != null)
                ShowTargetResult(curTarget);
            else
                ShowTargetResult(inspTargetList[0]);
        }

        Color GetJudgeColor(Judgment judgment)
        {
            return (judgment == Judgment.Accept ? GoodColor : (judgment == Judgment.Reject ? NgColor : UnknownColor));
        }

        private void UpdateTargetResult()
        {
            int selectedIndex = -1;
            for (int i=0; i< targetSelector.Rows.Count; i++)
            {
                Target target = (Target)targetSelector.Rows[i].Tag;
                MPAlignment.Data.InspectionResult inspectionResult = 
                    tryInspectionResult.GetTargetResult(target) as MPAlignment.Data.InspectionResult;
                if (inspectionResult != null)
                {
                    if (targetSelector.Rows[i].Selected)
                    {
                        selectedIndex = i;
                        targetSelector.Rows[i].Selected = false;
                    }
                    //zmsong
               //     targetSelector.Rows[i].Cells[1].Style.BackColor = GetJudgeColor(inspectionResult.Judgment);
                }
            }

            if (lastInspectionResult != null)
            {
                for (int i = 0; i < targetSelector.Rows.Count; i++)
                {
                    Target target = (Target)targetSelector.Rows[i].Tag;

                    MPAlignment.Data.InspectionResult inspectionResult = 
                        lastInspectionResult.GetTargetResult(target) as MPAlignment.Data.InspectionResult;
                    if (inspectionResult != null)
                    {
                        targetSelector.Rows[i].Cells[1].Style.BackColor = GetJudgeColor(inspectionResult.Judgment);

                        if (inspectionResult.Judgment == Judgment.Accept || 
                            inspectionResult.Judgment == Judgment.Skip)
                            targetSelector.Rows[i].Visible = ckbShowAllResult.Checked;
                        else
                            targetSelector.Rows[i].Visible = true;
                    }
                    else
                    {
                        targetSelector.Rows[i].Visible = ckbShowAllResult.Checked;
                    }
                }
            }

            if (selectedIndex > -1)
            {
                targetSelector.UpdateCellValue(0, selectedIndex);
                targetSelector.Rows[selectedIndex].Selected = true;
            }
        }

        private void deleteTargetButton_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void cameraImagePanel_FigureDeleted(List<Figure> figureList)
        {
            foreach (Target target in selectedTargets)
            {
                currentModel.TargetList.Remove(target);
            }

            ClearSelectedTarget();

            UpdateTargetTypeCmb();
            UpdateTargetSelector();
            targetParamControl.UpdateData(currentModel, null);

            SetModified();
        }

        private void Delete()
        {
            if (selectedTargets.Count == 0 || this.currentModel == null)
                return;

         //   cameraImagePanel.DeleteSelection();//zmsong
        }

        private void cameraImagePanel_FigureModified(List<Figure> figureList)
        {
            if (currentModel == null)
                return;

            //foreach (Figure figure in figureList)
            //{
            //    Target target = figure.Tag as Target;
            //    if (target != null)
            //    {
            //        RotatedRect rectangle = figure.GetRectangle();

            //        Rectangle figureRect = Rectangle.Truncate(rectangle.GetBoundRect());
            //        Rectangle imageRect = new Rectangle(0, 0, cameraImagePanel.ImageWidth, cameraImagePanel.ImageHeight);
            //        if (Rectangle.Intersect(imageRect, figureRect) != figureRect)
            //        {
            //            figure.SetRectangle(target.Region);
            //            continue;
            //        }
            //        bool isIntersectOk = true;
            //        foreach (Probe probe in target.ProbeList)
            //        {
            //            if ((probe.Region.Width >= rectangle.Width) || (probe.Region.Height >= rectangle.Height))
            //            {
            //                isIntersectOk = false;
            //                break;
            //            }
            //        }

            //        if (isIntersectOk == false)
            //        {
            //            MessageBox.Show("프로브보다 더 작은 크기로는 타겟을 만들 수 없습니다. 프로브를 먼저 지워주세요.");
            //            figure.SetRectangle(target.Region);
            //            return;
            //        }
            //        Bitmap targetImage = cameraImagePanel.ClipImage(Rectangle.Truncate(rectangle.GetBoundRect()));

            //        target.UpdateRegion(rectangle);
            //        target.Image = targetImage;

            //        int rowIndex;

            //        if (GetTargetIndex(target, out rowIndex) == true)
            //        {
            //            targetSelector.Rows[rowIndex].Selected = true;
            //        }

            //        targetParamControl.UpdateTargetImageFigure(target);
            //        targetParamControl.UpdateTargetImage(targetImage);

            //        modified = true;
            //    }
            //}
        }

        private void cameraImagePanel_FigureCopied(List<Figure> figureList, 
            CoordTransformer coordTransformer, FigureGroup workingFigures, FigureGroup backgroundFigures)
        {
            if (currentModel == null)
                return;

            List<Target> newTargetList = new List<Target>();

            foreach (Figure figure in figureList)
            {
                Target target = figure.Tag as Target;
                if (target != null)
                {
                    Target newTarget = (Target)target.Clone();

                    //RotatedRect rectangle = figure.GetRectangle(); //zmsong
                    //newTarget.UpdateRegion(rectangle);

                    currentModel.TargetList.Add(newTarget);

                    newTargetList.Add(newTarget);

                    newTarget.AppendFigures(workingFigures, null);
                }
            }

            TargetAdded(newTargetList);
        }

        private void UpdateButtonState()
        {
            bool enable = selectedTargets.Count() > 0;
            //copyTargetButton.Enabled = enable;
            //deleteTargetButton.Enabled = enable;
            //pasteTargetButton.Enabled = CopyBuffer.IsTypeValid(typeof(Target));
        }

        private void cameraImagePanel_FigureSelected(List<Figure> figureList, bool appendMode = true)
        {
            if (currentModel == null)
                return;

            if (figureList.Count == 0)
            {
                if (appendMode == false)
                {
                    selectedTargets.Clear();
                    targetSelector.ClearSelection();
                    tryInspectionResultView.ClearResult();
                    targetParamControl.UpdateData(currentModel, null);
                    cameraImagePanel.TempFigures.Clear();
                    cameraImagePanel.Invalidate();
                }
            }
            else
            {
                List<Target> targetList = new List<Target>();
                foreach (Figure figure in figureList)
                {
                    Target target = figure.Tag as Target;
                    if (target != null)
                        targetList.Add(target);
                }

                SelectTarget(targetList, appendMode);
            }

            UpdateButtonState();

            UpdateTargetTypeCmb();
            UpdateTargetSelector();

            if (lblMessage.Visible)
                lblMessage.Visible = false;
        }

        public Target AddTarget(Figure figure, FigureGroup workingFigures, bool select = true)
        {
            RotatedRect figureRect = figure.GetRectangle();

            Target target = AddTarget(figureRect, select);
            figure.Tag = target;

    //        figure.Pen = Target.TargetPen; //zmsong

            workingFigures.AddFigure(figure);

            return target;
        }

        private void cameraImagePanel_FigurePasted(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset)
        {
            if (currentModel == null)
                return;

            foreach (Figure figure in figureList)
            {
                AddTarget(figure, workingFigures);
            }
        }
        
        private void cameraImagePanel_FigureCreated(Figure figure, CoordMapper coordMapper)
        {
            if (currentModel == null)
                return;
            FigureGroup workingFigures = null;
            RotatedRect rectangle = new RotatedRect();
            rectangle.FromLTRB(100, 100, 200, 200);
            figure.SetRectangle(rectangle);
          //  AddTarget(figure, workingFigures);
        }

        private void addFiducialButton_Click(object sender, EventArgs e)
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - addFiducialButton_Click");

            if (selectedTargets.Count == 1)
            {
                Probe alignmentProbe = selectedTargets[0].GetAlignmentProbe();
                if (alignmentProbe != null)
                {
                    currentModel.FiducialSet.AddFiducial(alignmentProbe);
                    UpdateCameraImageFigure();
                }
                else
                {
                    MessageBox.Show("There is no Pattern Probe. Please, insert pattern probe before setting fiducial.");
                }
            }

            SetModified();
        }

        private void removeFiducialButton_Click(object sender, EventArgs e)
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - removeFiducialButton_Click");

            if (selectedTargets.Count == 1)
            {
                Probe alignmentProbe = selectedTargets[0].GetAlignmentProbe();
                if (alignmentProbe != null)
                {
                    currentModel.FiducialSet.RemoveFiducial(alignmentProbe);
                    UpdateCameraImageFigure();
                }
            }

            SetModified();
        }

        private void useLightList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentModel == null)
                return;

            if (lockUpdateImage == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - useLightList_SelectedIndexChanged");

            SetModified();
        }

        private void GrabCameraImage()
        {
            if (currentModel == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - GrabCameraImage");

            int groupId = 0;// currentModel.GroupId;

            //ImageBufferItem imageCell = imageAcquisition.ImageBuffer.GetImageBufferItem(groupId, lightTypeIndex);

            //imageAcquisition.AcquireImage(0, lightCtrl, OperationConfig.Instance.Time.StatbleTime);
        }

        private void copyTargetButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Copy Target");

            Copy();
        }

        private void Copy()
        {
            cameraImagePanel.Copy();
        }

        private void pasteTargetButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void Paste()
        {
            cameraImagePanel.Paste();
        }

        private void exitModellerButton_Click(object sender, EventArgs e)
        {
            //if (modified == false || ModellerExitMessageForm.ShowMessage(this, currentModel) == DialogResult.Yes)
            //{
            //    lightCtrl.TurnOff();
            //    Close();
            //    if(Settings.Instance().Machine.MachineType == MachineType.AV_823 || Settings.Instance().Machine.MachineType == MachineType.AV_824)
            //        ioHandler.EjectTable();
            //}
        }

        private void saveModelButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - saveModelButton_Click");

  //          currentModel.SaveModel(); zmsong

            modified = false;
        }

        private void saveImagesButton_Click(object sender, EventArgs e)
        {

        }

        private void loadImagesButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Modeller - Load Offline Image");
        }

        private void SetModified()
        {
            modified = true;
        }

        public void TargetParamControl_ValueChagned(ValueChangedType valueChagnedType, bool modified)
        {
            if (this.modified == false && modified == true)
                SetModified();
        }

        private void Modeller_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                Delete();
            }
            //else if (e.KeyCode == Keys.Enter)
            //{
            //    Test();
            //}
        }

        private void buttonToggleParameterPanel_Click(object sender, EventArgs e)
        {
            panelParameter.Visible = !panelParameter.Visible;
        }

        private void loadNextImage_Click(object sender, EventArgs e)
        {
            //if(currentModel.ImageBufferPathList.Count - 1 >= currentModel.NowImageBufferIndex + 1)
            //{
            //    int index = currentModel.NowImageBufferIndex + 1;
            //    currentModel.NowImageBufferIndex = index;
            //    currentModel.CurrentImageBufferPath = currentModel.ImageBufferPathList[index];
            //}
            UpdateModelInfo();
        }

        private void loadPreImage_Click(object sender, EventArgs e)
        {
            //if (currentModel.NowImageBufferIndex - 1 > 0)
            //{
            //    int index = currentModel.NowImageBufferIndex - 1;
            //    currentModel.NowImageBufferIndex = index;
            //    currentModel.CurrentImageBufferPath = currentModel.ImageBufferPathList[index];
            //}
            UpdateModelInfo();
        }

        private void buttonShowImageBufferList_Click(object sender, EventArgs e)
        {
            //ImageBufferListFormModeller form = new ImageBufferListFormModeller(currentModel);
            //if(form.ShowDialog() == DialogResult.OK)
            //{
            //    if (form.CurrentBufferIndex == -1)
            //        return;
            //    currentModel.CurrentImageBufferPath = currentModel.ImageBufferPathList[form.CurrentBufferIndex];
            //    currentModel.NowImageBufferIndex = form.CurrentBufferIndex;
            //    UpdateModelInfo();
            //}
        }

        private string GetshortPath(string path, int lenth = 3)
        {
            string[] shortPath = path.Split('\\');
            return shortPath[shortPath.Length - lenth];
        }

        private MPAlignment.Data.InspectionResult CreateInspectionResult(string path)
        {
            MPAlignment.Data.InspectionResult inspectionResult = new MPAlignment.Data.InspectionResult();
            string defectPath = String.Format("{0}\\result.csv", path);            
            try
            {
                using (StreamReader reader = new StreamReader(defectPath, Encoding.Default))
                {
                    reader.ReadLine(); // Skip
                    string[] words = reader.ReadLine().Split(new char[] { ',' });

                    while (reader.Peek() >= 0)
                    {
                        string[] words2 = reader.ReadLine().Split(new char[] { ',' });
                        if (words2[0].Trim() == "ProbeResult2")
                        {
                            int inspectStep = Convert.ToInt32(words2[1].Trim());
                            int targetGroupId = Convert.ToInt32(words2[2].Trim());
                            int targetId = Convert.ToInt32(words2[3].Trim());
                            //zmsong
                            //string targetFullId = String.Format("{0:00}.{1:00}.{2:000}", inspectStep, targetGroupId, targetId);
                            //ProbeResult probeResult = new ProbeResult();
                            //probeResult.TargetId = targetFullId;
                            //probeResult.Judgment = GetStingJudgement(words2[8]);
                            //inspectionResult.AddProbeResult(probeResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Operation, "CreateInspectionResult Failed");
            }
            return inspectionResult;
        }

        private Judgment GetStingJudgement(string judgementString)
        {
            //        Accept, Reject, FalseReject, Skip, Warn
            Judgment judgment = Judgment.Accept;
            switch (judgementString.Trim())
            {
                case "Good":
                    judgment = Judgment.Accept;
                    break;
                case "Overkill":
                    judgment = Judgment.FalseReject;// Overkill;
                    break;
                case "NG":
                    judgment = Judgment.Reject;
                    break;
            }
            return judgment;
        }

        private void ModellerDefault_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SystemManager.Instance().CurrentModel.NowImageBufferIndex = 0;
        }

        private void checkBoxAllLightOnOff_CheckedChanged(object sender, EventArgs e)
        {
            if (currentModel == null)
                return;

            if (lockUpdateImage == true)
                return;

            LogHelper.Debug(LoggerType.Operation, "Modeller - useLightList_SelectedIndexChanged");

            SetModified();
        }

        private void panelLightControl_Paint(object sender, PaintEventArgs e)
        {

        }

        static int lightval = 1;
        private void btnInspect_Click(object sender, EventArgs e)
        {
            // Inspect();
            if (lightval > 10)
            {
                lightval = 5;
                DevCtroller.MoveXY(500, 500);
            }
            else
            {
                DevCtroller.MoveXY(400, 400);
                lightval = 55;
            }


            var image = DevCtroller.Grab_Macro(lightval);
            if (image != null)
            {
                var bitmap = image.ToBitmap();
                if (bitmap != null)
                    cameraImagePanel.UpdateImage(bitmap);
                else
                {
                }
            }
            else
            {

            }

        }

        

        private  void  btnAddTarget_Click(object sender, EventArgs e)
        {
            cameraImagePanel.SetAddMode(FigureType.Rectangle);
            //DevCtroller.CheckOrigin(true);


        }

        private async void btnGrab_Clicked(object sender, EventArgs e)
        {
            if (currentModel == null)
                return;

            var task = ((UniScanX.MPAlignment.Operation.InspectRunner)SystemManager.Instance().InspectRunner)
                .Scan_WholePatternArea(currentModel.PatternSize);
   
            await task;
            if (task.Result != null)
            {
                cameraImagePanel.UpdateImage(task.Result);
            }
        }


        

        private void btnSave_Click(object sender, EventArgs e)
        {
            //  currentModel.SaveModel(); //zmsong
            DevCtroller.MoveXY(500, 500);
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            DevCtroller.MoveXY(400, 400);
            //OpenFileDialog diag = new OpenFileDialog();
            //diag.Filter = "bitmap (*.bmp)|*.bmp";
            //if(diag.ShowDialog() == DialogResult.OK) //zmsong
            //{
            //    //Camera camera = DeviceManager.Instance.CameraHandler.GetCamera(0);

            //    //Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(diag.FileName);
            //    //if (bitmap.Width != camera.ImageSize.Width || bitmap.Height != camera.ImageSize.Height)
            //    //{
            //    //    var messageForm = new NoticeMessageForm("Error", "Load fail. your selected image is not available.");
            //    //    messageForm.Show();
            //    //    return;
            //    //}

            //   // UpdateCameraImage(bitmap);
            //}
        }

        private void btnDeleteTarget_Click(object sender, EventArgs e)
        {
           // Delete();
           
        }

        private void btnCopyTarget_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void btnPasteTarget_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private async void btnConveyorLoad_Click(object sender, EventArgs e)
        {
            //AoiPortMap portMap = (AoiPortMap)DeviceManager.Instance.PortMap;
            //InPortListCV_100 inPorts = (InPortListCV_100)portMap.GetInPort();
            //OutPortListCV_100 outPorts = (OutPortListCV_100)portMap.GetOutPort();

            //DigitalIoHandler digitalIoHandler = DeviceManager.Instance.DigitalIoHandler;
            //bool entrySensor = digitalIoHandler.ReadInput(inPorts.EntrySensor);
            //bool outReadySensor = digitalIoHandler.ReadInput(inPorts.OutReadySensor);

            //if (entrySensor)
            //{
            //    DeviceManager.Instance.ConveyorUnit.ReceiveOnce();
            //}
            //else if (outReadySensor)
            //{
            //    DeviceManager.Instance.ConveyorUnit.ReceiveBack();
            //}

            //DeviceManager.Instance.ConveyorUnit.PcbReady += PcbReady;
        }

        private void IsStopConveyor()
        {
            //DeviceManager.Instance.ConveyorUnit.PcbEjectReady -= IsStopConveyor;
            //DeviceManager.Instance.ConveyorUnit.ReceiveBack();
            //DeviceManager.Instance.ConveyorUnit.PcbReady += PcbReady;
        }

        private void PcbReady()
        {
        //    if (InvokeRequired)
        //    {
        //        Invoke(new MethodInvoker(delegate { PcbReady(); }));
        //        return;
        //    }

        //    DeviceManager.Instance.ConveyorUnit.PcbReady -= PcbReady;

        //    GrabImage();
        //    //btnConveyorLoad.Enabled = true;
        }

        private void btnFidMgr_Click(object sender, EventArgs e)
        {
            //if (selectedTargetGroup == null)
            //    return;

            //FiducialSet fiducialSet = selectedTargetGroup.FiducialSet;

            //fiducialMgrForm.UpdateData(fiducialSet, curImage);
            //if (fiducialMgrForm.Visible == false)
            //    fiducialMgrForm.Show();
            //else
            //    fiducialMgrForm.Hide();
        }

        private void btnLoadAverage_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog dlg = new FolderBrowserDialog();
            //if (dlg.ShowDialog() == DialogResult.OK)
            //{
            //    AvgImageBuilder avgImageBuilder = new AvgImageBuilder();

            //    string[] fileNames = Directory.GetFiles(dlg.SelectedPath);
            //    for (int i=0; i< Math.Min(10, fileNames.Count()); i++)
            //    {
            //        string fileName = fileNames[i];

            //        Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(fileName);

            //        FiducialSet fiducialSet = selectedTargetGroup.FiducialSet;
            //        if (fiducialSet.Count > 0)
            //        {
            //            List<Bitmap> grabImageList = new List<Bitmap>();
            //            grabImageList.Add((Bitmap)bitmap);

            //            InspParam inspParam = new InspParam(grabImageList, true, false, false);
            //            InspectionResult inspectionResult = new InspectionResult();

            //            fiducialSet.Inspect(inspParam, inspectionResult);

            //            PositionAligner positionAligner = fiducialSet.Calculate(inspectionResult);

            //            Bitmap alignedImage = (Bitmap)ImageHelper.TransformImage(bitmap, positionAligner.Offset, positionAligner.RotationCenter, positionAligner.Angle, true);

            //            avgImageBuilder.Add(alignedImage);
            //        }
            //    }

            //    avgImageBuilder.Calculate();
            //    cameraImagePanel.UpdateImage(avgImageBuilder.GetStdDevNormImage(10));
            //}
        }

        private void btnCentroidView_Click(object sender, EventArgs e)
        {
            //CentroidViewForm centroidView = new CentroidViewForm();
            //centroidView.Initialize(cameraImagePanel.CloneImage());
            //if (centroidView.ShowDialog() == DialogResult.OK)
            //{
            //    List<Target> targetList = new List<Target>();

            //    foreach (Centroid centroid in centroidView.CentroidList)
            //    {
            //        RotatedRect targetRect = DrawingHelper.FromCenterSizeAngle(new PointF(centroid.PositionX, centroid.PositionY), new SizeF(15, 15), centroid.Angle);
            //        Figure rectangleFigure = new RectangleFigure(targetRect, Target.TargetPen);
            //        Target target = AddTarget(rectangleFigure, cameraImagePanel.WorkingFigures, false);
            //        if (target != null)
            //        {
            //            target.Name = centroid.RefName;
            //            target.TypeName = centroid.PartType;

            //            targetList.Add(target);
            //        }
            //    }

            //    TargetAdded(targetList);

            //    cameraImagePanel.Invalidate();
            //}
        }


        private void cmbTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (onUpdate == true)
            //    return;

            //string targetTypeName = (string)cmbTargetType.Items[cmbTargetType.SelectedIndex];

            //List<Figure> targetFigureList = new List<Figure>();
            //foreach (Target target in selectedTargetGroup.TargetList)
            //{
            //    if (target.TypeName == targetTypeName)
            //    {
            //        Figure figure = cameraImagePanel.WorkingFigures.GetFigureByTag(target);
            //        targetFigureList.Add(figure);

            //    }
            //}

            //cameraImagePanel.SelectFigure(targetFigureList);
            //UpdateTargetSelector();

            //cameraImagePanel.Invalidate();
        }

        private void btnEjectPcb_Click(object sender, EventArgs e)
        {
            //DeviceManager.Instance.ConveyorUnit?.EjectPCB();
        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            cameraImagePanel.ZoomIn();
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            cameraImagePanel.ZoomOut();
        }

        private void buttonZoomFit_Click(object sender, EventArgs e)
        {
            cameraImagePanel.ZoomFit();
        }

        private void ckbShowAllResult_CheckedChanged(object sender, EventArgs e)
        {
           // UpdateTargetSelector();
        }

        private void ModellerPage_VisibleChanged(object sender, EventArgs e)
        {
            //if (this.Visible == true)
            //{
            //    var getModel = ((MPAlignment.Data.ModelManager)(SystemManager.Instance().ModelManager)).CurrentModel;
            //    if(this.currentModel != getModel)
            //        Initialize(getModel);
            //}
        }

        static bool Running = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Running == true) return;
            Running = true;

            if (lightval > 10)
            {
                lightval = 5;
                DevCtroller.MoveXY(500, 500);
            }
            else
            {
                DevCtroller.MoveXY(400, 400);
                lightval = 55;
            }


            var image = DevCtroller.Grab_Macro(lightval);
            if (image != null)
            {
                var bitmap = image.ToBitmap();
                if (bitmap != null)
                    cameraImagePanel.UpdateImage(bitmap);
                else
                {
                }
            }
            else
            {

            }

            Running = false;
        }


    }
}
 