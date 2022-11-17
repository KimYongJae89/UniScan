using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
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
using UniScanG.Data;
using UniScanG.UI.Etc;

namespace DataAnalyzer
{
    public partial class Form1 : Form
    {
        CanvasPanel canvasPanelFull = null;
        CanvasPanel canvasPanelSmall = null;

        string workingPath = "";
        int loadedCount = 0;
        List<MergeSheetResult> mergeSheetResultList = new List<MergeSheetResult>();
        TreeNode lastSelectedNode = null;

        Bitmap masterBitmapImage = null;
        Dictionary<RectangleF, List<Tuple<int, int>>> dictionary = new Dictionary<RectangleF, List<Tuple<int, int>>>();

        public Form1()
        {
            InitializeComponent();

            if (Directory.Exists(@"D:\그라비아 퀄\3225\20180806_0418"))
                this.workingPath = @"D:\그라비아 퀄\3225\20180806_0418";

            canvasPanelFull = new CanvasPanel();
            canvasPanelFull.Dock = DockStyle.Fill;
            canvasPanelFull.SetPanMode();
            canvasPanelFull.FigureClicked = canvasPanel_FigureClicked;
            panel4.Controls.Add(canvasPanelFull);

            canvasPanelSmall = new CanvasPanel();
            canvasPanelSmall.Dock = DockStyle.Fill;
            canvasPanelSmall.SetMeasureMode(FigureType.Rectangle);
            canvasPanelSmall.ShowCenterGuide = false;
            panel5.Controls.Add(canvasPanelSmall);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateControl(this.textBox1, this.workingPath);
            treeViewPosition.Nodes.Clear();
            canvasPanelFull.UpdateImage(null);
        }

        private void canvasPanel_FigureClicked(Figure figure, MouseEventArgs e)
        {
            if(figure.Tag is TreeNode)
            {
                TreeNode treeNodes = figure.Tag as TreeNode;

                if (lastSelectedNode == treeNodes)
                    treeNodes.Checked = true;
                else
                    this.treeViewPosition.SelectedNode = treeNodes;
                    lastSelectedNode = treeNodes;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = this.workingPath;
            if (dlg.ShowDialog() ==   DialogResult.OK)
            {
                this.workingPath = dlg.SelectedPath;
                UpdateControl(this.textBox1, this.workingPath);
                treeViewPosition.Nodes.Clear();
                canvasPanelFull.UpdateImage(null);
                canvasPanelFull.WorkingFigures.Clear();
                LoadData();
            }
        }

        bool onLoading = false;
        private void LoadData()
        {
            if (Directory.Exists(this.workingPath) == false)
            {
                MessageBox.Show("Not invalid Path");
                return;
            }

            UniScanG.UI.Etc.ProgressForm progressForm = new UniScanG.UI.Etc.ProgressForm();
            progressForm.StartPosition = FormStartPosition.CenterScreen;
            progressForm.TitleText = "Load Data";
            progressForm.MessageText = "";

            progressForm.BackgroundWorker.DoWork += LoadBackgroundWorker_DoWork;
            progressForm.RunWorkerCompleted = LoadRunWorkerCompleted;

            progressForm.TopMost = true;

            progressForm.Show(this);
        }

        private void LoadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.onLoading = true;
            BackgroundWorker backgroundWorker = sender as BackgroundWorker;
            mergeSheetResultList.Clear();
            dictionary.Clear();
            masterBitmapImage?.Dispose();
            masterBitmapImage = null;
            int importGood = 0, importFail = 0;
            string[] subDirectorys = Directory.GetDirectories(this.workingPath);
            subDirectorys = SortDirectory(subDirectorys);
            int sheetIdx = -1;
            for (int i = 0; i < subDirectorys.Length; i++)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                string subDirectory = subDirectorys[i];
                string dirName = Path.GetFileName(subDirectory);
                bool ok = int.TryParse(dirName, out sheetIdx);
                if (ok == false)
                    sheetIdx++;

                MergeSheetResult mergeSheetResult = new MergeSheetResult(sheetIdx, 0, subDirectory, false);
                bool import = mergeSheetResult.Import(subDirectory);
                if (import)
                {
                    importGood++;
                    mergeSheetResultList.Add(mergeSheetResult);

                    if (masterBitmapImage == null)
                        masterBitmapImage = mergeSheetResult.PrevImage;

                    foreach (FoundedObjInPattern sheetSubResult in mergeSheetResult.SheetSubResultList)
                    {
                        RectangleF region = new Rectangle((int)sheetSubResult.Region.X, (int)sheetSubResult.Region.Y, (int)sheetSubResult.Region.Width, (int)sheetSubResult.Region.Height);
                        RectangleF key = dictionary.Keys.FirstOrDefault(f => GetOverlapScore(f, region, 100) > 0.7);
                        if (key.IsEmpty)
                        {
                            key = region;
                            dictionary.Add(key, new List<Tuple<int, int>>());
                        }

                        dictionary[key].Add(new Tuple<int, int>(sheetIdx, sheetSubResult.Index * 10 + sheetSubResult.CamIndex));
                    }
                }
                else
                {
                    importFail++;
                }
                backgroundWorker.ReportProgress(i * 100 / subDirectorys.Length, "Load Data");
            }
            this.loadedCount = importGood;
        }

