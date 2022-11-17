using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.UI.Setting;

namespace UniScanG.Module.Inspector.UI.Settings
{
    class SettingPageExtender : ISettingPageExtender
    {
        public bool IsVisibleCollectLogButton => false;

        public ISettingSubPage CreateAlarmPage()
        {
            return null;
        }

        public ISettingSubPage CreateCommPage()
        {
            return new SettingCommPage()
            {
                ShowEncoderButton = false,
                ShowImsPowControlButton = false,
            };
        }

        public ISettingSubPage CreateGradePage()
        {
            return null;
        }

        public ISettingSubPage CreateGeneralPage()
        {
            return new SettingGeneralPage();
        }

        public void CollectLog(IWin32Window parent) { }
    }
}
