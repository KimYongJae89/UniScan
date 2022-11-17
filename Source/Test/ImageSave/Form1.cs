using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSave
{
    public partial class Form1 : Form
    {
        Thread thread = null;
        float scaleFactor = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (thread != null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "JPG|*.jpg|BMP|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = dlg.FileNames;
                Array.Sort(filenames);
                textBox1.Lines = filenames;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread != null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Filter = "JPG|*.jpg|BMP|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] filenames = dlg.FileNames;
                Array.Sort(filenames);
                textBox2.Lines = filenames;
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (thread != null)
                return;

            this.scaleFactor = (float)this.rescale.Value;
            if (this.scaleFactor == 0)
                this.scaleFactor = 1;

            thread = new Thread(ThreadProc);
            thread.Start();
            
        }

        private delegate void UpdateStateDelegate(string state);
        private void UpdateState(string state)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateStateDelegate(UpdateState), state);
                return;
            }
            textBox4.Text = state;
        }

        private void ThreadProc()
        {
            int l1 = textBox1.Lines.Length;
            int l2 = textBox2.Lines.Length;
            if (l1 == l2)
            {
                int totalStep = l1 * 4;
                int curStep = 0;
                UpdateProgress(0, totalStep);
                for (int i = 0; i < l1; i++)
                {
                    string src1 = textBox1.Lines[i];
                    string src2 = textBox2.Lines[i];
                    string dst = Path.Combine(Path.GetDirectoryName(src1), string.Format("L{0:00}.jpg", i));

                    UpdateState("Load...");
                    ImageD image1 = new Image2D(src1);
                    image1.SaveImage(dst);
                    ImageD image2 = new Image2D(src2);
                    image2.SaveImage(dst);
                    if (image1.NumBand != image2.NumBand)
                        continue;

                    ImageType imageType = image1.NumBand == 1 ? ImageType.Grey : ImageType.Color;
                    AlgoImage algoImage1 = ImageBuilder.Build(ImagingLibrary.OpenCv, image1, imageType);
                    AlgoImage algoImage2 = ImageBuilder.Build(ImagingLibrary.OpenCv, image2, imageType);
                    UpdateProgress(++curStep, totalStep);
                    algoImage1.Save(dst);


                    UpdateState("Resize...");
                    Size newSize = Size.Round(new SizeF(algoImage1.Width * this.scaleFactor, algoImage1.Height * this.scaleFactor));
                    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage1);
                    AlgoImage algoImage3 = ImageBuilder.Build(ImagingLibrary.OpenCv, imageType, newSize);
                    AlgoImage algoImage4 = ImageBuilder.Build(ImagingLibrary.OpenCv, imageType, newSize);

                    ip.Resize(algoImage1, algoImage3);
                    ip.Resize(algoImage2, algoImage4);
                    UpdateProgress(++curStep, totalStep);
                    
                    UpdateState("Merge...");
                    AlgoImage algoImage5 = ImageBuilder.Build(ImagingLibrary.OpenCv, imageType, new Size(algoImage3.Width + algoImage4.Width, Math.Max(algoImage3.Height, algoImage4.Height)));

                    Point dstPoint = new Point();
                    algoImage5.Copy(algoImage3, Point.Empty, dstPoint, algoImage3.Size);
                    dstPoint.X += algoImage3.Width;
                    algoImage5.Copy(algoImage4, Point.Empty, dstPoint, algoImage4.Size);
                    UpdateProgress(++curStep, totalStep);

                    UpdateState("Save...");
                    //algoImage5.Save(dst);
                    ImageD imageD = algoImage5.ToImageD();
                    imageD.SaveImage(dst, ImageFormat.Jpeg);
                    imageD.Dispose();
                    UpdateProgress(++curStep, totalStep);
                    algoImage1.Dispose();
                    algoImage2.Dispose();
                    algoImage3.Dispose();
                    algoImage4.Dispose();
                    algoImage5.Dispose();
                    image1.Dispose();
                    image2.Dispose();

                }
            }
            UpdateState("Done");
            this.thread = null;
        }

        private delegate void UpdateProgressDelegate(int v, int totalStep);
        private void UpdateProgress(int v, int totalStep)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateProgressDelegate(UpdateProgress), v, totalStep);
                return;
            }
            progressBar1.Maximum = totalStep;
            progressBar1.Value = v;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
