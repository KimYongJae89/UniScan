using DynMvp.Base;
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
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.Inspect;

namespace UniScanG.Module.Monitor.UI.Inspect
{
    /// <summary>
    /// InspectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : Page, INotifyPropertyChanged
    {
        Brush goodBrush = new SolidColorBrush(Colors.LightGreen);
        Brush ngBrush = new SolidColorBrush(Colors.OrangeRed);
        Brush unselectedBrush = new SolidColorBrush(Colors.DarkCyan);

        public Brush Zone0BgColor => zoneBgColor[0];
        public Brush Zone1BgColor => zoneBgColor[1];
        public Brush Zone2BgColor => zoneBgColor[2];
        public Brush Zone3BgColor => zoneBgColor[3];
        public Brush Zone4BgColor => zoneBgColor[4];
        public Brush Zone5BgColor => zoneBgColor[5];

        Brush[] zoneBgColor = null;

        public BitmapSource BitmapSource => this.bitmapSource;
        BitmapSource bitmapSource = null;

        public InspectionResult InspectionResult => this.inspectionResult;
        InspectionResult inspectionResult = null;

        public TeachData TeachData => this.teachData;
        TeachData teachData;

        public int ZoneIndex => this.inspectionResult.ZoneIndex;

        public event PropertyChangedEventHandler PropertyChanged;

        public InspectPage()
        {
            this.zoneBgColor = new Brush[6];
            for (int i = 0; i < this.zoneBgColor.Length; i++)
                this.zoneBgColor[i] = unselectedBrush;

            InitializeComponent();

            SystemManager.Instance().InspectRunner.OnProductInspected += InspectRunner_OnProductInspected;
            SystemManager.Instance().OnModelChanged += InspectRunner_OnModelChanged;
        }

        private void InspectRunner_OnModelChanged()
        {
            this.teachData = ((Model)SystemManager.Instance().CurrentModel).TeachData;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TeachData"));
        }

        private void InspectRunner_OnProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            this.inspectionResult = inspectionResult as InspectionResult;
            //this.imageControl.InspectionResult = inspectionResult as InspectionResult;

            //System.Drawing.Bitmap bitmap = inspectionResult.GrabImageList.FirstOrDefault()?.ToBitmap();
            //if (bitmap == null)
            //    this.imageControl.BitmapSource = null;
            //else
            //    this.imageControl.BitmapSource = UniScanWPF.Helper.WPFImageHelper.ConvertImage(bitmap);

            UpdateData();
        }

        private void UpdateData()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InspectionResult"));
            for (int i = 0; i < this.zoneBgColor.Length; i++)
            {
                this.zoneBgColor[i] = (i != this.inspectionResult.ZoneIndex ? unselectedBrush : this.inspectionResult.IsGood() ? goodBrush : ngBrush);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Format("Zone{0}BgColor", i)));
            }
        }
    }
}
