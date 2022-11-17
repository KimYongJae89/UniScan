using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;
using UniScanS.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using DynMvp.Base;

namespace UniScanS.UI.Etc
{
    public partial class ModelPanel : UserControl, IModelListener, IMultiLanguageSupport
    {
        public ModelPanel()
        {
            InitializeComponent();
            
            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
            StringManager.AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void ModelTeachDone()
        {
            ModelStateChanged();
        }

        public void ModelChanged()
        {
            ModelStateChanged();
        }

        delegate void ModelStateChangedDelegate();
        public void ModelStateChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new ModelStateChangedDelegate(ModelStateChanged));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                modelName.Text = "Empty";
                modelName.BackColor = Colors.Idle;
                return;
            }

            modelName.Text = SystemManager.Instance().CurrentModel.Name;

            if (SystemManager.Instance().ExchangeOperator.ModelTrained(SystemManager.Instance().CurrentModel.ModelDescription) == true)
               modelName.BackColor = Colors.Trained;
            else
               modelName.BackColor = Colors.Untrained;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
