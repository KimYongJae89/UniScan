using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniScanWPF.Helper;

namespace WpfControlLibrary.Teach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TeachWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapSource BitmapSource { get => this.bitmapSource; set => this.bitmapSource = value; }
        BitmapSource bitmapSource;

        public DrawingHandler DrawingHandler => this.drawingHandler;
        DrawingHandler drawingHandler;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public TeachWindow()
        {
            this.drawingHandler = new DrawingHandler();
            this.drawingHandler.OnDragEnd += DrawingHandler_OnDragEnd;
            
            InitializeComponent();

            this.DataContext = this;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.bitmapSource == null)
            {
                this.bitmapSource = WPFImageHelper.LoadBitmapSource(@"C:\temp\marginMeasurePos.bmp");
                OnPropertyChanged("BitmapSource");
            }
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Thumb_DragStarted - X: {0}, Y: {1}", e.HorizontalOffset, e.VerticalOffset));

            this.drawingHandler.DragStart(e.HorizontalOffset, e.VerticalOffset);
            OnPropertyChanged("RectangleList");
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Thumb_DragDelta - dX: {0}, dY: {1}", e.HorizontalChange, e.VerticalChange));

            this.drawingHandler.DragOffset(e.HorizontalChange, e.VerticalChange);
            OnPropertyChanged("RectangleList");
        }
 
        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Thumb_DragCompleted - dX: {0}, dY: {1}", e.HorizontalChange, e.VerticalChange));

            this.drawingHandler.DragEnd(e.HorizontalChange, e.VerticalChange);
            OnPropertyChanged("RectangleList");
        }

        private void DrawingHandler_OnDragEnd(Rect rect)
        {
            if (selRadBtn.IsChecked.Value)
            {
                this.drawingHandler.Select(rect);
            }
            else if (rctRadBtn.IsChecked.Value)
            {
                if (rect.Width * rect.Height > 0)
                    this.drawingHandler.Add(new DrawObj(null));
            }
        }

        private void Tracker_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Tracker_DragStarted - X: {0}, Y: {1}", e.HorizontalOffset, e.VerticalOffset));

            System.Windows.Controls.Primitives.Thumb thumb = (System.Windows.Controls.Primitives.Thumb)sender;
            Tracker tracker = (Tracker)thumb.DataContext;
            this.drawingHandler.TrackerDragStarted(tracker, e.HorizontalOffset, e.VerticalOffset);
        }

        private void Tracker_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Tracker_DragDelta - dX: {0}, dY: {1}", e.HorizontalChange, e.VerticalChange));

            System.Windows.Controls.Primitives.Thumb thumb = (System.Windows.Controls.Primitives.Thumb)sender;
            Tracker tracker = (Tracker)thumb.DataContext;
            this.drawingHandler.TrackerDragDelta(tracker, e.HorizontalChange, e.VerticalChange);
        }

        private void Tracker_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Tracker_DragCompleted - dX: {0}, dY: {1}", e.HorizontalChange, e.VerticalChange));

            System.Windows.Controls.Primitives.Thumb thumb = (System.Windows.Controls.Primitives.Thumb)sender;
            Tracker tracker = (Tracker)thumb.DataContext;
            this.drawingHandler.TrackerDragCompleted(tracker, e.HorizontalChange, e.VerticalChange);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                this.drawingHandler.Remove(this.drawingHandler.Selected);
        }
    }
}
