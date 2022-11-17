using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.UI.TeachPage.Inspector
{
    public delegate void OnDefectTypeSelectChangedDelegate(string newType);
    public interface IImageControllerDefectTypeControl
    {
        OnDefectTypeSelectChangedDelegate OnDefectTypeSelectChanged { get; set; }
    }
}
