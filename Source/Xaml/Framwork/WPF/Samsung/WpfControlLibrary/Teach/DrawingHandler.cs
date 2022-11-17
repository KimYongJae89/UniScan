using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace WpfControlLibrary.Teach
{
    public delegate void OnDragEndDelegate(Rect rect);

    public class DrawingHandler: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event OnDragEndDelegate OnDragEnd;

        public Rect DragRect => GetDragRect();
        Tracker dragTracker;
        Point dragBase;
        Point dragStart;
        Point dragOffset;

        public ObservableCollection<DrawObj> List => this.list;
        ObservableCollection<DrawObj> list = new ObservableCollection<DrawObj>();
        public DrawObj[] Selected => this.list.ToList().FindAll(f=>f.IsSelected).ToArray();

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private Rect GetDragRect()
        {
            double w, h,x,y;
            if (this.dragTracker != null && this.dragTracker.TrackerPos == TrackerPos.CT)
            {
                w = this.dragStart.X - this.dragBase.X;
                h = this.dragStart.Y - this.dragBase.Y;
                x = Math.Max(0, this.dragBase.X + this.dragOffset.X);
                y = Math.Max(0, this.dragBase.Y + this.dragOffset.Y);
            }
            else
            {
                w = this.dragStart.X - this.dragBase.X + this.dragOffset.X;
                h = this.dragStart.Y - this.dragBase.Y + this.dragOffset.Y;
                x = this.dragBase.X + Math.Min(w, 0);
                y = this.dragBase.Y + Math.Min(h, 0);
            }
            
            return new Rect(x, y, Math.Abs(w), Math.Abs(h));
        }

        public void DragStart(double x, double y)
        {
            DragStart(new Point(x, y));
        }

        public void DragStart(Point dragStart)
        {
            this.dragBase = dragStart;
            this.dragStart = dragStart;
            this.dragOffset = new Point(0, 0);
            OnPropertyChanged("DragRect");
        }

        public void DragOffset(double w, double h)
        {
            DragOffset(new Point(w, h));
        }

        public void DragOffset(Point dragOffset)
        {
            this.dragOffset = dragOffset;
            OnPropertyChanged("DragRect");            
        }

        public void DragEnd(double w, double h)
        {
            DragEnd(new Point(w, h));
        }

        public void DragEnd(Point dragOffset)
        {
            OnDragEnd?.Invoke(GetDragRect());

            this.dragBase = new Point();
            this.dragStart = new Point();
            this.dragOffset = new Point();
            OnPropertyChanged("DragRect");
        }

        public void Select(Rect rect)
        {
            bool[] selected;
            if (rect.Width == 0 && rect.Height == 0)
            {
                selected = this.list.Select(f => f.Rect.IntersectsWith(rect)).ToArray();
            }
            else
            {
                selected = this.list.Select(f => Rect.Intersect(f.Rect, rect) == f.Rect).ToArray();
            }

            for (int i = 0; i < this.list.Count; i++)
                this.list[i].IsSelected = selected[i];
        }

        public void Add(DrawObj obj)
        {
            this.list.Add(obj);
            OnPropertyChanged("List");
        }

        public void Add(DrawObj[] objs)
        {
            Array.ForEach(objs, f => this.list.Add(f));
            OnPropertyChanged("List");
        }

        public void Refresh()
        {
            OnPropertyChanged("List");
            for (int i = 0; i < this.list.Count; i++)
                this.list[i].Refresh();
        }

        public void Remove(DrawObj obj)
        {
            this.list.Remove(obj);
            OnPropertyChanged("List");
        }

        public void Remove(DrawObj[] objs)
        {
            Array.ForEach(objs, f => this.list.Remove(f));
            OnPropertyChanged("List");
        }

        public void RemoveAll()
        {
            while(this.list.Count>0)
                this.list.RemoveAt(0);
            OnPropertyChanged("List");
        }

        public void TrackerDragStarted(Tracker tracker, double x, double y)
        {
            this.dragTracker = tracker;
            Rect rect = tracker.Parent.Rect;
            if (tracker.TrackerPos == TrackerPos.CT)
            {
                this.dragBase = rect.TopLeft;
                this.dragStart = rect.BottomRight;
            }
            else
            {
                switch (tracker.TrackerPos)
                {
                    case TrackerPos.CT:
                        break;
                    case TrackerPos.LT:
                        this.dragBase = rect.BottomRight;
                        break;
                    case TrackerPos.RT:
                        this.dragBase = rect.BottomLeft;
                        break;
                    case TrackerPos.RB:
                        this.dragBase = rect.TopLeft;
                        break;
                    case TrackerPos.LB:
                        this.dragBase = rect.TopRight;
                        break;
                }
                this.dragStart = tracker.Rect.Location;
            }

            this.dragOffset = new Point(0, 0);
            OnPropertyChanged("DragRect");
        }

        public void TrackerDragDelta(Tracker tracker, double dx, double dy)
        {
            //List<DrawObj> list = this.list.ToList().FindAll(f => f.IsSelected);
            //Tracker[] trackers = list.Select(f => Array.Find(f.Trackers, g => g.TrackerPos == tracker.TrackerPos)).ToArray();
            //Array.ForEach(trackers, f => f.Move(dx, dy));
            
            DragOffset(dx, dy);
        }

        public void TrackerDragCompleted(Tracker tracker, double dx, double dy)
        {
            List<DrawObj> list = this.list.ToList().FindAll(f => f.IsSelected);
            Tracker[] trackers = list.Select(f => Array.Find(f.Trackers, g => g.TrackerPos == tracker.TrackerPos)).ToArray();
            Array.ForEach(trackers, f => f.Move(dx, dy));

            this.dragTracker = null;
            this.dragBase = new Point();
            this.dragStart = new Point();
            this.dragOffset = new Point();
            OnPropertyChanged("DragRect");
        }
    }
}
