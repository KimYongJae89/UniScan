using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.ColorSens.Data;
using System.Collections.Concurrent;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;

using UniScanM.ColorSens.Settings;

namespace UniScanM.ColorSens.Algorithm
{
 
    class InspectSheetDeltaBrightness
    {
        List<float> line_B = null; //계측된 라인별 밝기를 저장함 (라인이 sheetSizeLineCount 만큼 모이면 한 Sheet가 됨)
        List<float> sheet_B = null; //각 시트의 밝기를 저장함

        private double sheetLength_mm = 330;
        private int sheetSizeLineCount = 1404;//330 / 0.235;  // = sheetsize / resolution
        private double resolutionLine_um = 235;

        private int max_Reference_Count = 30;
        private float inspection_Range = 1;

        public InspectSheetDeltaBrightness( double SheetLength_mm, double ResolutionLine_um)
        {
            sheetLength_mm = SheetLength_mm;
            resolutionLine_um = ResolutionLine_um;
            sheetSizeLineCount = (int)((sheetLength_mm * 1000) / resolutionLine_um);

            line_B = new List<float>();
            sheet_B = new List<float>();


       //1. 컬러센서 검사조건 결과도 같이 넘김
            UniScanM.ColorSens.Data.Model model = (UniScanM.ColorSens.Data.Model)(SystemManager.Instance().CurrentModel);
            ColorSensorParam param = (ColorSensorParam)model.InspectParam;
            inspection_Range = (float)param.SpecLimit;
            max_Reference_Count = ColorSensorSettings.Instance().IntegrateReferenceFrame;

        }
       
        public bool AddImage(AlgoImage algoImage)
        {
            bool bSampled = false;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);    
            
            //1.
            float[] projection = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);//가로 프로젝션, 데이터는 세로길이
            line_B.AddRange(projection);

            //2. remain only Sheet Length data
            int totalcount = line_B.Count;
            while (totalcount > sheetSizeLineCount) //여기 확인필요 minsong
            {
                line_B = line_B.Skip(totalcount - sheetSizeLineCount).ToList();
                sheet_B.Add(line_B.GetRange(0, sheetSizeLineCount).Average());
    
                if (sheet_B.Count > max_Reference_Count)
                {
                    bSampled = true;
                }

                totalcount = line_B.Count;
            }

