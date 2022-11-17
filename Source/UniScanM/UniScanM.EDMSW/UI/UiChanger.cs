using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.UI;
using UniScanM.EDMSW.UI;
using UniScanM.UI;
using UniEye.Base.Settings;

namespace UniScanM.EDMSW.UI
{
    internal class UiChanger : UniScanM.UI.UiChanger
    {
        public override IInspectionPanel CreateInspectionPanel(int index)
        {
            switch (index)
            {
                case 0:
                    return new InspectionPanelLeft_Length();
                    //return new InspectionPanelLeft();
                case 1:
                    return new InspectionPanelRight();
            }

            return null;
        }

        public override UniScanM.UI.ReportPageController CreateReportPageController()
        {
            return new UniScanM.EDMSW.UI.ReportPageController();
        }
        
        public override IReportPanel CreateReportPanel()
        {
            return new UI.ReportPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanM.EDMSW.UI.SettingPage();
        }

        public override ITeachPage CreateTeachPage()
        {
            return null;
        }
        public override IInspectionPage CreateInspectionPage()
        {
            EDMS_Type Edmstype = EDMS_Type.EDMS_Gravure;
            Enum.TryParse(OperationSettings.Instance().SystemType, out Edmstype);

            if (Edmstype == EDMS_Type.EDMS_Mobile)
                return (IInspectionPage)(new UniScanM.EDMSW.UI.InspectionPage());
            else
                return (IInspectionPage)(new UniScanM.UI.InspectionPage());
        }

    }

    public static class ControlOptimizer
    {

    }
}
