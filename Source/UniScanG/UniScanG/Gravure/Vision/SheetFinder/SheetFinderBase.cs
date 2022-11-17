using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
//using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.SheetFinder
{
    public enum BaseXSearchDir { Default, Left2Right, Right2Left }
    public enum EEdgeFinderVersion { V1, V2, V3, V4 }

    public abstract class SheetFinderBaseParam : UniScanG.Vision.AlgorithmParamG
    {
        public EEdgeFinderVersion EdgeFinderVersion { get; set; } = EEdgeFinderVersion.V1;

        public BaseXSearchDir BaseXSearchDir { get; set; } = BaseXSearchDir.Default;

        public float BoundaryHeightMm { get; set; } = 35.0f;

        public float MinBrightnessStdDev { get; set; } = 1;

        public float SearchSkipWidthMm { get; set; } = 0;

        public float AlignGlobalMm { get; set; } = 0;

        public bool BaseXSearchHalf { get; set; } = false;

        public bool FallowingThreshold { get; set; } = true;

        public bool PrecisionPatternSizeFInd { get; set; } = true;

        public abstract SheetFinderBase CreateFinder();

        public SheetFinderBaseParam() : base()
        {
            this.BoundaryHeightMm = 35.0f;
            this.SearchSkipWidthMm = 0;

            this.AlignGlobalMm = 0;
        }

        public BaseXSearchDir GetBaseXSearchDir()
        {
            return this.BaseXSearchDir == BaseXSearchDir.Default ? GetSearchDir(SystemManager.Instance().ExchangeOperator.GetCamIndex()) : this.BaseXSearchDir;
        }

        private BaseXSearchDir GetSearchDir(int camId)
        {
            return camId > 0 ? BaseXSearchDir.Right2Left : BaseXSearchDir.Left2Right;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetFinderBaseParam sheetFinderBaseParam = srcAlgorithmParam as SheetFinderBaseParam;
            if (sheetFinderBaseParam != null)
            {
                this.BaseXSearchDir = sheetFinderBaseParam.BaseXSearchDir;
                this.BoundaryHeightMm = sheetFinderBaseParam.BoundaryHeightMm;
                this.SearchSkipWidthMm = sheetFinderBaseParam.SearchSkipWidthMm;
                this.AlignGlobalMm = sheetFinderBaseParam.AlignGlobalMm;
                this.MinBrightnessStdDev = sheetFinderBaseParam.MinBrightnessStdDev;
                this.BaseXSearchHalf = sheetFinderBaseParam.BaseXSearchHalf;
                this.PrecisionPatternSizeFInd = sheetFinderBaseParam.PrecisionPatternSizeFInd;
            }
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            this.EdgeFinderVersion = XmlHelper.GetValue(paramElement, "EdgeFinderVersion", EEdgeFinderVersion.V1);

            this.BaseXSearchDir = XmlHelper.GetValue(paramElement, "BaseXSearchDir", BaseXSearchDir.Default);

            this.AlignGlobalMm = XmlHelper.GetValue(paramElement, "AlignGlobalMm", 0);
            this.BoundaryHeightMm = XmlHelper.GetValue(paramElement, "BoundaryHeightMm", 35.0f);
            this.SearchSkipWidthMm = XmlHelper.GetValue(paramElement, "SearchSkipWidthMm", 0);

            this.MinBrightnessStdDev = XmlHelper.GetValue(paramElement, "MinBrightnessStdDev", this.MinBrightnessStdDev);
            this.BaseXSearchHalf = XmlHelper.GetValue(paramElement, "BaseXSearchHalf", this.BaseXSearchHalf);

            this.FallowingThreshold = XmlHelper.GetValue(paramElement, "FallowingThreshold", this.FallowingThreshold);

            this.PrecisionPatternSizeFInd = XmlHelper.GetValue(paramElement, "PrecisionPatternSizeFInd", this.PrecisionPatternSizeFInd);

            //this.AlignGlobalMm = 0;
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "EdgeFinderVersion", this.EdgeFinderVersion);
            XmlHelper.SetValue(paramElement, "BaseXSearchDir", this.BaseXSearchDir);

            XmlHelper.SetValue(paramElement, "BoundaryHeightMm", this.BoundaryHeightMm);
            XmlHelper.SetValue(paramElement, "SearchSkipWidthMm", this.SearchSkipWidthMm);
            XmlHelper.SetValue(paramElement, "AlignGlobalMm", this.AlignGlobalMm);
            XmlHelper.SetValue(paramElement, "MinBrightnessStdDev", this.MinBrightnessStdDev);
            XmlHelper.SetValue(paramElement, "BaseXSearchHalf", this.BaseXSearchHalf);

            XmlHelper.SetValue(paramElement, "FallowingThreshold", this.FallowingThreshold);

            XmlHelper.SetValue(paramElement, "PrecisionPatternSizeFInd", this.PrecisionPatternSizeFInd);
        }
    }

    public abstract class SheetFinderBase : UniScanG.Vision.AlgorithmG
    {
        public static string TypeName { get { return "SheetFinder"; } }
        public virtual new SheetFinderBaseParam Param
        {
            get => this.param != null ? (SheetFinderBaseParam)this.param : (SheetFinderBaseParam)AlgorithmSetting.Instance().SheetFinderBaseParam;
            set => this.param = value;
        }

        public static SheetFinderBaseParam SheetFinderBaseParam => AlgorithmSetting.Instance().SheetFinderBaseParam;

        #region Abstract
        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        #endregion

        #region virtual
        #endregion

        //public int DefaultBoundaryHeight
        //{
        //    get => ((SheetFinderBaseParam)this.param).DefaultBoundaryHeight;
        //}


        //float[] projData = new float[0];
        public static int FindBasePosition(AlgoImage algoImage, EdgeFinderBuffer edgeFinderBuffer, Direction direction, int length, bool reverse)
        {
            SheetFinderBaseParam sheetFinderBaseParam = SheetFinderBase.SheetFinderBaseParam;
            return FindBasePosition(algoImage, edgeFinderBuffer, sheetFinderBaseParam, direction, length, reverse);
        }

        public static int FindBasePosition(AlgoImage algoImage, EdgeFinderBuffer edgeFinderBuffer, SheetFinderBaseParam sheetFinderBaseParam, Direction direction, int length, bool reverse)
        {
            // 밝은 부분이 [length] 이상이고, 어두운 부분이 나타나면 검출.
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            // 인쇄된게 없으면 쩌~~만큼 들어가서 오프셋을 찾음. 변형측정 결과 위치 어긋남.
            // 인쇄된게 없으면 -1 리턴.
            if (false)
            {
                Rectangle statRect = Rectangle.Inflate(new Rectangle(Point.Empty, algoImage.Size), -(algoImage.Width - 1024) / 2, -(algoImage.Height - 1024) / 2);
                using (AlgoImage statImage = algoImage.GetSubImage(statRect))
                {
                    StatResult statResult = imageProcessing.GetStatValue(statImage);
                    //double std = imageProcessing.GetStdDev(algoImage);
                    if ((statResult.max - statResult.min) < 80 || statResult.stdDev < 25)
                        return -1;
                }
            }

            int nbEntry = 0;
            float[] projData = edgeFinderBuffer?.Datas;
            if (projData == null)
                projData = new float[direction == Direction.Horizontal ? algoImage.Width : algoImage.Height];

            Rectangle rect = new Rectangle(0, 0, Math.Min(projData.Length, algoImage.Width), Math.Min(projData.Length, algoImage.Height));
            using (AlgoImage projImage = algoImage.GetSubImage(rect))
                nbEntry = imageProcessing.Projection(projImage, ref projData, direction, ProjectionType.Mean);

            //float[] projData = imageProcessing.Projection(algoImage, direction, ProjectionType.Mean);
            //int nbEntry = projData.Length;

            float threshold;
            switch (sheetFinderBaseParam.EdgeFinderVersion)
            {
                default:
                case EEdgeFinderVersion.V1:
                    threshold = projData.Take(nbEntry).Average();
                    break;

                case EEdgeFinderVersion.V2:
                    IEnumerable<float> t = projData.Take(nbEntry);
                    threshold = (t.Max() + t.Min()) / 2;
                    break;

                case EEdgeFinderVersion.V3:
                    int nbEntry3 = nbEntry / 3;
                    threshold = projData.Skip(nbEntry3).Take(nbEntry3).Average();
                    break;

                case EEdgeFinderVersion.V4:
                    return FindBasePositionSobel(projData, nbEntry, edgeFinderBuffer?.SobelBuffer, edgeFinderBuffer?.Bytes, length, reverse);

            }
            //File.WriteAllText(@"C:\temp\projData.txt", string.Join(Environment.NewLine, projData.Select(f => f.ToString())));
            int curLength = 0;

            BaseXSearchDir baseXSearchDir = BaseXSearchDir.Left2Right;// sheetFinderBaseParam.GetBaseXSearchDir();
            bool isLeft2Rigth = (baseXSearchDir == BaseXSearchDir.Left2Right || direction == Direction.Vertical);
            if (reverse)
                isLeft2Rigth = !isLeft2Rigth;

            if (isLeft2Rigth)
            // Left -> Right 
            {
                bool level = projData[0] > threshold;
                for (int i = 1; i < nbEntry; i++)
                {
                    bool curLevel = projData[i] > threshold;
                    if (curLevel == level)
                        curLength++;

                    if (level == true && curLevel == false && curLength > length)
                        return i;

                    if (curLevel != level)
                        curLength = 0;

                    level = curLevel;
                }
            }
            else
            // Right -> Left
            {
                bool level = projData[nbEntry - 1] > threshold;
                for (int i = nbEntry - 2; i >= 0; i--)
                {
                    bool curLevel = projData[i] > threshold;

                    if (curLevel == level)
                        curLength++;

                    if (level == true && curLevel == false && curLength > length)
                        return i;

                    if (curLevel != level)
                        curLength = 0;

                    level = curLevel;
                }
            }


            //float[] projData = imageProcessing.Projection(subImage, direction, ProjectionType.Mean);
            //subImage.Dispose();
            //float threshold = projData.Average();
            //int curLength = 0;

            //if (direction == Direction.Vertical ||
            //    sheetFinderBaseParam.BaseXSearchDir == BaseXSearchDir.Left2Right)
            //    // Left -> Right 
            //{
            //    bool level = projData[0] > threshold;
            //    for (int i = 1; i < projData.Length; i++)
            //    {
            //        bool curLevel = projData[i] > threshold;
            //        if (curLevel == level)
            //            curLength++;

            //        if (level == true && curLevel == false && curLength > length)
            //            return i;

            //        if (curLevel != level)
            //            curLength = 0;

            //        level = curLevel;
            //    }
            //}
            //else
            //// Right -> Left
            //{
            //    bool level = projData.Last() > threshold;
            //    for (int i = projData.Length-2; i >=0; i--)
            //    {
            //        bool curLevel = projData[i] > threshold;

            //        if (curLevel == level)
            //            curLength++;

            //        if (level == true && curLevel == false && curLength > length)
            //            return i;

            //        if (curLevel != level)
            //            curLength = 0;

            //        level = curLevel;
            //    }
            //}

            return -1;
        }

        private static int FindBasePositionSobel(float[] projData, int nbEntry, AlgoImage buffer1D, byte[] bufferBytes, int length, bool reverse)
        {
            int index = -1;

            bool disposeNeed = false;
            if (buffer1D == null)
            {
                disposeNeed = true;
                buffer1D = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Depth, new Size(nbEntry, 1));
                bufferBytes = new byte[nbEntry * sizeof(float)];
            }

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(buffer1D);

            //File.WriteAllText(@"C:\temp\projData.txt", string.Join(Environment.NewLine, projData.Select(f => f.ToString())));
            RCI.RCIHelper.GetBytes(projData, bufferBytes);
            buffer1D.SetByte(bufferBytes);
            ip.Sobel(buffer1D, buffer1D, Direction.Horizontal);

            buffer1D.GetByte(bufferBytes);
            RCI.RCIHelper.GetSingles(bufferBytes, projData);

            //File.WriteAllText(@"C:\temp\projData2.txt", string.Join(Environment.NewLine, projData.Select(f => f.ToString())));
            if (reverse)
            {
                index = Array.FindLastIndex(projData, f => f > +20);
            }
            else
            {
                index = Array.FindIndex(projData, f => f < -20);
            }


            if (disposeNeed)
            {
                buffer1D.Dispose();
                bufferBytes = null;
            }

            return index;
        }
    }
}
