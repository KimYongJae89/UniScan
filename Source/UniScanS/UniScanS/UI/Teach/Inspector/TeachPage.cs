using DynMvp.Base;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Util;

namespace UniScanS.UI.Teach.Inspector
{
    public interface IModellerControl
    {
        void SetModellerExtender(ModellerPageExtender modellerPageExtender);
    }

    public partial class TeachPage : UserControl, IMultiLanguageSupport
    {
        IModellerControl teachToolbar;
        IModellerControl imageController;
        IModellerControl paramController;

        ModellerPageExtender modellerPageExtender;

        public TeachPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            InspectorUiChanger inspectorUiChanger = (InspectorUiChanger)SystemManager.Instance().UiChanger;

            modellerPageExtender = (ModellerPageExtender)inspectorUiChanger.CreateModellerPageExtender();

            imageController = inspectorUiChanger.CreateImageController();
            paramController = inspectorUiChanger.CreateParamController();
            teachToolbar = inspectorUiChanger.CreateTeachToolBar();

            teachToolbar.SetModellerExtender(modellerPageExtender);
            imageController.SetModellerExtender(modellerPageExtender);
            paramController.SetModellerExtender(modellerPageExtender);

            imagePanel.Controls.Add((Control)imageController);
            paramPanel.Controls.Add((Control)paramController);
            toolbarPanel.Controls.Add((Control)teachToolbar);
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
