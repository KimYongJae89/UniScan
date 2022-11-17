using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;

namespace UniEye.Base.MachineInterface.UI
{
    public partial class MelsecMachineIfPanel : UserControl, IMachineIfPanel
    {
        MelsecMachineIfSetting machineIfSetting;
        public MelsecMachineIfSetting MachineIfSetting
        {
            get { return machineIfSetting; }
            set { machineIfSetting = value; }
        }

        public void SetDefault()
        {
        }

        public MelsecMachineIfPanel()
        {
            InitializeComponent();

            groupBoxTcpIp.Text = StringManager.GetString(this.GetType().FullName, groupBoxTcpIp.Text);
            labelIpAddress.Text = StringManager.GetString(this.GetType().FullName, labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName, labelPortNo.Text);

            groupBoxPlc.Text = StringManager.GetString(this.GetType().FullName, groupBoxPlc.Text);
            labelNetworkNo.Text = StringManager.GetString(this.GetType().FullName, labelNetworkNo.Text);
            labelPlcNo.Text = StringManager.GetString(this.GetType().FullName, labelPlcNo.Text);
            labelModuleIoNo.Text = StringManager.GetString(this.GetType().FullName, labelModuleIoNo.Text);
            labelModuleDeviceNo.Text = StringManager.GetString(this.GetType().FullName, labelModuleDeviceNo.Text);
            labelCpuInspectData.Text = StringManager.GetString(this.GetType().FullName, labelCpuInspectData.Text);
            chkAsciiType.Text = StringManager.GetString(this.GetType().FullName, chkAsciiType.Text);
        }

        public void Initialize()
        {
            Initialize(this.machineIfSetting);
        }

        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = (MelsecMachineIfSetting)machineIfSetting;
        }

        public bool Verify()
        {
            errorProvider1.Clear();

            try
            {
                // ip
                System.Predicate<string> ipPredicater = new System.Predicate<string>(f =>
                {
                    int i = Convert.ToInt32(f);
                    return 0 <= i && i <= 255;
                });

                string[] token = ipAddress.Text.Split('.');
                if ((token.Length == 4 && Array.TrueForAll(token, ipPredicater)) == false)
                    throw new Exception("Invalid IP Address");
            }
            catch (Exception e)
            {
                errorProvider1.SetError(ipAddress, e.Message);
            }

            // port
            try
            {
                int iPortNo = Convert.ToInt32(portNo.Text);
                if ((0 < iPortNo) == false)
                    throw new Exception("Invalid Port");
            }
            catch (Exception e)
            {
                errorProvider1.SetError(portNo, e.Message);
                return false;
            }

            TextBox[] array = new TextBox[] { networkNo, plcNo, moduleIoNo, moduleDeviceNo, cpuInspectData };
            int[] maxVal = new int[] { 0xff, 0xff, 0xffff, 0xff, 0xffff };
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    int ii = Convert.ToInt32(array[i].Text, 16);
                    if ((0 <= ii && ii <= maxVal[i]) == false)
                        throw new Exception("Invalid Value");
                }
                catch (Exception e)
                {
                    errorProvider1.SetError(array[i], e.Message);
                    return false;
                }
            };
            return true;
        }

        public void Apply()
        {
            MelsecMachineIfSetting melsecMachineIfSetting = (MelsecMachineIfSetting)machineIfSetting;

            melsecMachineIfSetting.TcpIpInfo = new DynMvp.Devices.Comm.TcpIpInfo(ipAddress.Text, int.Parse(portNo.Text));
            melsecMachineIfSetting.MelsecInfo = new MelsecInfo();
            melsecMachineIfSetting.MelsecInfo.NetworkNo = Convert.ToByte(this.networkNo.Text,16);
            melsecMachineIfSetting.MelsecInfo.PlcNo = Convert.ToByte(this.plcNo.Text,16);
            melsecMachineIfSetting.MelsecInfo.ModuleIoNo = Convert.ToInt16(this.moduleIoNo.Text,16);
            melsecMachineIfSetting.MelsecInfo.ModuleStationNo = Convert.ToByte(this.moduleDeviceNo.Text,16);
            melsecMachineIfSetting.MelsecInfo.WaitTime = Convert.ToInt16(this.cpuInspectData.Text,16);
            melsecMachineIfSetting.MelsecInfo.IsAsciiType = this.chkAsciiType.Checked;
        }

        private void MelsecConnectionInfoPanel_Load(object sender, EventArgs e)
        {
            MelsecMachineIfSetting melsecMachineIfSetting = (MelsecMachineIfSetting)machineIfSetting;
            if (melsecMachineIfSetting == null)
                return;

            ipAddress.Text = melsecMachineIfSetting.TcpIpInfo.IpAddress;
            portNo.Text = melsecMachineIfSetting.TcpIpInfo.PortNo.ToString();

            networkNo.Text = melsecMachineIfSetting.MelsecInfo.NetworkNo.ToString("X02");
            plcNo.Text = melsecMachineIfSetting.MelsecInfo.PlcNo.ToString("X02");
            moduleIoNo.Text = melsecMachineIfSetting.MelsecInfo.ModuleIoNo.ToString("X04");
            moduleDeviceNo.Text = melsecMachineIfSetting.MelsecInfo.ModuleStationNo.ToString("X02");
            cpuInspectData.Text = melsecMachineIfSetting.MelsecInfo.WaitTime.ToString("X04");
            chkAsciiType.Checked = melsecMachineIfSetting.MelsecInfo.IsAsciiType;
        }
    }
}
