using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControlLibrary.Teach
{
    public delegate Geometry GetGeometryDelegate(ITeachProbe teachProbe);

    public class DrawObj : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public GetGeometryDelegate GetGeometry;

        public ITeachProbe Parent { get => this.parent; set => this.parent = value; }
        ITeachProbe parent;

        public bool IsSelected
        {
            get => this.isSelected;
            set
            {
                this.isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Trackers"));
            }
        }
        bool isSelected;

        public Rect Rect
        {
            get => this.parent.Rect;
            set
            {
                this.parent.Rect = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rect"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Geometry"));                
            }
        }
        //Rect rect;

        public Geometry Geometry
        {
            get
            {
                if (this.GetGeometry == null)
                {
                    GeometryGroup geometryGroup = new GeometryGroup();
                    geometryGroup.Children.Add(new EllipseGeometry(this.parent.Rect));

                    return geometryGroup;
                }

                return this.GetGeometry(this.parent);
            }
        }

        public Tracker[] Trackers => this.trackers;
        Tracker[] trackers;

        public DrawObj(ITeachProbe parent)
        {
            this.parent = parent;

            this.trackers = new Tracker[]
            {
                new Tracker(this, TrackerPos.CT),
                new Tracker(this, TrackerPos.LT),
                new Tracker(this, TrackerPos.RT),
                new Tracker(this, TrackerPos.RB),
                new Tracker(this, TrackerPos.LB)
            };

            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Trackers"));
        }

        public DrawObj Clone()
        {
            DrawObj drawObj = new DrawObj(this.parent);
            drawObj.GetGeometry = this.GetGeometry;
            drawObj.isSelected = this.isSelected;
            return drawObj;
        }

        internal void UpdateTrackersPosition()
        {
            Array.ForEach(this.trackers, f => f.UpdatePosition());
        }

        internal Geometry GetTrackerGeometry(TrackerPos trackerPos)
        {
            Rect r = parent.Rect;
            switch (trackerPos)
            {
                case TrackerPos.CT:
                    Point ct = new Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
                    r = new Rect(ct, new Size(0, 0));
                    break;
                case TrackerPos.LT:
                    r = new Rect(r.TopLeft, new Size(0, 0));
                    break;
                case TrackerPos.RT:
                    r = new Rect(r.TopRight, new Size(0, 0));
                    break;
                case TrackerPos.RB:
                    r = new Rect(r.BottomRight, new Size(0, 0));
                    break;
                case TrackerPos.LB:
                    r = new Rect(r.BottomLeft, new Size(0, 0));
                    break;
                default:
                    throw new ArgumentException();
                    break;
            }
            //r.Offset(-this.rect.Left, -this.rect.Top);
            r.Inflate(3, 3);

            return new RectangleGeometry(r);
        }

        internal void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Geometry"));
            UpdateTrackersPosition();
        }
    }   

    public enum TrackerPos { CT, LT, RT, RB, LB }
    public class Tracker : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Geometry Geometry => this.parent.GetTrackerGeometry(this.trackerPos);

        public double CenterX => (this.Rect.Left + this.Rect.Right) / 2;
        public double CenterY => (this.Rect.Top + this.Rect.Bottom) / 2;

        public Rect Rect => Geometry.Bounds;
        public DrawObj Parent => this.parent;
        DrawObj parent;

        public TrackerPos TrackerPos { get => this.trackerPos; set => this.trackerPos = value; }
        TrackerPos trackerPos;

        public Cursor Cursor => this.cursor;
        Cursor cursor;

        public Tracker(DrawObj parent, TrackerPos trackerPos)
        {
            this.parent = parent;
            this.trackerPos = trackerPos;
            switch (trackerPos)
            {
                case TrackerPos.CT:
                    cursor = Cursors.SizeAll;
                    break;
                case TrackerPos.LT:
                case TrackerPos.RB:
                    cursor = Cursors.SizeNWSE;
                    break;
                case TrackerPos.RT:
                case TrackerPos.LB:
                    cursor = Cursors.SizeNESW;
                    break;
            }
        }

        public void UpdatePosition()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Rect"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Geometry"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CenterX"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CenterY"));
        }

        internal void Move(double dx, double dy)
        {
            Rect rect = this.parent.Rect;

            double l = rect.Left;
            double t = rect.Top;
            double r = rect.Right;
            double b = rect.Bottom;

            switch (trackerPos)
            {
                case TrackerPos.CT:
                    l += dx;
                    t += dy;
                    r += dx;
                    b += dy;
                    break;
                case TrackerPos.LT:
                    l += dx;
                    t += dy;
                    break;
                case TrackerPos.RB:
                    r += dx;
                    b += dy;
                    break;
                case TrackerPos.RT:
                    r += dx;
                    t += dy;
                    break;
                case TrackerPos.LB:
                    l += dx;
                    b += dy;
                    break;
            }
            this.parent.Rect = new Rect(l, t, Math.Max(1, r - l), Math.Max(1, b - t));
            System.Diagnostics.Debug.WriteLine(this.parent.Rect);
            this.parent.UpdateTrackersPosition();
        }
    }
}