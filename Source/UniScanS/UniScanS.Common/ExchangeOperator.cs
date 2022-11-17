using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;

namespace UniScanS.Common
{
    public interface IModelListener
    {
        void ModelChanged();
        void ModelTeachDone();
    }

    public interface IVisitListener
    {
        void PreparePanel(ExchangeCommand trayPanel);
        void Clear();
    }

    public interface ICommListener
    {
        void Connected();
        void Disconnected();
    }

    public interface IServerExchangeOperator
    {
        List<InspectorObj> GetInspectorList();
        Process OpenVnc(ExchangeCommand trayPanel, Process process, string ipAddress, IntPtr handle);
        void CloseVnc();
        bool ModelTrained(int camIndex, ModelDescription modelDescription);
        void Connected();
        void Disconnected();
        void AddCommmListener(ICommListener listener);
        void AlgorithmSettingChange();
    }

    public interface IClientExchangeOperator
    {
        void AddVisitListener(IVisitListener visitListener);
        void AddAlgorithmParamChangedListener(IAlgorithmParamChangedListener listener);
        void SendInspectDone(string inspectionNo, string time);
        int GetCamIndex();
        void AlgorithmSettingChanged();
    }

    public interface IAlgorithmParamChangedListener
    {
        void AlgorithmParamChanged();
    }

    public abstract class ExchangeOperator
    {
        protected List<IModelListener> modelListenerList = new List<IModelListener>();
        public abstract void Start();
        public abstract void Initialize();
        public virtual bool ModelExist(ModelDescription modelDescription)
        {
            return SystemManager.Instance().ModelManager.IsModelExist(modelDescription);
        }

        public virtual void ModelTeachDone(string camIndex = null)
        {
            foreach (IModelListener listener in modelListenerList)
                listener.ModelTeachDone();
        }

        public void AddModelListener(IModelListener modelListener)
        {
            modelListenerList.Add(modelListener);
        }

        public virtual bool SelectModel(string[] args)
        {
            if (SystemManager.Instance().LoadModel(args) == false)
                return false;

            foreach (IModelListener listener in modelListenerList)
                listener.ModelChanged();

            return true;
        }

        public virtual bool SelectModel(ModelDescription modelDescription)
        {
            if (SystemManager.Instance().LoadModel(modelDescription) == false)
                return false;

            foreach (IModelListener listener in modelListenerList)
                listener.ModelChanged();

            return true;
        }

        public virtual void DeleteModel(ModelDescription modelDescription)
        {
            Model model = (Model)SystemManager.Instance().CurrentModel;
            if (model != null && model.ModelDescription == modelDescription)
            {
                model.Release();
                model = null;
            }

            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;
            modelManager.DeleteModel(modelDescription);
        }

        public virtual bool NewModel(ModelDescription modelDescription)
        {
            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;

            if (modelManager.IsModelExist(modelDescription) == true)
                return false;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();

            model.Setup(1, 4, 1);
            model.LightParamSet.LightParamList[0].LightValue.Value[0] = 255;
            model.LightParamSet.LightParamList[0].LightValue.Value[1] = 255;
            model.LightParamSet.LightParamList[0].LightValue.Value[2] = 255;
            model.LightParamSet.LightParamList[0].LightValue.Value[3] = 30;

            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);

            return true;
        }

        public virtual bool ModelTrained(ModelDescription modelDescription)
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(modelDescription);
            
            return md.IsTrained;
        }

        public abstract void Release();
        public abstract void SendCommand(ExchangeCommand exchangeCommand, params string[] args);
    }
}
