using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;

namespace DynMvp.Devices.UI
{
    public partial class CameraGridView : UserControl
    {
        private CameraView[] cameraView;

        public CameraGridView()
        {
            InitializeComponent();
        }

        public void InitCameraViewPanel(ImageDeviceHandler imageDeviceHandler)
        {
            int numCamera = imageDeviceHandler.Count;

            int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));
            cameraViewPanel.ColumnCount = numCount;
            cameraViewPanel.RowCount = (int)Math.Floor(Math.Sqrt(numCamera));

            cameraViewPanel.ColumnStyles.Clear();
            cameraViewPanel.RowStyles.Clear();

            for (int i = 0; i < cameraViewPanel.ColumnCount; i++)
            {
                cameraViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / cameraViewPanel.ColumnCount));
            }

            for (int i = 0; i < cameraViewPanel.RowCount; i++)
            {
                cameraViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / cameraViewPanel.RowCount));
            }

            cameraView = new CameraView[numCamera];

            int index = 0;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                int rowIndex = index / numCount;
                int colIndex = index % numCount;

                cameraView[index] = new CameraView();
                cameraView[index].Dock = DockStyle.Fill;
                cameraView[index].SizeMode = PictureBoxSizeMode.Zoom;
                cameraView[index].Tag = imageDevice;
                cameraView[index].DoubleClick += new System.EventHandler(this.cameraView_DoubleClick);
                cameraView[index].SetImageDevice(imageDevice);

                cameraViewPanel.Controls.Add(cameraView[index], colIndex, rowIndex);

                index++;
            }
        }

        private void cameraView_DoubleClick(object sender, EventArgs e)
        {
            int numCamera = cameraView.Count();
            if (numCamera < 1)
                return;

            CameraView senderView = (CameraView)sender;

            if (senderView.Parent == this)
            {
                int numCount = (int)Math.Ceiling(Math.Sqrt(numCamera));
                int cameraIndex = (int)senderView.LinkedImageDevice.Index;

                int rowIndex = cameraIndex / numCount;
                int colIndex = cameraIndex % numCount;

                cameraViewPanel.Controls.Add(senderView, colIndex, rowIndex);
                cameraViewPanel.Show();
            }
            else
            {
                senderView.Parent = this;
                cameraViewPanel.Hide();
            }
        }

        public void LockImageUpdate(bool lockImageUpdate)
        {
            foreach ( CameraView view in cameraView)
            {
                view.LockImageUpdate = lockImageUpdate;
            }
        }

        public void EnableMeasureMode(float scaleX, float scaleY)
        {
            foreach (CameraView view in cameraView)
            {
                view.EnableMeasureMode(scaleX, scaleY);
            }
        }
    }
}
