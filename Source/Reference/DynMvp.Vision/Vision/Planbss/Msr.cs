using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using DynMvp.Vision.OpenCv;
using System.Xml;
using DynMvp.Base;

namespace DynMvp.Vision.Planbss
{
    public class Msr
    {
        #region < Constructor >
        public Msr()
        {        
            m_Frame.Set(0, eIMAGECOLOR.COLOR_RGB, eIMAGEMONO.MONO_GRAY);
        }
        #endregion

        #region < Variable and Default >
        private int       m_nErr = 0;
        private int       m_nMsrCount = 4;
        private bool      m_bUse = true;
        private Rectangle m_TcRoi = new Rectangle(0, 0, 0, 0);
        private Rectangle m_IsRoi = new Rectangle(0, 0, 0, 0);
        private IF_FRAME  m_Frame = new IF_FRAME();
        private PointD    m_OutPt = new PointD(0, 0);
        #endregion

        #region < Get/Set Function >    
        public int gsErr
        {
            get { return m_nErr; }
            set { m_nErr = value; }
        }
        public int gsMsrCount
        {
            get { return m_nMsrCount; }
            set { m_nMsrCount = value; }
        }        
        public bool gsUse 
        {
            get { return m_bUse; }
            set { m_bUse = value; }
        }
        public Rectangle gsTcRoi
        {
            get { return m_TcRoi; }
            set { m_TcRoi = value; }
        }
        public Rectangle gsIsRoi
        {
            get { return m_IsRoi; }
            set { m_IsRoi = value; }
        }
        public IF_FRAME gsFrame
        {
            get { return m_Frame; }
            set { m_Frame = value; }
        }
        public PointD gsOutPt
        {
            get { return m_OutPt; }
            set { m_OutPt = value; }
        }
        #endregion

        #region < Member Function >
        public virtual bool Verify(int SizeX, int SizeY)				///< Verify Parameter
        {
            if ((m_TcRoi.Left < 0) || (m_TcRoi.Right >= SizeX)) return false;
            if ((m_TcRoi.Top < 0) || (m_TcRoi.Bottom >= SizeY)) return false;
            if ((m_TcRoi.Width <= 0) || (m_TcRoi.Height <= 0))
                return false;

            if ((m_IsRoi.Left < 0) || (m_IsRoi.Right >= SizeX)) return false;
            if ((m_IsRoi.Top < 0) || (m_IsRoi.Bottom >= SizeY)) return false;
            if ((m_IsRoi.Width <= 0) || (m_IsRoi.Height <= 0))
                return false;

            if (m_Frame.Source.eMono == eIMAGEMONO.MONO_NONE) return false;
            if (m_Frame.Source.nIndex < 0) return false;
            if (m_Frame.eUnion != eIMAGEUNION.UNION_NONE)
            {
                if (m_Frame.Target.eMono == eIMAGEMONO.MONO_NONE) return false;
                if (m_Frame.Target.nIndex < 0) return false;
            }

            return true;
        }

        private String GetMsrName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Use";
                case 1: return "Area";
                case 2: return "Source";
                case 3: return "Target";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetMsr(String strTitle, String strData)
        {
            HelpMath helper = new HelpMath();

            for (int i = 0; i < gsMsrCount; i++)
            {
                if (false == strTitle.Equals( GetMsrName(i) ) )
                    continue;

                Rectangle rData = Rectangle.FromLTRB(0, 0, 0, 0);

                switch (i)
                {
                    case 0: gsUse = helper.ToSplit(strData); break;
                    case 1: helper.ToSplit(strData, ref m_TcRoi); break;
                    case 2: helper.ToSplit(strData, ref rData);
                            m_Frame.Source.nIndex = (int)rData.Left;
                            m_Frame.Source.eColor = (eIMAGECOLOR)rData.Top;
                            m_Frame.Source.eMono = (eIMAGEMONO)rData.Right;
                            m_Frame.eUnion = (eIMAGEUNION)rData.Bottom;
                        break;
                    case 3: helper.ToSplit(strData, ref rData);
                            m_Frame.Target.nIndex = (int)rData.Left;
                            m_Frame.Target.eColor = (eIMAGECOLOR)rData.Top;
                            m_Frame.Target.eMono = (eIMAGEMONO)rData.Right;
                        break;
                    default:
                        return false;
                }

                return true;
            }

            return false;	// !! NOT Error : Only SetMsr isn't have member.
        }

        public bool GetMsr(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsMsrCount))
                return false;

