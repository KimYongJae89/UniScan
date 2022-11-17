using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using DynMvp.Vision;
using UniEye.Base.Inspect;
using UniScanX.MPAlignment.Operation;
using UniScanX.MPAlignment.Settings;


namespace UniScanX.MPAlignment
{
    enum SystemVersion { Version_1_0_a }

    public class SystemManager : UniEye.Base.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new Operation.InspectRunner();
        }
        
        public override string[] GetSystemTypeNames()
        {
            return Enum.GetNames(typeof(SystemVersion));
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new UniScanX.MPAlignment.Operation.InspectRunnerExtender();
        }

        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

           //AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(GlassAlgorithm.TypeName, OperationSettings.Instance().ImagingLibrary, ""));

        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

           // AlgorithmBuilder.SetAlgorithmEnabled(GlassAlgorithm.TypeName, true);
        }

        public override void InitializeDataExporter()
        {
           // dataExporterList.Add(new UniScanM.CGInspector.Data.DataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            MPSettings.CreateInstance();
            MPSettings.Instance().Load();
        }
        public virtual void LoadDefaultModel()
        {
            if (ModelManager == null)
                return;
            string[] weekDays = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            ModelDescription md = ((ModelManager)ModelManager).GetModelDescription(weekDays);
            if (md == null)
            {
                md = (ModelDescription)modelManager.CreateModelDescription();
                md.Name = "DefaultModel";
               // md.Paste = "Unknown";
                modelManager.AddModel(md);
                Model model = (Model)modelManager.LoadModel(md, null);
                modelManager.SaveModel(model);
            }
            //모델이 널일때가 있네.
            if (this.currentModel?.ModelDescription == md)
                return;

            SystemManager.Instance().LoadModel(md);
        }
    }
}
