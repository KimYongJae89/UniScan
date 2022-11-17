using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Drawing;
using System.Xml;

namespace UniScanG.Data.Vision
{
    public abstract class RegionInfo
    {
        protected bool use = true;
        public bool Use
        {
            get { return this.use; }
            set { this.use = value; }
        }

        protected Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        protected Rectangle inflateSize;
        public Rectangle InflateSize
        {
            get { return inflateSize; }
            set { inflateSize = value; }
        }

        public RegionInfo()
        {
            this.use = false;
            this.region = Rectangle.Empty;
        }

        public RegionInfo(Rectangle region, Rectangle inflateSize)
        {
            this.use = true;
            this.region = region;
            this.inflateSize = inflateSize;
        }

        public abstract RegionInfo Clone();

        public virtual void Copy(RegionInfo srcRegionInfo)
        {
            this.use = srcRegionInfo.use;
            this.region = srcRegionInfo.region;
            this.inflateSize = srcRegionInfo.inflateSize;
        }
    
        public virtual void Dispose() { }

        public void SaveParam(XmlElement algorithmElement, string key)
        {
            XmlElement xmlElement = algorithmElement.OwnerDocument.CreateElement(key);
            algorithmElement.AppendChild(xmlElement);
            SaveParam(xmlElement);
        }

        public virtual void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "Use", use.ToString());
            XmlHelper.SetValue(algorithmElement, "Region", region);
            //XmlHelper.SetValue(algorithmElement, "X", region.X);
            //XmlHelper.SetValue(algorithmElement, "Y", region.Y);
            //XmlHelper.SetValue(algorithmElement, "Width", region.Width);
            //XmlHelper.SetValue(algorithmElement, "Height", region.Height);
            XmlHelper.SetValue(algorithmElement, "InflateSize", this.inflateSize);
        }

        public void LoadParam(XmlElement algorithmElement, string key)
        {
            XmlElement xmlElement=  algorithmElement[key];
            if (xmlElement == null)
                return;

            LoadParam(xmlElement);
        }

        public virtual void LoadParam(XmlElement algorithmElement)
        {
            use = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "Use", use.ToString()));
            this.region = XmlHelper.GetValue(algorithmElement, "Region", this.region);
            this.inflateSize = XmlHelper.GetValue(algorithmElement, "InflateSize", this.inflateSize);
            if (this.inflateSize.IsEmpty)
            {
                Size inflatedSize = XmlHelper.GetValue(algorithmElement, "InflatedSize", Size.Empty);
                this.inflateSize = Rectangle.FromLTRB(inflatedSize.Width, inflatedSize.Height, inflatedSize.Width, inflatedSize.Height);
            }

            //int x = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "X", "0"));
            //int y = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Y", "0"));
            //int width = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Width", "0"));
            //int height = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Height", "0"));
            //region = new Rectangle(x, y, width, height);
        }

        public Figure GetFigure()
        {
            return GetFigure(DynMvp.Authentication.UserHandler.Instance().CurrentUser.IsMasterAccount);
        }

        public abstract Figure GetFigure(bool drawDetail);
    }
}
