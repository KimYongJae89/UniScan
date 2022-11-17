using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Devices
{
    public enum DeviceType
    {
        FrameGrabber, MotionController, DigitalIo, LightController, DaqChannel, Camera, DepthScanner, BarcodeReader, BarcodePrinter
    }

    public enum DeviceState
    {
        Idle, Ready, Warning, Error,Disposed
    }

    public abstract class Device : IDisposable
    {
        protected string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        protected DeviceType deviceType;
        public DeviceType DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        protected DeviceState state;
        public DeviceState State
        {
            get { return state; }
            set { state = value; }
        }

        String stateMessage;
        public String StateMessage
        {
            get { return stateMessage; }
            set { stateMessage = value; }
        }

        public bool IsReady () { return (state == DeviceState.Ready); }

        public bool IsError() { return (state == DeviceState.Error); }

        public Device()
        {
            UpdateState(DeviceState.Idle);
        }

        public void UpdateState(DeviceState state, String stateMessage = "")
        {
            this.state = state;
            this.stateMessage = stateMessage;
        }

        public virtual void Release()
        {
            DeviceManager.Instance().RemoveDevice(this);
            UpdateState(DeviceState.Disposed);
        }

        public void Dispose()
        {
            Release();
        }
    }

    public class DeviceManager
    {
        static DeviceManager instance = null;
        public static DeviceManager Instance()
        {
            if (instance == null)
            {
                instance = new DeviceManager();
            }

            return instance;
        }

        List<Device> deviceList = new List<Device>();
        public List<Device> DeviceList
        {
            get { return deviceList; }
        }

        private DeviceManager()
        {

        }

        //private DeviceFactory GetDeviceFactory(DeviceType deviceType)
        //{
        //    switch(deviceType)
        //    {
        //        case DeviceType.FrameGrabber:
        //            return new FrameGrabber.GrabberFactory();
        //        case DeviceType.MotionController:
        //            return new MotionController.MotionFactory();
        //    }

        //    return null;
        //}

        //public void LoadConfiguration(string fileName)
        //{
        //    LogHelper.Debug(LoggerType.StartUp, "Load Device Configuration");

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.Load(fileName);

        //    XmlElement deviceListElement = xmlDocument.DocumentElement;
        //    foreach (XmlElement deviceElement in deviceListElement)
        //    {
        //        if (deviceElement.Name == "Device")
        //        {
        //            DeviceType deviceType = (DeviceType)Enum.Parse(typeof(DeviceType), XmlHelper.GetValue(deviceElement, "DeviceType", "FrameGrabber"));
        //            DeviceFactory deviceFactory = GetDeviceFactory(deviceType);
        //            if (deviceFactory == null)
        //                continue;

        //            Device device = deviceFactory.CreateDevice(deviceElement);
        //            device.Initialize(deviceElement);

        //            deviceList.Add(device);
        //        }
        //    }
        //}

        //public void SaveConfiguration(string fileName)
        //{
        //    LogHelper.Debug(LoggerType.StartUp, "Save Device Configuration");

        //    XmlDocument xmlDocument = new XmlDocument();

        //    XmlElement deviceListElement = xmlDocument.CreateElement("", "DeviceList", "");
        //    xmlDocument.AppendChild(deviceListElement);

        //    foreach (Device device in deviceList)
        //    {
        //        XmlElement deviceElement = xmlDocument.CreateElement("", "Device", "");
        //        deviceListElement.AppendChild(deviceElement);

        //        device.SaveXml(deviceElement);
        //    }

        //    xmlDocument.Save(fileName);
        //}

        public void AddDevice(Device device)
        {
            deviceList.Add(device);
        }

        public void RemoveDevice(Device device)
        {
            deviceList.Remove(device);
        }

        public void UpdateDeviceState(string name, DeviceState state, string stateMessage = "")
        {
            Device device = deviceList.Find(x => x.Name == name);
            if (device != null)
            {
                device.State = state;
                device.StateMessage = stateMessage;
            }
        }

        public bool IsAllDeviceReady()
        {
            return deviceList.TrueForAll(f => f.IsReady());
        }

        public Device GetDevice(DeviceType deviceType, string name)
        {
            foreach (Device device in deviceList)
            {
                if (device.DeviceType == deviceType && device.Name == name)
                    return device;
            }

            return null;
        }

        public List<Device> GetDeviceList(DeviceType deviceType)
        {
            List<Device> subDeviceList = new List<Device>();
            foreach (Device device in deviceList)
            {
                if (device.DeviceType == deviceType)
                    subDeviceList.Add(device);
            }

            return subDeviceList;
        }

        public ImageDeviceHandler GetImageDeviceHandler()
        {
            ImageDeviceHandler imageDeviceHandler = new ImageDeviceHandler();
            
            return imageDeviceHandler;
        }
    }
}
