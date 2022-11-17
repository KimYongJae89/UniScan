using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Drawing;
using System.Xml;

namespace UniScanS.Screen.Data
{
    public class RegionInfo
    {
        Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        int meanValue;
        public int MeanValue
        {
            get { return meanValue; }
            set { meanValue = value; }
        }

        int poleValue;
        public int PoleValue
        {
            get { return poleValue; }
            set { poleValue = value; }
        }

        int dielectricValue;
        public int DielectricValue
        {
            get { return dielectricValue; }
            set { dielectricValue = value; }
        }

        public RegionInfo Clone()
        {
            RegionInfo clone = new RegionInfo();

            clone.Copy(this);

            return clone;
        }

        public void Copy(RegionInfo srcRegionInfo)
        {
            this.region = srcRegionInfo.region;
            this.meanValue = srcRegionInfo.meanValue;
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "X", region.X.ToString());
            XmlHelper.SetValue(algorithmElement, "Y", region.Y.ToString());
            XmlHelper.SetValue(algorithmElement, "Width", region.Width.ToString());
            XmlHelper.SetValue(algorithmElement, "Height", region.Height.ToString());
            XmlHelper.SetValue(algorithmElement, "MeanValue", meanValue.ToString());
            XmlHelper.SetValue(algorithmElement, "PoleValue", poleValue.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricValue", dielectricValue.ToString());
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            int x = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "X", "0"));
            int y = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Y", "0"));
            int width = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Width", "0"));
            int height = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Height", "0"));

            region = new Rectangle(x, y, width, height);
            meanValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MeanValue", "0"));
            poleValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "PoleValue", "0"));
            dielectricValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DielectricValue", "0"));
        }

        public Figure GetFigure()
        {
            return new RectangleFigure(region, new Pen(Color.LightBlue, 1), new SolidBrush(Color.FromArgb(127, Color.LightBlue)));
        }
    }
}
