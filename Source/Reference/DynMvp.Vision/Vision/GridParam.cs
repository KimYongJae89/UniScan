using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class GridParam
    {
        bool useGrid;
        public bool UseGrid
        {
            get { return useGrid; }
            set { useGrid = value; }
        }

        int rowCount = 2;
        public int RowCount
        {
            get { return rowCount; }
            set { rowCount = value; }
        }

        int columnCount = 2;
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; }
        }

        int acceptanceScore;
        public int AcceptanceScore
        {
            get { return acceptanceScore; }
            set { acceptanceScore = value; }
        }

        SegmentCalcType calcType;
        public SegmentCalcType CalcType
        {
            get { return calcType; }
            set { calcType = value; }
        }

        public GridParam Clone()
        {
            GridParam param = new GridParam();

            param.Copy(this);

            return param;
        }

        public void Copy(GridParam gridParam)
        {
            useGrid = gridParam.UseGrid;
            rowCount = gridParam.RowCount;
            columnCount = gridParam.columnCount;
            acceptanceScore = gridParam.acceptanceScore;
            calcType = gridParam.calcType;
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            useGrid = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseGrid", "False"));
            rowCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RowCount", "1"));
            columnCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ColumnCount", "1"));
            acceptanceScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "AcceptanceScore", "1"));
            calcType = (SegmentCalcType)Enum.Parse(typeof(SegmentCalcType), XmlHelper.GetValue(algorithmElement, "CalcType", "Ratio"));
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "UseGrid", useGrid.ToString());
            XmlHelper.SetValue(algorithmElement, "RowCount", rowCount.ToString());
            XmlHelper.SetValue(algorithmElement, "ColumnCount", columnCount.ToString());
            XmlHelper.SetValue(algorithmElement, "AcceptanceScore", acceptanceScore.ToString());
            XmlHelper.SetValue(algorithmElement, "CalcType", calcType.ToString());
        }

        public int GetNumCol()
        {
            if (useGrid)
                return columnCount;

            return 1;
        }

        public int GetNumRow()
        {
            if (useGrid)
                return rowCount;

            return 1;
        }
    }
}
