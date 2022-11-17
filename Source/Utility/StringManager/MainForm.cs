using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StringManager
{
    public partial class MainForm : Form
    {
        StringEditor stringEditor;

        TreeNode rootTreeNode = null;
        StringNode selectStringNode = null;

        public MainForm()
        {
            InitializeComponent();
            
            this.rootTreeNode = new TreeNode("Root");
            this.rootTreeNode.Name = rootTreeNode.Text;

            this.stringEditor = new StringEditor();

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml Files (*.xml)|*.xml";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            foreach (string fileName in dlg.FileNames)
                this.stringEditor.Load(fileName);

            treeView1.Nodes.Clear();
            TreeNode treeNode = GetTreeNode(stringEditor.RootNode);
            treeNode.Expand();
            treeView1.Nodes.Add(treeNode);

            //TreeNode searchNode = Search(this.rootNode, searchBar.Text);
            //treeView1.Nodes.Add(searchNode);
            //searchNode.Expand();
        }

        private TreeNode GetTreeNode(StringNode rootNode)
        {
            TreeNode treeNode = new TreeNode(rootNode.Key);
            foreach (StringNode subNode in rootNode)
            {
                TreeNode subTreeNode = GetTreeNode(subNode);
                    treeNode.Nodes.Add(subTreeNode);
            }

            treeNode.Tag = rootNode;
            return treeNode;
        }

        private TreeNode GetTreeNode(StringNode rootNode, string search)
        {
            TreeNode treeNode = new TreeNode(rootNode.Key);
            foreach (StringNode subNode in rootNode)
            {
                TreeNode subTreeNode = GetTreeNode(subNode, search);
                if (subTreeNode!=null)
                    treeNode.Nodes.Add(subTreeNode);
            }

            treeNode.Tag = rootNode;
            
            if (treeNode.Nodes.Count > 0 || treeNode.Text.Contains(search))
                return treeNode;
            return null;
        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Xml Files (*.xml)|*.xml";
            //if (dlg.ShowDialog() != DialogResult.OK)
            //    return;

            //string filePath = dlg.FileName;
            //string fileName = Path.GetFileNameWithoutExtension(filePath);
            //string locale = fileName.Split('_').Last().Substring(0, 5);

            string savePath = @".\Language";
            Directory.CreateDirectory(savePath);

            this.stringEditor.Save(savePath);
        }
        
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadForm loadForm = new LoadForm();
            DialogResult dialogResult= loadForm.ShowDialog(this);
            if (dialogResult == DialogResult.Cancel)
                return;

            this.treeView1.Nodes.Clear();
            this.treeView2.Nodes.Clear();
            this.treeView3.Nodes.Clear();

            List<StringTable> refStringTableList = loadForm.RefStringTableList;
            List<StringTable> comStringTableList = loadForm.ComStringTableList;
            List<StringTable> mergedStringTableList = Merge(refStringTableList, comStringTableList);
            //UpdateList(refStringTableList, comStringTableList, mergedStringTableList);
        }

        private List<StringTable> Merge(List<StringTable> refStringTableList, List<StringTable> comStringTableList)
        {
            List<StringTable> stringTableList = new List<StringTable>();

            List<StringTable>.Enumerator ptrA = refStringTableList.GetEnumerator();
            List<StringTable>.Enumerator ptrB = comStringTableList.GetEnumerator();

            TreeNode rootNodeA = new TreeNode() { Tag = ptrA };
            TreeNode rootNodeB = new TreeNode() { Tag = ptrB };
            TreeNode rootNodeC = new TreeNode();

            InitTreeNode(rootNodeA, rootNodeB, rootNodeC);

            this.treeView1.Nodes.Add(rootNodeA);
            this.treeView2.Nodes.Add(rootNodeB);
            this.treeView3.Nodes.Add(rootNodeC);
            
            return stringTableList;
        }

        private void InitTreeNode(TreeNode nodeA, TreeNode nodeB, TreeNode nodeC)
        {
            int c = ddd<StringTable>(nodeA, nodeB, nodeC,
                (IEnumerator<StringTable>)nodeA.Tag, (IEnumerator<StringTable>)nodeB.Tag,
                new Func<StringTable, IComparable>(f => f.Name));

            for (int i = 0; i < c; i++)
            {
                StringTable tA = (StringTable)nodeA.Nodes[i].Tag;
                StringTable tB = (StringTable)nodeB.Nodes[i].Tag;
                IEnumerator<KeyValuePair<string, string>> ptrA = tA.GetEnumerator();
                IEnumerator<KeyValuePair<string, string>> ptrB = tB.GetEnumerator();

                int cc = ddd<KeyValuePair<string, string>>(nodeA.Nodes[i], nodeB.Nodes[i], nodeC.Nodes[i],
                    ptrA, ptrB,
                    new Func<KeyValuePair<string, string>, IComparable>(f => f.Key));

                for (int j = 0; j < cc; j++)
                {
                    KeyValuePair<string, string> ttA = (KeyValuePair<string, string>)nodeA.Nodes[i].Nodes[j].Tag;
                    KeyValuePair<string, string> ttB = (KeyValuePair<string, string>)nodeB.Nodes[i].Nodes[j].Tag;

                    nodeA.Nodes[i].Nodes[j].Nodes.Add(ttA.Value);
                    nodeB.Nodes[i].Nodes[j].Nodes.Add(ttB.Value);

                    string mergeString;
                    Color mergeColor;
                    if (string.IsNullOrEmpty(ttB.Value))
                    {
                        if (string.IsNullOrEmpty(ttA.Value))
                        {
                            mergeString = ttA.Key;
                            mergeColor = Color.Blue;
                        }
                        else
                        {
                            mergeString = ttA.Value;
                            mergeColor = Color.Blue;
                        }
                    }
                    else
                    {
                        mergeString = ttB.Value;
                        mergeColor = Color.Red;
                    }
                    TreeNode node = new TreeNode()
                    {
                        Text = mergeString,
                        ForeColor = mergeColor,
                        Tag = mergeString
                    };
                    nodeC.Nodes[i].Nodes[j].Nodes.Add(node);
                }
            }
        }

        private int ddd<T>(TreeNode rootNodeA, TreeNode rootNodeB, TreeNode rootNodeC,
            IEnumerator<T> ptrA, IEnumerator<T> ptrB, Func<T, IComparable> keySelector)
            where T : new()
        {
            bool boolA = ptrA.MoveNext();
            bool boolB = ptrB.MoveNext();
            while (boolA || boolB)
            {
                T t1 = ptrA.Current;
                T t2 = ptrB.Current;
                T t3 = new T();

                TreeNode nodeA = new TreeNode() { };
                TreeNode nodeB = new TreeNode() { };
                TreeNode nodeC = new TreeNode() { ContextMenuStrip = this.contextMenuStrip };

                int comp = boolA ? boolB ? keySelector(t1).CompareTo(keySelector(t2)) : -1 : 1;
                if (comp < 0)
                {
                    nodeA.Text = keySelector(t1).ToString();
                    nodeA.ForeColor = Color.Yellow;
                    nodeA.BackColor = Color.Blue;
                    nodeA.Tag = t1;

                    nodeB.Text = "---";
                    nodeB.ForeColor = Color.Yellow;
                    nodeB.BackColor = Color.Red;
                    nodeB.Tag = new T();

                    nodeC.Text = keySelector(t1).ToString();
                    nodeC.ForeColor = Color.Yellow;
                    nodeC.BackColor = Color.Blue;
                    nodeC.Tag = t3;

                    boolA = ptrA.MoveNext();
                }
                else if (comp > 0)
                {
                    nodeA.Text = "---";
                    nodeA.ForeColor = Color.Yellow;
                    nodeA.BackColor = Color.Blue;
                    nodeA.Tag = new T();

                    nodeB.Text = keySelector(t2).ToString();
                    nodeB.ForeColor = Color.Yellow;
                    nodeB.BackColor = Color.Red;
                    nodeB.Tag = t2;

                    nodeC.Text = keySelector(t2).ToString();
                    nodeC.ForeColor = Color.Yellow;
                    nodeC.BackColor = Color.Red;
                    nodeC.Tag = t3;

                    boolB = ptrB.MoveNext();
                }
                else
                {
                    nodeA.Text = keySelector(t1).ToString();
                    //nodeA.BackColor = Color.Black;
                    nodeA.Tag = t1;

                    nodeB.Text = keySelector(t2).ToString();
                    //nodeB.BackColor = Color.Black;
                    nodeB.Tag = t2;

                    nodeC.Text = keySelector(t2).ToString();
                    //nodeC.BackColor = Color.Black;
                    nodeC.Tag = t3;

                    boolA = ptrA.MoveNext();
                    boolB = ptrB.MoveNext();
                }


                rootNodeA.Nodes.Add(nodeA);
                rootNodeB.Nodes.Add(nodeB);
                rootNodeC.Nodes.Add(nodeC);
            }

            return Math.Min(rootNodeA.Nodes.Count, Math.Min(rootNodeB.Nodes.Count, rootNodeC.Nodes.Count));
        }

        private void UpdateList(List<StringTable> refStringTableList, List<StringTable> comStringTableList)
        {
            this.treeView1.Nodes.Clear();
            this.treeView2.Nodes.Clear();
            this.treeView3.Nodes.Clear();
            throw new NotImplementedException();
        }

        private TreeNode[] GetSameNode(TreeNode node)
        {
            List<int> indexList = new List<int>();
            while (node != null)
            {
                indexList.Insert(0, node.Index);
                node = node.Parent;
            }

            TreeNode[] nodes = new TreeNode[3];
            indexList.ForEach(f =>
            {
                nodes[0] = nodes[0] == null ? this.treeView1.Nodes[f] : nodes[0].Nodes[f];
                nodes[1] = nodes[1] == null ? this.treeView2.Nodes[f] : nodes[1].Nodes[f];
                nodes[2] = nodes[2] == null ? this.treeView3.Nodes[f] : nodes[2].Nodes[f];
            });
            return nodes;
        }

        bool onSelect = false;

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (onSelect)
                return;

            onSelect = true;
            TreeNode[] nodes = GetSameNode(e.Node);
            this.treeView1.SelectedNode = nodes[0];
            this.treeView2.SelectedNode = nodes[1];
            this.treeView3.SelectedNode = nodes[2];

            onSelect = false;   
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (onSelect)
                return;

            onSelect = true;
            TreeNode[] nodes = GetSameNode(e.Node);
            Array.ForEach(nodes, f => f.Expand());
            onSelect = false;
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (onSelect)
                return;

            onSelect = true;
            TreeNode[] nodes = GetSameNode(e.Node);
            Array.ForEach(nodes, f => f.Collapse());
            onSelect = false;
        }

        private void treeView3_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (onSelect)
                return;

            string curString = e.Node.Tag as string;
            if (curString != null)
            {
                onSelect = true;
                MessageBox.Show(string.Format("{0}", curString));
                onSelect = false;
            }
        }

        private void useLeftDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = this.treeView3.SelectedNode;
            TreeNode[] nodes = GetSameNode(node);

            //if(node.Tag is KeyValuePair<string,string>)
            //{
            //    KeyValuePair<string, string> from = (KeyValuePair<string, string>)nodes[0].Tag;
            //    node.Tag = from;
            //    node.Name = from.Key;
            //    //node.Update();
            //}
            //else if(node.Tag is StringTable)
            //{
            //    StringTable to = (StringTable)node.Tag;
            //    StringTable from = (StringTable)nodes[0].Tag;

            //    to.CopyFrom(from);
            //    node.Update()
            //}

        }

        private void useRightDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = (TreeNode)sender;
            TreeNode[] nodes = GetSameNode(node);
        }

        private void UpdateNode(TreeNode node)
        {
            if (node.Tag is StringTable)
            {
                StringTable stringTable = (StringTable)node.Tag;
                node.Text = stringTable.Name;

                node.Nodes.Clear();
                Dictionary<string, string>.Enumerator enumerator = stringTable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    TreeNode tn = new TreeNode();
                    node.Nodes.Add(tn);
                    tn.Tag = enumerator.Current;
                    UpdateNode(tn);
                }
            }

            if (node.Tag is KeyValuePair<string, string>)
            {
                KeyValuePair<string, string> pair = (KeyValuePair<string, string>)node.Tag;
                node.Text = pair.Key;

                TreeNode tn = new TreeNode();
                node.Nodes.Add(tn);
                tn.Tag = pair.Value;
                UpdateNode(tn);
            }

            if (node.Tag is string)
            {
                string str = (string)node.Tag;
                node.Text = str;
            }
        }
    }
}
