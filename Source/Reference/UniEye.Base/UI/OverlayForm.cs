using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.Devices;
using System;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public partial class OverlayForm : Form
    {
        DrawBox imageBox;

        public OverlayForm()
        {
            InitializeComponent();

            buttonCam1.Text = StringManager.GetString(this.GetType().FullName, buttonCam1.Text);
            buttonCam2.Text = StringManager.GetString(this.GetType().FullName, buttonCam2.Text);
            buttonSelectImage1.Text = StringManager.GetString(this.GetType().FullName, buttonSelectImage1.Text);
            buttonSelectImage2.Text = StringManager.GetString(this.GetType().FullName, buttonSelectImage2.Text);
            buttonSetCam1.Text = StringManager.GetString(this.GetType().FullName, buttonSetCam1.Text);
            buttonSetCam2.Text = StringManager.GetString(this.GetType().FullName, buttonSetCam2.Text);

            Initialize();
        }

        private void Initialize()
        {
            imageBox = new DrawBox();

            this.SuspendLayout();

            this.imageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageBox.Name = "imageBox";
            this.imageBox.TabIndex = 0;
            this.imageBox.TabStop = false;
            imageBox.OverlayMoveMode = true;
            imageBox.Enable = true;

            this.panelImage.Controls.Add(this.imageBox);
            
            this.ResumeLayout(false);

            ChangeVisibleControl();
        }

        private void buttonSelectImage2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImage2Path.Text = dialog.FileName;
                imageBox.OverlayImage = (Bitmap)ImageHelper.LoadImage(dialog.FileName);
                imageBox.Invalidate();
            }
        }

        private void buttonSelectImage1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxImage1Path.Text = dialog.FileName;
                imageBox.UpdateImage( (Bitmap)ImageHelper.LoadImage(dialog.FileName));
            }
        }

        private ImageD Grab(int index)
        {
            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            if (imageDevice == null)
                return null;

            imageDevice.GrabOnce();

            return imageDevice.GetGrabbedImage(IntPtr.Zero);
        }

        private void buttonCam1_Click(object sender, EventArgs e)
        {
            if (MachineSettings.Instance().RunningMode != RunningMode.Real)
                return;

            imageBox.UpdateImage (Grab(0).ToBitmap());
            imageBox.Invalidate();
        }

        private void buttonCam2_Click(object sender, EventArgs e)
        {
            if (MachineSettings.Instance().RunningMode != RunningMode.Real)
                return;

            imageBox.OverlayImage = Grab(1).ToBitmap();
            imageBox.Invalidate();
        }

        private void ChangeVisibleControl()
        {
            if (MachineSettings.Instance().RunningMode != RunningMode.Real)
                panelCam.Visible = false;
        }

        public PointF GetOffset()
        {
            if (imageBox.OverlayImage == null)
                return new PointF(0, 0);
            return imageBox.OverlayPos;
        }

        private void buttonSetCam1_Click(object sender, EventArgs e)
        {
//            Settings.Instance().CalibrationSettings.Calibration2 = GetOffset();
//            Settings.Save();
        }

        private void buttonSetCam2_Click(object sender, EventArgs e)
        {
//            Settings.Instance().CalibrationSettings.Calibration2 = GetOffset();
//            Settings.Save();
        }
    }
}
