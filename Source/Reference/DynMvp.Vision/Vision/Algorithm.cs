using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;

using DynMvp.Vision.Planbss;
using System.Drawing.Imaging;
using System.Threading;

namespace DynMvp.Vision
{
    public interface Searchable
    {
        Size GetSearchRangeSize();
        void SetSearchRangeSize(Size size);
    }

    public abstract class ResultValueItem
    {
        public abstract string GetValueString();
    }

    public class AlgorithmInspectParam
    {
        /// <summary>
        /// 검색 영역을 포함하여 Clip된 이미지. ROI가 회전 되었을 경우, 0도로 회전된 이미지가 Clip된다.
        /// </summary>
        ImageD clipImage;
        public ImageD ClipImage
        {
            get { return clipImage; }
            set { clipImage = value; }
        }

        ImageD clipImage2;
        public ImageD ClipImage2
        {
            get { return clipImage2; }
            set { clipImage2 = value; }
        }

        Size wholeImageSize;
        public Size WholeImageSize
        {
            get { return wholeImageSize; }
            set { wholeImageSize = value; }
        }

        RotatedRect probeRegionInFov;
        public RotatedRect ProbeRegionInFov
        {
            get { return probeRegionInFov; }
            set { probeRegionInFov = value; }
        }

        RotatedRect clipRegionInFov;
        public RotatedRect ClipRegionInFov
        {
            get { return clipRegionInFov; }
            set { clipRegionInFov = value; }
        }

        protected DebugContext debugContext;
        public DebugContext DebugContext
        {
            get { return debugContext; }
            set { debugContext = value; }
        }

        protected Calibration cameraCalibration;
        public Calibration CameraCalibration
        {
            get { return cameraCalibration; }
            set { cameraCalibration = value; }
        }

        SizeF offset;
        public SizeF Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        float pixelRes3D;
        public float PixelRes3D
        {
            get { return pixelRes3D; }
            set { pixelRes3D = value; }
        }

        bool teachMode;
        public bool TeachMode
        {
            get { return teachMode; }
            set { teachMode = value; }
        }
        
        CancellationToken cancellationToken;
        public CancellationToken CancellationToken
        {
            get { return cancellationToken; }
            set { cancellationToken = value; }
        }

        public AlgorithmInspectParam(ImageD clipImage)
        {
            this.clipImage = clipImage;
        }

        public AlgorithmInspectParam(AlgorithmInspectParam param)
        {
            if (param != null)
            {
                this.clipImage = param.clipImage;
                this.probeRegionInFov = param.probeRegionInFov;
                this.clipRegionInFov = param.clipRegionInFov;
                this.wholeImageSize = param.wholeImageSize;
                this.cameraCalibration = param.cameraCalibration;
                this.debugContext = param.debugContext;
            }
        }

        public AlgorithmInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
        {
            this.clipImage = clipImage;
            this.probeRegionInFov = probeRegionInFov;
            this.clipRegionInFov = clipRegionInFov;
            this.wholeImageSize = wholeImageSize;
            this.cameraCalibration = calibration;
            this.debugContext = debugContext;
        }

        public RectangleF GetProbeRegionInClipImage()
        {
            return new RectangleF(probeRegionInFov.X - clipRegionInFov.X, probeRegionInFov.Y - clipRegionInFov.Y, probeRegionInFov.Width, probeRegionInFov.Height);
        }

        public virtual void Dispose()
        {
            if (this.clipImage != null)
                this.clipImage.Dispose();
            this.clipImage = null;
        }
    }

