using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.Etc;
using UniScanG.Common.Util;
using DynMvp.Base;
using UniScanG.Gravure.Settings;

namespace UniScanG.Module.Controller.Device.Laser
{
    public partial class LaserStatusStripPanel : UserControl, IStatusStripPanel, IMultiLanguageSupport
    {
        HanbitLaser hanbitLaser = null;
        HanbitLaserControlForm form = null;
        public LaserStatusStripPanel(HanbitLaser hanbitLaser)
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.hanbitLaser = hanbitLaser;
            StateUpdate();
        }

        public void StateUpdate()
        {
            labelLaser.BackColor = this.hanbitLaser.IsConnected ? Colors.Connected : Colors.Disconnected;

            labelReady.Text = StringManager.GetString(this.GetType().FullName,this.hanbitLaser.IsReady ?  "Auto" : "Man");
            labelReady.BackColor =this.hanbitLaser.IsReady ? Colors.Run : Colors.Idle;

            Color stateColor = Colors.Idle;
            string stateText = "RDY";

            if (!hanbitLaser.HanbitLaserExtender.Use)
            {
                stateColor = Colors.Warn;
                stateText = "N/U";
            }
            else if (this.hanbitLaser.HanbitLaserExtender.IsStartRequest)
            {
                if (this.hanbitLaser.IsSetNotUse)
                {
                    stateColor = Colors.Warn;
                    stateText = "FRZ";
                }
                else if (this.hanbitLaser.HanbitLaserExtender.IsSetRun)
                {
                    stateColor = Colors.Run;
                    stateText = "RUN";
                }
            }

            if (this.hanbitLaser.IsError)
            {
                stateColor = Colors.Alarm;
                stateText = "ERR";

                if (this.hanbitLaser.IsOutofMeanderRange)
                {
                    stateText = "OMR";
                }
                else if (this.hanbitLaser.IsDecelMarkFault)
                {
                    stateText = "DMF";
                }
            }

            labelState.BackColor = stateColor;
            labelState.Text = StringManager.GetString(this.GetType().FullName, stateText);
            //labelState.ToolTipText = toolTipText;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void labelLaser_DoubleClick(object sender, EventArgs e)
        {
            if (this.form == null || this.form.IsDisposed)
                this.form = new HanbitLaserControlForm(this.hanbitLaser);

            if (this.form.Visible)
                form.Focus();
            else
                form.Show();
        }
    }
}
