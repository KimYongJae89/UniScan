using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using DynMvp.Base;
using DynMvp.Data;

namespace UniScan.Data
{
    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        //public override void Refresh(string modelPath = null)
        //{
        //    if (modelPath == null)
        //        modelPath = this.modelPath;

        //    DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
        //    if (modelRootDir.Exists == false)
        //    {
        //        Directory.CreateDirectory(modelPath);
        //        return;
        //    }

        //    this.modelPath = modelPath;

        //    modelDescriptionList.Clear();

        //    FileInfo[] fileList = modelRootDir.GetFiles();

        //    foreach (FileInfo fileInfo in fileList)
        //    {
        //        String modelName = Path.GetFileNameWithoutExtension(fileInfo.Name);
        //        ModelDescription modelDescription = (ModelDescription)LoadModelDescription(modelName);
        //        modelDescription.Name = modelName;
        //        modelDescriptionList.Add(modelDescription);

        //        if (String.IsNullOrEmpty(modelDescription.Category) == false)
        //            CategoryList.Add(modelDescription.Category);
        //    }
        //}

        //public override DynMvp.Data.ModelDescription LoadModelDescription(string modelName)
        //{
        //    ModelDescription modelDescription = (ModelDescription)base.LoadModelDescription(modelName);

        //    return modelDescription;
        //}

        //public override void SaveModelDescription(DynMvp.Data.ModelDescription modelDesc)
        //{
        //    string fileName = String.Format("{0}.dat", GetModelPath(modelDesc.Name));
        //    modelDesc.Save(fileName);
        //}

        public override DynMvp.Data.Model CreateModel()
        {
            Model newModel = new Model();
            return newModel;
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }
    }
}
