using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;

using DynMvp.UI;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class PylonCameraListForm2 : Form
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

        public PylonCameraListForm2()
        {
            InitializeComponent();

            autoDetectButton.Text = StringManager.GetString(autoDetectButton.Text);
            buttonMoveUp.Text = StringManager.GetString(buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(buttonCancel.Text);

        }

        private void PylonCameraListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            this.cameraInfoGrid.Rows.Clear();
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                CameraInfoPylon2 cameraInfoPylon = cameraInfo as CameraInfoPylon2;
                if (cameraInfoPylon != null)
                {
                    int numBand = cameraInfo.GetNumBand();
                    int i = cameraInfoGrid.Rows.Add(
                        cameraInfoPylon.Index,
                        cameraInfoPylon.DeviceUserId,
                        cameraInfoPylon.IpAddress,
                        cameraInfoPylon.SerialNo,
                        cameraInfoPylon.ModelName,
                        cameraInfoPylon.Width,
                        cameraInfoPylon.Height,
                        numBand.ToString(),
                        cameraInfoPylon.RotateFlipType.ToString(),
                        "Edit");
                    cameraInfoGrid.Rows[i].Tag = cameraInfoPylon;
                }
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
                    CameraInfoPylon2 cameraInfoPylon = new CameraInfoPylon2();
                    cameraInfoPylon.Index = int.Parse(row.Cells[0].Value.ToString());
                    cameraInfoPylon.DeviceUserId = row.Cells[1].Value.ToString();
                    cameraInfoPylon.IpAddress = row.Cells[2].Value.ToString();
                    cameraInfoPylon.SerialNo = row.Cells[3].Value.ToString();
                    cameraInfoPylon.ModelName = row.Cells[4].Value.ToString();
                    cameraInfoPylon.Width = int.Parse(row.Cells[5].Value.ToString());
                    cameraInfoPylon.Height = int.Parse(row.Cells[6].Value.ToString());

                    int numBand = int.Parse(row.Cells[7].Value.ToString());
                    cameraInfoPylon.SetNumBand(numBand);
                    cameraInfoPylon.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), row.Cells[8].Value.ToString());
                    cameraConfiguration.SetCameraInfo(cameraInfoPylon);
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

            Environment.SetEnvironmentVariable("PYLON_GIGE_HEARTBEAT", "3000"); // ??
            IList<Basler.Pylon.ICameraInfo> deviceList = Basler.Pylon.CameraFinder.Enumerate();
            LogHelper.Debug(LoggerType.StartUp, String.Format("{0} camera(s) are detected.", deviceList.Count));

            cameraInfoGrid.Rows.Clear();

            int index = 0;
            foreach(Basler.Pylon.ICameraInfo device in deviceList)
            {
                //string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];
                //string ipAddress = device[Basler.Pylon.CameraInfoKey.DeviceSocketAddress];
                //string[] splitIpAddress = ipAddress.Split(':');
                //ipAddress = splitIpAddress[0];
                //string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                //string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                //CameraInfoPylon cameraInfoPylon = new CameraInfoPylon();
                //cameraInfoPylon.DeviceIndex = (uint)index; //uint.Parse(device[Basler.Pylon.CameraInfoKey.DeviceIdx]);
                //cameraInfoPylon.IpAddress = ipAddress;
                //cameraInfoPylon.SerialNo = serialNo;

                string deviceUserId = device[Basler.Pylon.CameraInfoKey.FriendlyName];
                string ipAddress = device[Basler.Pylon.CameraInfoKey.DeviceIpAddress];
                string serialNo = device[Basler.Pylon.CameraInfoKey.SerialNumber];
                string modelName = device[Basler.Pylon.CameraInfoKey.ModelName];

                CameraInfoPylon2 cameraInfoPylon = new CameraInfoPylon2();
                cameraInfoPylon.DeviceUserId = deviceUserId;
                cameraInfoPylon.IpAddress = ipAddress;
                cameraInfoPylon.SerialNo = serialNo;
                cameraInfoPylon.ModelName = modelName;

                CameraPylon2 cameraPylon = new CameraPylon2(cameraInfoPylon);
                cameraPylon.Initialize(true);

                cameraInfoGrid.Rows.Add(index, deviceUserId, ipAddress, serialNo, modelName, cameraPylon.ImageSize.Width, cameraPylon.ImageSize.Height, cameraPylon.NumOfBand.ToString(), RotateFlipType.RotateNoneFlipNone.ToString());

                cameraPylon.Release();

                //cameraInfoGrid.Rows[index].Cells[0].ToolTipText = device[Basler.Pylon.CameraInfoKey.DeviceType];
                //cameraInfoGrid.Rows[index].Cells[1].ToolTipText = device[Basler.Pylon.CameraInfoKey.DeviceVersion];
                //cameraInfoGrid.Rows[index].Cells[2].ToolTipText = "null";//device[Basler.Pylon.CameraInfoKey.ManufacturerInfo];

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

        private void ButtonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void ButtonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = this.columnEdit.Index;
            if (e.ColumnIndex != columnIndex)
                return;

            CameraInfoPylon2 cameraInfoPylon2 = (CameraInfoPylon2)this.cameraInfoGrid.Rows[e.RowIndex].Tag;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfoPylon2;
            propertyGrid.Dock = DockStyle.Fill;

            Form form = new Form();
            form.Size = new System.Drawing.Size(480, 960);
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
