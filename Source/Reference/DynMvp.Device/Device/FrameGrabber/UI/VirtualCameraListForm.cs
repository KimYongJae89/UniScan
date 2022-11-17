using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class VirtualCameraListForm : Form
    {
        int requiredNumCamera;
        public int RequiredNumCamera
        {
            set { requiredNumCamera = value; }
        }

        CameraConfiguration cameraConfiguration;
        public CameraConfiguration CameraConfiguration
        {
            get { return cameraConfiguration; }
            set { cameraConfiguration = value; }
        }

        public VirtualCameraListForm()
        {
            InitializeComponent();

            detectAllButton.Text = StringManager.GetString(this.GetType().FullName,detectAllButton.Text);
            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void VirtualCameraListForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < requiredNumCamera; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(cameraInfoGrid);
                row.SetValues(0, 0, 0, false, "", "EDIT");

                row.Tag = cameraConfiguration.CameraInfos[i];
                cameraInfoGrid.Rows.Add(row);
            }

            UpdateData();
        }

        private void UpdateData()
        {
            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                CameraInfo cameraInfo = (CameraInfo)row.Tag;
                cameraInfo.Index = row.Index;
                row.SetValues(cameraInfo.Index, cameraInfo.Width, cameraInfo.Height, cameraInfo.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed, cameraInfo.VirtualImagePath, "EDIT");
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                //if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
                //{
                //    CameraInfo cameraInfo = new CameraInfo();
                //    cameraInfo.Index = int.Parse(row.Cells[0].Value.ToString());
                //    cameraInfo.Width = int.Parse(row.Cells[1].Value.ToString());
                //    cameraInfo.Height = int.Parse(row.Cells[2].Value.ToString());
                //    cameraInfo.PixelFormat = (bool.Parse(row.Cells[3].Value.ToString()) == true ? System.Drawing.Imaging.PixelFormat.Format24bppRgb : System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                //    cameraConfiguration.AddCameraInfo(cameraInfo);
                //}
                CameraInfo cameraInfo = (CameraInfo)row.Tag;
                cameraConfiguration.SetCameraInfo(cameraInfo);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            
            if (UiHelper.ShowSTADialog(dialog) == DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath) == false)
                    return;
                uint cameraIndex = 0;
                
                foreach (DataGridViewRow row in cameraInfoGrid.Rows)
                {
                    CameraInfo cameraInfo = (CameraInfo)row.Tag;
                    //uint cameraIndex = uint.Parse(row.Cells[0].Value.ToString());
                    //string searchPattern = string.Format("Image_C{0:00}_*.*", cameraIndex);
                    string searchPattern = string.Format("Image_C{0:00}*.*", cameraIndex);
                    //string searchPattern = string.Format("*.bmp");

                    String[] filePaths = Directory.GetFiles(dialog.SelectedPath, searchPattern);
                    if (filePaths.Count() == 0)
                        continue;

                    Bitmap defaultImage = new Bitmap(filePaths[0]);
                    cameraInfo.Width = defaultImage.Width;
                    cameraInfo.Height = defaultImage.Height;
                    cameraInfo.PixelFormat = defaultImage.PixelFormat;
                }
                UpdateData();
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != cameraInfoGrid.ColumnCount-1)
                return;

            CameraInfo cameraInfo = (CameraInfo)cameraInfoGrid.Rows[e.RowIndex].Tag;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfo;
            propertyGrid.Dock = DockStyle.Fill;

            Form form = new Form();
            form.Size = new System.Drawing.Size(480, 960);
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
