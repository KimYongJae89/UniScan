using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;

namespace DynMvp.Devices.UI
{
    public partial class SerialLightConfigPanel : UserControl
    {
        public SerialLightConfigPanel()
        {
            InitializeComponent();

            buttonEditPort.Text = StringManager.GetString(this.GetType().FullName,buttonEditPort.Text);
        }
    }
}
