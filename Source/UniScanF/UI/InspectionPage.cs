using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using Infragistics.Win.DataVisualization;
using DynMvp.Authentication;

namespace UniScan.UI
{
    public partial class InspectionPage : UserControl, IInspectionPage
    {
        public IInspectionPanel InspectionPanel => throw new NotImplementedException();

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        List<IInspectionPanel> IInspectionPage.InspectionPanelList => throw new NotImplementedException();

        //Control IMainTabPage.ShowHideControl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public InspectionPage()
        {
            InitializeComponent();

            SetupRealTimeChart(chartSheetThickness);
            SetupTrendChart(chartSheetThicknessTrend);
            SetupRealTimeChart(chartPetThickness);
            SetupTrendChart(chartPetThicknessTrend);
        }

        void SetupRealTimeChart(UltraDataChart realTimeChart)
        {

        }

        void SetupTrendChart(UltraDataChart trendChart)
        {
            //var commonXAxis = new NumericXAxis
            //{
            //    Label = "Date",
            //    LabelFontSize = 12,
            //    LabelExtent = 30,
            //    LabelLocation = AxisLabelsLocation.OutsideBottom
            //};
            //commonXAxis.FormatLabel += OnAxisXFormatLabel;

            //var priceYAxis = new NumericYAxis
            //{
            //    LabelExtent = 50,
            //    LabelFontSize = 12,
            //    //LabelTextColor = new SolidColorBrush { Color = Color.CadetBlue },
            //    LabelHorizontalAlignment = HorizontalAlignment.Right
            //};
            //priceYAxis.FormatLabel += OnAxisYFormatLabel;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int groupId, InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void Load2dImage(int cameraIndex, int stepIndex, int lightTypeIndex)
        {
            throw new NotImplementedException();
        }

        public void ModelChanged()
        {
            throw new NotImplementedException();
        }

        public void OnPostInspection()
        {
            throw new NotImplementedException();
        }

        public void OnPreInspection()
        {
            throw new NotImplementedException();
        }

        public void PreTargetGroupInspect(TargetGroup targetGroup, InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {
            throw new NotImplementedException();
        }

        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
        {
            throw new NotImplementedException();
        }

        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void UpdateInspectionNo(string inspectionNo)
        {
            throw new NotImplementedException();
        }

        public void UpdatePanel()
        {
            throw new NotImplementedException();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {

        }

        public ProductionBase GetCurrentProduction()
        {
            throw new NotImplementedException();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }

        public void EnableControls(UserType userType)
        {
            throw new NotImplementedException();
        }

        void IInspectionPage.ProductInspected(InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        void IMainTabPage.EnableControls(UserType userType)
        {
            throw new NotImplementedException();
        }

        void IPage.UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        void IPage.PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }
    }
}
