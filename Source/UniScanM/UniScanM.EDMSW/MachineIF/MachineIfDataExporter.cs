using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.MachineInterface;
using UniScanM.EDMSW.Settings;
using UniScanM.Operation;
using UniScanM.EDMSW.Data;
using System.Diagnostics;
using UniScanM.Data;

namespace UniScanM.EDMSW.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        private int RepetitionCount = 0;
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;
            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;
            if (OperationOption.Instance().OnTune)
                return;
       
            UniScanM.EDMSW.Data.InspectionResult myInspectionResult = (UniScanM.EDMSW.Data.InspectionResult)inspectionResult;
            Debug.WriteLine("----MachineIfDataExporter->RepetitionCount=" + RepetitionCount.ToString());

            if (myInspectionResult.State == State_EDMS.Inspecting) 
            {
                if(myInspectionResult.Judgment == Judgment.Skip)
                {
                    RepetitionCount = 0;
                }
                else
                {
                    //1st camera -------------------------------------------------------------------------------//
                    double[] edgeArrayLeft = myInspectionResult.TotalEdgePositionResultLeft;
                    if (edgeArrayLeft == null) return;

                    short L_filmEdge = (short)((edgeArrayLeft[(int)Data.DataType.FilmEdge]) * 1000);
                    short L_coating_Film = (short)((edgeArrayLeft[(int)Data.DataType.Coating_Film]) * 1000);
                    short L_printing_Coating = (short)((edgeArrayLeft[(int)Data.DataType.Printing_Coating]) * 1000);
                    short L_filmEdge_0 = (short)((edgeArrayLeft[(int)Data.DataType.FilmEdge_0]) * 1000);
                    short L_printingEdge_0 = (short)((edgeArrayLeft[(int)Data.DataType.PrintingEdge_0]) * 1000);
                    short L_printing_FilmEdge_0 = (short)((edgeArrayLeft[(int)Data.DataType.Printing_FilmEdge_0]));

                    //2nd camera ---------------------------------------------------------------------------------//
                    double[] edgeArrayRight = myInspectionResult.TotalEdgePositionResultRight;
                    if (edgeArrayRight == null) return;

                    short R_filmEdge = (short)((edgeArrayRight[(int)Data.DataType.FilmEdge]) * 1000);
                    short R_coating_Film = (short)((edgeArrayRight[(int)Data.DataType.Coating_Film]) * 1000);
                    short R_printing_Coating = (short)((edgeArrayRight[(int)Data.DataType.Printing_Coating]) * 1000);
                    short R_filmEdge_0 = (short)((edgeArrayRight[(int)Data.DataType.FilmEdge_0]) * 1000);
                    short R_printingEdge_0 = (short)((edgeArrayRight[(int)Data.DataType.PrintingEdge_0]) * 1000);
                    short R_printing_FilmEdge_0 = (short)((edgeArrayRight[(int)Data.DataType.Printing_FilmEdge_0]));

                    //lenth data
                    double[] lengthdata = myInspectionResult.TotalLengthData;
                    if (lengthdata == null) return;
                    
                    int W100=(int)((lengthdata[(int)Data.DataType_Length.W100]) * 1000);
                    int W101 = (int)((lengthdata[(int)Data.DataType_Length.W101]) * 1000);
                    int W102 = (int)((lengthdata[(int)Data.DataType_Length.W102]) * 1000);
                    int L100 = (int)((lengthdata[(int)Data.DataType_Length.L100]) * 1000);
                    int L200 = (int)((lengthdata[(int)Data.DataType_Length.L200]) * 1000);
                    int LDIFF = (int)((lengthdata[(int)Data.DataType_Length.LDIFF]) * 1000);


                    //PLC로 출력할 결과 만들기.
                    //각 옵션마다 각각 비교해서 재판단  (result.judge를 화면에 표시하는 문제로 여기선 다시 판단 필요)
                    string resultString = "0000";
                    int totalErrorFlag=0;
                    if (EDMSSettings.Instance().T103AlarmOriginOutEnable && myInspectionResult.ErrorOriginFlagT103 > 0)
                        totalErrorFlag++;
                    if (EDMSSettings.Instance().T103AlarmRecentOutEnable && myInspectionResult.ErrorRecentFlagT103 > 0)
                        totalErrorFlag++;
                    if (EDMSSettings.Instance().T105AlarmOriginOutEnable && myInspectionResult.ErrorOriginFlagT105 > 0)
                        totalErrorFlag++;
                    if (EDMSSettings.Instance().T105AlarmRecentOutEnable && myInspectionResult.ErrorRecentFlagT105 > 0)
                        totalErrorFlag++;

                    if (totalErrorFlag > 0)
                        RepetitionCount++;
                    else RepetitionCount = 0;

                    resultString = (RepetitionCount >= Settings.EDMSSettings.Instance().RepetitionCount) ? "0001" : "0000";

                    string mergeString =
                        string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}",
                        resultString
                        , string.Format("{0:X04}", L_filmEdge)  //Left
                        , string.Format("{0:X04}", L_coating_Film)
                        , string.Format("{0:X04}", L_printing_Coating)
                        , string.Format("{0:X04}", L_filmEdge_0)
                        , string.Format("{0:X04}", L_printingEdge_0)
                        , string.Format("{0:X04}", L_printing_FilmEdge_0)

                        , string.Format("{0:X04}", R_filmEdge) //Right
                        , string.Format("{0:X04}", R_coating_Film)
                        , string.Format("{0:X04}", R_printing_Coating)
                        , string.Format("{0:X04}", R_filmEdge_0)
                        , string.Format("{0:X04}", R_printingEdge_0)
                        , string.Format("{0:X04}", R_printing_FilmEdge_0)
                          //DataType_Length
                          //, string.Format("{0:X04}", W100)
                          //, string.Format("{0:X04}", W101)
                          //, string.Format("{0:X04}", W102)
                          //, string.Format("{0:X04}", L100)
                          //, string.Format("{0:X04}", L200)
                          //, string.Format("{0:X04}", LDIFF)
                          , MelsecDataConverter.WInt((int)(W100))
                          , MelsecDataConverter.WInt((int)(W101))
                          , MelsecDataConverter.WInt((int)(W102))
                          , MelsecDataConverter.WInt((int)(L100))
                          , MelsecDataConverter.WInt((int)(L200))
                          , MelsecDataConverter.WInt((int)(LDIFF))
                        );

                    SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfEDMSCommand.SET_EDMS, mergeString);
                    
                    if (resultString == "0001")
                        LogHelper.Error(LoggerType.Network, string.Format("★ EDMS Sended NG Signal to Machine PLC (position={0})", myInspectionResult.RollDistance.ToString()) );
                    //LogHelper.Error(LoggerType.Error, string.Format("MachineIf::Send Timeout. {0}", protocol.Command == null ? "" : protocol.Command.ToString()));

                }// != Judgment.Skip)
            }// if ((myInspectionResult.State == State_EDMS.Inspecting) )
            else  // init, waiting, zeroing, 
            {
                RepetitionCount = 0;
            }
        }

        private double MicrometerToMilimeter(double microMeter)
        {
            double milimeter = (microMeter != 0.0) ? milimeter = microMeter / 1000 : 0.0;
            return milimeter;
        }

    }
}
