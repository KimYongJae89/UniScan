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
    public partial class TcpIpMachineIfPanel : UserControl, IMachineIfPanel
    {
        TcpIpMachineIfSetting tcpIpMachineIfSettings;
        public TcpIpMachineIfSetting TcpIpMachineIfSettings
        {
            get { return tcpIpMachineIfSettings; }
            set { tcpIpMachineIfSettings = value; }
        }

        public void SetDefault()
        {
            tcpIpMachineIfSettings.MachineIfProtocolList.Initialize(tcpIpMachineIfSettings.MachineIfType);
        }
        public TcpIpMachineIfPanel()
        {
            InitializeComponent();

            groupBoxTcpIp.Text = StringManager.GetString(this.GetType().FullName,groupBoxTcpIp.Text);
            labelIpAddress.Text = StringManager.GetString(this.GetType().FullName,labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
        }

        public void Initialize()
        {
            Initialize(this.tcpIpMachineIfSettings);
        }

        public void Initialize(MachineIfSetting machineIfSetting)
        {
            this.tcpIpMachineIfSettings = (TcpIpMachineIfSetting)machineIfSetting;
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
            return true;
        }

        public void Apply()
        {
            tcpIpMachineIfSettings.TcpIpInfo = new DynMvp.Devices.Comm.TcpIpInfo(ipAddress.Text, int.Parse(portNo.Text));
        }

        private void TcpIpMachineIfPanel_Load(object sender, EventArgs e)
        {
            ipAddress.Text = tcpIpMachineIfSettings.TcpIpInfo.IpAddress;
            portNo.Text = tcpIpMachineIfSettings.TcpIpInfo.PortNo.ToString();
        }
    }
}
