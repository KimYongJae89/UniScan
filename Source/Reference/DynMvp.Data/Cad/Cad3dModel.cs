using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DynMvp.Cad
{
    public class Triangle
    {
        Point3d[] vertex = new Point3d[3];
        public Point3d[] Vertex
        {
            get { return vertex;  }
        }

        Point3d normalVector;
        public Point3d NormalVector
        {
            get { return normalVector; }
            set { normalVector = value; }
        }

        UInt32 attribute;
        public UInt32 Attribute
        {
            get { return attribute; }
            set { attribute = value; }
        }
    }

    public class Cad3dModel
    {
        List<Triangle> triangleList = new List<Triangle>();
        public List<Triangle> TriangleList
        {
            get { return triangleList; }
            set { triangleList = value; }
        }

        Point3d centerPt;
        public Point3d CenterPt
        {
            get { return centerPt; }
        }

        SizeF size;
        public SizeF Size
        {
            get { return size; }
        }

        float minValue;
        public float MinValue
        {
            get { return minValue; }
        }

        float maxValue;
        public float MaxValue
        {
            get { return maxValue; }
        }

        internal void AddTriangle(Triangle triangle)
        {
            triangleList.Add(triangle);
        }

        public void UpdateData()
        {
            float minX = float.MaxValue, minY = float.MaxValue, minZ = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue, maxZ = float.MinValue;

            foreach (Triangle triangle in triangleList)
            {
                Point3d[] vertex = triangle.Vertex;
                minX = (float)Math.Min(minX, Math.Min(Math.Min(vertex[0].X, vertex[1].X), vertex[2].X));
                minY = (float)Math.Min(minY, Math.Min(Math.Min(vertex[0].Y, vertex[1].Y), vertex[2].Y));
                minZ = (float)Math.Min(minZ, Math.Min(Math.Min(vertex[0].Z, vertex[1].Z), vertex[2].Z));
                maxX = (float)Math.Max(maxX, Math.Max(Math.Max(vertex[0].X, vertex[1].X), vertex[2].X));
                maxY = (float)Math.Max(maxY, Math.Max(Math.Max(vertex[0].Y, vertex[1].Y), vertex[2].Y));
                maxZ = (float)Math.Max(maxZ, Math.Max(Math.Max(vertex[0].Z, vertex[1].Z), vertex[2].Z));
            }

            size.Width = (maxX - minX);
            size.Height = (maxY - minY);
            minValue = minZ;
            maxValue = maxZ;
            centerPt = new Point3d((float)(maxX + minX) / 2, (float)(maxY + minY) / 2, (float)(maxZ + minZ) / 2);
        }
    }
}
