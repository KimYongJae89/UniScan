using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings.UI;
using UniEye.Base.Settings;
using UniScanG.Common.Data;

namespace UniScanG.Module.Monitor.Config
{
    public partial class CustomConfigPage : UserControl, ICustomConfigPage
    {
        public CustomConfigPage()
        {
            InitializeComponent();
        }

        public bool SaveData()
        {
            return true;
        }

        public void UpdateData()
        {
        }
    }
}
