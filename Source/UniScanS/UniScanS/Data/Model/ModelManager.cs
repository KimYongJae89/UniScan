using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.IO;
using UniEye.Base.Settings;
using UniScanS.Common;

namespace UniScanS.Data.Model
{
    public class ModelManager : UniScanS.Common.Data.ModelManager
    {
        public ModelManager() : base()
        {
            //Init(modelPath);
        }

        public override void Init(string modelPath)
        {
            base.Init(modelPath);

            this.modelPath = modelPath;
            this.Refresh();
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescriptionS();
        }

        public override DynMvp.Data.Model CreateModel()
        {
            return new Model();
        }

        public override bool IsModelExist(UniScanS.Common.Data.ModelDescription modelDescription)
        {
            ModelDescriptionS modelDescriptionG = (ModelDescriptionS)modelDescription;

            foreach (ModelDescriptionS m in modelDescriptionList)
            {
                if (m.Name == modelDescriptionG.Name && m.Thickness == modelDescriptionG.Thickness && m.Paste == modelDescriptionG.Paste)
                    return true;
            }

            return false;
        }

        public override string GetModelPath(UniScanS.Common.Data.ModelDescription modelDescription)
        {
            ModelDescriptionS modelDescriptionG = (ModelDescriptionS)modelDescription;

            return Path.Combine(modelPath, modelDescription.Name, modelDescriptionG.Thickness.ToString(), modelDescriptionG.Paste);
        }
        
        public override void Refresh(string modelPath = null)
        {
            if (modelPath == null)
                modelPath = this.modelPath;

            DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            modelDescriptionList.Clear();

            foreach (DirectoryInfo nameDirectory in modelRootDir.GetDirectories())
            {
                foreach (DirectoryInfo thicknessDir in nameDirectory.GetDirectories())
                {
                    foreach (DirectoryInfo pasteDir in thicknessDir.GetDirectories())
                    {
                        ModelDescriptionS modelDescription = (ModelDescriptionS)LoadModelDescription(pasteDir.FullName);
                        if (modelDescription == null)
                            continue;
                        
                        modelDescriptionList.Add(modelDescription);

                        if (String.IsNullOrEmpty(modelDescription.Category) == false)
                            CategoryList.Add(modelDescription.Category);
                    }
                }
            }
        }
        
        protected override void LoadModel(DynMvp.Data.Model srcModel, IReportProgress reportProgress)
        {
            Model model = (Model)srcModel;

            string modelPath = GetModelPath(model.ModelDescription);
            string algorithmPoolFilePath = String.Format("{0}\\AlgorithmPool.xml", modelPath);
            if (File.Exists(algorithmPoolFilePath))
                AlgorithmPool.Instance().Load(algorithmPoolFilePath);

            string modelFilePath = String.Format("{0}\\Model.xml", modelPath);

            if (File.Exists(modelFilePath))
            {
                ModelReader modelReader = ModelReaderBuilder.Create(modelFilePath);

                MachineSettings machineSettings = MachineSettings.Instance();
                modelReader.Initialize(SystemManager.Instance().AlgorithmArchiver, 1, machineSettings.NumCamera, machineSettings.NumLight, machineSettings.NumLightType);
                modelReader.Load(model, modelFilePath, reportProgress);
            }

            model.ModelPath = modelPath;
        }

        //public override DynMvp.Data.ModelDescription LoadModelDescription(string modelPath)
        //{
        //    string filePath = String.Format("{0}\\ModelDescription.xml", modelPath);
        //    if (File.Exists(filePath) == false)
        //        return null;

        //    ModelDescriptionG modelDesc = (ModelDescriptionG)CreateModelDescription();
        //    modelDesc.Load(filePath);

        //    return modelDesc;
        //}

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            model.ModelPath = GetModelPath((ModelDescriptionS)model.ModelDescription);
            return base.SaveModel(model);
        }

        //public override void SaveModelDescription(DynMvp.Data.ModelDescription srcModelDesc)
        //{
        //    ModelDescriptionG modelDescription = (ModelDescriptionG)srcModelDesc;

        //    string modelPath = GetModelPath(modelDescription);

        //    if (Directory.Exists(modelPath) == false)
        //        Directory.CreateDirectory(modelPath);

        //    string filePath = String.Format("{0}\\ModelDescription.xml", modelPath);
        //    modelDescription.Save(filePath);
        //    //이미지 폴더가 없을 경우 새로 생성 한다.
        //    string imageFolderPath = string.Format("{0}\\Image", modelPath);

        //    if (Directory.Exists(imageFolderPath) == false)
        //        Directory.CreateDirectory(imageFolderPath);
        //}

        public override void DeleteModel(UniScanS.Common.Data.ModelDescription modelDescription)
        {
            ModelDescriptionS modelDescriptionG = (ModelDescriptionS)modelDescription;

            ModelDescriptionS realMD = null;
            foreach (ModelDescriptionS md in modelDescriptionList)
            {
                if (md.Name == modelDescriptionG.Name && md.Thickness == modelDescriptionG.Thickness && md.Paste == modelDescriptionG.Paste)
                    realMD = md;
            }

            if (realMD == null)
                return;

            modelDescriptionList.Remove(realMD);

            string firstPath = String.Format("{0}\\{1}", modelPath, realMD.Name);
            string middlePath = String.Format("{0}\\{1}", firstPath, realMD.Thickness);
            string lastPath = String.Format("{0}\\{1}", middlePath, realMD.Paste);

            if (Directory.Exists(lastPath) == true)
            {
                Directory.Delete(lastPath, true);

                DirectoryInfo middleInfo = new DirectoryInfo(middlePath);
                if (middleInfo.GetFiles().Length + middleInfo.GetDirectories().Length == 0)
                    Directory.Delete(middlePath, true);

                DirectoryInfo firstInfo = new DirectoryInfo(firstPath);
                if (firstInfo.GetFiles().Length + firstInfo.GetDirectories().Length == 0)
                    Directory.Delete(firstPath, true);
            }
            
            Refresh();
        }

        public override DynMvp.Data.Model LoadModel(string[] args, IReportProgress reportProgress)
        {
            Refresh();

            ModelDescriptionS md = (ModelDescriptionS)CreateModelDescription();
            md.Name = args[0];
            md.Thickness = Convert.ToSingle(args[1]);
            md.Paste = args[2];

            ModelDescriptionS getMd = (ModelDescriptionS)GetModelDescription(md);

            return LoadModel(getMd, reportProgress);
        }

        public override UniScanS.Common.Data.ModelDescription GetModelDescription(UniScanS.Common.Data.ModelDescription modelDescription)
        {
            ModelDescriptionS modelDescriptionG = (ModelDescriptionS)modelDescription;
            
            foreach (ModelDescriptionS md in modelDescriptionList)
            {
                if (md.Name == modelDescriptionG.Name && md.Thickness == modelDescriptionG.Thickness && md.Paste == modelDescriptionG.Paste)
                    return md;
            }

            return null;
        }
    }
}
