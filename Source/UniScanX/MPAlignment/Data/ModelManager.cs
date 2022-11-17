using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DynMvp.UI;
using DynMvp.Data;
using DynMvp.Vision;
using DynMvp.Base;
using System.Windows.Forms;
using System.Diagnostics;


namespace UniScanX.MPAlignment.Data
{
    class ModelManager : UniEye.Base.Data.ModelManager
    {
        public event EventHandler ModelLoaded;
 //       public event EventHandler ModelListChanged;

        public override DynMvp.Data.Model CreateModel()
        {
            var model =  new UniScanX.MPAlignment.Data.MPModel();
            return model;
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }
        
        public string ResultPath { get; set; }
      //  public string CurModelPath { get; set; }

        public MPModel CurrentModel { get; set; }
        public MPModel CapturedModel { get; set; }

        public void Initailize( string resultPath )
        {
            ResultPath = resultPath;
        }

        protected override void LoadModel(Model model, IReportProgress reportProgress)
        {
            string modelPath = GetModelPath(model.ModelDescription);

            string algorithmPoolFilePath = String.Format("{0}\\AlgorithmPool.xml", modelPath);
            if (File.Exists(algorithmPoolFilePath))
                AlgorithmPool.Instance().Load(algorithmPoolFilePath);
            else
                AlgorithmPool.Instance().BuildAlgorithmPool();

            string modelFilePath = String.Format("{0}\\Model.xml", modelPath);
            model.ModelPath = modelPath;
            if (File.Exists(modelFilePath))
            {
                ModelReader modelReader = ModelReaderBuilder.Create(modelFilePath);

                // MachineSettings machineSettings = MachineSettings.Instance();
                // modelReader.Initialize(SystemManager.Instance().AlgorithmArchiver, 1, machineSettings.NumCamera, machineSettings.NumLight, machineSettings.NumLightType);

                int numInspectionStep = 1;
                int numCamera = 1;
                int numLight = 1;
                int numLightType = 1;
                modelReader.Initialize(SystemManager.Instance().AlgorithmArchiver, numInspectionStep, numCamera, numLight, numLightType);
                modelReader.Load(model, modelFilePath, reportProgress);
            }

            CurrentModel = (MPModel)model;
            ModelLoaded?.Invoke(model, EventArgs.Empty);
        }

        public void CloseCurrentModel()
        {
            if (CurrentModel == null)
            {
                LogHelper.Debug(LoggerType.Error, "ModelManager current model is null.");
                return;
            }
            CurrentModel.Clear();
            CurrentModel = null;
            ModelLoaded?.Invoke(this, EventArgs.Empty);
        }

        public Model GetModel(ModelDescription modeldesc)
        {
            MPModel model = CreateModel() as MPModel;
            model.ModelDescription = modeldesc;
            model.ModelPath = modeldesc.ModelPath;
            model.ResultPath = modeldesc.ResultPath;

            LoadModel(model, null);

            if (model.IsMasterModel == false)
            {
                model.LoadModelSchema();
  //              model.LoadProduction();
            }
            return model;
        }

        public void AddModel(ModelDescription modelDescription)
        {
            modelDescriptionList.Add(modelDescription);
            SaveModelDescription(modelDescription);
 //           ModelListChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DeleteModel(string modelName)
        {
            ModelDescription md = GetModelDescription(modelName);
            if (CurrentModel != null)
            {
                if (CurrentModel.Name == md.Name)
                {
                    MessageBox.Show("this is current model. can't delete this model close please.");
                    return;
                }
            }

            modelDescriptionList.Remove(md);
            string modelPath = GetModelPath(md);
            Directory.Delete(modelPath, true);

 //           ModelListChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool IsModelExist(string name)
        {
            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == name)
                    return true;
            }

            return false;
        }

        public void GetMasterModelDescriptionList(List<ModelDescription> masterModelDescriptionList)
        {
            foreach (ModelDescription modelDescription in modelDescriptionList)
            {
                if (modelDescription.UseByMasterModel)
                {
                    masterModelDescriptionList.Add(modelDescription);
                }
            }
        }

        public ModelDescription GetModelDescription(string name)
        {
            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == name)
                {
                    return m;
                }
            }
            return null;
        }

        bool IsModelFolder(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            var files = dirInfo.GetFiles();
            var xmlfile = files.Select(x => x.Extension == "xml").ToList();
            if (xmlfile.Count > 0)
            {
                return true;
            }
            else
                return false;
            //return File.Exists(Path.Combine(path, "*.xml")); //뭐든지 xml 만 있으면 모델파일로 상정한다.
        }

        public void Refresh()
        {
            if (Directory.Exists(modelPath) == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            UpdateList(modelPath);
        }

        public void UpdateList(string modelPath)
        {
            modelDescriptionList.Clear();
            string[] dirNames = Directory.GetDirectories(modelPath);

            foreach (string subModelPath in dirNames)
            {
                string modelName = Path.GetFileName(subModelPath);

                if (IsModelFolder(subModelPath))
                {
                    var modelDescription = LoadModelDescription(modelName);
                    modelDescriptionList.Add(modelDescription);
                }
                else
                {

                }
            }
        }

        //public void MoveDown(string dirName)
        //{
        //    string modelPath = Path.Combine(CurModelPath, dirName);
        //    UpdateList(modelPath);
        //}

        //public void MoveUp()
        //{
        //    string modelPath = Path.GetDirectoryName(CurModelPath);

        //    UpdateList(modelPath);
        //}

        public string PreviewImageName
        {
            get { return "Preview.bmp"; }
        }

        private string GetResultPath(string modelName)
        {
           // if (String.IsNullOrEmpty(ResultPath))
                return Path.Combine(ResultPath, modelName, "\\Result");

//            return GetModelPath(ResultPath).Replace(BaseModelPath, ResultPath);
        }

        new public ModelDescription LoadModelDescription(string modelName)
        {
            string modelPath = Path.Combine(this.modelPath, modelName);
            string filePath = String.Format("{0}\\ModelDescription.xml", modelPath);

            ModelDescription modelDesc = new ModelDescription();
            modelDesc.Load(filePath);
            modelDesc.ModelPath = modelPath;
            modelDesc.ResultPath = GetResultPath(modelName);
            modelDesc.Name = modelName;

            return modelDesc;
        }

        public void SaveModelDescription(ModelDescription modelDesc)
        {
            string modelPath = Path.Combine(this.modelPath, modelDesc.Name);
            modelDesc.ModelPath = modelPath;
            if (Directory.Exists(modelPath) == false)
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = String.Format("{0}\\ModelDescription.xml", modelPath);
            modelDesc.Save(filePath);
        }
    }
}
