using DynMvp.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Settings;

namespace UniScanG.Module.Controller.Device.Laser
{
    public partial class HanbitLaserControlForm : Form, IUserHandlerListener
    {
        HanbitLaser hanbitLaser = null;
        Timer updateTimer = new Timer();

        public HanbitLaserControlForm(HanbitLaser hanbitLaser)
        {
            InitializeComponent();

            this.hanbitLaser = hanbitLaser;

            this.cmIgnoreTimeMs.Minimum = this.counterReq.Minimum = this.counterOver.Minimum = this.counterDone.Minimum = this.counterGood.Minimum = 0;
            this.cmIgnoreTimeMs.Maximum = this.counterReq.Maximum = this.counterOver.Maximum = this.counterDone.Maximum = this.counterGood.Maximum = int.MaxValue;

            UserHandler.Instance().AddListener(this);

            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Interval = 250;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser != null)
                this.cmState.Enabled = curUser.IsSuperAccount || !this.hanbitLaser.HanbitLaserExtender.IsSetRun;

            this.cmUseLocal.BackColor = this.hanbitLaser.HanbitLaserExtender.UseFromLocal? Color.LightGreen : Color.OrangeRed;

            this.cmIgnoreTimeMs.Value = this.hanbitLaser.HanbitLaserExtender.IgnoreTimeMs;
            this.cmAlive.BackColor = this.hanbitLaser.IsSetAlive ? Color.LightGreen : Control.DefaultBackColor;
            this.cmEmergency.BackColor = this.hanbitLaser.IsSetEmergency ? Color.OrangeRed : Control.DefaultBackColor;
            this.cmReset.BackColor = this.hanbitLaser.IsSetReset ? Color.LightGreen : Control.DefaultBackColor;
            this.cmMark.BackColor = this.hanbitLaser.IsSetNG ? Color.OrangeRed : Control.DefaultBackColor;
            this.cmLotClear.BackColor = this.hanbitLaser.IsSetLotClear ? Color.LightGreen : Control.DefaultBackColor;
            this.cmFreeze.BackColor = this.hanbitLaser.IsSetNotUse ? Color.Yellow : Control.DefaultBackColor;
            this.cmRun.BackColor = this.hanbitLaser.HanbitLaserExtender.IsSetRun ? Color.LightGreen : Control.DefaultBackColor;

            this.laserAlive.BackColor = this.hanbitLaser.IsConnected ? Color.LightGreen : Control.DefaultBackColor;
            this.laserReady.BackColor = this.hanbitLaser.IsReady ? Color.LightGreen : Control.DefaultBackColor;
            this.laserDone.BackColor = this.hanbitLaser.IsDone ? Color.LightGreen : Control.DefaultBackColor;
            this.laserLotClearDone.BackColor = this.hanbitLaser.IsLotClearDone ? Color.LightGreen : Control.DefaultBackColor;
            this.laserError.BackColor = this.hanbitLaser.IsError ? Color.OrangeRed : Control.DefaultBackColor;
            this.laserOutofMeanderRange.BackColor = this.hanbitLaser.IsOutofMeanderRange ? Color.OrangeRed : Control.DefaultBackColor;
            this.laserDecelMarkFault.BackColor = this.hanbitLaser.IsDecelMarkFault ? Color.OrangeRed : Control.DefaultBackColor;
            this.laserMarkGood.BackColor = this.hanbitLaser.IsMGood ? Color.LightGreen : Control.DefaultBackColor;

            this.printerUseRemote.BackColor = this.hanbitLaser.HanbitLaserExtender.UseFromRemote ? Color.LightGreen : Control.DefaultBackColor;
            this.printerAblationAll.BackColor = this.hanbitLaser.HanbitLaserExtender.NgPrinter ? Color.OrangeRed : Control.DefaultBackColor;

            this.visionNg00.BackColor = this.hanbitLaser.HanbitLaserExtender.IsSetVNG(0, 0) ? Color.OrangeRed : Control.DefaultBackColor;
            this.visionNg01.BackColor = this.hanbitLaser.HanbitLaserExtender.IsSetVNG(0, 1) ? Color.OrangeRed : Control.DefaultBackColor;
            this.visionNg10.BackColor = this.hanbitLaser.HanbitLaserExtender.IsSetVNG(1, 0) ? Color.OrangeRed : Control.DefaultBackColor;
            this.visionNg11.BackColor = this.hanbitLaser.HanbitLaserExtender.IsSetVNG(1, 1) ? Color.OrangeRed : Control.DefaultBackColor;


            DynMvp.UI.UiHelper.SetNumericValue(this.counterDone, this.hanbitLaser.HanbitLaserExtender.DoneCount);
            DynMvp.UI.UiHelper.SetNumericValue(this.counterGood, this.hanbitLaser.HanbitLaserExtender.GoodCount);
            DynMvp.UI.UiHelper.SetNumericValue(this.counterReq, this.hanbitLaser.HanbitLaserExtender.ReqCount);
            DynMvp.UI.UiHelper.SetNumericValue(this.counterOver, this.hanbitLaser.HanbitLaserExtender.OverCount);

            this.systemStartStop.Text = this.hanbitLaser.HanbitLaserExtender.IsStartRequest ? "Inspecting" : "Idle";
            this.systemStartStop.BackColor = this.hanbitLaser.HanbitLaserExtender.IsStartRequest ? Color.LightGreen : Control.DefaultBackColor;
        }

