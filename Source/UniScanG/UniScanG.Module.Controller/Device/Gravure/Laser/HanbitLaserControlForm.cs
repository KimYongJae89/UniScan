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

namespace UniScanG.Module.Controller.Device.Gravure.Laser
{
    public partial class HanbitLaserControlForm : Form, IUserHandlerListener
    {
        HanbitLaser hanbitLaser = null;

        bool alive = false;
        bool ready = false;
        bool done = false;
        bool error = false;
        bool outofrange = false;

        Timer updateTimer = new Timer();

        public HanbitLaserControlForm(HanbitLaser hanbitLaser)
        {
            InitializeComponent();

            this.hanbitLaser = hanbitLaser;
            
            this.laserState.Enabled = this.hanbitLaser.IsVirtual;
            this.printerState.Enabled = false;
            UserHandler.Instance().AddListener(this);

            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Interval = 250;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.cmAlive.BackColor = this.hanbitLaser.IsSetAlive ? Color.LightGreen : Control.DefaultBackColor;
            this.cmEmergency.BackColor = this.hanbitLaser.IsSetEmergency ? Color.OrangeRed : Control.DefaultBackColor;
            this.cmReset.BackColor = this.hanbitLaser.IsSetReset ? Color.LightGreen : Control.DefaultBackColor;
            this.cmRun.BackColor = this.hanbitLaser.IsSetRun ? Color.LightGreen : Control.DefaultBackColor;
            this.cmNg.BackColor = this.hanbitLaser.IsSetNG ? Color.OrangeRed : Control.DefaultBackColor;
            this.cmNotUse.BackColor = this.hanbitLaser.IsSetNotUse ? Color.LightGreen : Control.DefaultBackColor;
            this.cmLotClear.BackColor = this.hanbitLaser.IsSetLotClear ? Color.LightGreen : Control.DefaultBackColor;
            this.cmUseLocal.BackColor = this.hanbitLaser.UseFromLocal? Color.LightGreen : Control.DefaultBackColor;

            this.laserAlive.BackColor = this.hanbitLaser.IsAlive ? Color.LightGreen : Control.DefaultBackColor;
            this.laserReady.BackColor = this.hanbitLaser.IsReady ? Color.LightGreen : Control.DefaultBackColor;
            this.laserDone.BackColor = this.hanbitLaser.IsDone ? Color.LightGreen : Control.DefaultBackColor;
            this.laserError.BackColor = this.hanbitLaser.IsError ? Color.OrangeRed : Control.DefaultBackColor;
            this.laserOutofrange.BackColor = this.hanbitLaser.IsOutOfRange ? Color.OrangeRed : Control.DefaultBackColor;
            this.laserLotClearDone.BackColor = this.hanbitLaser.IsLotClearDone ? Color.LightGreen : Control.DefaultBackColor;

            this.printerUseRemote.BackColor = this.hanbitLaser.UseFromRemote ? Color.LightGreen : Control.DefaultBackColor;
            this.printerEraseAll.BackColor = this.hanbitLaser.IsSetForceErase ? Color.OrangeRed : Control.DefaultBackColor;

            this.cmDoneCount.Text = this.hanbitLaser.DoneCount.ToString();
        }

        private void Set_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Name == cmResetDoneCount.Name)
                this.hanbitLaser.ResetDoneCount();
            else if (button.Name == cmAlive.Name)
                this.hanbitLaser.SetAlive(!this.hanbitLaser.IsSetAlive);
            if (button.Name == cmEmergency.Name)
                this.hanbitLaser.SetEmergency(!this.hanbitLaser.IsSetEmergency);
            else if (button.Name == cmReset.Name)
                this.hanbitLaser.SetReset(!this.hanbitLaser.IsSetReset);
            else if (button.Name == cmRun.Name)
                this.hanbitLaser.SetRun(!this.hanbitLaser.IsSetRun);
            else if (button.Name == cmNg.Name)
                this.hanbitLaser.SetNG(!this.hanbitLaser.IsSetNG);
            else if (button.Name == cmLotClear.Name)
                this.hanbitLaser.SetLotClear(!this.hanbitLaser.IsSetLotClear);
            else if (button.Name == cmNotUse.Name)
                this.hanbitLaser.SetNotUse(!this.hanbitLaser.IsSetNotUse);
            else if (button.Name == cmUseLocal.Name)
                this.hanbitLaser.UseFromLocal = !this.hanbitLaser.UseFromLocal;

            if (this.hanbitLaser is HanbitLaserVirtual)
            {
                HanbitLaserVirtual hanbitLaserVirtual = (HanbitLaserVirtual)this.hanbitLaser;
                
                if (button.Name == laserAlive.Name)
                    this.alive = hanbitLaserVirtual.SetLaserAlive(!this.hanbitLaser.IsAlive);

                else if (button.Name == laserReady.Name)
                    this.ready = hanbitLaserVirtual.SetLaserReady(!this.hanbitLaser.IsReady);

                else if (button.Name == laserDone.Name)
                    this.outofrange = hanbitLaserVirtual.SetLaserDone(!this.hanbitLaser.IsDone);

                else if (button.Name == laserError.Name)
                    this.error = hanbitLaserVirtual.SetLaserError(!this.hanbitLaser.IsError);

                else if (button.Name == laserOutofrange.Name)
                    this.outofrange = hanbitLaserVirtual.SetLaserOutOfRange(!this.hanbitLaser.IsOutOfRange);

                else if (button.Name == laserLotClearDone.Name)
                    this.outofrange = hanbitLaserVirtual.SetLaserLotClearDode(!this.hanbitLaser.IsLotClearDone);

            }
        }

        private void HanbitLaserControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void doneCountContextMenuStripMenuItemClear_Click(object sender, EventArgs e)
        {
            this.hanbitLaser.ResetDoneCount();
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser != null)
                this.cmState.Enabled = curUser.SuperAccount;
        }
    }
}
