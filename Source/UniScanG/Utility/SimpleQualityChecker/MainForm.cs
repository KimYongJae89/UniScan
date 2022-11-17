using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using DynMvp.Vision;
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

namespace SimpleQualityChecker
{
    public partial class MainForm : Form
    {
        List<ImageForm> imageFormList = new List<ImageForm>();
        ImagingLibrary imagingLibrary = ImagingLibrary.MatroxMIL;

        public MainForm()
        {
            InitializeComponent();

            if(imagingLibrary == ImagingLibrary.MatroxMIL)
                MatroxHelper.InitApplication();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "BITMAP(*.bmp)|*.bmp";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            string[] selectFiles = dlg.FileNames;
            foreach (string selectFile in selectFiles)
            {
                Image2D image2D = null;
                string fileName = Path.GetFileName(selectFile);
                for (int i = 0; i < 1; i++)
                {
                    string prefix = "";
                    Bitmap bitmap = null;
                    Bitmap bitmap2 = null;
                    double stdDev = 0;
                    string title = $"{fileName}{Environment.NewLine}Loading...";
                    SimpleProgressForm simpleProgressForm = new SimpleProgressForm(title);
                    simpleProgressForm.Show(() =>
                    {
                        if (image2D == null)
                        {
                            image2D = new Image2D();
                            image2D.LoadImage(selectFile);
                        }

                        AlgoImage algoImage = ImageBuilder.GetInstance(this.imagingLibrary).Build(image2D, ImageType.Grey);
                        ImageProcessing ip = ImageProcessingFactory.CreateImageProcessing(this.imagingLibrary);

                        if (i % 2 == 1)
                        {
                            prefix += "A";
                            ip.Average(algoImage);
                            ip.Average(algoImage);
                            ip.Average(algoImage);
                            ip.Average(algoImage);
                            ip.Average(algoImage);
                        }

                        if (i / 2 == 1)
                        {
                            prefix += "B";
                            ip.Binarize(algoImage, algoImage);
                        }

                        // Resize to View
                        using (AlgoImage resize = ImageBuilder.GetInstance(this.imagingLibrary).Build(ImageType.Grey, image2D.Width / 4, image2D.Height / 4))
                        {
                            ip.Resize(algoImage, resize);
                            resize.Save(@"C:\temp\resizeImage.bmp");
                            bitmap = resize.ToBitmap();
                        }

                        // Calculate
                        using (AlgoImage sobel = ImageBuilder.GetInstance(this.imagingLibrary).Build(ImageType.Grey, image2D.Width, image2D.Height))
                        {
                            //ip.Sobel(algoImage, sobel, Direction.Horizontal);
                            ip.Sobel(algoImage, sobel);

                            using (AlgoImage resize = ImageBuilder.GetInstance(this.imagingLibrary).Build(ImageType.Grey, sobel.Width / 4, sobel.Height / 4))
                            {
                                ip.Resize(sobel, resize);
                                resize.Save(@"C:\temp\sobel.bmp");
                                bitmap2 = resize.ToBitmap();
                            }

                            if (false)
                            {
                                float[] proj = ip.Projection(sobel, Direction.Vertical, ProjectionType.Mean);
                                double mean = proj.Average();
                                double mean2 = proj.Average(f => Math.Pow(f - mean, 2));
                                stdDev = Math.Sqrt(mean2);
                            }
                            else
                            {
                                StatResult statResult = ip.GetStatValue(sobel);
                                stdDev = statResult.stdDev;
                            }
                        }

                        algoImage.Dispose();
                    });

                    if (bitmap != null)
                    {
                        ImageForm imageForm = new ImageForm();
                        imageForm.SetData($"{prefix}{fileName}", bitmap, bitmap2, 4, stdDev);
                        imageForm.FormClosed += new FormClosedEventHandler((s, a) => this.imageFormList.Remove((ImageForm)s));
                        imageForm.Load += new EventHandler((s, a) => this.imageFormList.Add((ImageForm)s));
                        imageForm.Show(this);
                    }
                }
                image2D?.Dispose();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (imageFormList.Count > 0)
            {
                if (MessageBox.Show(this, "Close?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            ImageForm[] arr = imageFormList.ToArray();
            Array.ForEach(arr, f => f.Close());
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(imagingLibrary == ImagingLibrary.MatroxMIL)
                MatroxHelper.FreeApplication();
        }
    }
}
