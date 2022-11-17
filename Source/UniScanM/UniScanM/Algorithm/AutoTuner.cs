using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;

namespace UniScanM.Algorithm
{
    public abstract class TuneValue
    {
        int deviceIndex;
        bool tuneComplete;
        List<Tuple<int, float>> valueList = new List<Tuple<int, float>>();

        public int DeviceIndex { get => deviceIndex; }
        public bool TuneComplete { get => tuneComplete; set => tuneComplete = value; }
        public List<Tuple<int, float>> ValueList { get => valueList; }

        public TuneValue(int deviceIndex)
        {
            this.deviceIndex = deviceIndex;
        }
        
        private void AddValue(int lightValue, float std)
        {
            lock (valueList)
                valueList.Add(new Tuple<int, float>(lightValue, std));
        }
     
        public void Tune(int lightValue, ImageD image)
        {
            AddValue(lightValue, GetValue(image));
            tuneComplete = true;
        }

        protected abstract float GetValue(ImageD image);
    }

    public enum AutoTuneType
    {
        Average, Otsu, Saturation, TopArea
    }

    public delegate void TuneProcessDelegate();
    public delegate void TuneDoneDelegate(List<ImageD> tuneDoneImageList);

    public class AutoTuner
    {
        int lightStep = 5;

        int curLightValue;
        Thread tuneThread;
        List<TuneValue> tuneValueList = new List<TuneValue>();
        public List<TuneValue> TuneValueList { get => tuneValueList; }

        public int CurLightValue { get => curLightValue; }
        
        public TuneProcessDelegate TuneProcessDelegate;
        public TuneDoneDelegate TuneDoneDelegate;
        
        ImageD[] tuneDoneImageArray;
        public AutoTuner(AutoTuneType type, object obj)
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            tuneDoneImageArray = new ImageD[imageDeviceHandler.Count];

            foreach (ImageDevice device in imageDeviceHandler)
            {
                switch (type)
                {
                    case AutoTuneType.Average:
                        break;
                    case AutoTuneType.Otsu:
                        TuneValueList.Add(new OtsuTuneValue(device.Index));
                        break;
                    case AutoTuneType.Saturation:
                        TuneValueList.Add(new SaturationTuneValue(device.Index, (float)obj));
                        break;
                }
            }
        }

        public float GetProgressPercent()
        {
            return (float)curLightValue / 255.0f * 100.0f;
        }

        public void Start()
        {
            curLightValue = 10;

            foreach (TuneValue value in TuneValueList)
                value.ValueList.Clear();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed += ImageGrabbed;

            //그랩
            tuneThread = new Thread(TuneProcess);
            tuneThread.Start();
        }

        public void Stop()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed -= ImageGrabbed;

            if (tuneThread != null && tuneThread.IsAlive == true)
                tuneThread.Abort();
        }

        private void TuneProcess()
        {
            float maxStdValue = 0;

            for (int i = 0; i < tuneDoneImageArray.Length; i++)
                tuneDoneImageArray[i] = null;

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            lightCtrlHandler.TurnOn(new LightValue(1, curLightValue));
            imageDeviceHandler.GrabOnce();

            while (true)
            {
                Thread.Sleep(0);

                bool completeTune = true;
                foreach (TuneValue tuneValue in tuneValueList)
                {
                    if (tuneValue.TuneComplete == false)
                    {
                        completeTune = false;
                        break;
                    }
                }
                
                if (completeTune == false)
                    continue;

                foreach (TuneValue tuneValue in TuneValueList)
                    tuneValue.TuneComplete = false;
                
                float stdValue = 0;
                foreach (TuneValue value in tuneValueList)
                    stdValue += value.ValueList.Find(v => v.Item1 == curLightValue).Item2;

                if (maxStdValue < stdValue)
                    maxStdValue = stdValue;
                else if (maxStdValue > stdValue)
                    break;


                if (TuneProcessDelegate != null)
                    TuneProcessDelegate();

                if (CurLightValue >= 255)
                    break;

                curLightValue += lightStep;
                lightCtrlHandler.TurnOn(new LightValue(1, curLightValue));

                Thread.Sleep(100);

                imageDeviceHandler.GrabOnce();
            }//while

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed -= ImageGrabbed;
                imageDevice.ImageGrabbed += TuneDoneImageGrabbed;
            }

            Thread.Sleep(500);
            imageDeviceHandler.GrabOnce();
        }

        private void TuneDoneImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            lock (this)
            {
                imageDevice.ImageGrabbed -= TuneDoneImageGrabbed;
                tuneDoneImageArray[imageDevice.Index] = imageDevice.GetGrabbedImage(ptr);
                tuneDoneImageArray[imageDevice.Index].ConvertFromPtr();

                if (TuneDoneDelegate != null)
                {
                    bool grabDone = true;
                    foreach (ImageD image in tuneDoneImageArray)
                        grabDone = image != null;
                    
                    if (grabDone == true)
                        Task.Factory.StartNew(() => TuneDoneDelegate(tuneDoneImageArray.ToList()));
                }
            }
        }

        private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            foreach (TuneValue tuneValue in tuneValueList)
            {
                if (imageDevice.Index == tuneValue.DeviceIndex)
                    tuneValue.Tune(CurLightValue, imageDevice.GetGrabbedImage(ptr));
            }
        }
        
        //큰 값 기준..
        public int GetMaxTuneLightValue()
        {
            int count = 255;
            foreach (TuneValue value in tuneValueList)
            {
                if (count > value.ValueList.Count)
                    count = value.ValueList.Count;
            }
                         
            float maxValue = 0;
            int maxLightValue = 0;
            for (int i = 0; i < count; i++)
            {
                float stdValue = 0;
                foreach (TuneValue value in TuneValueList)
                    stdValue += value.ValueList[i].Item2;
                
                if (maxValue < stdValue)
                {
                    maxValue = stdValue;
                    maxLightValue = TuneValueList.First().ValueList[i].Item1;
                }
            }
            
            return maxLightValue;
        }
    }
}
