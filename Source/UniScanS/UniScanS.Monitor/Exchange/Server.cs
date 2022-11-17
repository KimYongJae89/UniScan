using DynMvp.Devices.Comm;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Threading;
using UniEye.Base.MachineInterface;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.Monitor;
using UniScan.Inspector.Exchange;

namespace UniScanS.Monitor.Exchange
{
    internal class Server : TcpIpMachineIfServer
    {
        ExchangeProtocolList exchangeProtocolList;

        List<InspectorObj> inspectorList = new List<InspectorObj>();
        internal List<InspectorObj> InspectorList
        {
            get { return inspectorList; }
        }

        public Server(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            exchangeProtocolList = (ExchangeProtocolList)MonitorSystemSettings.Instance().ServerSetting.MachineIfProtocolList;
        }

        public override void Initialize()
        {
            foreach (InspectorInfo inspectorInfo in MonitorSystemSettings.Instance().InspectorInfoList)
                inspectorList.Add(new InspectorObj(inspectorInfo));

            AddExecuter(new JobExecuter());
            AddExecuter(new StateExecuter());
            AddExecuter(new InspectExcuter());

            base.Initialize();

            serverSocket.ClientConnected += ClienctConnected;
            serverSocket.ClientDisconnected += ClienctDisConnected;

            //Start();
        }

        private void ClienctConnected(ClientHandlerSocket clientHandlerSocket)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Info.Address == clientHandlerSocket.GetRemoteIpAddress())
                {
                    inspector.CommState = CommState.CONNECTED;
                    inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                    inspector.OpState = UniEye.Base.Data.OpState.Idle;
                }
            }

            bool connectedAll = true;
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.CommState == CommState.DISCONNECTED)
                    connectedAll = false;
            }

            if (connectedAll == true)
            {
                IServerExchangeOperator serverExchangeOperator = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                serverExchangeOperator.Connected();
            }   
        }

        private void ClienctDisConnected(ClientHandlerSocket clientHandlerSocket)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Info.Address == clientHandlerSocket.GetRemoteIpAddress())
                {
                    inspector.CommState = CommState.DISCONNECTED;
                    inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                    inspector.OpState = UniEye.Base.Data.OpState.Idle;
                }
            }

            IServerExchangeOperator serverExchangeOperator = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            serverExchangeOperator.Disconnected();
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

        public bool ModelTrained(int camIndex, ModelDescription modelDescription)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (camIndex == inspector.Info.CamIndex)
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
            foreach (InspectorObj inspector in InspectorList)
                inspector.NewModel(modelDescription);

            SendCommand(exchangeProtocolList, ExchangeCommand.M_REFRESH);
        }

        public void SelectModel(ModelDescription modelDescription)
        {
            SendCommand(exchangeProtocolList, ExchangeCommand.M_SELECT, modelDescription.GetArgs());

            foreach (InspectorObj inspector in InspectorList)
                inspector.LoadModel(modelDescription);
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            SendCommand(exchangeProtocolList, ExchangeCommand.M_CLOSE);

            foreach (InspectorObj inspector in InspectorList)
                inspector.DeleteModel(modelDescription);

            SendCommand(exchangeProtocolList, ExchangeCommand.M_REFRESH);
        }

        public void SendVisit(ExchangeCommand eVisit)
        {
            SendCommand(exchangeProtocolList, eVisit);
        }

        public void SendCommand(ExchangeCommand eCommand, params string[] args)
        {
            SendCommand(exchangeProtocolList, eCommand, args);
        }

        public bool WaitJobDone(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            bool success = false;

            SimpleProgressForm jobWaitForm = new SimpleProgressForm(message);
            jobWaitForm.TopMost = true;

            jobWaitForm.Show(new Action(() =>
            {
                bool jobDoneAll = false;

                while (jobDoneAll == false)
                {
                    jobDoneAll = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        //if (inspector.JobState == JobState.RUN)
                        //    jobDoneAll = false;
                    }

                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        success = false;
                        return;
                    }

                    Thread.Sleep(10);
                }

                success = true;

            }), cancellationTokenSource);

            //foreach (InspectorObj inspector in InspectorList)
            //{
            //    if (inspector.JobState == JobState.ERROR)
            //    {
            //        success = false;
            //        break;
            //    }
            //}

            return true;
        }
    }
}
