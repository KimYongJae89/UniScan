using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniScan.Data;

namespace UniScan.UI
{
    public interface IResultPanel
    {
        void Initialize(ResultPanelInfo resultPanelInfo);
        void AddScanData(ScanData scanData);
        void AddValue(float position, float sheetThickness, float petThickness);
        void DisplayResult();
    }

    // 패널종류추가
    public enum PanelType
    {
        Profile, Trend, Overlay
    }

    public enum ValueType
    {
        Sheet, PET
    }

    public class ResultPanelInfo
    {
        PanelType panelType;
        public PanelType PanelType
        {
            get { return panelType; }
            set { panelType = value; }
        }

        ValueType valueType;
        public ValueType ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }

        public virtual ResultPanelInfo Clone()
        {
            ResultPanelInfo resultPanelInfo = new ResultPanelInfo();

            resultPanelInfo.panelType = this.panelType;
            resultPanelInfo.valueType = this.valueType;

            return resultPanelInfo;
        }

        public virtual void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("PanelType", panelType.ToString());
            xmlElement.SetAttribute("ValueType", valueType.ToString());
        }

        public virtual void Load(XmlElement xmlElement)
        {
            panelType = (PanelType)Enum.Parse(typeof(PanelType), xmlElement.GetAttribute("PanelType"));
            valueType = (ValueType)Enum.Parse(typeof(ValueType), xmlElement.GetAttribute("ValueType"));
        }
    }

    public class ResultPanelInfoFactory
    {
        public static ResultPanelInfo Create(PanelType panelType)
        {
            ResultPanelInfo resultPanelInfo = null;

            // 패널종류추가
            switch (panelType)
            {
                case PanelType.Profile:
                    resultPanelInfo = new ProfilePanel.Info();
                    break;
                case PanelType.Trend:
                    resultPanelInfo = new TrendPanel.Info();
                    break;
                case PanelType.Overlay:
                    resultPanelInfo = new OverlayPanel.Info();
                    break;
            }

            return resultPanelInfo;
        }
    }

    public class ResultPanelFactory
    {
        public static IResultPanel Create(PanelType panelType)
        {
            IResultPanel resultPanel = null;

            // 패널종류추가
            switch (panelType)
            {
                case PanelType.Profile:
                    {
                        ProfilePanel profilePanel = new ProfilePanel();

                        profilePanel.Name = "profilePanel";
                        profilePanel.TabIndex = 0;
                        profilePanel.Dock = DockStyle.Fill;

                        resultPanel = profilePanel;
                    }
                    break;

                case PanelType.Trend:
                    {
                        TrendPanel trendPanel = new TrendPanel();

                        trendPanel.Name = "trendPanel";
                        trendPanel.TabIndex = 0;
                        trendPanel.Dock = DockStyle.Fill;

                        resultPanel = trendPanel;
                    }
                    break;

                case PanelType.Overlay:
                    {
                        OverlayPanel overlayPanel = new OverlayPanel();

                        overlayPanel.Name = "profilePanel";
                        overlayPanel.TabIndex = 0;
                        overlayPanel.Dock = DockStyle.Fill;

                        resultPanel = overlayPanel;
                    }
                    break;
            }

            return resultPanel;
        }
    }
}
