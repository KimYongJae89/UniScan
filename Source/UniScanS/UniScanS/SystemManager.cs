// custom
using UniEye.Base.Inspect;
using UniScanS.Common;
using UniScanS.Data;
using UniScanS.Data.Model;
using UniScanS.UI;

namespace UniScanS
{
    public abstract class SystemManager : UniScanS.Common.SystemManager
    {
        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new UiChanger UiChanger
        {
            get { return (UiChanger)uiChanger; }
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
            set { currentModel = value; }
        }

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public override UniScanS.Common.Data.ModelManager CreateModelManager()
        {
            return new ModelManager();
        }

        public new UnitBaseInspectRunner InspectRunner
        {
            get { return (UnitBaseInspectRunner)inspectRunner; }
        }

    }
}
