using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.UI.Setting
{
    public delegate void UpdateDataDelegate();
    public interface ISettingSubPage
    {
        void Initialize();
        void UpdateData();
    }
}
