using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GpuTransfer
{
    public partial class Form1 : Form
    {
        static string LOGBEGIN = "<LOGBEGIN>:";
        static string LOGERROR = "<LOGERROR>:";

        Param param;

        public Form1()
        {
            InitializeComponent();
            this.param = new Param();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.propertyGrid1.SelectedObject = param;

            buttonEmgu.Tag = ImagingLibrary.OpenCv;
            buttonMil.Tag = ImagingLibrary.MatroxMIL;
            buttonCuCudas.Tag = ImagingLibrary.Custom;
            UpdateListViewSize();
        }

        private void listView1_SizeChanged(object sender, EventArgs e) => UpdateListViewSize();

        private void UpdateListViewSize()
        {
            int c = 1;
            int[] widths = new int[this.listView1.Columns.Count];
            for (int i = 0; i < this.listView1.Columns.Count; i++)
                widths[i] = (i == c ? 0 : this.listView1.Columns[i].Width);
            this.listView1.Columns[c].Width = this.listView1.Width - widths.Sum() - 5;
        }

        public delegate void AppendLogDelegate(string line, long timeMs = -1);
        public void AppendLog(string line, long timeMs = -1)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new AppendLogDelegate(AppendLog), line, timeMs);
                return;
            }

            //Color color = new Regex("[^</w{3}>]").IsMatch(line) ? Color.LightPink : SystemColors.ControlLight;
            Color color = Color.Transparent;
            if (line.StartsWith(LOGBEGIN))
            {
                line = line.Remove(0, LOGBEGIN.Length);
                color = Color.LightSkyBlue;
            }else if (line.StartsWith(LOGERROR))
            {
                line = line.Remove(0, LOGERROR.Length);
                color = Color.LightPink;
            }

            ListViewItem item = new ListViewItem(DateTime.Now.ToString("HH:mm:ss"));
            item.SubItems.Add(line);
            if (timeMs >= 0)
            {
                item.SubItems.Add(timeMs.ToString());
                item.SubItems.Add((timeMs / param.SizeGByte).ToString("F01"));                
                item.BackColor = Color.LightYellow;
            }

            if (color != Color.Transparent)
                item.BackColor = color;

            this.listView1.Items.Add(item);

            item.EnsureVisible();
        }

        private async Task BuildAlgoImages(AlgoImage[] algoImages, ImagingLibrary imagingLibrary)
        {
            await Task.Run(() =>
            {
                AppendLog($"== Alloc ==");

                if (imagingLibrary == ImagingLibrary.MatroxMIL)
                {
                    MatroxHelper.InitApplication(false, true);
                    MatroxHelper.ThrowExceptionOnError();
                }

                for (int i = 0; i < algoImages.Length; i++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    algoImages[i] = BuildAlgoImage(imagingLibrary, param.Size);
                    sw.Stop();
                    AppendLog($"{i:00}: Alloc GPU Image", sw.ElapsedMilliseconds);
                }
            });
        }

        private AlgoImage BuildAlgoImage(ImagingLibrary imagingLibrary, Size size)
        {
            switch(imagingLibrary )
            {
                case ImagingLibrary.Custom:
                    return new DynMvp.Vision.Cuda.CudaImageBuilder().Build(ImageType.Gpu, size.Width, size.Height);

                default:
                    return ImageBuilder.Build(imagingLibrary, ImageType.Gpu, size.Width, size.Height);
            }
        }

        private void ReleaseAlgoImage(AlgoImage[] algoImages, ImagingLibrary imagingLibrary)
        {
            AppendLog($"=== Free ===");

            Array.ForEach(algoImages, f => f?.Dispose());

            if (imagingLibrary == ImagingLibrary.MatroxMIL)
                MatroxHelper.FreeApplication();
        }

        private async void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ImagingLibrary imagingLibrary = (ImagingLibrary)button.Tag;

            AppendLog($"{LOGBEGIN}Start with {imagingLibrary}. Size: {param.SizeGByte:F02}[GB]. Using {(param.UseIntPtr ? "IntPtr" : "byte[]")}.");
            AlgoImage[] algoImages = new AlgoImage[param.Iterate];
            try
            {
                await BuildAlgoImages(algoImages, (ImagingLibrary)button.Tag);
                await new TransferTest(AppendLog).Test(algoImages, this.param);
            }
            catch (Exception ex)
            {
                AppendLog($"{LOGERROR}{ex.GetType().Name}: {ex.Message}");
            }
            finally
            {
                ReleaseAlgoImage(algoImages, imagingLibrary);
                AppendLog($"{LOGBEGIN}End with {imagingLibrary}");
            }
        }

        private void buttonLogClear_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
        }

        private void buttonLogSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "TXT File(*.txt)|*.txt",
                DefaultExt = ".txt",
                FileName = "LogFile.txt"
            };

            DialogResult dlgResult = dlg.ShowDialog(this);

            if (dlgResult != DialogResult.OK)
                return;

            StringBuilder sb = new StringBuilder();
            foreach(ListViewItem item in this.listView1.Items)
            {
                string[] tokens = new string[item.SubItems.Count];
                for (int i = 0; i < item.SubItems.Count; i++)
                    tokens[i] = item.SubItems[i].Text;
                sb.AppendLine(string.Join(", ", tokens));
            }

            System.IO.File.WriteAllText(dlg.FileName, sb.ToString());
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.param.SaveSettings();
        }
    }
}
