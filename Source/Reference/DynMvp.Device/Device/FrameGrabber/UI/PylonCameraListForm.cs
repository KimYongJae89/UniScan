using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.Base;

using PylonC.NET;
using PylonC.NETSupportLibrary;
using DynMvp.UI;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class PylonCameraListForm : Form
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

        public PylonCameraListForm()
        {
            InitializeComponent();

            autoDetectButton.Text = StringManager.GetString(this.GetType().FullName,autoDetectButton.Text);
            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void PylonCameraListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                CameraInfoPylon cameraInfoPylon = (CameraInfoPylon)cameraInfo;
                int i = cameraInfoGrid.Rows.Add(cameraInfoPylon.Index, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, cameraInfoPylon.ModelName,
                                                cameraInfoPylon.Width, cameraInfoPylon.Height, (cameraInfoPylon.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? "1" : "3"),
                                                cameraInfoPylon.RotateFlipType.ToString(),
                                                cameraInfoPylon.UseNativeBuffering, "Edit");
                cameraInfoGrid.Rows[i].Tag = cameraInfoPylon;
            }

            int index = cameraInfoGrid.Rows.Count;
            while (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add(index.ToString());
                index++;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    CameraInfoPylon cameraInfoPylon = (CameraInfoPylon)row.Tag;
                    if (cameraInfoPylon != null)
                    {
                        cameraInfoPylon.Index = int.Parse(row.Cells[0].Value.ToString());
                        cameraInfoPylon.DeviceUserId = row.Cells[1].Value.ToString();
                        cameraInfoPylon.IpAddress = row.Cells[2].Value.ToString();
                        cameraInfoPylon.SerialNo = row.Cells[3].Value.ToString();
                        cameraInfoPylon.ModelName = row.Cells[4].Value.ToString();
                        cameraInfoPylon.Width = int.Parse(row.Cells[5].Value.ToString());
                        cameraInfoPylon.Height = int.Parse(row.Cells[6].Value.ToString());
                        if (int.Parse(row.Cells[7].Value.ToString()) == 1)
                            cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                        else
                            cameraInfoPylon.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppRgb;
                        cameraInfoPylon.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), row.Cells[8].Value.ToString());

                        cameraInfoPylon.UseNativeBuffering = Convert.ToBoolean(row.Cells[9].Value);

                        cameraConfiguration.SetCameraInfo(cameraInfoPylon);
                    }
                }
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Auto Detect Camera(s)");

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "5000");

            PylonC.NET.Pylon.Initialize();

            List<DeviceEnumerator.Device> deviceList = DeviceEnumerator.EnumerateDevices();

            LogHelper.Debug(LoggerType.StartUp, String.Format("{0} camera(s) are detected.", deviceList.Count));

            string serialNo;
            string ipAddress;
            string deviceUserId;
            string modelName;

            cameraInfoGrid.Rows.Clear();

            int index = 0;
            foreach(DeviceEnumerator.Device device in deviceList)
            {
                GrabberPylon.GetFeature(device.Tooltip, out deviceUserId, out ipAddress, out serialNo, out modelName);

                CameraInfoPylon cameraInfoPylon = new CameraInfoPylon();
                cameraInfoPylon.Index = index;
                cameraInfoPylon.DeviceUserId = deviceUserId;
                cameraInfoPylon.IpAddress = ipAddress;
                cameraInfoPylon.SerialNo = serialNo;
                cameraInfoPylon.ModelName = modelName;
                cameraInfoPylon.DeviceIndex = device.Index;

                cameraConfiguration.SetCameraInfo(cameraInfoPylon);

                CameraPylon cameraPylon = new CameraPylon(cameraInfoPylon);
                cameraPylon.Initialize(true);

                int rowindex = cameraInfoGrid.Rows.Add(//index, deviceUserId, ipAddress, serialNo, modelName,
                                                        cameraInfoPylon.Index,
                                                        cameraInfoPylon.DeviceUserId,
                                                        cameraInfoPylon.IpAddress,
                                                        cameraInfoPylon.SerialNo,
                                                        cameraInfoPylon.ModelName,
                                                        cameraPylon.ImageSize.Width,
                                                        cameraPylon.ImageSize.Height,
                                                        cameraPylon.NumOfBand.ToString(),
                                                        System.Drawing.RotateFlipType.RotateNoneFlipNone.ToString());

                cameraInfoGrid.Rows[rowindex].Tag = cameraInfoPylon;
                cameraPylon.Release();

                cameraInfoGrid.Rows[index].Cells[0].ToolTipText = device.Tooltip;
                cameraInfoGrid.Rows[index].Cells[1].ToolTipText = device.Tooltip;
                cameraInfoGrid.Rows[index].Cells[2].ToolTipText = device.Tooltip;

                index++;
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera && requiredNumCamera > 0)
            {
                while (cameraInfoGrid.Rows.Count < requiredNumCamera)
                {
                    cameraInfoGrid.Rows.Add(index.ToString());
                    index++;
                }
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
            if (e.ColumnIndex != 10)
                return;

            CameraInfoPylon cameraInfo = (CameraInfoPylon)cameraInfoGrid.Rows[e.RowIndex].Tag;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfo;
            propertyGrid.Dock = DockStyle.Fill;

            Form form = new Form();
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
