using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Module.Controller.Settings.Monitor;

namespace UniScanG.Module.Controller.Exchange
{
    internal class Server : TcpIpMachineIfServer
    {
        ExchangeProtocolList exchangeProtocolList;

        internal List<InspectorObj> InspectorList => this.inspectorList;
        List<InspectorObj> inspectorList = new List<InspectorObj>();

        public Server(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            exchangeProtocolList = (ExchangeProtocolList)MonitorSystemSettings.Instance().ServerSetting.MachineIfProtocolList;
        }

        public override void Initialize()
        {
            List<InspectorInfo> inspectorInfoList = MonitorSystemSettings.Instance().InspectorInfoList.FindAll(f => f.Use);
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
                inspectorList.Add(new InspectorObj(inspectorInfo));

            AddExecuter(new JobExecuter());
            AddExecuter(new StateExecuter());
            AddExecuter(new ModelExecuter());
            AddExecuter(new InspectExcuter());
            AddExecuter(new CommExecuter());

            serverSocket.ClientConnected += ClienctConnected;
            serverSocket.ClientDisconnected += ClienctDisConnected;

            base.Initialize();
        }

        private void ClienctConnected(ClientHandlerSocket clientHandlerSocket)
        {
            string ip = clientHandlerSocket.GetRemoteIpAddress();
            List<InspectorObj> inspectorList = this.inspectorList.FindAll(f => f.Info.IpAddress == ip);
            inspectorList.ForEach(inspector =>
            {
                int code = 0;
                //Tuple<int, int> tuple = AddressManager.Instance().GetInspectorInfo(inspector.Info.IpAddress);

                // 검사기 연결되면 공유폴더 연결해봄
                if (inspector.Info.IpAddress != "127.0.0.1")
                    code = inspector.NetworkDrive.ConnectNetworkDrive(null, inspector.Info.Path, inspector.Info.UserId, inspector.Info.UserPw);

                if (code == 0)
                {
                    //inspector.CommState = CommState.CONNECTED;
                    //inspector.IpAddress = ip;
                    //ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected,
                    //    ErrorLevel.Info, inspector.Info.GetName(), "Inspector Connected.", null, ""));
                    //inspector.ModelManager.Init(Path.Combine(inspector.Info.Path, "Model"));
                }
                else
                {
                    //inspector.CommState = CommState.DISCONNECTED;
                    //ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Connected, 
                    //    ErrorLevel.Info, inspector.Info.GetName(), "Inspector Connected. But file share is disable", null, ""));
                }

                //inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                //inspector.OpState = UniEye.Base.Data.OpState.Idle;
                SyncAdditionalSettings(inspector);

                string cameraConfigFlag = DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag;

                SendCommand(ExchangeCommand.C_CONNECTED, inspector.Info.CamIndex.ToString(), inspector.Info.ClientIndex.ToString(), ip, cameraConfigFlag);
            });

            User user = UserHandler.Instance().CurrentUser;
            SendCommand(ExchangeCommand.U_CHANGE, user.Id, user.UserType.ToString());


            MachineIfData machineIfData = (MachineIfData)SystemManager.Instance().DeviceControllerG.MachineIfMonitor.MachineIfData;
            SendCommand(ExchangeCommand.C_SPD, machineIfData.GET_START_GRAVURE_INSPECTOR.ToString(), machineIfData.GET_PRESENT_SPEED_REAL.ToString());
            SendCommand(ExchangeCommand.C_LICENSE, LicenseManager.Licenses);
            SendCommand(ExchangeCommand.C_SYNC);
        }

        private void SyncAdditionalSettings(InspectorObj inspector)
        {
            try
            {
                IServerExchangeOperator server = SystemManager.Instance()?.ExchangeOperator as IServerExchangeOperator;
                if (server == null)
                    return;

                string src = Path.Combine(PathSettings.Instance().Config, AdditionalSettings.FileName);

                string dst = Path.Combine(inspector.Info.Path, "Config", AdditionalSettings.FileName);
                FileHelper.CopyFile(src, dst, true);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                string msg = StringManager.GetString("Setting Sync Fail");
                MessageForm.Show(ConfigHelper.Instance().MainForm, $"{msg}{Environment.NewLine}{ex.GetType().Name}: {ex.Message}");
            }
        }

        private void ClienctDisConnected(ClientHandlerSocket clientHandlerSocket)
        {
            string ip = clientHandlerSocket.GetRemoteIpAddress();
            List<InspectorObj> inspectorList = this.inspectorList.FindAll(f => f.Info.IpAddress == ip);
            if (inspectorList.Count == 0)
                return;

            inspectorList.ForEach(inspector =>
           {
               inspector.CommState = CommState.DISCONNECTED;
               inspector.InspectState = UniEye.Base.Data.InspectState.Done;
               inspector.OpState = UniEye.Base.Data.OpState.Idle;
               inspector.ResetJobState();

               inspector.NetworkDrive.DisconnectNetworkDrive();
               //ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Comms.Disconnected, ErrorLevel.Warning, inspector.Info.GetName(), "Inspector Disconnected", null, ""));
           });

            bool isAlarm = (SystemState.Instance().OpState != OpState.Idle);
            if (true)
            {
                //SystemManager.Instance().InspectRunner.ExitWaitInspection();
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Comms.Disconnected, isAlarm ? ErrorLevel.Error : ErrorLevel.Info,
                    inspectorList[0].Info.GetName(), "Inspector {0} was disconnected.", new object[] { inspectorList[0].Info.GetName() });
            }
        }