            strName = GetMsrName(nIndex);

            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", (gsUse == true) ? 1 : 0);
                    break;
                case 1: strData = String.Format("{0}, {1}, {2}, {3}", gsTcRoi.Left, gsTcRoi.Top, 
                                                                      gsTcRoi.Right, gsTcRoi.Bottom);
                    break;
                case 2: strData = "Not Support";
                    break;
                case 3: strData = "Not Support";
                    break;
                default:
                    return false;
            }

            return true;
        }

        #endregion        
    }

    public class MsrEdge : Msr
    {
        #region < Constructor >
        public MsrEdge()
        {
        }
        #endregion

        #region < Variable and Default >
        private int       m_nEdgeCount = 6;	// Edge Parameter Count
        private int       m_Direct = 1;		// [1 ~ 8] 0° 90° 180° 270° 45° 135° 225° 315°
        private int       m_WtoB = 0;		// [0] No Polarity, [1] WtoB, [2] BtoW
        private int       m_ThickW = 1;     // Edge Area Gv Width
        private int       m_ThickH = 5;     // Edge Area Gv Height
        private int       m_Distance = 5;   // Edge Compare Distance
        private int       m_Gv = 20;	    // Relative Threshold	
        private Rectangle m_Roi = new Rectangle(0, 0, 0, 0);    // Edge Find Roi
        private PointD    m_EdgeOutPt = new PointD(0, 0);       // Edge Out Point
        #endregion

        #region < Get/Set Function >
        public int gsEdgeCount
        {
            get { return m_nEdgeCount; }
            set { m_nEdgeCount = value; }
        }
        public int gsDirect
        {
            get { return m_Direct; }
            set { m_Direct = value; }
        }
        public int gsWtoB
        {
            get { return m_WtoB; }
            set { m_WtoB = value; }
        }
        public int gsGv
        {
            get { return m_Gv; }
            set { m_Gv = value; }
        }
        public int gsThickW
        {
            get { return m_ThickW; }
            set { m_ThickW = value; }
        }
        public int gsThickH
        {
            get { return m_ThickH; }
            set { m_ThickH = value; }
        }
        public int gsDistance
        {
            get { return m_Distance; }
            set { m_Distance = value; }
        }
        public Rectangle gsEdgeRoi
        {
            get { return m_Roi; }
            set { m_Roi = value; }
        }
        public PointD gsEdgeOutPt
        {
            get { return m_EdgeOutPt; }
            set { m_EdgeOutPt = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if( false == base.Verify(SizeX, SizeY) )
                return false;

            if ((m_Direct < 1) || (m_Direct > 8))
                return false;

            if ((m_WtoB < 0) || (m_WtoB > 2))
                return false;

            if ((m_ThickW <= 0) || (m_ThickH <= 0))
                return false;

            if (m_Distance <= 0)
                return false;

            if ((m_Gv == 0) || (m_Gv == 255))
                return false;            

            return true;
        }

        private String GetEdgeName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Direct";
                case 1: return "WtoB";
                case 2: return "ThickW";
                case 3: return "ThickH";
                case 4: return "Distance";
                case 5: return "GrayValue";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetEdge(String strTitle, String strData)
        {
            if (SetMsr(strTitle, strData))
                return true;

            bool bEntry = false;
            for (int i = 0; i < gsEdgeCount; i++)
            {
                if (false == strTitle.Equals(GetEdgeName(i)))
                    continue;

                switch (i)
                {
                    case 0: gsDirect = Convert.ToInt32(strData); break;
                    case 1: gsWtoB = Convert.ToInt32(strData); break;
                    case 2: gsThickW = Convert.ToInt32(strData); break;
                    case 3: gsThickH = Convert.ToInt32(strData); break;
                    case 4: gsDistance = Convert.ToInt32(strData); break;
                    case 5: gsGv = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }

        public bool GetEdge(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsEdgeCount))
                return false;

            strName = GetEdgeName(nIndex);

            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", gsDirect); break;
                case 1: strData = String.Format("{0}", gsWtoB); break;
                case 2: strData = String.Format("{0}", gsThickW); break;
                case 3: strData = String.Format("{0}", gsThickH); break;
                case 4: strData = String.Format("{0}", gsDistance); break;
                case 5: strData = String.Format("{0}", gsGv); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrLine : MsrEdge
    {
        #region < Constructor >
        public MsrLine()
        {
        }
        #endregion

        #region < Variable and Default >
        private int     m_nLineCount = 10;
        private int     m_nBumpyCount = 5;

        private int     m_Scan = 3;				    // Sampling
        private Point   m_Rate = new Point(60, 0);	// Edge Find Rate : (x,y) = (In, Out)		
        private PointD  m_Angle = new PointD(0, 0);	// Angle Spec (Teach) : Measured Angle (Radian)
        private double  m_AngleTolP =  5.0f;		// Angle Tolerance +
        private double  m_AngleTolM = -5.0f;		// Angle Tolerance -

        private bool        m_UseBumpy = false;
        private int         m_ScanBumpy = 2;		// Sampling
        private SearchSide  m_BGSide = SearchSide.Left;	// Background Position : L, T, R, B
        private int         m_Iteration = 3;		// Bumpy Iteration
        private int         m_Depth = 5;            // Bumpy Depth
        #endregion

        #region < Get/Set Function >
        public int gsLineCount
        {
            get { return m_nLineCount; }
            set { m_nLineCount = value; }
        }
        public int gsBumpyCount
        {
            get { return m_nBumpyCount; }
            set { m_nBumpyCount = value; }
        }
        public int gsScan
        {
            get { return m_Scan; }
            set { m_Scan = value; }
        }
        public Point gsRate
        {
            get { return m_Rate; }
            set { m_Rate = value; }
        }
        public int GetRateIn()
        {
            return m_Rate.X;
        }
        public int GetRateOut()
        {
            return m_Rate.Y;
        }
        public void SetRateOut(int nRate)
        {
            m_Rate.Y = nRate;
        }
        public PointD gsAngle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }
        public double GetAngleOut()
        {
            return m_Angle.Y;
        }
        public void SetAngleOut(double dAngle)
        {
            m_Angle.Y = dAngle;
        }
        public double gsAngleTolP
        {
            get { return m_AngleTolP; }
            set { m_AngleTolP = value; }
        }
        public double gsAngleTolM
        {
            get { return m_AngleTolM; }
            set { m_AngleTolM = value; }
        }

        public bool gsUseBumpy
        {
            get { return m_UseBumpy; }
            set { m_UseBumpy = value; }
        }
        public int gsScanBumpy
        {
            get { return m_ScanBumpy; }
            set { m_ScanBumpy = value; }
        }
        public SearchSide gsBGSide
        {
            get { return m_BGSide; }
            set { m_BGSide = value; }
        }
        public int gsIteration
        {
            get { return m_Iteration; }
            set { m_Iteration = value; }
        }
        public int gsDepth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
                return false;

            if (m_Scan <= 0)
                return false;

            if (gsUseBumpy)
            {
                if (m_ScanBumpy < 1)
                    return false;

                if ((m_BGSide < SearchSide.Left) || (m_BGSide > SearchSide.Bottom))
                    return false;

                if ((m_Iteration < 2) || (m_Depth < 3))
                    return false;
            }
            
            return true;
        }

        private String GetLineName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
            // Line
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Angle";
                case 3: return "AngleTolP";
                case 4: return "AngleTolM";
            // Bumpy
                case 5: return "UseBumpy";
                case 6: return "ScanBumpy";
                case 7: return "BGSide";
                case 8: return "Iteration";
                case 9: return "Depth";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetLine(String strTitle, String strData)
        {
            if (SetEdge(strTitle, strData))
                return true;

            HelpMath helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < gsLineCount; i++)
            {
                if (false == strTitle.Equals(GetLineName(i)))
                    continue;

                switch (i)
                {
                // Line
                    case 0: gsScan = Convert.ToInt32(strData); break;
                    case 1: m_Rate.X = Convert.ToInt32(strData); break;
                    case 2: m_Angle.X = Convert.ToDouble(strData); break;
                    case 4: gsAngleTolP = Convert.ToDouble(strData); break;
                    case 5: gsAngleTolM = Convert.ToDouble(strData); break;
                // Bumpy
                    case 6: gsUseBumpy = helper.ToSplit(strData); break;
                    case 7: gsScanBumpy = Convert.ToInt32(strData); break;
                    case 8: gsBGSide = (SearchSide)(Convert.ToInt32(strData)); break;
                    case 9: gsIteration = Convert.ToInt32(strData); break;
                    case 10: gsDepth = Convert.ToInt32(strData); break;                  
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }
        public bool GetLine(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsLineCount))
                return false;

            strName = GetLineName(nIndex);
            switch (nIndex)
            {
            // Line
                case 0: strData = String.Format("{0}", gsScan); break;
                case 1: strData = String.Format("{0}", gsRate.X); break;
                case 2: strData = String.Format("{0}", gsAngle.X); break;
                case 3: strData = String.Format("{0}", gsAngleTolP); break;
                case 4: strData = String.Format("{0}", gsAngleTolM); break;
            // Bumpy
                case 5: strData = String.Format("{0}", (gsUseBumpy == true) ? 1 : 0); break;
                case 6: strData = String.Format("{0}", gsScanBumpy); break;
                case 7: strData = String.Format("{0}", gsBGSide); break;
                case 8: strData = String.Format("{0}", gsIteration); break;
                case 9: strData = String.Format("{0}", gsDepth); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrCorner : MsrEdge
    {
        #region < Constructor >
        public MsrCorner()
        {
        }
        #endregion

        #region < Variable and Default >
        private int             m_nCornerCount = 6;
        private int             m_Scan = 3;				    // Sampling
        private Point           m_Rate = new Point(60, 0);  // Edge Find Rate : (x,y) = (In, Out)	
        private CardinalPoint   m_Cardinal = CardinalPoint.NorthEast;
        private bool            m_OutToIn = true;           // Edge Find Direction
        private int             m_Range = 30;               // Line Find Range
        private int             m_Length = 80;              // Line Find Length
        private double          m_OutAngleHor = 0;          // Hor-Line Out-Angle
        private double          m_OutAngleVer = 0;          // Ver-Line Out-Angle
        #endregion

        #region < Get/Set Function >
        public int gsCornerCount
        {
            get { return m_nCornerCount; }
            set { m_nCornerCount = value; }
        }
        public int gsScan
        {
            get { return m_Scan; }
            set { m_Scan = value; }
        }
        public Point gsRate
        {
            get { return m_Rate; }
            set { m_Rate = value; }
        }
        public int GetRateIn()
        {
            return m_Rate.X;
        }
        public void SetRateIn(int nRate)
        {
            m_Rate.X = nRate;
        }
        public int GetRateOut()
        {
            return m_Rate.Y;
        }
        public void SetRateOut(int nRate)
        {
            m_Rate.Y = nRate;
        }
        public CardinalPoint gsCardinal
        {
            get { return m_Cardinal; }
            set { m_Cardinal = value; }
        }
        public bool gsOtuToIn
        {
            get { return m_OutToIn; }
            set { m_OutToIn = value; }
        }
        public int gsRange
        {
            get { return m_Range; }
            set { m_Range = value; }
        }
        public int gsLength
        {
            get { return m_Length; }
            set { m_Length = value; }
        }
        public double gsOutAngleHor   
        {
            get { return m_OutAngleHor; }
            set { m_OutAngleHor = value; }
        }
        public double gsOutAngleVer
        {
            get { return m_OutAngleVer; }
            set { m_OutAngleVer = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
                return false;

            if (m_Scan <= 0)
                return false;

            return true;
        }

        private String GetCornerName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Cardinal";
                case 3: return "OutToIn";
                case 4: return "Range";
                case 5: return "Length";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetCorner(String strTitle, String strData)
        {
            if (SetEdge(strTitle, strData))
                return true;

            HelpMath helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < gsCornerCount; i++)
            {
                if (false == strTitle.Equals(GetCornerName(i)))
                    continue;

                switch (i)
                {
                    case 0: gsScan = Convert.ToInt32(strData); break;
                    case 1: m_Rate.X = Convert.ToInt32(strData); break;
                    case 2: gsCardinal = (CardinalPoint)(Convert.ToInt32(strData)); break;
                    case 3: gsOtuToIn = helper.ToSplit(strData); break;
                    case 4: gsRange = Convert.ToInt32(strData); break;
                    case 5: gsLength = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }
        public bool GetCorner(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsCornerCount))
                return false;

            strName = GetCornerName(nIndex);
            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", gsScan); break;
                case 1: strData = String.Format("{0}", gsRate.X); break;
                case 2: strData = String.Format("{0}", gsCardinal); break;
                case 3: strData = String.Format("{0}", gsOtuToIn); break;
                case 4: strData = String.Format("{0}", gsRange); break;
                case 5: strData = String.Format("{0}", gsLength); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrQuadrangle : MsrCorner
    {
        #region < Constructor >
        public MsrQuadrangle()
        {
        }
        #endregion

        #region < Variable and Default >
        private int         m_nQuadrangleCount = 1;
        private ConvexShape m_Convex = ConvexShape.None;
        private double      m_OutAngle = 0;  // Degree
        private PointD      m_VertexLT = new PointD(0, 0);
        private PointD      m_VertexRT = new PointD(0, 0);
        private PointD      m_VertexRB = new PointD(0, 0);
        private PointD      m_VertexLB = new PointD(0, 0);
        #endregion

        #region < Get/Set Function >
        public int gsQuadrangleCount
        {
            get { return m_nQuadrangleCount; }
            set { m_nQuadrangleCount = value; }
        }               
        public ConvexShape gsConvex
        {
            get { return m_Convex; }
            set { m_Convex = value; }
        }
        public double gsOutAngle
        {
            get { return m_OutAngle; }
            set { m_OutAngle = value; }
        }
        public PointD gsVertexLT
        {
            get { return m_VertexLT; }
            set { m_VertexLT = value; }
        }
        public PointD gsVertexRT
        {
            get { return m_VertexRT; }
            set { m_VertexRT = value; }
        }
        public PointD gsVertexRB
        {
            get { return m_VertexRB; }
            set { m_VertexRB = value; }
        }
        public PointD gsVertexLB
        {
            get { return m_VertexLB; }
            set { m_VertexLB = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
                return false;            

            return true;
        }

        private String GetQuadrangleName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Convex";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetQuadrangle(String strTitle, String strData)
        {
            if (SetCorner(strTitle, strData))
                return true;

            bool bEntry = false;
            for (int i = 0; i < gsCornerCount; i++)
            {
                if (false == strTitle.Equals(GetQuadrangleName(i)))
                    continue;

                switch (i)
                {                   
                    case 0: gsConvex = (ConvexShape)(Convert.ToInt32(strData));
                        break;                   
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }
        public bool GetQuadrangle(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsQuadrangleCount))
                return false;

            strName = GetQuadrangleName(nIndex);
            switch (nIndex)
            {             
                case 0: strData = String.Format("{0}", gsConvex);
                    break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }

    public class MsrCircle : MsrEdge
    {
        #region < Constructor >
        public MsrCircle()
        {
        }
        #endregion

        #region < Variable and Default >
        private int     circleCount = 6    // Circle
                                      +  5    // Check Bumpy
                                      + 16    // Check Width
                                      +  0;   // Check Uniformity
                                //    +  3;   // Check Uniformity;		// Circle Parameter Count
     // Circle
        private int     m_Scan = 8;				//   Sampling
        private Point   m_Rate = new Point(60, 0);				//   Edge Find Rate : (x,y) = (In, Out)	
        private PointD  m_Radius = new PointD(650, 0);			// * Expected Radius : (x,y) = (In, Out)	
        private int     m_Range = 24;			// * Edge Range	
        private int     m_RadiusTolP = 3;		//   Radius Tolerance +
        private int     m_RadiusTolM = 3;		//   Radius Tolerance -
     // Bumpy
        private bool        m_UseBumpy = false;
        private int         m_ScanBumpy = 2;		    // Sampling
        private SearchSide  m_BGSide = SearchSide.Left;	// Background Position : Inner, Outer
        private int         m_Iteration = 5;		    // Iteration Bumpy
        private int         m_Depth = 5;			    // Distance Bumpy
     // Width
        private bool    m_UseWidth = false;
        private int     m_ScanWidth = 1;        // Sampling
        private int     m_OffsetRadius = 0;		// Background Position : Inner, Outer
        private int     m_OuterOffset = 2;      // Expected Outer Edge Offset Value
        private bool    m_OuterOutToIn = true;
        private int     m_OuterRange = 24;
        private int     m_OuterWtoB = 2;
        private int     m_OuterGv = 15;
        private int     m_InnerOffset = 2;      // Expected Inner Edge Offset Value
        private bool    m_InnerOutToIn = false;
        private int     m_InnerRange = 24;
        private int     m_InnerWtoB = 2;
        private int     m_InnerGv = 15;
        private int     m_WidthThin = 0;		// Spec. Thin
        private int     m_WidthThick = 6;		// Spec. Thick
        private int     m_WidthLength = 20;     // Spec. Length
     // Uniformity
        private bool    m_UseUniform = false;
        private int     m_FanSize = 8;			// One Split 
        private int     m_GvUniform = 20;       // Gv Differance
        #endregion

        #region < Get/Set Function >
        public int CircleCount
        {
            get { return circleCount; }
            set { circleCount = value; }
        }
        public int gsScan
        {
            get { return m_Scan; }
            set { m_Scan = value; }
        }
        public Point gsRate
        {
            get { return m_Rate; }
            set { m_Rate = value; }
        }
        public void SetRateIn(int InRate)
        {
            m_Rate.X = InRate;
        }
        public void SetRateOut(int OutRate)
        {
            m_Rate.Y = OutRate;
        }
        public PointD gsRadius
        {
            get { return m_Radius; }
            set { m_Radius = value; }
        }
        public double GetRadiusOut()
        {
            return m_Radius.Y;
        }
        public void SetRadiusIn(double dRadius)
        {
            m_Radius.X = dRadius;
        }
        public void SetRadiusOut(double dRadius)
        {
            m_Radius.Y = dRadius;
        }
        public int gsRange
        {
            get { return m_Range; }
            set { m_Range = value; }
        }
        public int gsRadiusTolP
        {
            get { return m_RadiusTolP; }
            set { m_RadiusTolP = value; }
        }
        public int gsRadiusTolM
        {
            get { return m_RadiusTolM; }
            set { m_RadiusTolM = value; }
        }
    // Bumpy
        public bool gsUseBumpy
        {
            get { return m_UseBumpy; }
            set { m_UseBumpy = value; }
        }
        public int gsScanBumpy
        {
            get { return m_ScanBumpy; }
            set { m_ScanBumpy = value; }
        }
        public SearchSide gsBGSide
        {
            get { return m_BGSide; }
            set { m_BGSide = value; }
        }
        public int gsIteration
        {
            get { return m_Iteration; }
            set { m_Iteration = value; }
        }
        public int gsDepth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }
    // Width
        public bool gsUseWidth
        {
            get { return m_UseWidth; }
            set { m_UseWidth = value; }
        }
        public int gsScanWidth
        {
            get { return m_ScanWidth; }
            set { m_ScanWidth = value; }
        }
        public int gsOffsetRadius
        {
            get { return m_OffsetRadius; }
            set { m_OffsetRadius = value; }
        }
        // Width : Outer
        public int gsOuterOffset
        {
            get { return m_OuterOffset; }
            set { m_OuterOffset = value; }
        }
        public bool gsOuterOutToIn
        {
            get { return m_OuterOutToIn; }
            set { m_OuterOutToIn = value; }
        }
        public int gsOuterRange
        {
            get { return m_OuterRange; }
            set { m_OuterRange = value; }
        }
        public int gsOuterWtoB
        {
            get { return m_OuterWtoB; }
            set { m_OuterWtoB = value; }
        }
        public int gsOuterGv
        {
            get { return m_OuterGv; }
            set { m_OuterGv = value; }
        }
        // Width : Inner
        public int gsInnerOffset
        {
            get { return m_InnerOffset; }
            set { m_InnerOffset = value; }
        }
        public bool gsInnerOutToIn
        {
            get { return m_InnerOutToIn; }
            set { m_InnerOutToIn = value; }
        }
        public int gsInnerRange
        {
            get { return m_InnerRange; }
            set { m_InnerRange = value; }
        }
        public int gsInnerWtoB
        {
            get { return m_InnerWtoB; }
            set { m_InnerWtoB = value; }
        }
        public int gsInnerGv
        {
            get { return m_InnerGv; }
            set { m_InnerGv = value; }
        }
        // Width : Spec
        public int gsWidthThin
        {
            get { return m_WidthThin; }
            set { m_WidthThin = value; }
        }
        public int gsWidthThick
        {
            get { return m_WidthThick; }
            set { m_WidthThick = value; }
        }
        public int gsWidthLength
        {
            get { return m_WidthLength; }
            set { m_WidthLength = value; }
        }        
    // Uniformity
        public bool gsUseUniform
        {
            get { return m_UseUniform; }
            set { m_UseUniform = value; }
        }
        public int gsFanSize
        {
            get { return m_FanSize; }
            set { m_FanSize = value; }
        }
        public int gsGvUniform
        {
            get { return m_GvUniform; }
            set { m_GvUniform = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
           if( false == base.Verify(SizeX, SizeY) )
		        return false;
		
	        if( m_Scan <= 0 )
                return false;	

	        if( m_Radius.X <= 0 )
                return false;

	        if( (m_RadiusTolM < 0) || (m_RadiusTolP < 0) )
                return false;

	        if( gsUseBumpy )
	        {
		        if( m_ScanBumpy < 1 )
                    return false;

                if ((m_BGSide < SearchSide.Left) || (m_BGSide > SearchSide.Bottom))
                    return false;

		        if( (m_Iteration < 2) || (m_Depth < 3) )
                    return false;
	        }

            if (gsUseWidth)
            {
                if (m_ScanWidth < 1)
                    return false;
            }

	        if( gsUseUniform )
	        {
		        if( m_FanSize < 2 )
                    return false;

		        if( m_GvUniform < 2 )
                    return false;
	        }
	
	        return true;
        }

        public Rectangle GetOutCircle()
        {
            PointD OutCenter = gsOutPt;
            double OutRadius = GetRadiusOut();
                       
            int L = (int)((OutCenter.X - OutRadius) + 0.5f);
            int T = (int)((OutCenter.Y - OutRadius) + 0.5f);
            int R = (int)((OutCenter.X + OutRadius) + 0.5f);
            int B = (int)((OutCenter.Y + OutRadius) + 0.5f);

            return Rectangle.FromLTRB(L, T, R, B);
        }

        private String GetCircleName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
            // Check Circle
                case 0: return "Scan";
                case 1: return "Rate";
                case 2: return "Radius";
                case 3: return "Range";
                case 4: return "ToleranceP";
                case 5: return "ToleranceM";
            // Check Bumpy
                case 6: return "UseBumpy";
                case 7: return "ScanBumpy";
                case 8: return "BGSide";
                case 9: return "Iteration";
                case 10: return "Depth";
            // Check Width
                case 11: return "UseWidth";
                case 12: return "ScanWidth";
                case 13: return "OffsetRadius";
                case 14: return "OuterOffset";
                case 15: return "OuterOutToIn";
                case 16: return "OuterRange";
                case 17: return "OuterWtoB";
                case 18: return "OuterGv";
                case 19: return "InnerOffset";
                case 20: return "InnerOutToIn";
                case 21: return "InnerRange";
                case 22: return "InnerWtoB";
                case 23: return "InnerGv";
                case 24: return "WidthThin";
                case 25: return "WidthThick";
                case 26: return "WidthLength";
            // Check Uniform
                case 27: return "UseUniform";
                case 28: return "FanSize";
                case 29: return "GvUniform";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetCircle(String strTitle, String strData)
        {
            if (SetEdge(strTitle, strData))
                return true;

            HelpMath helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < CircleCount; i++)
            {
                if (false == strTitle.Equals(GetCircleName(i)))
                    continue;

                switch (i)
                {
                    case 0: gsScan = Convert.ToInt32(strData); break;
                    case 1: m_Rate.X = Convert.ToInt32(strData); break;
                    case 2: m_Radius.X = Convert.ToDouble(strData); break;
                    case 3: gsRange = Convert.ToInt32(strData); break;
                    case 4: gsRadiusTolP = Convert.ToInt32(strData); break;
                    case 5: gsRadiusTolM = Convert.ToInt32(strData); break;
                // Check Bumpy
                    case 6: gsUseBumpy = helper.ToSplit(strData); break;
                    case 7: gsScanBumpy = Convert.ToInt32(strData); break;
                    case 8: gsBGSide = (SearchSide)(Convert.ToInt32(strData)); break;
                    case 9: gsIteration = Convert.ToInt32(strData); break;
                    case 10: gsDepth = Convert.ToInt32(strData); break;
                // Check Width
                    case 11: gsUseWidth = helper.ToSplit(strData); break;
                    case 12: gsScanWidth = Convert.ToInt32(strData); break;
                    case 13: gsOffsetRadius = Convert.ToInt32(strData); break;
                    case 14: gsOuterOffset = Convert.ToInt32(strData); break;
                    case 15: gsOuterOutToIn = helper.ToSplit(strData); break;
                    case 16: gsOuterRange = Convert.ToInt32(strData); break;
                    case 17: gsOuterWtoB = Convert.ToInt32(strData); break;
                    case 18: gsOuterGv = Convert.ToInt32(strData); break;
                    case 19: gsInnerOffset = Convert.ToInt32(strData); break;
                    case 20: gsInnerOutToIn = helper.ToSplit(strData); break; 
                    case 21: gsInnerRange = Convert.ToInt32(strData); break;
                    case 22: gsInnerWtoB = Convert.ToInt32(strData); break;
                    case 23: gsInnerGv = Convert.ToInt32(strData); break;
                    case 24: gsWidthThin = Convert.ToInt32(strData); break;
                    case 25: gsWidthThick = Convert.ToInt32(strData); break;
                    case 26: gsWidthLength = Convert.ToInt32(strData); break;
                // Check Uniform
                    case 27: gsUseUniform = helper.ToSplit(strData); break;
                    case 28: gsFanSize = Convert.ToInt32(strData); break;
                    case 29: gsGvUniform = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }

        public bool GetCircle(int nIndex, ref String strName, ref String strData)
        {
            if( (nIndex < 0) || (nIndex >= CircleCount) )
		        return false;

            strName = GetCircleName(nIndex);
            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", gsScan); break;
                case 1: strData = String.Format("{0}", gsRate.X); break;
                case 2: strData = String.Format("{0}", gsRadius.X); break;
                case 3: strData = String.Format("{0}", gsRange); break;
                case 4: strData = String.Format("{0}", gsRadiusTolP); break;
                case 5: strData = String.Format("{0}", gsRadiusTolM); break;
           // Check Bumpy
                case 6: strData = String.Format("{0}", (gsUseBumpy == true) ? 1 : 0); break;
                case 7: strData = String.Format("{0}", gsScanBumpy); break;
                case 8: strData = String.Format("{0}", gsBGSide); break;
                case 9: strData = String.Format("{0}", gsIteration); break;
                case 10: strData = String.Format("{0}", gsDepth); break;
           // Check Width
                case 11: strData = String.Format("{0}", (gsUseWidth == true) ? 1 : 0); break;
                case 12: strData = String.Format("{0}", gsScanWidth); break;
                case 13: strData = String.Format("{0}", gsOffsetRadius); break;
                case 14: strData = String.Format("{0}", gsOuterOffset); break;
                case 15: strData = String.Format("{0}", gsOuterOutToIn); break;
                case 16: strData = String.Format("{0}", gsOuterRange); break;
                case 17: strData = String.Format("{0}", gsOuterWtoB); break;
                case 18: strData = String.Format("{0}", gsOuterGv); break;
                case 19: strData = String.Format("{0}", gsInnerOffset); break;
                case 20: strData = String.Format("{0}", gsInnerOutToIn); break;
                case 21: strData = String.Format("{0}", gsInnerRange); break;
                case 22: strData = String.Format("{0}", gsInnerWtoB); break;
                case 23: strData = String.Format("{0}", gsInnerGv); break;
                case 24: strData = String.Format("{0}", gsWidthThin); break;
                case 25: strData = String.Format("{0}", gsWidthThick); break;
                case 26: strData = String.Format("{0}", gsWidthLength); break;
           // Check Uniform
                case 27: strData = String.Format("{0}", (gsUseUniform == true) ? 1 : 0); break;
                case 28: strData = String.Format("{0}", gsFanSize); break;
                case 29: strData = String.Format("{0}", gsGvUniform); break;
                default:
                    return false;
            }
                        
            return true;
        }

        public void LoadParam(XmlElement paramElement)
        {
            gsScan = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Scan", gsScan.ToString()));
            m_Rate.X = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Rate", m_Rate.X.ToString()));
            m_Radius.X = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Radius", m_Radius.X.ToString()));
            gsRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Range", gsRange.ToString()));
            gsRadiusTolP = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ToleranceP", gsRadiusTolP.ToString()));
            gsRadiusTolM = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ToleranceM", gsRadiusTolM.ToString()));
            gsUseBumpy = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseBumpy", gsUseBumpy.ToString()));
            gsScanBumpy = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ScanBumpy", gsScanBumpy.ToString()));
            gsBGSide = (SearchSide)Enum.Parse(typeof(SearchSide), XmlHelper.GetValue(paramElement, "BGSide", gsBGSide.ToString()));
            gsIteration = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Iteration", gsIteration.ToString()));
            gsDepth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Depth", gsDepth.ToString()));
            gsUseWidth = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseWidth", gsUseWidth.ToString()));
            gsScanWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ScanWidth", gsScanWidth.ToString()));
            gsOffsetRadius = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRadius", gsOffsetRadius.ToString()));
            gsOuterOffset = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterOffset", gsOuterOffset.ToString()));
            gsOuterOutToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "OuterOutToIn", gsOuterOutToIn.ToString()));
            gsOuterRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterRange", gsOuterRange.ToString()));
            gsOuterWtoB = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterWtoB", gsOuterWtoB.ToString()));
            gsOuterGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OuterGv", gsOuterGv.ToString()));
            gsInnerOffset = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerOffset", gsInnerOffset.ToString()));
            gsInnerOutToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "InnerOutToIn", gsInnerOutToIn.ToString()));
            gsInnerRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerRange", gsInnerRange.ToString()));
            gsInnerWtoB = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerWtoB", gsInnerWtoB.ToString()));
            gsInnerGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "InnerGv", gsInnerGv.ToString()));
            gsWidthThin = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthThin", gsWidthThin.ToString()));
            gsWidthThick = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthThick", gsWidthThick.ToString()));
            gsWidthLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WidthLength", gsWidthLength.ToString()));
            gsUseUniform = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseUniform", gsUseUniform.ToString()));
            gsFanSize = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FanSize", gsFanSize.ToString()));
            gsGvUniform = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvUniform", gsGvUniform.ToString()));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Scan", gsScan.ToString());
            XmlHelper.SetValue(paramElement, "Rate", m_Rate.X.ToString());
            XmlHelper.SetValue(paramElement, "Radius", m_Radius.X.ToString());
            XmlHelper.SetValue(paramElement, "Range", gsRange.ToString());
            XmlHelper.SetValue(paramElement, "ToleranceP", gsRadiusTolP.ToString());
            XmlHelper.SetValue(paramElement, "ToleranceM", gsRadiusTolM.ToString());
            XmlHelper.SetValue(paramElement, "UseBumpy", gsUseBumpy.ToString());
            XmlHelper.SetValue(paramElement, "ScanBumpy", gsScanBumpy.ToString());
            XmlHelper.SetValue(paramElement, "BGSide", gsBGSide.ToString());
            XmlHelper.SetValue(paramElement, "Iteration", gsIteration.ToString());
            XmlHelper.SetValue(paramElement, "Depth", gsDepth.ToString());
            XmlHelper.SetValue(paramElement, "UseWidth", gsUseWidth.ToString());
            XmlHelper.SetValue(paramElement, "ScanWidth", gsScanWidth.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRadius", gsOffsetRadius.ToString());
            XmlHelper.SetValue(paramElement, "OuterOffset", gsOuterOffset.ToString());
            XmlHelper.SetValue(paramElement, "OuterOutToIn", gsOuterOutToIn.ToString());
            XmlHelper.SetValue(paramElement, "OuterRange", gsOuterRange.ToString());
            XmlHelper.SetValue(paramElement, "OuterWtoB", gsOuterWtoB.ToString());
            XmlHelper.SetValue(paramElement, "OuterGv", gsOuterGv.ToString());
            XmlHelper.SetValue(paramElement, "InnerOffset", gsInnerOffset.ToString());
            XmlHelper.SetValue(paramElement, "InnerOutToIn", gsInnerOutToIn.ToString());
            XmlHelper.SetValue(paramElement, "InnerRange", gsInnerRange.ToString());
            XmlHelper.SetValue(paramElement, "InnerWtoB", gsInnerWtoB.ToString());
            XmlHelper.SetValue(paramElement, "InnerGv", gsInnerGv.ToString());
            XmlHelper.SetValue(paramElement, "WidthThin", gsWidthThin.ToString());
            XmlHelper.SetValue(paramElement, "WidthThick", gsWidthThick.ToString());
            XmlHelper.SetValue(paramElement, "WidthLength", gsWidthLength.ToString());
            XmlHelper.SetValue(paramElement, "UseUniform", gsUseUniform.ToString());
            XmlHelper.SetValue(paramElement, "FanSize", gsFanSize.ToString());
            XmlHelper.SetValue(paramElement, "GvUniform", gsGvUniform.ToString());
        }

        public MsrCircle Clone()
        {
            MsrCircle cloneCircle = new MsrCircle();
            cloneCircle.gsScan = gsScan;
            cloneCircle.m_Rate.X = m_Rate.X;
            cloneCircle.m_Radius.X = m_Radius.X;
            cloneCircle.gsRange = gsRange;
            cloneCircle.gsRadiusTolP = gsRadiusTolP;
            cloneCircle.gsRadiusTolM = gsRadiusTolM;
            cloneCircle.gsUseBumpy = gsUseBumpy;
            cloneCircle.gsScanBumpy = gsScanBumpy;
            cloneCircle.gsBGSide = gsBGSide;
            cloneCircle.gsIteration = gsIteration;
            cloneCircle.gsDepth = gsDepth;
            cloneCircle.gsUseWidth = gsUseWidth;
            cloneCircle.gsScanWidth = gsScanWidth;
            cloneCircle.gsOffsetRadius = gsOffsetRadius;
            cloneCircle.gsOuterOffset = gsOuterOffset;
            cloneCircle.gsOuterOutToIn = gsOuterOutToIn;
            cloneCircle.gsOuterRange = gsOuterRange;
            cloneCircle.gsOuterWtoB = gsOuterWtoB;
            cloneCircle.gsOuterGv = gsOuterGv;
            cloneCircle.gsInnerOffset = gsInnerOffset;
            cloneCircle.gsInnerOutToIn = gsInnerOutToIn;
            cloneCircle.gsInnerRange = gsInnerRange;
            cloneCircle.gsInnerWtoB = gsInnerWtoB;
            cloneCircle.gsInnerGv = gsInnerGv;
            cloneCircle.gsWidthThin = gsWidthThin;
            cloneCircle.gsWidthThick = gsWidthThick;
            cloneCircle.gsWidthLength = gsWidthLength;
            cloneCircle.gsUseUniform = gsUseUniform;
            cloneCircle.gsFanSize = gsFanSize;
            cloneCircle.gsGvUniform = gsGvUniform;

            return cloneCircle;
        }
        #endregion
    }

    public class MsrBlob : Msr
    {
        #region < Constructor >
        public MsrBlob()
        {
        }
        #endregion

        #region < Variable and Default >
        private int m_nBlobCount = 9;   // Blob Parameter Count       
        private int m_Scan = 1;		    // Sampling
        private int m_BgAvgGv = 128;	
        private int m_GvLow = 0;			
        private int m_GvHigh = 200;
        private int m_Linking = 3;
        private int m_SizeLow = 5;
        private int m_SizeHigh = 5;
        private int m_RadiusInner = 0;  // Circle Blob From IsRoi Center
        private int m_RadiusOuter = 0;  // Circle Blob From IsRoi Center      
        #endregion

        #region < Get/Set Function >
        public int gsBlobCount
        {
            get { return m_nBlobCount; }
            set { m_nBlobCount = value; }
        }
        public int gsScan
        {
            get { return m_Scan; }
            set { m_Scan = value; }
        }
        public int gsBgAvgGv
        {
            get { return m_BgAvgGv; }
            set { m_BgAvgGv = value; }
        }        
        public int gsGvLow
        {
            get { return m_GvLow; }
            set { m_GvLow = value; }
        }
        public int gsGvHigh
        {
            get { return m_GvHigh; }
            set { m_GvHigh = value; }
        }
        public int gsLinking
        {
            get { return m_Linking; }
            set { m_Linking = value; }
        }
        public int gsSizeLow
        {
            get { return m_SizeLow; }
            set { m_SizeLow = value; }
        }
        public int gsSizeHigh
        {
            get { return m_SizeHigh; }
            set { m_SizeHigh = value; }
        }
        public int gsRadiusInner
        {
            get { return m_RadiusInner; }
            set { m_RadiusInner = value; }
        }
        public int gsRadiusOuter
        {
            get { return m_RadiusOuter; }
            set { m_RadiusOuter = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
                return false;

            if (gsUse)
            {
                if (gsScan <= 0)
                    return false;

                if ((gsGvLow < 0) || (gsGvHigh > 255))
                    return false;

                if (gsGvLow > gsGvHigh)
                    return false;

                if (gsLinking <= 0)
                    return false;

                if ((gsSizeLow  < 1))
                    return false;

                if ((gsSizeHigh < 1))
                    return false;
            }

            return true;
        }

        private String GetBlobName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Scan";
                case 1: return "BgAvgGv";
                case 2: return "GvLow";
                case 3: return "GvHigh";
                case 4: return "Linking";
                case 5: return "SizeLow";
                case 6: return "SizeHigh";
                case 7: return "RadiusInner";
                case 8: return "RadiusOuter";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetBlob(String strTitle, String strData)
        {
            if (SetMsr(strTitle, strData))
                return true;

            bool bEntry = false;
            for (int i = 0; i < gsBlobCount; i++)
            {
                if (false == strTitle.Equals(GetBlobName(i)))
                    continue;

                switch (i)
                {
                    case 0: gsScan = Convert.ToInt32(strData); break;
                    case 1: gsBgAvgGv = Convert.ToInt32(strData); break;
                    case 2: gsGvLow = Convert.ToInt32(strData); break;
                    case 3: gsGvHigh = Convert.ToInt32(strData); break;
                    case 4: gsLinking = Convert.ToInt32(strData); break;
                    case 5: gsSizeLow = Convert.ToInt32(strData); break;
                    case 6: gsSizeHigh = Convert.ToInt32(strData); break;
                    case 7: gsRadiusInner = Convert.ToInt32(strData); break;
                    case 8: gsRadiusOuter = Convert.ToInt32(strData); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }

        public bool GetBlob(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsBlobCount))
                return false;

            strName = GetBlobName(nIndex);
            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", gsScan); break;
                case 1: strData = String.Format("{0}", gsBgAvgGv); break;
                case 2: strData = String.Format("{0}", gsGvLow); break;
                case 3: strData = String.Format("{0}", gsGvHigh); break;
                case 4: strData = String.Format("{0}", gsLinking); break;
                case 5: strData = String.Format("{0}", gsSizeLow); break;
                case 6: strData = String.Format("{0}", gsSizeHigh); break;
                case 7: strData = String.Format("{0}", gsRadiusInner); break;
                case 8: strData = String.Format("{0}", gsRadiusOuter); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion

        public void LoadParam(XmlElement paramElement)
        {
            gsScan = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Scan", gsScan.ToString()));
            gsBgAvgGv = Convert.ToInt32(XmlHelper.GetValue(paramElement, "BgAvgGv", gsBgAvgGv.ToString()));
            gsGvLow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvLow", gsGvLow.ToString()));
            gsGvHigh = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GvHigh", gsGvHigh.ToString()));
            gsLinking = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Linking", gsLinking.ToString()));
            gsSizeLow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SizeLow", gsSizeLow.ToString()));
            gsSizeHigh = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SizeHigh", gsSizeHigh.ToString()));
            gsRadiusInner = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RadiusInner", gsRadiusInner.ToString()));
            gsRadiusOuter = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RadiusOuter", gsRadiusOuter.ToString()));
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Scan", gsScan.ToString());
            XmlHelper.SetValue(paramElement, "BgAvgGv", gsBgAvgGv.ToString());
            XmlHelper.SetValue(paramElement, "GvLow", gsGvLow.ToString());
            XmlHelper.SetValue(paramElement, "GvHigh", gsGvHigh.ToString());
            XmlHelper.SetValue(paramElement, "Linking", gsLinking.ToString());
            XmlHelper.SetValue(paramElement, "SizeLow", gsSizeLow.ToString());
            XmlHelper.SetValue(paramElement, "SizeHigh", gsSizeHigh.ToString());
            XmlHelper.SetValue(paramElement, "RadiusInner", gsRadiusInner.ToString());
            XmlHelper.SetValue(paramElement, "RadiusOuter", gsRadiusOuter.ToString());
        }

        public MsrBlob Clone()
        {
            MsrBlob cloneBlob = new MsrBlob();
            cloneBlob.gsScan = gsScan;
            cloneBlob.gsBgAvgGv = gsBgAvgGv;
            cloneBlob.gsGvLow = gsGvLow;
            cloneBlob.gsGvHigh = gsGvHigh;
            cloneBlob.gsLinking = gsLinking;
            cloneBlob.gsSizeLow = gsSizeLow;
            cloneBlob.gsSizeHigh = gsSizeHigh;
            cloneBlob.gsRadiusInner = gsRadiusInner;
            cloneBlob.gsRadiusOuter = gsRadiusOuter;

            return cloneBlob;
        }
    }

    public class MsrMatch : Msr
    {
        #region < Constructor >
        public MsrMatch()
        {          
        }
        #endregion

        #region < Variable and Default >
        private int     m_nMatchCount = 3;        		
        private PointD  m_Score = new PointD(60, 0);	    // Score : (x,y) = (In, Out)	
        private Point   m_MatchEx = new Point(20, 20);
        private PointD  m_Angle = new PointD(0, 0);		    // Angle : (x,y) = (In, Out)        
        OpenCvGreyImage m_Templete = new OpenCvGreyImage();
        private string  m_TempletePath;
        #endregion

        #region < Get/Set Function >
        public int gsMatchCount
        {
            get { return m_nMatchCount; }
            set { m_nMatchCount = value; }
        }
        public PointD gsScore
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
        public double GetScoreIn()
        {
            return m_Score.X;
        }
        public void SetScoreIn(double dScore)
        {
            m_Score.X = dScore;
        }
        public void SetScoreOut(double dScore)
        {
            m_Score.Y = dScore;
        }
        public Point gsMatchEx
        {
            get { return m_MatchEx; }
            set { m_MatchEx = value; }
        }        
        public PointD gsAngle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }
        public double GetAngleIn()
        {
            return m_Angle.X;
        }
        public void SetAngleIn(double dAngle)
        {
            m_Angle.X = dAngle;
        }
        public OpenCvGreyImage gsTemplete
        {
            get { return m_Templete; }
            set { m_Templete = value; }
        }
        public string gsTempletePath
        {
            get { return m_TempletePath; }
            set { m_TempletePath = value; }
        }
        #endregion

        #region < Member Function >
        public override bool Verify(int SizeX, int SizeY)
        {
            if (false == base.Verify(SizeX, SizeY))
                return false;

            if (gsUse)
            {
                if ((gsScore.X < 30) || (gsScore.X >= 100))
                    return false;

                if ((gsMatchEx.X < 1) || (gsMatchEx.Y < 1))
                    return false;

                if ((gsMatchEx.X > SizeX) || (gsMatchEx.Y > SizeY))
                    return false;
            }

            return true;
        }

        private String GetMatchName(int nIndex)
        {
            String strTitle = "Unknown";

            switch (nIndex)
            {
                case 0: return "Score";
                case 1: return "MatchEx";
                case 2: return "Angle";
                default:
                    break;
            }

            return strTitle;
        }

        public bool SetMatch(String strTitle, String strData)
        {
            if (SetMsr(strTitle, strData))
                return true;

            HelpMath helper = new HelpMath();

            bool bEntry = false;
            for (int i = 0; i < gsMatchCount; i++)
            {
                if (false == strTitle.Equals(GetMatchName(i)))
                    continue;

                switch (i)
                {
                    case 0: SetScoreIn(Convert.ToDouble(strData)); break;
                    case 1: helper.ToSplit(strData, ref m_MatchEx); break;
                    case 2: SetAngleIn(Convert.ToDouble(strData)); break;
                    default:
                        break;
                }

                bEntry = true; break;
            }
            if (!bEntry)
                return false;

            return true;
        }

        public bool GetMatch(int nIndex, ref String strName, ref String strData)
        {
            if ((nIndex < 0) || (nIndex >= gsMatchCount))
                return false;

            strName = GetMatchName(nIndex);
            switch (nIndex)
            {
                case 0: strData = String.Format("{0}", gsScore.X); break;
                case 1: strData = String.Format("{0}, {1}", gsMatchEx.X, gsMatchEx.Y); break;
                case 2: strData = String.Format("{0}", gsAngle.X); break;
                default:
                    return false;
            }

            return true;
        }
        #endregion
    }
}
