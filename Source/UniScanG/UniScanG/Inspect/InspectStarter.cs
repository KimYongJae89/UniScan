using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
//using DynMvp.Data;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.MachineIF;
using Production = UniScanG.Data.Production;

namespace UniScanG.Inspect
{
    public class InspectStarter : ThreadHandler
    {
        AdditionalSettings additionalSettings = null;
        MachineIF.MachineIfData machineIfData;
        CancellationTokenSource cancellationTokenSource = null;

        public InspectStarter() : base("InspectStarter")
        {
            this.workingThread = new System.Threading.Thread(ThreadProc);

            this.additionalSettings = (AdditionalSettings)AdditionalSettings.Instance();
            this.machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIF.MachineIfData;
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        bool logState = false;
        private void ThreadProc()
        {
            bool autoStartEnable = true;
            string oldMachineLot = null;
            RewinderZone oldRewinderZone = RewinderZone.Unknowen;

            Thread.Sleep(5000);
            while (this.requestStop == false)
            {
                try
                {
                    Thread.Sleep(1000);

                    string machineLot = this.machineIfData.GET_LOT;;
                    if (oldMachineLot == null)
                        oldMachineLot = machineLot;
                    
                    RewinderZone rewinderZone = this.machineIfData.RewinderZone;
                    if (oldRewinderZone == RewinderZone.Unknowen)
                        oldRewinderZone = rewinderZone;
                    
                    bool startRequest = this.machineIfData.GET_START_GRAVURE_INSPECTOR;
                    AppendLog(startRequest);

                    bool isSameLot = oldMachineLot.Equals(machineLot);
                    if (!isSameLot)
                        LogHelper.Info(LoggerType.Operation, string.Format("InspectStarter::ThreadProc - Lot: {0} => {1}", oldMachineLot, machineLot));

                    bool isSameZone = oldRewinderZone.Equals(rewinderZone);
                    if (!isSameZone)
                        LogHelper.Info(LoggerType.Operation, string.Format("InspectStarter::ThreadProc - Zone: {0} => {1}", oldRewinderZone, rewinderZone));

                    if (additionalSettings.AutoOperation)
                    {
                        if (startRequest)
                        // 시작신호 들어옴 
                        {
                             if (SystemState.Instance().OpState == OpState.Inspect)
                            // 검사 중 -> 로트/리와인더 정보 확인. Production 교체.
                            {
                                bool isLotChanged = (!isSameLot || !isSameZone);
                                if (isLotChanged)
                                // 왜인지 모르겠는데 가끔 이 루프를 탐... 
                                {
                                    LogHelper.Info(LoggerType.Operation, string.Format("InspectStarter::ThreadProc - lotChange: {0}", isLotChanged));

                                    if (AdditionalSettings.Instance().DebugAutoLotChange)
                                    {
                                        float newSpd = this.machineIfData.GET_TARGET_SPEED_REAL;
                                        string newLot = machineLot;
                                        if (string.IsNullOrEmpty(newLot))
                                            newLot = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                                        SystemManager.Instance().InspectRunner.EnterPauseInspection();

                                        Model curModel = SystemManager.Instance().CurrentModel;
                                        SystemManager.Instance().ProductionManager.LotChange(curModel, DateTime.Now, newLot, rewinderZone, newSpd);
                                        SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.I_LOTCHANGE,
                                            newLot, rewinderZone.ToString(), newSpd.ToString());

                                        SystemManager.Instance().InspectRunner.ExitPauseInspection();
                                    }
                                }
                            }
                            else
                            // 검시 시작
                            {
                                if (autoStartEnable)
                                {
                                    autoStartEnable = false; // 설비동작 후 검사시작 '아니오' 시 설비정비 이전까지 자동시작 막기

                                    Form mainForm = Common.ConfigHelper.Instance().MainForm;
                                    Task.Run(() =>
                                    {
                                        bool ok = EnterWaitInspection(mainForm, false, new CancellationTokenSource());
                                            //if (ok && cancellationTokenSource.IsCancellationRequested == false)
                                            //    ok = SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                                            (SystemManager.Instance().UiChanger.InspectControl as UI.Inspect.InspectPage)?.UpdateBlockStateChange(!ok);
                                    });
                                }
                            }
                        }
                        else
                        // 시작신호 꺼짐 -> 검사 정지
                        {
                            if (!autoStartEnable)
                            {
                                autoStartEnable = true;
                                cancellationTokenSource?.Cancel();
                            }

                            if (SystemState.Instance().OpState == OpState.Inspect)
                                ExitWaitInspection();
                        }

                    }
                    oldMachineLot = machineLot;
                    oldRewinderZone = rewinderZone;
                }
                catch (AlarmException ex)
                {
                    ErrorManager.Instance().Report(ex);
                }
            }
        }

