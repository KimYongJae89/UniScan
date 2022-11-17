using DynMvp.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniEye.Base.MachineInterface;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings.Monitor;
using UniScanS.Common.UI;
using UniScanS.Common.Util;
using UniScanS.Monitor.Exchange;

namespace UniScanS.Monitor
{
    internal class MonitorOperator : ExchangeOperator, IServerExchangeOperator, IUserHandlerListener
    {
        List<ICommListener> commListenerList = new List<ICommListener>();

        Server server;
        public Server Server
        {
            get { return server; }
        }
        
        public MonitorOperator()
        {
            this.server = new Server(MonitorSystemSettings.Instance().ServerSetting);
        }

        public override void Initialize()
        {
            this.server.Initialize();
            UserHandler.Instance().AddListener(this);
        }

        public override bool ModelTrained(ModelDescription modelDescription)
        {
            modelDescription.IsTrained = server.ModelTrained(modelDescription);

            return modelDescription.IsTrained;
        }

        public override void ModelTeachDone(string camIndex)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            SystemManager.Instance().CurrentModel.ModelDescription.IsTrained = server.ModelTrained(SystemManager.Instance().CurrentModel.ModelDescription);
            SystemManager.Instance().CurrentModel.ModelDescription.LastModifiedDate = DateTime.Now;

            SystemManager.Instance().ModelManager.SaveModelDescription(SystemManager.Instance().CurrentModel.ModelDescription);
            //SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);

            if (camIndex != null)
            {
                int index = Convert.ToInt32(camIndex);
                InspectorObj inspector = server.InspectorList.Find(obj => obj.Info.CamIndex == index);
                if (inspector != null)
                    inspector.LoadModel(SystemManager.Instance().CurrentModel.ModelDescription);
            }

            //server.InspectorList()
            base.ModelTeachDone();
        }

        public void AddCommmListener(ICommListener listener)
        {
            commListenerList.Add(listener);
        }

        public void Connected()
        {
            foreach (ICommListener commListner in commListenerList)
                commListner.Connected();
        }

        public void Disconnected()
        {
            if (UniEye.Base.Data.SystemState.Instance().IsInspectOrWait == true)
                SystemManager.Instance().InspectRunner.ExitWaitInspection();

            SystemManager.Instance().ModelManager.CloseModel();

            foreach (IModelListener listner in modelListenerList)
                listner.ModelChanged();

            foreach (ICommListener commListner in commListenerList)
                commListner.Disconnected();
        }

        public override bool ModelExist(ModelDescription modelDescription)
        {
            if (server == null)
                return false;

            return server.ModelExist(modelDescription);
        }

        public override bool SelectModel(ModelDescription modelDescription)
        {
            server.SelectModel(modelDescription);

            return base.SelectModel(modelDescription);
        }

        public override void DeleteModel(ModelDescription modelDescription)
        {
            server.DeleteModel(modelDescription);

            base.DeleteModel(modelDescription);
        }

        public override bool NewModel(ModelDescription modelDescription)
        {
            server.NewModel(modelDescription);
            
            return base.NewModel(modelDescription);
        }

        public List<InspectorObj> GetInspectorList()
        {
            return server.InspectorList; 
        }
        
        public void CloseVnc()
        {
            server.SendVisit(ExchangeCommand.V_DONE);
        }

        public Process OpenVnc(ExchangeCommand eVisit, Process process, string ipAddress, IntPtr handle)
        {
            server.SendVisit(eVisit);

            return VncHelper.OpenVnc(process, ipAddress, handle, MonitorSystemSettings.Instance().VncPath);
        }

        public override void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            server.SendCommand(exchangeCommand, args);
        }

        public bool ModelTrained(int camIndex, ModelDescription modelDescription)
        {
            return server.ModelTrained(camIndex, modelDescription);
        }

        public void UserChanged()
        {
            server.SendCommand(ExchangeCommand.U_CHANGE, UserHandler.Instance().CurrentUser.Id);
        }

        public override void Release()
        {
            server.Release();
        }

        public void AlgorithmSettingChange()
        {
            server.SendCommand(ExchangeCommand.M_PARAM);
        }

        public override void Start()
        {
            //server.Start();
        }
    }
}
