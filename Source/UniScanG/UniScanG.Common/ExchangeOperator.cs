using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Common
{
    public delegate void ModelChangedDelegate();
    public interface IModelListener
    {
        void ModelRefreshed();
        void ModelChanged();
        void ModelTeachDone(int camId);
    }

    public interface IVisitListener
    {
        void PreparePanel(ExchangeCommand trayPanel);
        void Clear();
    }

    public interface IServerExchangeOperator
    {
        InspectorObj[] Inspectors { get; }

        InspectorObj GetInspector(int camId, int clientId);
        List<InspectorObj> GetInspectorList(int sheetNo = -1);
        Process OpenVnc(Process process, string ipAddress, IntPtr handle);
        void CloseVnc();
        bool ModelTrained(int camIndex, int clientIndex, ModelDescription modelDescription);
        void SyncModel(int camId);
        void SendCommand(ExchangeCommand exchangeCommand, params string[] args);
    }

    public interface IClientExchangeOperator
    {
        void AddVisitListener(IVisitListener visitListener);
        void SendInspectDone(string inspectionNo, string path, Judgment judgment);
        void SendAlive();
        int GetCamIndex();
        int GetClientIndex();
        bool IsConnected { get; }
    }

    public abstract class ExchangeOperator
    {
        List<IModelListener> modelListenerList = new List<IModelListener>();

        public abstract void Initialize();
        public abstract void Release();

        public abstract int GetCamIndex();
        public abstract int GetClientIndex();
        public abstract string GetRemoteIpAddress();

        public abstract bool IsConnected { get; }

        public virtual bool SaveModel() { return true; }

        public virtual bool ModelExist(ModelDescription modelDescription)
        {
            ModelManager modelManager = SystemManager.Instance().ModelManager as ModelManager;
            if (modelManager == null)
                return false;

            return modelManager.IsModelExist(modelDescription);
        }

        public virtual void ModelTeachDone(int camId)
        {
            foreach (IModelListener listener in modelListenerList)
                listener.ModelTeachDone(camId);
        }

        public void AddModelListener(IModelListener modelListener)
        {
            modelListenerList.Add(modelListener);
        }

        public virtual bool SelectModel(string[] args)
        {
            bool loadOk = false;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Model Open");
            simpleProgressForm.Show(() =>
            {
                loadOk = SystemManager.Instance().LoadModel(args);
            });

            if (loadOk)
            {
                foreach (IModelListener listener in modelListenerList)
                    listener.ModelChanged();
            }

            return loadOk;
        }

        public virtual void UpdateModelList()
        {
            SystemManager.Instance().ModelManager.Refresh();
            foreach (IModelListener listener in modelListenerList)
                listener.ModelRefreshed();
        }

        public virtual bool SelectModel(ModelDescription modelDescription)
        {
            bool loadOk = false;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString("Opening Model"));

            simpleProgressForm.Show(ConfigHelper.Instance().MainForm, () =>
            {
                loadOk = SystemManager.Instance().LoadModel(modelDescription);

                if (loadOk)
                {
                    bool reTry = false;
                    do
                    {
                        try
                        {
                            reTry = false;
                            foreach (IModelListener listener in modelListenerList)
                                listener.ModelChanged();
                        }
                        catch (InvalidOperationException)
                        {
                            reTry = true;
                        }
                    } while (reTry);
                }
            });
            return loadOk;
        }

        public virtual void CloseModel()
        {
            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;
            modelManager.CloseModel();

            foreach (IModelListener listener in modelListenerList)
                listener.ModelChanged();
        }

        public virtual void DeleteModel(ModelDescription modelDescription)
        {
            Model model = (Model)SystemManager.Instance().CurrentModel;
            if (model != null && model.ModelDescription == modelDescription)
            {
                model.Release();
                model = null;
            }

            try
            {
                ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;
                modelManager.DeleteModel(modelDescription);
            }
            catch (IOException ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ExchangeOperator::DeleteModel - {0}, {1}", modelDescription.Name, ex.Message));
            }
        }

        public virtual bool NewModel(ModelDescription modelDescription)
        {
            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;

            if (modelManager.IsModelExist(modelDescription) == true)
                return false;

            modelManager.AddModel(modelDescription);

            ModelDescription defaultModelDescription = (ModelDescription)modelManager.ModelDescriptionList.Find(f => f.IsDefaultModel);
            if (defaultModelDescription == null)
            {
                Model model = (Model)modelManager.CreateModel();
                model.Modified = true;

                model.ModelDescription = modelDescription;
                modelManager.SaveModel(model);
            }
            else
            {
                DynMvp.Data.Model model = modelManager.LoadModel(defaultModelDescription, null);
                model.ModelDescription = modelDescription;
                model.Modified = true;
                modelManager.SaveModel(model);
            }
            return true;
        }

        public virtual bool ModelTrained(ModelDescription modelDescription)
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(modelDescription.GetArgs()) as ModelDescription;
            if (md == null)
                return false;

            return md.IsTrained;
        }

        public abstract void SendCommand(ExchangeCommand exchangeCommand, params string[] args);
    }
}
