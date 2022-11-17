using DynMvp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringManager
{
    public class Item
    {
        public string Locale => this.locale;
        string locale;

        public string Value { get => this.value; set => this.value = value; }
        string value;

        public Item(string locale, string value)
        {
            this.locale = locale;
            this.value = value;
        }
    }

    public class StringNode : IEnumerable<StringNode>
    {
        public StringNode this[int i] => subNodeList[i];

        public StringNode Parent => this.parent;
        StringNode parent = null;

        public string Key => this.key;
        string key;

        public List<Item> LocValDic => locValDic;
        List<Item> locValDic;

        public List<StringNode> SubNodeList => subNodeList;
        List<StringNode> subNodeList;

        public StringNode(string key)
        {
            this.key = key;
            this.locValDic = new List<Item>();
            this.subNodeList = new List<StringNode>();
        }

        public IEnumerator<StringNode> GetEnumerator()
        {
            return subNodeList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return subNodeList.GetEnumerator();
        }

        public void Add(string locale, string value)
        {
            Item item = this.locValDic.Find(f => f.Locale == locale);
            if (item == null)
            {
                item = new Item(locale, "");
                this.locValDic.Add(item);
            }

            item.Value = value;
        }

        public StringNode Add(StringNode node)
        {
            this.subNodeList.Add(node);
            node.parent = this;
            return node;
        }

        public void Remove(StringNode node)
        {
            this.subNodeList.Remove(node);
            node.parent = null;
        }

        public StringNode Find(string key)
        {
            return this.subNodeList.Find(f => f.key == key);
        }
    }


    class StringEditor
    {
        public List<string> LocaleList => this.localeList;
        List<string> localeList;

        public StringNode RootNode => this.rootNode;
        StringNode rootNode;

        public StringEditor()
        {
            this.localeList = new List<string>();
            this.rootNode = new StringNode("ROOT");
        }

        internal bool Load(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string locale = name.Split('_').Last().Substring(0, 5);

            bool ok = DynMvp.Base.StringManager.Load(fileName);
            if (ok)
            {
                List<StringTable> newStringTableList = DynMvp.Base.StringManager.StringTableList;
                foreach (StringTable newStringTable in newStringTableList)
                {
                    string newTableName = newStringTable.Name;
                    StringNode stringNode = this.rootNode.Find(newTableName);
                    if (stringNode == null)
                    {
                        stringNode = new StringNode(newTableName);
                        this.rootNode.Add(stringNode);
                    }

                    AppendTable(stringNode, newStringTable, locale);
                }
                this.localeList.Add(locale);
                return true;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Load Fail");
                sb.AppendLine(string.Format("File: {0}", fileName));
                return false;
            }
        }

        internal bool Save(string savePath)
        {
            this.localeList.ForEach(f =>
            {
                List<StringTable> stringTableList = new List<StringTable>();
                this.rootNode.SubNodeList.ForEach(g =>
                {
                    StringTable stringTable = GetStringTable(g, f);
                    stringTableList.Add(stringTable);
                });

                DynMvp.Base.StringManager.Load(savePath, f);
                DynMvp.Base.StringManager.StringTableList = stringTableList;
                bool good = DynMvp.Base.StringManager.Save();
                if (good == false)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Save Fail - ");
                    sb.AppendLine(string.Format("Path: {0}", DynMvp.Base.StringManager.ConfigPath));
                    sb.AppendLine(string.Format("Locale: {0}", DynMvp.Base.StringManager.LocaleCode));
                    System.Windows.Forms.MessageBox.Show(sb.ToString());
                }
                DynMvp.Base.StringManager.Clear();
            });

            return true;
        }

        private StringTable GetStringTable(StringNode stringNode, string locale)
        {
            StringTable stringTable = new StringTable(stringNode.Key);
            foreach (StringNode subStringNode in stringNode.SubNodeList)
            {
                Item item = subStringNode.LocValDic.Find(f => f.Locale == locale);
                if (item != null)
                    stringTable.AddString(subStringNode.Key, item.Value);
                else
                    stringTable.AddString(subStringNode.Key, subStringNode.Key);
            }

            return stringTable;
        }

        private void AppendTable(StringNode stringNode, StringTable newStringTable, string locale)
        {
            Dictionary<string, string>.Enumerator dic = newStringTable.GetEnumerator();
            while (dic.MoveNext())
            {
                StringNode subNode = stringNode.Find(dic.Current.Key);
                if (subNode == null)
                {
                    subNode = new StringNode(dic.Current.Key);
                    stringNode.Add(subNode);
                }
                subNode.Add(locale, dic.Current.Value);
            };
        }
    }
}
