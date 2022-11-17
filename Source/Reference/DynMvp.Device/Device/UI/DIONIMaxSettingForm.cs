using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.UI
{
    public partial class DIONIMaxSettingForm : Form
    {
        string inputLineStr;
        public string InputLineStr
        {
            get { return inputLineStr; }
            set { inputLineStr = value; }
        }

        string outputLineStr;
        public string OutputLineStr
        {
            get { return outputLineStr; }
            set { outputLineStr = value; }
        }

        public DIONIMaxSettingForm()
        {
            InitializeComponent();

            groupBox1.Text = StringManager.GetString(this.GetType().FullName,groupBox1.Text);
            labelInputLines.Text = StringManager.GetString(this.GetType().FullName,labelInputLines.Text);
            label1.Text = StringManager.GetString(this.GetType().FullName,label1.Text);
            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);

            this.inputLines.Text = inputLineStr;
            this.outputLines.Text = outputLineStr;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            inputLineStr = inputLines.Text;
            outputLineStr = outputLines.Text;
        }
    }
}
