using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class GrabberFactory
    {
        public static Grabber Create(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Create Grabber");

            Grabber grabber = null;
            switch (grabberInfo.Type)
            {
                case GrabberType.Pylon:
                    grabber = new GrabberPylon(grabberInfo.Name);
                    break;

                case GrabberType.Pylon2:
                    grabber = new GrabberPylon2(grabberInfo.Name);
                    break;

                case GrabberType.PylonLine:
                    grabber = new GrabberPylonLine(grabberInfo.Name);
                    break;

                case GrabberType.Virtual:
                    grabber = new GrabberVirtual(grabberInfo.Name);
                    break;
                case GrabberType.MultiCam:
                    grabber = new GrabberMultiCam(grabberInfo.Name);
                    break;
                case GrabberType.MIL:
                    grabber = new GrabberMil(grabberInfo.Name);
                    break;
                case GrabberType.GenTL:
                    grabber = new GrabberGenTL(grabberInfo.Name);
                    break;
                case GrabberType.File:
                    grabber = new GrabberFile(grabberInfo.Name);
                    break;
                case GrabberType.Sapera:
                    grabber = new GrabberSapera(grabberInfo.Name);
                    break;
                case GrabberType.HIKLine:
                    grabber = new GrabberHIKLine(grabberInfo.Name);
                    break;
            }

            if (grabber == null)
            {
                ErrorManager.Instance().Report(ErrorCodeGrabber.Instance.FailToCreate, ErrorLevel.Error,
                    grabberInfo.Name, "Can't create grabber. {0}", new object[] { grabberInfo.Type.ToString() });
                return null;
            }
            grabber.NumCamera = grabberInfo.NumCamera;
            grabber.UpdateState(DeviceState.Ready, "Grabber Create succeeded.");

            //if (grabber.Initialize(grabberInfo) == false)
            //{
            //    ErrorManager.Instance().Report(ErrorSections.Grabber, (int)CommonError.FailToInitialize, ErrorLevel.Error,
            //        ErrorSections.Grabber.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't initialize grabber. {0}", grabberInfo.Type.ToString()));

            //    grabber = new GrabberVirtual(grabberInfo.Type, grabberInfo.Name);
            //    grabber.UpdateState(DeviceState.Error, "Grabber is invalid.");
            //}
            //else
            //{
            //    grabber.UpdateState(DeviceState.Ready, "Grabber initialization succeeded.");
            //}

            DeviceManager.Instance().AddDevice(grabber);

            return grabber;
        }
    }
}
