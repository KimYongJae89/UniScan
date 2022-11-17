using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;

namespace UniScanS.UI.Etc
{
    public partial class StatusStripPanel : UserControl, IMultiLanguageSupport
    {
        InspectorObj inspector;

        public StatusStripPanel(InspectorObj inspector)
        {
            InitializeComponent();
            
            this.TabIndex = 0;

            this.inspector = inspector;
            labelConnect.Text = string.Format("{0} {1}", "Cam", inspector.Info.CamIndex + 1);
            StringManager.AddListener(this);
        }
        
        public void StateUpdate()
        {
            labelOpStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.OpState.ToString());
            labelInspectStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.InspectState.ToString());

            switch (inspector.CommState)
            {
                case CommState.CONNECTED:
                    labelConnect.BackColor = Colors.Connected;
                    break;
                case CommState.DISCONNECTED:
                    labelConnect.BackColor = Colors.Disconnected;
                    break;
            }

            switch (inspector.InspectState)
            {
                case InspectState.Run:
                    labelInspectStatus.BackColor = Colors.Run;
                    break;
                case InspectState.Done:
                    labelInspectStatus.BackColor = Colors.Idle;
                    break;
                case InspectState.Wait:
                    labelInspectStatus.BackColor = Colors.Wait;
                    break;
                case InspectState.Pause:
                    labelInspectStatus.BackColor = Color.Yellow;
                    break;
            }

            switch (inspector.OpState)
            {
                case OpState.Idle:
                    labelOpStatus.BackColor = Colors.Idle;
                    break;
                case OpState.Inspect:
                    labelOpStatus.BackColor = Colors.Wait;
                    break;
                case OpState.Teach:
                    labelOpStatus.BackColor = Colors.Teach;
                    break;
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            labelConnect.Text = StringManager.GetString(this.GetType().FullName, labelConnect.Text);
        }
    }
}