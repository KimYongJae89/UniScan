using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.UI;
using UniScanM.UI;

namespace UniScanM.Pinhole.UI
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
            return new Pinhole.UI.MenuPage.ReportPageController();
        }

        public override IReportPanel CreateReportPanel()
        {
            return new UI.MenuPage.SimpleReportDetailPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UI.MenuPanel.SettingPage();
        }

        public override ITeachPage CreateTeachPage()
        {
            return null;
        }
    }
}
