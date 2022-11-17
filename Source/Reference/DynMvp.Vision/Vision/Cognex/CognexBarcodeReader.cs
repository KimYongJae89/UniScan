using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Cognex.VisionPro;
using Cognex.VisionPro.ID;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision.Cognex
{
    public class CognexBarcodeReader : BarcodeReader
    {
        CogID cogId = new CogID();
        public CogID CogId
        {
            get { return cogId; }
            set { cogId = value; }
        }

        int productCount;
        public int ProductCount
        {
            get { return productCount; }
            set { productCount = value; }
        }

        public CognexBarcodeReader()
        {
            
        }

        public override Algorithm Clone()
        {
            CognexBarcodeReader barcodeReader = new CognexBarcodeReader();
            barcodeReader.CopyFrom(this);

            return barcodeReader;
        }

        public override AlgorithmResult Read(AlgoImage algoImage, RectangleF clipRect, DebugContext debugContext)
        {
            PointF centerPt = new PointF(algoImage.Width/2, algoImage.Height/2);

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, algoImage.Width, algoImage.Height, 0, 0);

            CognexGreyImage greyImage = (CognexGreyImage)algoImage;

            cogId.NumToFind = 100;

            EnableBarcodeTypes();
            cogId.DataMatrix.Enabled = true;
            //cogId.EANUCCComposite.Enabled = true;
            //cogId.UpcEan.Enabled = true;


            CogIDResults results = cogId.Execute(greyImage.Image, cogRectangle);

            AlgorithmResult barcodeReaderResult = CreateAlgorithmResult();
            barcodeReaderResult.Good = false;

            if (results.Count > 0)
            {
                barcodeReaderResult.Good = true;

                BarcodePositionList barcodePositionList = new BarcodePositionList();

                for (int i = 0; i < results.Count; i++)
                {
                    BarcodePosition barcodePosition = new BarcodePosition();
                    barcodePosition.StringRead = results[i].DecodedData.DecodedString;

                    barcodePosition.FoundPosition = new List<PointF>();

                    List<PointF> positionList = CognexHelper.Convert(results[i].BoundsPolygon);
                    for (int index = 3; index >= 0; index--)
                    {
                        barcodePosition.FoundPosition.Add(positionList[index]); // new PointF(positionList[index].X + rotatedRect.X, positionList[index].Y + rotatedRect.Y));
                    }
                    
                    barcodePositionList.Items.Add(barcodePosition);
                }

                barcodeReaderResult.ResultValueList.Add(new AlgorithmResultValue("BarcodePositionList", barcodePositionList));
            }

            return barcodeReaderResult;
        }

        private void EnableBarcodeTypes()
        {
            cogId.DisableAllCodes();

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            foreach (BarcodeType barcodeType in barcodeReaderParam.BarcodeTypeList)
            {
                switch (barcodeType)
                {
                    case BarcodeType.DataMatrix:
                        cogId.DataMatrix.Enabled = true;
                        break;
                    case BarcodeType.QRCode:
                        cogId.QRCode.Enabled = true;
                        break;
                    case BarcodeType.Codabar:
                        cogId.Codabar.Enabled = true;
                        break;
                    case BarcodeType.Code128:
                        cogId.Code128.Enabled = true;
                        break;
                    case BarcodeType.Code39:
                        cogId.Code39.Enabled = true;
                        break;
                    case BarcodeType.Code93:
                        cogId.Code93.Enabled = true;
                        break;
                    case BarcodeType.Interleaved2of5:
                        cogId.I2Of5.Enabled = true;
                        break;
                    case BarcodeType.Pharmacode:
                        cogId.Pharmacode.Enabled = true;
                        break;
                    case BarcodeType.PLANET:
                        cogId.Planet.Enabled = true;
                        break;
                    case BarcodeType.POSTNET:
                        cogId.Postnet.Enabled = true;
                        break;
                    case BarcodeType.FourStatePostal:
                        cogId.FourState.Enabled = true;
                        break;
                    case BarcodeType.UPCEAN:
                        cogId.UpcEan.Enabled = true;
                        break;
                    case BarcodeType.EANUCCComposite:
                        cogId.EANUCCComposite.Enabled = true;
                        break;
                    case BarcodeType.PDF417:
                        cogId.PDF417.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
