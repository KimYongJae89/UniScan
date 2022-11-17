using DynMvp.Base;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace UniScanG.Data.Model
{
    public class ModelDescription : UniScanG.Common.Data.ModelDescription
    {
        public override bool IsDefaultModel => base.IsDefaultModel && this.paste == "Default" && this.thickness == 0;

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


        public override bool Equals(object obj)
        {
            bool isSame = base.Equals(obj);
            if (isSame == false)
                return false;

            ModelDescription md = obj as ModelDescription;
            return md == null ? false : thickness == md.thickness && paste == md.paste;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ thickness.GetHashCode() ^ paste.GetHashCode();
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
            base.Save(modelDescElement);
            //XmlHelper.SetValue(modelDescElement, "Name", Name.ToString());
            XmlHelper.SetValue(modelDescElement, "Thickness", thickness.ToString());
            XmlHelper.SetValue(modelDescElement, "Paste", paste);

        }

        public override string[] GetArgs()
        {
            return new string[] { Name, thickness.ToString(), paste };
        }

        public override void FromArgs(string[] args)
        {
            this.name = args[0];
            this.thickness = float.Parse(args[1]);
            this.paste = args[2];
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
            thickness = md.thickness;
            paste = md.paste;
        }


    }
}
