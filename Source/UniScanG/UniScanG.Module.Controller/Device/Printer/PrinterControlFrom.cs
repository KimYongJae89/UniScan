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
using UniEye.Base.MachineInterface;
using UniScanG.MachineIF;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor;
//using UniScanG.MachineIF;

namespace UniScanG.Module.Controller.Device.Printer
{
    public partial class PrinterControlFrom : Form, IUserHandlerListener
    {
        bool isVirtual;
        MachineIF.MachineIfData machineIfData;
        Timer timer;

        public PrinterControlFrom(bool isVirtual, MachineIF.MachineIfData machineIfData)
        {
            InitializeComponent();

            this.isVirtual = isVirtual;
            this.machineIfData = machineIfData;

            UserHandler.Instance().AddListener(this);

            this.timer = new Timer();
        }


        public void UserChanged()
        {
            UpdateControl();
        }

        private void UpdateControl()
        {
            bool isMasterAccount = UserHandler.Instance().CurrentUser.IsMasterAccount;

            CONNECTION_ON.Enabled = CONNECTION_OFF.Enabled = this.isVirtual;

            // Printer->CM
            GET_START_STILLIMAGE_ON.Enabled = GET_START_STILLIMAGE_OFF.Enabled
                = GET_START_COLORSENSOR_ON.Enabled = GET_START_COLORSENSOR_OFF.Enabled
                = GET_START_EDMS_ON.Enabled = GET_START_EDMS_OFF.Enabled
                = GET_START_PINHOLE_ON.Enabled = GET_START_PINHOLE_OFF.Enabled
                = GET_START_RVMS_ON.Enabled = GET_START_RVMS_OFF.Enabled
                = GET_START_GRAVURE_INSPECTOR_ON.Enabled = GET_START_GRAVURE_INSPECTOR_OFF.Enabled
                = GET_START_GRAVURE_ERASER_ON.Enabled = GET_START_GRAVURE_ERASER_OFF.Enabled
                = GET_REWINDER_CUT_ON.Enabled = GET_REWINDER_CUT_OFF.Enabled
                = GET_TARGET_SPEED_VALUE.Enabled
                = GET_PRESENT_SPEED_VALUE.Enabled
                = GET_PRESENT_POSITION_VALUE.Enabled
                = GET_ROLL_DIAMETER_VALUE.Enabled
                = GET_LOT_VALUE.Enabled
                = GET_MODEL_VALUE.Enabled
                = GET_WORKER_VALUE.Enabled
                = GET_PASTE_VALUE.Enabled
                = this.isVirtual && isMasterAccount;

            // CM -> Printer
            SET_VISION_GRAVURE_INSP_READY_ON.Enabled = SET_VISION_GRAVURE_INSP_READY_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_RUNNING_ON.Enabled = SET_VISION_GRAVURE_INSP_RUNNING_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_RESULT_ON.Enabled = SET_VISION_GRAVURE_INSP_RESULT_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_REPDEF_P_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_REPDEF_P_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_REPDEF_N_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_REPDEF_N_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_NORDEF_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_NORDEF_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_SHTLEN_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_SHTLEN_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_DEFCNT_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_DEFCNT_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_MARGIN_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_MARGIN_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_STRIPE_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_STRIPE_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_NG_CRITICAL_ON.Enabled = SET_VISION_GRAVURE_INSP_NG_CRITICAL_OFF.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_ALL_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_NG_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_PINHOLE_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_COATING_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_CNT_NOPRINT_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_VALUE.Enabled
                = SET_VISION_GRAVURE_INSP_INFO_SHTLEN_VALUE.Enabled
                = isMasterAccount;

            // Printer -> Laser
            GET_FORCE_GRAVURE_ERASER_ON.Enabled = GET_FORCE_GRAVURE_ERASER_OFF.Enabled
                = this.isVirtual && isMasterAccount;

            // Laser -> Printer
            SET_VISION_GRAVURE_ERASER_READY_ON.Enabled = SET_VISION_GRAVURE_ERASER_READY_OFF.Enabled
                = SET_VISION_GRAVURE_ERASER_RUNNING_ON.Enabled = SET_VISION_GRAVURE_ERASER_RUNNING_OFF.Enabled
                = SET_VISION_GRAVURE_ERASER_CNT_ERASE_VALUE.Enabled
                = isMasterAccount;

            // VirtualMaxLotLength
            this.virtualMaxLength.Enabled = isMasterAccount;
        }

