using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCI_Test.Vision
{
    class WciAlgorithmParam
    {
        List<Rectangle> RectangleList => this.rectangleList;
        public List<Rectangle> rectangleList;

        public Size BlockSize { get => this.blockSize; set => this.blockSize = value; }
        Size blockSize;

        public Size InflateSize { get => this.inflateSize; set => this.inflateSize = value; }
        Size inflateSize;

        public float MatchingScore { get => this.matchingScore; set => this.matchingScore = value; }
        float matchingScore;

        public WciAlgorithmParam()
        {
            this.blockSize = new Size(80, 290);
            this.inflateSize = new Size(blockSize.Width / 2, blockSize.Height / 2);
            this.matchingScore = 50.0f;
            this.rectangleList = new List<Rectangle>();

            this.rectangleList.Add(new Rectangle(910, 2051, 7448, 4804));
            this.rectangleList.Add(new Rectangle(8570, 2052, 7436, 4801));
            this.rectangleList.Add(new Rectangle(16331, 2052, 1492, 4800));

            this.rectangleList.Add(new Rectangle(900, 6894, 7448, 4832));
            this.rectangleList.Add(new Rectangle(8559, 6894, 7436, 4830));
            this.rectangleList.Add(new Rectangle(16320, 6893, 1504, 4830));

            this.rectangleList.Add(new Rectangle(882, 12054, 7456, 4847));
            this.rectangleList.Add(new Rectangle(8547, 12053, 7440, 4849));
            this.rectangleList.Add(new Rectangle(16309, 12052, 1512, 4850));

            this.rectangleList.Add(new Rectangle(864, 16942, 7460, 9979));
            this.rectangleList.Add(new Rectangle(8523, 16946, 7452, 9975));
            this.rectangleList.Add(new Rectangle(16286, 16946, 1536, 9975));

            this.rectangleList.Add(new Rectangle(853, 26958, 7448, 4770));
            this.rectangleList.Add(new Rectangle(8513, 26960, 7436, 4766));
            this.rectangleList.Add(new Rectangle(16274, 26960, 1548, 4765));
        }
    }

    class WciInspectionParam
    {
        public ImageD MasterImageD { get; set; }
        public ImageD ImageD { get; set; }
    }

    class WciInspectionResult
    {
        public ImageD ResultImage { get; set; }
    }
}
