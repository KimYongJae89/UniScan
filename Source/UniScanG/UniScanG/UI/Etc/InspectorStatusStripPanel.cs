using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;

namespace UniScanG.UI.Etc
{
    public partial class InspectorStatusStripPanel : UserControl, IStatusStripPanel, IMultiLanguageSupport
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr w, IntPtr l);

        InspectorObj inspector;

        public InspectorStatusStripPanel(InspectorObj inspector, bool canImPowerOn)
        {
            InitializeComponent();
            
            this.TabIndex = 0;

            this.inspector = inspector;

            this.startUpToolStripMenuItem.Visible = canImPowerOn;
            this.shutdownToolStripMenuItem.Visible = canImPowerOn;
            this.toolStripSeparator1.Visible = canImPowerOn;

            StringManager.AddListener(this);

            StateUpdate();
        }
        
        public void StateUpdate()
        {
            switch (inspector.CommState)
            {
                case CommState.CONNECTED:
                    this.labelConnect.BackColor = Colors.Connected;
                    break;
                case CommState.DISCONNECTED:
                    this.labelConnect.BackColor = Colors.Disconnected;
                    break;
                case CommState.OFFLINE:
                    this.labelConnect.BackColor = Colors.Offline;
                    break;
            }

            if(inspector.OpState == OpState.Inspect)
            {
                this.labelOpStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.InspectState.ToString());
                switch (inspector.InspectState)
                {
                    case InspectState.Run:
                        this.labelOpStatus.BackColor = Colors.Run;
                        break;
                    case InspectState.Wait:
                        this.labelOpStatus.BackColor = Colors.Wait;
                        break;
                }
            }
            else
            {
                // OpState Wait 문자열을 Ready 로 바꿈..
                string opStr = (inspector.OpState == OpState.Wait) ? "Ready" : inspector.OpState.ToString();
                this.labelOpStatus.Text = StringManager.GetString(this.GetType().FullName, opStr);
                switch (inspector.OpState)
                {
                    case OpState.Idle:
                        this.labelOpStatus.BackColor = Colors.Idle;
                        break;

                    case OpState.Wait:
                        this.labelOpStatus.BackColor = Colors.Wait;
                        break;

                    case OpState.Teach:
                        this.labelOpStatus.BackColor = Colors.Teach;
                        break;

                    case OpState.Alarm:
                        this.labelOpStatus.BackColor = Colors.Alarm;
                        break;
                }
            }


            //this.labelInspectStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.InspectState.ToString());
            //switch (inspector.InspectState)
            //{
            //    case InspectState.Run:
            //        this.labelInspectStatus.BackColor = Colors.Run;
            //        break;
            //    case InspectState.Done:
            //        this.labelInspectStatus.BackColor = Colors.Idle;
            //        break;
            //    case InspectState.Wait:
            //        this.labelInspectStatus.BackColor = Colors.Wait;
            //        break;
            //}

            float loadFactorP;
            lock (inspector.LoadFactorList)
                loadFactorP = inspector.LoadFactorList.LastOrDefault().Value * 100;
            //if (loadFactorP < 50)
            //    SendMessage(this.toolStripProgressBar1.Control.Handle, 1040, (IntPtr)1, IntPtr.Zero); // Green
            //else if (loadFactorP < 80)
            //    SendMessage(this.toolStripProgressBar1.Control.Handle, 1040, (IntPtr)3, IntPtr.Zero); // Yellow
            //else
            //    SendMessage(this.toolStripProgressBar1.Control.Handle, 1040, (IntPtr)2, IntPtr.Zero); // Red

            this.toolStripProgressBar1.Style = ProgressBarStyle.Blocks;
            this.toolStripProgressBar1.Value = (int)Math.Round(loadFactorP * 10);
            this.toolStripProgressBar1.ToolTipText = $"{loadFactorP:F01} %";
        }

        public void UpdateLanguage()
        {
            this.labelConnect.Text = StringManager.GetString(this.GetType().FullName, this.inspector.Info.GetName());

            this.startUpToolStripMenuItem.Text = StringManager.GetString(this.GetType().FullName, this.startUpToolStripMenuItem);
            this.shutdownToolStripMenuItem.Text = StringManager.GetString(this.GetType().FullName, this.shutdownToolStripMenuItem);
            this.restartToolStripMenuItem.Text = StringManager.GetString(this.GetType().FullName, this.restartToolStripMenuItem);
        }

        private void startUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() => SystemManager.Instance().DeviceController.Startup(this.inspector.Info.GetName()));
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() => SystemManager.Instance().DeviceController.Shutdown(this.inspector.Info.GetName(), false));
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 1 = green
            // 2 = red 
            // 3 = yellow
            //SendMessage(this.toolStripProgressBar1.Control.Handle, 1040, (IntPtr)i++, IntPtr.Zero);

            System.Threading.Tasks.Task.Run(() => SystemManager.Instance().DeviceController.Shutdown(this.inspector.Info.GetName(), true));
        }

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Tasks.Task.Run(() => SystemManager.Instance().DeviceController.InitializeInspectModule(null, this.inspector.Info.GetName()));
        }
    }
}