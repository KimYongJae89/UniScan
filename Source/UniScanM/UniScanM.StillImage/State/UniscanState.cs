using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
//using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanM.StillImage.Algorithm;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Settings;

namespace UniScanM.StillImage.State
{
    public delegate void OnAsyncProcessDoneDelegate(DynMvp.InspData.InspectionResult inspectionResult);
    public abstract class UniscanState : IProcesser
    {
        protected OnAsyncProcessDoneDelegate OnAsyncProcessDone;

        protected bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }

        public abstract bool IsTeachState { get; }

        protected int imageSequnece = -1;
        public int ImageSequnece
        {
            get { return imageSequnece; }
        }
        
        public InspectState InspectState => inspectState;
        protected InspectState inspectState;
        
        /// <summary>
        /// 동기 상태: Process함수 종료 후 ProductInspected 호출.
        /// 비동기 상태: Process함수가 스레드로 동작. 함수 종료 후 ProductInspected 델리게이트 호출.
        /// </summary>
        public bool IsSyncState
        {
            get { return OnAsyncProcessDone == null; }
        }

        public UniscanState()
        {

        }

        public void Initialize()
        {
            //this.OnAsyncProcessDone = SystemManager.Instance().InspectRunner.ProductInspected;
            Init();
            initialized = true;
        }

        public ProcessTask Process(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            PreProcess();
            OnProcess(imageD, inspectionResult, inspectionOption);
            PostProcess(inspectionResult);
            return null;
        }

        /// <summary>
        /// 상태 변경시
        /// </summary>
        protected abstract void Init();

        /// <summary>
        ///  알고리즘 시작 전
        /// </summary>
        public abstract void PreProcess();

        /// <summary>
        /// 알고리즘
        /// </summary>
        /// <param name="algoImage"></param>
        /// <param name="inspectionResult"></param>
        public abstract void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null);

        /// <summary>
        /// 알고리즘 종료 후
        /// </summary>
        /// <param name="inspectionResult"></param>
        public abstract void PostProcess(DynMvp.InspData.InspectionResult inspectionResult);

        /// <summary>
        /// 다음 스탭 가져오기
        /// </summary>
        /// <param name="inspectionResult">현재 검사 결과</param>
        /// <returns></returns>
        public abstract UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult);

        public IProcesser GetNextProcesser()
        {
            throw new NotImplementedException();
        }

        public bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1)
        {
            throw new NotImplementedException();
        }

        public void CancelProcess(ProcessTask inspectionTask = null)
        {
            throw new NotImplementedException();
        }

        public void StartGrab()
        {

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class FindingSheet
    {
        SheetFinder sheetFinder = null;
        Random random = null;

        public FindingSheet()
        {
            this.sheetFinder = SheetFinder.Create(0);
            this.random = new Random();
        }

        public AlgoImage GetSheetImage(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            // Get Sheet
            Rectangle sheetRect = GetSheetRect(algoImage, inspectionResult);
            //algoImage.Save(@"d:\temp\tt.bmp");
            LogHelper.Debug(LoggerType.Inspection, $"FindingSheet::GetSheetImage -  sheetRect: {sheetRect}");
            if (sheetRect.Width == 0 || sheetRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame 1");
                inspectionResult?.SetDefect();
                //adjustImage.Save(Path.Combine(imageSavePath, "Grab", iamgeSaveName));
                return null;
            }

            if (inspectionResult2 != null)
            {
                inspectionResult2.SheetRectInFrame = sheetRect;
                //inspectionResult2.FovRectInSheet = GetFovRect(sheetRect.Size);
            }

            AlgoImage sheetImage = algoImage.GetSubImage(sheetRect);//.Clone();
            //sheetImage.Save(Path.Combine(imageSavePath, "Sheet", iamgeSaveName));
            
            return sheetImage;
        }

        public AlgoImage GetInspImage(AlgoImage frameImage, Size inspSize, DynMvp.InspData.InspectionResult inspectionResult)
        {
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            Rectangle sheetRect = GetSheetRect(frameImage, inspectionResult);
            LogHelper.Debug(LoggerType.Inspection, $"FindingSheet::GetInspImage - sheetRect: {sheetRect}");
            if (sheetRect.Width == 0 || sheetRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame 2");
                inspectionResult.SetDefect();
                //adjustImage.Save(Path.Combine(imageSavePath, "Grab", iamgeSaveName));
                return null;
            }
            inspectionResult2.SheetRectInFrame = sheetRect;

            // Get FOV //높이는 설정된 값  // float yPos = ((Data.Model)SystemManager.Instance().CurrentModel).FovYPos * sheetSize.Height;
            Rectangle inspRect = SheetFinder.GetInspRect(sheetRect.Size, inspSize);// sheet 사이즈의 가로 가운데 세로, 설정된  inspSize 사각형 위치를 리턴
            //inspRect.Offset(sheetRect.Location);
            LogHelper.Debug(LoggerType.Inspection, $"FindingSheet::GetInspImage - inspRect: {inspRect}");
            if (inspRect.Width == 0 || inspRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame 3");
                inspectionResult.SetDefect();
                return null;
            }
            inspectionResult2.InspRectInSheet = inspRect;

            Settings.StillImageSettings additionalSettings = Settings.StillImageSettings.Instance() as Settings.StillImageSettings;
            if(additionalSettings.OperationMode == EOperationMode.Random)
            {
                int availableY1 = sheetRect.Top;
                int availableY2 = sheetRect.Bottom - inspRect.Height;
                int newY = random.Next(availableY2 - availableY1) + availableY1;
                inspRect.Y = newY;
            }
            else
            {
                inspRect.Offset(sheetRect.Location); //프레임이미지 좌표로 변환
            }

            AlgoImage inspImage = frameImage.GetSubImage(inspRect);
            return inspImage;
        }

        private Rectangle GetSheetRect(AlgoImage frameImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
            // Extract Sheet in Image
            //frameImage.Save("d:\\tt\\temp.bmp");
            Rectangle sheetRect = this.sheetFinder.FindSheet(frameImage,  inspectionResult);
            return sheetRect;
        }
    }
}
