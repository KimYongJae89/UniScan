using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using Euresys.Open_eVision_1_2;

using DynMvp.Base;
using DynMvp.Vision.Euresys;
using DynMvp.Vision.OpenCv;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections;

namespace DynMvp.Vision
{
    // 뻘짓
    public class DebuugImageSaver
    {
        struct DebugImageSaverStruct
        {
            AlgoImage algoImage;
            string name;
            DebugContext debugContext;

            public DebugImageSaverStruct(AlgoImage algoImage, string name, DebugContext debugContext)
            {
                this.algoImage = algoImage;
                this.name = name;
                this.debugContext = debugContext;
            }

            public void Save()
            {
                algoImage.Save(name, debugContext);
            }
        }
        List<DebugImageSaverStruct> list = new List<DebugImageSaverStruct>();

        public void Add(AlgoImage algoImage, string name, DebugContext debugContext)
        {
            list.Add(new DebugImageSaverStruct(algoImage, name, debugContext));
        }

        public void Save()
        {
            list.ForEach(f => f.Save());
            list.Clear();
        }

    }
    public class DebugContext
    {
        private bool saveDebugImage;
        public bool SaveDebugImage
        {
            get { return saveDebugImage; }
            set { saveDebugImage = value; }
        }

        //public string Path => this.path;
        protected string path;

        public virtual string FullPath => this.path;

        public DebugContext()
        {
            saveDebugImage = false;
            path = "";
        }

        public DebugContext(DebugContext debugContext)
        {
            this.saveDebugImage = debugContext.saveDebugImage;
            this.path = debugContext.path;
        }

        public DebugContext(DebugContext debugContext, string subPath)
        {
            this.saveDebugImage = debugContext.saveDebugImage;
            this.path = Path.Combine(debugContext.path, subPath);
        }

        public DebugContext(bool saveDebugImage, string path)
        {
            this.saveDebugImage = saveDebugImage;
            this.path = path;
        }
    }

    public class DebugHelper
    {
        public static void SaveText(string text, string fileName, DebugContext debugContext)
        {
            if (debugContext.SaveDebugImage == false)
                return;

            Directory.CreateDirectory(debugContext.FullPath);

            String filePath = String.Format("{0}\\{1}", debugContext.FullPath, fileName);

            File.WriteAllText(filePath, text);
        }

        public static void SaveImage(ImageD image, string fileName, DebugContext debugContext)
        {
            if (debugContext.SaveDebugImage == false)
                return;

            Directory.CreateDirectory(debugContext.FullPath);

            String filePath = String.Format("{0}\\{1}", debugContext.FullPath, fileName);
            if (image is Image2D)
            {
                if (((Image2D)image).IsUseIntPtr() != true)
                    image.SaveImage(filePath, ImageFormat.Bmp);
            }
            else
            {
                image.SaveImage(filePath, ImageFormat.Bmp);
            }
        }

        public static void SaveImage(AlgoImage image, string fileName, DebugContext debugContext, bool forceSave = false)
        {
            if (forceSave == false && debugContext.SaveDebugImage == false)
                return;

            Directory.CreateDirectory(debugContext.FullPath);

            image.Save(fileName, debugContext);
        }

        public static void ExportArray(string fileName, float[] values)
        {
            string fullPath = Path.Combine(Configuration.TempFolder, fileName);

            FileStream fs = new FileStream(fullPath, FileMode.Create);
            if (fs != null)
            {
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                foreach (float value in values)
                    sw.WriteLine(value.ToString());

                sw.Close();
                fs.Close();
            }
        }
    }
}
