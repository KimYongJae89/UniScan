using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanS.Common.Data;
using UniScanS.Screen.Vision.Detector;

namespace UniScanS.Vision.FiducialFinder
{
    public class FiducialFinderAlgorithmResult : AlgorithmResult, IExportable
    {
        float score;
        public float Score
        {
            get { return score; }
            set { score = value; }
        }

        Bitmap foundImage;
        public Bitmap FoundImage
        {
            get { return foundImage; }
            set { foundImage = value; }
        }

        public FiducialFinderAlgorithmResult() : base(FiducialFinder.TypeName)
        {
        }

        public void Export(string key, string path)
        {
            FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);

            if (fiducialFinder == null)
                return;

            FiducialFinderParam param = (FiducialFinderParam)fiducialFinder.Param;

            string fileName = Path.Combine(path, string.Format("{0}.csv", key));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t", param.MinScore, param.SearchRangeHalfWidth, param.SearchRangeHalfHeight);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("{0}\t{1}\t", OffsetFound.Width, RealOffsetFound.Height);
            stringBuilder.AppendFormat("{0}\t{1}\t", RealOffsetFound.Width, RealOffsetFound.Height);
            
            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public static Tuple<AlgorithmResult, AlgorithmParam> Import(int index, string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", index));
            string[] lines = File.ReadAllLines(path);
            
            FiducialFinderAlgorithmResult result = new FiducialFinderAlgorithmResult();
            FiducialFinderParam param = new FiducialFinderParam();

            List<string> resultList = new List<string>();
            foreach (string line in lines)
                resultList.AddRange(line.Split('\t'));

            if (resultList.Count < 6)
                return null;

            param.MinScore = Convert.ToInt32(resultList[0]);
            param.SearchRangeHalfWidth = Convert.ToInt32(resultList[1]);
            param.SearchRangeHalfHeight = Convert.ToInt32(resultList[2]);

            result.OffsetFound = new System.Drawing.SizeF(Convert.ToSingle(resultList[3]), Convert.ToSingle(resultList[4]));
            result.RealOffsetFound = new System.Drawing.SizeF(Convert.ToSingle(resultList[5]), Convert.ToSingle(resultList[6]));

            Tuple<AlgorithmResult, AlgorithmParam> set = new Tuple<AlgorithmResult, AlgorithmParam>(result, param);

            return set;
        }

        public Figure GetFigure()
        {
            return new RectangleFigure(this.resultRect, new Pen(Color.Blue, 50));
        }
    }
}
