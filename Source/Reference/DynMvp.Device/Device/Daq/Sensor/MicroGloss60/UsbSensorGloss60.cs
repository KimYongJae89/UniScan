using DynMvp.Devices.Daq;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DynMvp.Device.Daq.Sensor.UsbSensorGloss_60
{
    public enum BykCommand
    {
        IdentificationCode = 0x00,
        SerialNumber = 0x02,
        CatalogNumber = 0x03,
        CalibrationDateTime = 0x07,
        Measure = 0x09,
        GetValue = 0x47,
        CalibrateGloss = 0x40,
        CalibrateStatusFlag = 0x39,
        SetDateTime = 0x14,
        Reset = 0x27
    }

    static class BykCommandExtension
    {
        public static StringBuilder ToCmdString(BykCommand duration, byte[] paramArray = null)
        {
            StringBuilder cmdStringBuilder = new StringBuilder();
            cmdStringBuilder.Append(Encoding.ASCII.GetString(new byte[] { (byte)duration }));
            if (paramArray != null)
            {
                foreach (var param in paramArray)
                {
                    cmdStringBuilder.Append(Encoding.ASCII.GetString(new byte[] { param }));
                }
            }
            return cmdStringBuilder;
        }
    }

    public class GlossSensorInfo
    {
        public int PortNo { get; set; }
        public string Name { get; set; }
    }

    public static class UsbSensorGloss60
    {
        static int bykHandler = 0;

        [DllImport("bykusbcom.dll")]
        static extern int BYKCom_Open(int portNumber, ref int bykHandler);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_Close(int bykHandler);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_FmtCommand(int bykHandler, StringBuilder Cmd, int CmdLen, StringBuilder result, int MaxReturn, ref int Written);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_RawCommand(int bykHandler, StringBuilder Cmd, int CmdLen, StringBuilder result, int MaxReturn, ref int Written);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_Info(int bykHandler, StringBuilder result, int MaxReturn, ref int Written);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_ListDevices(StringBuilder result, int MaxReturn, out int written);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_VersionDLL(StringBuilder result, int MaxReturn);

        [DllImport("bykusbcom.dll")]
        public static extern int BYKCom_Settings(StringBuilder result);

        public static int Settings(string settingMsg)
        {
            StringBuilder stringBuilder = new StringBuilder(512);
            stringBuilder.Append(settingMsg);

            return BYKCom_Settings(stringBuilder);
        }

        public static int Open(int portNumber)
        {
            return BYKCom_Open(portNumber, ref bykHandler);
        }

        public static int Close()
        {
            return BYKCom_Close(bykHandler);
        }

        public static double Measure()
        {
            string result = "";

            UsbSensorGloss60.FmtCommand(BykCommand.Measure, out result);
            UsbSensorGloss60.FmtCommand(BykCommand.GetValue, out result);

            string[] values = result.Split('\t');
            if (values.Length != 1)
            {
                result = values[1].Substring(0, 6);
                return Convert.ToDouble(result);
            }
            return -1;
        }

        public static void Calibrate()
        {
            UsbSensorGloss60.FmtCommand(BykCommand.CalibrateGloss, out string result);
        }

        public static string GetCalibrationDate()
        {
            UsbSensorGloss60.FmtCommand(BykCommand.CalibrationDateTime, out string result);

            string[] values = result.Split('\t');
            if (values.Length != 1)
            {
                return result = values[1];
            }
            return "-1";
        }

        public static string GetCalibrateStatusFlag()
        {
            UsbSensorGloss60.FmtCommand(BykCommand.CalibrateStatusFlag, out string result);

            string[] values = result.Split('\t');
            if (values.Length != 1)
            {
                result = values[1];
                switch (result)
                {
                    case "1":
                    return "Error Code [1] Generally occurs only with major changes in climatic or weather conditions.";
                    case "2":
                        return "Error Code [2] Generally occurs when there is a significant amount of dirt or dust on the standard or optics.";
                    case "3":
                        return "Error Code [3] Defect in the electronics or operating error";
                    case "4":
                        return "Error Code [4] Defect in the lamp or electronics.";
                    case "5":
                        return "Error Code [5] Defect in the electronics.";

                }
            }

            return "0";
        }

        public static string SetDateTime()
        {
            UsbSensorGloss60.FmtCommand(BykCommand.SetDateTime, out string result);
            return result;
        }

        public static void Reset()
        {
            UsbSensorGloss60.FmtCommand(BykCommand.Reset, out string result);
        }

        public static string FmtCommand(BykCommand command, out string result)
        {
            byte[] paramArray = null;
            int resultLength = 0;

            switch (command)
            {
                case BykCommand.Measure:
                    paramArray = new byte[] { 0x02 };
                    resultLength = 0;
                    break;
                case BykCommand.GetValue:
                    resultLength = 35;
                    break;
                case BykCommand.IdentificationCode:
                    resultLength = 320;
                    break;
                case BykCommand.SerialNumber:
                    resultLength = 64;
                    break;
                case BykCommand.CatalogNumber:
                    resultLength = 32;
                    break;
                case BykCommand.CalibrationDateTime:
                    resultLength = 458;
                    break;
                case BykCommand.CalibrateGloss:
                    resultLength = 0;
                    break;
                case BykCommand.CalibrateStatusFlag:
                    resultLength = 10;
                    break;
                case BykCommand.SetDateTime:
                    string date = DateTime.Now.ToString("yyMMddHHmmss");
                    char[] charArray = date.ToCharArray();
                    Array.Reverse(charArray);
                    paramArray = Encoding.ASCII.GetBytes(charArray);
                    resultLength = 10;
                    break;
            }

            int written = 0;
            StringBuilder resultStr = new StringBuilder(1024);
            StringBuilder cmdStr = BykCommandExtension.ToCmdString(command, paramArray);
            int retVal = BYKCom_FmtCommand(bykHandler, cmdStr, cmdStr.Length, resultStr, resultLength, ref written);

            return result = resultStr.ToString();
        }

        public static int RawCommand(StringBuilder Cmd, int CmdLen, StringBuilder result, int MaxReturn, ref int Written)
        {
            return BYKCom_RawCommand(bykHandler, Cmd, CmdLen, result, MaxReturn, ref Written);
        }

        public static int Info(StringBuilder result, int MaxReturn, ref int Written)
        {
            return BYKCom_Info(bykHandler, result, MaxReturn, ref Written);
        }

        public static List<GlossSensorInfo> ListDevices()
        {
            UsbSensorGloss60.Settings("CheckOnlyUsb 1\r\n"); // 대략 8.6초 정도 빨라짐

            List<GlossSensorInfo> deviceList = new List<GlossSensorInfo>();

            StringBuilder resultStr = new StringBuilder(512);
            int written = 0;
            BYKCom_ListDevices(resultStr, 512, out written);
            if (resultStr.ToString() == "")
                return null;

            string[] deviceStrings = resultStr.ToString().Split(new char[] { '\\' });
            foreach (string deviceString in deviceStrings)
            {
                string[] tokens = deviceString.Split(new char[] { '\t' });

                GlossSensorInfo glossSensorInfo = new GlossSensorInfo();

                glossSensorInfo.PortNo = Convert.ToInt32(tokens[0]);
                glossSensorInfo.Name = tokens[1];

                deviceList.Add(glossSensorInfo);
            }

            return deviceList;
        }

        public static string VersionDLL()
        {
            StringBuilder result = new StringBuilder();
            BYKCom_VersionDLL(result, 255);

            return result.ToString();
        }
    }
}