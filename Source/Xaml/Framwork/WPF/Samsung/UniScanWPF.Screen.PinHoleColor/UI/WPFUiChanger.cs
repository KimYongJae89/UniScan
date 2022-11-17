using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    public class WPFUiChanger : UniScanWPF.UI.WPFUiChanger
    {
        public override UIElement GetMainPage()
        {
            return new TabControl();
        }

        public override UIElement GetStatusStrip()
        {
            StatusStrip statusStrip = new StatusStrip();

            BufferManager.Instance().SetBufferViewer(statusStrip);

            return statusStrip;
        }
    }
}
