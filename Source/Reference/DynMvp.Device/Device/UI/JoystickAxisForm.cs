using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.UI
{
    public partial class JoystickAxisForm : Form
    {
        bool initialized = false;
        ImageDevice imageDevice;
        CameraView cameraView;
        IJoystickControl joystickControl;
        LightCtrlHandler lightCtrlHandler;
        LightParamSet lightParamSet = null;

        public JoystickAxisForm(AxisHandler axisHandler, ImageDevice imageDevice, LightCtrlHandler lightCtrlHandler)
        {
            InitializeComponent();
            int numAxis = 0;
            if (axisHandler != null)
            {
                numAxis = axisHandler.NumUniqueAxis;
            }

            switch (numAxis)
            {
                case 2:
                    this.joystickControl = new Joystick2AxisControl();
                    break;
                case 3:
                    this.joystickControl = new Joystick3AxisControl();
                    break;
                default:
                    this.joystickControl = null;
                    break;
            }

//            panelImage.Controls.Add(cameraView);

            this.lightCtrlHandler = lightCtrlHandler;
            this.imageDevice = imageDevice;

            //cameraView = new CameraView();
            //cameraView.Dock = DockStyle.Fill;
            //cameraView.SizeMode = PictureBoxSizeMode.Zoom;

            if (this.joystickControl != null)
            {
                this.joystickControl.InitControl();
                this.joystickControl.Initialize(axisHandler);
                this.panelJoystick.Controls.Add((UserControl)joystickControl);

                axisHandler.OnEndMove += axisHandler_RobotMoved;

                initialized = true;
            }

            comboLightType.SelectedIndex = 0;
        }

        public void ToggleView(IWin32Window parentWnd, LightParamSet lightParamSet)
        {
            if (Visible)
                Hide();
            else
            {
                this.lightParamSet = lightParamSet;
                Show(parentWnd);
            }
        }

        private void axisHandler_RobotMoved(AxisPosition axisPosition)
        {
            if (Visible == false || lightCtrlHandler == null || lightParamSet == null)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new RobotEventDelegate(axisHandler_RobotMoved), axisPosition);
                return;
            }

            LightParam lightParam = lightParamSet.LightParamList[comboLightType.SelectedIndex];
            LightValue lightValue = lightParam.LightValue;
            lightCtrlHandler.TurnOn(lightValue);

            imageDevice.GrabOnce();
            imageDevice.WaitGrabDone();

            ImageD grabImage = imageDevice.GetGrabbedImage(IntPtr.Zero);

            pictureImage.Image = grabImage.ToBitmap();
            //cameraView.Invalidate();

            lightCtrlHandler.TurnOff();
        }

        //public void Initialize(AxisHandler axisHandler, ImageDevice imageDevice)
        //{
        //    cameraView.Tag = imageDevice;
        //    cameraView.SetImageDevice(imageDevice);

        //    this.joystickControl.Initialize(axisHandler);
        //    this.initialized = true;
        //}

        private void joystickControl_RobotMoved(AxisPosition axisPosition)
        {
            throw new NotImplementedException();
        }

        private void JoystickAxisForm_Load(object sender, EventArgs e)
        {
            if (!initialized)
            {
                this.Close();
                return;
            }
        }

        private void JoystickAxisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
