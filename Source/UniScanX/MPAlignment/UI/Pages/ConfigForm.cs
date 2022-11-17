using DynMvp.Device.Dio;
using DynMvp.Device.FrameGrabber;
using DynMvp.Devices;
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


namespace UniScanX.MPAlignment.UI.Pages
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            //this.SuspendLayout();
            //cmbDioType.SelectedIndex = 0;
            //LoadConfig();
            //this.ResumeLayout();

        }

        void LoadConfig()
        {
            //cmbGrabberType.SelectedIndex = (int)DeviceConfig.Instance.GrabberType;
            //cmbDioType.SelectedIndex = (int)DeviceConfig.Instance.DigitalIoType;
            //nudCamera.Value = (int)DeviceConfig.Instance.NumCamera;
            //virtualMode.Checked = DeviceConfig.Instance.VirtualDevice;
            //DeviceConfig.Instance.Load(SystemConfig.Instance.ConfigPath);

            //imagingLibrary.SelectedIndex = (int)SystemConfig.Instance.ImagingLibrary;
            //txtVirtualImagePath.Text = DeviceConfig.Instance.VirtualImageFolderPath;
        }

        void SaveConfig() 
        {
            //DeviceConfig.Instance.GrabberType = (GrabberType)cmbGrabberType.SelectedIndex;
            //DeviceConfig.Instance.DigitalIoType = (DigitalIoType)cmbDioType.SelectedIndex;
            //DeviceConfig.Instance.VirtualImageFolderPath = txtVirtualImagePath.Text;
            //DeviceConfig.Instance.VirtualDevice = virtualMode.Checked;
            //DeviceConfig.Instance.Save(SystemConfig.Instance.ConfigPath);

            //SystemConfig.Instance.ImagingLibrary = (ImagingLibrary)imagingLibrary.SelectedIndex;
            //SystemConfig.Instance.IsVirtualMode = virtualMode.Checked;
            
            //SystemConfig.Instance.Save();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void buttonCameraConfiguration_Click(object sender, EventArgs e)
        {
            //Grabber grabber = GrabberFactory.Create((GrabberType)cmbGrabberType.SelectedIndex);
            //string grabberTypeString = grabber.Type.ToString();

            //CameraConfiguration cameraConfiguration = new CameraConfiguration();
            //string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", SystemConfig.Instance.ConfigPath, grabberTypeString);
            //if (File.Exists(filePath) == true)
            //{
            //    cameraConfiguration.LoadCameraConfiguration(grabber.Type, filePath);
            //}

            //grabber.SetupCameraConfiguration((int)nudCamera.Value, cameraConfiguration);

            //if (cameraConfiguration.CameraInfoList.Count < (int)nudCamera.Value)
            //{
            //    MessageBox.Show("The number of camera is less then required number of camera");
            //    return;
            //}
            //cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private void buttonVirtual_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtVirtualImagePath.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
