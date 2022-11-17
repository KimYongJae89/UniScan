using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Data
{
    public static class ABDataConverter
    {

        public static byte[] Str2Byte(string @string)
        {
            int len = @string.Length / 2;
            byte[] bytes = new byte[len];

            for (int i = 0; i < len; i++)
                bytes[i] = Convert.ToByte(@string.Substring(i * 2, 2), 16);

            return bytes;
        }

        public static short[] Str2Word(string @string)
        {
            byte[] bytes = Str2Byte(@string);

            int len = bytes.Length / 2;
            short[] shorts = new short[len];

            for (int i = 0; i < len; i++)
                shorts[i] = Convert.ToInt16(@string.Substring(i * 4, 4), 16);

            return shorts;
        }

        public static short GetShort(int startIndex, byte[] receivedData)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = (byte)receivedData[startIndex + 1];
            valueByte[1] = (byte)receivedData[startIndex];

            return BitConverter.ToInt16(valueByte, 0);
        }

        public static int GetInt(int startIndex, byte[] receivedData)
        {
            byte[] copyByte = new byte[4];
            copyByte[0] = receivedData[startIndex + 3];
            copyByte[1] = receivedData[startIndex + 2];
            copyByte[2] = receivedData[startIndex + 1];
            copyByte[3] = receivedData[startIndex + 0];

            //int tempResult = BitConverter.ToInt32(valueByteResult, 0);
            var rr = BitConverter.ToInt32(copyByte, 0);
            return BitConverter.ToInt32(copyByte, 0);
        }

        public static string GetString(int startIndex, int length, byte[] receivedData)
        {
            StringBuilder valueStrBuilder = new StringBuilder();
            for (int i = 0; i < length; i ++)
                valueStrBuilder.Append((char)receivedData[startIndex + i]);

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
