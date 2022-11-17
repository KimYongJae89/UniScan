using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Windows.Forms;
using DynMvp.UI;
using System.ComponentModel;
using DynMvp.UI.Touch;
using System.Windows.Forms.DataVisualization.Charting;

public interface IMultiLanguageSupport
{
    void UpdateLanguage();
}

namespace DynMvp.Base
{
    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        string key;
        public LocalizedCategoryAttribute(string key, string value) : base(value)
        {
            this.key = key;
        }

        protected override string GetLocalizedString(string value)
        {
            return StringManager.GetString(key, value);
        }
    }

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        string key;
        public LocalizedDisplayNameAttribute(string key, string value)
        {
            this.key = key;
            base.DisplayNameValue = StringManager.GetString(key, value);
        }
        public LocalizedDisplayNameAttribute()
        { }
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        string key;
        public LocalizedDescriptionAttribute(string key, string value)
        {
            this.key = key;
            base.DescriptionValue = StringManager.GetString(key, value);
        }
    }

    public static class StringManager
    {
        static string curConfigPath = "";
        static string curLocaleCode = "";
        public static List<StringTable> StringTableList = new List<StringTable>();
        public static List<IMultiLanguageSupport> MultiLanguageSupportList = new List<IMultiLanguageSupport>();

        public static string ConfigPath => curConfigPath;

        public static string LocaleCode => curLocaleCode;


        public static void ReloadTable()
        {
            Load(curConfigPath, curLocaleCode);
        }

        public static bool ChangeLanguage(string newLocaleCode)
        {
            bool ok = Load(curConfigPath, newLocaleCode);
            if(ok)
                MultiLanguageSupportList.ForEach(f => f.UpdateLanguage());
            return ok;

        }

        public static bool Load(string configPath, string localeCode)
        {
            curConfigPath = configPath;
            curLocaleCode = localeCode;

            string xmlFilePath = GetXmlFilePath(configPath, localeCode);
            return Load(xmlFilePath);
        }

        public static bool Load(string languageFile)
        {
            if (System.IO.File.Exists(languageFile) == false)
            {               
                Save(languageFile);
                return false;
            }

            lock (StringTableList)
                StringTableList.Clear();

            XmlDocument xmldocument = new XmlDocument();
            try
            {
                xmldocument.Load(languageFile);
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("StringManager::Load - {0}", ex.Message));
                MessageForm.Show(null, "StringTable load Fail!");
                return false;
        }
#endif
            finally { }

            XmlElement rootElement = xmldocument.DocumentElement;
            if (rootElement.Name != "StringTableV2" && rootElement.Name != "StringTableV3")
                return false;

            lock (StringTableList)
            {
                StringTableList.Clear();
                foreach (XmlElement stringElment in rootElement)
                {
                    StringTable st = new StringTable(stringElment.Name);
                    st.LoadXml(stringElment);

                    StringTable oldSt = StringTableList.Find(f => f.Name == st.Name);
                    if (oldSt != null)
                        oldSt.Merge(st);
                    else
                        StringTableList.Add(st);
                }
            }

            return true;
        }

        private static string GetXmlFilePath(string configPath,string localeCode)
        {
            if (string.IsNullOrEmpty(configPath))
                return null;

            if (localeCode == "")
                return String.Format("{0}\\StringTable.xml", configPath);
            else
                return String.Format("{0}\\StringTable_{1}.xml", configPath, localeCode);

        }

        public static bool Save(string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = GetXmlFilePath(curConfigPath, curLocaleCode);

            if (string.IsNullOrEmpty(path))
                return false;

            XmlDocument xmldocument = new XmlDocument();
            XmlElement rootElement = xmldocument.CreateElement("", "StringTableV3", "");
            xmldocument.AppendChild(rootElement);

            lock (StringTableList)
            {
                StringTableList.Sort((f, g) => f.Name.CompareTo(g.Name));
                foreach (StringTable stringTable in StringTableList)
                {
                    XmlElement stringTableElement = rootElement.OwnerDocument.CreateElement("", stringTable.Name, "");
                    rootElement.AppendChild(stringTableElement);

                    stringTable.SaveXml(stringTableElement);
                }
            }

            try
            {
                xmldocument.Save(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public static void AddListener(IMultiLanguageSupport item)
        {
            lock(MultiLanguageSupportList)
                MultiLanguageSupportList.Add(item);

            item.UpdateLanguage();
            //UpdateString(item);
        }

        public static void RemoveListener(IMultiLanguageSupport item)
        {
            lock(MultiLanguageSupportList)
                MultiLanguageSupportList.Remove(item);
        }

        public static void Clear()
        {
            lock(StringTableList)
                StringTableList.Clear();
            curConfigPath = "";
            curLocaleCode = "";
        }

        public static string GetString(string searchKey)
        {
            StringTable st = GetStringTable("Message");
            if (st == null)
                st = AddStringTable(new StringTable("Message"));

            if (st.IsExist(searchKey))
            {
                return st.GetString(searchKey);
            }
            else
            {
                st.AddString(searchKey, searchKey);
                Save();
                return searchKey;
            }
        }

        public static string GetString(string tableName, string messageKey)
        {
            if (string.IsNullOrEmpty(messageKey))
                return "";

            StringTable stringTable = GetStringTable(tableName);
            if (stringTable == null)
            {
                stringTable = new StringTable(tableName);
                lock(StringTableList)
                    StringTableList.Add(stringTable);
            }

            lock (stringTable)
            {
                if (stringTable.IsExist(messageKey))
                {
                    return stringTable.GetString(messageKey);
                }
                else
                {
                    stringTable.AddString(messageKey, messageKey);
                    Save();
                    return messageKey;
                }
            }
        }

        /// <summary>
        /// StringTableV2의 SearchKey는 Control.Text
        /// StringTableV3의 SearchKey는 Control.Name, DefaultValue는 Control.Text
        /// V2와 V3의 호환을 위해 Control.Name을 키로 갖는 값이 없으면 Control.Text로 한 번 더 찾아봄.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="controlName"></param>
        /// <param name="controlText"></param>
        /// <returns></returns>
        public static string GetString(string tableName, string controlName, string controlText)
        {
            bool needSave = false;
            string resultValue = "";

            if (string.IsNullOrEmpty(controlName) == false)
            {
                StringTable stringTable = GetStringTable(tableName);
                if (stringTable == null)
                {
                    stringTable = AddStringTable(new StringTable(tableName));
                    needSave = true;
                }

                if (stringTable.IsExist(controlName))
                {
                    resultValue = stringTable.GetString(controlName);
                }
                else
                {
                    // messageKey를 Key로 하여 찾아봄. 없으면 messageKey 신규 등록
                    resultValue = controlText;
                    if (string.IsNullOrEmpty(controlText) == false)
                    {
                        needSave = true;
                        if (stringTable.IsExist(controlText))
                        {
                            string oldValue = stringTable.GetString(controlText);
                            stringTable.DeleteString(controlText);
                            stringTable.AddString(controlName, oldValue);
                            resultValue = oldValue;
                        }
                        else
                        {
                            stringTable.AddString(controlName, controlText);
                        }
                    }
                }
            }

            if (needSave)
                Save();

            return resultValue;
        }

        public static string GetString(string tableName, Control control)
        {
            return GetString(tableName, control.Name, control.Text);
        }

        public static string GetString(string tableName, DataGridViewColumn dataGridViewColumn)
        {
            if (string.IsNullOrEmpty(dataGridViewColumn.HeaderText))
                return "";

            return GetString(tableName, dataGridViewColumn.Name, dataGridViewColumn.HeaderText);
        }

        public static string GetString(string tableName, ToolStripItem toolStripItem)
        {
            if (string.IsNullOrEmpty(toolStripItem.Text))
                return "";

            return GetString(tableName, toolStripItem.Name, toolStripItem.Text);
        }

        public static void UpdateString(IMultiLanguageSupport obj)
        {
            Type type = obj.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                object item = (object)fieldInfo.GetValue(obj);
                
                if(item is DataGridViewColumn)
                {
                    DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)item;
                    //dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn.Name, dataGridViewColumn.HeaderText);
                    dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn);
                }
                else if (item is ToolStripItem)
                {
                    ToolStripItem toolStripItem = (ToolStripItem)item;
                    //toolStrip.Text = StringManager.GetString(type.FullName, toolStrip.Name, toolStrip.Text);
                    toolStripItem.Text = StringManager.GetString(type.FullName, toolStripItem);
                }
                else if (item is TabPage)
                {
                    TabPage tabPage = (TabPage)item;
                    tabPage.Text = StringManager.GetString(type.FullName, tabPage);
                }
                else if (item is Control)
                {
                    Control control = (Control)item;
                    if (control is DateTimePicker ||
                        control is TextBox ||
                        control is ComboBox ||
                        control is Panel ||
                        control is PictureBox ||
                        control is NumericUpDown||
                        control is Chart)
                        continue;

                    control.Text = StringManager.GetString(type.FullName, control);
                    //UiHelper.AutoFontSize(control);
                }
             
            }
        }

        private static StringTable GetStringTable(string tableName)
        {
            StringTable stringTable = StringTableList.Find(f => f.Name == tableName);
            return stringTable;
        }

        public static StringTable AddStringTable(StringTable stringTable)
        {
            lock(StringTableList)
                StringTableList.Add(stringTable);
            return stringTable;
        }
    }
}
