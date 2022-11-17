using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    internal class UPSSettings : SubSettings
    {
        public string UPSID { get; set; } = "UniEyeUPS";

        public bool MultiCast { get; set; } = false;
        public string MultiCastIP { get; set; } = "192.168.0.0";
        public int MultiCastPort { get; set; } = 1500;


        public bool BroadCast { get; set; } = false;
        public string BroadCastIP { get; set; } = "192.168.0.255";
        public int BroadCastPort { get; set; } = 1500;


        public UPSSettings() : base(false) { }

        protected override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
        }

        protected override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);
        }
    }

    internal class UPSOperator : Operator
    {
        private UPSSettings settings;

        private PowerStatus ps;

        public override bool Use => settings.Use;

        public UPSOperator(UPSSettings settings) : base()
        {
            this.settings = settings;
        }

        internal override Color GetStripColor()
        {
            Color color = Control.DefaultBackColor;
            if (this.Use && this.ps != null)
            {
                if (ps.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
                    color = Color.Red;
                else if (ps.PowerLineStatus == PowerLineStatus.Online)
                    color = Color.LightGreen;
                else
                    color = Color.LightPink;
            }

            return color;

        }

        public void CheckUpsStatus()
        {
            if (!this.settings.Use)
                return;

            this.ps = SystemInformation.PowerStatus;
            //if(powerStatus.PowerLineStatus == PowerLineStatus.Offline)

            //Console.WriteLine($"BatteryChargeStatus: {ps.BatteryChargeStatus}");
            //Console.WriteLine($"BatteryFullLifetime: {ps.BatteryFullLifetime}");
            //Console.WriteLine($"BatteryLifePercent: {ps.BatteryLifePercent * 100}"); // 남은용량 (%)
            //Console.WriteLine($"BatteryLifeRemaining: {ps.BatteryLifeRemaining}"); // 남은시간 (초)
            //Console.WriteLine($"PowerLineStatus: {ps.PowerLineStatus}"); // 전원 상태 (On/Off)
            //Console.WriteLine($"==========================================");

            bool isBattaryExist = (ps.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery);
            bool isPowerOnline = (ps.PowerLineStatus == PowerLineStatus.Online);
            int battaryLifePercent = (int)(ps.BatteryLifePercent * 100);
            int battaryRemainTimeSec = ps.BatteryLifeRemaining;

            byte[] bytes = Encoding.UTF8.GetBytes($"{this.settings.UPSID}:{isBattaryExist}:{isPowerOnline}:{battaryLifePercent}:{battaryRemainTimeSec}");
            if (this.settings.BroadCast)
                Send(this.settings.BroadCastIP, this.settings.BroadCastPort, bytes);            

            if (this.settings.MultiCast)
                Send(this.settings.MultiCastIP, this.settings.MultiCastPort, bytes);
        }

        private void Send(string ip, int portNo, byte[] bytes)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), portNo);
            using (UdpClient udpClient = new UdpClient())
            {
                udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpClient.Send(bytes, bytes.Length, iPEndPoint);
                udpClient.Close();
            }
        }
    }
}