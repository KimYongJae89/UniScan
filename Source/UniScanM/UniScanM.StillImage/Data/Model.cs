using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;
using UniScanM.Data;
using System.Diagnostics;
using System.IO;

namespace UniScanM.StillImage.Data
{
    public class Model: UniScanM.Data.Model
    {

        int sheetHeigthPx = 0;
        Dictionary<AxisPosition, TeachData> teachDataDic = null;
        float fovYPos = 0.4f;
        Size inspSize = Size.Empty;
        Feature feature = new Feature();

        public int SheetHeigthPx { get { return this.sheetHeigthPx; } set { this.sheetHeigthPx = value; } }

        public Model() : base()
        {
            this.inspectParam = new InspectParam();
        }

        public Dictionary<AxisPosition, TeachData> TeachDataDic
        {
            get { return teachDataDic; }
            set { teachDataDic = value; }
        }

        public Size InspSize
        {
            get { return inspSize; }
            set { inspSize = value; }
        }

        public Feature Feature
        {
            get { return feature; }
            set { feature = value; }
        }

        public float FovYPos
        {
            get { return fovYPos; }
            set { fovYPos = value; }
        }

        public string GetImageName(string extension = "Bmp")
        {
            return string.Format("Prev.{0}", extension.ToLower());
        }

        public override bool IsTaught()
        {
            // PLC에서 모델이 입력되지 않으면 항상 티칭함.
            if (this.Name == "Unknown")
                return false;

            return (teachDataDic != null && teachDataDic.Count > 0);
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);
            
            if (teachDataDic != null)
            {
                foreach (KeyValuePair<AxisPosition, TeachData> pair in teachDataDic)
                {
                    XmlElement subElement = xmlElement.OwnerDocument.CreateElement("TeachDataDic");
                    xmlElement.AppendChild(subElement);

                    StringBuilder sb = new StringBuilder();
                    foreach (float f in pair.Key.Position)
                        sb.AppendFormat("{0},", f);
                    XmlHelper.SetValue(subElement, "AxisPosition", sb.ToString().Trim(','));
                    if (pair.Value != null)
                    {
                        pair.Value.Export(subElement, "TeachData");
                        pair.Value.SaveImageData(this.GetImagePath(), pair.Key.ToString()); /////////////////////////////////////
                    }
                }
            }

            XmlHelper.SetValue(xmlElement, "FovYPos", fovYPos);
            XmlHelper.SetValue(xmlElement, "FovSize", inspSize);

            //inspectParam.Export(xmlElement, "InspectParam");
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);
            
            teachDataDic = new Dictionary<AxisPosition, TeachData>();
            XmlNodeList subElementList = xmlElement.GetElementsByTagName("TeachDataDic");
            foreach (XmlElement subElement in subElementList)
            {
                string axisPositionValue = XmlHelper.GetValue(subElement, "AxisPosition", "");
                List<string> token = axisPositionValue.Split(',').ToList();
                List<float> aa = token.ConvertAll<float>(f => float.Parse(f));
                AxisPosition axisPosition = new AxisPosition(aa.ToArray());

                TeachData teachData = TeachData.Load(subElement, "TeachData");
                teachDataDic.Add(axisPosition,teachData);
                if(teachData.IsInspectable ==true)
                    teachData.LoadImageData(this.GetImagePath(), axisPosition.ToString()); /////////////////////////////////////
            }

            fovYPos = XmlHelper.GetValue(xmlElement, "FovYPos", fovYPos);
            inspSize = XmlHelper.GetValue(xmlElement, "FovSize", Size.Empty);
            inspectParam.Import(xmlElement, "InspectParam");
        }



        public void UpdateFullImage()
        {
            List<TeachData> validTeachDeta = this.teachDataDic.Values.ToList();//.FindAll(f => f != null && f.TeachDone);
            if (validTeachDeta.Count > 0)
            {
                Image2D fullImage = null;
                DynMvp.UI.Touch.SimpleProgressForm form = new DynMvp.UI.Touch.SimpleProgressForm("Build Image");
                //form.Show(() =>
                //{
                List<ImageD> imageList = validTeachDeta.ConvertAll<ImageD>(f => f.ScaledImage);
                imageList.RemoveAll(f => f == null);

                Rectangle[] rectangle = new Rectangle[imageList.Count];
                Point point = Point.Empty;
                for (int i = 0; i < imageList.Count; i++)
                {
                    if (i > 0)
                        point.Offset(imageList[i - 1].Width, 0);

                    Size size = imageList[i].Size;
                    rectangle[i] = new Rectangle(point, size);
                }

                int fullWidth = rectangle.Sum(f => f.Width);
                int fullHeight = rectangle.Max(f => f.Height);
                fullImage = new Image2D(fullWidth, fullHeight, 1);

                Parallel.For(0, imageList.Count, i =>
                {
                    Image2D image2D = (Image2D)imageList[i];

                    Rectangle copyRect = rectangle[i];
                    Debug.Assert(Rectangle.Intersect(copyRect, new Rectangle(Point.Empty, fullImage.Size)) == copyRect);

                    fullImage.CopyFrom(image2D, new Rectangle(Point.Empty, image2D.Size), image2D.Pitch, copyRect.Location);
                });


                string modelImagePath = this.GetImagePath();
                string modelImageName = this.GetImageName("bmp");
                fullImage.SaveImage(Path.Combine(modelImagePath, modelImageName));
                fullImage.Dispose();
            }
        }
    }

    public class ModelDescription : UniScanM.Data.ModelDescription
    {
        public ModelDescription() : base()
        {
        }
        
        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            string imageString = XmlHelper.GetValue(modelDescElement, "Image", "");
            Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);
        }
        
        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();
            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
        }
    }
    
}
