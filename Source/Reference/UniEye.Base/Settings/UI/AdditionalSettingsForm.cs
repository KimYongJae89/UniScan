using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.Settings.UI
{
    public partial class AdditionalSettingsForm : Form
    {
        public AdditionalSettingsForm()
        {
            InitializeComponent();
        }

        public void SetInstance(AdditionalSettings instance)
        {
            this.propertyGrid1.SelectedObject = instance;
        }
    }
}
