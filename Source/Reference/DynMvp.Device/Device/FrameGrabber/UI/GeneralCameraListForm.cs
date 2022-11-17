using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class GeneralCameraListForm : Form
    {
        int requiredNumCamera;
        public int RequiredNumCamera
        {
            set { requiredNumCamera = value; }
        }

        public CameraConfiguration CameraConfiguration { get => this.cameraConfiguration; set => this.cameraConfiguration = value; }
        CameraConfiguration cameraConfiguration;
        
        public GeneralCameraListForm()
        {
            InitializeComponent();
        }

        private void GeneralCameraListForm_Load(object sender, EventArgs e)
        {
            comboBox.DataSource = this.cameraConfiguration.CameraInfos;
            comboBox.DisplayMember = "Name";
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            CameraInfo cameraInfo = (CameraInfo)comboBox.SelectedItem;
            this.propertyGrid.SelectedObject = cameraInfo;
        }
    }
}
