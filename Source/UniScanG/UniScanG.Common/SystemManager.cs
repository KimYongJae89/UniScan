using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniEye.Base.UI;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;
using UniScanG.Common.UI;

namespace UniScanG.Common
{
    public abstract class SystemManager : UniEye.Base.SystemManager
    {
        public ModellerPageExtender ModellerPageExtender { get => this.modellerPageExtender; }
        protected ModellerPageExtender modellerPageExtender;

        ExchangeOperator exchangeOperator;
        public ExchangeOperator ExchangeOperator
        {
            get { return exchangeOperator; }
            set { exchangeOperator = value; }
        }

        UiController uiController;
        public UiController UiController
        {
            get { return uiController; }
            set { uiController = value; }
        }

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public virtual ModelManager CreateModelManager()
        {
            return new ModelManager();
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
        }

        public override void Release()
        {
            base.Release();
            this.exchangeOperator?.Release();
        }

        public void InitalizeModellerPageExtender()
        {
            this.modellerPageExtender = this.uiChanger.CreateModellerPageExtender();
        }

        public bool SaveModel(bool force = false)
        {
            if (this.currentModel == null)
                return false;

            if (force)
                this.currentModel.Modified = true;

            return this.modelManager.SaveModel(this.currentModel);
        }

        public bool LoadModel(string[] args)
        {
            try
            {
                ModelManager modelManager = this.modelManager as ModelManager;
                currentModel = modelManager.LoadModel(args, progressForm);
                if (currentModel == null)
                    return false;

                if (deviceController != null)
                    deviceController.OnModelLoaded(currentModel);
            }
            catch (InvalidModelNameException)
            {
                currentModel = null;
                return false;
            }

            return true;
        }

        //public bool LoadModel(ModelDescription modelDescription)
        //{
        //    try
        //    {
        //        currentModel = modelManager.LoadModel(modelDescription, progressForm);
        //        if (currentModel == null)
        //            return false;

        //        if (deviceController != null)
        //            deviceController.OnModelLoaded(currentModel);
        //    }
        //    catch (InvalidModelNameException)
        //    {
        //        currentModel = null;
        //        return false;
        //    }

        //    return true;
        //}

        public override void InitializeResultManager()
        {
            float minimumFreeSpace = OperationSettings.Instance().MinimumFreeSpace;
            float resultCopyDays = OperationSettings.Instance().ResultCopyDays;
            int resultStoringDays = OperationSettings.Instance().ResultStoringDays;

            if (resultCopyDays >= 0)
                //dataManagerList.Add(new DynMvp.Data.DataCopier(DynMvp.Data.DataStoringType.Seq, PathSettings.Instance().Result, OperationSettings.Instance().ResultCopyDays, "yy-MM-dd", 10f));
                dataManagerList.Add(new DynMvp.Data.DataCopier(SystemManager.Instance().productionManager, resultCopyDays, minimumFreeSpace));

            if (resultStoringDays >= 0)
            {
                DirectoryInfo log = new DirectoryInfo(PathSettings.Instance().Log);
                dataManagerList.Add(new DataRemoverG(resultStoringDays, minimumFreeSpace, SystemManager.Instance().productionManager, log));

                //if (OperationSettings.Instance().ResultCopyDays >= 0)
                //{
                //    // 복사 안정화 이전 데이터 호환 위함 - 한달 후 삭제해되 됨.
                //    List<DriveInfo> driveInfoList = DynMvp.Data.DataCopier.GetTargetDriveInfoList();
                //    DirectoryInfo root = new DirectoryInfo(PathSettings.Instance().Result).Root;
                //    foreach (DriveInfo driveInfo in driveInfoList)
                //    {
                //        string newPath = PathSettings.Instance().Result.Replace(root.Name, driveInfo.Name);
                //        dataManagerList.Add(new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Seq, newPath, OperationSettings.Instance().ResultStoringDays, "yy-MM-dd", true));
                //    }
                //}
            }
        }
    }
}
