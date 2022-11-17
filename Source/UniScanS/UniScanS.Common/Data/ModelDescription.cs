using DynMvp.Base;
using System;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanS.Common.Data
{
    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        bool isTrained;
        public bool IsTrained
        {
            get { return isTrained; }
            set { isTrained = value; }
        }

        public override void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "IsTrained", isTrained.ToString());

            base.Save(modelDescElement);
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            isTrained = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "IsTrained", "false"));
        }

        public virtual string[] GetArgs()
        {
            return new string[] { Name };
        }

        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();

            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
            ModelDescription md = (ModelDescription)srcDesc;

            isTrained = md.IsTrained;
        }
    }
}
