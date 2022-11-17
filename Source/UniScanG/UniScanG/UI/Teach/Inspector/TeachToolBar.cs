using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Util;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;

namespace UniScanG.UI.Teach.Inspector
{
    public partial class TeachToolBar : UserControl, IModellerControl, IMultiLanguageSupport
    {
        ModellerPageExtender modellerPageExtender;

        public TeachToolBar()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = (ModellerPageExtender)modellerPageExtender;
        }

        public void UpdateData() { }

        private void buttonGrab_Click(object sender, System.EventArgs e)
        {
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn();

            try
            {
                CancellationTokenSource token = new CancellationTokenSource();
                modellerPageExtender.GrabSheet(1, token);

            }
            catch (Exception ex)
            {
                MessageForm.Show(null, string.Format("{0}{1}{2}", StringManager.GetString("Grab Fail."), Environment.NewLine, ex.Message));
            }
            finally
            {
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            }

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
            UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return;

            string path = Path.Combine(curModel.ModelPath, DateTime.Now.ToString("yy-MM-dd hh-mm-ss"));
            modellerPageExtender.DataExport(path);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            foreach (ToolStripItem item in toolStripMain.Items)
                item.Text = StringManager.GetString(this.GetType().FullName, item.Text);
        }
    }
}
