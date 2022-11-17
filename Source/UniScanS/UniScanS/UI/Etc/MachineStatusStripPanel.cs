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
using DynMvp.Devices.Dio;
using DynMvp.Device.Serial;

namespace UniScanS.UI.Etc
{
    public partial class MachineStatusStripPanel : UserControl, IMultiLanguageSupport
    {
        IoPort ioRun;
        IoPort ioRolling;
        IoPort ioStop;
        IoPort ioAlarm;

        public MachineStatusStripPanel(IoPort ioRun, IoPort ioRolling, IoPort ioStop, IoPort ioAlarm)
        {
            this.ioRun = ioRun;
            this.ioRolling = ioRolling;
            this.ioStop = ioStop;
            this.ioAlarm = ioAlarm;

            InitializeComponent();

            this.TabIndex = 0;
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            labelRun.Text = StringManager.GetString(this.GetType().FullName, labelRun.Text);
        }

        bool onStop = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            bool isRun = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioRun);

            if (SystemManager.Instance().DeviceBox.DigitalIoHandler.Count == 0)
                isRun = true;

            labelRun.BackColor = isRun == true ? Colors.Run : Colors.Idle;
            labelRolling.BackColor = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioRolling) == true ? Colors.Run : Colors.Idle;
            labelStop.BackColor = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(ioStop) == true ? Colors.Alarm : Colors.Idle;
            labelAlarm.BackColor = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadOutput(ioAlarm) == true ? Colors.NG : Colors.Idle;

            if (SystemState.Instance().OpState == OpState.Wait || SystemState.Instance().OpState == OpState.Inspect)
            {
                if (isRun == true && onStop == true)
                {
                    onStop = false;
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_EXIT_PAUSE);
                }
                else if (isRun == false && onStop == false)
                {
                    onStop = true;
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_ENTER_PAUSE);
                }
            }
            else
            {
                if (onStop == true)
                    onStop = false;
            }
        }
    }
}