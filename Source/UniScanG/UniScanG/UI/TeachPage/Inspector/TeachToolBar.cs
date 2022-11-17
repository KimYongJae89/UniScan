using DynMvp.Base;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.Screen.UI.Teach;

namespace UniScanG.UI.TeachPage.Inspector
{
    public partial class TeachToolBar : UserControl, IModellerControl, IMultiLanguageSupport
    {
        ModellerPageExtender modellerPageExtender;

        public TeachToolBar()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
        }

        public void SetModellerExtender(UniEye.Base.UI.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = (ModellerPageExtender)modellerPageExtender;
        }

        private void buttonGrab_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Grab();
        }
        
        private void buttonAutoTeach_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Teach();
        }

        private void buttonInspect_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Inspect();
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.SaveModel();
        }

        private void buttonLoadImage_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.LoadImage();
        }

        private void buttonExportData_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.ExportData();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            
            foreach (ToolStripItem item in toolStripMain.Items)
                item.Text = StringManager.GetString(this.GetType().FullName, item.Text);
        }
    }
}
