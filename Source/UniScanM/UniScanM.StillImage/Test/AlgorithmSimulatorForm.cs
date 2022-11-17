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
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Algorithm;
using UniScanM.Data;

namespace UniScanM.StillImage.Test
{
    public partial class AlgorithmSimulatorForm : Form
    {
        SheetFinder sheetFinder = null;
        LightTuner CheckBrightness = null;
        Teacher teacher = null;
        Inspector processer = null;

        DrawBox dBox = null;
        UniScanM.Data.FigureDrawOption figureDrawOption = null;

        string opendFile = "";
        Image2D curImageD = null;
        AlgoImage curAlgoImage = null;
        UniScanM.StillImage.Data.InspectionResult curInspectionResult = null;
        TeachData curTeachData = null;

        public AlgorithmSimulatorForm()
        {
            InitializeComponent();

            this.dBox = new DrawBox();
            this.dBox.Dock = DockStyle.Fill;
            this.dBox.AutoFitStyle = this.checkBoxFullScale.Checked ? AutoFitStyle.FitWidthOnly : AutoFitStyle.KeepRatio;
            panel1.Controls.Add(dBox);

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

            this.sheetFinder =new SheetFinderV1();
            this.CheckBrightness = new LightTunerV1();
            this.teacher = new TeacherV2_TCI();
            this.processer = Inspector.Create(0); //((InspectRunner)SystemManager.Instance().InspectRunner).InspectProcesser;

            UpdateTitle();

            string testFile = @"C:\Users\lotus\Desktop\GravureMonitor\2018_03_19\Grab\15_45_34_710.bmp";
            if (File.Exists(testFile))
                LoadFile(testFile);
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

                    curAlgoImage?.Dispose();
                    curAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, this.curImageD, ImageType.Grey);

                    opendFile = filePath;

