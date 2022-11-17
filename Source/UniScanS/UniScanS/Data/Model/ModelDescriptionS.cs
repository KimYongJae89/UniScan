using DynMvp.Base;
using System;
using System.Text;
using System.Xml;

namespace UniScanS.Data.Model
{
    public class ModelDescriptionS : UniScanS.Common.Data.ModelDescription
    {
        float thickness;
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        private string paste;
        public string Paste
        {
            get { return paste; }
            set { paste = value; }
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            Name = XmlHelper.GetValue(modelDescElement, "Name", "");
            paste = XmlHelper.GetValue(modelDescElement, "Paste", "");
            thickness = Convert.ToSingle(XmlHelper.GetValue(modelDescElement, "Thickness", "0"));
        }

        public override void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "Name", Name.ToString());
            XmlHelper.SetValue(modelDescElement, "Thickness", thickness.ToString());
            XmlHelper.SetValue(modelDescElement, "Paste", paste);

            base.Save(modelDescElement);
        }

        public override string[] GetArgs()
        {
            return new string[] { Name, thickness.ToString(), paste };
        }

        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescriptionS discription = new ModelDescriptionS();

            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
            ModelDescriptionS md = (ModelDescriptionS)srcDesc;
            thickness = md.thickness;
            paste = md.paste;
        }

        public override bool Equals(object obj)
        {
            ModelDescriptionS modelDescriptionS = obj as ModelDescriptionS;
            if (modelDescriptionS == null)
                return false;

            if (!base.Equals(obj))
                return false;
            return this.Paste == modelDescriptionS.Paste && this.Thickness == modelDescriptionS.Thickness;
        }
    }
}
