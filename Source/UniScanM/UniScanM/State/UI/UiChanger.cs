using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using UniEye.Base;
using UniEye.Base.UI;
using UniScanM.Data;

namespace UniScanM.UI
{
    public abstract class UiChanger : UniEye.Base.UI.UiChanger
    {
        public bool LiveReportMode { get; protected set; } = false;

        public override IMainForm CreateMainForm()
        {
            return (IMainForm)(new MainForm());
        }

        public virtual IInspectionPage CreateInspectionPage()
        {
            return (IInspectionPage)(new InspectionPage());
        }

        public virtual IModelManagerPage CreateModelManagerPage()
        {
            return (IModelManagerPage)(new ModelManagerPage());
        }

        public virtual PLCStatusPanel CreatePLCStatusPanel()
        {
            return new UI.PLCStatusPanel() { Dock = System.Windows.Forms.DockStyle.Fill, IsGlossOnly = false };
        }

        public abstract ITeachPage CreateTeachPage();
        public abstract ReportPageController CreateReportPageController();
    }

    public delegate void UpdateDataDelegate(DataImporter dataImporter, bool showNgOnly);
    public interface IUniScanMReportPanel : IReportPanel
    {
        string AxisYFormat { get; }
        bool ShowNgOnlyButton();

        void UpdateData(DataImporter dataImporter, bool showNgOnly);

        Dictionary<string, List<DataPoint>> GetChartData(int startPos, int endPos, DataImporter dataImporter);

        DataImporter CreateDataImprter();

        bool InitializeChart(Chart chart);
    }
}
