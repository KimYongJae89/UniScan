using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.MotionController.UI
{
    public partial class AjinMotionInfoForm : Form
    {
        string newParamFile = "";
        AjinMotionInfo ajinMotionInfo;
        public AjinMotionInfo AjinMotionInfo
        {
            get { return ajinMotionInfo; }
            set { ajinMotionInfo = value;  }
        }

        public AjinMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelParamFile.Text = StringManager.GetString(this.GetType().FullName,labelParamFile.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void PciMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = ajinMotionInfo.Name;
            paramFile.Text = Path.GetFileName(ajinMotionInfo.ParamFile);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ajinMotionInfo.Name = name.Text;
            if(string.IsNullOrEmpty(newParamFile)==false)
                ajinMotionInfo.ParamFile = newParamFile;
        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string initPath = Path.GetFullPath(Environment.CurrentDirectory + @"\..\Config");
            dlg.InitialDirectory = initPath;
            dlg.RestoreDirectory = true;
            dlg.Filter = "mot files (*.mot)|*.mot|All files (*.*)|*.*";
            if(dlg.ShowDialog()== DialogResult.OK)
            {
                this.newParamFile = dlg.FileName;
                this.paramFile.Text = Path.GetFileName(this.newParamFile);
            }
        }
    }
}