    public class AlgorithmConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(Algorithm))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is Algorithm)
            {
                Algorithm algorithm = (Algorithm)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class AlgorithmParamConverter : ExpandableObjectConverter
    {
        public AlgorithmParamConverter():base()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(AlgorithmParam))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is AlgorithmParam)
            {
                AlgorithmParam algorithmParam = (AlgorithmParam)value;
                return algorithmParam;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(AlgorithmParamConverter))]
    public abstract class AlgorithmParam
    {
        [Browsable(false)]
        public DateTime ModifiedDateTime { get => modifiedDateTime; }
        protected DateTime modifiedDateTime = DateTime.MinValue;

        [Browsable(false)]
        public ImageType SourceImageType { get => this.sourceImageType; set => this.sourceImageType = value; }
        private ImageType sourceImageType;

        [Browsable(false)]
        public ImageBandType ImageBand { get => this.imageBand; set => this.imageBand = value; }
        private ImageBandType imageBand = ImageBandType.Luminance;

        public abstract AlgorithmParam Clone();
        public abstract void Dispose();

        public virtual void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            sourceImageType = srcAlgorithmParam.sourceImageType;
            imageBand = srcAlgorithmParam.imageBand;
        }

        public virtual void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public void LoadParam(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement[key];
            if (subElement == null)
                return;

            LoadParam(subElement);
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            sourceImageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(algorithmElement, "SourceImageType", "Grey"));
            imageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), XmlHelper.GetValue(algorithmElement, "ImageBand", "Luminance"));
            modifiedDateTime = XmlHelper.GetValue(algorithmElement, "ModifiedDateTime", "yyyyMMdd.HHmmss.fff", modifiedDateTime);
        }

        public void SaveParam(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
            xmlElement.AppendChild(subElement);

            SaveParam(subElement);
        }

        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "SourceImageType", sourceImageType.ToString());
            XmlHelper.SetValue(algorithmElement, "ImageBand", imageBand.ToString());
            XmlHelper.SetValue(algorithmElement, "ModifiedDateTime", DateTime.Now.ToString("yyyyMMdd.HHmmss.fff"));
        }

        public virtual void Clear()
        {
            sourceImageType = ImageType.Grey;
            imageBand = ImageBandType.Luminance;
        }
    }

    [TypeConverterAttribute(typeof(AlgorithmConverter))]
    public abstract class Algorithm : IDisposable
    {
        private string algorithmName = "";
        public string AlgorithmName
        {
            get { return algorithmName; }
            set { algorithmName = value; }
        }

        private bool isAlgorithmPoolItem;
        [BrowsableAttribute(false)]
        public bool IsAlgorithmPoolItem
        {
            get { return isAlgorithmPoolItem; }
            set { isAlgorithmPoolItem = value; }
        }

        // Algorithm Pool Item일 경우, 이 알고리즘을 공유하는 
        [BrowsableAttribute(false)]
        public List<Algorithm> SubAlgorithmList => subAlgorithmList;
        List<Algorithm> subAlgorithmList = new List<Algorithm>();


        protected bool isColorAlgorithm = false;
        [BrowsableAttribute(false)]
        public bool IsColorAlgorithm
        {
            get { return isColorAlgorithm; }
        }

        protected AlgorithmParam param = null;
        public AlgorithmParam Param { get => this.param; set => this.param = value; }

        [BrowsableAttribute(false)]
        public List<IFilter> FilterList { get => this.filterList; }
        List<IFilter> filterList = new List<IFilter>();

        public bool Enabled { get => this.enabled; set => this.enabled = value; }
        bool enabled = true;
        
        public abstract Algorithm Clone();

        public virtual void Clear()
        {
            param.Clear();
        }

        public virtual void SyncParam(Algorithm srcAlgorithm)
        {
            isColorAlgorithm = srcAlgorithm.isColorAlgorithm;
            param.SyncParam(srcAlgorithm.Param);

            if (srcAlgorithm.FilterList?.Count > 0)
            {
                this.filterList = srcAlgorithm.FilterList;
            }

            enabled = srcAlgorithm.enabled;
        }

        public virtual void CopyFrom(Algorithm algorithm)
        {
            isColorAlgorithm = algorithm.isColorAlgorithm;
            param.CopyFrom(algorithm.Param);

            filterList.Clear();

            foreach (IFilter filter in algorithm.FilterList)
                filterList.Add(filter.Clone());

            enabled = algorithm.enabled;
        }

        protected void CopyGraphics(AlgorithmResult algorithmResult, HelpDraw helpDraw, int offsetX, int offsetY)
        {
            long countRect = helpDraw.SizeRect();
            for (int i = 0; i < countRect; i++)
            {
                DrawRect drRect = helpDraw.GetRect(i);

                Rectangle Area = drRect.mrArea;
                Rectangle rect = new Rectangle(offsetX + Area.Left, offsetY + Area.Top, Area.Width, Area.Height);
                if (drRect.mbCircle) algorithmResult.ResultFigureGroup.AddFigure(new EllipseFigure(rect, drRect.gsColor));
                else algorithmResult.ResultFigureGroup.AddFigure(new RectangleFigure(rect, drRect.gsColor));
            }

            long countCross = helpDraw.SizeCross();
            for (int i = 0; i < countCross; i++)
            {
                DrawCross drCross = helpDraw.GetCross(i);

                Point pt = new Point(drCross.mpt.X + offsetX, drCross.mpt.Y + offsetY);
                CrossFigure crossFigure = new CrossFigure(pt, drCross.mnLength, drCross.gsColor);
                algorithmResult.ResultFigureGroup.AddFigure(crossFigure);
            }

            long countLine = helpDraw.SizeLine();
            for (int i = 0; i < countLine; i++)
            {
                DrawLine drLine = helpDraw.GetLine(i);

                Point pt1 = new Point(drLine.mpt1.X + offsetX, drLine.mpt1.Y + offsetY);
                Point pt2 = new Point(drLine.mpt2.X + offsetX, drLine.mpt2.Y + offsetY);
                LineFigure lineFigure = new LineFigure(pt1, pt2, drLine.gsColor);
                algorithmResult.ResultFigureGroup.AddFigure(lineFigure);
            }
        }

        public virtual string[] GetPreviewNames()
        {
            return new string[] { "Default", "Whole Area" };
        }

        public virtual ImageD Filter(ImageD image, int previewFilterType)
        {
            if (image is Image3D)
                return image;

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey, param.ImageBand);
            switch (previewFilterType)
            {
                case 0:
                    Filter(algoImage);
                    break;
            }

            Image2D fillterredImage = (Image2D)algoImage.ToImageD();
            return fillterredImage;
        }

        public virtual ImageD PreFilterClipImage(ImageD filteredImage, Rectangle clipRegion, int previewFilterType)
        {
            ImageD clipImage = filteredImage.ClipImage(clipRegion);

            return clipImage;
        }

        public virtual ImageD PostFilterCopyForm(ImageD filteredImage, ImageD clipFilteredImage, Rectangle clipRegion, int previewFilterType)
        {
            filteredImage.CopyFrom(clipFilteredImage, new Rectangle(0, 0, clipRegion.Width, clipRegion.Height), clipFilteredImage.Pitch, new Point(clipRegion.X, clipRegion.Y));

            return filteredImage;
        }

        public void Filter(AlgoImage algoImage)
        {
            //algoImage.Save(@"d:\preFilter.bmp", null);
            if (filterList.Count == 0)
                return;

            AlgoImage srcAlgoImage = algoImage.Clone();
            foreach (IFilter filter in filterList)
            {
                if (filter.NeedMasterImage())
                {
                    filter.SetMasterImage(srcAlgoImage, true);
                }
                filter.Filter(algoImage);
                //algoImage.Save(@"d:\filter.bmp", null);
            }
        }

        public virtual void PrepareInspection()
        {

        }

        public virtual void ClearInspection()
        {

        }

        public virtual bool CanProcess3dImage()
        {
            return false;
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            this.algorithmName = XmlHelper.GetValue(algorithmElement, "AlgorithmName", this.algorithmName);

            param.LoadParam(algorithmElement);
        }
        
        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "AlgorithmName", algorithmName);
            XmlHelper.SetValue(algorithmElement, "AlgorithmType", GetAlgorithmType().ToString());
            param.SaveParam(algorithmElement);
        }

        public virtual AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
        {
            return new AlgorithmInspectParam(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext);
        }

        public virtual AlgorithmResult CreateAlgorithmResult()
        {
            return new AlgorithmResult("NoName");
        }

        public virtual void BuildMessage(AlgorithmResult algorithmResult)
        {
            MessageBuilder resultMessage = algorithmResult.MessageBuilder;

            resultMessage.AddTextLine(String.Format("---- {0} ----", GetAlgorithmType()));

            resultMessage.BeginTable(null, "Item", "Value", "Good Range");

            resultMessage.AddTableRow((algorithmResult.Good ? Color.Transparent : Color.LightPink),
                            "Result", (algorithmResult.Good ? "Good" : "NG"));

            foreach (AlgorithmResultValue resultValue in algorithmResult.ResultValueList)
            {
                Color resultColor = (resultValue.Good ? Color.Transparent : Color.LightPink);

                if (MathHelper.IsNumeric(resultValue.Value) == true)
                    resultMessage.AddTableRow(resultColor, resultValue.Name, resultValue.Value.ToString(), String.Format("{0} ~ {1}", resultValue.Lcl, resultValue.Ucl));
                else
                    resultMessage.AddTableRow(resultColor, resultValue.Name, resultValue.Value.ToString());
            }

            resultMessage.EndTable();

            if (algorithmResult.ResultValueList.Count > 0)
            {
                AlgorithmResultValue resultValue = algorithmResult.ResultValueList[0];
                algorithmResult.ShortResultMessage = String.Format("{0} : {1}", resultValue.Name, resultValue.Value.ToString());
            }
        }

        public abstract List<AlgorithmResultValue> GetResultValues();

        public abstract void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region);

        public abstract String GetAlgorithmType();
        public abstract String GetAlgorithmTypeShort();
        public abstract void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage);

        public abstract AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam);

        public virtual void Dispose()
        {
            this.param.Dispose();
        }
    }
}
