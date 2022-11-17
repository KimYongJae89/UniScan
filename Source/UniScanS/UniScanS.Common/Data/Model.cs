using System.Xml;
using UniEye.Base.Settings;

namespace UniScanS.Common.Data
{
    public abstract class Model : DynMvp.Data.Model
    {
        public new ModelDescription ModelDescription
        {
            get { return (ModelDescription)modelDescription; }
            set { modelDescription = value; }
        }


        public bool IsTrained
        {
            get { return ModelDescription.IsTrained; }
            set { ModelDescription.IsTrained = value; }
        }

        public override bool IsTaught()
        {
            return IsTrained;
        }

        public void Release()
        {

        }
    }
}
