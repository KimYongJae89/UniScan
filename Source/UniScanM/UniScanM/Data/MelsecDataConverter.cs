using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Data
{
    public static class MelsecDataConverter
    {
        public static short GetShort(int startIndex, byte[] receivedData)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = (byte)receivedData[startIndex + 1];
            valueByte[1] = (byte)receivedData[startIndex];

            return BitConverter.ToInt16(valueByte, 0);
        }

        public static int GetInt(int startIndex, byte[] receivedData)
        {
            byte[] valueByte = new byte[4];
            byte[] valueByteResult = new byte[4];
            for (int i = 0; i < 4; i++)
                valueByte[i] = (byte)receivedData[startIndex + i];
            valueByteResult[0] = valueByte[1];
            valueByteResult[1] = valueByte[0];
            valueByteResult[2] = valueByte[3];
            valueByteResult[3] = valueByte[2];

            //int tempResult = BitConverter.ToInt32(valueByteResult, 0);
            return BitConverter.ToInt32(valueByteResult, 0);
        }

        public static string GetString(int startIndex, int length, byte[] receivedData)
        {
            StringBuilder valueStrBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
                valueStrBuilder.Append((char)receivedData[startIndex + i]);

            string result = valueStrBuilder.ToString();
            return result.Replace("\0", string.Empty);
        }


        public static string GetString_LittleEndian(int startIndex, int length, byte[] receivedData)
        {
            StringBuilder valueStrBuilder = new StringBuilder();
            for (int i = 0; i < length; i += 2)
            {
                valueStrBuilder.Append((char)receivedData[startIndex + i + 1]);
                valueStrBuilder.Append((char)receivedData[startIndex + i + 0]);
            }

            string result = valueStrBuilder.ToString();
            return result.Replace("\0", string.Empty);
        }

        public static string GetSwapString(int startIndex, int length, byte[] recivedData)
        {
            StringBuilder valueStrBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                valueStrBuilder.Append((char)recivedData[startIndex + i]);
            }
            string result = valueStrBuilder.ToString();
            char[] charResult = result.ToArray();


            if (result.Length % 2 == 0)
            {
                for (int i = 0; i < result.Length; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }
            else
            {
                for (int i = 0; i < result.Length - 1; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }

            string returnResult = "";
            for (int i = 0; i < charResult.Length; i++)
            {
                returnResult += charResult[i].ToString();
            }

            return returnResult.Replace("\0", string.Empty);
        }

        public static byte[] GetByte(int startIndex, int length, string receivedData)
        {
            byte[] getByte = new byte[length];
            for (int i = 0; i < length; i++)
                getByte[i] = (byte)receivedData[startIndex + i];
            return getByte;
        }

        public static byte[] GetSwapBit(byte[] data)
        {
            for (int i = 0; i < data.Length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }

        public static byte[] GetSwapBit(byte[] data, int startIndex, int length)
        {
            for (int i = startIndex; i < length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }

        public static string WInt(int data)
        {
            string sdata = string.Format("{0:X08}", data);

            string temp1 = sdata.Substring(0, 4);
            string temp2 = sdata.Substring(4, 4);

            sdata = string.Format("{0}{1}", temp2, temp1);
            return  sdata;
        }
    }
}