        protected override TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new ExchangePacketParser(exchangeProtocolList);
        }

        public bool ModelTrained(ModelDescription modelDescription)
        {
            bool trained = true;

            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.IsTrained(modelDescription) == false)
                {
                    trained = false;
                    break;
                }
            }

            return trained;
        }

        public bool ModelTrained(int camIndex, int clientIndex, ModelDescription modelDescription)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (camIndex == inspector.Info.CamIndex && clientIndex == inspector.Info.ClientIndex)
                {
                    if (inspector.IsTrained(modelDescription) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ModelExist(ModelDescription modelDescription)
        {
            bool exist = true;

            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Exist(modelDescription) == false)
                {
                    exist = false;
                    break;
                }
            }

            return exist;
        }

        public void NewModel(ModelDescription modelDescription)
        {
            Predicate<InspectorObj> match = new Predicate<InspectorObj>(f => f.IsConnected);
            ResetModelSelectState(match);

            List<object> argList = new List<object>();
            argList.Add("-1");
            argList.Add("-1");
            argList.AddRange(modelDescription.GetArgs());
            SendCommand(exchangeProtocolList, ExchangeCommand.M_CREATE, argList.ConvertAll(f => f.ToString()).ToArray());

            WaitModelSelectState(match, StringManager.GetString("Create Remote Model"), StringManager.GetString("Model [{0}] Create Fail"), modelDescription.Name);
        }

        public void SelectModel(int camId, int cliendId, ModelDescription modelDescription)
        {
            Predicate<InspectorObj> match = new Predicate<InspectorObj>(f => f.IsConnected
            && (camId < 0 || f.Info.CamIndex == camId)
            && (cliendId < 0 || f.Info.ClientIndex == cliendId));
            List<InspectorObj> inspectorObjList = this.inspectorList.FindAll(match);

            ResetModelSelectState(match);

            List<object> argList = new List<object>();
            argList.Add(camId);
            argList.Add(cliendId);
            argList.AddRange(modelDescription.GetArgs());
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_SELECT, argList.ConvertAll(f => f.ToString()).ToArray());

            WaitModelSelectState(match, StringManager.GetString("Open Remote Model"), StringManager.GetString("Model [{0}] Open Fail"), modelDescription.Name);
        }

        public void ReselectModel(int camId)
        {
            Predicate<InspectorObj> match = new Predicate<InspectorObj>(f => f.IsConnected && f.Info.CamIndex == camId);
            string modelName = SystemManager.Instance().CurrentModel?.Name;

            ResetModelSelectState(match);

            List<string> argList = new List<string>();
            argList.Add(camId.ToString());
            argList.Add("-1");
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_RESELECT, argList.ToArray());

            WaitModelSelectState(match, StringManager.GetString("Open Remote Model"), StringManager.GetString("Model [{0}] Open Fail"), modelName);
        }

        public void CloseModel()
        {
            Predicate<InspectorObj> match = new Predicate<InspectorObj>(f => f.IsConnected);
            ResetModelSelectState(match);

            string modelName = SystemManager.Instance().CurrentModel?.Name;
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_CLOSE);

            WaitModelSelectState(match, StringManager.GetString("Close Remote Model"), StringManager.GetString("Model [{0}] Close Fail"), modelName);
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            Predicate<InspectorObj> match = new Predicate<InspectorObj>(f => f.IsConnected);
            ResetModelSelectState(match);

            List<object> argList = new List<object>();
            argList.Add("-1");
            argList.Add("-1");
            argList.AddRange(modelDescription.GetArgs());
            SendCommand(exchangeProtocolList, ExchangeCommand.M_DELETE, argList.ConvertAll(f => f.ToString()).ToArray());

            WaitModelSelectState(match, StringManager.GetString("Delete Remote Model"), StringManager.GetString("Model [{0}] Delete Fail"), modelDescription.Name);

            //foreach (InspectorObj inspector in InspectorList)
            //    inspector.DeleteModel(modelDescription);

            SendCommand(exchangeProtocolList, ExchangeCommand.M_REFRESH);
        }

        public void SendCommand(ExchangeCommand eCommand, params string[] args)
        {
            SendCommand(exchangeProtocolList, eCommand, args);
        }

        public void ResetModelSelectState(Predicate<InspectorObj> match)
        {
            this.inspectorList.FindAll(match).ForEach(f => f.ResetModelSelectState());
        }

        public void WaitModelSelectState(Predicate<InspectorObj> match, string message, string exceptionMessageFormat, string modelName)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            SimpleProgressForm jobWaitForm = new SimpleProgressForm(message);
            jobWaitForm.Show(ConfigHelper.Instance().MainForm, new Action(() =>
            {
                bool done = false;
                bool error = false;
                do
                {
                    List<InspectorObj> connectedInspectorList = this.inspectorList.FindAll(match);
                    done = connectedInspectorList.TrueForAll(f => f.ModelSelectState.IsJobDone);
                    error = connectedInspectorList.Exists(f => f.ModelSelectState.IsJobError);

                    Thread.Sleep(100);
                } while (!done && !error && !cancellationTokenSource.IsCancellationRequested);
            }), cancellationTokenSource);

            if (cancellationTokenSource.Token.IsCancellationRequested)
                throw new OperationCanceledException("Operation is Canceled");
            //cancellationTokenSource.Token.ThrowIfCancellationRequested();
            if (this.inspectorList.Exists(f => f.ModelSelectState.IsJobError))
                throw new Exception(string.Format(exceptionMessageFormat, modelName));
        }
    }
}
