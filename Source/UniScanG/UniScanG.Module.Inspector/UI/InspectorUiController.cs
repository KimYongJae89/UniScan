using Infragistics.Win.UltraWinTabControl;
using System;
using UniScanG.Common;
using UniScanG.Common.UI;
using UniScanG.UI;

namespace UniScanG.Module.Inspector.UI
{
    public class InspectorUiController : Common.UI.UiController
    {
        public override void ChangeTab(string key)
        {
            MainTabKey e;
            bool ok = Enum.TryParse<MainTabKey>(key, out e);
            if(ok)
            {
                InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;
                switch (e)
                {
                    case MainTabKey.Inspect:
                        client.PreparePanel(Common.Exchange.ExchangeCommand.V_INSPECT);
                        break;
                    case MainTabKey.Model:
                        client.PreparePanel(Common.Exchange.ExchangeCommand.V_MODEL);
                        break;
                    case MainTabKey.Teach:
                        client.PreparePanel(Common.Exchange.ExchangeCommand.V_TEACH);
                        break;
                    case MainTabKey.Report:
                        client.PreparePanel(Common.Exchange.ExchangeCommand.V_REPORT);
                        break;
                    case MainTabKey.Setting:
                        client.PreparePanel(Common.Exchange.ExchangeCommand.V_SETTING);
                        break;
                }
            }
        }

    }
}
