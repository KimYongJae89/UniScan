using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.Base;

namespace DynMvp.Vision
{
    public class MatchPos
    {
        private PointF pos = new PointF(0, 0);
        public PointF Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        private float score;
        public float Score
        {
            get { return score; }
            set { score = value; }
        }

        private Size patternSize;
        public Size PatternSize
        {
            get { return patternSize; }
            set { patternSize = value; }
        }

        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        private PatternType patternType = PatternType.Good;
        public PatternType PatternType
        {
            get { return patternType; }
            set { patternType = value; }
        }

        public RectangleF RectF
        {
            get { return DrawingHelper.FromCenterSize(this.pos, this.patternSize); }
        }

        public Rectangle Rect
        {
            get { return Rectangle.Ceiling(this.RectF); }
        }

        public MatchPos()
        {
        }

        public MatchPos(PointF pos, float score)
        {
            this.pos = pos;
            this.score = score;
        }

        public override string ToString()
        {
            return String.Format("(X:{0:0.00}, Y:{1:0.00}), θ:{2:0.0} / Score {3:0.0}", pos.X, pos.Y, angle, score * 100);
        }
    }

    public class PatternResult : SubResult
    {
        private List<MatchPos> matchPosList = new List<MatchPos>();
        public List<MatchPos> MatchPosList
        {
            get { return matchPosList; }
            set { matchPosList = value; }
        }

        bool badImage = false;
        public bool BadImage
        {
            get { return badImage; }
            set { badImage = value; }
        }

        bool notTrained = false;
        public bool NotTrained
        {
            get { return notTrained; }
            set { notTrained = value; }
        }

        public PatternResult() : base("PatternSubResult") { }

        public void AddMatchPos(MatchPos matchPos)
        {
            matchPosList.Add(matchPos);
        }

        public float MaxScore
        {
            get {
                if (matchPosList.Count > 0)
                    return (float)matchPosList.Max(x => x.Score);

                return 0;
            }
        }

        public MatchPos MaxMatchPos
        {
            get
            {
                float maxScore = MaxScore;
                return matchPosList.Find(f => f.Score == maxScore);
                //MatchPos maxMatchPos = new MatchPos();
                //foreach (MatchPos matchPos in matchPosList)
                //{
                //    if (matchPos.Score > maxMatchPos.Score)
                //        maxMatchPos = matchPos;
                //}

                //return maxMatchPos;
            }
        }
    }

    //public class PatternMatchingResult : AlgorithmResult
    //{
    //    string recogString;
    //    public string RecogString
    //    {
    //        get { return recogString; }
    //        set { recogString = value; }
    //    }

    //    public float MaxScore
    //    {
    //        get {
    //            if (subResultList.Count > 0)
    //                return subResultList.Max(x => ((PatternResult)x).MaxScore);

    //            return 0;
    //        }
    //    }

    //    public MatchPos MaxMatchPos
    //    {
    //        get {
    //            MatchPos maxMatchPos = new MatchPos();
    //            if (subResultList.Count > 0)
    //            {
    //                foreach (PatternResult subResult in subResultList)
    //                {
    //                    MatchPos matchPos = subResult.MaxMatchPos;
    //                    if (matchPos.Score > maxMatchPos.Score)
    //                        maxMatchPos = matchPos;
    //                }
    //            }

    //            return maxMatchPos;
    //        }
    //    }

    //    public PatternMatchingResult() : base(true, false)
    //    {

    //    }
    //}
}
