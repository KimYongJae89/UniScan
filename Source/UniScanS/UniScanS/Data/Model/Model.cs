namespace UniScanS.Data.Model
{
    public class Model : UniScanS.Common.Data.Model
    {
        public Model()
        {
        }

        public new ModelDescriptionS ModelDescription
        {
            get { return (ModelDescriptionS)modelDescription; }
        }
    }
}