using DynMvp.Base;
using DynMvp.Devices.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class IOSettingForm : Form
    {
        private bool isLoading = false;

        public IOSettingForm()
        {
            InitializeComponent();

            //StringManager.AddListener(this);
        }

        private void IOSettingForm_Load(object sender, EventArgs e)
        {
            numericMCOn.Value = (decimal)GlossSettings.Instance().OutMCPort;
        }

        public void UpdateLanguage()
        {
            //StringManager.UpdateString(this);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            GlossSettings.Instance().Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //GlossSettings.Instance().Load();
            this.Close();
        }

        private void valueChanged(object sender, EventArgs e)
        {
            //    return;

            NumericUpDown control = sender as NumericUpDown;

            switch (Convert.ToInt32(control.Tag))
            {
                case 0: GlossSettings.Instance().OutMCPort = Convert.ToInt32(control.Value); break;
            }
        }

        private void checkBoxRevert_CheckedChanged(object sender, EventArgs e)
        {
            //SystemConfig.Instance().RevertTrigger = checkBoxRevert.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //IoPortViewer ioPortViewer = new IoPortViewer();

            //ioPortViewer.Show();
        }
    }
}
