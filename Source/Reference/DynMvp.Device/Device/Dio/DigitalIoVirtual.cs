using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

using DynMvp.Base;

namespace DynMvp.Devices.Dio
{
    [Serializable]
    public class InputPortData
    {
        const int maxIntSize = 32;

        public int data0;
        public int data1;
        public int data2;
        public int data3;
        public int data4;
        public int data5;
        public int data6;
        public int data7;
        public int data8;
        public int data9;
        public int data10;
        public int data11;
        public int data12;
        public int data13;
        public int data14;
        public int data15;
        public int data16;
        public int data17;
        public int data18;
        public int data19;
        public int data20;
        public int data21;
        public int data22;
        public int data23;
        public int data24;
        public int data25;
        public int data26;
        public int data27;
        public int data28;
        public int data29;
        public int data30;
        public int data31;

        public int[] GetDataArray()
        {
            int[] dataArray = new int[maxIntSize];

            dataArray[0] = data0;
            dataArray[1] = data1;
            dataArray[2] = data2;
            dataArray[3] = data3;
            dataArray[4] = data4;
            dataArray[5] = data5;
            dataArray[6] = data6;
            dataArray[7] = data7;
            dataArray[8] = data8;
            dataArray[9] = data9;
            dataArray[10] = data10;
            dataArray[11] = data11;
            dataArray[12] = data12;
            dataArray[13] = data13;
            dataArray[14] = data14;
            dataArray[15] = data15;
            dataArray[16] = data16;
            dataArray[17] = data17;
            dataArray[18] = data18;
            dataArray[19] = data19;
            dataArray[20] = data20;
            dataArray[21] = data21;
            dataArray[22] = data22;
            dataArray[23] = data23;
            dataArray[24] = data24;
            dataArray[25] = data25;
            dataArray[26] = data26;
            dataArray[27] = data27;
            dataArray[28] = data28;
            dataArray[29] = data29;
            dataArray[30] = data30;
            dataArray[31] = data31;

            return dataArray;
        }

        public uint GetInputValue()
        {
            int[] dataArray = GetDataArray();

            uint inputValue = 0;
            for (int index = 0; index < maxIntSize; index++)
            {
                if (dataArray[index] != 0)
                {
                    inputValue |= (uint)(1 << index);
                }
            }

            return inputValue;
        }
    }

    public class DigitalIoVirtual : DigitalIo
    {
        uint inputPortStatus = 0;
        uint outputPortStatus = 0;
        string inputPortDataFile;
        DateTime lastOutputFileTime = DateTime.Now;

        public override bool IsVirtual => true;

        public DigitalIoVirtual(string name) : base(DigitalIoType.Virtual, name)
        {
        }

        public DigitalIoVirtual(DigitalIoType digitalIoType, string name) : base(digitalIoType, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            this.inputPortDataFile = String.Format(@"{0}\..\Config\VirtualInputPort.xml", Environment.CurrentDirectory);
            if (File.Exists(this.inputPortDataFile))
                File.Delete(this.inputPortDataFile);

            CreateInputPortDataFile();

            outputPortStatus = 0;
            inputPortStatus = 0;

            this.NumInPortGroup = digitalIoInfo.NumInPortGroup;
            this.InPortStartGroupIndex = digitalIoInfo.InPortStartGroupIndex;
            this.NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
            this.OutPortStartGroupIndex = digitalIoInfo.OutPortStartGroupIndex;

            this.NumInPort = digitalIoInfo.NumInPort;
            this.NumOutPort = digitalIoInfo.NumOutPort;

            UpdateState(DeviceState.Ready);
            return true;
        }

        public override void Release()
        {
            base.Release();
        }

        public void CreateInputPortDataFile()
        {
            FileStream fileStream = File.Create(inputPortDataFile);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(InputPortData), "InputPortData");
            xmlSerializer.Serialize(fileStream, new InputPortData());
            fileStream.Close();
        }

        public bool ReadInputPortDataFile()
        {
            try
            {
                FileStream fileStream = File.OpenRead(inputPortDataFile);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(InputPortData), "InputPortData");
                InputPortData inputPortData = (InputPortData)xmlSerializer.Deserialize(fileStream);
                fileStream.Close();

                inputPortStatus = inputPortData.GetInputValue();
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("DigitalIoVirtual::ReadInputPortDataFile - {0}", ex.Message));
                return false;
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            this.outputPortStatus = outputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            this.inputPortStatus = inputPortStatus;
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            lock (this.inputPortDataFile)
            {
                DateTime curOutputFileTime = File.GetLastWriteTime(inputPortDataFile);
                if (curOutputFileTime > lastOutputFileTime)
                {
                    if(ReadInputPortDataFile())
                    lastOutputFileTime = curOutputFileTime;
                }
            }
            return inputPortStatus;
        }
    }
}