        private void Set_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name == cmUseLocal.Name)
                this.hanbitLaser.HanbitLaserExtender.UseFromLocal = !this.hanbitLaser.HanbitLaserExtender.UseFromLocal;
            else if (button.Name == counterClear.Name)
                this.hanbitLaser.HanbitLaserExtender.ClearDone();

            else if (button.Name == cmAlive.Name)
                this.hanbitLaser.SetAlive(!this.hanbitLaser.IsSetAlive);
            if (button.Name == cmEmergency.Name)
                this.hanbitLaser.SetEmergency(!this.hanbitLaser.IsSetEmergency);
            else if (button.Name == cmReset.Name)
                this.hanbitLaser.SetReset(!this.hanbitLaser.IsSetReset);
            else if (button.Name == cmMark.Name)
                this.hanbitLaser.SetMark(!this.hanbitLaser.IsSetNG);
            else if (button.Name == cmLotClear.Name)
                this.hanbitLaser.SetLotClear(!this.hanbitLaser.IsSetLotClear);
            else if (button.Name == cmFreeze.Name)
                this.hanbitLaser.SetNotUse(!this.hanbitLaser.IsSetNotUse);

            else if (button.Name == cmRun.Name)
                this.hanbitLaser.HanbitLaserExtender.SetRun(!this.hanbitLaser.HanbitLaserExtender.IsSetRun);
            else if (button.Name == visionNg00.Name)
                this.hanbitLaser.HanbitLaserExtender.SetVNG(0, 0, !this.hanbitLaser.HanbitLaserExtender.IsSetVNG(0, 0));
            else if (button.Name == visionNg01.Name)
                this.hanbitLaser.HanbitLaserExtender.SetVNG(0, 1, !this.hanbitLaser.HanbitLaserExtender.IsSetVNG(0, 1));
            else if (button.Name == visionNg10.Name)
                this.hanbitLaser.HanbitLaserExtender.SetVNG(1, 0, !this.hanbitLaser.HanbitLaserExtender.IsSetVNG(1, 0));
            else if (button.Name == visionNg11.Name)
                this.hanbitLaser.HanbitLaserExtender.SetVNG(1, 1, !this.hanbitLaser.HanbitLaserExtender.IsSetVNG(1, 1));

            else if (this.hanbitLaser is HanbitLaserVirtual)
            {
                HanbitLaserVirtual hanbitLaserVirtual = (HanbitLaserVirtual)this.hanbitLaser;

                if (button.Name == laserAlive.Name)
                    hanbitLaserVirtual.SetLaserAlive(!this.hanbitLaser.IsConnected);
                else if (button.Name == laserReady.Name)
                    hanbitLaserVirtual.SetLaserReady(!this.hanbitLaser.IsReady);
                else if (button.Name == laserDone.Name)
                    hanbitLaserVirtual.SetLaserDone(!this.hanbitLaser.IsDone);
                else if (button.Name == laserLotClearDone.Name)
                    hanbitLaserVirtual.SetLaserLotClearDode(!this.hanbitLaser.IsLotClearDone);
                else if (button.Name == laserError.Name)
                    hanbitLaserVirtual.SetLaserError(!this.hanbitLaser.IsError);
                else if (button.Name == laserOutofMeanderRange.Name)
                    hanbitLaserVirtual.SetLaserOutOfRange(!this.hanbitLaser.IsOutofMeanderRange);
                else if (button.Name == laserDecelMarkFault.Name)
                    hanbitLaserVirtual.SetLaserDecelMarkFault(!this.hanbitLaser.IsDecelMarkFault);
                else if (button.Name == laserMarkGood.Name)
                    hanbitLaserVirtual.SetLaserMarkGood(!this.hanbitLaser.IsMGood);
            }
        }

        private void HanbitLaserControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserHandler.Instance().RemoveListener(this);
        }

        private void doneCountContextMenuStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.hanbitLaser.HanbitLaserExtender.ClearDone();
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;

            bool isMasterAccount = curUser.IsSuperAccount;

            this.counter.Enabled = isMasterAccount;
            this.visionState.Enabled = isMasterAccount;

            this.laserState.Enabled = this.hanbitLaser.IsVirtual && isMasterAccount;
            this.printerState.Enabled = false;

            systemStartStop.Enabled = isMasterAccount;
            cmUseLocal.Enabled = true;
        }

        private void HanbitLaserControlForm_Load(object sender, EventArgs e)
        {

        }

        private void counter_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numericUpDown = (NumericUpDown)sender;

            if (numericUpDown.Name == this.cmIgnoreTimeMs.Name)
                this.hanbitLaser.HanbitLaserExtender.IgnoreTimeMs = (int)this.cmIgnoreTimeMs.Value;

            else if (numericUpDown.Name == this.counterDone.Name)
                DynMvp.UI.UiHelper.SetNumericValue(this.counterDone, this.hanbitLaser.HanbitLaserExtender.DoneCount);
            else if (numericUpDown.Name == this.counterGood.Name)
                DynMvp.UI.UiHelper.SetNumericValue(this.counterGood, this.hanbitLaser.HanbitLaserExtender.GoodCount);
            else if (numericUpDown.Name == this.counterReq.Name)
                DynMvp.UI.UiHelper.SetNumericValue(this.counterReq, this.hanbitLaser.HanbitLaserExtender.ReqCount);
            else if (numericUpDown.Name == this.counterOver.Name)
                DynMvp.UI.UiHelper.SetNumericValue(this.counterOver, this.hanbitLaser.HanbitLaserExtender.OverCount);

            if (this.hanbitLaser.HanbitLaserExtender.IsSetRun)
                this.hanbitLaser.HanbitLaserExtender.UpdateNgSignal();
        }

        private void systemStartStop_Click(object sender, EventArgs e)
        {
            if (this.hanbitLaser.HanbitLaserExtender.IsStartRequest)
                this.hanbitLaser.HanbitLaserExtender.Stop();
            else
                this.hanbitLaser.HanbitLaserExtender.Start();
        }
    }
}
