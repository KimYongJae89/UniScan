using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Vision;

namespace UniScanG.Gravure.Vision.RCI
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SensitiveOption
    {
        public bool Multi => (Low > 0 && Low < High);
        public byte Low { get; set; } = 25;
        public byte High { get; set; } = 30;

        public SensitiveOption() { }

        public void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);

                SaveParam(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Low", this.Low);
            XmlHelper.SetValue(xmlElement, "High", this.High);
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                LoadParam(subElement, "");
                return;
            }

            this.Low = XmlHelper.GetValue(xmlElement, "Low", this.Low);
            this.High = XmlHelper.GetValue(xmlElement, "High", this.High);
        }

        public void CopyFrom(SensitiveOption sensitiveOption)
        {
            this.Low = sensitiveOption.Low;
            this.High = sensitiveOption.High;
        }

        public SensitiveOption Clone()
        {
            SensitiveOption clone = new SensitiveOption();
            clone.CopyFrom(this);
            return clone;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StickerOption
    {

        public bool Use { get; set; } = true;
        public bool BrightOnly { get; set; } = true;
        public byte High { get; set; } = 50;
        public byte Low { get; set; } = 30;

        public StickerOption() { }

        public void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);

                SaveParam(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Use", this.Use);
            XmlHelper.SetValue(xmlElement, "BrightOnly", this.BrightOnly);
            XmlHelper.SetValue(xmlElement, "High", this.High);
            XmlHelper.SetValue(xmlElement, "Low", this.Low);
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                LoadParam(subElement, "");
                return;
            }

            this.Use = XmlHelper.GetValue(xmlElement, "Use", this.Use);
            this.BrightOnly = XmlHelper.GetValue(xmlElement, "BrightOnly", this.BrightOnly);
            this.High = XmlHelper.GetValue(xmlElement, "High", this.High);
            this.Low = XmlHelper.GetValue(xmlElement, "Low", this.Low);
        }

        public void CopyFrom(StickerOption stickerOption)
        {
            this.Use = stickerOption.Use;
            this.BrightOnly = stickerOption.BrightOnly;
            this.High = stickerOption.High;
            this.Low = stickerOption.Low;
        }

        public StickerOption Clone()
        {
            StickerOption clone = new StickerOption();
            clone.CopyFrom(this);
            return clone;
        }
    }


    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RCIReconOptions
    {
        public enum EReconstruct { None, Blob, Remove }

        [Category("Train"), DisplayName("Reconstruct")]
        public EReconstruct Reconstruct { get; set; } = EReconstruct.Remove;

        [Category("Train"), DisplayName("Edge Smooth Count")]
        public int EdgeSmoothCount { get; set; } = 2;

        [Category("Train"), DisplayName("Edge Weigth Value")]
        public int EdgeValue { get; set; } = 100;

        public void CopyFrom(RCIReconOptions rciReconOptions)
        {
            this.Reconstruct = rciReconOptions.Reconstruct;
            this.EdgeSmoothCount = rciReconOptions.EdgeSmoothCount;
            this.EdgeValue = rciReconOptions.EdgeValue;
        }

        internal void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);

                SaveParam(subElement, "");
                return;
            }

            XmlHelper.SetValue(xmlElement, "Reconstruct", this.Reconstruct);
            XmlHelper.SetValue(xmlElement, "EdgeSmoothCount", this.EdgeSmoothCount);
            XmlHelper.SetValue(xmlElement, "EdgeValue", this.EdgeValue);
        }

        internal void LoadParam(XmlElement xmlElement, string key)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                LoadParam(subElement, "");
                return;
            }

            this.Reconstruct = XmlHelper.GetValue(xmlElement, "Reconstruct", this.Reconstruct);
            this.EdgeSmoothCount = XmlHelper.GetValue(xmlElement, "EdgeSmoothCount", this.EdgeSmoothCount);
            this.EdgeValue = XmlHelper.GetValue(xmlElement, "EdgeValue", this.EdgeValue);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RCIGlobalOptions
    {
        [Category("Train"), DisplayName("Right To Left")]
        public bool RightToLeft { get; set; } = false;

        [Category("Inspect"), DisplayName("Uniformize Grey Value")]
        public byte UniformizeGv { get; set; } = 180;


        [Category("Inspect"), DisplayName("Parall")]
        public bool Parall { get; set; } = true;


        [Category("Inspect"), DisplayName("Sticker")]
        public StickerOption StickerOption { get; set; } = new StickerOption();


        public void SaveParam(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                SaveParam(subElement, null);
                return;
            }

            XmlHelper.SetValue(xmlElement, "RightToLeft", this.RightToLeft);
            XmlHelper.SetValue(xmlElement, "UniformizeGv", this.UniformizeGv);
            XmlHelper.SetValue(xmlElement, "Parall", this.Parall);

            StickerOption.SaveParam(xmlElement, "StickerOption");
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            CopyFrom(Load(xmlElement, key));
        }

        public static RCIGlobalOptions Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
                return Load(xmlElement[key], null);

            RCIGlobalOptions globalOptions = new RCIGlobalOptions();
            if (xmlElement == null)
                return globalOptions;

            globalOptions.RightToLeft = XmlHelper.GetValue(xmlElement, "RightToLeft", globalOptions.RightToLeft);
            globalOptions.UniformizeGv = XmlHelper.GetValue(xmlElement, "UniformizeGv", globalOptions.UniformizeGv);
            globalOptions.Parall = XmlHelper.GetValue(xmlElement, "Parall", globalOptions.Parall);

            globalOptions.StickerOption.LoadParam(xmlElement, "StickerOption");

            return globalOptions;
        }

        public void CopyFrom(RCIGlobalOptions globalOptions)
        {
            this.RightToLeft = globalOptions.RightToLeft;
            this.UniformizeGv = globalOptions.UniformizeGv;
            this.Parall = globalOptions.Parall;
            this.StickerOption.CopyFrom(globalOptions.StickerOption);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RCIStandaloneOptions
    {

        [Category("Inspect"), DisplayName("Sensitivity Size")]
        public SizeF SensitivitySz { get; set; } = new SizeF(100, 100);

        [Category("Inspect"), DisplayName("Pixel Size")]
        public SizeF PelSize { get; set; } = new SizeF(14, 14);

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, null);
                return;
            }

            XmlHelper.SetValue(xmlElement, "SensitivitySz", this.SensitivitySz);
            XmlHelper.SetValue(xmlElement, "PelSize", this.PelSize);
        }

        public static RCIStandaloneOptions Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
                return Load(xmlElement[key], null);

            RCIStandaloneOptions options = new RCIStandaloneOptions();
            if (xmlElement == null)
                return options;

            options.SensitivitySz = XmlHelper.GetValue(xmlElement, "SensitivitySz", options.SensitivitySz);
            options.PelSize = XmlHelper.GetValue(xmlElement, "PelSize", options.PelSize);
            
            return options;
        }


        public void CopyFrom(RCIStandaloneOptions options)
        {
            this.SensitivitySz = options.SensitivitySz;
            this.PelSize = options.PelSize;
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RCIOptions: INotifyPropertyChanged
    {
        // PatternFind
        [Category("PatternFind"), DisplayName("Pattern Gap Length [mm]")]
        [TypeConverter(typeof(Px2MmConverter))]
        public float PatternGapLengthMm { get; set; } = 35;

        // Train
        [Category("Train"), DisplayName("TM Inflate")]
        public SizeF PTM_InflateUm { get; set; } = new SizeF(140, 280);

        //[Category("Train"), DisplayName("TM Inflate")]
        //public float PTM_InflateWidthUm { get; set; } = 280f; // 20px @ 14um/px
        //[Category("Train"), DisplayName("TM Inflate")]
        //public float PTM_InflateHeigthUm { get; set; } = 280f; // 20px @ 14um/px

        [Category("Train"), DisplayName("Reconstruct")]
        public RCIReconOptions ReconstructOptions { get; set; } = new RCIReconOptions();

        [Category("Train"), DisplayName("Skip Head Row")]
        public bool SkipHeadRow { get; set; } = false;

        [Category("Train"), DisplayName("Skip Tail Row")]
        public bool SkipTailRow { get; set; } = false;

        // Inspect
        [Category("Inspect"), DisplayName("ROI Finding Seed")]
        public Rectangle ROISeedRect { get; set; } = new Rectangle(0, 0, -1, -1);

        [Category("Inspect"), DisplayName("Sensitivity GV")]
        public SensitiveOption SensitiveOption { get; set; } = new SensitiveOption();


        [Category("Inspect"), DisplayName("TM Correction Count")]
        public int PTMCorrectionCount { get; set; } = 0;

        [Category("Inspect"), DisplayName("SplitX")]
        public bool SplitX { get; set; } = true;


        [Category("Debug"), DisplayName("Build Debug Image")]
        public bool BuildDebugImage { get; set; } = false;

        [Category("Debug"), DisplayName("Show TextFigure")]
        public bool ShowTextFigure { get; set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement, null);
                return;
            }
            
            XmlHelper.SetValue(xmlElement, "PatternGapLengthMm", this.PatternGapLengthMm);
            XmlHelper.SetValue(xmlElement, "PTM_InflateUm", this.PTM_InflateUm);

            XmlHelper.SetValue(xmlElement, "SkipHeadRow", this.SkipHeadRow);
            XmlHelper.SetValue(xmlElement, "SkipTailRow", this.SkipTailRow);
            XmlHelper.SetValue(xmlElement, "ROISeedRect", this.ROISeedRect);
            XmlHelper.SetValue(xmlElement, "PTMCorrectionCount", this.PTMCorrectionCount);
            XmlHelper.SetValue(xmlElement, "ShowTextFigure", this.ShowTextFigure);

            XmlHelper.SetValue(xmlElement, "SplitX", this.SplitX);
            XmlHelper.SetValue(xmlElement, "BuildDebugImage", this.BuildDebugImage);

            this.ReconstructOptions.SaveParam(xmlElement, "ReconstructOptions");
            this.SensitiveOption.SaveParam(xmlElement, "SensitiveOption");
        }

        public static RCIOptions Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
                return Load(xmlElement[key], null);

            RCIOptions options = new RCIOptions();
            if (xmlElement == null)
                return options;

            options.PatternGapLengthMm = XmlHelper.GetValue(xmlElement, "PatternGapLengthMm", options.PatternGapLengthMm);
            options.PTM_InflateUm = XmlHelper.GetValue(xmlElement, "PTM_InflateUm", options.PTM_InflateUm);
            
            options.SkipHeadRow = XmlHelper.GetValue(xmlElement, "SkipHeadRow", options.SkipHeadRow);
            options.SkipTailRow = XmlHelper.GetValue(xmlElement, "SkipTailRow", options.SkipTailRow);
            options.ROISeedRect = XmlHelper.GetValue(xmlElement, "ROISeedRect", options.ROISeedRect);
            options.PTMCorrectionCount = XmlHelper.GetValue(xmlElement, "PTMCorrectionCount", options.PTMCorrectionCount);
            options.ShowTextFigure = XmlHelper.GetValue(xmlElement, "ShowTextFigure", options.ShowTextFigure);

            options.SplitX = XmlHelper.GetValue(xmlElement, "SplitX", options.SplitX);
            options.BuildDebugImage = XmlHelper.GetValue(xmlElement, "BuildDebugImage", options.BuildDebugImage);

            options.ReconstructOptions.LoadParam(xmlElement, "ReconstructOptions");
            options.SensitiveOption.LoadParam(xmlElement, "SensitiveOption");

            return options;
        }


        public void CopyFrom(RCIOptions options)
        {
            this.PatternGapLengthMm = options.PatternGapLengthMm;
            this.PTM_InflateUm = options.PTM_InflateUm;

            this.SkipHeadRow = options.SkipHeadRow;
            this.SkipTailRow= options.SkipTailRow;
            this.ROISeedRect = options.ROISeedRect;
            this.PTMCorrectionCount = options.PTMCorrectionCount;
            this.ShowTextFigure = options.ShowTextFigure;

            this.SplitX = options.SplitX;
            this.BuildDebugImage = options.BuildDebugImage;

            this.ReconstructOptions.CopyFrom(options.ReconstructOptions);
            this.SensitiveOption.CopyFrom(options.SensitiveOption);
        }

    }
}
