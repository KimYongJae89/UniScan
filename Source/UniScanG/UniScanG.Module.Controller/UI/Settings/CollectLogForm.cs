using DynMvp.Base;
using DynMvp.UI.Touch;
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
using UniScanG.Common.Data;

namespace UniScanG.Module.Controller.UI.Settings
{
    public partial class CollectLogForm : Form, IMultiLanguageSupport
    {
        public class _Item
        {
            public bool Use { get; set; }
            public string Name { get; set; }
            public int Camera { get; set; }
            public int Client { get; set; }
            public string Path { get; set; }
            public string Size { get; set; }
        }

        public CollectLogForm()
        {
            InitializeComponent();
            InitializeDataGrid();
            StringManager.AddListener(this);

            comboBoxDays.Items.AddRange(new string[] { "1", "3", "5", "7", "14", "21", "28", "All" });
            comboBoxDays.SelectedIndex = 1;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void InitializeDataGrid()
        {
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { DataPropertyName = "Use", HeaderText = "Use", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Camera", HeaderText = "Camera", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Client", HeaderText = "Client", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Path", HeaderText = "Path", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Size", HeaderText = "Size", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ReadOnly = true });
        }

        private void CollectLogForm_Load(object sender, EventArgs e)
        {
            List<InspectorObj> inspectorObjList = ((Common.IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspectorList();

            List<_Item> itemList = inspectorObjList.Select(f =>
            {
                return new _Item()
                {
                    Use = true,
                    Name = f.Info.GetName(),
                    Camera = f.Info.CamIndex,
                    Client = f.Info.ClientIndex,
                    Path = Path.Combine(f.Info.Path, "Log")
                };
            }).ToList();

            itemList.Insert(0, new _Item() { Use = true, Name = "CM", Camera = 0, Client = 0, Path = UniEye.Base.Settings.PathSettings.Instance().Log });

            this.dataGridView1.DataSource = itemList;
        }

        private FileInfo[] GetFileInfos(DirectoryInfo directoryInfo, int days)
        {
            if (!directoryInfo.Exists)
                return new FileInfo[0];

            List<FileInfo> fInfoList = directoryInfo.GetFiles().ToList();
            fInfoList.RemoveAll(f =>
            {
                string ext = f.Extension;
                if (DateTime.TryParseExact(ext, ".yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime datetime))
                {
                    TimeSpan timeSpan = DateTime.Today - datetime;
                    if (timeSpan.Days >= days)
                        return true;
                }
                return false;
            });

            return fInfoList.ToArray();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBoxDays.Text, out int days))
                return;

            FolderBrowserDialog dlg = new FolderBrowserDialog()
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                //SelectedPath = @"D:\새 폴더",
                ShowNewFolderButton = true,
                Description = "Select Folder",
            };

            if (dlg.ShowDialog(this) == DialogResult.Cancel)
                return;

            // Target
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            DirectoryInfo targetInfo = new DirectoryInfo(Path.Combine(dlg.SelectedPath, $"CollectLog_{SystemInformation.ComputerName}_{DateTime.Now.ToString("yyyy_MM_dd")}"));
            if (!targetInfo.Exists)
                targetInfo.Create();

            // Source
            List<_Item> list = (List<_Item>)this.dataGridView1.DataSource;
            list.RemoveAll(f => !f.Use);
            Tuple<string, string>[] tuples = list.Select(f => new Tuple<string, string>(f.Path, Path.Combine(targetInfo.FullName, f.Name))).ToArray();

            new SimpleProgressForm("Copying...").Show(this, () =>
            {
                try
                {
                    //list.ForEach(f =>
                    tuples.AsParallel().ForAll(f =>
                    {
                        DirectoryInfo src = new DirectoryInfo(f.Item1);
                        FileInfo[] fileInfos = GetFileInfos(src, days);
                        if (!Directory.Exists(f.Item2))
                            Directory.CreateDirectory(f.Item2);

                        Array.ForEach(fileInfos, g =>
                        {
                            if (cancellationTokenSource.IsCancellationRequested)
                                return;

                            try
                            {

                                File.Copy(g.FullName, Path.Combine(f.Item2, g.Name));
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Error(LoggerType.Error, $"CollectLogForm - {ex.GetType().Name}: {ex.Message}");
                            }
                        });
                    });
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { LogHelper.Error(LoggerType.Error, $"CollectLogForm - {ex.GetType().Name}: {ex.Message}"); }
            });

            if (!cancellationTokenSource.IsCancellationRequested)
            {
                new SimpleProgressForm("Compressing...").Show(() =>
                {
                    try
                    {
                        FileInfo zipInfo = new FileInfo($"{targetInfo.FullName}.zip");
                        FileHelper.CompressZip(targetInfo, zipInfo, cancellationTokenSource);

                        FileInfo zipInfo2 = new FileInfo(Path.Combine(targetInfo.FullName, $"{targetInfo.Name}.zip"));
                        FileHelper.Move(zipInfo.FullName, zipInfo2.FullName);

                    }
                    catch (OperationCanceledException) { }
                    catch (Exception ex) { LogHelper.Error(LoggerType.Error, $"SettingPageExtender::CollectLog - Compress, {ex.GetType().Name}: {ex.Message}"); }
                }, cancellationTokenSource);
            }

            System.Diagnostics.Process.Start(targetInfo.FullName);
        }

        private void buttonUpdateSize_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(comboBoxDays.Text, out int days))
                return;

            new SimpleProgressForm("Update...").Show(this, () =>
            {
                try
                {
                    List<_Item> list = (List<_Item>)this.dataGridView1.DataSource;
                    list.AsParallel().ForAll(f =>
                    {
                        string size = "";
                        if (f.Use)
                        {
                            DirectoryInfo src = new DirectoryInfo(f.Path);
                            FileInfo[] fileInfos = GetFileInfos(src, days);
                            if (fileInfos.Length > 0)
                            {
                                double sumMB = fileInfos.Sum(g => g.Length / 1024.0 / 1024.0); // MB
                                double sumGB = sumMB / 1024;
                                if (sumGB > 1)
                                    size = $"{sumGB:F1}GB";
                                else
                                    size = $"{sumMB:F1}MB";
                            }
                        }
                        f.Size = size;
                    });
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { LogHelper.Error(LoggerType.Error, $"SettingPageExtender::CollectLog - Copy, {ex.GetType().Name}: {ex.Message}"); }
            });

            this.dataGridView1.Refresh();
        }
    }
}
