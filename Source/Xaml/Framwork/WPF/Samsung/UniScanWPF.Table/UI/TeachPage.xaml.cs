using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
//using System.Drawing;
using System.Globalization;
using System.IO;
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
using System.Windows.Threading;
using UniScanWPF.Helper;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    public partial class TeachPage : Page, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public MarginMeasurePos MarginMeasurePos => this.marginMeasurePos;
        MarginMeasurePos marginMeasurePos = new MarginMeasurePos();
        float mouseImageZoomScale = 0.16f;

        public System.Windows.Size MarkSize
        {
            get
            {
                double scale = Scale.ScaleY / Scale.ScaleX;
                double x = Math.Min(2500 * scale, 100.0 / Scale.ScaleX);
                double y = Math.Min(2500 / scale, 100.0 / Scale.ScaleY);
                return new System.Windows.Size(x, y);
            }
        }

        public double StrokeThickness { get => 5 / Math.Max(Scale.ScaleY, Scale.ScaleX); }
        public double MarkFontSize { get => Math.Min(35791, Math.Min(MarkSize.Width, MarkSize.Height) / 2); }
        public System.Drawing.PointF HomeMarkPos
        {
            get
            {
                System.Drawing.PointF homePos = InfoBox.Instance.DispHomePos;
                System.Windows.Size markSize = MarkSize;
                return new System.Drawing.PointF((float)(homePos.X - markSize.Width / 2), (float)(homePos.Y - markSize.Height / 2));
            }
        }

        public System.Windows.Point CurMarkPos
        {
            get => new System.Windows.Point(
                HomeMarkPos.X - (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionX / DeveloperSettings.Instance.Resolution),
                HomeMarkPos.Y + (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionY / DeveloperSettings.Instance.Resolution)
                );
        }
        public SolidColorBrush CurMarkBrush { get => SystemManager.Instance().MachineObserver.MotionBox.OnMoveBrush; }

        public TeachPage()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);

            //SystemManager.Instance().OperatorManager.OnTeachStart += new OnStartDelegate(() => this.TeachBorderVisible = Visibility.Visible);
            //SystemManager.Instance().OperatorManager.OnTeachEnd += new OnEndDelegate((e, c) => this.TeachBorderVisible = Visibility.Hidden);

            this.DataContext = InfoBox.Instance;

            this.DifferenceTextBlock.DataContext = SystemManager.Instance().OperatorManager.TeachOperator.Settings;
            this.MainCanvas.DataContext = InfoBox.Instance;
            this.MarginImage.DataContext = this;

            this.HomeLabel.DataContext = this;
            this.MachineCircleLabel.DataContext = this;

            this.LightTuneImage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            this.LightTuneMessage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.LightTuneOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.LightTuneOperator;
            this.ScanOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            this.ExtractOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.TeachOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.TeachOperator;

            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
                Border border = new Border();
                border.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
                border.Tag = i;
                border.BorderThickness = new Thickness(15);
                border.MouseEnter += new MouseEventHandler((s, e) =>
                {
                    Border b = (Border)s;
                    b.BorderBrush = new SolidColorBrush(Colors.Red);
                    lock (ImageCanvas.Children)
                    {
                        ImageCanvas.Children.Remove(b);
                        ImageCanvas.Children.Add(b);
                    }
                });
                border.MouseMove += new MouseEventHandler((s, e) =>
                {
                    Border b = (Border)s;
                    Point pt = e.GetPosition(b);
                    System.Drawing.Point ptImg = new System.Drawing.Point((int)(pt.X * InfoBox.Instance.ImageCanvasScale), (int)(pt.Y * InfoBox.Instance.ImageCanvasScale));
                    UpdateMouseImage((int)b.Tag, ptImg);
                });
                border.MouseWheel += new MouseWheelEventHandler((s, e) =>
                {
                    Border b = (Border)s;
                    Point pt = e.GetPosition(b);
                    System.Drawing.Point ptImg = new System.Drawing.Point((int)(pt.X * InfoBox.Instance.ImageCanvasScale), (int)(pt.Y * InfoBox.Instance.ImageCanvasScale));

                    float mouseImageZoomScaleMax = 0.3f;
                    float mouseImageZoomScaleMin = 0.07f;
                    float mouseImageZoomScaleStep = 0.01f;

                    this.mouseImageZoomScale += e.Delta < 0 ? mouseImageZoomScaleStep : -mouseImageZoomScaleStep;
                    if (this.mouseImageZoomScale > mouseImageZoomScaleMax)
                        this.mouseImageZoomScale = mouseImageZoomScaleMax;
                    if (this.mouseImageZoomScale < mouseImageZoomScaleMin)
                        this.mouseImageZoomScale = mouseImageZoomScaleMin;

                    UpdateMouseImage((int)b.Tag, ptImg);
                });
                border.MouseLeave += new MouseEventHandler((s, e) => ((Border)s).BorderBrush = new SolidColorBrush(Colors.Transparent));
                border.MouseDown += new MouseButtonEventHandler((s, e) =>
                {
                    MarginMeasurePos marginMeasurePos = this.marginMeasurePos.Clone(true);
                    marginMeasurePos.Name = $"M{(InfoBox.Instance.CurrentModel.MarginMeasurePosList.Count() + 1)}";
                    InfoBox.Instance.CurrentModel.MarginMeasurePosList.Add(marginMeasurePos);
                    InfoBox.Instance.ModelChanged();
                    //OnPropertyChanged("CurrentModel");
                });
                border.SetBinding(Canvas.LeftProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[0]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                border.SetBinding(Canvas.TopProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[1]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                ImageCanvas.Children.Add(border);

                Grid grid = new Grid();
                border.Child = grid;

                Image image = new Image();
                image.SetBinding(Image.SourceProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].TopLightBitmap", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                grid.Children.Add(image);

                //if (i == 0)
                {
                    ItemsControl itemsControl = new ItemsControl();
                    itemsControl.Tag = i;
                    itemsControl.DataContext = InfoBox.Instance;
                    itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
                    {
                        Path = new PropertyPath(string.Format("CurrentModel.MarginMeasurePosList{0}", i)), // 이게 최선입니까.....
                        Mode = BindingMode.OneWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });

                    //itemsControl.ItemContainerStyle = new Style(typeof(ContentPresenter));
                    //itemsControl.ItemContainerStyle.Setters.Add(new Setter(Canvas.LeftProperty, new Binding() { Path = new PropertyPath("Rectangle.Left"), Mode = BindingMode.OneWay }));
                    //itemsControl.ItemContainerStyle.Setters.Add(new Setter(Canvas.TopProperty, new Binding() { Path = new PropertyPath("Rectangle.Top"), Mode = BindingMode.OneWay }));

                    itemsControl.ItemTemplate = new DataTemplate();
                    FrameworkElementFactory canvasFactory = new FrameworkElementFactory(typeof(Canvas));
                    FrameworkElementFactory rectangleFactory = new FrameworkElementFactory(typeof(Rectangle));
                    rectangleFactory.SetValue(Canvas.LeftProperty, new Binding() { Path = new PropertyPath("Rectangle.Left"), Mode = BindingMode.OneWay, Converter = new Conv() });
                    rectangleFactory.SetValue(Canvas.TopProperty, new Binding() { Path = new PropertyPath("Rectangle.Top"), Mode = BindingMode.OneWay, Converter = new Conv() });
                    rectangleFactory.SetValue(Rectangle.WidthProperty, new Binding() { Path = new PropertyPath("Rectangle.Width"), Mode = BindingMode.OneWay, Converter = new Conv() });
                    rectangleFactory.SetValue(Rectangle.HeightProperty, new Binding() { Path = new PropertyPath("Rectangle.Height"), Mode = BindingMode.OneWay, Converter = new Conv() });
                    rectangleFactory.SetValue(Rectangle.StrokeThicknessProperty, 10.0);
                    rectangleFactory.SetValue(Rectangle.StrokeProperty, Brushes.Yellow);
                    canvasFactory.AppendChild(rectangleFactory);
                    itemsControl.ItemTemplate.VisualTree = canvasFactory;
                    grid.Children.Add(itemsControl);
                }
            }

            HomeLabel.SetBinding(Control.VisibilityProperty, new Binding()
            {
                Source = InfoBox.Instance,
                Path = new PropertyPath("Stopable"),
                Converter = new BooleanToVisibilityConverter()
            });

            MachineCircleLabel.SetBinding(Control.VisibilityProperty, new Binding()
            {
                Source = InfoBox.Instance,
                Path = new PropertyPath("Stopable"),
                Converter = new BooleanToVisibilityConverter()
            });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Start();
        }

        private void UpdateMouseImage(int flowPosition, System.Drawing.Point ptImg)
        {
            ResultCombiner resultCombiner = SystemManager.Instance().OperatorManager.ResultCombiner;

            AlgoImage algoImage = resultCombiner.ScanOperatorResultArray[flowPosition].TopLightImage;
            if (algoImage == null)
                return;

            int scanPixelPositionX = resultCombiner.ScanOperatorResultArray[flowPosition].PixelPositionX;

            AlgoImage maskBuffer = resultCombiner.ExtractOperatorResultArray[flowPosition]?.MaskBuffer;
            if (maskBuffer == null)
                return;

            ExtractOperatorResult extractOperatorResult = resultCombiner.ExtractOperatorResultArray[0];
            if (extractOperatorResult == null)
                return;

            int w = (int)(Math.Min(algoImage.Width, algoImage.Height) * this.mouseImageZoomScale);

            System.Drawing.Rectangle mouseRect = DrawingHelper.FromCenterSize(ptImg, new System.Drawing.Size(w, (int)(1.8 * w)));
            int offsetX = mouseRect.Left < 0 ? (-mouseRect.Left) : (mouseRect.Right > algoImage.Width ? (algoImage.Width - mouseRect.Right) : (0));
            int offsetY = mouseRect.Top < 0 ? (-mouseRect.Top) : (mouseRect.Bottom > algoImage.Height ? (algoImage.Height - mouseRect.Bottom) : (0));
            mouseRect.Offset(offsetX, offsetY);
            //DynMvp.ConsoleEx.WriteLine($"Flow {flowPosition}, mouseRect.X: {mouseRect.X}");

            List<System.Drawing.PointF> blobCenterPointList;
            using (AlgoImage subMaskBuffer = maskBuffer.GetSubImage(mouseRect))
            {
                System.Drawing.Point centerPt = new System.Drawing.Point(subMaskBuffer.Width / 2, subMaskBuffer.Height / 2);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subMaskBuffer);
                using (BlobRectList blobRectList = ip.Blob(subMaskBuffer, new BlobParam()))
                {
                    List<BlobRect> totalBlobRectList = blobRectList.GetList();
                    List<BlobRect> noTouchBorderList = totalBlobRectList.FindAll(f => !f.IsTouchBorder);

                    if (noTouchBorderList.Count > 0)
                        blobCenterPointList = noTouchBorderList.Select(f => f.CenterPt).OrderBy(f => MathHelper.GetLength(f, centerPt)).ToList();
                    else
                        blobCenterPointList = totalBlobRectList.Select(f => f.CenterPt).OrderBy(f => MathHelper.GetLength(f, centerPt)).ToList();
                }

                if (blobCenterPointList.Count > 0)
                {
                    System.Drawing.Point firstBlobPos = System.Drawing.Point.Round(blobCenterPointList.First());
                    //System.Drawing.Point offset = new System.Drawing.Point(mouseRect.Left + firstBlobPos.X - ptImg.X, mouseRect.Top + firstBlobPos.Y - ptImg.Y);
                    System.Drawing.Point offset = new System.Drawing.Point(firstBlobPos.X - mouseRect.Width / 2, firstBlobPos.Y - mouseRect.Height / 2);
                    mouseRect.Offset(offset.X, offset.Y);
                }
            }

            if (extractOperatorResult.VertexPoints.Length == 4)
            {
                System.Drawing.Point baseLT = extractOperatorResult.VertexPoints[0];
                System.Drawing.Point baseLB = extractOperatorResult.VertexPoints[3];
                System.Drawing.Point baseLoc = new System.Drawing.Point(scanPixelPositionX + mouseRect.X, mouseRect.Y);
                //DynMvp.ConsoleEx.WriteLine($"Flow {flowPosition}, BaseLocX: {scanPixelPositionX}+{mouseRect.X}={baseLoc.X}");

                //if (InfoBox.Instance.CurrentModel.MarginMeasurePosList.Count > 0)
                //    baseLB = InfoBox.Instance.CurrentModel.MarginMeasurePosList.First().BasePos;

                this.marginMeasurePos = new MarginMeasurePos();
                this.marginMeasurePos.BasePos = new System.Drawing.Point[] { baseLB, baseLT, baseLoc };
                this.marginMeasurePos.Name = string.Format("M_F{0}_Y{1}", flowPosition, ptImg.Y);
                this.marginMeasurePos.FlowPosition = flowPosition;
                this.marginMeasurePos.Rectangle = mouseRect;

                mouseRect.Intersect(new System.Drawing.Rectangle(System.Drawing.Point.Empty, algoImage.Size));
                if (mouseRect.Width > 0 && mouseRect.Height > 0)
                {
                    using (DynMvp.Vision.AlgoImage algoImage2 = algoImage.GetSubImage(mouseRect))
                        this.marginMeasurePos.BgBitmapSource = algoImage2.ToBitmapSource();

                    this.marginMeasurePos.MeasureParam.ThresholdA = this.marginMeasurePos.GetAutoThresholdValue();
                    this.marginMeasurePos.MeasureParam.ThresholdM = this.marginMeasurePos.MeasureParam.ThresholdA;
                }
                else
                {
                    this.marginMeasurePos.BgBitmapSource = null;
                }

                OnPropertyChanged("MarginMeasurePos");
            }
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            List<PatternGroup> pgList = GetSelectedPattern(this.CandidatePatternListView);

            pgList.ForEach(f =>
            {
                SystemManager.Instance().CurrentModel.CandidatePatternList.Remove(f);
                SystemManager.Instance().CurrentModel.InspectPatternList.Add(f);
            });
            SystemManager.Instance().CurrentModel.SortPatternGroup();

            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().CurrentModel.Modified = true;
            //SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            List<PatternGroup> pgList = GetSelectedPattern(this.InspectPatternListView);
            pgList.ForEach(f =>
            {
                SystemManager.Instance().CurrentModel?.InspectPatternList.Remove(f);
                SystemManager.Instance().CurrentModel?.CandidatePatternList.Add(f);
            });
            SystemManager.Instance().CurrentModel.SortPatternGroup();

            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().CurrentModel.Modified = true;
            //SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().OperatorManager.Teach();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            List<PatternGroup> pgList = GetSelectedPattern(this.CandidatePatternListView);
            if (pgList.Count == 0)
            {
                CustomMessageBox.Show(LocalizeHelper.GetString("There is no selected Candidate pattern"));
                return;
            }

            string message = LocalizeHelper.GetString("Are you really going to delete the selected patterns?");
            if (CustomMessageBox.Show(message, null, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            pgList.ForEach(pg => SystemManager.Instance().CurrentModel?.CandidatePatternList.Remove(pg));
            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().CurrentModel.Modified = true;
            //SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private List<PatternGroup> GetSelectedPattern(ListView listView)
        {
            List<PatternGroup> pgList = new List<PatternGroup>();
            foreach (object obj in listView.SelectedItems)
            {
                if (obj is PatternGroup)
                    pgList.Add((PatternGroup)obj);
            }

            return pgList;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            OnPropertyChanged("CurMarkPos");
            SystemManager.Instance().OperatorManager.Start(true);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().OperatorManager.Cancle(null);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SimpleProgressWindow teachWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Save"));
            bool ok = true;
            teachWindow.Show(() =>
            {
                ok = SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                if (ok)
                    SystemManager.Instance().CurrentModel.Modified = false;
            });

            if (!ok)
                CustomMessageBox.Show("Model Save Fail", MessageBoxButton.OK);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnPropertyChanged("CurMarkPos");
            OnPropertyChanged("CurMarkBrush");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int trX = 250;
            int trY = 5;
            double scaleX = Math.Max(0, (MainCanvas.ActualWidth - trX)) / ((double)(InfoBox.Instance.DispRobotRegion.Width));
            double scaleY = MainCanvas.ActualHeight / ((double)(InfoBox.Instance.DispRobotRegion.Height));
            double scale = Math.Min(scaleX, scaleY);
            Scale.ScaleX = Scale.ScaleY = scale;
            RobotRegion.StrokeThickness = 1 / scale;

            Translate.X = trX / scale;
            Translate.Y = trY / scale;

            OnPropertyChanged("MarkSize");
            OnPropertyChanged("HomeMarkPos");
            OnPropertyChanged("MarkFontSize");
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void MarginDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarginSelected();
        }

        private void MarginDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MarginSelected();
        }

        private void MarginSelected()
        {
            MarginMeasurePos selected = (MarginMeasurePos)((DataGrid)marginDataGrid).SelectedItem;
            if (selected == null)
                return;

            this.marginMeasurePos = selected;
            //this.marginMeasurePos.CopyFrom(selected, true);
            OnPropertyChanged("MarginMeasurePos");
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            Transform transform = canvas.RenderTransform;
            double scaleX = (double)transform.GetValue(ScaleTransform.ScaleXProperty);
            double scaleY = (double)transform.GetValue(ScaleTransform.ScaleYProperty);

            double d = 0.02;
            if (e.Delta < 0)
                d *= -1;

            transform.SetValue(ScaleTransform.ScaleXProperty, 1.0);
            transform.SetValue(ScaleTransform.ScaleYProperty, 1.0);
            canvas.InvalidateProperty(ScaleTransform.ScaleXProperty);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem listViewItem = sender as ListViewItem;
            if (listViewItem == null)
                return;

            PatternGroup pg = listViewItem.Content as PatternGroup;
            if (pg == null)
                return;

            string imageFile = pg.RefImagePath;
            //if (!File.Exists(imageFile))
            {
                new SimpleProgressWindow("Wait").Show(() =>
                {
                    Directory.CreateDirectory(@"C:\temp");
                    imageFile = @"C:\temp\temp.bmp";
                    UniScanWPF.Helper.WPFImageHelper.SaveBitmapSource(imageFile, pg.RefImage);
                });
            }
            System.Diagnostics.Process.Start(imageFile);
        }

        private void gridMargin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Helper.WPFImageHelper.SaveBitmapSource(@"c:\temp\marginMeasurePos.bmp", marginMeasurePos.BitmapSource);

            if (e.ClickCount > 1)
            {
                marginDataGrid.IsReadOnly = true;

                MarginTeachWindow marginTeachWindow = new MarginTeachWindow();
                marginTeachWindow.WindowState = WindowState.Normal;
                marginTeachWindow.RenderSize = new Size(200, 200);
                marginTeachWindow.MarginMeasurePos = this.marginMeasurePos.Clone(true);
                marginTeachWindow.Model = marginTeachWindow.MarginMeasurePos;
                marginTeachWindow.OnTestInspection += MarginTeachWindow_OnTestInspection;
                marginTeachWindow.ShowDialog();

                this.marginMeasurePos.CopyFrom((MarginMeasurePos)marginTeachWindow.Model, true);

                marginDataGrid.Items.Refresh();
                marginDataGrid.IsReadOnly = false;
            }
        }

        private void MarginTeachWindow_OnTestInspection(MarginMeasurePos marginMeasurePos)
        {
            int flowPos = marginMeasurePos.FlowPosition;
            ResultCombiner resultCombiner = SystemManager.Instance().OperatorManager.ResultCombiner;

            //AlgoImage topLightImage = resultCombiner.ScanOperatorResultArray[flowPos]?.TopLightImage;
            //AlgoImage maskBuffer = resultCombiner.ExtractOperatorResultArray[flowPos]?.MaskBuffer;
            //if (topLightImage == null || maskBuffer == null)
            //    return;

            //System.Drawing.Rectangle rect = marginMeasurePos.Rectangle;
            //AlgoImage subTopLightImage = topLightImage.Clip(rect);
            //AlgoImage subMaskBuffer = maskBuffer.Clip(rect);

            BitmapSource bitmapSource = marginMeasurePos.BgBitmapSource;
            System.Drawing.Size bitmapSourceSize = new System.Drawing.Size(bitmapSource.PixelWidth, bitmapSource.PixelHeight);
            byte[] bitmapSourceBytes = WPFImageHelper.BitmapSourceToBytes(bitmapSource);

            AlgoImage subTopLightImage = ImageBuilder.Build(UniEye.Base.Settings.OperationSettings.Instance().ImagingLibrary, ImageType.Grey, bitmapSourceSize);
            AlgoImage subMaskBuffer = ImageBuilder.Build(UniEye.Base.Settings.OperationSettings.Instance().ImagingLibrary, ImageType.Grey, bitmapSourceSize);

            subTopLightImage.SetByte(bitmapSourceBytes);
            subTopLightImage.Save(@"C:\temp\subTopLightImage.bmp");
            Tuple<System.Drawing.RectangleF, float[], BitmapSource> result = Algorithm.MarginMeasureAlgorhtm.Measure(subTopLightImage, subMaskBuffer, marginMeasurePos, null);

            subMaskBuffer.Dispose();
            subTopLightImage.Dispose();

            Helper.WPFImageHelper.SaveBitmapSource(@"C:\temp\result.bmp", result.Item3);
            Process.Start(@"C:\temp\result.bmp");

            Debug.WriteLine(string.Format("MarginTeachWindow_OnTestInspection - {0}, {1}, {2}, {3}", result.Item2[0], result.Item2[1], result.Item2[2], result.Item2[3]));
        }
    }

    public class NumericValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }

        public float Min { get => min; set => min = value; }
        float min;

        public float Max { get => max; set => max = value; }
        float max;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                float f = float.Parse(value.ToString());
                if (f < min)
                    return new ValidationResult(false, string.Format("input number is Less than {0}", min));
                else if (f > max)
                    return new ValidationResult(false, string.Format("input number is Greater than {0}", max));

                return ValidationResult.ValidResult;
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "is not a number");
            }
        }
    }

    public class Conv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type srcType = value.GetType();
            int dValue = (int)value;
            return (double)dValue / InfoBox.Instance.ImageCanvasScale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double dValue = (double)value;
            return dValue * InfoBox.Instance.ImageCanvasScale;
        }
    }

    public static class Filter
    {
        public static readonly DependencyProperty ByProperty = DependencyProperty.RegisterAttached(
            "By",
            typeof(Predicate<object>),
            typeof(Filter),
            new PropertyMetadata(default(Predicate<object>), OnByChanged));

        public static void SetBy(ItemsControl element, Predicate<object> value)
        {
            element.SetValue(ByProperty, value);
        }

        public static Predicate<object> GetBy(ItemsControl element)
        {
            return (Predicate<object>)element.GetValue(ByProperty);
        }

        private static void OnByChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsControl itemsControl &&
                itemsControl.Items.CanFilter)
            {
                itemsControl.Items.Filter = (Predicate<object>)e.NewValue;
            }
        }
    }
}