            return bSampled;
        }

        public float[] Inspect(ColorSens.Data.InspectionResult colorInspectionResult)
        {
            float[] arr_delta = new float[1];
            float delta = 0;

            //1.기준이 될만한 시트가 충분히 안모였으면 리턴.
            if (sheet_B.Count < max_Reference_Count)
            {
                colorInspectionResult.Judgment = DynMvp.InspData.Judgment.Skip;
                return arr_delta;                 
            }
                                                
            float lastvalue = sheet_B.Last();

            //2. 새로운 기준값을 계산, 기준값은 지금껏 시트의 밝기의 중위 데이터
            List<float> sortIntensity = sheet_B.GetRange(0, sheet_B.Count -1) ;
            sortIntensity.Sort();
            int start = sortIntensity.Count / 3;
            int mediandataCount = sortIntensity.Count / 3;
            mediandataCount = mediandataCount > 1  ? mediandataCount: 1;
            List<float> mediandata = sortIntensity.GetRange(start, mediandataCount);

            //2.1 기준값 sheet의 갯수의 3배 이상은 저장안함.
            if(sheet_B.Count > max_Reference_Count * 3) //
                sheet_B = sheet_B.Skip(sheet_B.Count - max_Reference_Count * 3).ToList();
            //
            float Reference = mediandata.Average();  //******************************************검사 Reference 값***//

            //3.최종 결과값.
            delta = (float)(lastvalue - Reference) * 100f / (float)255;

            //4.최종 결과값이 에러이면 sheet_B 리스트에서 제거함. -> 다음번 기준값에서 제외
            if (-inspection_Range > delta || +inspection_Range < delta)
                sheet_B.RemoveAt(sheet_B.Count - 1);

            colorInspectionResult.ReferenceBrightness = Reference;
            colorInspectionResult.SheetBrightness = (float)lastvalue;
            colorInspectionResult.DeltaBrightness = Math.Abs(delta);

            colorInspectionResult.Uppperlimit = Reference + inspection_Range;
            colorInspectionResult.Lowerlimit = Reference - inspection_Range;
                                       
            colorInspectionResult.Judge();

            arr_delta[0] = delta;
            return arr_delta;
        }
        
    }
    /// <summary>
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    class InspectSheetBrightness
    {
        List<float>[] listArray_accumulorBright = null;
        double tragetBrightness;

        private int seperateStep = 1; //영상을 Colum으로 쪼갠 갯수 1이면 전체, 2면 Framㄷ 세로 2분활 => 한영역은 Frame.Width / seperateStpe;
        private double sheetLength_mm = 330;
        private int sheetSizeLineCount = 1404;//330 / 0.235;  // = sheetsize / resolution
        private double resolutionLine_um = 235;

        public InspectSheetBrightness(int SeperateStep, double SheetLength_mm, double ResolutionLine_um)
        {
            seperateStep = SeperateStep;
            sheetLength_mm = SheetLength_mm;
            resolutionLine_um = ResolutionLine_um;
            sheetSizeLineCount = (int)((sheetLength_mm * 1000) / resolutionLine_um);

            listArray_accumulorBright = new List<float>[seperateStep];
            for (int i = 0; i < listArray_accumulorBright.Length; i++)
            {
                listArray_accumulorBright[i] = new List<float>();
            }
        }
        public InspectSheetBrightness(ColorSensorSettings config)
        {
            //seperateStep = config.SensorCount;
            //sheetLength_mm = config.SheetLength_MM;
            resolutionLine_um = config.OpticResolution_umPerPixel;
            sheetSizeLineCount = (int)((sheetLength_mm * 1000) / resolutionLine_um);

            listArray_accumulorBright = new List<float>[seperateStep];
            for (int i = 0; i < listArray_accumulorBright.Length; i++)
            {
                listArray_accumulorBright[i] = new List<float>();
            }
        }

        public InspectSheetBrightness(double TragetBrightness, double SheetLength_MM, double OpticResolution_umPerPixel)
        {
            tragetBrightness = TragetBrightness;
            seperateStep = 1;
            sheetLength_mm = SheetLength_MM;
            resolutionLine_um = OpticResolution_umPerPixel;
            sheetSizeLineCount = (int)((sheetLength_mm * 1000) / resolutionLine_um);

            listArray_accumulorBright = new List<float>[seperateStep];
            for (int i = 0; i < listArray_accumulorBright.Length; i++)
            {
                listArray_accumulorBright[i] = new List<float>();
            }
        }

        public bool AddImage(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage RoiImage = null;
            bool bOnPeriod = false;
            for (int i = 0; i < seperateStep; i++)
            {
                //0. 
                var curlist = listArray_accumulorBright[i];
                //1. 현재 ROI 지정
                int left = (algoImage.Width / seperateStep * i);
                int top = 0;
                int ntemp = (algoImage.Width / seperateStep * (i + 1));
                int right = (ntemp < algoImage.Width) ? ntemp : algoImage.Width;
                int bottom = algoImage.Height;

                //2. ROI 영역 이미지 따기
                Rectangle RoiRect = Rectangle.FromLTRB(left, top, right, bottom);
                //RoiRect.Inflate();
                Debug.Assert(RoiRect.Width > 0 && RoiRect.Height > 0);
                RoiImage = algoImage.GetSubImage(RoiRect);

                //3. Projection  Data push
                float[] projection = imageProcessing.Projection(RoiImage, Direction.Vertical, ProjectionType.Mean);//가로 프로젝션, 데이터는 세로길이
                curlist.AddRange(projection);

                //4. remain only Sheet Length data
                int totalcount = curlist.Count;
                if (totalcount > sheetSizeLineCount) //여기 확인필요 minsong
                {
                    //curlist = curlist.Skip(totalcount - sheetSizeLineCount).Take(sheetSizeLineCount).ToList();
                    curlist = curlist.Skip(totalcount - sheetSizeLineCount).ToList();
                    listArray_accumulorBright[i] = curlist;
                    bOnPeriod = true;
                }
            }
            return bOnPeriod;
        }

        public float[] Inspect()
        {
            float[] arr_avg = new float[listArray_accumulorBright.Length];
            float targetvalue = (float)tragetBrightness;
            targetvalue = tragetBrightness < 1 ? 1 : targetvalue;
            for (int i = 0; i < listArray_accumulorBright.Length; i++) // seperateStep 랑 같음
            {
                var curlist = listArray_accumulorBright[i];

                arr_avg[i] = curlist.Average() / (float)255 * 100f;

            }
            return arr_avg;
        }
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    class FinderSheetPeriod
    {
        List<float> listArray_Intensity = new List<float>();

        
        private double resolutionLine_um = 235;
        //330 / 0.235;  // = sheetsize / resolution 2005

        public FinderSheetPeriod()
        {

        }

        public void AddImage(AlgoImage algoImage)  //가능한 많이 넣는게 최소한 10장의 sheet 길이 정도는 넣는게 좋음
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            //0. Projection  Data push
            float[] projection = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);//가로 프로젝션, 데이터는 세로길이
            listArray_Intensity.AddRange(projection);
        }

        public void Reset()
        {
            listArray_Intensity.Clear();
        }
        public int CalculatePixelResolution( double sheetPeriod_mm,ref double umPerPixel)
        {

            LightTurnOn();
            Setup_Camera();
            if (false == Grab(10))
                return 11;



            if (listArray_Intensity.Count < 10000)
                return -1;

            List<int> listX = new List<int>();  //period
            List<float> listY = new List<float>(); //sheet intensity diff

            int cen = (int)(sheetPeriod_mm*1000 / resolutionLine_um + 0.5);
            int startPeriod = cen - 100;
            int endPeriod = cen + 100;
            //float []intensity=listArray_Intensity.ToArray();

            for (int period= startPeriod; period < endPeriod; period++)
            {
                List<float> onesheetI = new List<float>();
                int count = listArray_Intensity.Count / period;
                for (int i=0; i< count-1; i++)
                {
                    float sheetI =listArray_Intensity.GetRange(i, period).Average();
                    onesheetI.Add(sheetI);
                }
                listX.Add(period);
                float diff = onesheetI.Max() - onesheetI.Min();
                listY.Add(diff); 
            }

            
            ////////////////////
            int index= listY.IndexOf(listY.Min());
            int onesheetperiod_line = startPeriod + index;
            umPerPixel = sheetPeriod_mm * 1000 / onesheetperiod_line;

            return onesheetperiod_line;
        }//Calculate()



        public double CalculateOnePatternBright(double sheetPeriod_mm)
        {
            resolutionLine_um = ColorSensorSettings.Instance().OpticResolution_umPerPixel;
            LightTurnOn();
            Setup_Camera();
            if (false == Grab(10))
                return 11;

            if (listArray_Intensity.Count < 10000)
                return 12;

            List<int> listX = new List<int>();  //period
            List<float> listY = new List<float>(); //sheet intensity diff

            int PeriodPix = (int)(sheetPeriod_mm * 1000 / resolutionLine_um);

            List<float> onesheetI = new List<float>();
            int count = listArray_Intensity.Count / PeriodPix;
            for (int i = 0; i < count ; i++)
            {
                float sheetI = listArray_Intensity.GetRange(i* PeriodPix, PeriodPix).Average();
                onesheetI.Add(sheetI);
            }

            double AvgBrightness = onesheetI.Average();
            return AvgBrightness/255*100;
        }//Calculate()

        ManualResetEvent isGrabDone = new ManualResetEvent(false);
        ImageD lastGrabedImage = null;

        int grabbedCount = 0;
        public int GrabbedCount { get => grabbedCount; }
        
        int targetGrabCount = 0;
        public int TargetGrabCount { get => targetGrabCount; }
        bool Grab(int count)
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(0);
            isGrabDone.Reset();
            camera.ImageGrabbed += ImageGrabbed;
            camera.SetTriggerMode(TriggerMode.Software);
            grabbedCount = 0;
            targetGrabCount = count;
            camera.GrabMulti();
            //Wait Grab Done Syncronous...
            //camera.Stop(); //stop이 waitgrabdone 보다 먼저 실행되어야함.
            //camera.WaitGrabDone();
            //camera.Stop();
            if (false == isGrabDone.WaitOne(30000))//실패! 타임아웃
            {
                camera.ImageGrabbed = null;
                camera.Stop();
                return false;
            }
            camera.ImageGrabbed = null;
            camera.Stop();
            //get image....
            //ImageD imageD = camera.GetGrabbedImage(IntPtr.Zero);
            //ImageD imageD = camera.GetLatestImage();
            //Image2D imgBuf = (Image2D)imageD.Clone();
            //imgBuf.Tag = imageD.Tag;
            return true;
        }

        List<ImageD> listGrabbedImage = new List<ImageD>();

        void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "autoBrightcheck::ImageGrabbed");
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            lastGrabedImage = imageD.Clone();
            //listGrabbedImage.Add(lastGrabedImage);
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, lastGrabedImage, ImageType.Grey);
            AddImage(algoImage);

            grabbedCount++;
            if (GrabbedCount == TargetGrabCount)
            {
                isGrabDone.Set();
            }

            //AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imgBuf, ImageType.Grey);
        }
        private void LightTurnOn()
        {
            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;
            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;

            int LightValue = (int)colorSensorParam.LightValue;
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            if (SystemManager.Instance().DeviceBox.LightCtrlHandler.Count > 0)
            {
                UniScanM.ColorSens.Data.Model model = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
                model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)LightValue;
                lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);
                Thread.Sleep(1000);
            }
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
