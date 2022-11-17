using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using UniScanM.Algorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base;
using UniScanM.CGInspector.Data;
using UniScanM.Data;
using UniScanM.CGInspector.Algorithm;
using DynMvp.Data;
using UniScanM.CGInspector.Algorithm.Glass;

namespace UniScanM.CGInspector.Test
{
    public partial class AlgorithmSimulatorForm : Form
    {
        CanvasPanel canvasPanel = null;
        //TeachBox teachBox = null;
        CanvasPanel canvasPanel2 = null;
        UniScanM.Data.FigureDrawOption figureDrawOption = null;

        string opendFile = "";
        Image2D curImageD = null;
        Figure editFigure = null;

        Data.Model model;
        Data.InspectionResult curInspectionResult = null;
        Probe addProbe = null;

        public AlgorithmSimulatorForm()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.SetPanMode();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.MouseDoubleClick += CanvasPanel_MouseDoubleClick;
            //this.canvasPanel.FastMode = true;
            this.canvasPanel.ShowToolbar = true;
            this.canvasPanel.HorizontalAlignment = HorizontalAlignment.Center;
            this.canvasPanel.FigureSelected = CanvasPanel_FigureSelected;
            this.canvasPanel.FigureModified = CanvasPanel_FigureModified;
            this.canvasPanel.FigureCreated = CanvasPanel_FigureCreated;
            this.canvasPanel.FigureRemoved = CanvasPanel_FigureRemoved;
            panelCanvas.Controls.Add(canvasPanel);

            this.canvasPanel2 = new CanvasPanel();
            this.canvasPanel2.SetPanMode();
            this.canvasPanel2.Dock = DockStyle.Fill;
            this.canvasPanel2.MouseDoubleClick += CanvasPanel_MouseDoubleClick;
            //this.canvasPanel2.DrawScale = 0.5f;
            this.canvasPanel2.HorizontalAlignment = HorizontalAlignment.Center;
            panelResult.Controls.Add(canvasPanel2);

            this.roiX.Maximum = this.roiY.Maximum = this.roiW.Maximum = this.roiH.Maximum
                = this.blobMin.Maximum = this.blobMax.Maximum
                = int.MaxValue;

            this.binMin.Maximum = this.binMax.Maximum
                = byte.MaxValue;

