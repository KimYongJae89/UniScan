using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Xml;

namespace UniScanM.StillImage.Data
{
    public class TeachData : UniScanM.Data.IExportable, UniScanM.Data.IImportable
    {
        int binValue = -1;
        bool inspectable;
        Size inspSize = Size.Empty;

        ImageD scaledImage = null;
        ImageD clippedImage = null;

        private AlgoImage majorElectrodeImage = null;

        public AlgoImage MajorElectrodeImage
        {
            get { return majorElectrodeImage; }
            set { majorElectrodeImage = value; }
        }
        private AlgoImage majorElectrodeEdgeImage = null;

        public AlgoImage MajorElectrodeEdgeImage
        {
            get { return majorElectrodeEdgeImage; }
            set { majorElectrodeEdgeImage = value; }
        }

        List<PatternInfoGroup> patternInfoGroupList = null;
        //ShapeOfInterest shapeOfInterest = null;

        /// <summary>
        /// 이진화 값
        /// </summary>
        public int BinValue
        {
            get { return binValue; }
        }

        /// <summary>
        /// 타 티칭정보와 비교결과 검사 대상인 경우 true
        /// </summary>
        public bool IsInspectable
        {
            get { return inspectable; }
            set { inspectable = value; }
        }

        public bool TeachDone
        {
            get { return binValue >= 0; }
        }
        

        /// <summary>
        /// 모델러 창에서 사용할 이미지
        /// </summary>
        public ImageD ClippedImage
        {
            get { return clippedImage; }
            set { clippedImage = value; }
        }


        /// <summary>
        /// 티칭에 사용된 시트 이미지
        /// </summary>
        public ImageD ScaledImage
        {
            get { return scaledImage; }
        }

        /// <summary>
        /// 영상 내 패턴 목록
        /// </summary>
        public List<PatternInfoGroup> PatternInfoGroupList
        {
            get { return patternInfoGroupList; }
        }

        /// <summary>
        /// Sheet 내에서 검사할 영역 크기
        /// </summary>
        public Size InspSize
        {
            get { return this.inspSize; }
            set { this.inspSize = value; }
        }
        /// <summary>
        /// 관심패턴
        /// </summary>
        //public ShapeOfInterest ShapeOfInterest
        //{
        //    get { return shapeOfInterest; }
        //    set { shapeOfInterest = value; }
        //}

        public override string ToString()
        {
            //return string.Format("TH {0}, {1} Item(s)",this.binValue, objectList.Count);
            return base.ToString();
        }

        public TeachData() { }
        public TeachData(int binValue, ImageD image)
        {
            this.binValue = binValue;
            this.inspectable = false;
            this.scaledImage = image;
            this.patternInfoGroupList = new List<PatternInfoGroup>();
        }

        public void SaveDebug(string path)
        {
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, scaledImage, ImageType.Grey);
            algoImage.Save(Path.Combine(path, "TeachResult_Image.bmp"));

            AlgoImage drawImage = algoImage.Clone();
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            foreach (PatternInfoGroup group in this.patternInfoGroupList)
            {
                AlgoImage algoImage2 = algoImage.Clone();
                group.ShapeInfoList.ForEach(f =>
                {
                    Rectangle drawRect = f.BaseRect;
                    imageProcessing.DrawRect(algoImage2, drawRect, 192, false);
                    imageProcessing.DrawRect(drawImage, f.BaseRect, 192, false);

                    drawRect.Inflate((int)group.TeachInfo.Feature.Margin.Width, (int)group.TeachInfo.Feature.Margin.Height);
                    imageProcessing.DrawRect(algoImage2, drawRect, 128, false);
                });
                algoImage2.Save(Path.Combine(path, string.Format("TeachResult_Rect_{0}.bmp", group.TeachInfo.Id)));
                algoImage2.Dispose();

                File.WriteAllText(Path.Combine(path, string.Format("TeachResult_Rect_{0}.txt", group.TeachInfo.Id)), group.debug);
            }

