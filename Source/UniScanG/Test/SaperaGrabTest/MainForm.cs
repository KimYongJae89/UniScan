using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.OpenCv;
using Emgu.CV;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Device;
using UniEye.Base.Settings;

namespace SaperaGrabTest
{
    public partial class MainForm : Form
    {
        Grabber grabber;
        Camera camera;
        ConcurrentQueue<ImageD> queue;
        ThreadHandler ThreadHandler;

        bool initializeDevice;

        public MainForm()
        {
            InitializeComponent();
            initializeDevice = InitializeDevice();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!initializeDevice)
                Close();
        }

        private bool InitializeDevice()
        {
            try
            {
                this.grabber = new GrabberSapera("Sapera");

                CameraConfiguration cameraConfiguration = new CameraConfiguration(1);
                string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabber.Name);
                cameraConfiguration.LoadCameraConfiguration(filePath);

                this.camera = new CameraSapera(cameraConfiguration.CameraInfos[0]);
                this.camera.Initialize(false);
                //this.camera.SetTriggerMode(TriggerMode.Hardware);

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

                    CameraSaperaBufferTag tag = (CameraSaperaBufferTag)imageD.Tag;
                    string str = string.Format("{0}: {1} x {2}", tag.DateTime.ToString("HH:mm:ss"), tag.FrameSize.Width, tag.FrameSize.Height);
                    AddListBox(str);

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
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
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
            this.listBox1.SelectedItem = str;
        }

        private void Camera_ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            this.queue.Enqueue(imageD);
        }

        private void buttonGrab_Click(object sender, EventArgs e)
        {
            if (camera.IsStopped())
            {
                camera.GrabMulti();
                this.camera.ImageGrabbed += Camera_ImageGrabbed;
            }
        }

        private void buttonSnap_Click(object sender, EventArgs e)
        {
            if (camera.IsStopped())
                camera.GrabOnce();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.camera.ImageGrabbed -= Camera_ImageGrabbed;
            camera.Stop();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ThreadHandler?.Stop();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = (Bitmap)this.pictureBox1.Image.Clone();
            SaveFileDialog d = new SaveFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    bitmap.Save(d.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
        }
    }
}
