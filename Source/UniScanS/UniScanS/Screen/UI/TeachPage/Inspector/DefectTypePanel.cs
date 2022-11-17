using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.TeachPage.Inspector;

namespace UniScanG.Screen.UI.TeachPage.Inspector
{
    public partial class DefectTypePanel : UserControl, IImageControllerDefectTypeControl
    {
        private OnDefectTypeSelectChangedDelegate onDefectTypeSelectChanged;

        public OnDefectTypeSelectChangedDelegate OnDefectTypeSelectChanged { get => onDefectTypeSelectChanged; set => onDefectTypeSelectChanged=value; }

        public DefectTypePanel()
        {
            InitializeComponent();
        }
    }
}
