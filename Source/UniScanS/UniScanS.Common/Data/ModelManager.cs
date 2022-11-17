using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Drawing;
using System.IO;
using UniEye.Base.Settings;

namespace UniScanS.Common.Data
{
    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        public ModelManager()
        {
            //this.Init(modelPath);
        }

        public Bitmap GetPreviewImage(ModelDescription modelDescription)
        {
            string imagePath = GetPreviewImagePath(modelDescription);

            if (File.Exists(imagePath) == false)
                return null;

            Bitmap image = (Bitmap)ImageHelper.LoadImage(imagePath);

            return image;
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public string GetPreviewImagePath(ModelDescription modelDescription)
        {
            return Path.Combine(GetModelPath(modelDescription), "Image", "Prev.bmp");
        }

        public virtual ModelDescription GetModelDescription(ModelDescription modelDescription)
        {
            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == modelDescription.Name)
                    return m;
            }

            return null;
        }

        public virtual string GetModelPath(ModelDescription modelDescription)
        {
            return String.Format("{0}\\{1}", modelPath, modelDescription.Name);
        }

        public virtual bool IsModelExist(ModelDescription modelDescription)
        {
            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == modelDescription.Name)
                    return true;
            }

            return false;
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
