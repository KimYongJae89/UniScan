using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Authentication
{
    public enum PermissionType
    {
        Grantted, Denied
    }

    public class Permission
    {
        private int code;
        public int Code
        {
            get { return code; }
            set { code = value; }
        }

        private PermissionType permissionType;
        public PermissionType PermissionType
        {
            get { return permissionType; }
            set { permissionType = value; }
        }

        public Permission(int code, PermissionType permissionType)
        {
            this.code = code;
            this.permissionType = permissionType;
        }

        public override int GetHashCode()
        {
            return code;
        }
    }

    public class PermissionControl
    {
        private List<Permission> permisionList = new List<Permission>();
        public List<Permission> PermisionList
        {
            get { return permisionList; }
        }

        private bool superAccount;
        public bool SuperAccount
        {
            get { return superAccount; }
            set { superAccount = value; }
        }

        public void SetPermissions(string permissionXml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(permissionXml);

            XmlElement permissionListElement = xmlDocument.DocumentElement;
            foreach (XmlElement permissionElement in permissionListElement)
            {
                if (permissionElement.Name == "Task")
                {
                    int code = Convert.ToInt32(XmlHelper.GetValue(permissionElement, "Code", "0"));
                    PermissionType permissionType = (PermissionType)Enum.Parse(typeof(PermissionType), XmlHelper.GetValue(permissionElement, "PermissionType", "Denied"));

                    permisionList.Add(new Permission(code, permissionType));
                }
            }
        }

        public string GetPermissions()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement permissionListElement = xmlDocument.CreateElement("", "PermissionList", "");
            xmlDocument.AppendChild(permissionListElement);

            foreach (Permission permission in permisionList)
            {
                XmlElement permissionElement = xmlDocument.CreateElement("", "Task", "");
                permissionListElement.AppendChild(permissionElement);

                XmlHelper.SetValue(permissionElement, "Code", permission.Code.ToString());
                XmlHelper.SetValue(permissionElement, "PermissionType", permission.PermissionType.ToString());
            }

            return xmlDocument.InnerXml;
        }

        public bool IsGranted(int taskCode)
        {
            if (superAccount == true)
                return true;

            foreach (Permission permission in permisionList)
            {
                if (permission.Code == taskCode)
                {
                    return (permission.PermissionType == PermissionType.Grantted);
                }
            }

            return false;
        }

        public void Clear()
        {
            permisionList.Clear();
        }

        public void AddGranted(int taskCode)
        {
            if (IsGranted(taskCode))
                return;

            permisionList.Add(new Permission(taskCode, PermissionType.Grantted));
        }

        public override string ToString()
        {
            string grantString = "";
            string deniedString = "";

            foreach (Permission permission in permisionList)
            {
                if (permission.PermissionType == PermissionType.Grantted)
                {
                    grantString += permission.Code.ToString() + ";";
                }
                else
                {
                    deniedString += permission.Code.ToString() + ";";
                }
            }

            return "[G]" + grantString + "/[D]" + deniedString;
        }
    }
}
