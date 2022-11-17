using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public interface ITeachObject
    {
        RotatedRect BaseRegion { get; }
        void AppendAdditionalFigures(FigureGroup figureGroup);
        void UpdateRegion(RotatedRect rotatedRect, PositionAligner positionAligner);
        object Clone();
        void UpdateTargetImage(Image2D targetImage, int lightTypeIndex);
    }

    public abstract class TeachHandler
    {
        protected List<ITeachObject> selectedObjs = new List<ITeachObject>();
        public List<ITeachObject> SelectedObjs
        {
            get { return selectedObjs; }
            set { selectedObjs = value; }
        }

        protected Rectangle boundary;
        public Rectangle Boundary
        {
            get { return boundary; }
            set { boundary = value; }
        }

        float pixelRes3d;
        public float PixelRes3d
        {
            get { return pixelRes3d; }
            set { pixelRes3d = value; }
        }

        //protected InspectionResult inspectionResult;
        //public InspectionResult InspectionResult
        //{
        //    get { return inspectionResult; }
        //    set { inspectionResult = value; }
        //}

        public void Clear()
        {
            selectedObjs.Clear();
        }

        protected bool movable = true;
        public bool Movable
        {
            get { return movable; }
            set { movable = value; }
        }

        public abstract bool IsEditable();
        public abstract bool IsSingleSelected();
        public abstract bool IsSelected();
        public abstract RectangleF GetBoundRect();
        public abstract void GetFigures(FigureGroup activeFigures, FigureGroup backgroundFigures, FigureGroup tempFigureGroup, InspectionResult inspectionResult);
        public abstract void ClearSelection();
        public abstract void ShowTracker(DrawBox drawBox);
        public abstract void Select(Figure figure);
        public abstract void Unselect(Figure figure);
        public abstract bool IsSelectable(Figure figure);
        public abstract void Copy(List<Figure> figureList);
        public abstract void Move(List<Figure> figureList);
        public abstract void DeleteObject();
        public abstract void AddObject(Rectangle rectangle, Point startPos, Point endPos, Bitmap wholeImage);
        public abstract InspectionResult Inspect(DeviceImageSet deviceImageList, bool saveDebugImage, Calibration calibration, DigitalIoHandler digitalIoHandler, InspectionResult inspectionResult);
    }
}
