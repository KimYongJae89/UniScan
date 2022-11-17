using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using UniScanM.EDMSW.Settings;
using UniEye.Base.Settings;
using UniScanM.EDMSW.MachineIF;

namespace UniScanM.EDMSW.Data
{
    public enum DataType
    {
        FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0, ENUM_MAX
       //T100       T101               T102           T103      T104                T105                   T134 (레포트용)
    }

    public enum DataType_Length
    {
        W100, W101, W102,
        L100, L200, LDIFF,
        ENUM_MAX
    }

    public enum State_EDMS
    {
        Init, Waiting, Zeroing, Inspecting
    }

    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        public State_EDMS State=State_EDMS.Init;

        Bitmap displayBitmapLeft;
        public Bitmap DisplayBitmapLeft { get => displayBitmapLeft; set => displayBitmapLeft = value; }

        Bitmap displayBitmapRight;
        public Bitmap DisplayBitmapRight { get => displayBitmapRight; set => displayBitmapRight = value; }

        float frameAvgIntensityLeft = 0;
        public float FrameAvgIntensityLeft
        {
            get { return frameAvgIntensityLeft; }
            set { frameAvgIntensityLeft = value; }
        }

        float frameAvgIntensityRight = 0;
        public float FrameAvgIntensityRight
        {
            get { return frameAvgIntensityRight; }
            set { frameAvgIntensityRight = value; }
        }


        int remainWaitDist = 0;
        public int RemainWaitDist
        {
            get { return remainWaitDist; }
            set { remainWaitDist = value; }
        }

        bool resetZeroing;
        public bool ResetZeroing
        {
            get { return resetZeroing; }
            set { resetZeroing = value; }
        }

        int zeroingNum;
        public int ZeroingNum
        {
            get { return zeroingNum; }
            set { zeroingNum = value; }
        }

        double[] edgePostionResultLeft; //픽셀단위, 화면에 그리기용
        public double[] EdgePositionResultLeft
        {
            get { return edgePostionResultLeft; }
            set { edgePostionResultLeft = value; }
        }
        
        double[] edgePostionResultRight; //픽셀단위, 화면에 그리기용
        public double[] EdgePositionResultRight
        {
            get { return edgePostionResultRight; }
            set { edgePostionResultRight = value; }
        }


        double[] lengthDataPix; //픽셀단위, 화면에 그리기용 //DataType_Length
        public double[] LengthDataPix
        {
            get { return lengthDataPix; }
            set { lengthDataPix = value; }
        }

        //float[] profile = null;
        //public float[] Profile
        //{
        //    get { return profile; }
        //    set { profile = value; }
        //}

        float[] profileLeftHor = null;
        public float[] ProfileLeftHor
        {
            get { return profileLeftHor; }
            set { profileLeftHor = value; }
        }

        float[] profileRightHor = null;
        public float[] ProfileRightHor
        {
            get { return profileRightHor; }
            set { profileRightHor = value; }
        }

        float[] profileLeftVer = null;
        public float[] ProfileLeftVer
        {
            get { return profileLeftVer; }
            set { profileLeftVer = value; }
        }

        float[] profileRightVer = null;
        public float[] ProfileRightVer
        {
            get { return profileRightVer; }
            set { profileRightVer = value; }
        }

        //moving Average data
        //double[] totalEdgePostionResult;
        //public double[] TotalEdgePositionResult 
        //{
        //    get { return totalEdgePostionResult; }
        //    set { totalEdgePostionResult = value; }
        //}

        double[] totalEdgePostionResultLeft;  //metric:um
        public double[] TotalEdgePositionResultLeft
        {
            get { return totalEdgePostionResultLeft; }
            set { totalEdgePostionResultLeft = value; }
        }

        double[] totalEdgePostionResultRight;  //metric:um
        public double[] TotalEdgePositionResultRight
        {
            get { return totalEdgePostionResultRight; }
            set { totalEdgePostionResultRight = value; }
        }

        double[] totalLengthData; //Metric : mm
        public double[] TotalLengthData
        {
            get { return totalLengthData; }
            set { totalLengthData = value; }
        }
               
        double firstFilmEdgeLeft = 0.0;
        public double FirstFilmEdgeLeft
        {
            get { return firstFilmEdgeLeft; }
            set { firstFilmEdgeLeft = value; }
        }