                    Bitmap bitmap = curImageD.ToBitmap();
                    dBox.UpdateImage(bitmap);
                    bitmap.Dispose();
                });

                curInspectionResult = null;
                UpdateResult();
                UpdateTitle();
            }
            catch (Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
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

        private bool ShowMessage(bool rectExist, bool teached)
        {
            if (this.curAlgoImage == null)
            {
                MessageForm.Show(null, "There is no loaded image");
                return false;
            }

            if ((teached || rectExist) && (this.curInspectionResult == null || this.curInspectionResult.InspRectInSheet.IsEmpty))
            {
                //MessageForm.Show(null, "There is no Roi Data");
                this.curInspectionResult = new UniScanM.StillImage.Data.InspectionResult();
                SimpleProgressForm form = new SimpleProgressForm("Wait");
                form.Show(() => FovFind());
            }

            if (teached && (this.curTeachData == null))
            {
                //MessageForm.Show(null, "There is no Teached Data");
                SimpleProgressForm form = new SimpleProgressForm("Wait");
                form.Show(() => Teach());
            }

            return true;
        }

        private void buttonImageLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (UiHelper.ShowSTADialog(dlg) == DialogResult.OK)
                LoadFile(dlg.FileName);
        }

        private void buttonSheetFind_Click(object sender, EventArgs e)
        {
            if (ShowMessage(false, false) == false)
                return;

            this.curInspectionResult = new StillImage.Data.InspectionResult();
            SimpleProgressForm form = new SimpleProgressForm("Wait");
            form.Show(() =>
            {
                SheetFind();
            });
            UpdateResult();
        }

        private Rectangle SheetFind(UniScanM.Data.InspectionResult inspectionResult =null)
        {
            Rectangle sheetRect = this.sheetFinder.FindSheet(curAlgoImage, inspectionResult);
            if (sheetRect.Width > 0 && sheetRect.Height > 0)
            {
                this.curInspectionResult.SheetRectInFrame = sheetRect;
                this.curInspectionResult.SetGood();
            }
            return sheetRect;
        }

        private void buttonGetRoi_Click(object sender, EventArgs e)
        {
            if (ShowMessage(false, false) == false)
                return;

            this.curInspectionResult = new StillImage.Data.InspectionResult();
            SimpleProgressForm form = new SimpleProgressForm("Wait");
            form.Show(() =>
            {
                FovFind();
            });
            UpdateResult();
        }

        private void FovFind()
        {
            Rectangle sheetRect = SheetFind();
            int aSize = Math.Min(sheetRect.Width, sheetRect.Height);
            Size fovSize = new Size(aSize, aSize);

            Rectangle fovRect = SheetFinder.GetInspRect(sheetRect.Size, fovSize);
            this.curInspectionResult.InspRectInSheet = fovRect;
            //Rectangle roiRect = new Rectangle(Point.Empty, sheetRect.Size);
            //roiRect.Inflate(0, -100);
            //this.curInspectionResult.RoiRectInSheet = roiRect;
        }

        private void buttonClipSave_Click(object sender, EventArgs e)
        {
            if (ShowMessage(true, false) == false)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Bitmap|(*.bmp)";
            dlg.FileName = this.opendFile;
            if (UiHelper.ShowSTADialog(dlg) == DialogResult.Cancel)
                return;

            string path = Path.GetDirectoryName(dlg.FileName);
            string name = Path.GetFileNameWithoutExtension(dlg.FileName);
            string ext = Path.GetExtension(dlg.FileName);

            AlgoImage subImage = null;
            try
            {
                if (this.curInspectionResult.SheetRectInFrame.IsEmpty == false)
                {
                    subImage = curAlgoImage.GetSubImage(this.curInspectionResult.SheetRectInFrame);
                    subImage.Save(Path.Combine(path, string.Format("{0}_{1}.{2}", name, "Sheet", ext)));
                    subImage.Dispose();
                }

                if (this.curInspectionResult.InspRectInSheet.IsEmpty == false)
                {
                    subImage = curAlgoImage.GetSubImage(this.curInspectionResult.InspRectInSheet);
                    subImage.Save(Path.Combine(path, string.Format("{0}_{1}.{2}", name, "Roi", ext)));
                    subImage.Dispose();
                }

            }
            finally
            {
                subImage?.Dispose();
            }
        }

        private void buttonBrightness_Click(object sender, EventArgs e)
        {
            if (ShowMessage(true, false) == false)
                return;

            SimpleProgressForm form = new SimpleProgressForm("Wait");
            form.Show(() =>
            {
                Brightness();
            });
            UpdateResult();
        }

        private void Brightness()
        {
            AlgoImage subImage = null;
            try
            {
                Rectangle rect = (Rectangle)this.curInspectionResult.SheetRectInFrame;
                subImage = curAlgoImage.GetSubImage(rect);
                this.CheckBrightness.Tune(subImage, this.curInspectionResult);
            }
            finally
            {
                subImage?.Dispose();
            }
        }

        private void buttonTeaching_Click(object sender, EventArgs e)
        {
            if (ShowMessage(true, false) == false)
                return;

            SimpleProgressForm form = new SimpleProgressForm("Wait");
            form.Show(() =>
            {
                Teach();
            });
            UpdateResult();
        }

        private void Teach()
        {
            AlgoImage subImage = null;
            try
            {
                this.curInspectionResult.TeachData = null;
                this.curInspectionResult.ProcessResultList = null;

                Rectangle rect = (Rectangle)this.curInspectionResult.SheetRectInFrame;
                subImage = curAlgoImage.GetSubImage(rect);
                this.teacher.Teach(subImage, this.curInspectionResult);

                curTeachData = this.curInspectionResult.TeachData;
            }
            finally
            {
                subImage?.Dispose();
            }
        }

        private void buttonInspect_Click(object sender, EventArgs e)
        {
            if (ShowMessage(true, true) == false)
                return;



            SimpleProgressForm form = new SimpleProgressForm("Wait");
            form.Show(() =>
            {
                Inspect();
            });

            UpdateResult();
        }

        private void Inspect()
        {
            AlgoImage sheetImage = null;
            AlgoImage roiImage = null;
            try
            {
                this.curInspectionResult.ProcessResultList = null;
                this.curInspectionResult.TeachData = curTeachData;

                Rectangle sheetRect = (Rectangle)this.curInspectionResult.SheetRectInFrame;
                Rectangle fovRect = (Rectangle)this.curInspectionResult.InspRectInSheet;
                sheetImage = curAlgoImage.GetSubImage(sheetRect);
                roiImage = sheetImage.GetSubImage(fovRect);

                this.processer.Inspect(roiImage, (StillImage.Data.InspectParam)(SystemManager.Instance().CurrentModel).InspectParam, this.curInspectionResult);
            }
            finally
            {
                sheetImage?.Dispose();
                roiImage?.Dispose();
            }
        }

        private void UpdateResult()
        {
            this.dBox.FigureGroup.Clear();
            this.dataGridViewResult.Rows.Clear();
            this.dataGridViewTeach.Rows.Clear();
            this.dataGridViewInsp.Rows.Clear();

            if (this.curInspectionResult == null)
                return;

            this.dataGridViewResult.Rows.Add("SheetRect", Rect2String(curInspectionResult.SheetRectInFrame));

            if(curInspectionResult.InspRectInSheet.IsEmpty==false)
                this.dataGridViewResult.Rows.Add("FovRect", Rect2String(curInspectionResult.InspRectInSheet));
            Rectangle viewRect = curInspectionResult.InspRectInSheet;

            if (curInspectionResult.LightTuneResult != null)
                this.dataGridViewResult.Rows.Add("Brightness", "", "O" + curInspectionResult.LightTuneResult.OffsetLevel.ToString());

            if (curInspectionResult.TeachData != null)
            {
                //viewRect.Offset(curInspectionResult.TeachData.RoiRectInFov.Location);
                //viewRect = curInspectionResult.TeachData.ShapeOfInterest.Rectangle;
                viewRect.Offset(curInspectionResult.InspRectInSheet.Location);

                this.dataGridViewResult.Rows.Add("Teach", "C" + +curInspectionResult.TeachData.PatternInfoGroupList.Sum(f => f.ShapeInfoList.Count), "B" + curInspectionResult.TeachData.BinValue.ToString());
                foreach (PatternInfoGroup group in curInspectionResult.TeachData.PatternInfoGroupList)
                {
                    TeachInfo teachInfo = group.TeachInfo;
                    string s1 = string.Format("GID{0:D2}", group.Id);
                    string s2 = string.Format("A{0:F2} B{1:F2}/{2:F2} M{3:F2}/{4:F2}", teachInfo.Feature.Area, teachInfo.Feature.Blot.Width, teachInfo.Feature.Blot.Height, teachInfo.Feature.Margin.Width, teachInfo.Feature.Margin.Height);
                    string s3 = string.Format("C{0}", group.ShapeInfoList.Count());
                    this.dataGridViewResult.Rows.Add(s1, s2, s3);

                    foreach (ShapeInfo shapeInfo in group.ShapeInfoList)
                    {
                        int sId = shapeInfo.Id;
                        int gId = group.TeachInfo.Id;
                        string rect = Rect2String(shapeInfo.BaseRect);
                        if (group.TeachInfo.IsValid)
                        {
                            float area = group.TeachInfo.Feature.Area;
                            SizeF Blot = group.TeachInfo.Feature.Blot;
                            SizeF margin = group.TeachInfo.Feature.Margin;
                            // SID, GID, Rect, Area, Blot, Margin
                            this.dataGridViewTeach.Rows.Add(sId, gId, rect, string.Format("{0:F0}", area),
                                string.Format("W{0:F2}/L{1:F2}", Blot.Width, Blot.Height), string.Format("W{0:F2}/L{1:F2}", margin.Width, margin.Height));
                        }
                        else
                        {
                            // SID, GID, Rect, Area, Blot, Margin
                            this.dataGridViewTeach.Rows.Add(sId, gId, rect, "", "", "");
                        }
                    }
                }
            }

            if (curInspectionResult.ProcessResultList != null)
            {
                this.dataGridViewResult.Rows.Add("Process", "C" + curInspectionResult.ProcessResultList.ResultList.Count.ToString());
                List<ProcessResult> list = this.curInspectionResult.ProcessResultList.ResultList;
                foreach (ProcessResult item in list)
                {
                    int sId = item.InspPatternInfo.ShapeInfo.Id;
                    int gId = item.TrainPatternInfo.TeachInfo.Id;
                    string rect = Rect2String(item.InspPatternInfo.ShapeInfo.BaseRect);
                    if (item.IsInspected)
                    {
                        float area = item.OffsetValue.Area;
                        SizeF Blot = item.OffsetValue.Blot;
                        SizeF margin = item.OffsetValue.Margin;
                        // SID, GID, Rect, Area, Blot, Margin
                        int rowNo = this.dataGridViewInsp.Rows.Add(sId, gId, rect, string.Format("{0:F2}", area),
                            string.Format("W{0:F2}/L{1:F2}", Blot.Width, Blot.Height), string.Format("W{0:F2}/L{1:F2}", margin.Width, margin.Height));

                        this.dataGridViewInsp.Rows[rowNo].Cells[0].Style.BackColor = item.IsGood ? Color.White : Color.LightPink;

                    }
                    else
                    {
                        // SID, GID, Rect, Area, Blot, Margin
                        //this.dataGridViewInsp.Rows.Add(sId, gId, rect, "", "", "");
                    }

                }
            }

            if (viewRect.IsEmpty == false)
            {
                viewRect.Offset(curInspectionResult.SheetRectInFrame.Location);
                this.dBox.ScrollCenterTo(DrawingHelper.CenterPoint(viewRect));
            }

            this.curInspectionResult.AppendResultFigures(this.dBox.FigureGroup, figureDrawOption);
            this.dBox.Invalidate();
        }

        private string Rect2String(Rectangle rect)
        {
            return string.Format("X{0} Y{1} W{2} H{3}", rect.X, rect.Y, rect.Width, rect.Height);
        }

        private string Point2String(PointF point)
        {
            return string.Format("X{0} Y{1}", point.X, point.Y);
        }

        private void SheetFIndTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.curImageD?.Dispose();
            this.curAlgoImage?.Dispose();
        }

        private void SheetFIndTestForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.dBox != null)
                this.dBox.ZoomFit();
        }

        private void checkBoxFullScale_CheckedChanged(object sender, EventArgs e)
        {
            this.dBox.AutoFitStyle = (checkBoxFullScale.Checked ? AutoFitStyle.FitWidthOnly : AutoFitStyle.KeepRatio);
            this.dBox.ZoomFit();
        }

        private void SheetFIndTestForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
