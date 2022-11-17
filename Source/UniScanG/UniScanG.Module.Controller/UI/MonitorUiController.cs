using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniScanG.Common.UI;

namespace UniScanG.Module.Controller.UI
{
    public class MonitorUiController : UniScanG.Common.UI.UiController
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
