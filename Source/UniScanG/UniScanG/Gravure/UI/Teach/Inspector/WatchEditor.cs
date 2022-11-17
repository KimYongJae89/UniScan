using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Data;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class WatchEditor : UserControl
    {
        public float scale = 1.0f;
        ImageD fullImageD = null;
        CanvasPanel canvasPanel = null;
        List<ExtItem> watchItemList = null;
        List<ExtItem> watchItemListWork = null;
        List<ExtItem> watchItemListDone = null;
        UniScanG.Data.OffsetStructSet offsetStructSet = null;

        bool onUpdate = false;
        List<ExtItem> selectedWatchItemList = new List<ExtItem>();

        public List<ExtItem> WatchItemListDone { get { return watchItemListDone; } }

        public WatchEditor(float scale)
        {
            InitializeComponent();

            this.scale = scale;

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.SetPanMode();
            this.canvasPanel.ShowCenterGuide = false;
            this.canvasPanel.RotationLocked = true;
            this.canvasPanel.FastMode = true;

            this.canvasPanel.FigureCreated = canvasPanel_FigureCreated;
            this.canvasPanel.FigureRemoved = canvasPanel_FigureRemoved;
            this.canvasPanel.FigureModified = canvasPanel_FigureModified;
            this.canvasPanel.FigureSelected = canvasPanel_FigureSelected;

            this.canvasPanel.KeyDown += canvasPanel_KeyDown;

            this.panelImage.Controls.Add(this.canvasPanel);

            if (AlgorithmSetting.Instance().UseExtObserve)
            {
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.CHIP));
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.FP));
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.INDEX));
            }

            if (AlgorithmSetting.Instance().UseExtStopImg)
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.StopIMG));

            if (AlgorithmSetting.Instance().UseExtMargin)
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.Margin));

            if (AlgorithmSetting.Instance().UseExtTransfrom)
                toolStripSplitButtonAdd.DropDownItems.Add(BuildToolStripMenuItem(ExtType.PMVariance));
        }

        private ToolStripMenuItem BuildToolStripMenuItem(ExtType watchType)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = string.Format("toolStripMenuItem{0}", watchType);
            item.Text = watchType.ToString();
            item.Tag = watchType;
            item.Size = new System.Drawing.Size(180, 26);
            item.Click += new System.EventHandler(this.toolStripSplitButtonAdd_ButtonClick);
            return item;
        }

        private void canvasPanel_FigureSelected(List<Figure> figureList)
        {
            this.selectedWatchItemList.Clear();
            this.canvasPanel.TempFigures.Clear();

            figureList.ForEach(f =>
            {
                ExtItem selWatchItem = f.Tag as ExtItem;
                if (selWatchItem != null)
                {
                    this.selectedWatchItemList.Add(selWatchItem);
                    this.canvasPanel.TempFigures.AddFigure(selWatchItem.GetTempFigure(this.offsetStructSet));
                }
            });

            UpdateData();
        }

        private void canvasPanel_FigureModified(List<Figure> figureList)
        {
            figureList.ForEach(f =>
            {
                ExtItem selWatchItem = f.Tag as ExtItem;
                if (selWatchItem != null)
                {
                    UpdateExtItem(selWatchItem, f);
                }
            });
            UpdateFigure(false);
        }

        private void UpdateExtItem(ExtItem extItem, Figure f)
        {
            Point offset = Point.Empty;
            if (this.offsetStructSet != null)
                offset = this.offsetStructSet.GetOffset(extItem.ContainerIndex);

            Rectangle figureRect = f.GetRectangle().ToRectangle();
            figureRect = DrawingHelper.Offset(figureRect, offset, true);
            Rectangle masterRect = DrawingHelper.Mul(figureRect, 1 / this.scale);
            ImageD imageD = this.fullImageD.ClipImage(masterRect);
            extItem.MasterImageD = imageD;

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            RectangleF masterRectUm = calibration.PixelToWorld(masterRect);
            extItem.SetMasterRectangleUm(masterRectUm);
        }

        private void canvasPanel_KeyDown(object sender, KeyEventArgs e)
        {
            Point pt = Point.Empty;

            Keys ketData = e.KeyData & ~(Keys.Control | Keys.Shift);
            bool control = (e.KeyData & Keys.Control) > 0;
            bool shift = (e.KeyData & Keys.Shift) > 0;

            if (control)
            {
                int diff = shift ? 1 : 50;
                switch (ketData)
                {
                    case Keys.Left:
                        pt.X = -diff;
                        break;
                    case Keys.Up:
                        pt.Y = -diff;
                        break;
                    case Keys.Right:
                        pt.X = +diff;
                        break;
                    case Keys.Down:
                        pt.Y = +diff;
                        break;
                }
            }

            if (pt.IsEmpty == false)
            {
                selectedWatchItemList.ForEach(f => f.Offset(pt));
                UpdateFigure(false);
            }
        }

        private void UpdateData()
        {
            ExtItem watchItem = selectedWatchItemList.FirstOrDefault();
            if (watchItem == null || selectedWatchItemList.Count != 1)
            {
                this.toolStripType.Text = "";
                this.toolStripId.Text = "";
                this.toolStripName.Text = "";
                this.toolStripName.Enabled = false;
            }
            else
            {
                this.toolStripType.Text = watchItem.ExtType.ToString();
                this.toolStripId.Text = watchItem.Index.ToString();
                this.toolStripName.Text = watchItem.Name.ToString();
                this.toolStripName.Enabled = true;
            }

            this.toolStripLabelSelCount.Text = string.Format(StringManager.GetString(this.GetType().FullName, "{0} Item Selected"), selectedWatchItemList.Count);
        }

        private void canvasPanel_FigureCreated(Figure figure, CoordMapper coordMapper)
        {
            ExtItem extItem = null;
            switch (this.extType)
            {
                default:
                case ExtType.NONE:
                    break;

                case ExtType.CHIP:
                case ExtType.FP:
                case ExtType.INDEX:
                    extItem = new Observer(extType);
                    break;

                case ExtType.StopIMG:
                    extItem = new StopImg();
                    break;

                case ExtType.Margin:
                    extItem = new Margin();
                    break;

                case ExtType.PMVariance:
                    extItem = new Transform();
                    break;
            }

            if (extItem == null)
                return;

            extItem.Use = true;
            extItem.Index = -1;
            extItem.Name = "";

            UpdateExtItem(extItem, figure);
            this.watchItemListWork.Add(extItem);
            this.extType = ExtType.NONE;
            UpdateFigure(true);

            this.canvasPanel.SetPanMode();
        }

        private void canvasPanel_FigureRemoved(List<Figure> figureList)
        {
            this.selectedWatchItemList.ForEach(f => watchItemListWork.Remove(f));
            UpdateFigure(true);
            UpdateData();
        }

        public void Initialize(ImageD imageD, List<ExtItem> watchItemList, UniScanG.Data.OffsetStructSet offsetStructSet)
        {
            this.fullImageD = imageD;
            Bitmap bitmap = null;
            if (this.scale < 1)
            {
                Size size = Size.Round(new SizeF(imageD.Width * this.scale, imageD.Height * this.scale));
                //AlgoImage algoImage = ImageBuilder.Build(UniEye.Base.Settings.OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                //AlgoImage algoImage2 = ImageBuilder.Build(UniEye.Base.Settings.OperationSettings.Instance().ImagingLibrary, ImageType.Grey, size);
                //ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

                AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy("Resizer");
                if (algorithmStrategy == null)
                    algorithmStrategy = new AlgorithmStrategy("Resizer", UniEye.Base.Settings.OperationSettings.Instance().ImagingLibrary, "");

                AlgoImage algoImage = ImageBuilder.Build(algorithmStrategy.LibraryType, imageD, ImageType.Grey);
                AlgoImage algoImage2 = ImageBuilder.Build(algorithmStrategy.LibraryType, ImageType.Grey, size);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

                ip.Resize(algoImage, algoImage2);
                bitmap = algoImage2.ToBitmap();
                algoImage2.Dispose();
                algoImage.Dispose();
            }
            else
                bitmap = imageD.ToBitmap();

            this.canvasPanel.UpdateImage(bitmap);

            this.watchItemList = watchItemList;

            this.watchItemListWork = new List<ExtItem>();
            this.watchItemList.ForEach(f => this.watchItemListWork.Add(f.Clone()));

            this.offsetStructSet = offsetStructSet;
            UpdateFigure(true);
        }

        private void UpdateFigure(bool clearSelection)
        {
            this.canvasPanel.WorkingFigures.Clear();
            this.canvasPanel.BackgroundFigures.Clear();
            this.canvasPanel.TempFigures.Clear();

            if (clearSelection)
            {
                this.canvasPanel.ClearSelection();
                this.selectedWatchItemList.Clear();
            }

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

            foreach (ExtItem watchItem in this.watchItemListWork)
            {
                Point offset = Point.Empty;
                if (this.offsetStructSet != null)
                    offset = this.offsetStructSet.GetOffset(watchItem.ContainerIndex);

                Figure figure = watchItem.GetFigure(this.offsetStructSet, calibration);
                //figure.Offset(offset);
                figure.Scale(this.scale);
                this.canvasPanel.WorkingFigures.AddFigure(figure);
                //if (watchItem.WatchType == WatchType.PMVariance)
                //{
                //    this.canvasPanel.BackgroundFigures.AddFigure(figure);
                //}
                //else
                //{
                //    figure.Tag = watchItem;
                //    this.canvasPanel.WorkingFigures.AddFigure(figure);
                //}
            }
            this.selectedWatchItemList.ForEach(f => 
            {
                Figure figure = f.GetTempFigure(this.offsetStructSet);
                figure.Scale(this.scale);
                this.canvasPanel.TempFigures.AddFigure(figure);
            });
            this.canvasPanel.Invalidate();
        }

        private void RegionEditor_SizeChanged(object sender, EventArgs e)
        {
            this.canvasPanel.ZoomFit();
        }

        private void Apply()
        {
            //UpdateIndex();
            UpdateFigure(true);
            this.watchItemListDone = this.watchItemListWork.Select(f => f.Clone()).ToList();
            //watchItemListWork.ForEach(f => watchItemListDone.Add(f.Clone()));
        }

        private void UpdateIndex()
        {
            Array enums = Enum.GetValues(typeof(ExtType));
            foreach (ExtType e in enums)
            {
                UpdateIndex(e);
            }

            //List<ExtItem> founded = this.watchItemListWork.FindAll(f => f.WatchType == );
            //founded.ForEach(f => f.Index = -1);

            //founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.CHIP);
            //for (int i = 0; i < founded.Count; i++)
            //    founded[i].Index = i;

            //founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.FP);
            //for (int i = 0; i < founded.Count; i++)
            //    founded[i].Index = i;

            //founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.INDEX);
            //for (int i = 0; i < founded.Count; i++)
            //    founded[i].Index = i;

            //UpdateIndex(WatchType.CHIP);
            //UpdateIndex(WatchType.INDEX);
            //UpdateIndex(WatchType.Margin);
            //UpdateIndex(WatchType.PMVariance);


        }

        private void UpdateIndex(ExtType watchType)
        {
            List<ExtItem> founded = this.watchItemListWork.FindAll(f => f.ExtType == watchType);
            for (int i = 0; i < founded.Count; i++)
                if (!founded[i].IsFixed)
                {
                    founded[i].Index = (watchType == ExtType.NONE ? -1 : i);
                    if (string.IsNullOrEmpty(founded[i].Name))
                        founded[i].Name = founded[i].Index.ToString();
                }
        }

        private void Reset()
        {
            this.watchItemListWork = this.watchItemList.Select(f => f.Clone()).ToList();
        }

        ExtType extType = ExtType.NONE;
        private void toolStripSplitButtonAdd_ButtonClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                if (item.Tag is ExtType)
                    this.extType = (ExtType)item.Tag;
                else
                    this.extType = ExtType.NONE;

                if (this.extType != ExtType.NONE)
                    this.canvasPanel.SetAddMode(FigureType.Rectangle);
            }
        }

        private void addBlockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvasPanel.SetAddMode(FigureType.Rectangle);
        }

        private void addPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvasPanel.SetAddMode(FigureType.Rectangle);
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageForm.Show(null, string.Format(StringManager.GetString("{0} Item(s) will be Remove."), this.selectedWatchItemList.Count), MessageFormType.YesNo);
            if (dialogResult == DialogResult.No)
                return;

            this.selectedWatchItemList.ForEach(f =>
            {
                ExtItem watchItem = f as ExtItem;
                if (!f.IsFixed)
                    this.watchItemListWork.Remove(watchItem);
            });
            selectedWatchItemList.Clear();

            this.canvasPanel.ClearSelection();
            UpdateFigure(true);
            UpdateData();
        }

        private void toolStripButtonAll_Click(object sender, EventArgs e)
        {
            selectedWatchItemList.Clear();
            this.canvasPanel.ClearSelection();

            selectedWatchItemList.AddRange(watchItemListWork);
            this.canvasPanel.SelectFigure(this.canvasPanel.WorkingFigures.FigureList);
            UpdateData();
            UpdateFigure(false);
        }

        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            Reset();
            UpdateFigure(true);
            MessageForm.Show(null, "Reset Success");
        }

        private void toolStripButtonApply_Click(object sender, EventArgs e)
        {
            Apply();
            MessageForm.Show(null, "Apply Success");
        }

        private void toolStripName_TextChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (this.selectedWatchItemList.Count != 1)
                return;

            ExtItem watchItem = selectedWatchItemList.FirstOrDefault();
            if (watchItem == null)
                return;

            watchItem.Name = toolStripName.Text;
        }

    }
}
