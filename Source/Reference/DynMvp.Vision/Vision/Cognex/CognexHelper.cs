using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Cognex.VisionPro;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision.Cognex
{
    public class CognexHelper
    {
        public static List<PointF> Convert(CogPolygon polygon)
        {
            List<PointF> pointList = new List<PointF>();
            for (int i = 0; i < polygon.GetVertices().GetLength(0); i++)
            {
                pointList.Add(new PointF((float)polygon.GetVertexX(i), (float)polygon.GetVertexY(i)));
            }

            return pointList;
        }

        public static bool LicenseExist(string subLibraryType)
        {
            return false;
            //return CogMisc.IsLicensedFeatureEnabled(subLibraryType);
        }
    }
}
