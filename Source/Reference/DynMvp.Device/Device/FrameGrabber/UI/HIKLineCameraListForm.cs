using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MvCamCtrl.NET;
using DynMvp.Base;
using PylonC.NET;
using PylonC.NETSupportLibrary;
using DynMvp.UI;
using System.Runtime.InteropServices;
using System.Net;


namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class HIKLineCameraListForm : Form
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

        public HIKLineCameraListForm()
        {
            InitializeComponent();

            autoDetectButton.Text = StringManager.GetString(this.GetType().FullName, autoDetectButton.Text);
            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName, buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName, buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName, buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName, buttonCancel.Text);

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
                CameraInfoHIKLine cameraInfoHIK = (CameraInfoHIKLine)cameraInfo;
                int i = cameraInfoGrid.Rows.Add(
                    cameraInfoHIK.Index, 
                    cameraInfoHIK.DeviceUserId, 
                    cameraInfoHIK.IpAddress, 
                    cameraInfoHIK.SerialNo, 
                    cameraInfoHIK.ModelName,
                    cameraInfoHIK.Width, 
                    cameraInfoHIK.Height, 
                    (cameraInfoHIK.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed ? "1" : "3"),
                    cameraInfoHIK.RotateFlipType.ToString(),
                    cameraInfoHIK.UseNativeBuffering, "Edit");

                cameraInfoGrid.Rows[i].Tag = cameraInfoHIK;
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
                    CameraInfoHIKLine cameraInfo = (CameraInfoHIKLine)row.Tag;
                    if (cameraInfo != null)
                    {
                        cameraInfo.Index = int.Parse(row.Cells[0].Value.ToString());
                        cameraInfo.DeviceUserId = row.Cells[1].Value.ToString();
                        cameraInfo.IpAddress = row.Cells[2].Value.ToString();
                        cameraInfo.SerialNo = row.Cells[3].Value.ToString();
                        cameraInfo.ModelName = row.Cells[4].Value.ToString();
                        cameraInfo.Width = int.Parse(row.Cells[5].Value.ToString());
                        cameraInfo.Height = int.Parse(row.Cells[6].Value.ToString());
                        if (int.Parse(row.Cells[7].Value.ToString()==""? "1": row.Cells[7].Value.ToString() ) == 1)
                            cameraInfo.PixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                        else
                            cameraInfo.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppRgb;
                        cameraInfo.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), row.Cells[8].Value.ToString());

                        cameraInfo.UseNativeBuffering = Convert.ToBoolean(row.Cells[9].Value);

                        cameraConfiguration.SetCameraInfo(cameraInfo);
                    }
                }
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Size GetImageSize(ref MyCamera.MV_CC_DEVICE_INFO device)
        {
            int nRet = 0;
            Size size = new Size();
            var heightValue = new MyCamera.MVCC_INTVALUE();
            var widthValue = new MyCamera.MVCC_INTVALUE();

            var tempCamera = new MyCamera();
            nRet = tempCamera.MV_CC_CreateDevice_NET(ref device);
            nRet = tempCamera.MV_CC_OpenDevice_NET();

            int nRet1 = tempCamera.MV_CC_GetHeight_NET(ref heightValue);
            int nRet2 = tempCamera.MV_CC_GetWidth_NET(ref widthValue);

            nRet = tempCamera.MV_CC_DestroyDevice_NET();

            size.Width = (int)widthValue.nCurValue;
            size.Height = (int)heightValue.nCurValue;

            return size;
        }

        public static uint ConvertFromIpAddressToInteger(string ipAddress)
        {
            var address = IPAddress.Parse(ipAddress);
            byte[] bytes = address.GetAddressBytes();
            // flip big-endian(network order) to little-endian     
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static string ConvertFromIntegerToIpAddress(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);
            // flip little-endian to big-endian(network order)     
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return new IPAddress(bytes).ToString();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Auto Detect Camera(s)");
            var m_stDeviceList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            int nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref m_stDeviceList);
            LogHelper.Debug(LoggerType.StartUp, string.Format("{0} camera(s) are detected.", nRet));

            cameraInfoGrid.Rows.Clear();

            int index = 0;
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                var device = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));


            if (device.nTLayerType == MyCamera.MV_GIGE_DEVICE)
                {
                    var gigeInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                    string ipAddress = ConvertFromIntegerToIpAddress(gigeInfo.nCurrentIp);//IPAddress.Parse()
                    var size = GetImageSize(ref device);

                    CameraInfoHIKLine cameraInfo = new CameraInfoHIKLine();
                    cameraInfo.Index = index;
                    //cameraInfo.DeviceIndex = device.Index;
                    cameraInfo.DeviceUserId = gigeInfo.chUserDefinedName;
                    cameraInfo.IpAddress = ipAddress;
                    cameraInfo.SerialNo = gigeInfo.chSerialNumber;
                    cameraInfo.ModelName = gigeInfo.chModelName;

                    //CameraPylon cameraDev = new CameraPylon(cameraInfoPylon);
                    //cameraDev.Initialize(true);

                    int rowindex = cameraInfoGrid.Rows.Add(  //1no, 2id, 3ip, 4sn, 5modelname, 6width, 7height, 8band, 9rotate, 10 native buf, 11 edit
                                                            cameraInfo.Index, //1
                                                            cameraInfo.DeviceUserId, //2
                                                            cameraInfo.IpAddress,  //3
                                                            cameraInfo.SerialNo,  //4
                                                            cameraInfo.ModelName,  //5
                                                            size.Width,  //6
                                                            size.Height,  //7
                                                            "1",//cameraDev.NumOfBand.ToString(), //8                                                            //cameraInfo.PixelFormat,
                                                            System.Drawing.RotateFlipType.RotateNoneFlipNone.ToString()); //9
                    cameraInfoGrid.Rows[rowindex].Tag = cameraInfo;
                   // cameraDev.Release();

                }
                else if (device.nTLayerType == MyCamera.MV_USB_DEVICE)
                {
                    //var usbInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(device.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    //if (usbInfo.chUserDefinedName != "")
                    //{
                    //    cameraInfoGrid.Rows.Add(i, usbInfo.chUserDefinedName,
                    //        widthValue.nCurValue, heightValue.nCurValue,
                    //        usbInfo.chSerialNumber, usbInfo.chModelName);
                    //}
                    //else
                    //{
                    //    cameraInfoGrid.Rows.Add(i, "", widthValue.nCurValue, heightValue.nCurValue,
                    //        usbInfo.chSerialNumber, usbInfo.chModelName);
                    //}
                }

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

            CameraInfoHIKLine cameraInfo = (CameraInfoHIKLine)cameraInfoGrid.Rows[e.RowIndex].Tag;

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
