using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.Base;
using DynMvp.UI;
using System.Xml;

namespace DynMvp.Vision
{
    public enum PatternType
    {
        Good, Ng
    }

    public abstract class Pattern : IDisposable
    {
        protected ImagingLibrary imagingLibrary;
        
        PatternType patternType;
        public PatternType PatternType
        {
            get { return patternType; }
            set { patternType = value; }
        }

        protected FigureGroup maskFigures = new FigureGroup();
        public FigureGroup MaskFigures
        {
            get { return maskFigures; }
            set { maskFigures = value; }
        }
        
        protected Image2D patternImage = null;
        public Image2D PatternImage
        {
            get { return patternImage; }
            set { patternImage = value; }
        }

        public virtual void Dispose()
        {
            if (patternImage != null)
                patternImage.Dispose();
        }

        ~Pattern()
        {
            patternImage?.Dispose();
        }

        public abstract Pattern Clone();

        public virtual void Copy(Pattern pattern)
        {
            patternType = pattern.patternType;
            maskFigures = (FigureGroup)pattern.maskFigures.Clone();

            Dispose();

            //Train(pattern.PatternImage, Patter);
        }

        public string GetPatternImageString()
        {
            if (patternImage == null)
                return "";

            Bitmap patternBitmap = patternImage.ToBitmap();
            string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
            patternBitmap.Dispose();

            return patternImageString;
        }

        public void SetPatternImageString(string patternImageString)
        {
            Bitmap patternBitmap = ImageHelper.Base64StringToBitmap(patternImageString);
            if (patternBitmap != null)
            {
                Dispose();

                Image2D patternImage = Image2D.FromBitmap(patternBitmap);
                patternBitmap.Dispose();
                this.patternImage = patternImage;
                //Train(patternImage, null);
                //patternImage.Dispose();
            }
        }

        public void Train(Image2D image, PatternMatchingParam patternMatchingParam)
        {
            //LogHelper.Debug(LoggerType.Grab, "Pattern::Train");
            AlgoImage algoImage = ImageBuilder.Build(this.imagingLibrary, image, ImageType.Grey);
            Train(algoImage, patternMatchingParam);
            algoImage.Dispose();
        }

        public void SaveParam(XmlElement patternElement)
        {
            XmlHelper.SetValue(patternElement, "Image", GetPatternImageString());
            XmlHelper.SetValue(patternElement, "PatternType", patternType.ToString());

            if (maskFigures.FigureExist)
            {
                XmlElement maskFiguresElement = patternElement.OwnerDocument.CreateElement("", "MaskFigures", "");
                patternElement.AppendChild(maskFiguresElement);
                maskFigures.Save(maskFiguresElement);
            }
        }

        public void LoadParam(XmlElement patternElement)
        {
            SetPatternImageString(XmlHelper.GetValue(patternElement, "Image", ""));

            patternType = (PatternType)Enum.Parse(typeof(PatternType), XmlHelper.GetValue(patternElement, "PatternType", "Good"));
            
            foreach (XmlElement maskFiguresElement in patternElement)
            {
                if (maskFiguresElement.Name == "MaskFigures")
                {
                    maskFigures.Load(maskFiguresElement);
                    UpdateMaskImage();
                }
            }
        }

        public abstract void Train(AlgoImage image, PatternMatchingParam patternMatchingParam);

        public virtual Image2D GetMaskedImage()
        {
            return patternImage;
        }

        public abstract void UpdateMaskImage();

        public abstract PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext);
    }
}
