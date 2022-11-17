using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;

namespace UniScanM.Data
{
    public abstract class InspectParam : IExportable, IImportable
    {
        public abstract void Export(XmlElement element, string subKey = null);
        public abstract void Import(XmlElement element, string subKey = null);
    }

    public class Model: DynMvp.Data.Model
    {
        public string PlcModel { get; set; }

        public Model()
        {

        }

        public new ModelDescription ModelDescription
        {
            set { modelDescription = value; }
            get { return (ModelDescription)modelDescription; }
        }

        protected InspectParam inspectParam;
        public InspectParam InspectParam
        {
            get { return inspectParam; }
        }

        public override bool IsTaught()
        {
            return inspectParam != null;
        }
        
        public override void SaveModel(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "PlcModel", this.PlcModel);
            inspectParam?.Export(xmlElement, "InspectParam");
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            this.PlcModel = XmlHelper.GetValue(xmlElement, "PlcModel", "");
            inspectParam?.Import(xmlElement, "InspectParam");
        }
    }
}
