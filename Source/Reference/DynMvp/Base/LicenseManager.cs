using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Management;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Xml;

namespace DynMvp.Base
{
    public static class LicenseManager
    {
        public enum ELicenses
        {
            /// <summary>
            /// Parse Environment CommandLine Args
            /// </summary>
            EnvArgs,

            /// <summary>
            /// Gravure FP/Index Observer Function
            /// </summary>
            ExtObserve,

            /// <summary>
            /// Gravure Stop Image Function
            /// </summary>
            ExtStopImg,

            /// <summary>
            /// Gravure Margin Measure Function
            /// </summary>
            ExtMargin,

            /// <summary>
            /// Gravure Transform Measure Function
            /// </summary>
            ExtTransfrom,

            /// <summary>
            /// Gravure Roll Grade Decision Function
            /// </summary>
            DecGrade,


        }

        public static string DefaultFileName => "License.key";

        public static string[] Licenses => licenseList.ToArray();

        public static bool IsInitialized => File.Exists(keyFile);

        static List<string> licenseList = new List<string>();
        static string keyFile;
        static string key = GetBaseboardID(8);

        public static bool IsAvailable(string taskName)
        {
            bool result = false;
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Planbss\License");
            if (registryKey != null)
                result = Convert.ToBoolean(registryKey.GetValue(taskName).ToString());

            return result;
        }

        public static void Clear()
        {
            licenseList.Clear();
        }

        public static bool Exist(ELicenses license)
        {
            return licenseList.Contains(license.ToString());
        }

        public static bool Exist(string license)
        {
            return licenseList.Contains(license);
        }

        public static void Set(ELicenses license, bool use)
        {
            Set(license.ToString(), use);
        }

        public static void Set(string license, bool use)
        {
            if (use)
                Add(license);
            else
                Remove(license);
        }

        public static void Add(ELicenses license)
        {
            Add(license.ToString());
        }

        public static void Add(string license)
        {
            if (!string.IsNullOrEmpty(license) && !licenseList.Contains(license))
                licenseList.Add(license);
        }

        public static void Remove(ELicenses license)
        {
            Remove(license.ToString());
        }

        public static void Remove(string license)
        {
            licenseList.Remove(license);
        }

        public static string GetBaseboardID(int digit)
        {
            // Win32_CPU will work too
            var search = new ManagementObjectSearcher("SELECT * FROM Win32_baseboard");
            var mobos = search.Get();

            foreach (var m in mobos)
            {
                var serial = m["SerialNumber"]; // ProcessorID if you use Win32_CPU
                //Console.WriteLine("Baseboard ID : " + serial);
                return StringHelper.PadTrim(serial.ToString(), 8, '0', false);
                //return serial.ToString().PadRight(digit, '0').Substring(0, digit);
            }
            return "";
        }

        public static string GetString()
        {
            return string.Join(";", licenseList);
        }

        private static byte[] GetEncryptBytes(string key)
        {
            string data = GetString();
            byte[] bytes = Encoding.Default.GetBytes(data);

            return Encrypt(key, bytes);
        }

        private static void SetEncryptBytes(string key, byte[] encryptBytes)
        {
            //string key = "00000001"; // 8-length string;
            byte[] bytes = Decrypt(key, encryptBytes);

            string data = Encoding.Default.GetString(bytes);
            string[] lines = data.Split(';');
            licenseList.Clear();
            Array.ForEach(lines, f => Add(f.Trim('\0')));
        }

        public static byte[] Encrypt(string key, byte[] bytes)
        {
            if ((key ?? "").Length != 8)
                throw new InvalidOperationException("encryption key must have 8-length");

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.Default.GetBytes(key);
            provider.IV = Encoding.Default.GetBytes(key);

            byte[] encBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cryStream = new CryptoStream(ms, provider.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryStream.Write(bytes, 0, bytes.Length);

                    cryStream.FlushFinalBlock();
                    cryStream.Close();
                }
                encBytes = ms.ToArray();
            }

            //System.Diagnostics.Debug.WriteLine($"LicenseManager::GetEncryptString - Key: {key}, Bytes.Length: {encBytes.Length}");
            return encBytes;
        }

        public static byte[] Decrypt(string key, byte[] bytes)
        {
            if ((key ?? "").Length != 8)
                throw new InvalidOperationException("decryption key must have 8-length");

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.Default.GetBytes(key);
            provider.IV = Encoding.Default.GetBytes(key);

            List<byte> byteList = new List<byte>();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (CryptoStream cryStream = new CryptoStream(ms, provider.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    int b;
                    while ((b = cryStream.ReadByte()) >= 0)
                        byteList.Add((byte)b);
                }
            }


            //System.Diagnostics.Debug.WriteLine($"LicenseManager::GetEncryptString - Key: {key}, Bytes.Length: {encBytes.Length}");
            return byteList.ToArray();
        }



        //public static void SaveXml(XmlElement xmlElement, string key = "")
        //{
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
        //        xmlElement.AppendChild(subElement);
        //        SaveXml(subElement);
        //        return;
        //    }

        //    byte[] bytes = GetEncryptBytes();
        //    string str = Convert.ToBase64String(bytes);
        //    XmlHelper.SetValue(xmlElement, "License", str);
        //}

        public static void Save()
        {
            if (string.IsNullOrEmpty(keyFile))
                throw new InvalidOperationException($"Default .key file is not defined.");

            SaveAs(keyFile);
        }

        public static void SaveAs(string path, string key = "")
        {
            if (string.IsNullOrEmpty(key))
                key = LicenseManager.key;

            byte[] bytes = GetEncryptBytes(key);
            string str = Convert.ToBase64String(bytes);

            using (StreamWriter sw = new StreamWriter(path, false))
                sw.Write(str);
        }

        //public static bool LoadXml(XmlElement xmlElement, string key = "")
        //{
        //    if (!string.IsNullOrEmpty(key))
        //    {
        //        XmlElement subElement = xmlElement[key];
        //        if (subElement == null)
        //            return false;

        //        return LoadXml(subElement);
        //    }

        //    try
        //    {
        //        string str = XmlHelper.GetValue(xmlElement, "License", "");
        //        byte[] bytes = Convert.FromBase64String(str);
        //        SetEncryptBytes(bytes);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Error(LoggerType.Error, $"LicenseManager::LoadXml - {ex.Message}");
        //        throw ex;
        //    }
        //}

        //public static void Load()
        //{
        //    if (!IsInitialized)
        //        return;

        //    Load(licenseFile, licenseKey);
        //}

        public static void Reload()
        {
            if (string.IsNullOrEmpty(keyFile))
                throw new InvalidOperationException($"Default .key file is not defined.");

            Load(keyFile, key);
        }

        public static void Load(string keyFile, string key)
        {
            if (!File.Exists(keyFile))
                throw new FileNotFoundException($"cannot find {new FileInfo(keyFile).Name} file.");

            if ((key ?? "").Length != 8)
                throw new InvalidOperationException("decryption key must have 8-length");

            try
            {
                using (StreamReader sr = new StreamReader(keyFile))
                {
                    string str = sr.ReadToEnd();
                    byte[] bytes = Convert.FromBase64String(str);
                    SetEncryptBytes(key, bytes);
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Error(LoggerType.Error, $"LicenseManager::Load - {ex.Message}");
                LogHelper.Error(LoggerType.Error, ex);
                throw ex;
            }
        }

        public static void Set(string keyFile, string key = null)
        {
            LicenseManager.keyFile = keyFile;
            LicenseManager.key = (key ?? GetBaseboardID(8)).PadLeft(8, '0').Substring(0, 8);
            Reload();
        }
    }
}
