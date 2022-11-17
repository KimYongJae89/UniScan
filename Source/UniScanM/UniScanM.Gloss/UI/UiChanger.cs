using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.UI;
using UniScanM.Gloss.Settings;
using UniScanM.UI;

namespace UniScanM.Gloss.UI
{
    public class UiChanger : UniScanM.UI.UiChanger
    {
        public UiChanger() : base()
        {
            LiveReportMode = true;
        }

        public override IInspectionPanel CreateInspectionPanel(int index)
        {
            switch (index)
            {
                case 0:
                    return new InspectionPanelLeft();
                case 1:
                    return new InspectionPanelRight();
            }
            return null;
        }

        public override ReportPageController CreateReportPageController()
        {
            return new UniScanM.Gloss.UI.Chart.ReportPageController();
        }

        public override IModelManagerPage CreateModelManagerPage()
        {
            var iModelManagerPage = base.CreateModelManagerPage();
            var modelManagerPage = iModelManagerPage as ModelManagerPage;
            modelManagerPage.Visible = false;
            return modelManagerPage;
        }

        public override IReportPanel CreateReportPanel()
        {
            return new Gloss.UI.ReportPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanM.Gloss.UI.SettingPage((GlossSettings)GlossSettings.Instance());
        }

        public override ITeachPage CreateTeachPage()
        {
            return new UniScanM.Gloss.UI.ManualPanel();
        }

        public override PLCStatusPanel CreatePLCStatusPanel()
        {
            return new PLCStatusPanel() { Dock = System.Windows.Forms.DockStyle.Fill, IsGlossOnly = true };
        }
    }
}
