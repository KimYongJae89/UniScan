using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Device;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public delegate void OnStartDelegate();
    public delegate void OnEndDelegate(bool isError, bool isCanceled);

    public class OperatorManager : IOperatorListner
    {
        //public event OnStartDelegate OnInspectStart;
        //public event OnEndDelegate OnInspectEnd;

        //public event OnStartDelegate OnTeachStart;
        //public event OnEndDelegate OnTeachEnd;

        public bool IsTeachMode => this.isTeachMode;
        private bool isTeachMode = false;

        CancellationTokenSource cancellationTokenSource;

        IoPort doorPort;

        ResultCombiner resultCombiner;

        ResultStorage resultStorage;

        List<Operator> operatorList;
        LightTuneOperator lightTuneOperator;
        ScanOperator scanOperator;
        ExtractOperator extractOperator;
        InspectOperator inspectOperator;
        TeachOperator teachOperator;
        
        public LightTuneOperator LightTuneOperator { get => lightTuneOperator; }
        public ScanOperator ScanOperator { get => scanOperator; }
        public ExtractOperator ExtractOperator { get => extractOperator; }
        public InspectOperator InspectOperator { get => inspectOperator; }
        public TeachOperator TeachOperator { get => teachOperator; }
        public ResultCombiner ResultCombiner { get => resultCombiner; }
        public ResultStorage ResultStorage { get => resultStorage; }
        
        public bool IsRun
        {
            get => operatorList.Exists(op =>op.OperatorState != OperatorState.Idle);
        }

        public bool IsIdle
        {
            get => operatorList.All(op => op.OperatorState == OperatorState.Idle);
        }

        public OperatorManager()
        {
            ThreadPool.GetMaxThreads(out int workerMaxThreads, out int ioMaxhreads);
            ThreadPool.SetMinThreads(workerMaxThreads, ioMaxhreads);

            operatorList = new List<Operator>();

            lightTuneOperator = new LightTuneOperator();
            scanOperator = new ScanOperator();
            extractOperator = new ExtractOperator();
            inspectOperator = new InspectOperator();
            teachOperator = new TeachOperator();

            resultCombiner = new ResultCombiner();
            resultStorage = new ResultStorage();

            operatorList.Add(lightTuneOperator);
            operatorList.Add(scanOperator);
            operatorList.Add(extractOperator);
            operatorList.Add(inspectOperator);
            operatorList.Add(teachOperator);

            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;

            if (SystemManager.Instance().DeviceBox.PortMap != null)
            {
                PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;
                doorPort = portMap.GetOutPort(PortMap.OutPortName.OutDoorLock);
                SystemManager.Instance().DeviceBox.DigitalIoHandler?.WriteOutput(doorPort, false);
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            }

            SystemManager.Instance().AddOperatorListnerList(this);
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            Cancle();
        }

        public bool Cancle(string message = null)
        {
            LogHelper.Info(LoggerType.Inspection, "Stop Operation");

            cancellationTokenSource?.Cancel();
            SystemManager.Instance().DeviceController.RobotStage?.StopMove();
            SystemManager.Instance().DeviceBox.ImageDeviceHandler?.Stop();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
            SystemManager.Instance().DeviceController.RobotStage?.WaitMoveDone();

            if (message != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CustomMessageBox.Show(message, null, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }));
            }

            operatorList.ForEach(oper => oper.Release());

            return false;
        }
        
        public bool Start(bool teachMode)
        {
            if (operatorList.Exists(op => op.OperatorState != OperatorState.Idle))
                return Cancle(LocalizeHelper.GetString("Machine is running !!"));
            
            if (SystemManager.Instance().CurrentModel == null)
                return Cancle(LocalizeHelper.GetString("Not selected model !!"));

            IOBox ioBox = SystemManager.Instance().MachineObserver.IoBox;
            if (ioBox.InOnDoor1 == false || ioBox.InOnDoor2 == false)
                return Cancle(LocalizeHelper.GetString("Door is open !!"));

            if (teachMode == false)
                if (SystemManager.Instance().ProductionManager.CurProduction == null)
                    return Cancle(LocalizeHelper.GetString("Not input lot no !!"));

            int totalSteps = this.scanOperator.GetTotalSteps();
            if (totalSteps < 0)
                return Cancle(LocalizeHelper.GetString("Scan Region Setting is Worng !!"));

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            if (imageDeviceHandler.IsVirtual)
                imageDeviceHandler.SetExposureTime(500000);

            cancellationTokenSource = new CancellationTokenSource();
            this.isTeachMode = teachMode;
            Task.Factory.StartNew(() =>
            {
                if (this.isTeachMode == false)
                {
                    LogHelper.Info(LoggerType.Inspection, "Start Inspection");

                    Production production = SystemManager.Instance().ProductionManager.CurProduction;
                    ResultKey resultKey = new ResultKey(DateTime.Now, SystemManager.Instance().CurrentModel, production);

                    //OnInspectStart?.Invoke();
                    operatorList.ForEach(op => op.Initialize(resultKey, totalSteps, cancellationTokenSource));
                }
                else
                {
                    if(this.scanOperator.IsSaving())
                    {
                        Cancle(LocalizeHelper.GetString("Previous Image is Saving. Try Later."));
                        return;
                    }

                    LogHelper.Info(LoggerType.Inspection, "Start Teach");

                    ResultKey resultKey = new ResultKey(DateTime.Now, SystemManager.Instance().CurrentModel, null);

                    //OnTeachStart?.Invoke();
                    operatorList.ForEach(op => op.Initialize(resultKey, totalSteps, cancellationTokenSource));
                }

                resultCombiner.Clear();
                resultStorage.Clear();
                SystemManager.Instance().OperatorCompleted(null);

                if (MachineOperator.IsHomeOK() == false)
                    if (MachineOperator.MoveHome(0, null, cancellationTokenSource) == false)
                    {
                        Cancle(LocalizeHelper.GetString("Homing fail !!"));
                        return;
                    }

                InfoBox.Instance.LastStartTime = DateTime.Now;
                
                if (this.isTeachMode == false)
                {
                    lightTuneOperator.Release();
                    teachOperator.Release();
                    scanOperator.Start();
                }
                else
                {
                    inspectOperator.Release();
                    lightTuneOperator.Start();
                }

            }, cancellationTokenSource.Token);

            return !cancellationTokenSource.IsCancellationRequested;
        }

        public void Processed(OperatorResult operatorResult)
        {
            LogHelper.Debug(LoggerType.Inspection, operatorResult.GetLog("Processed"), true);

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            resultCombiner.AddResult(operatorResult);

            if (operatorResult.IsError)
            {
                Cancle(operatorResult.Exception.Message);
                return;
            }

            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    break;
                case ResultType.Scan:
                    extractOperator.StartExtract(operatorResult);
                    break;
                case ResultType.Extract:
                    resultStorage.AddResult(operatorResult);
                    if (operatorResult.ResultKey.Production != null)
                        inspectOperator.StartInspect((ExtractOperatorResult)operatorResult);
                    break;
                case ResultType.Inspect:
                    resultStorage.AddResult(operatorResult);
                    inspectOperator.StartLumpInspect(((InspectOperatorResult)operatorResult).ExtractOperatorResult);
                    break;
                case ResultType.Train:
                    break;
                case ResultType.InspectLump:
                    resultStorage.AddResult(operatorResult);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        public void Completed(OperatorResult operatorResult)
        {
            if (operatorResult == null)
                return;

            LogHelper.Debug(LoggerType.Inspection, operatorResult.GetLog("Completed"), true);
            if (operatorResult.IsError)
            {
                Cancle(operatorResult.Exception.Message);
                return;
            }

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    lightTuneOperator.Release();
                    scanOperator.Start();
                    break;

                case ResultType.Scan:
                    scanOperator.Release();
                    extractOperator.WaitDone();
                    SystemManager.Instance().OperatorCompleted(new ExtractOperatorResult(extractOperator.ResultKey, null, (ScanOperatorResult)operatorResult));
                    break;

                case ResultType.Extract:
                    extractOperator.Release();
                    if (extractOperator.ResultKey.Production != null)
                    {
                        inspectOperator.WaitDone();
                        SystemManager.Instance().OperatorCompleted(new InspectOperatorResult(inspectOperator.ResultKey, null, (ExtractOperatorResult)operatorResult));
                    }
                    else
                    {
                        Teach();
                    }
                    break;

                case ResultType.Inspect:
                    inspectOperator.Measure();
                    LogHelper.Info(LoggerType.Operation, "End Inspection");
                    inspectOperator.Release();
                    resultCombiner.SaveInspectOperatorResult();
                    break;

                case ResultType.Train:
                    teachOperator.Settings.Save();
                    LogHelper.Info(LoggerType.Inspection, "End Teach");
                    operatorResult.ResultKey.Model.ModelTraind(operatorResult);
                    break;

                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        public bool Teach()
        {
            ResultKey resultKey = resultStorage.LastResultKey;

            if (resultKey == null || resultStorage.ContainsKey(resultKey) == false)
                return false;

            if (resultStorage[resultKey].ContainsKey(ResultType.Extract) == false)
                return false;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SimpleProgressWindow teachWindow = new SimpleProgressWindow(LocalizeHelper.GetString("Trainning.."));
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                teachWindow.Show(() =>
                {
                    ResultDictinary dictionary = resultStorage[resultStorage.LastResultKey];
                    //resultStorage[resultStorage.LastResultKey][ResultType.Train].Clear();

                    List<OperatorResult> opResult = dictionary[ResultType.Extract];
                    teachOperator.Train(opResult.ConvertAll(extractOperatorResult => (ExtractOperatorResult)extractOperatorResult));

                    inspectOperator.Settings.DiffThreshold = teachOperator.Settings.DiffGroupThreshold;
                }, cancellationTokenSource);
            }));

            return true;
        }

        public void SaveAll()
        {
            this.lightTuneOperator.Settings.Save();
            this.scanOperator.Settings.Save();
            this.extractOperator.Settings.Save();
            this.inspectOperator.Settings.Save();
            this.teachOperator.Settings.Save();
        }
    }
}