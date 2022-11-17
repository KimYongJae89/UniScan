using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Devices;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public partial class SchemaViewer : UserControl, InspectionResultVisualizer
    {
        public AddRegionCapturedDelegate AddRegionCaptured;
        public FigureSelectedDelegate FigureSelected;
        public FigureMovedDelegate FigureMoved;
        public FigureCopyDelegate FigureCopy;

        private Schema schema = new Schema();
        public Schema Schema
        {
            get { return schema; }
            set 
            {
                schema = value;
                Invalidate();
            }
        }

        private bool appendMode;
        private Tracker tracker;

        private bool enable = false;
        public bool Enable
        {
            set
            {
                enable = value;
                tracker.Enable = value;
            }
        }

        private bool showResultValue = true;
        public bool ShowResultValue
        {
            set { showResultValue = value; }
        }

        private bool addFigureMode;
        public bool AddFigureMode
        {
            set
            {
                addFigureMode = value;
                tracker.AddFigureMode = value;
            }
        }

        public bool RotationLocked
        {
            set { tracker.RotationLocked = value; }
        }

        public SchemaViewer()
        {
            InitializeComponent();
            tracker = new Tracker(this);
            tracker.Enable = true;
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = Tracker_AddFigureCaptured;
        }

        private void Tracker_AddFigureCaptured(List<PointF> pointList)
        {
            ResetSelection();
//            AddRegionCaptured(rectangle, startPos, endPos);
        }

        public void AddFigure(Figure figure)
        {
            schema.AddFigure(figure);
            Invalidate();
        }

        public void DeleteAll()
        {
            foreach (Figure figure in tracker)
                schema.RemoveFigure(figure);

            tracker.ClearFigure();

            Invalidate();
        }

        public void Delete(Figure figure)
        {
            schema.RemoveFigure(figure);
            Invalidate();
        }

        public void ResetSelection()
        {
            Debug.WriteLine("SchemaViewer.ResetSelection");

            tracker.ClearFigure();

            if (FigureSelected != null)
                FigureSelected(null);
        }

        public void SelectFigure(Figure figure)
        {
            Debug.WriteLine("SchemaViewer.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void SelectFigureByTag(Object tag)
        {
            Debug.WriteLine("SchemaViewer.SelectFigureByTag");
            tracker.AddFigure(schema.FigureGroup.GetFigureByTag(tag));
        }

        public void SelectFigure(List<Figure> figureList)
        {
            tracker.AddFigure(figureList);
            Invalidate();
        }

        public void SelectFigureByCrosshair(List<Figure> figureList)
        {
            RotatedRect unionRect = new RotatedRect();
            bool firstFigure = true;
            foreach (Figure figure in figureList)
            {
                if (firstFigure == true)
                {
                    unionRect = figure.GetRectangle();
                    firstFigure = false;
                }
                else
                {
                    unionRect = RotatedRect.Union(unionRect, figure.GetRectangle());
                }
            }

            PointF centerPt = DrawingHelper.CenterPoint(unionRect);

            PointF horzStartPt = new PointF(schema.Region.Left, centerPt.Y);
            PointF horzEndPt = new PointF(schema.Region.Right, centerPt.Y);
            PointF vertStartPt = new PointF(centerPt.X, schema.Region.Top);
            PointF vertEndPt = new PointF(centerPt.X, schema.Region.Bottom);

            FigureGroup crosshairFigureList = new FigureGroup();

            crosshairFigureList.AddFigure(new LineFigure(horzStartPt, horzEndPt, new Pen(Color.Red)));
            crosshairFigureList.AddFigure(new LineFigure(vertStartPt, vertEndPt, new Pen(Color.Red)));

            schema.TempFigureGroup = crosshairFigureList;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (enable == false)
                return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            Debug.WriteLine("SchemaViewer.OnMouseDown");

            appendMode = (Control.ModifierKeys == Keys.Control);

            tracker.CoordTransformer = GetCoordTransformer();

            tracker.MouseDown(e.Location);

            base.OnMouseDown(e);
        }

        void Tracker_SelectionPointCaptured(Point point)
        {
            Debug.WriteLine("SchemaViewer.Tracker_SelectionPointCaptured");

            if (appendMode == false)
                ResetSelection();

            Figure figure = schema.FigureGroup.Select(point);
            if (figure != null)
            {
                tracker.AddFigure(figure);

                if (FigureSelected != null)
                    FigureSelected(figure);
            }

            Invalidate();
        }

        void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            Debug.WriteLine("SchemaViewer.Tracker_SelectionRectCaptured");

            if (addFigureMode == false)
            {
                if (appendMode == false)
                    ResetSelection();

                List<Figure> figureList = schema.FigureGroup.Select(rectangle);
                if (figureList.Count() > 0)
                {
                    tracker.AddFigure(figureList);

                    if (FigureSelected != null)
                    {
                        foreach (Figure figure in figureList)
                            FigureSelected(figure);
                    }
                }
            }

            Invalidate();
        }

        void Tracker_FigureMoved()
        {
            Debug.WriteLine("SchemaViewer.Tracker_FigureMoved");

            if (appendMode == true)
            {
                if (FigureCopy != null)
                    FigureCopy(tracker.GetFigureList());
            }
            else
            {
                if (FigureMoved != null)
                    FigureMoved(tracker.GetFigureList());
            }

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            Debug.WriteLine("SchemaViewer.OnMouseUp");

            tracker.MouseUp(e.Location);

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            Debug.WriteLine("SchemaViewer.OnMouseMove");

            tracker.MouseMove(e.Location);

            base.OnMouseMove(e);
        }

        private CoordTransformer GetCoordTransformer()
        {
            CoordTransformer coordTransformer = new CoordTransformer(schema.ViewScale);
//            if (schema.AutoFit)
            {
                coordTransformer.InvertY = schema.InvertY;
                coordTransformer.SetSrcRect(schema.Region);
                coordTransformer.SetDisplayRect(new RectangleF(0, 0, Width, Height));
            }

            return coordTransformer;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin OnPaint");

            base.OnPaint(e);

            if (schema != null)
            {
                CoordTransformer coordTransformer = GetCoordTransformer();

                schema.Draw(e.Graphics, coordTransformer, enable);
                tracker.Draw(e.Graphics, coordTransformer);
            }

            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End OnPaint");
        }

        private void menuMoveUp_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
                schema.MoveUp(figure);

            Invalidate();
        }

        private void menuMoveTop_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
                schema.MoveTop(figure);

            Invalidate();
        }

        private void SchemaViewer_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && enable == true)
            {
                Figure figure = tracker.GetFirstFigure();
                if (figure != null)
                {
                    visibleToolStripMenuItem.Checked = figure.Visible;
                }

                contextMenu.Show(this, e.Location);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                if (figure.Deletable == true)
                    schema.RemoveFigure(figure);
            }

            tracker.ClearFigure();

            Invalidate();
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            foreach (Figure figure in tracker)
                schema.MoveDown(figure);

            Invalidate();
        }

        private void moveBottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
                schema.MoveBottom(figure);

            Invalidate();
        }

        private void propertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tracker.IsSingleSelected() == true)
            {
                FigurePropertyForm figurePropertyForm = new FigurePropertyForm();
                figurePropertyForm.Figure = tracker.GetFirstFigure();
                figurePropertyForm.ShowDialog(this);
                Invalidate();
            }
        }

        private void visibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Figure figure in tracker)
            {
                figure.Visible = !figure.Visible;
            }

            tracker.ClearFigure();

            Invalidate();
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (AddRegionCaptured != null)
                AddRegionCaptured(new Rectangle(), new Point(), new Point());

            base.OnMouseDoubleClick(e);
        }

        public void Copy()
        {
            List<ICloneable> objList = new List<ICloneable>();
            foreach (Figure figure in tracker)
            {
                Figure copyFigure = (Figure)figure.Clone();

                objList.Add(copyFigure);

                figure.Offset(10, 10);
            }

            CopyBuffer.SetData(objList);
        }

        public void Paste()
        {
            List<ICloneable> objList = CopyBuffer.GetData();
            if (objList.Count == 0)
                return;

            foreach (ICloneable obj in objList)
            {
                try
                {
                    if (obj is Figure)
                    {
                        Figure srcFigure = (Figure)obj;

                        Figure figure = (Figure)srcFigure.Clone();

                        AddFigure(figure);

                        srcFigure.Offset(10, 10);
                    }
                }
                catch (InvalidCastException)
                {

                }
            }

            Invalidate();
        }

        public void Update(Model model)
        {
            if (model != null)
                schema = model.ModelSchema;
            else
                schema = new Schema();

            Invalidate();
        }

        private void UpdateJudgmentFigure(List<Figure> figureList, string figureId, bool result)
        {            
            foreach (Figure figure in figureList)
            {
                UpdateJudgmentFigure(figure as FigureGroup, figureId, result);
            }
        }

        private void UpdateValueFigure(List<Figure> figureList, string figureId, string valueStr)
        {
            foreach (Figure figure in figureList)
            {
                UpdateValueFigure(figure as FigureGroup, figureId, valueStr);
            }
        }

        public void BuildLinkFigures(Model model)
        {
            foreach(InspectionStep inspectionStep in model.InspectionStepList)
            {
                foreach (TargetGroup targetGroup in inspectionStep.TargetGroupList)
                {
                    foreach (Target target in targetGroup.TargetList)
                    {
                        target.ShemaFigures = schema.GetFigureByTag(target.FullId);

                        foreach (Probe probe in target.ProbeList)
                        {
                            probe.ShemaFigures = schema.GetFigureByTag(probe.FullId);
                        }
                    }
                }
            }
        }

        void UpdateTargetResult(Target target, InspectionResult targetInspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin UpdateTargetResult");

            List<Figure> targetFigureList = target.ShemaFigures;

            UpdateJudgmentFigure(targetFigureList, target.FullId, targetInspectionResult.Judgment == Judgment.Accept);

            int count = targetInspectionResult.Count();
            for (int i = 0; i<count; i++)
            {
                ProbeResult probeResult = targetInspectionResult[i];
                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateTargetResult - probe - 0"));

                Probe probe = probeResult.Probe;

                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateTargetResult - probe - 1"));

                List<Figure> probeFigureList = probe.ShemaFigures;

                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateTargetResult - probe {0} - 1", probe.FullId));

                //UpdateJudgmentFigure(probeFigureList, probe.FullId, probeResult.Judgment == Judgment.Accept);
                UpdateJudgmentFigure(probeFigureList, probe.FullId, probeResult.Judgment == Judgment.Accept);

                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateTargetResult - probe {0} - 2", probe.FullId));

                if (showResultValue)
                {
                    foreach (ProbeResultValue probeResultValue in probeResult.ResultValueList)
                    {
                        string probeResultId = probe.FullId + "." + probeResultValue.Name;

                        List<Figure> probeResultFigureList = schema.GetFigureByTag(probeResultId);
                        if (probeResultValue.Value is string)
                        {
                            UpdateValueFigure(probeResultFigureList, probeResultId, probeResultValue.Value.ToString());
                        }
                        else
                        {
                            try
                            {
                                float value = Convert.ToSingle(probeResultValue.Value);
                                UpdateValueFigure(probeResultFigureList, probeResultId, value.ToString("0.00"));
                            }
                            catch (InvalidCastException)
                            {

                            }
                        }
                    }
                }

                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateTargetResult - probe {0} - 3", probe.FullId));
            }

            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End UpdateTargetResult");
        }

        public void UpdateResult(Target target, InspectionResult targetInspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin UpdateResult Target");

            UpdateTargetResult(target, targetInspectionResult);

            Invalidate();

            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End UpdateResult Target");
        }

        public void UpdateResult(TargetGroup targetGroup, InspectionResult targetGroupInspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : Begin UpdateResult TargetGroup");

            LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateResult - targetGroup {0}", targetGroup.FullId));

            foreach (Target target in targetGroup)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateResult - search target result {0}", targetGroup.FullId));

                InspectionResult targetResult = targetGroupInspectionResult.GetTargetResult(target.FullId);
                UpdateTargetResult(target, targetResult);
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Finish UpdateResult - targetGroup {0}", targetGroup.FullId));

            Invalidate();

            LogHelper.Debug(LoggerType.Inspection, "SchemaViewer : End UpdateResult TargetGroup");
        }

        private int GetFigureTotalCount(InspectionResult inspectionResult)
        {
            int count = 0;

            if (count <= 0) return 0;
            else
            {
                ResultCount resultCount = new ResultCount();
                inspectionResult.GetResultCount(resultCount);
                foreach (KeyValuePair<string, int> pair in resultCount.numTargetTypeDefects)
                {
                    List<Figure> figureList = schema.GetFigureByTag("TargetType." + pair.Key);

                    foreach (Figure figure in figureList)
                    {
                        count++;
                    }
                }
            }
            return count * 100;
        }

        public void UpdateTargetType(InspectionResult inspectionResult)
        {
            ResultCount resultCount = new ResultCount();
            inspectionResult.GetResultCount(resultCount);

            foreach(KeyValuePair<string, int> pair in resultCount.numTargetTypeDefects)
            {
                List<Figure> figureList = schema.GetFigureByTag("TargetType." + pair.Key);

                foreach (Figure figure in figureList)
                {
                    UpdateValueFigure(figure as FigureGroup, pair.Key, pair.Value.ToString());
                }
            }

            Invalidate();
        }

        private void UpdateJudgmentFigure(FigureGroup figureGroup, string objectId, bool result)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find target figure - {0}", objectId));
                return;
            }

            Figure resultFigure = figureGroup.GetFigure("rectangle") as RectangleFigure;
            if (resultFigure == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find rectangle figure - {0}", objectId));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("UpdateJudgmentFigure - {0}", objectId));

            if (result == true)
            {
                resultFigure.TempBrush = new SolidBrush(Color.LimeGreen);
            }
            else
            {
                resultFigure.TempBrush = new SolidBrush(Color.Red);
            }
        }

        private void UpdateValueFigure(FigureGroup figureGroup, string objectId, string valueStr)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find target figure - {0}", objectId));
                return;
            }

            TextFigure textFigure = figureGroup.GetFigure("value") as TextFigure;
            if (textFigure == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find text figure - {0}", objectId));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Update Text - {0}", objectId));

            textFigure.Text = valueStr;
        }

        private void UpdateTargetTypeFigure(FigureGroup figureGroup, string objectId, string valueStr)
        {
            if (figureGroup == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find target figure - {0}", objectId));
                return;
            }

            TextFigure textFigure = figureGroup.GetFigure("value") as TextFigure;
            if (textFigure == null)
            {
                LogHelper.Warn(LoggerType.Inspection, String.Format("Can't find text figure - {0}", objectId));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Update Text - {0}", objectId));

            textFigure.Text = valueStr;
        }

        public void UpdateImage(ImageBuffer imageBuffer)
        {
            foreach (Figure figure in schema.FigureGroup)
            {
                if (figure is ImageFigure)
                {
                    if (figure.Tag is string)
                    {
                        string fullId = (string)figure.Tag;
                        string[] idList = fullId.Split(new char[]{'.'});
                        if (idList.Count() != 3)
                            continue;

                        ImageFigure imageFigure = (ImageFigure)figure;
                        imageFigure.Image = imageBuffer.GetImageBuffer2dItem(Convert.ToInt32(idList[1]), Convert.ToInt32(idList[2])).Image.ToBitmap();
                    }
                }
            }

            Invalidate();
        }

        private void SchemaViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetResult();
        }

        public void ResetResult()
        {
            schema.ResetTempProperty();

            foreach (Figure figure in schema.FigureGroup)
            {
                if (figure is FigureGroup)
                {
                    FigureGroup figureGroup = (FigureGroup)figure;

                    TextFigure textFigure = figureGroup.GetFigure("value") as TextFigure;
                    if (textFigure == null)
                        continue;

                    textFigure.Text = "0";
                }
            }
            Invalidate();
        }

        private void GetDefaultProperty(Figure figure)
        {
            if (figure is TextFigure)
            {
                TextFigure textFigure = figure as TextFigure;

                Schema.DefaultFigureProperty.Font = (Font)textFigure.FigureProperty.Font.Clone();
                Schema.DefaultFigureProperty.TextColor = textFigure.FigureProperty.TextColor;
                Schema.DefaultFigureProperty.Alignment = textFigure.FigureProperty.Alignment;
            }
            else if ((figure is LineFigure) || (figure is RectangleFigure) || (figure is EllipseFigure))
            {
                Schema.DefaultFigureProperty.Pen = (Pen)figure.FigureProperty.Pen.Clone();

                if ((figure is RectangleFigure) || (figure is EllipseFigure))
                {
                    Schema.DefaultFigureProperty.Brush = (SolidBrush)figure.FigureProperty.Brush.Clone();
                }
            }
        }

        private void setDefaultPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tracker.IsSingleSelected() == true)
            {
                Figure figure = tracker.GetFirstFigure();
                if (figure is FigureGroup)
                {
                    FigureGroup figureGroup = figure as FigureGroup;
                    foreach (Figure subFigure in figureGroup)
                    {
                        GetDefaultProperty(subFigure);
                    }
                }
                else
                {
                    GetDefaultProperty(figure);
                }
            }
        }

        private void backgroundColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            Color color;
            if (result == DialogResult.OK)
            {
                color = colorDialog1.Color;
                //Gradient(color);
                schema.BackColor = color;
            }

        }
        public void Gradient(Color StartColor)
        {
            Bitmap canvas = new Bitmap(this.Width, this.Height);
            Color middleColor = Color.FromArgb(150, 50, 30);
            Color endColor = Color.White;
            Graphics g = Graphics.FromImage(canvas);
            LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle, StartColor,
                                                            System.Drawing.Color.White, 0, false);
            ColorBlend cb = new ColorBlend();

            br.RotateTransform(90); 
            g.FillRectangle(br, this.ClientRectangle);

            this.BackgroundImage = canvas;
        }

        private void SchemaViewer_Load(object sender, EventArgs e)
        {
            //Gradient(Color.DarkSlateGray);
        }
    }
}