            drawImage.Save(Path.Combine(path, "TeachResult_Rect.bmp"));

            drawImage.Dispose();
            algoImage.Dispose();
        }

        public void SaveImageData(string path, string info)
        {
            if(majorElectrodeImage != null)
            {
                majorElectrodeImage.Save(Path.Combine(path, "Ref_" + info + ".bmp"));
            }

            if (majorElectrodeEdgeImage != null)
            {
                majorElectrodeEdgeImage.Save(Path.Combine(path, "Edge_" + info + ".bmp"));
            }
        }

        public void LoadImageData(string path, string info)
        {
            //if (majorElectrodeImage != null)
            {
               string filepath = Path.Combine(path, "Ref_" + info + ".bmp");

                //var image = ImageHelper.LoadImage(filepath);
                if (File.Exists(filepath))
                {
                    ImageD imageD = new Image2D();
                    imageD.LoadImage(filepath);
                    majorElectrodeImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                }
            }

            //if (majorElectrodeEdgeImage != null)
            {
                string filepath = Path.Combine(path, "Edge_" + info + ".bmp");
                if (File.Exists(filepath))
                {
                    ImageD imageD = new Image2D();
                    imageD.LoadImage(filepath);
                    majorElectrodeEdgeImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                }
            }
        }
        public void Export(XmlElement element, string subKey = null)
        {
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "BinValue", binValue.ToString());
            XmlHelper.SetValue(element, "Inspectable", inspectable.ToString());
            XmlHelper.SetValue(element, "InspSize", inspSize);

            // MajorElectrodeImage
            // majorElectrodeEdgeImage
            //if (scaledImage != null)
            //{
            //    string scaledImageStr = ImageHelper.BitmapToBase64String(scaledImage.ToBitmap());
            //    XmlHelper.SetValue(element, "Scaledimage", scaledImageStr);
            //}

            //if (clippedImage != null)
            //{
            //    string clippedImageStr = ImageHelper.BitmapToBase64String(clippedImage.ToBitmap());
            //    XmlHelper.SetValue(element, "ClippedImage", clippedImageStr);
            //}

            if (patternInfoGroupList != null)
            {
                foreach (PatternInfoGroup patternInfoGroup in patternInfoGroupList)
                    patternInfoGroup.Export(element, "PatternInfoGroup");
            }

            //shapeOfInterest?.Export(element, "ShapeOfInterest");
        }

        public void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            binValue = Convert.ToInt32(XmlHelper.GetValue(element, "BinValue", binValue.ToString()));
            inspectable = Convert.ToBoolean(XmlHelper.GetValue(element, "Inspectable", inspectable.ToString()));
            inspSize = XmlHelper.GetValue(element, "InspSize", Size.Empty);

            //string scaledimageStr = XmlHelper.GetValue(element, "Scaledimage", "");
            //if (string.IsNullOrEmpty(scaledimageStr) == false)
            //    scaledImage = Image2D.ToImage2D(ImageHelper.Base64StringToBitmap(scaledimageStr));

            //string clippedImageStr = XmlHelper.GetValue(element, "ClippedImage", "");
            //if (string.IsNullOrEmpty(clippedImageStr) == false)
            //    clippedImage = Image2D.ToImage2D(ImageHelper.Base64StringToBitmap(clippedImageStr));

            patternInfoGroupList = new List<PatternInfoGroup>();
            XmlNodeList xmlNodeList = element.GetElementsByTagName("PatternInfoGroup");
            foreach (XmlElement patternInfoListElement in xmlNodeList)
            {
                PatternInfoGroup patternInfoGroup = PatternInfoGroup.Load(patternInfoListElement);
                this.patternInfoGroupList.Add(patternInfoGroup);
            }

            //shapeOfInterest = ShapeOfInterest.Load(element, "ShapeOfInterest");
        }

        public static TeachData Load(XmlElement xmlElement, string key = null)
        {
            TeachData teachData = new TeachData();
            teachData.Import(xmlElement, key);
            return teachData;
        }
    }
}
