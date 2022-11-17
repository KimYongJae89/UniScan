using DynMvp.Data.Forms;
using UniEye.Base.UI.ParamControl;
using UniEye.Base.UI;
using UniScanG.Settings;
using UniScanG.UI.Model;
using System.Windows.Forms;
using UniScanG.Common;
using System;
using UniScanG.UI;
using System.Collections.Generic;
using UniScanG.Common.Data;
using UniScanG.UI.Teach.Monitor;
using UniScanG.Common.Exchange;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.Common.Util;
using UniScanG.Gravure.UI.Teach;
using UniScanG.Gravure.UI.Teach.Inspector;
using UniScanG.Common.Settings;
using UniScanG.Gravure.UI;
using UniScanG.Gravure.UI.Inspect;
using UniScanG.Gravure.Vision;

namespace UniScanG.Module.Inspector.UI
{
    public class InspectorUiChangerG : InspectorUiChanger
    {
        public override IMainTabPage CreateReportPage()
        {
            return this.report = new UniScanG.UI.Report.ReportPage(false);
        }

        public override Control CreateInfoBufferPanel()
        {
            IInfoPanelBufferState infoPanelBufferState = new UI.Inspect.InfoPanelBufferState();
            return new Gravure.UI.Inspect.InfoPanel(infoPanelBufferState, false);
        }

        public override IInspectDefectPanel CreateDefectPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.DefectPanel() { Dock = DockStyle.Right };
        }

        public override IInspectExtraPanel CreateExtraInfoPanel()
        {
            //if (AlgorithmSetting.Instance().UseExtMargin)
            //    return new UniScanG.Module..Gravure.UI.Inspect.MarginPanel();
            return null;
        }

        public override IInspectExtraPanel CreateExtraImagePanel()
        {
            return null;
        }

        public override IModellerControl CreateImageController()
        {
            return new ImageController();
        }

        public override UniEye.Base.UI.ModellerPageExtender CreateModellerPageExtender()
        {
            return new ModellerPageExtenderG();
        }

        public override IModellerControl CreateParamController()
        {
            if (AlgorithmSetting.Instance().TrainerVersion == ETrainerVersion.RCI && AlgorithmSetting.Instance().CalculatorVersion == ECalculatorVersion.V3_RCI)
                return new UniScanG.Gravure.UI.Teach.Inspector.ParamControllerRCI();

            return new UniScanG.Gravure.UI.Teach.Inspector.ParamController();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanG.Gravure.UI.Setting.SettingPage(new UI.Settings.SettingPageExtender());
        }

        public override IMainTabPage CreateTeachPage()
        {
            this.teach = new UniScanG.UI.Teach.Inspector.TeachPage();
            return this.teach;
        }

        public override IModellerControl CreateTeachToolBar()
        {
            return new TeachToolBarG();
        }

        public override IUiControlPanel CreateUiControlPanel()
        {
            throw new NotImplementedException();
        }

        public override IInspectExtraPanel CreateExtraLengthPanel()
        {
            return new UniScanG.Gravure.UI.Inspect.LengthChart();
        }
    }
}
