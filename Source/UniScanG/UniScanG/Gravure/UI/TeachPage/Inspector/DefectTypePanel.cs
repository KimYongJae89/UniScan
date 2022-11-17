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

namespace UniScanG.Grabure.UI.TeachPage.Inspector
{
    public partial class DefectTypePanel : UserControl, IImageControllerDefectTypeControl
    {
        OnDefectTypeSelectChangedDelegate onDefectTypeSelectChanged;

        OnDefectTypeSelectChangedDelegate IImageControllerDefectTypeControl.OnDefectTypeSelectChanged
        {
            get { return onDefectTypeSelectChanged; }
            set { onDefectTypeSelectChanged = value; }
        }

        public DefectTypePanel()
        {
            InitializeComponent();
        }


        private void typeTotal_CheckedChanged(object sender, EventArgs e)
        {
            onDefectTypeSelectChanged(typeTotal.Text);
        }
    }
}