        private string[] SortDirectory(string[] subDirectorys)
        {
            List<int> sheetNoList = new List<int>();
            Array.ForEach(subDirectorys, f =>
            {
                int sheetNo;
                bool ok = int.TryParse(Path.GetFileName(f), out sheetNo);
                if (ok)
                {
                    sheetNoList.Add(sheetNo);
                }
            });
            sheetNoList.Sort();

            List<string> directroyName = new List<string>();
            sheetNoList.ForEach(f => directroyName.Add(Path.Combine(this.workingPath, f.ToString())));
            if (directroyName.Count == 0)
                return subDirectorys;
            return directroyName.ToArray();
        }

        private double GetOverlapScore(RectangleF a, RectangleF b, float intflate)
        {
            a.Inflate(intflate, intflate);
            b.Inflate(intflate, intflate);
            float totalSize = a.Width * a.Height + b.Width * b.Height;
            RectangleF overlap = RectangleF.Intersect(a, b);
            float overlapSize = overlap.Width * overlap.Height;
            return overlapSize * 2 / totalSize;
        }

        private delegate void UpdateControlDelegate(Control control, string text);
        private void UpdateControl(Control control, string text)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), control, text);
                return;
            }

            control.Text = text;
        }

        private void LoadRunWorkerCompleted(object result)
        {
            // Update Image
            canvasPanelFull.UpdateImage(this.masterBitmapImage);
            canvasPanelFull.ZoomFit();

            UpdatePositionTreeview();
            UpdateSheetListView();
            this.onLoading = false;
        }

        private void UpdateSheetListView()
        {
            UpdateFlag(true);
            int sheetNoMin = dictionary.Min(f => f.Value.Min(g => g.Item1));
            int sheetNoMax = dictionary.Max(f => f.Value.Max(g => g.Item1));
            for (int i = sheetNoMin; i < sheetNoMax; i++)
                this.listBox1.Items.Add(i);
            UpdateFlag(false);
        }

        private void UpdatePositionTreeview()
        {
            UpdateFlag(true);
            treeViewPosition.Nodes.Clear();

            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() =>
            {
                List<Figure> figureList = new List<Figure>();


                List<RectangleF> keyList = dictionary.Keys.ToList();
                foreach (RectangleF key in keyList)
                {
                    Figure figure = GetFigure(key);
                    figureList.Add(figure);
                    
                    TreeNode treeNode = AddTreeView(treeViewPosition, key);
                    treeNode.Tag = figure;
                    figure.Tag = treeNode;
                }

                canvasPanelFull.WorkingFigures.Clear();
                canvasPanelFull.WorkingFigures.AddFigure(figureList.ToArray());

                UpdateControl(this.subResultCnt, mergeSheetResultList.Count.ToString());
            });

            UpdateFlag(false);
            lastSelectedNode = null;
        }

        private Color GetColor(Rectangle key)
        {
            List<Tuple<int, int>> childList = dictionary[key];
            if (childList.Count == this.loadedCount)
                return Color.Red;
            else if(childList.Count > this.loadedCount*0.5)
                return Color.Blue;
            return Color.Yellow;
        }

        private delegate void UpdateFlagDelegate(bool onUpdate);
        private void UpdateFlag(bool onUpdate)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateFlagDelegate(UpdateFlag), onUpdate);
                return;
            }

            if (onUpdate)
            {
                treeViewPosition.BeginUpdate();
                listBox1.BeginUpdate();
            }
            else
            {
                treeViewPosition.EndUpdate();
                listBox1.EndUpdate();
            }
        }

        private TreeNode AddTreeView(TreeView treeView,RectangleF key)
        {
            TreeNode parentNode = new TreeNode("");
            AddNode(treeView.Nodes, parentNode);

            //TreeNode parentNode = this.treeView1.Nodes.Add("");
            parentNode.Name = key.ToString();

            DefectType defectType = DefectType.Total;
            List<Tuple<int, int>> childList = dictionary[key];
            foreach (Tuple<int, int> child in childList)
            {
                int sheetNo = child.Item1;
                int cameraNo = child.Item2 % 10;
                int imageNo = child.Item2 / 10;

                FoundedObjInPattern ssr = GetSheetSubResult(sheetNo, cameraNo, imageNo);
                DefectType dt = ssr.GetDefectType();
                if (defectType == DefectType.Total)
                    defectType = dt;
                else if (defectType != DefectType.Unknown && defectType != dt)
                    defectType = DefectType.Unknown;

                string nodeText = string.Format("{0} - Sheet{1} - Cam{2} - Image{3}", dt.ToString(), sheetNo, cameraNo, imageNo);
                TreeNode childNode = new TreeNode(nodeText);
                AddNode(parentNode.Nodes, childNode);
                //TreeNode childNode = parentNode.Nodes.Add(nodeText);
                childNode.Tag = child;
            }

            UpdateNode(parentNode, string.Format("{0} - {1} - {2}", parentNode.Index, defectType.ToString(), childList.Count));
            return parentNode;
        }

        private delegate void AddNodeDelegate(TreeNodeCollection nodes, TreeNode node);
        private void AddNode(TreeNodeCollection nodes, TreeNode node)
        {
            if(InvokeRequired)
            {
                Invoke(new AddNodeDelegate(AddNode), nodes, node);
                return;
            }
            nodes.Add(node);
        }

        private delegate void UpdateNodeDelegate(TreeNode node, string name);
        private void UpdateNode(TreeNode node, string v)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateNodeDelegate(UpdateNode), node, v);
                return;
            }
            node.Text = v;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lastSelectedNode = e.Node;
            if (e.Node.Tag is Figure)
            {
                Figure figure = e.Node.Tag as Figure;
                if (e.Action == TreeViewAction.Unknown)
                {
                    treeViewPosition.CollapseAll();
                    e.Node.Expand();
                }
                else
                {
                    figure.Visible = true;
                    Rectangle rectangle = Rectangle.Round(figure.GetRectangle().GetBoundRect());
                    FocusCanvasPanel(rectangle);
                }
            }

            if (e.Node.Tag is Tuple<int, int>)
            {
                Tuple<int, int> tuple = e.Node.Tag as Tuple<int, int>;
                FoundedObjInPattern ssr = GetSheetSubResult(tuple.Item1, tuple.Item2);
                //ssr.RealRegion
                Bitmap bitmap = (Bitmap)ssr.Image.Clone();
                canvasPanelSmall.UpdateImage(bitmap);
                canvasPanelSmall.ZoomFit();
                defectWidth.Text = string.Format("{0} / {1:0.00}", ssr.Region.Width, ssr.RealRegion.Width);
                defectHeight.Text = string.Format("{0} / {1:0.00}", ssr.Region.Height, ssr.RealRegion.Height);
            }
            else
            {
                canvasPanelSmall.UpdateImage(null);
                defectWidth.Text = defectHeight.Text = "";
            }
        }

        private void FocusCanvasPanel(Rectangle rectangle)
        {
            rectangle.Inflate(canvasPanelFull.Width / 10, canvasPanelFull.Height/ 10);
            //Rectangle scaledRectangle = Rectangle.FromLTRB(rectangle.Left / 10, rectangle.Top / 10, rectangle.Right / 10, rectangle.Bottom / 10);
            canvasPanelFull.ZoomRange(rectangle);
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Tag is Figure)
            {
                Figure figure = (Figure)e.Node.Tag;
                figure.FigureProperty.Brush = e.Node.Checked ? new SolidBrush(Color.Red) : null;
            }

            foreach (TreeNode node in e.Node.Nodes)
            {
                node.Checked = e.Node.Checked;
            }
        }

        private FoundedObjInPattern GetSheetSubResult(int sheetNo, int cameraNo, int imageNo)
        {
            return mergeSheetResultList.Find(f => f.Index == sheetNo).SheetSubResultList.Find(f => f.CamIndex == cameraNo && f.Index == imageNo);
        }

        private FoundedObjInPattern GetSheetSubResult(int sheetNo, int defectNo)
        {
            int cameraNo = defectNo % 10;
            int imageNo = defectNo / 10;
            return GetSheetSubResult(sheetNo, cameraNo, imageNo);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = this.workingPath;
            dlg.Filter = "CSV Files(*.csv)|*.csv";
            dlg.FileName = "Export.csv";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            SimpleProgressForm form = new SimpleProgressForm();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            form.Show(() =>
            {

                StringBuilder stringBuilder = new StringBuilder();
                Dictionary<string, Image> saveImageDic = new Dictionary<string, Image>();
                foreach (TreeNode node in treeViewPosition.Nodes)
                {
                    Export(stringBuilder, saveImageDic, node, cancellationTokenSource.Token);
                }

                string path = Path.GetDirectoryName(dlg.FileName);
                File.WriteAllText(dlg.FileName, stringBuilder.ToString());
                foreach (KeyValuePair<string, Image> pair in saveImageDic)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    Bitmap bitmap = pair.Value as Bitmap;
                    if (bitmap != null)
                    {
                        ImageD imageD = Image2D.FromBitmap(pair.Value as Bitmap);
                        imageD.SaveImage(Path.Combine(path, pair.Key), System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageD.Dispose();
                    }
                }
            }, cancellationTokenSource);
        }

        private void Export(StringBuilder stringBuilder, Dictionary<string, Image> saveImageDic, TreeNode node, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            if (node.Checked)
            {
                Tuple<int, int> tuple = node.Tag as Tuple<int, int>;
                if (tuple != null)
                {
                    FoundedObjInPattern ssr = GetSheetSubResult(tuple.Item1, tuple.Item2);
                    string exportData = ssr.ToExportData().Replace('\t', ',');
                    if (stringBuilder.Length == 0)
                        stringBuilder.AppendLine(string.Format("SheetNo,{0}", FoundedObjInPattern.GetExportHeader().Replace('\t', ',')));

                    stringBuilder.AppendLine(string.Format("{0},{1}", tuple.Item1, exportData));
                    saveImageDic.Add(string.Format("S{0}C{1}I{2}.jpg", tuple.Item1, ssr.CamIndex, ssr.Index), ssr.Image);
                }
            }

            foreach (TreeNode childNode in node.Nodes)
                Export(stringBuilder, saveImageDic, childNode, cancellationToken);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.onLoading)
                return;

            if (e.KeyCode == Keys.Enter)
            {
                this.workingPath = textBox1.Text;
                canvasPanelFull.UpdateImage(null);
                canvasPanelFull.WorkingFigures.Clear();
                LoadData();
            }
        }

        private void deselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TreeNode treeNode in treeViewPosition.Nodes)
                treeNode.Checked = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            int sheetNo = (int)listBox1.SelectedItem;
            List<Figure> figureList = new List<Figure>();
            foreach (KeyValuePair <RectangleF, List<Tuple<int, int>>> pair in  this.dictionary)
            {
                List<Tuple<int, int>> found = pair.Value.FindAll(f => f.Item1 == sheetNo);
                if(found.Count>0)
                {
                    Figure figure = GetFigure(pair.Key);
                    figureList.Add(figure);
                }
            }
            canvasPanelFull.WorkingFigures.Clear();
            canvasPanelFull.WorkingFigures.AddFigure(figureList.ToArray());
            canvasPanelFull.Invalidate();
        }

        private Figure GetFigure(RectangleF key)
        {
            RectangleF scaledRect = RectangleF.FromLTRB(key.Left / 10, key.Top / 10, key.Right / 10, key.Bottom / 10);
            scaledRect.Inflate(2, 2);

            //Color color = GetColor(key);
            return new RectangleFigure(scaledRect, new Pen(Color.Aquamarine, 1));
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    UpdatePositionTreeview();
                    break;
                case 1    :
                    UpdateSheetListView();
                    break;
            }
        }
    }
}
