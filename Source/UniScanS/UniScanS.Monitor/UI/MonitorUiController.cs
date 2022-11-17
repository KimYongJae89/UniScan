using UniScanS.Common;
using UniScanS.Common.Exchange;
using UniScanS.Common.UI;

namespace UniScanS.Monitor.UI
{
    public class MonitorUiController : UniScanS.Common.UI.UiController
    {
        public override void UpdateTab(bool trained)
        {
            //SystemManager.Instance().ExchangeOperator.SendCommand(ETab.UPDATE, trained.ToString());

            base.UpdateTab(trained);
        }
        
        public override void TabChanged(string key)
        {
            //SystemManager.Instance().ExchangeOperator.SendCommand(ETab.CHANGE, key.ToString());
        }
    }
}