        public void UpdateData()
        {
            // Connection
            this.CONNECTION_ON.BackColor = (machineIfData.IsConnected ? Color.LightGreen : SystemColors.Control);
            this.CONNECTION_OFF.BackColor = (!machineIfData.IsConnected ? Color.OrangeRed : SystemColors.Control);

            // GET_DATA
            this.GET_START_STILLIMAGE_ON.BackColor = (machineIfData.GET_START_STILLIMAGE ? Color.LightGreen : SystemColors.Control);
            this.GET_START_STILLIMAGE_OFF.BackColor = (!machineIfData.GET_START_STILLIMAGE ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_COLORSENSOR_ON.BackColor = (machineIfData.GET_START_COLORSENSOR ? Color.LightGreen : SystemColors.Control);
            this.GET_START_COLORSENSOR_OFF.BackColor = (!machineIfData.GET_START_COLORSENSOR ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_EDMS_ON.BackColor = (machineIfData.GET_START_EDMS ? Color.LightGreen : SystemColors.Control);
            this.GET_START_EDMS_OFF.BackColor = (!machineIfData.GET_START_EDMS ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_PINHOLE_ON.BackColor = (machineIfData.GET_START_PINHOLE ? Color.LightGreen : SystemColors.Control);
            this.GET_START_PINHOLE_OFF.BackColor = (!machineIfData.GET_START_PINHOLE ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_RVMS_ON.BackColor = (machineIfData.GET_START_RVMS ? Color.LightGreen : SystemColors.Control);
            this.GET_START_RVMS_OFF.BackColor = (!machineIfData.GET_START_RVMS ? Color.OrangeRed : SystemColors.Control);

            if (!this.GET_TARGET_SPEED_VALUE.Focused)
                SetNumericUpDownValue(this.GET_TARGET_SPEED_VALUE, (decimal)machineIfData.GET_TARGET_SPEED_REAL);
            if (!this.GET_PRESENT_SPEED_VALUE.Focused)
                SetNumericUpDownValue(this.GET_PRESENT_SPEED_VALUE, (decimal)machineIfData.GET_PRESENT_SPEED_REAL);
            if (!this.GET_PRESENT_POSITION_VALUE.Focused)
                SetNumericUpDownValue(this.GET_PRESENT_POSITION_VALUE, (decimal)machineIfData.GET_PRESENT_POSITION);
            if (!this.GET_LOT_VALUE.Focused)
                this.GET_LOT_VALUE.Text = machineIfData.GET_LOT?.ToString();
            if (!this.GET_MODEL_VALUE.Focused)
                this.GET_MODEL_VALUE.Text = machineIfData.GET_MODEL?.ToString();
            if (!this.GET_WORKER_VALUE.Focused)
                this.GET_WORKER_VALUE.Text = machineIfData.GET_WORKER?.ToString();
            if (!this.GET_PASTE_VALUE.Focused)
                this.GET_PASTE_VALUE.Text = machineIfData.GET_PASTE?.ToString();
            if (!this.GET_ROLL_DIAMETER_VALUE.Focused)
                SetNumericUpDownValue(this.GET_ROLL_DIAMETER_VALUE, (decimal)machineIfData.GET_ROLL_DIAMETER_REAL);

            this.GET_REWINDER_CUT_ON.BackColor = (machineIfData.GET_REWINDER_CUT ? Color.LightGreen : SystemColors.Control);
            this.GET_REWINDER_CUT_OFF.BackColor = (!machineIfData.GET_REWINDER_CUT ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_GRAVURE_INSPECTOR_ON.BackColor = (machineIfData.GET_START_GRAVURE_INSPECTOR ? Color.LightGreen : SystemColors.Control);
            this.GET_START_GRAVURE_INSPECTOR_OFF.BackColor = (!machineIfData.GET_START_GRAVURE_INSPECTOR ? Color.OrangeRed : SystemColors.Control);
            this.GET_START_GRAVURE_ERASER_ON.BackColor = (machineIfData.GET_START_GRAVURE_ERASER ? Color.LightGreen : SystemColors.Control);
            this.GET_START_GRAVURE_ERASER_OFF.BackColor = (!machineIfData.GET_START_GRAVURE_ERASER ? Color.OrangeRed : SystemColors.Control);
            this.GET_FORCE_GRAVURE_ERASER_ON.BackColor = (machineIfData.GET_FORCE_GRAVURE_ERASER ? Color.LightGreen : SystemColors.Control);
            this.GET_FORCE_GRAVURE_ERASER_OFF.BackColor = (!machineIfData.GET_FORCE_GRAVURE_ERASER ? Color.OrangeRed : SystemColors.Control);

            // SET_DATA
            this.SET_VISION_GRAVURE_INSP_READY_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_READY ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_READY_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_READY ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_RUNNING_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_RUNNING ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_RUNNING_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_RUNNING ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_RESULT_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_RESULT ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_RESULT_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_RESULT ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_P_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_P_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_N_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_N_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_B_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_B_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_REPDEF_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_NORDEF_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_NORDEF_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_SHTLEN_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_SHTLEN_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN ? Color.OrangeRed : SystemColors.Control);

            this.SET_VISION_GRAVURE_INSP_NG_DEFCNT_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_DEFCNT_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_MARGIN_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_MARGIN_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_STRIPE_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_STRIPE_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_CRITICAL_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_INSP_NG_CRITICAL_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL ? Color.OrangeRed : SystemColors.Control);

            if (!this.SET_VISION_GRAVURE_INSP_CNT_ALL_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_ALL_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_ALL);
            if (!this.SET_VISION_GRAVURE_INSP_CNT_NG_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_NG_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_NG);
            if (!this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_PINHOLE);
            if (!this.SET_VISION_GRAVURE_INSP_CNT_COATING_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_COATING_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_COATING);
            if (!this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK);
            if (!this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_CNT_NOPRINT);

            if (!this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_REAL);
            if (!this.SET_VISION_GRAVURE_INSP_INFO_SHTLEN_VALUE.Focused)
                SetNumericUpDownValue(this.SET_VISION_GRAVURE_INSP_INFO_SHTLEN_VALUE, (decimal)machineIfData.SET_VISION_GRAVURE_INSP_INFO_SHTLEN_REAL);

            this.SET_VISION_GRAVURE_ERASER_READY_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_ERASER_READY ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_ERASER_READY_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_ERASER_READY ? Color.OrangeRed : SystemColors.Control);
            this.SET_VISION_GRAVURE_ERASER_RUNNING_ON.BackColor = (machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING ? Color.LightGreen : SystemColors.Control);
            this.SET_VISION_GRAVURE_ERASER_RUNNING_OFF.BackColor = (!machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING ? Color.OrangeRed : SystemColors.Control);

            if (!this.SET_VISION_GRAVURE_ERASER_CNT_ERASE_VALUE.Focused)
                this.SET_VISION_GRAVURE_ERASER_CNT_ERASE_VALUE.Text = machineIfData.SET_VISION_GRAVURE_ERASER_CNT_ERASE.ToString();

            // VirtualMaxLotLength
            this.virtualMaxLength.Value = (decimal)SystemManager.Instance().DeviceControllerG.MachineIfMonitor.VirtualMaxLotLength;
        }

        private bool SetNumericUpDownValue(NumericUpDown numericUpDown, decimal value)
        {
            decimal validValue = Math.Min(Math.Max(value, numericUpDown.Minimum), numericUpDown.Maximum);
            numericUpDown.Value = validValue;
            return value == validValue;
        }

        private void PrinterControlFrom_Load(object sender, EventArgs e)
        {
            SetToolTip();
            UpdateControl();

            if (MonitorSystemSettings.Instance().UseLaserBurner != LaserMode.Virtual)
            {
                groupPrinter2Laser.Visible = false;
                groupLaser2Printer.Visible = false;
            }

            // VirtualMaxLotLength
            this.virtualMaxLength.Visible = this.isVirtual;

            this.timer.Tick += new EventHandler((f, g) =>
            {
                UpdateData();
            });
            this.timer.Interval = 500;
            this.timer.Start();
        }

        private void SetToolTip()
        {
            ToolTip toolTip = new ToolTip();
            this.toolTip.IsBalloon = true;
            this.toolTip.ShowAlways = true;

            SetToolTip(toolTip, this.GET_START_STILLIMAGE, GetAddress(MelsecProtocolCommon.GET_START_STILLIMAGE));

            SetToolTip(toolTip, this.GET_START_COLORSENSOR, GetAddress(MelsecProtocolCommon.GET_START_COLORSENSOR));
            SetToolTip(toolTip, this.GET_START_EDMS, GetAddress(MelsecProtocolCommon.GET_START_EDMS));
            SetToolTip(toolTip, this.GET_START_PINHOLE, GetAddress(MelsecProtocolCommon.GET_START_PINHOLE));
            SetToolTip(toolTip, this.GET_START_RVMS, GetAddress(MelsecProtocolCommon.GET_START_RVMS));
            SetToolTip(toolTip, this.GET_START_GRAVURE_INSPECTOR, GetAddress(MelsecProtocolCommon.GET_START_GRAVURE_INSPECTOR));
            SetToolTip(toolTip, this.GET_START_GRAVURE_ERASER, GetAddress(MelsecProtocolCommon.GET_START_GRAVURE_ERASER));
            SetToolTip(toolTip, this.GET_REWINDER_CUT, GetAddress(MelsecProtocolCommon.GET_REWINDER_CUT));

            SetToolTip(toolTip, this.GET_TARGET_SPEED, GetAddress(MelsecProtocolCommon.GET_TARGET_SPEED));
            SetToolTip(toolTip, this.GET_PRESENT_SPEED, GetAddress(MelsecProtocolCommon.GET_PRESENT_SPEED));
            SetToolTip(toolTip, this.GET_PRESENT_POSITION, GetAddress(MelsecProtocolCommon.GET_PRESENT_POSITION));
            SetToolTip(toolTip, this.GET_LOT, GetAddress(MelsecProtocolCommon.GET_LOT));
            SetToolTip(toolTip, this.GET_MODEL, GetAddress(MelsecProtocolCommon.GET_MODEL));
            SetToolTip(toolTip, this.GET_WORKER, GetAddress(MelsecProtocolCommon.GET_WORKER));
            SetToolTip(toolTip, this.GET_PASTE, GetAddress(MelsecProtocolCommon.GET_PASTE));
            SetToolTip(toolTip, this.GET_ROLL_DIAMETER, GetAddress(MelsecProtocolCommon.GET_ROLL_DIAMETER));

            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_READY, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_READY));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_RUNNING, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_RUNNING));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_RESULT, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_RESULT));                                                                           

            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_REPDEF_P, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_P));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_REPDEF_N, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_N));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_REPDEF_B, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF_B));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_REPDEF, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_REPDEF));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_NORDEF, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_NORDEF));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_SHTLEN, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_SHTLEN));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_DEFCNT, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_DEFCNT));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_MARGIN, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_MARGIN));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_STRIPE, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_STRIPE));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_NG_CRITICAL, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_NG_CRITICAL));

            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_ALL, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_ALL));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_NG, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NG));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_PINHOLE, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_PINHOLE));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_COATING, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_COATING));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_CNT_NOPRINT, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_CNT_NOPRINT));
                                     
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_INSP_INFO_SHTLEN, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_INSP_INFO_SHTLEN));
                                    
            SetToolTip(toolTip, this.GET_FORCE_GRAVURE_ERASER, GetAddress(MelsecProtocol.GET_FORCE_GRAVURE_ERASER));
                                 
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_ERASER_READY, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_ERASER_READY));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_ERASER_RUNNING, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_ERASER_RUNNING));
            SetToolTip(toolTip, this.SET_VISION_GRAVURE_ERASER_CNT_ERASE, GetAddress(MelsecProtocol.SET_VISION_GRAVURE_ERASER_CNT_ERASE));

            this.ActiveControl = this.CONNECTION_ON;
            this.CONNECTION_ON.Focus();
        }

        private void SetToolTip(ToolTip toolTip, Label label, string caption)
        {
            this.toolTip.SetToolTip(label, caption);

            if (caption == "[N/U]")
                label.Parent.Visible = false;
        }

        private string GetAddress(Enum @enum)
        {
            MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
            MachineIfProtocol protocol = list.GetProtocol(@enum);

            if (!protocol.Use)
                return "[N/U]";

            if (protocol is MelsecMachineIfProtocol)
            {
                MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)protocol;

                if (string.IsNullOrEmpty(melsecMachineIfProtocol.Address))
                    return "[EMPTY]";

                return string.Format("[{0}]", melsecMachineIfProtocol.Address);
            }
            else if (protocol is AllenBreadleyMachineIfProtocol)
            {
                AllenBreadleyMachineIfProtocol allenBreadleyMachineIfProtocol = (AllenBreadleyMachineIfProtocol)protocol;

                if (string.IsNullOrEmpty(allenBreadleyMachineIfProtocol.TagName))
                    return "[EMPTY]";


                return $"[{allenBreadleyMachineIfProtocol.TagName}.{allenBreadleyMachineIfProtocol.OffsetByte4}]";
            }
            return "";
        }

        private void Button_On_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            if (button.Name == CONNECTION_ON.Name)
                ((IVirtualMachineIf)SystemManager.Instance().DeviceBox.MachineIf).SetStateConnect(true);
                //this.machineIfData.IsConnected = true;

            else if (button.Name == GET_START_STILLIMAGE_ON.Name)
                this.machineIfData.GET_START_STILLIMAGE = true;
            else if (button.Name == GET_START_COLORSENSOR_ON.Name)
                this.machineIfData.GET_START_COLORSENSOR = true;
            else if (button.Name == GET_START_EDMS_ON.Name)
                this.machineIfData.GET_START_EDMS = true;
            else if (button.Name == GET_START_PINHOLE_ON.Name)
                this.machineIfData.GET_START_PINHOLE = true;
            else if (button.Name == GET_START_RVMS_ON.Name)
                this.machineIfData.GET_START_RVMS = true;
            else if (button.Name == GET_START_GRAVURE_INSPECTOR_ON.Name)
                this.machineIfData.GET_START_GRAVURE_INSPECTOR = true;
            else if (button.Name == GET_START_GRAVURE_ERASER_ON.Name)
                this.machineIfData.GET_START_GRAVURE_ERASER = true;
            else if (button.Name == GET_REWINDER_CUT_ON.Name)
                this.machineIfData.GET_REWINDER_CUT = true;

            else if (button.Name == SET_VISION_GRAVURE_INSP_READY_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_READY = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_RUNNING_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_RUNNING = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_RESULT_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_RESULT = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_P_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_N_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_B_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_NORDEF_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_SHTLEN_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_DEFCNT_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_MARGIN_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_STRIPE_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE = true;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_CRITICAL_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL = true;

            else if (button.Name == GET_FORCE_GRAVURE_ERASER_ON.Name)
                this.machineIfData.GET_FORCE_GRAVURE_ERASER = true;

            else if (button.Name == SET_VISION_GRAVURE_ERASER_READY_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_ERASER_READY = true;
            else if (button.Name == SET_VISION_GRAVURE_ERASER_RUNNING_ON.Name)
                this.machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING = true;

            UpdateData();
        }

        private void Button_Off_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == null)
                return;

            if (button.Name == CONNECTION_OFF.Name)
                ((IVirtualMachineIf)SystemManager.Instance().DeviceBox.MachineIf).SetStateConnect(false);
            //this.machineIfData.IsConnected = false;

            else if (button.Name == GET_START_STILLIMAGE_OFF.Name)
                this.machineIfData.GET_START_STILLIMAGE = false;
            else if (button.Name == GET_START_COLORSENSOR_OFF.Name)
                this.machineIfData.GET_START_COLORSENSOR = false;
            else if (button.Name == GET_START_EDMS_OFF.Name)
                this.machineIfData.GET_START_EDMS = false;
            else if (button.Name == GET_START_PINHOLE_OFF.Name)
                this.machineIfData.GET_START_PINHOLE = false;
            else if (button.Name == GET_START_RVMS_OFF.Name)
                this.machineIfData.GET_START_RVMS = false;
            else if (button.Name == GET_START_GRAVURE_INSPECTOR_OFF.Name)
                this.machineIfData.GET_START_GRAVURE_INSPECTOR = false;
            else if (button.Name == GET_START_GRAVURE_ERASER_OFF.Name)
                this.machineIfData.GET_START_GRAVURE_ERASER = false;
            else if (button.Name == GET_REWINDER_CUT_OFF.Name)
                this.machineIfData.GET_REWINDER_CUT = false;

            else if (button.Name == SET_VISION_GRAVURE_INSP_READY_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_READY = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_RUNNING_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_RUNNING = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_RESULT_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_RESULT = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_P_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_P = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_N_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_N = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_B_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF_B = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_REPDEF_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_REPDEF = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_NORDEF_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_SHTLEN_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_DEFCNT_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_MARGIN_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_STRIPE_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE = false;
            else if (button.Name == SET_VISION_GRAVURE_INSP_NG_CRITICAL_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL = false;

            else if (button.Name == GET_FORCE_GRAVURE_ERASER_OFF.Name)
                this.machineIfData.GET_FORCE_GRAVURE_ERASER = false;

            else if (button.Name == SET_VISION_GRAVURE_ERASER_READY_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_ERASER_READY = false;
            else if (button.Name == SET_VISION_GRAVURE_ERASER_RUNNING_OFF.Name)
                this.machineIfData.SET_VISION_GRAVURE_ERASER_RUNNING = false;

            UpdateData();
        }

        private void Value_TextChanged(object sender, EventArgs e)
        {
            bool updateData = true;
            if (sender is TextBox)
            {
                TextBox textBox = sender as TextBox;

                string text = textBox.Text.ToUpper();
                if (textBox.Name == GET_LOT_VALUE.Name)
                    this.machineIfData.GET_LOT = text;
                else if (textBox.Name == GET_MODEL_VALUE.Name)
                    this.machineIfData.GET_MODEL = text;
                else if (textBox.Name == GET_WORKER_VALUE.Name)
                    this.machineIfData.GET_WORKER = text;
                else if (textBox.Name == GET_PASTE_VALUE.Name)
                    this.machineIfData.GET_PASTE = text;
            }
            else if (sender is NumericUpDown)
            {
                NumericUpDown numericUpDown = sender as NumericUpDown;

                if (numericUpDown.Name == GET_TARGET_SPEED_VALUE.Name)
                    this.machineIfData.GET_TARGET_SPEED_REAL = (float)numericUpDown.Value;
                else if (numericUpDown.Name == GET_PRESENT_SPEED_VALUE.Name)
                    this.machineIfData.GET_PRESENT_SPEED_REAL = (float)numericUpDown.Value;
                else if (numericUpDown.Name == GET_PRESENT_POSITION_VALUE.Name)
                    this.machineIfData.GET_PRESENT_POSITION = (float)numericUpDown.Value;
                else if (numericUpDown.Name == GET_ROLL_DIAMETER_VALUE.Name)
                    this.machineIfData.GET_ROLL_DIAMETER_REAL = (float)numericUpDown.Value;
                
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_ALL_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_ALL = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_NG_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_NG = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_PINHOLE_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_PINHOLE = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_COATING_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_COATING = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_SHEETATTACK = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_CNT_NOPRINT_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_CNT_NOPRINT = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_ERASER_CNT_ERASE_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_ERASER_CNT_ERASE = (int)numericUpDown.Value;

                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_INFO_CHIPSHARE_REAL = (int)numericUpDown.Value;
                else if (numericUpDown.Name == SET_VISION_GRAVURE_INSP_INFO_SHTLEN_VALUE.Name)
                    this.machineIfData.SET_VISION_GRAVURE_INSP_INFO_SHTLEN_REAL = (float)numericUpDown.Value;                
            }

            if (updateData)
                UpdateData();
        }

        private void PrinterControlFrom_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.timer?.Stop();
            this.timer?.Dispose();
            UserHandler.Instance().RemoveListener(this);
        }

        private void virtualMaxLength_ValueChanged(object sender, EventArgs e)
        {
            SystemManager.Instance().DeviceControllerG.MachineIfMonitor.VirtualMaxLotLength = (float)virtualMaxLength.Value;
        }
    }
}
