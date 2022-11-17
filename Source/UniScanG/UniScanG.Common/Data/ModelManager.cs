using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Drawing;
using System.IO;
using UniEye.Base.Settings;

namespace UniScanG.Common.Data
{
    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        public ModelManager() : base()
        {
            //this.Init(modelPath);
        }

        public Bitmap GetPreviewImage(ModelDescription modelDescription, string postfix)
        {
            string imagePathJpg = GetPreviewImagePath(modelDescription, postfix);

            if (File.Exists(imagePathJpg))
                return (Bitmap)ImageHelper.LoadImage(imagePathJpg);

            string imagePathBmp = imagePathJpg.Replace(".jpg", ".bmp");
            if (File.Exists(imagePathBmp))
                return (Bitmap)ImageHelper.LoadImage(imagePathBmp);

            return null;
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public string GetPreviewImagePath(ModelDescription modelDescription, string postfix)
        {
            string fileName = string.Format("Prev{0}.jpg", postfix.Trim());
            return Path.Combine(GetModelPath(modelDescription), "Image", fileName);
        }

        public ModelDescription GetModelDescription(ModelDescription modelDescription)
        {
            foreach (ModelDescription m in modelDescriptionList)
            {
                if (modelDescription.Equals(m))
                    return m;
            }

            return null;
        }

        public virtual bool IsModelExist(ModelDescription modelDescription)
        {
            return modelDescriptionList.Contains(modelDescription);
            //foreach (ModelDescription m in modelDescriptionList)
            //{
            //    if (m.Name == modelDescription.Name)
            //        return true;
            //}

            //return false;
        }

        public virtual void DeleteModel(ModelDescription modelDescription)
        {
            modelDescriptionList.Remove(modelDescription);
            
            string modelPath = GetModelPath(modelDescription);

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

        public virtual DynMvp.Data.Model LoadModel(string[] args, IReportProgress reportProgress)
        {
            ModelDescription md = (ModelDescription)CreateModelDescription();
            md.Name = args[0];

            return LoadModel(md, reportProgress);
        }
    }
}
