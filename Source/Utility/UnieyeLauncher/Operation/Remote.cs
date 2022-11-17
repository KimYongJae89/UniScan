using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnieyeLauncher.Operation
{
    [Serializable]
    public class RemoteSettings : SubSettings
    {
        public string ID { get; set; } = "UniEye";
        public int Port { get; set; } = 7803;

        public IPEndPoint IPEndPointAny => new IPEndPoint(IPAddress.Any, this.Port);

        public RemoteSettings() : base(false) { }
    }

    public class RemoteOperator : Operator
    {
        private UdpClient udpClient = null;
        private IAsyncResult asyncResult = null;

        RemoteSettings settings;

        public override bool Use => settings.Use;

        public RemoteOperator(RemoteSettings settings)
        {
            this.settings = settings;

            this.udpClient = new UdpClient();
            this.udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.udpClient.ExclusiveAddressUse = false;
            this.udpClient.Client.Bind(this.settings.IPEndPointAny);
        }

        internal override Color GetStripColor()
        {
            return this.Use ? Color.LightGreen : Control.DefaultBackColor;
        }

        public void Start()
        {
            Console.WriteLine("RemoteOperator::Start");
            asyncResult = this.udpClient?.BeginReceive(OnUdpClientReceive, new object());
        }

        public void Stop()
        {
            this.udpClient?.Close();
        }


        private void OnUdpClientReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint iPEndPoint = this.settings.IPEndPointAny;
                byte[] bytes = this.udpClient?.EndReceive(ar, ref iPEndPoint);
                asyncResult = this.udpClient?.BeginReceive(OnUdpClientReceive, new object());
                //OnEvent(EventType.Message, $"RemoteOperator::OnUdpClientReceive - bytes[{bytes.Length}]");
                if (this.Use && bytes != null)
                {
                    string message = Encoding.Default.GetString(bytes);
                    OnEvent(EventType.Message, $"RemoteOperator::OnUdpClientReceive - {message}");

                    string[] token = message.Split(';');
                    if (token.Length < 3)
                        return;

                    if (token[0] != this.settings.ID)
                        return;

                    string processName = token[2];
                    switch (token[1])
                    {
                        case "KILL":
                            Program.MainForm.LaunchProcess.Kill(processName);
                            break;

                        case "RUN":
                            string[] args = token.Skip(3).ToArray();
                            Program.MainForm.LaunchProcess.Kill(processName);
                            System.Threading.Thread.Sleep(5000);
                            Program.MainForm.LaunchProcess.Launch(token[2], args);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
