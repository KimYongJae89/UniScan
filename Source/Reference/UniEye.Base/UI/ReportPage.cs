using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Authentication;

namespace UniEye.Base.UI
{
    public partial class ReportPage : UserControl, IMainTabPage, IReportPage
    {
     
        IReportPanel reportPanelItem = null;

        public ReportPage()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void EnableControls(UserType user)
        {

        }

        public void Initialize()
        {
            reportPanel.SuspendLayout();

            reportPanel.Controls.Clear();
            reportPanelItem = SystemManager.Instance().UiChanger.CreateReportPanel(); 

            ((Control)reportPanelItem).Dock = DockStyle.Fill;
            reportPanelItem.Initialize();
            reportPanel.Controls.Add((Control)reportPanelItem);

            reportPanel.ResumeLayout(false);
        }

        public void ModelChanged()
        {
            Initialize();
        }

        public void RefreshReportPage()
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
                reportPanelItem.Refresh();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        internal void ModelAutoSelector()
        {
            throw new NotImplementedException();
        }

        void IReportPage.ModelAutoSelector()
        {
            throw new NotImplementedException();
        }
    }
}
