using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.UI;

namespace UniScanX.MPAlignment.UI
{
    public class UiChanger : UniEye.Base.UI.UiChanger
    {

        public override IMainForm CreateMainForm()
        {
            return new MainForm();
        }
    }
}
