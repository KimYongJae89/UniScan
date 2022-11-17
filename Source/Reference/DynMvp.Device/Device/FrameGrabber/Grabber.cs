using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using System.Xml;
using DynMvp.Devices.FrameGrabber.UI;

namespace DynMvp.Devices.FrameGrabber
{
    public enum GrabberType
    {
        Virtual, Pylon, MultiCam, uEye, MIL, GenTL, File, Sapera, Pylon2, PylonLine, HIKLine
    }

    public class GrabberInfo
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        GrabberType type;
        public GrabberType Type
        {
            get { return type;  }
            set { type = value; }
        }

        int numCamera;
        public int NumCamera
        {
            get { return numCamera; }
            set { numCamera = value; }
        }

        public GrabberInfo()
        {

        }

        public GrabberInfo(string name, GrabberType type)
        {
            this.name = name;
            this.type = type;
        }

        public GrabberInfo(string name, GrabberType type, int numCamera)
        {
            this.name = name;
            this.type = type;
            this.numCamera = numCamera;
        }

        public void LoadXml(XmlElement grabberElement)
        {
            name = XmlHelper.GetValue(grabberElement, "Name", "");
            type = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(grabberElement, "Type", "Pylon"));
            numCamera = Convert.ToInt32(XmlHelper.GetValue(grabberElement, "NumCamera", ""));
        }

        public void SaveXml(XmlElement grabberElement)
        {
            XmlHelper.SetValue(grabberElement, "Name", name.ToString());
            XmlHelper.SetValue(grabberElement, "Type", type.ToString());
            XmlHelper.SetValue(grabberElement, "NumCamera", numCamera.ToString());
        }

        public GrabberInfo Clone()
        {
            GrabberInfo grabberInfo = new GrabberInfo();
            grabberInfo.Copy(this);

            return grabberInfo;
        }

        public virtual void Copy(GrabberInfo srcGrabberInfo)
        {
            name = srcGrabberInfo.name;
            type = srcGrabberInfo.type;
            numCamera = srcGrabberInfo.numCamera;
        }
    }

    public class GrabberInfoList : List<GrabberInfo>
    {
        public int NumCamera
        {
            get
            {
                int numCamera = 0;
                foreach (GrabberInfo grabberInfo in this)
                {
                    numCamera += grabberInfo.NumCamera;
                }

                return numCamera;
            }
        }

        public GrabberInfoList Clone()
        {
            GrabberInfoList newGrabberInfoList = new GrabberInfoList();

            foreach (GrabberInfo grabberInfo in this)
            {
                newGrabberInfoList.Add(grabberInfo.Clone());
            }

            return newGrabberInfoList;
        }
    }


    public abstract class Grabber : Device
    {
        GrabberType type;
        public GrabberType Type
        {
            get { return type; }
            set { type = value; }
        }

        int numCamera;
        public int NumCamera
        {
            get { return numCamera; }
            set { numCamera = value; }
        }

        public bool CalibrationMode => this.calibrationMode;
        bool calibrationMode;

        public Grabber(GrabberType grabberType, string name)
        {
            if (name == "")
                Name = grabberType.ToString();
            else
                Name = name;

            DeviceType = DeviceType.FrameGrabber;
            this.type = grabberType;
            UpdateState(DeviceState.Idle, "Created");
        }

        public abstract Camera CreateCamera(CameraInfo cameraInfo);

        public abstract bool Initialize(GrabberInfo grabberInfo);
        //public abstract void UpdateCameraInfo(CameraInfo cameraInfo);

        public virtual bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            GeneralCameraListForm form = new GeneralCameraListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }
    }

    public class GrabberList : List<Grabber>
    {
        public Grabber GetGrabber(string name)
        {
            return Find(x => x.Name == name);
        }

        public Grabber GetGrabber(GrabberType grabberType)
        {
            return Find(x => x.Type == grabberType);
        }

        public void Release()
        {
            foreach (Grabber grabber in this)
            {
                if(grabber.IsReady())
                    grabber.Release();
            }
            this.Clear();
        }

        public void Initialize(GrabberInfoList grabberInfoList, bool isVirtualMode)
        {
            foreach (GrabberInfo grabberInfo in grabberInfoList)
            {
                if (isVirtualMode)
                {
                    GrabberInfo virtualGrabberInfo = new GrabberInfo(grabberInfo.Name, GrabberType.Virtual, grabberInfo.NumCamera);
                    Grabber grabber = GrabberFactory.Create(virtualGrabberInfo);
                    Add(grabber);
                }
                else
                {
                    Grabber grabber = GrabberFactory.Create(grabberInfo);
                    grabber.Initialize(grabberInfo);
                    Add(grabber);
                }
            }
        }
    }
}
