using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using UniScanM.EDMS.Settings;
using UniEye.Base.Settings;
using UniScanM.EDMS.MachineIF;

namespace UniScanM.EDMS.Data
{
    public enum DataType
    {
        FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0, FilmEdge0WithPrintingEdge0, ENUM_MAX
       //T100       T101               T102           T103      T104                T105                   T134 (레포트용)
    }

    public enum State_EDMS
    {
        Init, Waiting, Zeroing, Inspecting
    }

    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        public State_EDMS State=State_EDMS.Init;
        float frameAvgIntensity = 0;
        public float FrameAvgIntensity
        {
            get { return frameAvgIntensity; }
            set { frameAvgIntensity = value; }
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

        double[] edgePostionResult;
        public double[] EdgePositionResult
        {
            get { return edgePostionResult; }
            set { edgePostionResult = value; }
        }

        float[] profile = null;
        public float[] Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        double[] totalEdgePostionResult;
        public double[] TotalEdgePositionResult
        {
            get { return totalEdgePostionResult; }
            set { totalEdgePostionResult = value; }
        }

        double firstFilmEdge = 0.0;
        public double FirstFilmEdge
        {
            get { return firstFilmEdge; }
            set { firstFilmEdge = value; }
        }

        double firstPrintingEdge = 0.0;
        public double FirstPrintingEdge
        {
            get { return firstPrintingEdge; }
            set { firstPrintingEdge = value; }
        }

        double? recentFilmEdgeDiff = 0.0;
        public double? RecentFilmEdgeDiff
        {
            get { return recentFilmEdgeDiff; }
            set { recentFilmEdgeDiff = value; }
        }

        double? recentPrintingEdgeDiff = 0.0;
        public double? RecentPrintingEdgeDiff
        {
            get { return recentPrintingEdgeDiff; }
            set { recentPrintingEdgeDiff = value; }
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

        public void AddEdgePositionResult(double[] edgeData, double pixelWidth)
        {
            edgePostionResult = edgeData;

            //double umPerPixel = 1.0;
            double umPerPixel = pixelWidth;

            double[] edgeResult = new double[6];
            edgeResult[(int)DataType.FilmEdge] = edgeData[0] * umPerPixel / 1000;
            edgeResult[(int)DataType.Coating_Film] = edgeData[1] <= 0 ? 0 : (edgeData[1] - edgeData[0]) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_Coating] = edgeData[2] <= 0 ? 0 : (edgeData[2] - edgeData[1]) * umPerPixel / 1000;
            edgeResult[(int)DataType.FilmEdge_0] = firstFilmEdge < 0 ? 0 : (edgeData[0] - firstFilmEdge) * umPerPixel / 1000;
            edgeResult[(int)DataType.PrintingEdge_0] = firstPrintingEdge <= 0 ? 0 : (edgeData[2] - firstPrintingEdge) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_FilmEdge_0] = firstPrintingEdge <= 0 ? 0 : (int)((edgeData[2] - firstPrintingEdge) - (edgeData[0] - firstFilmEdge)) * umPerPixel;

            totalEdgePostionResult = edgeResult;
        }
        
        public override void UpdateJudgement()
        {
            if (this.State == State_EDMS.Inspecting && this.Judgment != DynMvp.InspData.Judgment.Skip)
            {
                EDMSSettings setting = EDMSSettings.Instance();

                double[] resultArray = this.TotalEdgePositionResult;
                int judge_Reject = 0;
                int judge_Warn = 0;

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

                if ( setting.T103AlarmRecentOutEnable && setting.T103RecentDataCount > 0 && this.recentFilmEdgeDiff.HasValue)
                {
                    if (Math.Abs(recentFilmEdgeDiff.GetValueOrDefault()) > Math.Abs(setting.T103RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT103++;
                    }
                }

                if ( setting.T105AlarmRecentOutEnable && setting.T105RecentDataCount > 0 && this.recentPrintingEdgeDiff.HasValue)
                {
                    if (Math.Abs(recentPrintingEdgeDiff.GetValueOrDefault()) > Math.Abs(setting.T105RecentErrorRange))
                    {
                        judge_Reject++;
                        errorRecentFlagT105++;
                    }
                }

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