            figureDrawOption = new UniScanM.Data.FigureDrawOption()
            {
                useTargetCoord = true,

                PatternConnection = true,

                TeachResult = new FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new DrawSet(null, null),
                    Invalid = new DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0xFF, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0xFF, 0x00))),

                    ShowText = true,
                    FontSet = new FontSet(new Font("Gulim", 20), Color.Yellow)
                },

                ProcessResult = new FigureDrawOptionProperty()
                {
                    ShowFigure = true,
                    Good = new DrawSet(new Pen(Color.FromArgb(64, 0x90, 0xEE, 0x90), 3), new SolidBrush(Color.FromArgb(32, 0x90, 0xEE, 0x90))),
                    Ng = new DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0x00, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0x00, 0x00))),
                    Invalid = new DrawSet(new Pen(Color.FromArgb(64, 0xFF, 0xFF, 0x00), 3), new SolidBrush(Color.FromArgb(32, 0xFF, 0xFF, 0x00))),

                    ShowText = true,
                    FontSet = new FontSet(new Font("Gulim", 20), Color.Red)
                }
            };

            this.dataGridView1.AutoGenerateColumns = true;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UpdateTitle();

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, ""));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(GlassAlgorithm.TypeName, ImagingLibrary.OpenCv, ""));
        }

        private void CanvasPanel_FigureRemoved(List<Figure> figureList)
        {
            Target target = this.model.GetTarget(0, 0, 0);
            figureList.ForEach(f =>
            {
                VisionProbe visionProbe = f.Tag as VisionProbe;
                target.RemoveProbe(visionProbe);
            });

            this.canvasPanel.Clear();
            target.AppendFigures(this.canvasPanel.WorkingFigures, null, true);
        }

        private void CanvasPanel_FigureCreated(Figure figure, CoordMapper coordMapper)
        {
            if (addProbe != null)
            {
                figure.Tag = addProbe;

                this.model.GetTarget(0, 0, 0).AddProbe(addProbe);
                addProbe.UpdateRegion(figure.GetRectangle(), null);

                addProbe = null;
                DrawFigure();
            }
        }

        private void CanvasPanel_FigureSelected(List<Figure> figureList)
        {
            this.editFigure = figureList.Find(f => f.Tag is VisionProbe && ((VisionProbe)f.Tag).InspAlgorithm is GlassAlgorithm);
            UpdateParam();
        }

        private void CanvasPanel_FigureModified(List<Figure> figureList)
        {
            figureList.ForEach(f =>
            {
                if(f.Tag is VisionProbe)
                {
                    VisionProbe visionProbe = (VisionProbe)f.Tag;
                    visionProbe.BaseRegion = f.GetRectangle();
                    visionProbe.Target.UpdateRegion(null);
                }
            });

            UpdateFigure();
            UpdateParam();
        }

        private void CanvasPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UpdateFigure();

            CanvasPanel canvasPanel = (CanvasPanel)sender;
            canvasPanel.ZoomFit();
        }

        public void LoadFile(string filePath)
        {
            try
            {
                SimpleProgressForm form = new SimpleProgressForm("Wait");
                form.Show(() =>
                {
                    curImageD?.Dispose();
                    curImageD = new Image2D();
                    curImageD.LoadImage(filePath);
                    this.opendFile = filePath;

                    //using (ImageD resize = ResizeImageD(curImageD))
                    //{
                    //    //teachBox.UpdateImage(resize);
                    //    Bitmap resizeBitmap = resize.ToBitmap();
                    //    this.canvasPanel.UpdateImage(resizeBitmap);
                    //}
                    this.canvasPanel.UpdateImage(curImageD.ToBitmap());
                });

                UpdateResult(null);
                UpdateTitle();
            }
            catch (Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
        }

        private ImageD ResizeImageD(ImageD curImageD)
        {
            Size resize = DrawingHelper.Mul(curImageD.Size, 0.5f);
            ImageD imageD = null;

            using (AlgoImage srcAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, curImageD, ImageType.Grey),
                dstAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Grey, resize))
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(srcAlgoImage);
                ip.Resize(srcAlgoImage, dstAlgoImage);
                imageD = dstAlgoImage.ToImageD();
            }

            return imageD;
        }

        private delegate void UpdateResultDelegate(DynMvp.InspData.InspectionResult inspectionResult);
        private void UpdateResult(DynMvp.InspData.InspectionResult inspectionResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectionResult);
                return;
            }

            this.curInspectionResult = inspectionResult as Data.InspectionResult;
            if (this.curInspectionResult == null)
                return;

            if (!this.curInspectionResult.AlgorithmResultLDic.ContainsKey(GlassAlgorithm.TypeName))
                return;

            GlassAlgorithmResult coverGlassAlgorithmResult = (GlassAlgorithmResult)this.curInspectionResult.AlgorithmResultLDic[GlassAlgorithm.TypeName];
            Bitmap drawBitmap = null;
            if (false)
            {
                using (ImageD cloneImageD = this.curImageD.Clone())
                {
                    ImageD resultImageD = coverGlassAlgorithmResult.ResultImageD;
                    cloneImageD.CopyFrom(resultImageD, Rectangle.Empty, resultImageD.Pitch, coverGlassAlgorithmResult.ClipRect.ToRectangle().Location);

                    using (ImageD resizeImageD = ResizeImageD(cloneImageD))
                    {
                        drawBitmap = resizeImageD.ToBitmap();
                    }
                }
            }
            else
            {
                //drawBitmap = (Bitmap)this.teachBox.Image.Clone();
                drawBitmap = (Bitmap)this.canvasPanel.Image.Clone();
            }
            this.canvasPanel2.UpdateImage(drawBitmap);

            this.labelResultCount.Text = coverGlassAlgorithmResult.DefectOnScreens.Length.ToString();
            this.dataGridView1.DataSource = coverGlassAlgorithmResult.DefectOnScreens;
        }

        private delegate void UpdateTitleDelegate();
        private void UpdateTitle()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateTitleDelegate(UpdateTitle));
                return;
            }

            if (string.IsNullOrEmpty(this.opendFile))
                this.Text = string.Format("Algorithm Simulator");
            else
                this.Text = string.Format("Algorithm Simulator - {0}", Path.GetFileName(this.opendFile));
        }

        private void buttonImageLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (UiHelper.ShowSTADialog(dlg) == DialogResult.OK)
            {
                LoadFile(dlg.FileName);
                UpdateFigure();
                UpdateParam();
            }
        }

        private void AlgorithmSimulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.curImageD?.Dispose();
        }

        private void AlgorithmSimulatorForm_SizeChanged(object sender, EventArgs e)
        {
            //this.teachBox.ZoomFit();
            this.canvasPanel?.ZoomFit();
            this.canvasPanel2?.ZoomFit();
        }

        private void AlgorithmSimulatorForm_Load(object sender, EventArgs e)
        {
            int maxX = Screen.AllScreens.Max(f => f.WorkingArea.X);
            Screen screen = Array.Find(Screen.AllScreens, f => f.WorkingArea.X == maxX);
            this.Location = screen.Bounds.Location;
            this.WindowState = FormWindowState.Maximized;

            this.model = new Data.Model();
            InspectionStep inspectionStep = this.model.CreateInspectionStep();
            TargetGroup targetGroup = inspectionStep.CreateTargetGroup();
            Target target = targetGroup.CreateTarget();

            string testFile = @"D:\UniScan\CGInspector\Developer\ImageD70.bmp";
            if (File.Exists(testFile))
            {
                LoadFile(testFile);
                DrawFigure();
            }
        }

        private void buttonDoProcess_Click(object sender, EventArgs e)
        {
            Rectangle roiRect = new Rectangle((int)this.roiX.Value, (int)this.roiY.Value, (int)this.roiW.Value, (int)this.roiH.Value);
            RotatedRect rotatedRect = new RotatedRect(roiRect, 0);
            ImageD clipImageD = this.curImageD.ClipImage(roiRect);
            Calibration calibration = AlgorithmBuilder.CreateCalibration();
            calibration.PelSize = new SizeF(10, 10);

            AlgorithmInspectParam algorithmInspectParam = new AlgorithmInspectParam(clipImageD)
            {
                ProbeRegionInFov = new RotatedRect(roiRect, 0),
                ClipRegionInFov = new RotatedRect(roiRect, 0),
                WholeImageSize = this.curImageD.Size,
                CameraCalibration = calibration,
                DebugContext = new DebugContext(true, @"C:\temp\CGInspector\")
            };

            GlassAlgorithm glassAlgorithm = new GlassAlgorithm();
            glassAlgorithm.Param = new GlassAlgorithmParam(
                new MinMaxPair<sbyte>((sbyte)this.binMin.Value, (sbyte)this.binMax.Value),
                new MinMaxPair<int>((byte)this.blobMin.Value, (byte)this.blobMax.Value),
                (int)this.closeNum.Value);

            GlassAlgorithmResult coverGlassAlgorithmResult = (GlassAlgorithmResult)glassAlgorithm.Inspect(algorithmInspectParam);

            Data.InspectionResult inspectionResult = new Data.InspectionResult();
            inspectionResult.AlgorithmResultLDic.Add(GlassAlgorithm.TypeName, coverGlassAlgorithmResult);

            UpdateResult(inspectionResult);
        }

        private void buttonSaveResult_Click(object sender, EventArgs e)
        {

        }

        private void roi_ValueChanged(object sender, EventArgs e)
        {
            if (this.editFigure == null)
                return;

            VisionProbe visionProbe = (VisionProbe)this.editFigure.Tag;

            RotatedRect rotatedRect = this.editFigure.GetRectangle();
            rotatedRect.X = (float)this.roiX.Value;
            rotatedRect.Y = (float)this.roiY.Value;
            rotatedRect.Width = (float)this.roiW.Value;
            rotatedRect.Height = (float)this.roiH.Value;
            rotatedRect.Angle = (float)this.roiR.Value;

            this.editFigure.SetRectangle(rotatedRect);
            canvasPanel.OnSelectionUpdated(this.editFigure);

            visionProbe.BaseRegion = rotatedRect;
            visionProbe.Target.UpdateRegion(null);

            UpdateFigure();
        }

        private void UpdateParam()
        {
            if (this.editFigure == null)
            {
                this.roiX.Enabled = this.roiY.Enabled = this.roiW.Enabled = this.roiH.Enabled = this.roiR.Enabled = false;
                return;
            }

            this.roiX.Enabled = this.roiY.Enabled = this.roiW.Enabled = this.roiH.Enabled = this.roiR.Enabled = true;
            RotatedRect rotatedRect = this.editFigure.GetRectangle();
            this.roiX.Value = (decimal)rotatedRect.X;
            this.roiY.Value = (decimal)rotatedRect.Y;
            this.roiW.Value = (decimal)rotatedRect.Width;
            this.roiH.Value = (decimal)rotatedRect.Height;
            this.roiR.Value = (decimal)rotatedRect.Angle;
        }

        private void DrawFigure()
        {
            Target target = this.model.InspectionStepList[0].TargetGroupList[0].TargetList[0];
            this.canvasPanel.Clear();
            target.AppendFigures(this.canvasPanel.WorkingFigures, null, true);

            this.canvasPanel.Invalidate();
        }

        private void UpdateFigure()
        {
            Target target = this.model.InspectionStepList[0].TargetGroupList[0].TargetList[0];
            //this.canvasPanel.WorkingFigures.Clear();
            target.AppendFigures(this.canvasPanel.WorkingFigures, null, true);

            this.canvasPanel.Invalidate();
            //Rectangle roiRect = new Rectangle((int)this.roiX.Value, (int)this.roiY.Value, (int)this.roiW.Value, (int)this.roiH.Value);

            //Figure roiFigure = this.canvasPanel.WorkingFigures.GetFigureByName("ROI");
            //if (roiFigure == null)
            //{
            //    roiFigure = new RectangleFigure(Rectangle.Empty, new Pen(Brushes.Yellow, 1));
            //    roiFigure.Name = "ROI";
            //    this.canvasPanel.WorkingFigures.AddFigure(roiFigure);
            //}

            //roiFigure.SetRectangle(new RotatedRect(roiRect, 0));
            //this.canvasPanel.Invalidate();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            if (dataGridView == null)
                return;

            if (dataGridView.SelectedRows.Count == 0)
                return;

            DefectOnScreen[] defectOnScreens = dataGridView.SelectedRows.Cast<DataGridViewRow>().Select(f => (DefectOnScreen)f.DataBoundItem).ToArray();
            DrawDefectFigure(defectOnScreens);
            //Data.DefectOnScreen defectOnScreen = ((Data.DefectOnScreen[])dataGridView.DataSource)[dataGridView.SelectedRows[0].Index] as Data.DefectOnScreen;
        }

        private void DrawDefectFigure(DefectOnScreen[] defectOnScreen)
        {
            if (this.curInspectionResult == null)
                return;

            if (!this.curInspectionResult.AlgorithmResultLDic.ContainsKey(GlassAlgorithm.TypeName))
                return;

            FigureGroup figureGroup = this.canvasPanel2.WorkingFigures.GetFigureByName("DefectGroup") as FigureGroup;
            if (figureGroup == null)
            {
                figureGroup = new FigureGroup("DefectGroup");// Rectangle.Empty, new Pen(Brushes.Red, 2));
                this.canvasPanel2.WorkingFigures.AddFigure(figureGroup);
            }

            figureGroup.Clear();

            Array.ForEach(defectOnScreen, f =>
            {
                Rectangle drawRect = f.Rectangle;
                RectangleFigure rectangleFigure = new RectangleFigure(drawRect, new Pen(Brushes.Red, 2));
                figureGroup.AddFigure(rectangleFigure);

                PolygonFigure polygonFigure = new PolygonFigure(true, new Pen(Brushes.Blue, 1));
                polygonFigure.AddPoints(Array.ConvertAll(f.Points, g => new PointF(g.X, g.Y)));
                figureGroup.AddFigure(polygonFigure);
            });

            GlassAlgorithmResult coverGlassAlgorithmResult = (GlassAlgorithmResult)this.curInspectionResult.AlgorithmResultLDic[GlassAlgorithm.TypeName];
            Point offset = coverGlassAlgorithmResult.ClipRect.ToRectangle().Location;

            figureGroup.Offset(offset);

            this.canvasPanel2.Invalidate();
        }

        private void buttonAddRoi_Click(object sender, EventArgs e)
        {
            VisionProbe visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);
            visionProbe.BaseRegion = new RotatedRect(3264, 4352, 7248, 13080, 0);
            visionProbe.InspAlgorithm = new GlassAlgorithm();
            visionProbe.InspAlgorithm.Param = new GlassAlgorithmParam(
                new MinMaxPair<sbyte>((sbyte)this.binMin.Value, (sbyte)this.binMax.Value),
                new MinMaxPair<int>((byte)this.blobMin.Value, (byte)this.blobMax.Value),
                (int)this.closeNum.Value);

            this.addProbe = visionProbe;

            this.canvasPanel.SetAddMode(FigureType.Rectangle);
            this.canvasPanel.Focus();
        }

        private void fastDraw_CheckedChanged(object sender, EventArgs e)
        {
            this.canvasPanel.FastMode = this.canvasPanel2.FastMode = this.fastDraw.Checked;           
        }
    }
}
