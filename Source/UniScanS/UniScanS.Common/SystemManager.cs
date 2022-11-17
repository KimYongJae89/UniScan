using DynMvp.Base;
using DynMvp.Vision;
using System.Drawing;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniEye.Base.UI;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.UI;
using UniScanS.Common.UI;

namespace UniScanS.Common
{
    public class SystemManager : UniEye.Base.SystemManager
    {
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

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public virtual ModelManager CreateModelManager()
        {
            return new ModelManager();
        }

        public virtual UniEye.Base.Data.ProductionManager CreateProductionManager(string defaultPath)
        {
            return new UniEye.Base.Data.ProductionManager(defaultPath);
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
        }

        public bool LoadModel(string[] args)
        {
            try
            {
                currentModel = ModelManager.LoadModel(args, progressForm);
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

        public bool LoadModel(ModelDescription modelDescription)
        {
            try
            {
                currentModel = modelManager.LoadModel(modelDescription, progressForm);
                if (currentModel == null)
                    return false;

                if (SystemManager.Instance().ExchangeOperator is IClientExchangeOperator)
                    return true;

                if (currentModel.LightParamSet.NumLight != MachineSettings.Instance().NumLight)
                {
                    currentModel.Setup(MachineSettings.Instance().NumCamera, MachineSettings.Instance().NumLight, MachineSettings.Instance().NumLightType);

                    currentModel.LightParamSet.LightParamList[0].LightValue.Value[0] = 255;
                    currentModel.LightParamSet.LightParamList[0].LightValue.Value[1] = 255;
                    currentModel.LightParamSet.LightParamList[0].LightValue.Value[2] = 255;
                    currentModel.LightParamSet.LightParamList[0].LightValue.Value[3] = 30;
                }
                else
                {
                    if (currentModel.LightParamSet.LightParamList[0].LightValue.Value[0] == 0
                        && currentModel.LightParamSet.LightParamList[0].LightValue.Value[1] == 0
                        && currentModel.LightParamSet.LightParamList[0].LightValue.Value[2] == 0
                        && currentModel.LightParamSet.LightParamList[0].LightValue.Value[3] == 0)
                    {
                        currentModel.LightParamSet.LightParamList[0].LightValue.Value[0] = 255;
                        currentModel.LightParamSet.LightParamList[0].LightValue.Value[1] = 255;
                        currentModel.LightParamSet.LightParamList[0].LightValue.Value[2] = 255;
                        currentModel.LightParamSet.LightParamList[0].LightValue.Value[3] = 30;
                    }
                }

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

        public override void Release()
        {
            exchangeOperator.Release();

            base.Release();
        }
    }
}
