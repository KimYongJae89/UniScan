using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Data
{
    public abstract class ModelManager : IEnumerable<ModelDescription>
    {
        protected string modelPath;

        private HashSet<string> categoryList = new HashSet<string>();
        public HashSet<string> CategoryList
        {
            get { return categoryList; }
        }
        protected List<ModelDescription> modelDescriptionList = new List<ModelDescription>();
        public List<ModelDescription> ModelDescriptionList { get => modelDescriptionList; }

        public ModelManager()
        {
        }

        public IEnumerator<ModelDescription> GetEnumerator()
        {
            return modelDescriptionList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddModel(ModelDescription modelDescription)
        {
            modelDescriptionList.Add(modelDescription);
            SaveModelDescription(modelDescription);
        }

        public void EditModel(ModelDescription modelDescription)
        {
            SaveModelDescription(modelDescription);
        }

        public virtual void DeleteModel(ModelDescription md)
        {
            modelDescriptionList.Remove(md);

            string modelPath = GetModelPath(md);

            if (Directory.Exists(modelPath))
            {
                try
                {
                    Directory.Delete(modelPath, true);
                }
                catch (IOException exception)
                {
                    Directory.Delete(modelPath, true);
                }
            }
        }

        public void CopyModelData(ModelDescription srcModelName, ModelDescription destModelName)
        {
            string srcModelPath = GetModelPath(srcModelName);
            string destModelPath = GetModelPath(destModelName);

            Directory.CreateDirectory(destModelPath);

            string srcModelFileName = srcModelPath + "\\model.xml";
            if (File.Exists(srcModelFileName) == true)
                File.Copy(srcModelFileName, destModelPath + "\\model.xml");

            string srcModelSchemaFileName = srcModelPath + "\\modelSchema.xml";
            if (File.Exists(srcModelSchemaFileName) == true)
                File.Copy(srcModelSchemaFileName, destModelPath + "\\modelSchema.xml");

            string srcImagePathName = srcModelPath + "\\Image";
            if (Directory.Exists(srcImagePathName) == true)
            {
                string destImagePath = destModelPath + "\\Image";
                Directory.CreateDirectory(destImagePath);
                FileHelper.CopyDirectory(srcImagePathName, destImagePath, true, true);
            }
        }

        internal bool IsModelExist(string name)
        {
            foreach (ModelDescription md in modelDescriptionList)
            {
                if (md.Name == name)
                    return true;
            }

            return false;
        }

        public ModelDescription GetModelDescription(params string[] args)
        {
            List<string> list = new List<string>(args);

            return modelDescriptionList.Find(md => list.SequenceEqual(md.GetArgs()));
        }

        public virtual void Refresh(string modelPath = null)
        {
            if (modelPath == null)
                modelPath = this.modelPath;

            DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            this.modelPath = modelPath;

            lock (modelDescriptionList)
            {
                modelDescriptionList.Clear();

                DirectoryInfo[] dirList = modelRootDir.GetDirectories();

                foreach (DirectoryInfo modelDir in dirList)
                {
                    ModelDescription modelDescription = LoadModelDescription(modelDir.FullName);
                    if (modelDescription == null)
                        continue;

                    modelDescription.Name = modelDir.Name;
                    modelDescriptionList.Add(modelDescription);

                    if (String.IsNullOrEmpty(modelDescription.Category) == false)
                        categoryList.Add(modelDescription.Category);
                }
            }
        }

        public virtual string GetModelPath(ModelDescription modelDescription)
        {
            return String.Format("{0}\\{1}", modelPath, modelDescription.Name);
        }

        public ModelDescription LoadModelDescription(string path)
        {
            string filePath = String.Format("{0}\\ModelDescription.xml", path);
            if (File.Exists(filePath) == false)
                return null;

            ModelDescription modelDesc = CreateModelDescription();
            modelDesc.Load(filePath);

            return modelDesc;
        }

        public void SaveModelDescription(ModelDescription modelDesc)
        {
            string modelPath = GetModelPath(modelDesc);
            if (Directory.Exists(modelPath) == false)
            {
                Directory.CreateDirectory(modelPath);
            }

            string filePath = String.Format("{0}\\ModelDescription.xml", modelPath);
            modelDesc.Save(filePath);

            //이미지 폴더가 없을 경우 새로 생성 한다.
            string imageFolderPath = string.Format("{0}\\Image", modelPath);
            if (Directory.Exists(imageFolderPath) == false)
                Directory.CreateDirectory(imageFolderPath);
        }

        public virtual Model CreateModel()
        {
            return new Model();
        }

        public virtual ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public virtual bool IsCompatible(Model model)
        {
            return true;
        }

        public virtual bool IsCompatible(ModelDescription modelDescription)
        {
            return true;
        }

        public Model LoadModel(ModelDescription modelDesc, IReportProgress reportProgress)
        {
            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Model.Information, ErrorLevel.Info,
                "Loading Model: [{0}]", new object[] { modelDesc.Name }, ""));

            if (!this.modelDescriptionList.Contains(modelDesc))
                throw new InvalidModelNameException();

            Model model = CreateModel();
            model.ModelDescription = modelDesc;
            LoadModel(model, reportProgress);

            model.LoadModelSchema();
            model.Modified = false;

            return model;
        }

        protected abstract void LoadModel(Model model, IReportProgress reportProgress);
        public abstract bool SaveModel(Model model);
    }
}
