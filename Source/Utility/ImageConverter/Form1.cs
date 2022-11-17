using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageConverter
{
    public partial class Form1 : Form
    {
        string Filter = "Images(bmp.png))|*.bmp;*.png|BMP|*.bmp|PNG|*.png|ALL(*)|*.*";
        string[] Extensions = new string[] { ".bmp", ".png" };

        BindingSource bs = null;
        CancellationTokenSource cancellationTokenSource = null;
        RadioButton[] radioButtons = null;

        public Form1(string[] argumentFiles)
        {
            InitializeComponent();

            this.radioButtons = new RadioButton[] { this.radioPNG, this.radioBMP, this.radioJPG };
            this.radioPNG.Tag = ".png";
            this.radioBMP.Tag = ".bmp";
            this.radioJPG.Tag = ".jpg";

            bs = new ImageConverter.BindingSource();

            Array.ForEach(argumentFiles, f =>
            {
                AddFile(new FileInfo(f));
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Converter.OnProgressChanged += Converter_OnProgressChanged;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "DisplayPath", HeaderText = "DisplayPath", AutoSizeMode= DataGridViewAutoSizeColumnMode.Fill });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "State", HeaderText = "State", AutoSizeMode= DataGridViewAutoSizeColumnMode.AllCells });
            dataGridView1.DataSource = bs;

            UpdateEnable();
        }

        private void Converter_OnProgressChanged()
        {
            if(this.InvokeRequired)
            {
                Invoke(new MethodInvoker(Converter_OnProgressChanged));
                return;
            }

            this.toolStripProgressBar1.Value = this.toolStripProgressBar1.Value + 1;
            //this.bs.ResetBindings(true);
        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = Filter;
            if(openFileDialog.ShowDialog() == DialogResult.OK)
                Array.ForEach(openFileDialog.FileNames, f => AddFile(new FileInfo(f)));
        }

        private void buttonAddFolder_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            //Console.WriteLine("dataGridView1_DragEnter");
            e.Effect = DragDropEffects.Copy;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine("dataGridView1_DragDrop");
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                List<string> fileList = new List<string>((string[])e.Data.GetData(DataFormats.FileDrop));
                Add(fileList.ToArray());
            }
        }

        private void Add(string[] files)
        {
            Array.ForEach(files, f =>
            {
                FileAttributes attr = File.GetAttributes(f);
                if (attr.HasFlag(FileAttributes.Directory))
                    AddFolder(new DirectoryInfo(f));
                else
                    AddFile(new FileInfo(f));
            });
        }

        private void AddFolder(DirectoryInfo directoryInfo)
        {
            if (!directoryInfo.Exists)
                return;

            FileInfo[] fileInfos = directoryInfo.GetFiles();
            Array.ForEach(fileInfos, f => AddFile(f));

            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            Array.ForEach(subDirectoryInfos, f => AddFolder(f));
        }

        private void AddFile(FileInfo fileInfo)
        {
            if (Converter.IsRunning)
                return;

            if (!Extensions.Contains(fileInfo.Extension.ToLower()))
                return;

            bs.Add(new PathState(fileInfo.FullName));
            this.toolStripStatusLabel1.Text = bs.CommonPath;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.cancellationTokenSource == null)
            // 시작
            {
                ConverterParam converterParam = new ConverterParam()
                {
                    TargetExt = (string)(this.radioButtons.First(f => f.Checked).Tag),
                    MakeThumbnail = this.checkThumbnail.Checked,
                    ThumbnailSize = (int)this.thumbnailSize.Value,
                    DeleteOrigin = this.checkBox1.Checked
                };
                this.cancellationTokenSource = new CancellationTokenSource();

                this.toolStripProgressBar1.Minimum = 0;
                this.toolStripProgressBar1.Maximum = this.bs.List.Count;
                this.toolStripProgressBar1.Value = 0;

                Converter.Run(this.bs.List.ToList(), converterParam, this.cancellationTokenSource);
            }
            else
            {
                if (Converter.IsRunning)
                // 종료
                {
                    if (!this.cancellationTokenSource.IsCancellationRequested)
                    {
                        if (MessageBox.Show(this, "Cancel?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            this.cancellationTokenSource.Cancel();
                    }
                }
                else
                // 목록 정리
                {
                    List<PathState> list = this.bs.List.ToList();
                    this.bs.Clear();
                    list.ForEach(f =>
                    {
                        if (!f.IsDone)
                        {
                            f.SetState(ConvertState.Idle);
                            bs.Add(f);
                        }
                    });
                    this.cancellationTokenSource = null;
                }
            }

            this.buttonStart.Update();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || this.bs.List.Count <= e.RowIndex)
                return;

            string path = System.IO.Path.GetDirectoryName(this.bs.List[e.RowIndex].FullPath);
            System.Diagnostics.Process.Start(path);
        }

        private void checkThumbnail_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnable();
        }

        private void radio_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnable();
            //RadioButton radioButton = (RadioButton)sender;
            //checkThumbnail.Enabled = !radioButton.Tag.Equals(".jpg");
            //if (!checkThumbnail.Enabled)
            //    checkThumbnail.Checked = false;
        }

        private void UpdateEnable()
        {
            radioJPG.Enabled = !checkThumbnail.Checked;
            if (!radioJPG.Enabled)
                radioJPG.Checked = false;

            checkThumbnail.Enabled = !radioJPG.Checked;
            if (!checkThumbnail.Enabled)
                checkThumbnail.Checked = false;
        }
    }
}
