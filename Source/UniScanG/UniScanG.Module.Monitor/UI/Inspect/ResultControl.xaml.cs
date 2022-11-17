using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Exporter;
using UniScanG.Module.Monitor.Inspect;
using UniScanG.Module.Monitor.Settings;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.UI.Inspect
{
    /// <summary>
    /// Result.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ResultControl : UserControl, INotifyPropertyChanged, IMultiLanguageSupport
    {
        public static readonly DependencyProperty InspectionResultProperty =
            DependencyProperty.Register("InspectionResult", typeof(InspectionResult), typeof(ResultControl));

        public static readonly DependencyProperty TeachDataProperty =
            DependencyProperty.Register("TeachData", typeof(TeachData), typeof(ResultControl));

        public bool IsTeachMode { get => this.isTeachMode; set => this.isTeachMode = value; }
        bool isTeachMode;


        public event PropertyChangedEventHandler PropertyChanged;

        public InspectionResult InspectionResult
        {
            get { return (InspectionResult)GetValue(InspectionResultProperty); }
            set { SetValue(InspectionResultProperty, value); }
        }

        public TeachData TeachData
        {
            get { return (TeachData)GetValue(TeachDataProperty); }
            set { SetValue(TeachDataProperty, value); }
        }

        public Visibility TeachVisibility => isTeachMode ? Visibility.Visible : Visibility.Collapsed;

        public string ButtonString => isTeachMode ? LocalizeHelper.GetString(this.GetType().FullName, "Comm Closed") : LocalizeHelper.GetString(this.GetType().FullName, "Comm Opened");

        public System.Windows.Media.Brush ButtonColor => isTeachMode ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.LightGreen;

        public ResultControl()
        {
            InitializeComponent();
            LocalizeHelper.AddListener(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            this.isTeachMode = true;
#else
            this.isTeachMode = false;
#endif
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MelsecDataExporter melsecDataExporter = SystemManager.Instance().DataExporterList.Find(f => f is MelsecDataExporter) as MelsecDataExporter;
            if (melsecDataExporter != null)
            {
                this.isTeachMode = !this.isTeachMode;
                melsecDataExporter.BlockUpdate = isTeachMode;
                if (!this.isTeachMode)
                {
                    SystemManager.Instance().SaveModel(true);
                }
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ButtonString"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ButtonColor"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TeachVisibility"));
        }
    }

    public class BackGbConvertor : MarkupExtension,IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return System.Windows.Media.Brushes.LightGreen;
            return System.Windows.Media.Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
