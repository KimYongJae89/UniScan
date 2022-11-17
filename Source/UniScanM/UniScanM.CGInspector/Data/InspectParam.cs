using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.CGInspector.Data
{
    public class InspectParam : UniScanM.Data.InspectParam, IExportable, IImportable
    {
        public InspectParam()
        {
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;
        }

        public override void Export(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }
        }

        public static InspectParam Load(XmlElement paramElement, string subKey = null)
        {
            InspectParam param = new InspectParam();
            param.Import(paramElement, subKey);
            return param;
        }
    }
}
