using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynMvp.Base;
using UniEye.Base.Device;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.FrameGrabber;
using System.Threading;
using DynMvp.UI.Touch;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScanX.MPAlignment.Devices
{
    class DeviceController  : UniEye.Base.Device.DeviceController
    {
        protected ManualResetEvent isGrabDone = new ManualResetEvent(false);

        public override void Initialize(DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);
            var icore = LightCtrl; //초기화 목적
            MacroCamera.SetExposureTime(16666.6f);
            MicroCamera.SetExposureTime(33333.3f);
        }


        public CameraPylon2 MacroCamera
        {
            get { return SystemManager.Instance().DeviceBox.ImageDeviceHandler[0] as CameraPylon2; }
        }

        public CameraPylon2 MicroCamera
        {
            get { return SystemManager.Instance().DeviceBox.ImageDeviceHandler[1] as CameraPylon2; }
        }

        public IPulse LightCtrl
        {
            get
            {
                return //_iPulse; }
                  GetLightCtrl();
            }
        }

        static protected IPulse _iPulse = null;
        public IPulse GetLightCtrl()
        {
            if (_iPulse != null) return _iPulse;
            else
            {
                var lightctrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
                _iPulse = new IPulse(lightctrl);
            }
            return _iPulse;
        }

        public AxisHandler Robot
        {
            get { return SystemManager.Instance().DeviceController.RobotStage; }
        }

        public static void RobotZ()
        {

        }
        public void CheckOrigin(bool userQuary)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return;

            if (axisHandler.IsAllHomeDone())
                return;

            bool needHome = true;
            if (userQuary)
                needHome = (MessageForm.Show(null, "Origin Move?", "UniScan", MessageFormType.YesNo, 5000, DialogResult.Yes) == DialogResult.Yes);

            if (needHome)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                SimpleProgressForm form = new SimpleProgressForm("Origin");
                form.Show(() =>
                {
                    Task homeTask = axisHandler.StartHomeMove();
                    homeTask.Wait(100);
                    axisHandler.WaitHomeDone(cancellationTokenSource);
                }, cancellationTokenSource);
            }
        }

        public ImageD Grab_MicroIR(int lightVal)
        {
            LightCtrl.LightOn(ICoreChannel.IR, lightVal);
            MicroCamera.GrabOnce();//카메라 스트로보 신호가 출력되면, 조명컨트롤러에서 해당 조명이 켜진다.
            MicroCamera.WaitGrabDone();
            LightCtrl.LightOffAll();
            return null;
        }

        public ImageD Grab_MicroBLUE(int lightVal)
        {
            LightCtrl.LightOn(ICoreChannel.Blue, lightVal);
            MicroCamera.GrabOnce(); //카메라 스트로보 신호가 출력되면, 조명컨트롤러에서 해당 조명이 켜진다.
            MicroCamera.WaitGrabDone();
            LightCtrl.LightOffAll();
            return null;
        }

        //public ImageD[] Grab_MicroIrBlue(int IRval, int BLUEval)
        //{
        //    base.LightCtrl.SetLightValue(ICoreChannel.IR, IRval);
        //    base.LightCtrl.SetLightValue(ICoreChannel.Blue, BLUEval);
        //    return null;
        //}

        //private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        //{
        //    isGrabDone.Set();
        //    ImageD imageD = imageDevice.GetGrabbedImage(ptr);
           
        //    var DisplayBitmap = imageD.ToBitmap();
        //}

        public ImageD Grab_Macro(int lightVal)
        {
            LightCtrl.LightOn(ICoreChannel.White, lightVal);

           // MacroCamera.ImageGrabbed += ImageGrabbed;

            MacroCamera.GrabOnceSync();
        //    bool ok =  MacroCamera.WaitGrabDone(1000);
        //   MacroCamera.Stop();

            LightCtrl.LightOffAll();

            return MacroCamera.GetGrabbedImage();
        }

        public void MoveXYZ(float mmX, float mmY, float mmZ)
        {
            MoveXY(mmX, mmY);
            MoveZ(mmZ);
            Robot.WaitMoveDone();
        }

        public void MoveXY(float mmX, float mmY)
        {
            
            AxisPosition pos = Robot.GetActualPos();
            pos.Position[0] = mmX * 1000;
            pos.Position[1] = mmY * 1000;
            //pos.Position[2] = mmZ * 1000;
            Robot.Move(pos);
            Robot.WaitMoveDone();
        }

        public void MoveZ(float mmZ)
        {
            AxisPosition pos = Robot.GetActualPos();
            pos[3] = mmZ * 1000;
            Robot.Move(pos);
            Robot.WaitMoveDone();
        }

        public void Homming()
        {

        }

        public float AutoFocus()
        {
            float focusedPos = 0.0f;
            return focusedPos;
        }
    }
}
