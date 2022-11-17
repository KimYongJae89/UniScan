using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UniEye.Base.Data;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;

namespace UniScanS.Common.Data
{
    public enum CommState
    {
        CONNECTED, DISCONNECTED
    }
    
    public class InspectorObj
    {
        AlgorithmPool algorithmPool;
        public AlgorithmPool AlgorithmPool
        {
            get { return algorithmPool; }
        }

        private ModelManager modelManager;
        public ModelManager ModelManager
        {
            get { return modelManager; }
        }

        private InspectorInfo info;
        public InspectorInfo Info
        {
            get { return info; }
        }

        CommState commState = CommState.DISCONNECTED;
        public CommState CommState
        {
            get { return commState; }
            set { commState = value; }
        }

        InspectState inspectState = InspectState.Done;
        public InspectState InspectState
        {
            get { return inspectState; }
            set { inspectState = value; }
        }

        OpState opState = OpState.Idle;
        public OpState OpState
        {
            get { return opState; }
            set { opState = value; }
        }
        
        Production curProduction;
        public Production CurProduction
        {
            get { return curProduction; }
        }

        public InspectorObj(InspectorInfo inspectorInfo)
        {
            this.info = inspectorInfo;
            
            commState = CommState.DISCONNECTED;
            algorithmPool = new AlgorithmPool();
            modelManager = SystemManager.Instance().CreateModelManager();
            curProduction = null;
            
            Initialize();
        }

        public void LoadModel(ModelDescription modelDescription)
        {
            string modelPath = modelManager.GetModelPath(modelDescription);
            string algorithmPoolPath = Path.Combine(modelPath, "AlgorithmPool.xml");

            if (File.Exists(algorithmPoolPath) == false)
                return;

            algorithmPool.Load(algorithmPoolPath);
            //modelPath.
            //algorithmPool.Load();
        }

        public void Initialize()
        {
            algorithmPool.Initialize(SystemManager.Instance().AlgorithmArchiver);
            modelManager.Init(Path.Combine(this.info.Path, "Model"));
        }

        public bool Exist(ModelDescription modelDescription)
        {
            return modelManager.IsModelExist(modelDescription);
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            modelManager.DeleteModel(modelDescription);
        }

        public void NewModel(ModelDescription modelDescription)
        {
            if (modelManager.IsModelExist(modelDescription) == true)
                return;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();

            AlgorithmPool.Instance().BuildAlgorithmPool();

            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);
            modelManager.SaveModelDescription(model.ModelDescription);
        }

        public void Refesh()
        {
            modelManager.Refresh();
        }

        public bool IsTrained(ModelDescription modelDescription)
        {
            modelManager.Refresh();
            ModelDescription getModelDescription = modelManager.GetModelDescription(modelDescription);

            if (getModelDescription == null)
            {
                ModelDescription clone = (ModelDescription)modelDescription.Clone();
                
                NewModel(clone);
                
                getModelDescription = modelManager.GetModelDescription(clone);
            }

            return getModelDescription.IsTrained;
        }

        public Bitmap GetPreviewImage(ModelDescription modelDescription)
        {
            string imagePath = modelManager.GetPreviewImagePath(modelDescription);

            Bitmap image = null;

            if (File.Exists(imagePath) == true)
                image = (Bitmap)ImageHelper.LoadImage(imagePath);
            
            return image;
        }
    }
}
