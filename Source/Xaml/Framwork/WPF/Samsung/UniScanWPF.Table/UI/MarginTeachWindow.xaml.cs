using DynMvp.Vision;
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
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.Helper;
using WpfControlLibrary.Teach;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public delegate void TestInspectionDelegate(MarginMeasurePos model);
    public partial class MarginTeachWindow : Window, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event TestInspectionDelegate OnTestInspection;

        public double Scale => Math.Min((this.CanvasGrid.ActualWidth-10) / ShowBitmapSource.Width, (this.CanvasGrid.ActualHeight - 10) / ShowBitmapSource.Height);

        public InspectOperatorSettings InspectOperatorSettings => SystemManager.Instance().OperatorManager.InspectOperator.Settings;

        public ITeachModel Model { get ; set; }

        public BitmapSource ShowBitmapSource { get => (showBinalImage ? GetBinalBitmapSource() : this.Model?.BgBitmapSource); }

        public MarginMeasurePos MarginMeasurePos { get; set; }

        public MarginMeasureParam MarginMeasureParam
        {
            get => marginMeasureParam;
            set
            {
                if (this.marginMeasureParam != value)
                {
                    this.marginMeasureParam = value;
                    OnPropertyChanged("MarginMeasureParam");
                }
            }
        }
        MarginMeasureParam marginMeasureParam;

        public bool UseLocalSetting
        {
            get => this.MarginMeasurePos.OverrideParam;
            set
            {
                if (this.MarginMeasurePos.OverrideParam != value)
                {
                    this.MarginMeasurePos.OverrideParam = value;
                    MarginMeasureParam = value ? MarginMeasurePos.MeasureParam : InspectOperatorSettings.MarginMeasureParam;
                }
            }
        }

        public bool ShowBinalImage
        {
            get => this.showBinalImage;
            set
            {
                Set("ShowBinalImage", ref this.showBinalImage, value);
                OnPropertyChanged("ShowBitmapSource");
            }
        }
        bool showBinalImage = false;

        public bool ShowFigures
        {
            get => this.showFigures;
            set
            {
                Set("ShowFigures", ref this.showFigures, value);
                OnPropertyChanged("ShowFigures");
            }
        }
        bool showFigures = true;

        public DrawingHandler DrawingHandler => this.drawingHandler;
        DrawingHandler drawingHandler;

        public SelectionInfo SelectionInfo => new SelectionInfo()
        {
            Count = this.drawingHandler.Selected.Length,
            IsL = Array.Exists(this.drawingHandler.Selected, f => ((MarginMeasureSubPos)f.Parent).UseL),
            IsW = Array.Exists(this.drawingHandler.Selected, f => ((MarginMeasureSubPos)f.Parent).UseW)
        };

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public MarginTeachWindow()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);

            this.drawingHandler = new DrawingHandler();
            this.drawingHandler.OnDragEnd += DrawingHandler_OnDragEnd;

            this.DataContext = this;
        }

        private void Set<T>(string propName, ref T target, T value)
        {
            if (target.Equals(value))
                return;

            target = value;
            OnPropertyChanged(propName);
        }

        private BitmapSource GetBinalBitmapSource()
        {
            BitmapSource bitmapSource;
            BitmapSource srcBitmapSource = this.Model.BgBitmapSource;
            if (srcBitmapSource == null)
                return null;

            System.Drawing.Size imageSize = new System.Drawing.Size(srcBitmapSource.PixelWidth, srcBitmapSource.PixelHeight);
            using (AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, imageSize))
            {
                byte[] bytes = WPFImageHelper.BitmapSourceToBytes(this.Model?.BgBitmapSource);
                algoImage.SetByte(bytes);

                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                ip.Binarize(algoImage, algoImage, (int)this.MarginMeasureParam.Threshold, true);
                bitmapSource = algoImage.ToBitmapSource();
            }

            return bitmapSource;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Size size = new System.Windows.Size(this.Model.BgBitmapSource.PixelWidth, this.Model.BgBitmapSource.PixelHeight);
            double r = size.Width / size.Height;
            if(r>1)
            {
                size.Width = 500;
                size.Height = 500 / r + 80;
            }else 
            {
                size.Width = 850 * r;
                size.Height = 850 + 80;
            }
            this.Width = size.Width;
            this.Height = size.Height;

            this.drawingHandler.Add(this.Model.GetDrawObjs());

            if (this.MarginMeasurePos.MeasureParam.ThresholdA <= 0)
                this.MarginMeasurePos.MeasureParam.ThresholdA = this.MarginMeasurePos.GetAutoThresholdValue();

            if (this.MarginMeasurePos.MeasureParam.ThresholdM <= 0)
                this.MarginMeasurePos.MeasureParam.ThresholdM = this.MarginMeasurePos.MeasureParam.ThresholdA;

            this.MarginMeasureParam = MarginMeasurePos.OverrideParam ? this.MarginMeasurePos.MeasureParam : InspectOperatorSettings.MarginMeasureParam;
            OnPropertyChanged("ShowBitmapSource");
            OnPropertyChanged("Source");
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
                OnPropertyChanged("SelectionInfo");
            }
            else if (rctRadBtn.IsChecked.Value)
            {
                if (rect.Width * rect.Height > 0)
                {
                    ITeachProbe teachProbe = this.Model.CreateProbe(rect);
                    this.Model.AddProbe(teachProbe);
                    this.drawingHandler.Add(teachProbe.GetDrawObj());
                }
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
            double scale = this.Scale;
            this.drawingHandler.TrackerDragDelta(tracker, e.HorizontalChange / scale, e.VerticalChange / scale);
        }

        private void Tracker_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Tracker_DragCompleted - dX: {0}, dY: {1}", e.HorizontalChange, e.VerticalChange));

            System.Windows.Controls.Primitives.Thumb thumb = (System.Windows.Controls.Primitives.Thumb)sender;
            Tracker tracker = (Tracker)thumb.DataContext;
            double scale = this.Scale;
            this.drawingHandler.TrackerDragCompleted(tracker, e.HorizontalChange / scale, e.VerticalChange / scale);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DrawObj[] drawObjs = this.drawingHandler.Selected;
                Array.ForEach(drawObjs, f => 
                {
                    this.drawingHandler.Remove(f);
                    this.Model.RemoveProbe(f.Parent);
                });
            }
        }

        private void CheckBoxW_Clicked(object sender, RoutedEventArgs e)
        {
            bool settingValue = !((CheckBox)sender).IsChecked.Value;
            Array.ForEach(this.drawingHandler.Selected, f => ((MarginMeasureSubPos)f.Parent).UseW = settingValue);

            this.drawingHandler.Refresh();
            OnPropertyChanged("SelectionInfo");
        }

        private void CheckBoxL_Clicked(object sender, RoutedEventArgs e)
        {
            bool settingValue = !((CheckBox)sender).IsChecked.Value;
            Array.ForEach(this.drawingHandler.Selected, f => ((MarginMeasureSubPos)f.Parent).UseL = settingValue);

            this.drawingHandler.Refresh();
            OnPropertyChanged("SelectionInfo");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{e.NewSize} => {this.Model.BgBitmapSource.PixelWidth},{this.Model.BgBitmapSource.PixelHeight}");
            OnPropertyChanged("Scale");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnTestInspection?.Invoke((MarginMeasurePos)this.Model);
        }

        private void Button_Up_Click(object sender, RoutedEventArgs e)
        {
            this.MarginMeasureParam.ThresholdM += 1;
            OnPropertyChanged("MarginMeasureParam");
            OnPropertyChanged("ShowBitmapSource");
        }

        private void Button_Down_Click(object sender, RoutedEventArgs e)
        {
            this.MarginMeasureParam.ThresholdM -= 1;
            OnPropertyChanged("MarginMeasureParam");
            OnPropertyChanged("ShowBitmapSource");
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }

        private void CheckBoxAuto_Checked(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("MarginMeasureParam");
            OnPropertyChanged("ShowBitmapSource");
        }
    }

    public struct SelectionInfo
    {
        public int Count { get; set; }
        public bool IsW { get; set; }
        public bool IsL { get; set; }
    }
}
