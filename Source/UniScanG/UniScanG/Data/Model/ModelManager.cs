using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Gravure.Vision;

namespace UniScanG.Data.Model
{
    public class ModelManager : UniScanG.Common.Data.ModelManager
    {
        public ModelManager() : base()
        {
            //Init(modelPath);
        }

        public override void Init(string modelPath)
        {
            base.Init(modelPath);

            this.modelPath = modelPath;
            try
            {
                //this.Refresh();
            }
            catch (IOException ex)
            { }
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription() { Name = "Default", Paste = "Default", Thickness = 0 };
        }

        public override DynMvp.Data.Model CreateModel()
        {
            Model model = new Model();
            model.Modified = true;
            return model;
        }

        //public override bool IsModelExist(UniScanG.Common.Data.ModelDescription modelDescription)
        //{
        //    ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

        //    foreach (ModelDescription m in modelDescriptionList)
        //    {
        //        if (m.Name == modelDescriptionG.Name && m.Thickness == modelDescriptionG.Thickness && m.Paste == modelDescriptionG.Paste)
        //            return true;
        //    }

        //    return false;
        //}

        public override string GetModelPath(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            return Path.Combine(modelPath, modelDescription.Name, modelDescriptionG.Thickness.ToString(), modelDescriptionG.Paste);
        }
        
        public override void Refresh(string modelPath = null)
        {
            if (modelPath == null)
                modelPath = this.modelPath;

            bool exist = Directory.Exists(modelPath);
            DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                try
                {
                    Directory.CreateDirectory(modelPath);
                }
                catch (IOException ex)
                {
                    LogHelper.Error(LoggerType.Error, string.Format("ModelManager::Refresh - {0}", ex.Message));
                    DynMvp.ConsoleEx.WriteLine(ex.Message);
                }
                return;
            }

            lock (modelDescriptionList)
            {
                modelDescriptionList.Clear();

                foreach (DirectoryInfo nameDirectory in modelRootDir.GetDirectories())
                {
                    foreach (DirectoryInfo thicknessDir in nameDirectory.GetDirectories())
                    {
                        foreach (DirectoryInfo pasteDir in thicknessDir.GetDirectories())
                        {
                            ModelDescription modelDescription = (ModelDescription)LoadModelDescription(pasteDir.FullName);
                            if (modelDescription == null)
                                continue;

                            modelDescriptionList.Add(modelDescription);

                            if (String.IsNullOrEmpty(modelDescription.Category) == false)
                                CategoryList.Add(modelDescription.Category);
                        }
                    }
                }

                if(!modelDescriptionList.Exists(f=>f.IsDefaultModel))
                {
                    ModelDescription defaultModelDescription = (ModelDescription)CreateModelDescription();
                    modelDescriptionList.Add(defaultModelDescription);
                }
            }
        }

        public override void DeleteModel(UniScanG.Common.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            ModelDescription realMD = null;
            foreach (ModelDescription md in modelDescriptionList)
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

        protected override void LoadModel(DynMvp.Data.Model model, IReportProgress reportProgress)
        {
            base.LoadModel(model, reportProgress);
            Model modelG = (Model)model;

            string modelPath = GetModelPath(model.ModelDescription);
            string algorithmPoolFilePathOld = String.Format("{0}\\AlgorithmPool.xml", modelPath);
            if (File.Exists(algorithmPoolFilePathOld))
            // 예전 데이터 옮겨오기
            {
                AlgorithmPool.Instance().Load(algorithmPoolFilePathOld);
                Gravure.Vision.Trainer.TrainerParam trainerParam = (Gravure.Vision.Trainer.TrainerParam)AlgorithmPool.Instance().GetAlgorithm(Gravure.Vision.Trainer.TrainerBase.TypeName)?.Param;
                if (trainerParam != null && !trainerParam.IsEmpty)
                {
                    modelG.TrainerModelParam.CopyFrom(trainerParam.ModelParam);
                }

                Gravure.Vision.Calculator.CalculatorParam calculatorParam = (Gravure.Vision.Calculator.CalculatorParam)AlgorithmPool.Instance().GetAlgorithm(Gravure.Vision.Calculator.CalculatorBase.TypeName)?.Param;
                if (calculatorParam != null && !calculatorParam.IsEmpty)
                {
                    modelG.CalculatorModelParam.CopyFrom(calculatorParam.ModelParam);
                    //modelG.SheetSizePx = calculatorParam.SheetSize;

                    //modelG.EdgeParam = calculatorParam.EdgeParam;
                    //modelG.SensitiveParam = calculatorParam.SensitiveParam;
                    //if (calculatorParam?.PatternGroupList != null)
                    //    modelG.PatternGroupList = new System.Collections.Generic.List<Vision.SheetPatternGroup>(calculatorParam.PatternGroupList);

                    //if (calculatorParam?.RegionInfoList != null)
                    //    modelG.RegionInfoList = new System.Collections.Generic.List<Gravure.Data.RegionInfoG>(calculatorParam.RegionInfoList);
                }

                Gravure.Vision.Detector.DetectorParam detectorParam = (Gravure.Vision.Detector.DetectorParam)AlgorithmPool.Instance().GetAlgorithm(Gravure.Vision.Detector.Detector.TypeName)?.Param;
                if (detectorParam != null && !detectorParam.IsEmpty)
                    modelG.DetectorModelParam.CopyFrom(detectorParam.ModelParam);

                Gravure.Vision.Extender.WatcherParam watcherParam = (Gravure.Vision.Extender.WatcherParam)AlgorithmPool.Instance().GetAlgorithm(Gravure.Vision.Extender.Watcher.TypeName)?.Param;
                if (watcherParam != null && !watcherParam.IsEmpty)
                    modelG.WatcherModelParam.CopyFrom(watcherParam.ModelParam);

                AlgorithmPool.Instance().Clear();
                //File.Delete(algorithmPoolFilePathOld);
            }

            AlgorithmSetting.Instance().Load();

            AlgorithmPool.Instance().BuildAlgorithmPool();
            
        }

        public override DynMvp.Data.Model LoadModel(string[] args, IReportProgress reportProgress)
        {
            Refresh();

            ModelDescription md = (ModelDescription)CreateModelDescription();
            md.FromArgs(args);
            //md.Name = args[0];
            //md.Thickness = Convert.ToSingle(args[1]);
            //md.Paste = args[2];

            ModelDescription getMd = (ModelDescription)GetModelDescription(md);
            if (getMd == null)
                return null;

            return LoadModel(getMd, reportProgress);
        }

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            if (model.Modified == false)
                return true;

            model.Modified = false;
            model.ModelPath = GetModelPath((ModelDescription)model.ModelDescription);
            bool ok = base.SaveModel(model);
            Gravure.Vision.AlgorithmSetting.Instance().Save(); // 여기. 일반화 수정 필요!!
            return ok;
        }


    }
}
