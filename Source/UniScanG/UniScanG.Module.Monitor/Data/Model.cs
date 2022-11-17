using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
using DynMvp.UI;

namespace UniScanG.Module.Monitor.Data
{
    public class Model : UniScanG.Data.Model.Model
    {
        public TeachData TeachData => this.teachData;
        TeachData teachData;

        public override bool IsTaught()
        {
            return true;
        }

        public Model() : base()
        {
            this.teachData = new TeachData();
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            teachData.Save(xmlElement, "TeachData");
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            teachData.Load(xmlElement, "TeachData");
        }
    }

    public class ModelManager : UniScanG.Data.Model.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new Model();
        }
    }
}
