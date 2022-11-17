using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace DynMvp.Base
{
    public class RegistryHelper
    {
        public static bool IsInstalled(string programName)
        {
            foreach(string item in Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall").GetSubKeyNames())
            {
                object displayNameObj = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + item).GetValue("DisplayName");
                if (displayNameObj != null)
                {
                    string displayName = displayNameObj.ToString();
                    if (displayName.Contains(programName) == true)
                    {
                        return true;
                    } 
                }
            }

            return false;
        }

        public static void WriteSoftwareKey()
        {
            RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\UK");
            registryKey.SetValue("Master", "I&Dw7bdWofe*e(fHcUCLM*", RegistryValueKind.String);
        }

        public static bool IsValidSoftwareKey()
        {
            RegistryKey registryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\UK", RegistryKeyPermissionCheck.ReadSubTree);
            object keyValue = registryKey.GetValue("Master");
            if (keyValue != null)
            {
                if (keyValue.ToString() == "I&Dw7bdWofe*e(fHcUCLM*")
                    return true;
            }

            return false;
        }

        public static void SetProgramValue(string programName, string keyName, string valueName, string value)
        {
            string keyFullName = String.Format(@"SOFTWARE\UniEye\{0}\{1}", programName, keyName);
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(keyFullName);

            registryKey.SetValue(valueName, value);
        }

        public static string GetProgramValue(string programName, string keyName, string valueName)
        {
            string keyFullName = String.Format(@"SOFTWARE\UniEye\{0}\{1}", programName, keyName);
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(keyFullName);
            if (registryKey == null)
                return "";

            object obj = registryKey.GetValue(valueName);
            if (obj == null)
                return "";

            return obj.ToString();
        }
    }
}

