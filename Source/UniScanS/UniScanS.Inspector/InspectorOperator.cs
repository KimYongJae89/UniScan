using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.Inspector;
using UniScanS.Common.UI;
using UniScanS.Inspector.Exchange;

namespace UniScanS.Inspector
{
    public class InspectorOperator : ExchangeOperator, IClientExchangeOperator, IInspectStateListener, IOpStateListener
    {
        Client client;
        Server server;

        List<IVisitListener> visitListenerList = new List<IVisitListener>();
        List<IAlgorithmParamChangedListener> algorithmParamChangedListener = new List<IAlgorithmParamChangedListener>();

        public InspectorOperator()
        {
            
        }

        public override void Initialize()
        {
            client = new Client(InspectorSystemSettings.Instance().ClientSetting);

            if (InspectorSystemSettings.Instance().ClientIndex == 0 /*&& InspectorSystemSettings.Instance().SlaveInfoList.Count > 0*/)
            {
                server = new Server(InspectorSystemSettings.Instance().ServerSetting);
                server.Initialize();
            }

            SystemState.Instance().SetIdle();

            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);
        }

        public void AddVisitListener(IVisitListener visitListener)
        {
            visitListenerList.Add(visitListener);
        }

        public void AddAlgorithmParamChangedListener(IAlgorithmParamChangedListener listener)
        {
            algorithmParamChangedListener.Add(listener);
        }

        public void AlgorithmSettingChanged()
        {
            foreach (IAlgorithmParamChangedListener listener in algorithmParamChangedListener)
                listener.AlgorithmParamChanged();
        }

        public override void ModelTeachDone(string camIndex)
        {
            client.SendCommand(ExchangeCommand.M_TEACH_DONE, InspectorSystemSettings.Instance().CamIndex.ToString());

            base.ModelTeachDone();
        }

        public void PreparePanel(ExchangeCommand eVisit)
        {
            foreach (IVisitListener listener in visitListenerList)
                listener.PreparePanel(eVisit);
        }

        public void ClearPanel()
        {
            foreach (IVisitListener listener in visitListenerList)
                listener.Clear();
        }

        public override void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            client.SendCommand(exchangeCommand, args);
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            switch (curOpState)
            {
                case OpState.Idle:
                    client.SendCommand(ExchangeCommand.S_IDLE, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case OpState.Inspect:
                    client.SendCommand(ExchangeCommand.S_INSPECT, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case OpState.Teach:
                    client.SendCommand(ExchangeCommand.S_TEACH, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case OpState.Wait:
                    client.SendCommand(ExchangeCommand.S_WAIT, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
            }
        }

        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            switch (curInspectState)
            {
                case InspectState.Run:
                    client.SendCommand(ExchangeCommand.S_RUN, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case InspectState.Pause:
                    client.SendCommand(ExchangeCommand.S_PAUSE, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case InspectState.Wait:
                    client.SendCommand(ExchangeCommand.S_WAIT, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
                case InspectState.Done:
                    client.SendCommand(ExchangeCommand.S_DONE, InspectorSystemSettings.Instance().CamIndex.ToString());
                    break;
            }
        }

        public void SendInspectDone(string inspectionNo, string time)
        {
            client.SendCommand(ExchangeCommand.I_DONE, InspectorSystemSettings.Instance().CamIndex.ToString(), inspectionNo, time);
        }

        public int GetCamIndex()
        {
            return InspectorSystemSettings.Instance().CamIndex;
        }

        public override void Release()
        {
            client?.Release();
            server?.Release();
        }

        public override void Start()
        {
            //client.Start();
            //server.Start();
        }
    }
}