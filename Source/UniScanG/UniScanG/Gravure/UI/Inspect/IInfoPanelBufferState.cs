using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.UI.Inspect
{
    public interface IInfoPanelBufferState
    {
        void UpdateBufferState();
        void Clear();
        string GetLineSpdPv();
        string GetLineSpdSv();
        void TogCurLineSpdSpdType();
    }
}
