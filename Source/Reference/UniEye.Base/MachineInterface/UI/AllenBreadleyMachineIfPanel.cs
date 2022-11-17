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
    public partial class AllenBreadleyMachineIfPanel : UserControl, IMachineIfPanel
    {
        AllenBreadleyMachineIfSetting machineIfSetting;
        //public AllenBreadleyMachineIf MachineIfSetting
        //{
        //    get { return machineIfSetting; }
        //    set { machineIfSetting = value; }
        //}

        public void SetDefault()
        {
        }

        public AllenBreadleyMachineIfPanel()
        {
            InitializeComponent();

            //groupBoxTcpIp.Text = StringManager.GetString(this.GetType().FullName, groupBoxTcpIp.Text);
            //labelIpAddress.Text = StringManager.GetString(this.GetType().FullName, labelIpAddress.Text);
            //labelCpuType.Text = StringManager.GetString(this.GetType().FullName, labelCpuType.Text);
        }

        public void Initialize()
        {
            Initialize(this.machineIfSetting);
        }

        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = (AllenBreadleyMachineIfSetting)machineIfSetting;
        }

        public bool Verify()
        {
            errorProvider1.Clear();

            try
            {
                // ip
                System.Predicate<string> ipPredicater = new System.Predicate<string>(f =>
                {
                    int a = -1;
                    int.TryParse(f, out a);
                    return 0 <= a && a <= 255;
                });

                string[] token = ipAddress.Text.Split('.');
                if ((token.Length == 4 && Array.TrueForAll(token, ipPredicater)) == false)
                    throw new Exception("Invalid IP Address");
            }
            catch (Exception e)
            {
                errorProvider1.SetError(ipAddress, e.Message);
            }

            return true;
        }

        public void Apply()
        {
            machineIfSetting.TcpIpInfo = new DynMvp.Devices.Comm.TcpIpInfo(ipAddress.Text, 0);
            machineIfSetting.CPU_TYPE = this.cpuType.Text;
            machineIfSetting.PLC_PATH = this.plcPath.Text;
        }

        private void MelsecConnectionInfoPanel_Load(object sender, EventArgs e)
        {
            if (this.machineIfSetting== null)
                return;

            this.ipAddress.Text = machineIfSetting.TcpIpInfo.IpAddress;
            this.cpuType.Text = machineIfSetting.CPU_TYPE;
            this.plcPath.Text = machineIfSetting.PLC_PATH;
        }
    }
}
