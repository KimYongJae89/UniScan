using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using System.Threading.Tasks;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;

using UniEye.Base.Device;
using UniEye.Base.Settings;

using System.Diagnostics;
using System.IO;


namespace MatroxTest
{
    public partial class Form1 : Form
    {
        Grabber grabber;
        Camera camera;
        ConcurrentQueue<ImageD> queue;
        ThreadHandler ThreadHandler;
        List<ImageD> _listImage = new List<ImageD>();

        public Form1()
        {
            InitializeComponent();
            MatroxHelper.InitApplication();
            InitializeCamera();
            this.checkBox1.Checked = true;
        }

        private bool InitializeCamera()
        {
            try
            {
                this.grabber = new GrabberMil("Eliixa");

                CameraConfiguration cameraConfiguration = new CameraConfiguration(1);
                string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabber.Name);
                cameraConfiguration.LoadCameraConfiguration(filePath);

                this.camera = new CameraMil(cameraConfiguration.CameraInfos[0]);
                this.camera.Initialize(false);
                this.camera.ImageGrabbed += Camera_ImageGrabbed;
                this.camera.SetTriggerMode(TriggerMode.Hardware);

                this.queue = new ConcurrentQueue<ImageD>();
                this.ThreadHandler = new ThreadHandler("ThreadHandler", new System.Threading.Thread(ThreadProc));
                this.ThreadHandler.Start();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void ThreadProc()
        {
            while (!this.ThreadHandler.RequestStop)
            {
                ImageD imageD;
                if (this.queue.TryDequeue(out imageD))
                {
                    if (this.queue.Count > 0)
                        continue;

                    //CameraSaperaBufferTag tag = (CameraSaperaBufferTag)imageD.Tag;
                    //string str = string.Format("{0}: {1} x {2}", tag.DateTime.ToString("HH:mm:ss"), tag.FrameSize.Width, tag.FrameSize.Height);
                    string str = string.Format("Grabbed");
                    
                    AddListBox(str);
                    _listImage.Add(imageD);

                    if (this.checkBox1.Checked)
                    {
                        AlgoImage algoImage = OpenCvImageBuilder.Build(ImagingLibrary.OpenCv, imageD, ImageType.Grey);
                        AlgoImage algoImage2 = OpenCvImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Grey, imageD.Width / 10, imageD.Height / 10);
                        ImageProcessing ip = ImageProcessingFactory.CreateImageProcessing(algoImage.LibraryType);
                        ip.Resize(algoImage, algoImage2);
                        Bitmap bb = algoImage2.ToBitmap();
                        UiHelper.SetPictureboxImage(this.pictureBox1, bb);
                        algoImage2.Dispose();
                        algoImage.Dispose();

                        UiHelper.SetPictureboxImage(this.pictureBox1, imageD.ToBitmap());
                    }
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }
        private void Camera_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            this.queue.Enqueue(imageD);

            imageD.SaveImage("D:\\imageD.bmp");
           //this.pictureBox1.Image = imageD.ToBitmap();

            Debug.WriteLine("Camera_ImageGrabbed...");
        }


        private delegate void AddListBoxDelegate(string str);
        private void AddListBox(string str)
        {
            if (this.listBox1.InvokeRequired)
            {
                this.listBox1.Invoke(new AddListBoxDelegate(AddListBox), str);
                return;
            }
            this.listBox1.Items.Add(str);
            //this.listBox1.SelectedItem = str;
        }


        private void button_Init_Click(object sender, EventArgs e)
        {
            InitializeCamera();
        }

        private void buttonSeQuenceGrab_Click(object sender, EventArgs e)
        {
            int count  = (int)nudSeQcount.Value;
            if (camera.IsStopped())
                camera.GrabMulti(count);
        }


        private void button_Grab_Click(object sender, EventArgs e)
        {
            if (camera.IsStopped())
                camera.GrabMulti();
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            camera.Stop();
        }

        private void buttonOneShot_Click(object sender, EventArgs e)
        {
            if (camera.IsStopped())
                camera.GrabOnce();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ThreadHandler.RequestStop = true;
            MatroxHelper.FreeApplication();
        }

        private void buttonClearListBox_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            _listImage.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0) return;

            pictureBox1.Image = _listImage[index].ToBitmap();
        }

        private void buttonDeleteSelectedList_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            listBox1.Items.RemoveAt(index);
            _listImage.RemoveAt(index);
        }

        private void buttonSaveImage_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".bmp";
            //dlg.FileName = string.Format("CGimage_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            dlg.FileName = string.Format("CGimage_.bmp");
            dlg.Filter = "Bitmap(BMP)|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string ResultPath = dlg.FileName;
                this.Text = dlg.FileName;
                string filename = Path.GetFileNameWithoutExtension(dlg.FileName);
                string path = Path.GetDirectoryName(dlg.FileName);
                string fileExt = Path.GetExtension(dlg.FileName);

                int index = 0;
                foreach( var image in _listImage)
                {
                    var savePath = Path.Combine(path, filename + index.ToString() + fileExt);
                    image.SaveImage(savePath);
                    index++;
                }
            }
            //Path.GetFileName(fullName);                   //test.txt
            //Path.GetFileNameWithoutExtension(fullName);   //test
            //Path.GetExtension(fullName);                  //.txt
            //Path.GetPathRoot(fullName);                   //c:\
            //Path.GetDirectoryName(fullName);              //c:\\folder\\
        }

        private void buttonSaveMerge_Click(object sender, EventArgs e)
        {
            if (_listImage.Count == 0) return;
            Cursor.Current = Cursors.WaitCursor;
            this.Cursor = Cursors.WaitCursor;

            int WImageWidht = _listImage[0].Width;
            int WImageHeight = 0;
            int sImageHeight = _listImage[0].Height;

            foreach (var image in _listImage)
            {
                WImageHeight += image.Height;    
            }
            Image2D wholeImage = new Image2D(WImageWidht, WImageHeight, 1);

            Rectangle srcRect= new Rectangle(0,0,WImageWidht,sImageHeight);


            Point destPt = Point.Empty;
            int index = 0;
            foreach (var image in _listImage)
            {
                destPt.X = 0;
                destPt.Y = index * sImageHeight;
                wholeImage.CopyFrom(image, srcRect, image.Pitch, destPt);
                index++;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".bmp";
            dlg.FileName = string.Format("CGimageWhile_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            dlg.Filter = "Bitmap(BMP)|*.bmp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string ResultPath = dlg.FileName;
                this.Text = dlg.FileName;
                string filename = Path.GetFileNameWithoutExtension(dlg.FileName);
                string path = Path.GetDirectoryName(dlg.FileName);
                string fileExt = Path.GetExtension(dlg.FileName);

                wholeImage.SaveImage(ResultPath);
            }
        }
    }
}
