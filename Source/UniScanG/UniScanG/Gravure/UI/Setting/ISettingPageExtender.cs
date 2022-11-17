using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanG.Gravure.UI.Setting
{
    public interface ISettingPageExtender
    {
        bool IsVisibleCollectLogButton { get; }

        ISettingSubPage CreateAlarmPage();
        ISettingSubPage CreateCommPage();
        ISettingSubPage CreateGradePage();
        ISettingSubPage CreateGeneralPage();

        void CollectLog(IWin32Window parent);
    }
}
