using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.UI.Inspect
{
    public interface IRepeatedDefectAlarmForm
    {
        void Clear();
        void Show();
        void Silent();
        bool IsAlarmState { get; }
    }
}
