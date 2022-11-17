using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class ImageListForm : Form
    {
        public bool UseAutoTeach { get => this.useAutoTeach.Checked; }
        public string ResultPath { get => this.resultPath.Text; }
        public string ModelName { get; private set; }

        public ImageListForm(string modelName)
        {
            InitializeComponent();
            ModelName = modelName;

        }

        private void ImageListForm_Load(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();

            this.resultPath.Text = Path.Combine(PathSettings.Instance().Temp, "IterationTest", ModelName);
        }

        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length >= 1)
                {
                    List<FileInfo> fileInfoList = files.Select(f => new FileInfo(f)).ToList();
                    fileInfoList.ForEach(f => Add(f, true));
                }
            }
        }

        private void Add(FileInfo fileInfo, bool recursive)
        {
            string searchPattern = this.filterPattern.Text;
            string pat = "^" + Regex.Escape(searchPattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
            bool isMatch = Regex.IsMatch(Path.GetFileName(fileInfo.Name), pat);
            if (isMatch)
                listView1.Items.Add(new ListViewItem(fileInfo.FullName));

            if (recursive)
            {
                if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo dInfo = new DirectoryInfo(fileInfo.FullName);
                    FileInfo[] fileInfos = dInfo.GetFiles();
                    Array.ForEach(fileInfos, f => Add(f, true));
                }
            }
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; //포인터
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                int[] selIds = new int[this.listView1.SelectedIndices.Count];
                this.listView1.SelectedIndices.CopyTo(selIds, 0);

                selIds = selIds.Reverse().ToArray();
                Array.ForEach(selIds, f => this.listView1.Items.RemoveAt(f));
            }
        }

        internal FileInfo[] GetFiles()
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            System.Collections.IEnumerator enumerator = this.listView1.Items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ListViewItem listViewItem = (ListViewItem)enumerator.Current;
                fileInfoList.Add(new FileInfo(listViewItem.Text));
            }

            return fileInfoList.ToArray();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (!OpenPath())
                    return;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, ex.GetType().Name);
            }
        }

        private void buttonOpenResultPath_Click(object sender, EventArgs e)
        {
            OpenPath();
        }

        private bool OpenPath()
        {
            if (!Directory.Exists(ResultPath))
            {
                DialogResult dialogResult = MessageBox.Show(this, "Result path is not exist. Create new?", this.Text, MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.Cancel)
                    return false;

                Directory.CreateDirectory(ResultPath);
            }

            if (Directory.Exists(ResultPath))
                System.Diagnostics.Process.Start(ResultPath);
            return true;
        }
    }
}
