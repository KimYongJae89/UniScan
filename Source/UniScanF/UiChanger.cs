using UniScan.UI;
using UniEye.Base;
using UniEye.Base.UI;
using UniEye.Base.UI.ParamControl;

namespace UniScan
{
    public class UiChanger : UniEye.Base.UI.UiChanger
    {
        public override IMainForm CreateMainForm()
        {            
            return new MainForm();
        }
    }
}
