using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using UniScanG.Module.Monitor.Inspect;

namespace UniScanG.Module.Monitor.UI.Control
{
    /// <summary>
    /// ImageControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageControl : UserControl, INotifyPropertyChanged
    {
        Rect emptyRect = new Rect(0, 0, 0, 0);

        public static readonly DependencyProperty InspectionResultProperty =
            DependencyProperty.Register("InspectionResult", typeof(InspectionResult), typeof(ImageControl), new FrameworkPropertyMetadata(InspectionResultPropertyChangedCallback));

        private static void InspectionResultPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageControl imageControl = d as ImageControl;
            InspectionResult inspectionResult = e.NewValue as InspectionResult;
            imageControl.OnInspectionResultPropertyChanged();
        }

        public InspectionResult InspectionResult
        {
            get { return (InspectionResult)GetValue(InspectionResultProperty); }
            set { SetValue(InspectionResultProperty, value); }
        }

        public BitmapSource BitmapSource => bitmapSource;
        BitmapSource bitmapSource;


        //public static readonly DependencyProperty BitmapSourceProperty =
        //    DependencyProperty.Register("BitmapSource", typeof(BitmapSource), typeof(ImageControl));

        //public BitmapSource BitmapSource
        //{
        //    get { return (BitmapSource)GetValue(BitmapSourceProperty); }
        //    set
        //    {
        //        Dispatcher.Invoke(() => SetValue(BitmapSourceProperty, value));
        //        OnBitmapSourcePropertyChanged();
        //    }
        //}


        public event PropertyChangedEventHandler PropertyChanged;

        public double Scale
        {
            get
            {
                if (BitmapSource != null && this.dockPanel.ActualWidth > 0 && this.dockPanel.ActualHeight > 0)
                    return Math.Min(this.dockPanel.ActualWidth / BitmapSource.Width, this.dockPanel.ActualHeight / BitmapSource.Height);
                return 1;
            }
        }

        public Point Offset => BitmapSource == null ? new Point(0,0) : new Point((dockPanel.ActualWidth - BitmapSource.Width * Scale) / 2, (dockPanel.ActualHeight - BitmapSource.Height * Scale) / 2);

        public System.Windows.Media.Brush MarginBrush =>
            InspectionResult == null ? System.Windows.Media.Brushes.Transparent :
            InspectionResult.Judgment == DynMvp.InspData.Judgment.Skip? System.Windows.Media.Brushes.Yellow:
            InspectionResult.MarginResult ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.Red;

        public System.Windows.Media.Brush BlotBrush =>
            InspectionResult == null ? System.Windows.Media.Brushes.Transparent :
            InspectionResult.Judgment == DynMvp.InspData.Judgment.Skip ? System.Windows.Media.Brushes.Yellow :
            InspectionResult.BlotResult ? System.Windows.Media.Brushes.LightGreen : System.Windows.Media.Brushes.Red;

        public Rect MarginRect => InspectionResult == null ? this.emptyRect : WpfControlLibrary.Helper.Converter.ToRect(InspectionResult.MarginRect);

        public Rect BlotRect => InspectionResult == null ? this.emptyRect : WpfControlLibrary.Helper.Converter.ToRect(InspectionResult.BlotRect);

        public Rect DefectRect => InspectionResult == null ? this.emptyRect : WpfControlLibrary.Helper.Converter.ToRect(InspectionResult.MaxDefectRect);

        Rect[] defectRects;
        public ImageControl()
        {
            InitializeComponent();
        }

        private void OnInspectionResultPropertyChanged()
        {
            this.bitmapSource = null;
            System.Drawing.Bitmap bitmap = InspectionResult.GrabImageList.FirstOrDefault()?.ToBitmap();
            if (bitmap == null)
                this.bitmapSource = null;
            else
                this.bitmapSource = UniScanWPF.Helper.WPFImageHelper.BitmapToBitmapSource(bitmap);

            OnPropertyChanged("BitmapSource");
            OnPropertyChanged("Scale");
            OnPropertyChanged("Offset");

            this.defectRects = Array.ConvertAll(InspectionResult.Defects, f => WpfControlLibrary.Helper.Converter.ToRect(f.Rectangle));
            this.defectRectCanvas.Children.Clear();
            Array.ForEach(this.defectRects, f =>
            {
                Rectangle rectangle = new Rectangle();
                rectangle.StrokeThickness = 5;
                rectangle.Stroke = Brushes.Red;
                rectangle.Width = f.Width+10;
                rectangle.Height = f.Height+10;
                Canvas.SetTop(rectangle, f.Y-5);
                Canvas.SetLeft(rectangle, f.X-5);
                
                this.defectRectCanvas.Children.Add(rectangle);
            });

            OnPropertyChanged("MarginBrush");
            OnPropertyChanged("MarginRect");
            OnPropertyChanged("BlotBrush");
            OnPropertyChanged("BlotRect");
            OnPropertyChanged("DefectRect");
        }

        private void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
