using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Data;
using DynMvp.Vision;
using System.Diagnostics;
using DynMvp.Devices.Dio;
using DynMvp.Devices;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public delegate void ObjectSelected(ImageD image);
    public delegate void ObjectMultiSelected();
    public delegate void ObjectMoved();
    public delegate void ObjectAdded();

    public partial class TeachBox : UserControl
    {
        TargetGroup targetGroup;
        public TargetGroup TargetGroup
        {
            get { return targetGroup; }
            set { targetGroup = value; }
        }

        DrawBox drawBox;
        public DrawBox DrawBox
        {
            get { return drawBox; }
            set { drawBox = value; }
        }

        TeachHandler teachHandler;
        public TeachHandler TeachHandler
        {
            get { return teachHandler; }
            set { teachHandler = value; }
        }

        public bool Enable
        {
            get { return drawBox.Enable; }
            set { drawBox.Enable = value; }
        }

        public bool RotationLocked
        {
            get { return drawBox.RotationLocked; }
            set { drawBox.RotationLocked = value; }
        }

        private bool inplacePreview;
        public bool InplacePreview
        {
            get { return inplacePreview; }
            set { inplacePreview = value; }
        }

        public Image Image
        {
            get { return drawBox.Image; }
        }

        public bool ShowCenterGuide
        {
            get { return drawBox.ShowCenterGuide; }
            set { drawBox.ShowCenterGuide = value; }
        }

        InspectionResult inspectionResultSelected;
        public InspectionResult InspectionResultSelected
        {
            get { return inspectionResultSelected; }
            set { inspectionResultSelected = value; }
        }

        public ObjectSelected ObjectSelected;
        public ObjectMultiSelected ObjectMultiSelected;
        public ObjectMoved ObjectMoved;
        public ObjectAdded ObjectAdded;
        public MouseClickedDelegate MouseClicked;
        public MouseDoubleClickedDelegate MouseDoubleClicked;
        public PositionShiftedDelegate PositionShifted;

        public TeachBox()
        {
            InitializeComponent();

            this.drawBox = new DrawBox();

            this.SuspendLayout();

            this.drawBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.drawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawBox.Name = "drawBox";
            this.drawBox.TabIndex = 0;
            this.drawBox.TabStop = false;
            this.drawBox.FigureAdd += drawBox_AddFigure;
            this.drawBox.FigureMoved += drawBox_FigureMoved;
            this.drawBox.FigureSelected += drawBox_FigureSelected;
            this.drawBox.FigureMultiSelected += drawBox_FigureMultiSelected;
            this.drawBox.FigureSelectable += drawBox_FigureSelectable;
            this.drawBox.FigureCopy += drawBox_FigureCopy;
            this.drawBox.MouseDoubleClicked += drawBox_MouseDoubleClicked;
            this.drawBox.MouseClicked += drawBox_MouseClicked;
            this.drawBox.PositionShifted += drawBox_PositionShifted;

            this.Controls.Add(this.drawBox);

            this.ResumeLayout(false);
        }
        
        private void drawBox_AddFigure(List<PointF> pointList, FigureType figureType)
        {
            throw new NotImplementedException();
        }

        private void drawBox_MouseClicked(DrawBox senderView, Point clickPos, ref bool processingCancelled)
        {
            MouseClicked?.Invoke(senderView, clickPos, ref processingCancelled);
        }

        private void drawBox_PositionShifted(SizeF offset)
        {
            if (PositionShifted != null)
                PositionShifted(offset);
        }

        public void AutoFit(bool onOff)
        {
            this.drawBox.AutoFit(onOff);
        }

        public void SetAddMode()
        {
            this.Cursor = Cursors.Cross;
            this.drawBox.AddFigureMode = true;
        }

        delegate void UpdateImageDelegate(ImageD image, bool fReSelect);
        public void UpdateImage(ImageD image, bool fReSelect = true)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateImageDelegate(UpdateImage), image, fReSelect);
                return;
            }

            LogHelper.Debug(LoggerType.Operation, "TeachBox - Begin UpdateImage");

            if (drawBox.Image != null)
                drawBox.Image.Dispose();

            drawBox.UpdateImage(image?.ToBitmap());
            //drawBox.UpdateImage();

            LogHelper.Debug(LoggerType.Operation, "TeachBox - End UpdateImage");

            drawBox.Invalidate(true);
        }

        public delegate void UpdateFigureDelegate();
        public void UpdateFigure()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateFigureDelegate(UpdateFigure));
                return;
            }

            if (teachHandler.IsEditable() == false)
                return;

            Debug.WriteLine("TeachBox - UpdateFigure");

            FigureGroup activeFigures = new FigureGroup();
            FigureGroup backgroundFigures = new FigureGroup();
            FigureGroup tempFigureGroup = new FigureGroup();

            // 모든 Target의 Figure 및 각 Target에 속한 Probe의 Figure를 얻어온다.
            if (targetGroup != null)
                targetGroup.AppendFigures(activeFigures, null, true);

            teachHandler.GetFigures(activeFigures, backgroundFigures, tempFigureGroup, inspectionResultSelected);

            drawBox.FigureGroup = activeFigures;
            drawBox.BackgroundFigures = backgroundFigures;
            drawBox.TempFigureGroup = tempFigureGroup;

            drawBox.Invalidate(true);

            Debug.WriteLine("TeachBox - Figure Updated");
        }

        private void drawBox_FigureCopy(List<Figure> figureList)
        {
            if (teachHandler.IsEditable() == false)
                return;

            teachHandler.Copy(figureList);

            UpdateFigure();

            teachHandler.ShowTracker(drawBox);

            if (ObjectAdded != null)
            {
                ObjectAdded();
            }
        }

        public void ClearSelection()
        {
            teachHandler.ClearSelection();
            drawBox.ResetSelection();
        }

        private bool drawBox_FigureSelectable(Figure figure)
        {
            return teachHandler.IsSelectable(figure);
        }

        private void drawBox_FigureSelected(Figure figure, bool select)
        {
            if (teachHandler.IsEditable() == false)
                return;

            if (figure == null)
            {
                teachHandler.ClearSelection();

                if (ObjectSelected != null)
                    ObjectSelected(null);
                return;
            }

            if (select)
            {
                teachHandler.Select(figure);
            }
            else
            {
                teachHandler.Unselect(figure);
            }
            UpdateFigure();

            if (ObjectSelected != null)
            {
                Image2D image2d = null;
                if (teachHandler.IsSingleSelected())
                {
                    Bitmap objectImage = null;
                    objectImage = ImageHelper.ClipImage((Bitmap)drawBox.Image, teachHandler.GetBoundRect());

                    image2d = Image2D.FromBitmap(objectImage);
                }

                ObjectSelected(image2d);
            }
        }

        private void drawBox_FigureMultiSelected(List<Figure> figureList, bool select)
        {
            if (teachHandler.IsEditable() == false)
                return;

            if (figureList.Count() == 0)
            {
                teachHandler.ClearSelection();

                if (ObjectSelected != null)
                    ObjectSelected(null);
                return;
            }

            //if (select)
            //{
            //    teachHandler.Select(figure);
            //}
            //else
            //{
            //    teachHandler.Unselect(figure);
            //}

            if (figureList.Count() == 1)
            {
                if (select)
                {
                    teachHandler.Select(figureList[0]);
                }
                else
                {
                    teachHandler.Unselect(figureList[0]);
                }
            }
            else
            {
                Figure firstFigure = null;

                List<Figure> filteredList = new List<Figure>();
                foreach (Figure figure in figureList)
                {
                    ITeachObject teachObject = figure.Tag as ITeachObject;
                    if (teachObject != null)
                    {
                        if (teachObject is Target)
                            continue;

                        if (firstFigure == null)
                            firstFigure = figure;
                        else
                        {
                            if (IsSameObject((ITeachObject)firstFigure.Tag, (ITeachObject)figure.Tag) == false)
                                continue;
                        }
                        teachHandler.Select(figure);
                        filteredList.Add(figure);
                    }
                }

                figureList.Clear();
                figureList.AddRange(filteredList);
            }

            UpdateFigure();

            if (ObjectMultiSelected != null)
            {
                ObjectMultiSelected();
            }
        }

        bool IsSameObject(ITeachObject obj1, ITeachObject obj2)
        {
            if ((obj1 is VisionProbe) && (obj2 is VisionProbe))
            {
                VisionProbe visionProbe1 = (VisionProbe)obj1;
                VisionProbe visionProbe2 = (VisionProbe)obj2;
                if (visionProbe1.InspAlgorithm.GetAlgorithmType() == visionProbe2.InspAlgorithm.GetAlgorithmType())
                    return true;
            }

            if (obj1.GetType().Name == obj2.GetType().Name)
                return true;

            return true;
        }

        private void drawBox_FigureMoved(List<Figure> figureList)
        {
            if (teachHandler.IsEditable() == false)
                return;

            if (teachHandler.Movable == false)
                return;

            teachHandler.Move(figureList);

            UpdateFigure();

            if (ObjectMoved != null)
            {
                ObjectMoved();
                // Sample code for ObjectMoved
                //int rowIndex;
                //int colIndex;

                //if (GetTargetIndex(target, out colIndex, out rowIndex) == true)
                //{
                //    targetSelector.Rows[rowIndex].Cells[colIndex].Value = target.Image;
                //}

                //if (figureList.Count == 1)
                //    targetParamControl.UpdateTargetImage(targetImage);
            }

            Invalidate(true);
        }

        private void drawBox_AddRegionCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            if (teachHandler.IsEditable() == false)
                return;

            drawBox.AddFigureMode = false;
            this.Cursor = Cursors.Default;

            teachHandler.AddObject(rectangle, startPos, endPos, (Bitmap)drawBox.Image);

            UpdateFigure();

            teachHandler.ShowTracker(drawBox);

            ObjectAdded?.Invoke();
        }

        private void drawBox_MouseDoubleClicked(DrawBox senderBox)
        {
            inspectionResultSelected?.Clear(false);
            drawBox.TempFigureGroup.Clear();
            drawBox.Invalidate();

            MouseDoubleClicked?.Invoke(senderBox);
        }

        public void Delete()
        {
            teachHandler.DeleteObject();

            UpdateFigure();
        }

        public InspectionResult Inspect(DeviceImageSet deviceImageList, bool saveDebugImage, Calibration calibration, DigitalIoHandler digitalIoHandler, InspectionResult totalInspectionResult)
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            if (teachHandler.SelectedObjs.Count == 0)
            {
                List<Target> targetList = TargetGroup.GetTargetList();
                foreach (Target target in targetList)
                {
                    teachHandler.SelectedObjs.Add(target);
                }
            }

            inspectionResultSelected = teachHandler.Inspect(deviceImageList, saveDebugImage, calibration, digitalIoHandler, totalInspectionResult);

            foreach (ProbeResult probeResult in inspectionResultSelected)
            {
                probeResult.AppendResultFigures(drawBox.FigureGroup, false);
                
            }

            foreach (ProbeResult probeResult in inspectionResultSelected)
            {
                /*TextFigure textFigure = new TextFigure("Run...", new Point(camResultView.Image.Width * 10 - fontPosition, fontPosition), font, SamsungUtil.Instance().Run);
                textFigure.Alignment = StringAlignment.Far;
                drawBox.FigureGroup.AddFigure()
                probeResult.AppendResultFigures(drawBox.FigureGroup, false);*/
            }

            sw.Stop();
            //drawBox.TempFigureGroup = new FigureGroup();
            //drawBox.Invalidate();

            return inspectionResultSelected;
        }

        public void DrawCrossLine(PointF centerPt)
        {
            drawBox.DrawCrossLine(centerPt);
        }

        public void DrawEllipse(Rectangle rect)
        {
            drawBox.DrawEllipse(rect);
        }

        public void ZoomIn()
        {
            drawBox.ZoomIn();
        }

        public void ZoomOut()
        {
            drawBox.ZoomOut();
        }

        public void ZoomFit()
        {
            drawBox.ZoomFit();
        }
    }
}
