using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.UI.Report;

namespace ProductionRecover
{
    public partial class MainForm : Form
    {
        ProductionManagerG productionManager;
        BackgroundWorker backgroundWorker;
        List<Tuple<ProductionG, DataGridViewRow>> tupleList;

        bool onListUpdate = false;

        public MainForm()
        {
            InitializeComponent();
            
            this.productionManager = new ProductionManagerG("");
            this.backgroundWorker = new BackgroundWorker();
            this.tupleList = new List<Tuple<ProductionG, DataGridViewRow>>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string defaultPath = @"D:\UniScan\Gravure_Controller\Result";
            if (Directory.Exists(defaultPath))
            {
                this.xmlPath.Text = defaultPath;
                this.productionManager.Load(defaultPath, "ProductionList.xml");
            }
            UpdateFoundList(true);
        }

        private void browseXml_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = this.xmlPath.Text;
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                this.productionManager.Clear();

                this.xmlPath.Text = fbd.SelectedPath;
                string targetXml = Path.Combine(fbd.SelectedPath, "ProductionList.xml");
                if (!File.Exists(targetXml))
                {
                    MessageBox.Show(Properties.Resources.XmlNotExist);
                    return;
                }

                this.productionManager.Load(fbd.SelectedPath, "ProductionList.xml");
                UpdateFoundList(true);
            }
        }

        private delegate void UpdateFoundListDelegate(bool redrawList);
        private void UpdateFoundList(bool redrawList)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateFoundListDelegate(UpdateFoundList), redrawList);
                return;
            }

            labelScanCount.Text = string.Format("{0}", this.productionManager.List.Count);
            this.startDeepScan.Enabled = (this.productionManager.List.Count != 0);
            if (redrawList)
            {
                this.onListUpdate = true;
                foundGridView.Rows.Clear();
                this.tupleList.Clear();
                this.productionManager.List.ForEach(f =>
                {
                    ProductionG productionG = f as ProductionG;
                    if (productionG != null)
                    {
                        DataGridViewRow newRow = new DataGridViewRow();
                        newRow.CreateCells(foundGridView);
                        FillRow(newRow, productionG);
                        foundGridView.Rows.Add(newRow);
                        this.tupleList.Add(new Tuple<ProductionG, DataGridViewRow>(productionG, newRow));
                    }
                });
                this.foundGridView.Sort(this.columnDate, ListSortDirection.Ascending);
                this.onListUpdate = false;
            }
        }

        private void FillRow(DataGridViewRow row, ProductionG productionG)
        {
            row.Cells[0].Value = !productionG.UpdateRequire;
            row.Cells[1].Value = productionG.StartTime;
            row.Cells[2].Value = productionG.Name;
            row.Cells[3].Value = productionG.LotNo;
            row.Cells[4].Value = productionG.Done;
            row.Cells[5].Value = productionG.Patterns.NoPrint;
            row.Cells[6].Value = productionG.Patterns.PinHole;
            row.Cells[7].Value = productionG.Patterns.Spread;
            row.Cells[8].Value = productionG.Patterns.SheetAttack;
            row.Cells[9].Value = productionG.Patterns.Dielectric;
            row.Cells[10].Value = productionG.Patterns.Sticker;
            row.Cells[11].Value = productionG.Ng;
            row.Cells[12].Value = productionG.EraseNum;
            row.Cells[13].Value = productionG.SpecBlackUm;
            row.Cells[14].Value = productionG.SpecWhiteUm;
            row.Cells[15].Value = productionG.LineSpeedMpm;
        }

        private void UpdateFoundList(ProductionG productionG)
        {
            DataGridViewRow row = this.tupleList.Find(f => f.Item1.StartTime == productionG.StartTime && f.Item1 == productionG)?.Item2;
            if (row != null)
                FillRow(row, productionG);
        }

        private void startSearch_Click(object sender, EventArgs e)
        {
            StartSearch(false);         
        }

        private void startDeepScan_Click(object sender, EventArgs e)
        {
            StartSearch(true);
        }

        private void StartSearch(bool isDeepScan)
        {
            if (this.backgroundWorker.IsBusy)
            {
                if (this.backgroundWorker.CancellationPending)
                {
                    MessageBox.Show(this, Properties.Resources.ScanCancelling);
                    return;
                }

                if (MessageBox.Show(this, Properties.Resources.CancelScanQ, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    this.backgroundWorker.CancelAsync();
                return;
            }

            this.backgroundWorker = new BackgroundWorker();
            this.backgroundWorker.DoWork += BackgroundWorker_DoWork;
            this.backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            this.backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.RunWorkerAsync(new BgWorkerParam(this.xmlPath.Text, isDeepScan));

            LockButtons(false);
        }


        private void LockButtons(bool v)
        {
            xmlPath.Enabled = v;
            browseXml.Enabled = v;
            clearResult.Enabled = v;
            saveResult.Enabled = v;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty((string)e.UserState))
                toolStripStatusLabel1.Text = (string)e.UserState;
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backgroundWorker = (BackgroundWorker)sender;
            BgWorkerParam bgWorkerParam = (BgWorkerParam)e.Argument;
            backgroundWorker.ReportProgress(0, "Begin");

            if (!bgWorkerParam.IsDeepScan)
            {
                // 날짜, 모델, 두께, 페이스트, 로트_존 (or 로트)
                List<ProductionG> productionList = new List<ProductionG>();

                backgroundWorker.ReportProgress(0, "Searching");
                DirectoryInfo root = new DirectoryInfo(bgWorkerParam.WorkingPath);
                DirectoryInfo[] dateTimes = root.GetDirectories("??-??-??", SearchOption.TopDirectoryOnly);
                Array.ForEach(dateTimes, dateTime =>
                {
                    DateTime startDateTime = DateTime.ParseExact(dateTime.Name, "yy-MM-dd", null).AddDays(1).AddMilliseconds(-1);
                    DirectoryInfo[] models = dateTime.GetDirectories("*", SearchOption.TopDirectoryOnly);
                    Array.ForEach(models, model =>
                    {
                        DirectoryInfo[] thicknesses = model.GetDirectories("*", SearchOption.TopDirectoryOnly);
                        Array.ForEach(thicknesses, thickness =>
                        {
                            DirectoryInfo[] pastes = thickness.GetDirectories("*", SearchOption.TopDirectoryOnly);
                            Array.ForEach(pastes, paste =>
                            {
                                DirectoryInfo[] lots = paste.GetDirectories("*", SearchOption.TopDirectoryOnly);
                                Array.ForEach(lots, lot =>
                                {
                                    string lotNo = lot.Name;
                                    RewinderZone rewinderZone = RewinderZone.Unknowen;

                                    string[] tokens = lotNo.Split('_');
                                    if (tokens.Length > 1 && Enum.TryParse<RewinderZone>(tokens.Last(), out rewinderZone))
                                        lotNo = string.Join("_", tokens.Take(tokens.Length - 1));

                                    ProductionG productionG = new ProductionG(model.Name, startDateTime, lotNo, rewinderZone, float.Parse(thickness.Name), paste.Name, 0, 0);

                                    FileInfo[] fileInfo = lot.GetFiles("Production.xml");
                                    if (fileInfo.Length == 1)
                                        productionG.Load(fileInfo[0].FullName);
                                    else
                                        productionG.UpdateRequire = true;

                                    productionList.Add(productionG);
                                });
                            });
                        });
                    });
                });

                productionList.RemoveAll(f => this.productionManager.List.Contains(f));
                this.productionManager.List.AddRange(productionList);
            }
            else 
            {
                UpdateFoundList(true);

                List<ProductionBase> scanList =  this.productionManager.List.FindAll(f => (f is ProductionG) && ((ProductionG)f).UpdateRequire);
                int steps = scanList.Count;
                for (int i = 0; i < steps; i++)
                {
                    if (backgroundWorker.CancellationPending)
                        return;

                    ProductionG productionG = (ProductionG)scanList[i];
                    string productionPath = productionG.GetResultPath(bgWorkerParam.WorkingPath);
                    if (!Directory.Exists(productionPath))
                        continue;

                    List<MergeSheetResult> list = new List<MergeSheetResult>();
                    backgroundWorker.ReportProgress((int)(100f * i / steps), string.Format("{0} / {1}", (i + 1), steps));

                    ReportProgressForm reportProgressForm = new ReportProgressForm("", false);
                    reportProgressForm.Visible = false;
                    reportProgressForm.Show(this, ref productionPath, list);

                    SetValue(productionG, productionPath, list);
                    productionG.UpdateRequire = false;
                    UpdateFoundList(productionG);
                }
            }
        }

        private void SetValue(ProductionG productionG,string path, List<MergeSheetResult> list)
        {
            string[] teachData = new string[]
            {
                Path.Combine(path, "TeachData_C0.xml"),
                Path.Combine(path, "TeachData_C1.xml")
            };
            productionG.UpdateSpec(teachData);
            productionG.UpdateFrom(list);
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateFoundList(true);
            MessageBox.Show(Properties.Resources.ScanCompleted);
            LockButtons(true);
        }

        private void saveResult_Click(object sender, EventArgs e)
        {
            this.productionManager.Save(this.xmlPath.Text, "ProductionList_Restore.xml");
            System.Diagnostics.Process.Start(this.xmlPath.Text);
            MessageBox.Show(Properties.Resources.SaveDone);
        }

        private void clearResult_Click(object sender, EventArgs e)
        {
            this.onListUpdate = true;
            this.productionManager.Clear();
            this.tupleList.Clear();
            this.foundGridView.Rows.Clear();
            this.onListUpdate = false;
        }

        private void foundGridView_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (onListUpdate)
                return;

            this.productionManager.List.RemoveAt(e.RowIndex);
            UpdateFoundList(false);
        }

        private void ToolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            DataGridViewRow[] selectedRows = new DataGridViewRow[this.foundGridView.SelectedRows.Count];
            this.foundGridView.SelectedRows.CopyTo(selectedRows, 0);
            Array.ForEach(selectedRows, f => this.foundGridView.Rows.Remove(f));            
        }
    }
}
