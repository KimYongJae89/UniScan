using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public class SelectionContainer
    {
        private List<Figure> figures = new List<Figure>();
        public List<Figure> Figures
        {
            get { return figures; }
        }

        public SelectionContainer()
        {
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return figures.GetEnumerator();
        }

        public List<Figure> GetRealFigures()
        {
            return this.figures.Select(f => (Figure)f.Tag).ToList();
            //List<Figure> figureList = new List<Figure>();
            //foreach (Figure figure in figures)
            //    figureList.Add((Figure)figure.Tag);

            //return figureList;
        }

        public void ClearSelection()
        {
            LogHelper.Debug(LoggerType.Function, "SelectionContainer.ClearSelection.");
            figures.Clear();
        }

        public void RemoveSelection(Figure figure)
        {
            LogHelper.Debug(LoggerType.Function, "SelectionContainer.RemoveSelection.");
            figures.RemoveAll(f=>(Figure)f.Tag == figure);
        }

        public void AddSelection(Figure figure)
        {
            // Tracking 상태를 표시하기 위해, 현재 Figure를 Clone한다.
            Figure cloneFigure = null;
            if (figure != null)
            {
                LogHelper.Debug(LoggerType.Function, "SelectionContainer.AddFigure.");
                Type tagType = figures.FirstOrDefault()?.Tag?.GetType();
                if (tagType != null)
                {
                    // Tag에 지정되어 있는 개체의 종류가 동일한 것만 추가 가능하도록 제약
                    if (tagType == figure.Tag.GetType())
                    {
                        cloneFigure = (Figure)figure.Clone();
                    }
                }
                else
                {
                    cloneFigure = (Figure)figure.Clone();
                }
            }

            if (cloneFigure != null)
            {
                cloneFigure.Tag = figure;
                cloneFigure.FigureProperty.Pen = new Pen(Color.Cyan);
                cloneFigure.FigureProperty.Pen.DashStyle = DashStyle.Dot;
                figures.Add(cloneFigure);
            }
        }

        public void AddSelection(List<Figure> figureList)
        {
            if (figureList != null && figureList.Count > 0)
            {
                LogHelper.Debug(LoggerType.Function, "SelectionContainer.AddFigure.List");
                foreach (Figure figure in figureList)
                    AddSelection(figure);
            }
        }

        public bool IsSelected(Figure searchfigure)
        {
            foreach (Figure figure in figures)
            {
                if (figure.Tag == searchfigure)
                    return true;
            }

            return false;
        }

        public void Offset(SizeF offset)
        {
            foreach (Figure figure in figures)
            {
                Figure realFigure = (Figure)figure.Tag;
                if (realFigure != null)
                    realFigure.Offset(offset.Width, offset.Height);
                else
                    figure.Offset(offset.Width, offset.Height);
            }
        }

        public void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked, bool confirm)
        {
            foreach (Figure figure in figures)
            {
                if (confirm)
                {
                    Figure realFigure = (Figure)figure.Tag;
                    realFigure.TrackMove(trackPos, offset, rotationLocked);
                }
                else
                {
                    figure.TrackMove(trackPos, offset, rotationLocked);
                }
            }
        }

        public void GetTrackPath(List<GraphicsPath> trackPathList, SizeF offset, TrackPos trackPos)
        {
            foreach (Figure figure in figures)
            {
                figure.GetTrackPath(trackPathList, offset, trackPos);
            }
        }

        public void Draw(Graphics g, CoordMapper coordMapper, bool rotationLocked)
        {
            foreach (Figure figure in figures)
            {
                figure.DrawSelection(g, coordMapper, rotationLocked);
            }
        }

        public TrackPos GetTrackPos(CoordMapper coordMapper, PointF point, bool rotationLocked)
        {
            TrackPos trackPos = new TrackPos(TrackPosType.None, 0);

            int polygonIndex = 0;

            if (figures.Count == 1)
            {
                trackPos = ((Figure)figures[0].Tag).GetTrackPos(point, coordMapper, rotationLocked, ref polygonIndex);
            }
            else
            {
                foreach (Figure figure in figures)
                {
                    trackPos = ((Figure)figure.Tag).GetTrackPos(point, coordMapper, rotationLocked, ref polygonIndex);
                    if (trackPos.PosType != TrackPosType.None)
                    {
                        //trackPos.PosType = TrackPosType.Inner;
                        break;
                    }
                }
            }

            return trackPos;
        }

        internal void OnSelectionUpdated(Figure figure)
        {
            Figure modified = this.figures.Find(f => (Figure)f.Tag == figure);
            if (modified == null)
                return;

            modified.SetRectangle(figure.GetRectangle());
        }

        internal void OnSelectionUpdated(List<Figure> figureList)
        {
            Dictionary<Figure, Figure> dic = figureList.ToDictionary(f => f, f => this.figures.Find(g => (Figure)g.Tag == f));
            List<KeyValuePair<Figure, Figure>> list = dic.ToList();
            list.ForEach(f =>
            {
                f.Value.SetRectangle(f.Key.GetRectangle());
            });
        }
    }
}