        double firstPrintingEdgeLeft = 0.0;
        public double FirstPrintingEdgeLeft
        {
            get { return firstPrintingEdgeLeft; }
            set { firstPrintingEdgeLeft = value; }
        }

        double firstFilmEdgeRight = 0.0;
        public double FirstFilmEdgeRight
        {
            get { return firstFilmEdgeRight; }
            set { firstFilmEdgeRight = value; }
        }

        double firstPrintingEdgeRight = 0.0;
        public double FirstPrintingEdgeRight
        {
            get { return firstPrintingEdgeRight; }
            set { firstPrintingEdgeRight = value; }
        }

        double? recentFilmEdgeDiffLeft = 0.0;
        public double? RecentFilmEdgeDiffLeft
        {
            get { return recentFilmEdgeDiffLeft; }
            set { recentFilmEdgeDiffLeft = value; }
        }

        double? recentFilmEdgeDiffRight = 0.0;
        public double? RecentFilmEdgeDiffRight
        {
            get { return recentFilmEdgeDiffRight; }
            set { recentFilmEdgeDiffRight = value; }
        }

        double? recentPrintingEdgeDiffLeft = 0.0;
        public double? RecentPrintingEdgeDiffLeft
        {
            get { return recentPrintingEdgeDiffLeft; }
            set { recentPrintingEdgeDiffLeft = value; }
        }

        double? recentPrintingEdgeDiffRight = 0.0;
        public double? RecentPrintingEdgeDiffRight
        {
            get { return recentPrintingEdgeDiffRight; }
            set { recentPrintingEdgeDiffRight = value; }
        }

        private int errorOriginFlagT103 = 0;
        public int ErrorOriginFlagT103
        {
            get { return errorOriginFlagT103; }
            set { errorOriginFlagT103 = value; }
        }

        private int errorOriginFlagT105 = 0;
        public int ErrorOriginFlagT105
        {
            get { return errorOriginFlagT105; }
            set { errorOriginFlagT105 = value; }
        }
        private int errorRecentFlagT103 = 0;
        public int ErrorRecentFlagT103
        {
            get { return errorRecentFlagT103; }
            set { errorRecentFlagT103 = value; }
        }

        private int errorRecentFlagT105 = 0;
        public int ErrorRecentFlagT105
        {
            get { return errorRecentFlagT105; }
            set { errorRecentFlagT105 = value; }
        }


        int waitRemain;
        public int WaitRemain { get => this.waitRemain; set => this.waitRemain = value; }

