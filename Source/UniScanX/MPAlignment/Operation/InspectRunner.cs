using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using System.Drawing;
using UniScanX.MPAlignment.Settings;
using UniScanX.MPAlignment.Devices;
using DynMvp.Base;
using DynMvp.Vision;
using UniEye.Base.Settings;

namespace UniScanX.MPAlignment.Operation
{
    class InspectRunner : UniEye.Base.Inspect.InspectRunner
    {
        UniScanX.MPAlignment.Devices.DeviceController DevCtroller
        {
            get => (UniScanX.MPAlignment.Devices.DeviceController)SystemManager.Instance().DeviceController;
        }

        public override bool EnterWaitInspection()
        {
            return  base.EnterWaitInspection();

            //Inspect(null,IntPtr.Zero, )
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();
        }

        
        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            //1. Macro Array Grab & Make Full Image
            

            //2. Model. 검사
            {
               //3. 피듀셜 검사
               //3.1 피듀셜 모션이동 영상그랩 
               //3.2 피듀셜 검사  -> 못찾으로 그냥 에러처리
               //3.3 얼라이너 만들기

               //4. //모델 프로브 
               //4.1 MpProbe 모션이동 (얼라이너)
               //4.2 MpProbe.Inspect(얼라이너)
               //4.3 Result(검출 모든 결과 및 패어, 실패도 있을수 있음
            }
            //5. 결과 반환
            //6. 전체 이미지 뷰.
        }

        public async Task<Bitmap> Scan_WholePatternArea(SizeF wholeSize)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                var tableOffset = MPSettings.Instance().TableOffset;
                var tableSize = MPSettings.Instance().TableSize;

                //테이블 사이즈를 초과할수 없다.
                if (wholeSize.Width > tableSize.Width || wholeSize.Height > tableSize.Height)
                {
                    LogHelper.Debug(LoggerType.Inspection, "InspectRunner - Scan_Whole Pattern Area = Scan Area over the Table Size");
                    return null;
                }

                //int WholeImageWidth = (int)(wholeSize.Width*1000 / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width);
                //int WholeImageHeight = (int)(wholeSize.Height*1000 / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height);
                //Bitmap wholeImage = new Bitmap(WholeImageWidth, WholeImageHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                //Image2D imageD = new Image2D(WholeImageWidth, WholeImageHeight, 1);
                //AlgoImage algImage=ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);

                SizeF FovSize = DevCtroller.MacroCamera.FovSize;
                FovSize.Width /= 1000f;  //um -> mm
                FovSize.Height /= 1000f; //um -> mm
                var camsize = DevCtroller.MacroCamera.ImageSize;

                int XstepCount = (int)Math.Ceiling(wholeSize.Width / FovSize.Width);
                int YstepCount = (int)Math.Ceiling(wholeSize.Height / FovSize.Height);

                int WholeImageWidth = XstepCount * camsize.Width; //pix
                int WholeImageHeight = YstepCount * camsize.Height ; //pix
                Bitmap wholeImage = new Bitmap(WholeImageWidth, WholeImageHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                PointF startPosition = new PointF();
                startPosition.X = tableOffset.X; //mm
                //startPosition.Y = tableOffset.Y + (tableSize.Height - wholeSize.Height); //mm
                startPosition.Y = tableOffset.Y + (tableSize.Height - YstepCount * FovSize.Height); //mm

                int Xstep, Ystep;
                for (Ystep = 0; Ystep < YstepCount; Ystep++)
                {
                    for (Xstep = 0; Xstep < XstepCount; Xstep++)
                    {
                        float moveXmm = Xstep * FovSize.Width + startPosition.X;
                        float moveYmm = Ystep * FovSize.Height + startPosition.Y;

                        DevCtroller.MoveXY(moveXmm, moveYmm);
                        //grab
                        var image = DevCtroller.Grab_Macro(55);
                        Bitmap bitmap = image.ToBitmap();
                        
                        Point point = new Point(camsize.Width*Xstep, camsize.Height*Ystep);

                       // if (Ystep == YstepCount - 1 || Xstep == XstepCount - 1)
                       //     continue;
                        ImageHelper.Copy(bitmap, wholeImage, point);
                    }
                }
                //DevCtroller.MoveXY(tableOffset.X, tableOffset.Y);
                DevCtroller.MoveXY(0, 0);
                return wholeImage;
            }); //return await System.Threading.Tasks.Task.Run(() =>
        }
    }
}
