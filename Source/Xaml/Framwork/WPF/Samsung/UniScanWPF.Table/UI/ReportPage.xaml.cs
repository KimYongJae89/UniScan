using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using UniEye.Base.Settings;
using UniScanWPF.Table;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using UniScanWPF.Table.UI;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class ReportPage : Page, INotifyPropertyChanged, IMultiLanguageSupport
    {
        TranslateTransform originTranslateTransform = new TranslateTransform();
        ImagePanel[] imagePanels = new ImagePanel[2];

        public bool ShowGuideLine
        {
            get => this.showGuideLine;
            set
            {
                if (this.showGuideLine != value)
                {
                    this.showGuideLine = value;
                    Array.ForEach(this.imagePanels, f => f.ShowGuideLine = value);
                    OnPropertyChanged("ShowGuideLine");
                }
            }
        }
        bool showGuideLine;

        public int[][] SummaryData { get => summaryData; }
        int[][] summaryData = new int[1][] { new int[] { 0, 0, 0 } };

        public double[] LengthHeight { get => this.lengthHeight; }
        double[] lengthHeight = new double[3];

        public double[] LengthWidth { get => this.lengthWidth; }
        double[] lengthWidth = new double[3];

        public MeanderMeasure[] MeanderMeasureData { get => this.meanderMeasureData; }
        MeanderMeasure[] meanderMeasureData = new MeanderMeasure[3];

        public ObservableCollection<ExtraMeasure> MarginMeasureData { get => this.marginMeasureData; }
        ObservableCollection<ExtraMeasure> marginMeasureData = new ObservableCollection<ExtraMeasure>();

        public string[] ExtraValues => extraValues;
        string[] extraValues = new string[6] {"", "", "", "", "", "" };

        public Brush JudgementBrush => extraValues[5] == "NG" ? new SolidColorBrush(Colors.OrangeRed) : new SolidColorBrush(Colors.White);

        public InspectOperatorSettings InspectOperatorSettings { get => this.inspectOperatorSettings; }
        InspectOperatorSettings inspectOperatorSettings = null;

        public BitmapSource BigDefectBitmapSource
        {
            get => bigDefectBitmapSource;
            set
            {
                if (this.bigDefectBitmapSource != value)
                {
                    this.bigDefectBitmapSource = value;
                    OnPropertyChanged("BigDefectBitmapSource");
                }
            }
        }
        BitmapSource bigDefectBitmapSource = null;

        public bool IsHideChecked
        {
            get => visibilitySecondImagePanel;
            set
            {
                visibilitySecondImagePanel = value;
                OnPropertyChanged("IsHideChecked");
            }
        }
        bool visibilitySecondImagePanel = true;

        ObservableCollection<CanvasDefect> defectList = new ObservableCollection<CanvasDefect>();
        List<LoadItem> curResultList = null;
        TranslateTransform offsetTranslateTransform = new TranslateTransform();

        public ObservableCollection<CanvasDefect> DefectList { get => defectList; set => defectList = value; }


        public string PatternCheckBoxText { get => LocalizeHelper.GetString(string.Format("Pattern({0})", !this.PatternCheckBox.IsChecked.HasValue ? "Lump" : this.PatternCheckBox.IsChecked.Value ? "Normal" : "Off")); }
        public string MarginCheckBoxText { get => LocalizeHelper.GetString(string.Format("Margin({0})", !this.MarginCheckBox.IsChecked.HasValue ? "Circle" : this.MarginCheckBox.IsChecked.Value ? "All" : "Off")); }
        public string ShapeCheckBoxText { get => LocalizeHelper.GetString(string.Format("Shape({0})", this.ShapeCheckBox.IsChecked.Value ? "All" : "Off")); }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReportPage()
        {
            InitializeComponent();
            this.DataContext = this;

            this.imagePanels[0] = new ImagePanel(true, true);
            this.imagePanels[0].Notify = SetCurrentIndex;
            this.imagePanels[0].OnTranslateChanged = ImagePanel0_OnTranslateChanged;
            this.imagePanels[0].OnZoomChanged = ImagePanels_OnZoomChanged;
            this.firstImagePanel.Navigate(this.imagePanels[0]);

            this.imagePanels[1] = new ImagePanel(true, false);
            this.imagePanels[1].Notify = SetCurrentIndex;
            this.imagePanels[1].OnTranslateChanged = ImagePanel1_OnTranslateChanged;
            this.secondImagePanelFrame.Navigate(this.imagePanels[1]);

            LocalizeHelper.AddListener(this);
        }

        private void ImagePanels_OnZoomChanged(ScaleTransform scaleTransform)
        {
            this.imagePanels[1].ScaleTransform = scaleTransform;
        }

        private void ImagePanel0_OnTranslateChanged(TranslateTransform translateTransform)
        {
            this.imagePanels[1].TranslateTransform.X = translateTransform.X + this.offsetTranslateTransform.X;
            this.imagePanels[1].TranslateTransform.Y = translateTransform.Y + this.offsetTranslateTransform.Y;
        }

        private void ImagePanel1_OnTranslateChanged(TranslateTransform translateTransform)
        {
            this.offsetTranslateTransform.X = this.imagePanels[1].TranslateTransform.X - this.imagePanels[0].TranslateTransform.X;
            this.offsetTranslateTransform.Y = this.imagePanels[1].TranslateTransform.Y - this.imagePanels[0].TranslateTransform.Y;
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

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDate == null || EndDate == null)
                return;

            if (StartDate.SelectedDate > EndDate.SelectedDate)
            {
                DatePicker datePicker = sender as DatePicker;
                if (datePicker.Name == "StartDate")
                    EndDate.SelectedDate = StartDate.SelectedDate;
                else
                    StartDate.SelectedDate = EndDate.SelectedDate;
                return;
            }

            SearchProduction();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SearchProduction();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
                SearchProduction();
        }

        private void SearchProduction()
        {
            if (ProductionList == null)
                return;

            ProductionList.ItemsSource = null;

            ProductionList.ItemsSource = SystemManager.Instance().ProductionManager.List.FindAll(f =>
            {
                Production production = f as Production;
                if (production == null)
                    return false;

                return production.StartTime >= StartDate.SelectedDate
                && production.StartTime <= EndDate.SelectedDate.Value.AddDays(1)
                && ((Production)production).Count > 0;
            });
        }

        private void DefectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CanvasDefect selectedDefect = defectListBox.SelectedItem as CanvasDefect;
            this.BigDefectBitmapSource = selectedDefect?.Defect.GetBitmapSource();

            Array.ForEach(this.imagePanels, f =>
             {
                 f.ClearSelection();
                 f.SetSelection(selectedDefect);
             });
        }

        private void PatternCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f => f?.VisiblePatternCanvas(PatternCheckBox.IsChecked));
            OnPropertyChanged("PatternCheckBoxText");
        }

        private void MarginCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f => f?.VisibleMarginCanvas(MarginCheckBox.IsChecked));
            OnPropertyChanged("MarginCheckBoxText");
        }

        private void ShapeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f => f?.VisibleShapeCanvas(ShapeCheckBox.IsChecked));
            OnPropertyChanged("ShapeCheckBoxText");
        }

        private void ProductionChange(ImagePanel sender, int index)
        {
            this.BigDefectBitmapSource = null;

            if (curResultList != null && curResultList.Count != 0)
            {
                LoadItem tuple = curResultList[index];
                bool loaded = tuple.IsLoaded;
                if (!loaded)
                {
                    //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    //SimpleProgressWindow simpleProgressWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Load"));
                    //simpleProgressWindow.Show(() =>
                    //{
                    //    loaded = tuple.Load(cancellationTokenSource.Token);
                    //}, cancellationTokenSource);
                    //if (cancellationTokenSource.IsCancellationRequested || loaded == false)
                    //    return;

                    ProgressBarWindow progressBarWindow = new ProgressBarWindow(LocalizeHelper.GetString("Load"), null);
                    progressBarWindow.BackgroundWorker.DoWork += new DoWorkEventHandler((s, e) =>
                    {
                        BackgroundWorker bWorker = (BackgroundWorker)s;
                        DoWorkEventArgs arg = (DoWorkEventArgs)e;
                        loaded = tuple.Load(bWorker);
                    });
                    progressBarWindow.ShowDialog();
                }

                if (loaded == false)
                    return;

                if (sender == null || sender == this.imagePanels[0])
                {
                    defectList.Clear();
                    CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Filter = Filter;
                    UpdateTeachDataProperty(tuple.OperatorSettingList);
                    UpdateLengthProperty(tuple.CanvasDefectList);
                    tuple.CanvasDefectList.ForEach(f => defectList.Add(f));
                }

                if (sender != null)
                    sender.ProductionChange(tuple);
                else
                    Array.ForEach(this.imagePanels, f => f?.ProductionChange(tuple));
            }
            OnPropertyChanged("DefectList");
        }

        private void UpdateTeachDataProperty(List<OperatorSettings> operatorSettingList)
        {
            this.inspectOperatorSettings = (InspectOperatorSettings)operatorSettingList.Find(f => f is InspectOperatorSettings);
            OnPropertyChanged("InspectOperatorSettings");
        }

        private void UpdateLengthProperty(List<CanvasDefect> canvasDefectList)
        {
            UpdateLengthWHProperty(canvasDefectList);
            UpdateLengthMeanderProperty(canvasDefectList);
            UpdateLengthMarginProperty(canvasDefectList);
        }

        private void UpdateLengthMarginProperty(List<CanvasDefect> canvasDefectList)
        {
            List<ExtraMeasure> list = canvasDefectList.FindAll(f => f.Defect is ExtraMeasure).ConvertAll(f => (ExtraMeasure)f.Defect);
            //this.marginMeasureData.Clear();
            //this.marginMeasureData.AddRange(list);

            this.marginMeasureData = new ObservableCollection<ExtraMeasure>(list);
            this.extraValues = new string[] { "", "", "", "", "", "" };
            if (list.Count > 0)
            {
                if (inspectOperatorSettings.MarginMeasureParam.DesignedUm.Width == inspectOperatorSettings.MarginMeasureParam.DesignedUm.Height)
                    extraValues[0] = $"{inspectOperatorSettings.MarginMeasureParam.DesignedUm.Width}";
                else
                    extraValues[0] = $"W {inspectOperatorSettings.MarginMeasureParam.DesignedUm.Width}{Environment.NewLine}L {inspectOperatorSettings.MarginMeasureParam.DesignedUm.Height}";

                extraValues[1] = $"{inspectOperatorSettings.MarginMeasureParam.JudgementSpecUm}";

                List<float> wList = new List<float>();
                List<float> lList = new List<float>();

                list.ForEach(f =>
                {
                    if (float.TryParse(f.Width, out float ww) && ww > 0)
                        wList.Add(ww);

                    if (float.TryParse(f.Height, out float ll) && ll > 0)
                        lList.Add(ll);
                });

                float[] w = wList.ToArray();
                float[] l = lList.ToArray();

                float wMin = w.Min();
                float wMax = w.Max();
                float wAverage = w.Average();
                bool? wJudgement = null;
                if ((inspectOperatorSettings != null) && (inspectOperatorSettings.MarginMeasureParam.DesignedUm.Width > 0) && w.Length > 0)
                    wJudgement = !(w.Select(f => Math.Abs(f - inspectOperatorSettings.MarginMeasureParam.DesignedUm.Width)).Max() > inspectOperatorSettings.MarginMeasureParam.JudgementSpecUm);

                if (l.Length > 0)
                {
                    float lMin = l.Min();
                    float lMax = l.Max();
                    float lAverage = l.Average();
                    bool? lJudgement = null;
                    if ((inspectOperatorSettings != null) && (inspectOperatorSettings.MarginMeasureParam.DesignedUm.Height > 0))
                        lJudgement = !(l.Select(f => Math.Abs(f - inspectOperatorSettings.MarginMeasureParam.DesignedUm.Height)).Max() > inspectOperatorSettings.MarginMeasureParam.JudgementSpecUm);

                    bool? judgement = null;
                    if (wJudgement.HasValue && lJudgement.HasValue)
                        judgement = wJudgement.Value && lJudgement.Value;
                    else if(wJudgement.HasValue)
                        judgement = wJudgement.Value;
                    else if (lJudgement.HasValue)
                        judgement = lJudgement.Value;

                    this.extraValues[2] = $"W {wMin:F02}{Environment.NewLine}L {lMin:F02}";
                    this.extraValues[3] = $"W {wMax:F02}{Environment.NewLine}L {lMax:F02}";
                    this.extraValues[4] = $"W {wAverage:F02}{Environment.NewLine}L {lAverage:F02}";
                    this.extraValues[5] = $"{((judgement.HasValue) ? (judgement.Value ? "OK" : "NG") : string.Empty)}";
                }
                else
                {
                    this.extraValues[2] = $"W {wMin:F02}";
                    this.extraValues[3] = $"W {wMax:F02}";
                    this.extraValues[4] = $"W {wAverage:F02}";
                    this.extraValues[5] = $"{((wJudgement.HasValue) ? (wJudgement.Value ? "OK" : "NG") : string.Empty)}";
                }

                //this.marginMeasureData = list.ToArray();
                //int length = Math.Min(list.Count, this.marginMeasureData.Length);
                //for (int i = 0; i < this.marginMeasureData.Length; i++)
                //{
                //    if (i < list.Count)
                //        this.marginMeasureData[i] = list[i];
                //    else
                //        this.marginMeasureData[i] = null;
                //}
            }

            OnPropertyChanged("MarginMeasureData");
            OnPropertyChanged("VisibleMarginMeasureData");
            OnPropertyChanged("ExtraValues");
            //OnPropertyChanged("ExtraJudgement");
            OnPropertyChanged("JudgementBrush");
        }

        private void UpdateLengthMeanderProperty(List<CanvasDefect> canvasDefectList)
        {
            // Update Meander
            List<MeanderMeasure> list = canvasDefectList.FindAll(f => f.Defect is MeanderMeasure).ConvertAll(f => (MeanderMeasure)f.Defect);
            int length = Math.Min(list.Count, this.meanderMeasureData.Length);
            for (int i = 0; i < this.meanderMeasureData.Length; i++)
            {
                if (i < list.Count)
                    this.meanderMeasureData[i] = list[i];
                else
                    this.meanderMeasureData[i] = null;
            }
            OnPropertyChanged("MeanderMeasureData");
        }

        private void UpdateLengthWHProperty(List<CanvasDefect> canvasDefectList)
        {
            List<CanvasDefect> lengthCanvasDefectList = canvasDefectList.FindAll(f => f.Defect is LengthMeasure);

            // Update Height
            Array.Clear(this.lengthHeight, 0, this.lengthHeight.Length);
            List<CanvasDefect> verticalList = lengthCanvasDefectList.FindAll(f =>
            {
                LengthMeasure dd = f.Defect as LengthMeasure;
                return dd != null && dd.Direction == DynMvp.Vision.Direction.Vertical && dd.IsValid;
            });

            if (verticalList.Count > 0)
            {
                int indexSrc = verticalList.FindIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexDst = verticalList.FindLastIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexMid = (indexSrc + indexDst) / 2;

                if (indexSrc >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexSrc].Defect;
                    this.lengthHeight[0] = lengthMeasure.LengthMm;
                }
                if (indexMid >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexMid].Defect;
                    this.lengthHeight[1] = lengthMeasure.LengthMm;
                }
                if (indexDst >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexDst].Defect;
                    this.lengthHeight[2] = lengthMeasure.LengthMm;
                }
            }
            OnPropertyChanged("LengthHeight");


            Array.Clear(this.lengthWidth, 0, this.lengthWidth.Length);
            List<CanvasDefect> horizontalList = lengthCanvasDefectList.FindAll(f => ((LengthMeasure)f.Defect).Direction == DynMvp.Vision.Direction.Horizontal && ((LengthMeasure)f.Defect).LengthMm > 0);
            if (horizontalList.Count > 0)
            {
                int indexSrc = horizontalList.FindIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexDst = horizontalList.FindLastIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexMid = (indexSrc + indexDst) / 2;

                if (indexSrc >= 0)
                {
                    this.lengthWidth[0] = ((LengthMeasure)horizontalList[indexSrc].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexSrc]);
                }
                if (indexMid >= 0)
                {
                    this.lengthWidth[1] = ((LengthMeasure)horizontalList[indexMid].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexMid]);
                }
                if (indexDst >= 0)
                {
                    this.lengthWidth[2] = ((LengthMeasure)horizontalList[indexDst].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexDst]);
                }
            }
            OnPropertyChanged("LengthWidth");
        }

        private void ProductionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UniScanWPF.Table.Data.Production production = (UniScanWPF.Table.Data.Production)ProductionList.SelectedItem;
            if (production == null)
                return;

            SimpleProgressWindow loadProgressWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Load"));
            loadProgressWindow.Show(new Action(() =>
            {
                curResultList = StoringOperator.Load(production);
            }));

            UpdateSummary(production);

            Array.ForEach(this.imagePanels, f => f?.UpdateCombobox(production));

            SetCurrentIndex(null, 0);

            //ProductionChange();
        }

        private void SetCurrentIndex(ImagePanel sender, int index)
        {
            if (curResultList == null)
                return;

            int valid = Math.Min(curResultList.Count - 1, Math.Max(index, 0));
            ProductionChange(sender, valid);
        }

        private void UpdateSummary(Production production)
        {
            Array.Clear(summaryData[0], 0, summaryData[0].Length);
            summaryData[0][0] = production.PatternCount;
            summaryData[0][1] = production.MarginCount;
            summaryData[0][2] = production.ShapeCount;
            OnPropertyChanged("SummaryData");
        }

        //private bool Filter(object item)
        //{
        //    Enum type = ((CanvasDefect)item).Defect.ResultObjectType;

        //    switch (type)
        //    {
        //        case DefectType.CircularPattern:
        //        case DefectType.Pattern:
        //            if (PatternCheckBox.IsChecked == true)
        //                return true;
        //            break;
        //        case DefectType.CircularMargin:
        //        case DefectType.Margin:
        //            if (MarginCheckBox.IsChecked == true)
        //                return true;
        //            break;
        //        case DefectType.Shape:
        //            if (ShapeCheckBox.IsChecked == true)
        //                return true;
        //            break;
        //    }

        //    return false;
        //}

        private bool Filter(object item)
        {
            Enum type = ((CanvasDefect)item).Defect.ResultObjectType;

            switch (type)
            {
                case DefectType.Lump:
                    return !PatternCheckBox.IsChecked.HasValue;

                case DefectType.Pattern:
                case DefectType.CircularPattern:
                    return PatternCheckBox.IsChecked.HasValue && PatternCheckBox.IsChecked.Value;
                    //return !PatternCheckBox.IsChecked.HasValue;

                case DefectType.Margin:
                    return MarginCheckBox.IsChecked.HasValue && MarginCheckBox.IsChecked.Value;
                case DefectType.CircularMargin:
                    return !MarginCheckBox.IsChecked.HasValue;

                case DefectType.Shape:
                    return ShapeCheckBox.IsChecked.Value;

                default:
                    return false;
            }
                    return false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.PatternCheckBox.Foreground = Defect.GetBrush(DefectType.Pattern);
            this.MarginCheckBox.Foreground = Defect.GetBrush(DefectType.Margin);
            this.ShapeCheckBox.Foreground = Defect.GetBrush(DefectType.Shape);

            Array.ForEach(this.imagePanels, f =>
            {
                f?.VisibleMeanderCanvas(false);
                f?.VisibleMarginDistCanvas(true);
                f?.VisiblePLengthCanvas(false);
            });
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Production production = (Production)ProductionList.SelectedItem;
            BuildSummary(production);
        }

        private void BuildSummary(Production production)
        {
            Exception exception = null;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            new SimpleProgressWindow(LocalizeHelper.GetString("Build Summary")).Show(new Action(() =>
            {
                try
                {
                    curResultList.ForEach(f => f.Load(cancellationTokenSource.Token));
                    if (cancellationTokenSource.IsCancellationRequested)
                        return;

                    production.UpdateCount(curResultList);
                    SystemManager.Instance().ProductionManager.Save();
                }
                catch (Exception ex)
                { exception = ex; }
            }), cancellationTokenSource);

            if (exception != null)
            {
                LogHelper.Error(LoggerType.Error, $"ReportPage::BuildSummary - {exception.GetType()}: {exception.Message}");
                return;
            }

            UpdateSummary(production);
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            this.offsetTranslateTransform.X = 0;
            this.offsetTranslateTransform.Y = 0;

            this.imagePanels[1].ScaleTransform = this.imagePanels[0].ScaleTransform;
            this.imagePanels[1].TranslateTransform = this.imagePanels[0].translateTransform;
        }

        private void HideButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            imagePanelGrid.RowDefinitions[1].Height = new GridLength(!this.visibilitySecondImagePanel ? 1 : 0, GridUnitType.Star);
            imagePanels[1].Visibility = !this.visibilitySecondImagePanel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Array.ForEach(this.imagePanels, f => f.ZoomFit());
        }

        private void OpenReportPathButton_Click(object sender, RoutedEventArgs e)
        {
            string reportFolder = PathSettings.Instance().Result.Replace("Result", "Report");

            UniScanWPF.Table.Data.Production production = (UniScanWPF.Table.Data.Production)ProductionList.SelectedItem;
            if (production == null)
            {
            if (System.IO.Directory.Exists(reportFolder))
                System.Diagnostics.Process.Start(reportFolder);
                return;
            }

            string reportFolder2 = System.IO.Path.Combine(reportFolder, production.StartTime.ToString("yyyy-MM-dd"), $"{production.Name}_{production.LotNo}.xlsx");
            if (System.IO.File.Exists(reportFolder2))
            {
                //Process process = new Process();
                //process.StartInfo.FileName = reportFolder2;
                //process.StartInfo.Verb = "explore.exe";
                //process.Start();
                System.Diagnostics.Process.Start("Explorer.exe", $"/select, \"{reportFolder2}\"");
                return;
            }
        }

        private void OpenMarginPathButton_Click(object sender, RoutedEventArgs e)
        {
            string marginFolder = PathSettings.Instance().Result.Replace("Result", "Margin");
            if (System.IO.Directory.Exists(marginFolder))
                System.Diagnostics.Process.Start(marginFolder);
        }

        private void MarginMeasureGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MarginSelected((DataGrid)sender);
        }

        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MarginSelected((DataGrid)sender);
        }

        private void MarginSelected(DataGrid dataGrid)
        {
            ExtraMeasure extraMeasure = (ExtraMeasure)dataGrid.SelectedItem;
            if (extraMeasure == null)
                return;

            int loadItemIdx = this.imagePanels[0].CurLoadItem - 1;
            CanvasDefect canvasDefect = this.curResultList[loadItemIdx].CanvasDefectList.Find(f => f.Defect == extraMeasure);
            this.BigDefectBitmapSource = canvasDefect.Defect.GetBitmapSource();

            Array.ForEach(this.imagePanels, f =>
            {
                f.ClearSelection();
                f.SetSelection(canvasDefect);
            });
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                if (this.bigDefectBitmapSource != null)
                {
                    Directory.CreateDirectory(@"C:\temp");
                    Helper.WPFImageHelper.SaveBitmapSource(@"C:\temp\UniScanWpfTemp.bmp", this.bigDefectBitmapSource);
                    Process.Start(@"C:\temp\UniScanWpfTemp.bmp");
                }
            }
        }

    }
}
