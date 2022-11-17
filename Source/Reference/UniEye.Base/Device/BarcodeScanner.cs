using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniEye.Base.Device
{
    public enum BarcodeReaderType
    {
        Serial, Socket
    }

    public class BarcodeScanner
    {
        protected string barcodeRead;
        public string BarcodeRead
        {
            get { return barcodeRead; }
            set { barcodeRead = value; }
        }
    }

    public class BarcodeReaderFactory
    {
        public static BarcodeScanner Create(BarcodeReaderType barcodeReaderType)
        {
            return new SerialBarcodeReader();
        }
    }

    public class SerialBarcodeReader : BarcodeScanner
    {
        SerialPortEx barcodeReader;
        public SerialPortEx BarcodeReader
        {
            get { return barcodeReader; }
        }

        public void Init()
        {
            SerialPortInfo serialPortInfo = MachineSettings.Instance().GetSerialPortInfo("BarcodeReader");
            if (serialPortInfo == null)
                return;

            SimplePacketParser simplePacketParser = new SimplePacketParser();
            simplePacketParser.DataReceived += BarcodeDataReceived;
            simplePacketParser.EndChar = new byte[1] { (byte)'\r' };

            barcodeReader = new SerialPortEx(simplePacketParser);
            barcodeReader.Open(serialPortInfo.PortName, serialPortInfo);

            //barcodeReader.PacketHandler.PacketParser = simplePacketParser;
            barcodeReader.StartListening();
        }

        private void BarcodeDataReceived(ReceivedPacket receivedPacket)
        {
            barcodeRead = Encoding.Default.GetString(receivedPacket.ReceivedData);
        }
    }
}