        private void AppendLog(bool v)
        {
            if (this.logState != v)
            {
                LogHelper.Info(LoggerType.Operation, string.Format("InspectStarter::ThreadProc - StartRequest {0}", v));

                float targetSpd = this.machineIfData.GET_TARGET_SPEED_REAL;
                string mess = "Printing Ended.";
                if (v)
                {
                    mess = "Printing Started.";
                    if (targetSpd > 0)
                        mess += " Speed {0}[mpm]";
                }

                ErrorManager.Instance().Report(new AlarmException(ErrorCodeInspect.Instance.Information, ErrorLevel.Info,
                    mess, new object[] { this.machineIfData.GET_TARGET_SPEED_REAL }, ""));
                this.logState = v;
            }
        }

        bool OnStarting = false;
        public delegate bool EnterWaitInspectionDelegate(Control mainForm, bool isUserStart, CancellationTokenSource cancellationTokenSource);
        public bool EnterWaitInspection(Control mainForm, bool isUserStart, CancellationTokenSource cancellationTokenSource)
        {
            if (mainForm.InvokeRequired)
            {
                return (bool)mainForm.Invoke(new EnterWaitInspectionDelegate(EnterWaitInspection), mainForm, isUserStart, cancellationTokenSource);
            }

            if (this.OnStarting)
                return false;

            this.OnStarting = true;

            this.cancellationTokenSource?.Cancel();
            this.cancellationTokenSource = cancellationTokenSource;

            try
            {
                if (!SystemManager.Instance().InspectRunnerG.IsRunable())
                    return false;

                Model curModel = SystemManager.Instance().CurrentModel;
                string modelName = curModel.Name;
                string machineModelname = this.machineIfData?.GET_MODEL;

                if (this.machineIfData != null)
                {
                    if (!this.machineIfData.GET_START_GRAVURE_INSPECTOR)
                    {
                        if (!UserHandler.Instance().CurrentUser.IsMasterAccount)
                        {
                            MessageForm.Show(mainForm, StringManager.GetString("Please Check the machine is running."));
                            return false;
                        }

                        string message = StringManager.GetString("Machine is not running. Continue Anyway?");
                        if (MessageForm.Show(mainForm, message, MessageFormType.YesNo, DialogResult.No, this.cancellationTokenSource.Token)!= DialogResult.Yes)
                            return false;                        
                    }
                }

                // 인쇄기 모델명과 검사기 모델명이 다르면 물어봄.
                // '시작 전 질의' 가 False이면 묻지 않음.
                if (additionalSettings.StartUserQuary && (modelName != machineModelname))
                {
                    StringBuilder modelCheckStringBuilder = new StringBuilder();
                    modelCheckStringBuilder.AppendLine(string.Format(StringManager.GetString("Selected model is [{0}]."), modelName));

                    if (string.IsNullOrEmpty(machineModelname))
                        modelCheckStringBuilder.AppendLine(StringManager.GetString("There is no progressing model."));
                    else
                        modelCheckStringBuilder.AppendLine(string.Format(StringManager.GetString("Progressing model is [{0}]."), machineModelname));

                    modelCheckStringBuilder.AppendLine(StringManager.GetString("Do you want to continue?"));

                    bool userConfirm = MessageForm.Show(mainForm, modelCheckStringBuilder.ToString().Trim(), MessageFormType.YesNo, DialogResult.No, this.cancellationTokenSource.Token) == DialogResult.Yes;
                    if (userConfirm == false)
                        return false;
                }

                // LOT 읽음
                UniScanG.Data.Production oldProduction = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.GetLastProduction(curModel);
                string newLotName = this.machineIfData?.GET_LOT.Trim(' ', '\0');
                if (string.IsNullOrEmpty(newLotName))
                    newLotName = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                if (isUserStart)
                // 사용자 시작
                {
                    //if (isUserStart)
                    {
                        // 확인창
                        UniScanG.Data.UI.InputForm inputForm = new UniScanG.Data.UI.InputForm("Lot No", newLotName);
                        inputForm.ValidCheckFunc = new Data.UI.ValidCheckFunc(f => !f.Contains('\\') && !string.IsNullOrEmpty(f.Trim(' ', '\0')));
                        inputForm.CancellationToken = this.cancellationTokenSource.Token;

                        DialogResult dialogResult = inputForm.ShowDialog(mainForm);
                        if (dialogResult == DialogResult.Cancel)
                            return false;

                        newLotName = inputForm.InputText.Trim(' ', '\0');
                    }
                }

                RewinderZone rewinderZone = this.machineIfData == null ? RewinderZone.Unknowen : !this.machineIfData.GET_REWINDER_CUT ? RewinderZone.ZoneA : RewinderZone.ZoneB;
                float targetSpeed = this.machineIfData != null ? this.machineIfData.GET_TARGET_SPEED_REAL : additionalSettings.AsyncMode == EAsyncMode.True ? additionalSettings.AsyncGrabMpm : 0;
                SystemManager.Instance().ProductionManager.LotChange(curModel, DateTime.Now.Date, newLotName, rewinderZone, targetSpeed);
                SystemManager.Instance().ProductionManager.Save();
                (SystemManager.Instance().UiChanger.InspectControl as UI.Inspect.InspectPage)?.InspectDefectPanel.Reset();
                SystemManager.Instance().UiController.ChangeTab(UI.MainTabKey.Inspect.ToString());

                float presentPos = this.machineIfData == null ? 0 : this.machineIfData.GET_PRESENT_POSITION;
                string logFormat = presentPos == 0 ? "Begin Inspection." : "Begin Inspection. Distance: {0}[m]";
                object[] logArgs = presentPos == 0 ? new object[0] : new object[] { this.machineIfData?.GET_PRESENT_POSITION.ToString("F0") };
                ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Info,
                     logFormat, logArgs, ""));
                bool ok = SystemManager.Instance().InspectRunner.EnterWaitInspection();
                if (ok)
                {
                    ok = SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                    if (!ok )
                    {
                        bool isAlarmed = ErrorManager.Instance().IsAlarmed();
                        if(!isAlarmed)
                            ExitWaitInspection();
                    }
                }
                return ok;
            }
            catch (Exception ex)
            {
                SystemManager.Instance().InspectRunner.ExitWaitInspection();
                if (isUserStart)
                    MessageForm.Show(mainForm, StringManager.GetString(this.GetType().FullName, ex.Message));
                return false;
            }
            finally
            {
                this.OnStarting = false;
            }
        }

        public void ExitWaitInspection()
        {
            this.cancellationTokenSource?.Cancel();
            float presentPos = this.machineIfData == null ? 0 : this.machineIfData.GET_PRESENT_POSITION;
            string logFormat = presentPos == 0 ? "End Inspection." : "End Inspection. Distance: {0}[m]";
            object[] logArgs = presentPos == 0 ? new object[0] : new object[] { this.machineIfData?.GET_PRESENT_POSITION.ToString("F0") };
            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Info, logFormat, logArgs, ""));
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
            //ErrorManager.Instance().ResetAllAlarm();
        }

        private MachineIfProtocolResponce SendCommand(Enum e)
        {
            MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
            MachineIfProtocol protocol = list?.GetProtocol(e);
            MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
            responce?.WaitResponce();
            return responce;
        }
    }
}
