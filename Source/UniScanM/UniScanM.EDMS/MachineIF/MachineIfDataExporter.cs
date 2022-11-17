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
using UniScanM.EDMS.Settings;
using UniScanM.Operation;
using UniScanM.EDMS.Data;
using System.Diagnostics;

namespace UniScanM.EDMS.MachineIF
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
       
            UniScanM.EDMS.Data.InspectionResult myInspectionResult = (UniScanM.EDMS.Data.InspectionResult)inspectionResult;
            Debug.WriteLine("----MachineIfDataExporter->RepetitionCount=" + RepetitionCount.ToString());

            if (myInspectionResult.State == State_EDMS.Inspecting) 
            {
                if(myInspectionResult.Judgment == Judgment.Skip)
                {
                    RepetitionCount = 0;
                }
                else
                {
                    double[] edgeArray = myInspectionResult.TotalEdgePositionResult;
                    if (edgeArray == null) return;

                    short filmEdge = (short)((edgeArray[(int)Data.DataType.FilmEdge]) * 1000);
                    //filmEdge = Get3Values(filmEdge);

                    short coating_Film = (short)((edgeArray[(int)Data.DataType.Coating_Film]) * 1000);
                    //coating_Film = Get3Values(coating_Film);

                    short printing_Coating = (short)((edgeArray[(int)Data.DataType.Printing_Coating]) * 1000);
                    //printing_Coating = Get3Values(printing_Coating);

                    short filmEdge_0 = (short)((edgeArray[(int)Data.DataType.FilmEdge_0]) * 1000);
                    //filmEdge_0 = Get4Values(filmEdge_0);

                    short printingEdge_0 = (short)((edgeArray[(int)Data.DataType.PrintingEdge_0]) * 1000);
                    //printingEdge_0 = Get4Values(printingEdge_0);

                    short printing_FilmEdge_0 = (short)((edgeArray[(int)Data.DataType.Printing_FilmEdge_0]));
                    //printing_FilmEdge_0 = Get4Values(printing_FilmEdge_0);

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

                    //string mergeString =
                    //    string.Format("{0}{1}{2}{3}{4}{5}{6}",
                    //    resultString
                    //    , string.Format("{0:X04}", filmEdge)
                    //    , string.Format("{0:X04}", coating_Film)
                    //    , string.Format("{0:X04}", printing_Coating)
                    //    , string.Format("{0:X04}", filmEdge_0)
                    //    , string.Format("{0:X04}", printingEdge_0)
                    //    , string.Format("{0:X04}", printing_FilmEdge_0));
                    string[] mergeString = new string[]
                    {
                        resultString,
                        filmEdge.ToString("X04"),
                        coating_Film.ToString("X04"),
                        printing_Coating.ToString("X04"),
                        filmEdge_0.ToString("X04"), // -1은 0xFFFF = 65535
                        printingEdge_0.ToString("X04"), // -1은 0xFFFF = 65535
                        printing_FilmEdge_0.ToString("X04") // -1은 0xFFFF = 65535
                    };

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
