using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Common;
using UniScanG.Vision.Test.Algorithm;

namespace UniScanG.Vision.Test
{
    public partial class Form1 : Form
    {
        [DllImport("UniScanG.Vision.dll", CallingConvention = CallingConvention.Cdecl)]
        extern public static int HelloWorld(int integer);

        [DllImport("UniScanG.Vision.dll", CallingConvention = CallingConvention.Cdecl)]
        extern static IntPtr GetSIMDType();

        string DataFilePath => Path.Combine(modelPathPath, "AlgorithmPool.xml");
        string ImageFilePath => Path.GetFullPath(Path.Combine(modelPathPath, "Image", "Image_C00_S000_L00.bmp"));

        string modelPathPath = "";
        Class1 myClass = new Class1();

        HardwareMonitor hardwareMonitor;

        public Form1()
        {
            InitializeComponent();

            this.numericUpDown1.Maximum = int.MaxValue;
            this.hardwareMonitor = new HardwareMonitor(@"C:\HardwareMonitor", true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string str = Marshal.PtrToStringAnsi(GetSIMDType());

            Array arr = Enum.GetValues(typeof(AlgoLibrary));
            foreach (AlgoLibrary algoLibrary in arr)
            {
                string text = algoLibrary.ToString();
                if (algoLibrary == AlgoLibrary.SIMD)
                    text = string.Format("{0}.{1}", algoLibrary, str);
                comboBox1.Items.Add(text);
            }
            comboBox1.SelectedIndex = 0;

            this.modelPathPath = @"D:\UniScan\Gravure_Inspector\Model\440-32BMJE502-GL08SC\6.8\PASTE";
            UpdateForm();

            //this.hardwareMonitor.Start(1000);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.hardwareMonitor?.Stop();
        }

        private void UpdateForm()
        {
            textBox1.Text = modelPathPath;
            labelDataFile.Visible = File.Exists(DataFilePath);
            labelImageFile.Visible = File.Exists(ImageFilePath);

            //this.panel1.Enabled = this.myClass.IsLoaded;
        }

        private void buttonAlgorithmPool_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"D:\UniScan\Gravure_Inspector\Model\440-32BMJE502-GL08SC\6.8\PASTE";
            DialogResult dialogResult = fbd.ShowDialog();
            if (dialogResult == DialogResult.OK)
                modelPathPath = fbd.SelectedPath;

            UpdateForm();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            bool loadOk = false;
            if (!string.IsNullOrEmpty(this.modelPathPath))
                loadOk = this.myClass.Load(this.DataFilePath, this.ImageFilePath);

            if (loadOk)
                AddTab(Path.GetFileName(this.ImageFilePath), this.myClass.Image2D.ToBitmap());
            UpdateForm();
        }

        public void AddTab(string title, Bitmap bitmap)
        {
            if(InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { AddTab(title, bitmap); });
                return;
            }

            TabPage tabPage = this.tabControl1.Controls.Find(title, false).FirstOrDefault() as TabPage;
            if (tabPage == null)
            {
                tabPage = new TabPage(title);
                tabPage.Name = title;
                this.tabControl1.Controls.Add(tabPage);
            }

            CanvasPanel canvasPanel = tabPage.Controls.Find("CanvasPanel", false).FirstOrDefault() as CanvasPanel;
            if (canvasPanel == null)
            {
                canvasPanel = new CanvasPanel();
                canvasPanel.Dock = DockStyle.Fill;
                canvasPanel.SetPanMode();
                canvasPanel.DoubleClick += delegate (object s, EventArgs e) { (s as CanvasPanel)?.ZoomFit(); };
                canvasPanel.MouseClick += delegate (object sender, MouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        canvasPanel.Image.Save(string.Format(@"C:\temp\{0}.bmp", title));
                        System.Diagnostics.Process.Start(@"C:\temp\");
                    }
                };

                tabPage.Controls.Add(canvasPanel);
            }

            canvasPanel.UpdateImage(bitmap, new RectangleF(PointF.Empty, canvasPanel.Size));
            this.tabControl1.SelectTab(tabPage);
        }

        private void DoProcess(AlgoLibrary algoLibrary, int v, int r, bool parallel)
        {
            this.dataGridView1.DataSource = null;

            ProgressForm progressForm = new ProgressForm();
            progressForm.BackgroundWorker.DoWork += delegate (object sender, DoWorkEventArgs e)
            {
                DebugContext debugContext = new DebugContext();
                e.Result = debugContext;

                debugContext.AddMessage(string.Format("Start {0} Process {1} - W{2} H{3}", algoLibrary, v, this.myClass.Image2D.Width, this.myClass.Image2D.Height));
                for (int i = 0; i < r; i++)
                {
                    Image2D image2D = this.myClass.DoProcess(v, algoLibrary, parallel, debugContext);
                    AddTab(string.Format("{0} - Process{1}", algoLibrary, v), image2D.ToBitmap());
                    image2D.Dispose();
                    progressForm.ReportProgress((int)((i + 1) * 100f / r), string.Format("{0} / {1}", i + 1, r));

                    if (progressForm.BackgroundWorker.CancellationPending)
                    {
                        debugContext.AddMessage("Cancelled");
                        break;
                    }
                }
                debugContext.AddMessage("Done");
            };
            progressForm.BackgroundWorker.RunWorkerCompleted += delegate (object sender, RunWorkerCompletedEventArgs e)
            {
                DebugContext debugContext = e.Result as DebugContext;
                this.dataGridView1.DataSource = debugContext.TupleList;
                MessageBox.Show(e.Cancelled ? "Cancelled" : "Done");
            };
            progressForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlgoLibrary algoLibrary = (AlgoLibrary)this.comboBox1.SelectedIndex;
            DoProcess(algoLibrary, 1, (int)this.numericUpDown1.Value, this.parallel.Checked);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AlgoLibrary algoLibrary = (AlgoLibrary)this.comboBox1.SelectedIndex;
            DoProcess(algoLibrary, 2, (int)this.numericUpDown1.Value, this.parallel.Checked);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AlgoLibrary algoLibrary = (AlgoLibrary)this.comboBox1.SelectedIndex;
            DoProcess(algoLibrary, 3, (int)this.numericUpDown1.Value, this.parallel.Checked);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SIMDProcess.Multiply();
            //SIMDProcess.SIMDTEST();
        }
    }
}
