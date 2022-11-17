using DynMvp.Data;
using DynMvp.Data.Forms;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.UI;

namespace UniScanWPF.UI
{
    public abstract class WPFUiChanger
    {
        public abstract UIElement GetMainPage();
        public abstract UIElement GetStatusStrip();
    }
}