        public void AddEdgePositionResultLeft(double[] edgeData, double pixelWidth)
        {
            edgePostionResultLeft = edgeData;

            //double umPerPixel = 1.0;
            double umPerPixel = pixelWidth;

            double[] edgeResult = new double[6];
            edgeResult[(int)DataType.FilmEdge] = edgeData[0] * umPerPixel / 1000;
            edgeResult[(int)DataType.Coating_Film] = edgeData[1] <= 0 ? 0 : (edgeData[1] - edgeData[0]) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_Coating] = edgeData[2] <= 0 ? 0 : (edgeData[2] - edgeData[1]) * umPerPixel / 1000;
            edgeResult[(int)DataType.FilmEdge_0] = firstFilmEdgeLeft < 0 ? 0 : (edgeData[0] - firstFilmEdgeLeft) * umPerPixel / 1000;
            edgeResult[(int)DataType.PrintingEdge_0] = firstPrintingEdgeLeft <= 0 ? 0 : (edgeData[2] - firstPrintingEdgeLeft) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_FilmEdge_0] = firstPrintingEdgeLeft <= 0 ? 0 : (int)((edgeData[2] - firstPrintingEdgeLeft) - (edgeData[0] - firstFilmEdgeLeft)) * umPerPixel;

            totalEdgePostionResultLeft = edgeResult;
        }


        public void AddEdgePositionResultRight(double[] edgeData, double pixelWidth)
        {
            edgePostionResultRight = edgeData;

            //double umPerPixel = 1.0;
            double umPerPixel = pixelWidth;

            double[] edgeResult = new double[6];
            edgeResult[(int)DataType.FilmEdge] = edgeData[0] * umPerPixel / 1000;
            edgeResult[(int)DataType.Coating_Film] = edgeData[1] <= 0 ? 0 : (edgeData[1] - edgeData[0]) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_Coating] = edgeData[2] <= 0 ? 0 : (edgeData[2] - edgeData[1]) * umPerPixel / 1000;
            edgeResult[(int)DataType.FilmEdge_0] = firstFilmEdgeRight < 0 ? 0 : (edgeData[0] - firstFilmEdgeRight) * umPerPixel / 1000;
            edgeResult[(int)DataType.PrintingEdge_0] = firstPrintingEdgeRight <= 0 ? 0 : (edgeData[2] - firstPrintingEdgeRight) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_FilmEdge_0] = firstPrintingEdgeRight <= 0 ? 0 : (int)((edgeData[2] - firstPrintingEdgeRight) - (edgeData[0] - firstFilmEdgeRight)) * umPerPixel;

            totalEdgePostionResultRight = edgeResult;
        }

        public override void UpdateJudgement()
        {
            if (this.State == State_EDMS.Inspecting && this.Judgment != DynMvp.InspData.Judgment.Skip)
            {
                EDMSSettings setting = EDMSSettings.Instance();

                int judge_Reject = 0;
                int judge_Warn = 0;
                double[] resultArray = this.TotalEdgePositionResultLeft;

                //Left -----------------------------------------------------------------------------------------------
                if (Math.Abs(resultArray[(int)DataType.FilmEdge_0]) > Math.Abs(setting.T103WarningRange)) judge_Warn++;
                if (Math.Abs(resultArray[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.T105WarningRange)) judge_Warn++;

                if (Math.Abs(resultArray[(int)DataType.FilmEdge_0]) > Math.Abs(setting.T103ErrorRange))
                {
                    judge_Reject++;
                    if (setting.T103AlarmOriginOutEnable == true)
                        errorOriginFlagT103++;
                }

                if (Math.Abs(resultArray[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.T105ErrorRange))
                {
                    judge_Reject++;
                    if (setting.T105AlarmOriginOutEnable == true)
                        errorOriginFlagT105++;
                }

                if ( setting.T103AlarmRecentOutEnable && setting.T103RecentDataCount > 0 && this.recentFilmEdgeDiffLeft.HasValue)
                {
                    if (Math.Abs(recentFilmEdgeDiffLeft.GetValueOrDefault()) > Math.Abs(setting.T103RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT103++;
                    }
                }

                if ( setting.T105AlarmRecentOutEnable && setting.T105RecentDataCount > 0 && this.recentPrintingEdgeDiffLeft.HasValue)
                {
                    if (Math.Abs(recentPrintingEdgeDiffLeft.GetValueOrDefault()) > Math.Abs(setting.T105RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT105++;
                    }
                }
                //right/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                resultArray = this.TotalEdgePositionResultRight;

                //Left -----------------------------------------------------------------------------------------------
                if (Math.Abs(resultArray[(int)DataType.FilmEdge_0]) > Math.Abs(setting.T103WarningRange)) judge_Warn++;
                if (Math.Abs(resultArray[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.T105WarningRange)) judge_Warn++;

                if (Math.Abs(resultArray[(int)DataType.FilmEdge_0]) > Math.Abs(setting.T103ErrorRange))
                {
                    judge_Reject++;
                    if (setting.T103AlarmOriginOutEnable == true)
                        errorOriginFlagT103++;
                }

                if (Math.Abs(resultArray[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.T105ErrorRange))
                {
                    judge_Reject++;
                    if (setting.T105AlarmOriginOutEnable == true)
                        errorOriginFlagT105++;
                }

                if (setting.T103AlarmRecentOutEnable && setting.T103RecentDataCount > 0 && this.recentFilmEdgeDiffRight.HasValue)
                {
                    if (Math.Abs(recentFilmEdgeDiffRight.GetValueOrDefault()) > Math.Abs(setting.T103RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT103++;
                    }
                }

                if (setting.T105AlarmRecentOutEnable && setting.T105RecentDataCount > 0 && this.recentPrintingEdgeDiffRight.HasValue)
                {
                    if (Math.Abs(recentPrintingEdgeDiffRight.GetValueOrDefault()) > Math.Abs(setting.T105RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT105++;
                    }
                }

                //sum ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (judge_Reject > 0)
                    this.Judgment = DynMvp.InspData.Judgment.Reject;
                else if (judge_Warn > 0)
                    this.Judgment = DynMvp.InspData.Judgment.Warn;
                else
                    this.Judgment = DynMvp.InspData.Judgment.Accept;
            }
        }
    }
}

