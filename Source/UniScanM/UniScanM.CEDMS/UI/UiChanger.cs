using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.UI;
using UniScanM.CEDMS.Settings;
using UniScanM.UI;

namespace UniScanM.CEDMS.UI
{
    internal class UiChanger : UniScanM.UI.UiChanger
    {
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
            return new UniScanM.CEDMS.UI.Chart.ReportPageController();
        }

        public override IReportPanel CreateReportPanel()
        {
            return new CEDMS.UI.ReportPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanM.CEDMS.UI.SettingPage((CEDMSSettings)CEDMSSettings.Instance());
        }

        public override ITeachPage CreateTeachPage()
        {
            return null;
        }
    }
}
