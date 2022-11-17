using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Planbss
{
    class VisionDef
    {
    }

    public enum YesNo
    {
        Yes, No
    }
    
    public enum eUSER
    {
        OPERATOR, ADMIN, DEVELOPER
    }

    public enum SearchSide
    {
        Left, Top, Right, Bottom
    }

    public enum CardinalPoint
    {
        East, West, South, North, NorthEast, NorthWest, SouthEast, SouthWest
    }

    public enum ConvexShape
    {
        None, Left, Top, Right, Bottom
    }

    public enum eIMAGEMONO
    {
        MONO_NONE, MONO_GRAY, MONO_CH1, MONO_CH2, MONO_CH3, MONO_BAYER
    }

    public enum eIMAGECOLOR
    {
        COLOR_NONE, COLOR_RGB, COLOR_HLS, COLOR_HSV, COLOR_YCrCb, COLOR_Lab,
        COLOR_Luv, COLOR_XYZ, COLOR_YUV, COLOR_HSLFULL, COLOR_HSVFULL
    }

    public enum eIMAGEUNION
    {
        UNION_NONE, UNION_MIN, UNION_MAX, UNION_PLUS, UNION_MINUS, UNION_MINEWSN,
        UNION_DIFFERENCE, UNION_MINUSMIN, UNION_MINUSMAX,
        UNION_BITWISEAND, UNION_BITWISEOR, UNION_BITWISEXOR, UNION_BITWISENOT
    }
    
    public struct PointD
    {
        private double x;
        public double X
        {
            get { return x;  }
            set { x = value; }
        }

        private double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
    
    public struct IF_IMAGE
    {
        public int nIndex;			///< Frame Index
        public eIMAGECOLOR eColor;			///< Frame Channel
        public eIMAGEMONO eMono;			///< Image Union Operation Type	
    }

    public struct IF_FRAME
    {
        public IF_IMAGE Source;
        public IF_IMAGE Target;
        public eIMAGEUNION eUnion;

        public void Set(int _nIndexSrc, eIMAGECOLOR _eColorSrc, eIMAGEMONO _eMonoSrc, eIMAGEUNION _eUnion = eIMAGEUNION.UNION_NONE,
                        int _nIndexTgt = 0, eIMAGECOLOR _eColorTgt = eIMAGECOLOR.COLOR_RGB, eIMAGEMONO _eMonoTgt = eIMAGEMONO.MONO_GRAY)
        {
            Source.nIndex = _nIndexSrc;
            Source.eColor = _eColorSrc;
            Source.eMono = _eMonoSrc;
            eUnion = _eUnion;
            Target.nIndex = _nIndexTgt;
            Target.eColor = _eColorTgt;
            Target.eMono = _eMonoTgt;
        }
    }
}
