using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



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

using System.Diagnostics;
using UniScanM.ColorSens.Settings;

namespace UniScanM.ColorSens.Algorithm
{
    class autoLightTune
    {
        ManualResetEvent isGrabDone = new ManualResetEvent(false);

        int tryCount = 10;
        public int TryCount { get => tryCount; }

        int curTryCount;
        public int CurTryCount { get => curTryCount; }

        ImageD lastGrabedImage = null;

        
        

        double saturaionRatio = 0.03f;

        public autoLightTune(double saturaionRatio) 
        {
            this.saturaionRatio = saturaionRatio;
            isGrabDone.Reset();
        }

        protected bool GetValue(ImageD image, ref double wholeAverage, ref double topAverage)
        {
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            //1. Histogram
            long[] histo = Histogram(algoImage);

            long count = 0;
            double intensity = 0;
            long targetsize = (long)(algoImage.Width * algoImage.Height * saturaionRatio);
            for (int i = 255; i >= 0; i--) //와 254..
            {
                count += histo[i];

                intensity += histo[i] * i;
                if(count > targetsize)
                    break;
            }
            double averageIntensityTopArea = intensity / (double)count;
            topAverage = (averageIntensityTopArea);

            //System.Diagnostics.Debug.WriteLine(whiteNum.ToString());
            count = 0;
            intensity = 0;
            for (int i = 255; i >= 0; i--)
            {
                count += histo[i];
                intensity += histo[i] * i;
            }
            wholeAverage = intensity / (double)count;
            algoImage.Dispose();
            return true;
        }

        

        public int tune2()
        {
            double wholeAverage = 0;
            double topAverage=0;
            int LightValue = 0;
            int beforLightValue = 0;
            curTryCount = 0;

            //camera 
            Setup_Camera();

            while (LightValue < 255)
            {
                //1. 현재 기본 조명 값으로 영상얻고 데이터 뽑기
                LightTurnOn(LightValue);
                ImageD grabImage = Grab();
                if (grabImage == null) break;
                GetValue(grabImage, ref wholeAverage, ref topAverage);
                //
               
                System.Diagnostics.Debug.WriteLine("#######" +
                    LightValue.ToString() + "/" +
                    wholeAverage.ToString("F3") + "/" +
                    topAverage.ToString("F3") + "/" 
                    );
                LightValue += 10;

            }

            curTryCount = tryCount;

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.RemoveImageGrabbed(ImageGrabbed);
            return LightValue;
        }
        public int tune()
        {
            double wholeAverage = 0;
            double topAverage = 0;
            int LightValue = 50;
            int beforLightValue = 0;
            curTryCount = 0;

            //camera 
            Setup_Camera();
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(ImageGrabbed);

            System.Diagnostics.Debug.WriteLine("----------------Start tune---------------\n");
            while (curTryCount++ < tryCount)
            {
                //1. 현재 기본 조명 값으로 영상얻고 데이터 뽑기
                LightTurnOn(LightValue);
                ImageD grabImage = Grab();
                if (grabImage == null) break;
                GetValue(grabImage, ref wholeAverage, ref topAverage);
                //

                //2. TopAVG 가 255 이면. 255값 개수와 설정 비율(3%) 비율비로 조명값 낮춤
                if (topAverage == 255) //포화상태
                {
                    if (wholeAverage == 255)
                        LightValue = (int)((double)LightValue * 128/wholeAverage);
                    else
                        LightValue = (int)((double)LightValue * 0.95);
                }
                //3. TopAVG 가 255 이하이면 비율 계산하고 >> 조명값 설정 >> 끝
                else if (topAverage >= 240 && topAverage < 255)
                    break;
                else if (topAverage >= 20 && topAverage < 240)
                    LightValue = (int)((double)LightValue * 255.0 / topAverage);

                else if (topAverage < 20)
                    LightValue = (int)((double)LightValue * 4);

                //arrage 8bit
                LightValue = LightValue <= 255 ? LightValue : 255;

                if (beforLightValue == 255 && LightValue == 255)
                {
                    break;
                }
                System.Diagnostics.Debug.WriteLine("##"+beforLightValue.ToString() + "/" +
                    LightValue.ToString() + "/" +
                    wholeAverage.ToString("F3") + "/" +
                    topAverage.ToString("F3") + "/"
                    );

                beforLightValue = LightValue;
                // - 다시 1번으로
            }
            System.Diagnostics.Debug.WriteLine("@@" + beforLightValue.ToString() + "/" +
                  LightValue.ToString() + "/" +
                  wholeAverage.ToString("F3") + "/" +
                  topAverage.ToString("F3") + "/"
                  );

            curTryCount = tryCount;

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.RemoveImageGrabbed(ImageGrabbed);
            return LightValue;
        }

        ImageD Grab()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(0);
            isGrabDone.Reset();
            camera.SetTriggerMode(TriggerMode.Software);
            camera.GrabOnce();
            //Wait Grab Done Syncronous...
            //camera.Stop(); //stop이 waitgrabdone 보다 먼저 실행되어야함.
            camera.WaitGrabDone();
            camera.Stop();
            if (false == isGrabDone.WaitOne(100))//실패! 타임아웃
                return null;

            //get image....
            //ImageD imageD = camera.GetGrabbedImage(IntPtr.Zero);
            //ImageD imageD = camera.GetLatestImage();
            //Image2D imgBuf = (Image2D)imageD.Clone();
            //imgBuf.Tag = imageD.Tag;
            return lastGrabedImage;
        }
        
        void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "autoLightTune::ImageGrabbed");
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            lastGrabedImage = (Image2D)imageD.Clone();
            isGrabDone.Set();

            //AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imgBuf, ImageType.Grey);
        }
        private void LightTurnOn(int LightValue)
        {
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            if (SystemManager.Instance().DeviceBox.LightCtrlHandler.Count > 0)
            {
                UniScanM.ColorSens.Data.Model model = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;

                //model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)model.InspectParam.LightValue;
                model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)LightValue;
                lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);
                Thread.Sleep(100);
            }
        }

        public static long[] Histogram(AlgoImage srcImage)
        {
            Debug.Assert(srcImage.ImageType == ImageType.Grey);
            byte[] imageData = srcImage.GetByte();
            long[] histogram = new long[256];

            
            int width = srcImage.Width;
            int height = srcImage.Height;
            int pitch = width;// srcImage.Pitch;

            int sx = width / 8 * 3;
            int ey = width / 8 * 5;

            for (int j = 0; j < height; j++)
            {
                for (int i = sx; i < ey; i++)
                {
                    histogram[imageData[(j * pitch) + i]]++;
                }
            }
            return histogram;
        }

        void Setup_Camera()
        {
            double lineSpeed = 50;
            //05. 라인속도에 따른 카메라설정
            lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(0);
            if (camera != null)
            {
                camera.SetExposureTime((float)100.0); //us 130um@100m/min
                camera.SetTriggerMode(TriggerMode.Software);
                double Hz = lineSpeed * 1000000 / 60 / ColorSensorSettings.Instance().OpticResolution_umPerPixel;

                camera.SetAcquisitionLineRate((float)Hz);//7092.1985(100m/min)
            }
        }
    }
}

