using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Device.Device.UPS
{
    public class UdpUpsSetting : UpsSetting
    {
        public string RemoteUpsID { get; set; } = "UniEyeUPS";
        public string MulticastGroupIP { get; set; } = "224.0.1.0";
        public int MulticastPort { get; set; } = 1500;
        public int TimeoutMs { get; set; } = 10000;

        public UdpUpsSetting() : base(UpsType.Remote) { }

        protected override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);

            XmlHelper.SetValue(xmlElement, "RemoteUpsID", this.RemoteUpsID);
            XmlHelper.SetValue(xmlElement, "MulticastGroupIP", this.MulticastGroupIP);
            XmlHelper.SetValue(xmlElement, "MulticastPort", this.MulticastPort);
            XmlHelper.SetValue(xmlElement, "TimeoutMs", this.TimeoutMs);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);

            this.RemoteUpsID = XmlHelper.GetValue(xmlElement, "RemoteUpsID", this.RemoteUpsID);
            this.MulticastGroupIP = XmlHelper.GetValue(xmlElement, "MulticastGroupIP", this.MulticastGroupIP);
            this.MulticastPort = XmlHelper.GetValue(xmlElement, "MulticastPort", this.MulticastPort);
            this.TimeoutMs = XmlHelper.GetValue(xmlElement, "TimeoutMs", this.TimeoutMs);
        }
    }

    public class RemoteUps : Ups
    {
        private UdpClient udpClient = null;
        private IAsyncResult asyncResult = null;
        private SystemPowerStatus lastRecivedPowerState = null;
        private bool stopService = false;

        public IPEndPoint IPEndPointAny { get; private set; }
        public IPAddress IPAddressMulticastGroup { get; private set; }

        public RemoteUps(UdpUpsSetting upsSetting) : base(upsSetting)
        {
            IPEndPointAny = new IPEndPoint(IPAddress.Any, ((UdpUpsSetting)this.UpsSetting).MulticastPort);
            IPAddressMulticastGroup = IPAddress.Parse(((UdpUpsSetting)this.UpsSetting).MulticastGroupIP);

            udpClient = new UdpClient();
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udpClient.ExclusiveAddressUse = false;
            udpClient.Client.Bind(IPEndPointAny);
            udpClient.JoinMulticastGroup(IPAddressMulticastGroup);
        }

        public override void StartService()
        {
            if (this.stopService)
                return;

            if (asyncResult == null || asyncResult.IsCompleted)
                asyncResult = this.udpClient?.BeginReceive(OnReceive, new object());
        }

        public override void StopService()
        {
            this.stopService = true;
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint iPEndPoint = IPEndPointAny;
                byte[] bytes = this.udpClient?.EndReceive(ar, ref iPEndPoint);
                StartService();
                if (bytes != null)
                {
                    string message = Encoding.Default.GetString(bytes);
                    SystemPowerStatus status = SystemPowerStatus.FromLine(message, out string id);
                    if (((UdpUpsSetting)this.UpsSetting).RemoteUpsID == id)
                        this.lastRecivedPowerState = status;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"UdpUps::OnReceive - {ex.GetType().Name} - {ex.Message}");
            }

        }
        public override SystemPowerStatus GetPowerState()
        {
            return this.lastRecivedPowerState;
        }
    }
}
