using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision.OpenCv
{
    class OpenCvCharReader : CharReader
    {
        public override Algorithm Clone()
        {
            OpenCvCharReader charReader = new OpenCvCharReader();
            charReader.CopyFrom(this);

            return charReader;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            CharReaderResult charReaderResult = new CharReaderResult();
            charReaderResult.Good = false;
        
            charReaderResult.ResultValueList.Add(new AlgorithmResultValue("Desired String", charReaderResult.DesiredString));
            charReaderResult.ResultValueList.Add(new AlgorithmResultValue("String Read", charReaderResult.StringRead));
            
            return charReaderResult;
        }

        public override CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, DebugContext debugContext)
        {
            throw new NotImplementedException();
        }

        public override void AddCharactor(CharPosition charPosition, int charactorCode)
        {
            throw new NotImplementedException();
        }

        public override void AddCharactor(AlgoImage charImage, string charactorCode)
        {

        }

        public override void RemoveFont(CharFont charFont)
        {

        }

        public override List<CharFont> GetFontList()
        {
            List<CharFont> fontList = new List<CharFont>();

            return fontList;
        }

        public override void AutoSegmentation(AlgoImage algoImage, RotatedRect rotatedRect, string desiredString)
        {
            throw new NotImplementedException();
        }

        public override void Train(string fontFileName)
        {
            throw new NotImplementedException();
        }

        public override void SaveFontFile(string fontFileName)
        {
            return;
        }

        public override CharReaderResult Extract(AlgoImage algoImage, RectangleF characterRegion, int threshold, DebugContext debugContext)
        {
            CharReaderResult charReaderResult = new CharReaderResult();

            return charReaderResult;
        }

        public override bool Trained
        {
            get { return false; }
        }

        public override int CalibrateFont(AlgoImage charImage, string calibrationString)
        {
            return 0;
        }
    }
}
