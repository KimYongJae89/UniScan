using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace WpfControlLibrary.Helper
{
    public static class LocalizeHelper
    {
        static string xmlFilePath = "";
        public static List<StringTable> stringTableList = new List<StringTable>();
        public static List<IMultiLanguageSupport> multiLanguageSupportList = new List<IMultiLanguageSupport>();
        public static StringTable messageTable;

        public static void Load(string path)
        {

#if DEBUG
            string fileName = Path.GetFileName(path);
            string[] debugConfigPath = new string[]
            {
                @"U:\Project_UniScan\UniScan\Runtime\Config",
                @"D:\Project_UniScan\UniScan\Runtime\Config"
            };

            string[] debugStringTablepath = debugConfigPath.Select(f => Path.Combine(f, fileName)).ToArray();
            string exist = debugStringTablepath.FirstOrDefault(f => File.Exists(f));
            if (!string.IsNullOrEmpty(exist))
                path = exist;
#endif

            xmlFilePath = path;

            if (System.IO.File.Exists(path) == false)
                return;

            XmlDocument xmldocument = new XmlDocument();
            try
            {
                xmldocument.Load(path);
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("StringManager::Load - {0}", ex.Message));
                //MessageForm.Show(null, "StringTable load Fail!");
                return;
            }
#endif
            finally { }

            XmlElement rootElement = xmldocument.DocumentElement;
            if (rootElement.Name == "LocalizeHelper")
            {
                lock (stringTableList)
                {
                    stringTableList.Clear();
                    foreach (XmlElement stringElment in rootElement)
                    {
                        StringTable st = new StringTable(stringElment.Name);
                        st.LoadXml(stringElment);
                        stringTableList.Add(st);

                        if (stringElment.Name == "Message")
                            messageTable = st;
                    }

                    if (messageTable == null)
                    {
                        messageTable = new StringTable("Message");
                        stringTableList.Add(messageTable);
                    }
                }

            }
            multiLanguageSupportList.ForEach(f => f.UpdateLanguage());
        }

        public static void Save(string path = null)
        {
            if (string.IsNullOrEmpty(path))
                path = xmlFilePath;

            XmlDocument xmldocument = new XmlDocument();
            XmlElement rootElement = xmldocument.CreateElement("", "LocalizeHelper", "");
            xmldocument.AppendChild(rootElement);

            stringTableList.Sort((f, g) => f.Name.CompareTo(g.Name));
            foreach (StringTable stringTable in stringTableList)
            {
                XmlElement stringTableElement = rootElement.OwnerDocument.CreateElement("", stringTable.Name, "");
                rootElement.AppendChild(stringTableElement);

                stringTable.SaveXml(stringTableElement);
            }

            try
            {
                if (string.IsNullOrEmpty(path) == false)
                    xmldocument.Save(path);
            }
            catch (Exception)
            { }
        }

        public static void AddListener(IMultiLanguageSupport item)
        {
            lock (multiLanguageSupportList)
                multiLanguageSupportList.Add(item);

            item.UpdateLanguage();
        }

        public static void RemoveListener(IMultiLanguageSupport item)
        {
            lock (multiLanguageSupportList)
                multiLanguageSupportList.Remove(item);
        }

        public static void Clear()
        {
            stringTableList.Clear();
        }

        public static string GetString(string searchKey)
        {
            if (messageTable == null)
                return searchKey;

            if (messageTable.IsExist(searchKey))
            {
                return messageTable.GetString(searchKey);
            }
            else
            {
                messageTable.AddString(searchKey, searchKey);
                Save();
                return searchKey;
            }
        }

        private static string GetString(object obj, string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey))
                return "";

            return GetString(obj.GetType().Name, searchKey);
        }

        public static string GetString(string sectionName, string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey))
                return "";

            StringTable stringTable = GetStringTable(sectionName);
            if (stringTable == null)
            {
                stringTable = new StringTable(sectionName);
                stringTableList.Add(stringTable);
            }

            if (stringTable.IsExist(searchKey))
            {
                return stringTable.GetString(searchKey);
            }
            else
            {
                stringTable.AddString(searchKey, searchKey);
                Save();
                return searchKey;
            }
        }

        public static void FindVisualChildren(Visual depObj, List<object> objectList)
        {
            if (depObj == null)
                return;

            if (depObj is Visual == false)
                return;

            foreach (object obj in LogicalTreeHelper.GetChildren(depObj))
            {
                if (obj != null)
                {
                    objectList.Add(obj);

                    if (obj is Visual)
                        FindVisualChildren((Visual)obj, objectList);
                }
            }
        }

        public static void UpdateString(IMultiLanguageSupport target)
        {
            List<object> objectList = new List<object>();

            FindVisualChildren(target as Visual, objectList);

            foreach (object obj in objectList)
            {
                if (obj is ContentControl)
                {
                    ContentControl contentControl = obj as ContentControl;

                    if (contentControl.Tag is String)
                        contentControl.Content = GetString(target.GetType().FullName, contentControl.Tag.ToString());
                    else if (contentControl.Content is String)
                        contentControl.Content = GetString(target.GetType().FullName, contentControl.Content.ToString());
                }
                else if (obj is TextBlock)
                {
                    TextBlock textBlock = obj as TextBlock;

                    if (!string.IsNullOrEmpty(textBlock.Text))
                        textBlock.Text = GetString(target.GetType().FullName, textBlock.Text);
                    //else
                    //textBlock.Text = GetString(target.GetType().FullName, textBlock.Name);
                }
                else if(obj is DataGrid)
                {
                    DataGrid dataGrid = (DataGrid)obj;
                    string sectionName = string.IsNullOrEmpty(dataGrid.Name) ? target.GetType().FullName : string.Format("{0}.{1}", target.GetType().FullName, dataGrid.Name);
                    foreach (DataGridColumn dataGridColumn in  dataGrid.Columns)
                    {
                        if(dataGridColumn.Header is string)
                            dataGridColumn.Header = GetString(sectionName, dataGridColumn.Header.ToString());
                    }
                }
            }

            //Type type = obj.GetType();
            //FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            //foreach (FieldInfo fieldInfo in fieldInfos)
            //{
            //    object item = (object)fieldInfo.GetValue(obj);

            //    //if (item is DataGridViewColumn)
            //    //{
            //    //    DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)item;
            //    //    //dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn.Name, dataGridViewColumn.HeaderText);
            //    //    dataGridViewColumn.HeaderText = StringManager.GetString(type.FullName, dataGridViewColumn);
            //    //}
            //    //else if (item is Control)
            //    //{
            //    //    Control control = (Control)item;
            //    //    //control.Text = StringManager.GetString(type.FullName, control.Name, control.Text);
            //    //    control.Text = StringManager.GetString(type.FullName, control);
            //    //    //UiHelper.AutoFontSize(control);
            //    //}
            //    //else if (item is ToolStripItem)
            //    //{
            //    //    ToolStripItem toolStripItem = (ToolStripItem)item;
            //    //    //toolStrip.Text = StringManager.GetString(type.FullName, toolStrip.Name, toolStrip.Text);
            //    //    toolStripItem.Text = StringManager.GetString(type.FullName, toolStripItem);
            //    //}
            //}
        }

        private static StringTable GetStringTable(string tableName)
        {
            StringTable stringTable = stringTableList.Find(f => f.Name == tableName);
            return stringTable;
        }
    }
}
