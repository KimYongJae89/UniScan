using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScan.Data
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        AxisPosition scanStartPos = new AxisPosition(1);
        AxisPosition scanEndPos = new AxisPosition(1);

        AxisPosition referencePos = new AxisPosition(1);
        AxisPosition backGroundPos = new AxisPosition(1);

        float standardThickness;
        float upperControlLimit;
        float upperControlWarning;
        float lowerControlWarning;
        float lowerControlLimit;

        public AxisPosition ScanStartPos
        {
            get { return scanStartPos; }
            set { scanStartPos = value; }
        }
        public AxisPosition ScanEndPos
        {
            get { return scanEndPos; }
            set { scanEndPos = value; }
        }
        public AxisPosition ReferencePos
        {
            get { return referencePos; }
            set { referencePos = value; }
        }
        public AxisPosition BackGroundPos
        {
            get { return backGroundPos; }
            set { backGroundPos = value; }
        }

        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription modelDescription = new ModelDescription();

            modelDescription.Copy(this);

            return modelDescription;
        }

        public virtual void Copy(ModelDescription srcDesc)
        {
            base.Copy(srcDesc);

            scanStartPos = srcDesc.scanStartPos.Clone();
            scanEndPos = srcDesc.scanEndPos.Clone();
            referencePos = srcDesc.referencePos.Clone();
            backGroundPos = srcDesc.backGroundPos.Clone();

            standardThickness = srcDesc.standardThickness;
            upperControlLimit = srcDesc.upperControlLimit;
            upperControlWarning = srcDesc.upperControlWarning;
            lowerControlWarning = srcDesc.lowerControlWarning;
            lowerControlLimit = srcDesc.lowerControlLimit;
        }

        // Spectrometer

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);

            XmlElement controlValueElement = modelDescElement.OwnerDocument.CreateElement("ControlValue");
            modelDescElement.AppendChild(controlValueElement);

            XmlHelper.SetAttributeValue(controlValueElement, "StandardThickness", standardThickness.ToString());
            XmlHelper.SetAttributeValue(controlValueElement, "UpperControlLimit", upperControlLimit.ToString());
            XmlHelper.SetAttributeValue(controlValueElement, "UpperControlWarning", upperControlWarning.ToString());
            XmlHelper.SetAttributeValue(controlValueElement, "LowerControlWarning", lowerControlWarning.ToString());
            XmlHelper.SetAttributeValue(controlValueElement, "LowerControlLimit", lowerControlLimit.ToString());

            XmlElement scanValueElement = modelDescElement.OwnerDocument.CreateElement("ScanValue");
            modelDescElement.AppendChild(scanValueElement);

            if (scanStartPos != null)
                XmlHelper.SetAttributeValue(scanValueElement, "ScanStartPos", scanStartPos.Position[0].ToString());
            else
                XmlHelper.SetAttributeValue(scanValueElement, "ScanStartPos", 0.ToString());

            if (scanEndPos != null)
                XmlHelper.SetAttributeValue(scanValueElement, "ScanEndPos", scanEndPos.Position[0].ToString());
            else
                XmlHelper.SetAttributeValue(scanValueElement, "ScanEndPos", 0.ToString());

            if (ReferencePos != null)
                XmlHelper.SetAttributeValue(scanValueElement, "ReferencePos", ReferencePos.Position[0].ToString());
            else
                XmlHelper.SetAttributeValue(scanValueElement, "ReferencePos", 0.ToString());

            if (BackGroundPos != null)
                XmlHelper.SetAttributeValue(scanValueElement, "BackGroundPos", BackGroundPos.Position[0].ToString());
            else
                XmlHelper.SetAttributeValue(scanValueElement, "BackGroundPos", 0.ToString());

            XmlElement spectrometerParamElement = modelDescElement.OwnerDocument.CreateElement("SpectrometerParam");
            modelDescElement.AppendChild(spectrometerParamElement);
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            XmlElement controlValueElement = modelDescElement["ControlValue"];
            if (controlValueElement != null)
            {
                standardThickness = Convert.ToSingle(XmlHelper.GetAttributeValue(modelDescElement, "StandardThickness", "0"));
                upperControlLimit = Convert.ToSingle(XmlHelper.GetAttributeValue(modelDescElement, "UpperControlLimit", "0"));
                upperControlWarning = Convert.ToSingle(XmlHelper.GetAttributeValue(modelDescElement, "UpperControlWarning", "0"));
                lowerControlWarning = Convert.ToSingle(XmlHelper.GetAttributeValue(modelDescElement, "LowerControlWarning", "0"));
                lowerControlLimit = Convert.ToSingle(XmlHelper.GetAttributeValue(modelDescElement, "LowerControlLimit", "0"));
            }

            XmlElement scanValueElement = modelDescElement["ScanValue"];
            if (scanValueElement != null)
            {
                scanStartPos.Position[0] = Convert.ToSingle(XmlHelper.GetAttributeValue(scanValueElement, "ScanStartPos", "0"));
                scanEndPos.Position[0] = Convert.ToSingle(XmlHelper.GetAttributeValue(scanValueElement, "ScanEndPos", "0"));
                ReferencePos.Position[0] = Convert.ToSingle(XmlHelper.GetAttributeValue(scanValueElement, "ReferencePos", "0"));
                BackGroundPos.Position[0] = Convert.ToSingle(XmlHelper.GetAttributeValue(scanValueElement, "BackGroundPos", "0"));
            }
        }
    }
}
