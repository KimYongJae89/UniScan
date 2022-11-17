using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Util;
using UniScanS.Data;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision;
using UniScanS.Screen.Vision.Detector;
using UniScanS.UI;

namespace UniScanS.Screen.UI.Inspect
{
    public partial class InfoPanel : UserControl, IInspectStateListener, IOpStateListener,IMultiLanguageSupport, IModelListener, IProductionListener
    {
        List<RadioButton> radioButtonList = new List<RadioButton>();

        public InfoPanel()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemManager.Instance().InspectRunner.AddInspectDoneDelegate(InspectDone);
            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);
            ((ProductionManagerS)SystemManager.Instance().ProductionManager).AddListener(this);

            InitCheckBoxCam();

            Clear();
        }

        void InitCheckBoxCam()
        {
            panelSelectCam.Visible = SystemManager.Instance().ExchangeOperator is IServerExchangeOperator;

            //radioButtonList.Add(defaultButton);

            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator)
            {
                IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                List<InspectorObj> inspectorList = server.GetInspectorList();
                for (int i = inspectorList.Count - 1; i >= 0; i--)
                {
                    RadioButton radioButton = new RadioButton();
                    radioButton.Appearance = defaultButton.Appearance;
                    radioButton.AutoSize = defaultButton.AutoSize;
                    radioButton.Dock = defaultButton.Dock;
                    radioButton.FlatAppearance.BorderSize = defaultButton.FlatAppearance.BorderSize;
                    radioButton.FlatAppearance.CheckedBackColor = defaultButton.FlatAppearance.CheckedBackColor;
                    radioButton.FlatAppearance.MouseDownBackColor = defaultButton.FlatAppearance.MouseDownBackColor;
                    radioButton.FlatAppearance.MouseOverBackColor = defaultButton.FlatAppearance.MouseOverBackColor;
                    radioButton.FlatStyle = defaultButton.FlatStyle;
                    radioButton.Margin = defaultButton.Margin;
                    radioButton.Name = "radioButtonCam" + (inspectorList[i].Info.CamIndex + 1).ToString();
                    radioButton.Size = defaultButton.Size;
                    radioButton.TabIndex = 0;
                    radioButton.Text = (inspectorList[i].Info.CamIndex + 1).ToString();
                    radioButton.TextAlign = defaultButton.TextAlign;
                    radioButton.UseVisualStyleBackColor = defaultButton.UseVisualStyleBackColor;
                    radioButton.CheckedChanged += Button_CheckedChanged;
                    radioButton.Tag = inspectorList[i];
                    radioButtonList.Add(radioButton);
                    panelSelectCam.Controls.Add(radioButton);
                }

                RadioButton mButton = new RadioButton();
                mButton.Appearance = defaultButton.Appearance;
                mButton.AutoSize = defaultButton.AutoSize;
                mButton.Dock = defaultButton.Dock;
                mButton.FlatAppearance.BorderSize = defaultButton.FlatAppearance.BorderSize;
                mButton.FlatAppearance.CheckedBackColor = defaultButton.FlatAppearance.CheckedBackColor;
                mButton.FlatAppearance.MouseDownBackColor = defaultButton.FlatAppearance.MouseDownBackColor;
                mButton.FlatAppearance.MouseOverBackColor = defaultButton.FlatAppearance.MouseOverBackColor;
                mButton.FlatStyle = defaultButton.FlatStyle;
                mButton.Margin = defaultButton.Margin;
                mButton.Name = "radioButtonMonitoring";
                mButton.Size = defaultButton.Size;
                mButton.TabIndex = 0;
                mButton.Text = "M";
                mButton.TextAlign = defaultButton.TextAlign;
                mButton.UseVisualStyleBackColor = defaultButton.UseVisualStyleBackColor;
                mButton.CheckedChanged += Button_CheckedChanged;
                radioButtonList.Add(mButton);
                panelSelectCam.Controls.Add(mButton);

                mButton.Checked = true;
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void InspectDoneDelegate(InspectionResult inspectionResult);
        public void InspectDone(InspectionResult inspectionResult)
        {
            if (inspectionResult.AlgorithmResultLDic.ContainsKey(SheetInspector.TypeName) == false)
                return;
            
            if (InvokeRequired)
            {
                Invoke(new InspectDoneDelegate(InspectDone), inspectionResult);
                return;
            }

            AlgorithmResult algorithmResult = inspectionResult.AlgorithmResultLDic[SheetInspector.TypeName];
            processTime.Text = string.Format("{0} s", algorithmResult.SpandTime.ToString("ss\\.fff"));

            UpdateData();
        }
        
        delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            UpdateProductionInfo();
        }

        private void UpdateProductionInfo()
        {
            foreach (RadioButton radioButton in radioButtonList)
            {
                if (radioButton.Checked == true)
                {
                    InspectorObj obj = (InspectorObj)radioButton.Tag;
                    ProductionS productionG = null;
                    if (obj == null)
                        productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
                    else
                        productionG = (ProductionS)obj.CurProduction;

                    if (productionG == null)
                        return;

                    lotNo.Text = productionG.LotNo;
                    startTime.Text = productionG.StartTime.ToString("MM-dd HH:mm");
                    endTime.Text = productionG.LastUpdateTime.ToString("MM-dd HH:mm");

                    productionTotal.Text = productionG.Total.ToString();
                    productionNG.Text = productionG.Ng.ToString();
                    productionRatio.Text = string.Format("{0:0.00} %", productionG.NgRatio);

                    UpdateNum(productionG, obj);
                }
            }
        }

        private void UpdateNum(ProductionS production, InspectorObj inspectorObj)
        {
            if (radioTotalPattern.Checked)
            {
                sheetAttack.Text = production.SheetAttackNum.ToString();
                pole.Text = production.PoleNum.ToString();
                dielectric.Text = production.DielectricNum.ToString();
                pinHole.Text = production.PinHoleNum.ToString();
                shape.Text = production.ShapeNum.ToString();
            }
            else
            {
                sheetAttack.Text = production.SheetAttackPatternNum.ToString();
                pole.Text = production.PolePatternNum.ToString();
                dielectric.Text = production.DielectricPatternNum.ToString();
                pinHole.Text = production.PinHolePatternNum.ToString();
                shape.Text = production.ShapePatternNum.ToString();
                //asdasd
                //if (lastSheetResult == null)
                //    return;

                //SheetResult tempSheetResult = new SheetResult();

                //if (inspectorObj == null)
                //    tempSheetResult.Copy(lastSheetResult);
                //else
                //    tempSheetResult.Copy(lastSheetResult, inspectorObj.Info.CamIndex + 1);

                //sheetAttack.Text = tempSheetResult.SheetAttackList.Count.ToString();
                //pole.Text = tempSheetResult.PoleList.Count.ToString();
                //dielectric.Text = tempSheetResult.DielectricList.Count.ToString();
                //pinHole.Text = tempSheetResult.PinHoleList.Count.ToString();
                //shape.Text = tempSheetResult.ShapeList.Count.ToString();
            }
        }
        
        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            if (InvokeRequired)
            {
                Invoke(new InspectStateChangedDelegate(InspectStateChanged), curInspectState);
                return;
            }
            
            switch (curInspectState)
            {
                case UniEye.Base.Data.InspectState.Run:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Run;
                    break;
                case UniEye.Base.Data.InspectState.Wait:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
                case UniEye.Base.Data.InspectState.Done:
                    status.Text = StringManager.GetString(this.GetType().FullName, curInspectState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    break;
            }
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(OpStateChanged), curOpState, prevOpState);
                return;
            }

            switch (curOpState)
            {
                case OpState.Idle:
                    status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
                    status.Appearance.BackColor = Colors.Idle;
                    break;
                case OpState.Inspect:
                    status.Text = StringManager.GetString(this.GetType().FullName, curOpState.ToString());
                    status.Appearance.BackColor = Colors.Wait;
                    UpdateData();
                    break;
            }
        }

        private void Clear()
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Clear));
                return;
            }

            lotNo.Text = string.Empty;
            startTime.Text = string.Empty;

            processTime.Text = string.Empty;

            productionTotal.Text = string.Empty;
            productionNG.Text = string.Empty;
            productionRatio.Text = string.Empty;

            sheetAttack.Text = string.Empty;
            pole.Text = string.Empty;
            dielectric.Text = string.Empty;
            pinHole.Text = string.Empty;
            shape.Text = string.Empty;
        }

        public void ModelChanged()
        {
            Clear();

            foreach (RadioButton radioButton in radioButtonList)
            {
                if (radioButton.Checked == true)
                    UpdateParam(radioButton.Tag);
            }
        }

        public void ProductionChanged()
        {
            Clear();

            UpdateData();
        }

        delegate void UpdateParamDelegate(object tag);
        private void UpdateParam(object tag)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateParamDelegate(UpdateParam), tag);
                return;
            }

            layoutParam.Visible = tag != null;

            if (tag != null)
            {
                InspectorObj obj = (InspectorObj)tag;

                Algorithm sheetInspector = obj.AlgorithmPool.GetAlgorithm(SheetInspector.TypeName);

                if (sheetInspector != null)
                {
                    SheetInspectorParam param = (SheetInspectorParam)sheetInspector.Param;

                    poleLowerParam.Text = param.PoleParam.LowerThreshold.ToString();
                    poleUpperParam.Text = param.PoleParam.UpperThreshold.ToString();

                    dielectricLowerParam.Text = param.DielectricParam.LowerThreshold.ToString();
                    dielectricUpperParam.Text = param.DielectricParam.UpperThreshold.ToString();

                    shapeLowerParam.Text = param.ShapeParam.DiffTolerence.ToString();
                    shapeUpperParam.Text = param.ShapeParam.DiffTolerence.ToString();

                    shapeHeightLowerParam.Visible = param.ShapeParam.UseHeightDiffTolerence;
                    shapeHeightUpperParam.Visible = param.ShapeParam.UseHeightDiffTolerence;
                    labelShapeHeightLowerParam.Visible = param.ShapeParam.UseHeightDiffTolerence;
                    labelShapeHeightUpperParam.Visible = param.ShapeParam.UseHeightDiffTolerence;
                    labelShapeHeightParam.Visible = param.ShapeParam.UseHeightDiffTolerence;

                    if (param.ShapeParam.UseHeightDiffTolerence == true)
                    {
                        shapeHeightLowerParam.Text = param.ShapeParam.HeightDiffTolerence.ToString();
                        shapeHeightUpperParam.Text = param.ShapeParam.HeightDiffTolerence.ToString();
                    }
                }
            }

            layoutSizeParam.Visible = tag == null;
            sizeParamSheetAttack.Text = AlgorithmSetting.Instance().SheetAttackMinSize.ToString();
            sizeParamPole.Text = AlgorithmSetting.Instance().PoleMinSize.ToString();
            sizeParamDielctric.Text = AlgorithmSetting.Instance().DielectricMinSize.ToString();
            sizeParamPinHole.Text = AlgorithmSetting.Instance().PinHoleMinSize.ToString();
        }

        public void ModelTeachDone()
        {
            foreach (RadioButton radioButton in radioButtonList)
            {
                if (radioButton.Checked == true)
                    UpdateParam(radioButton.Tag);
            }
        }

        private void Button_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Checked == false)
                return;

            UpdateParam(radioButton.Tag);
            UpdateProductionInfo();
        }

        private void checkBoxRatio_CheckedChanged(object sender, System.EventArgs e)
        {
            labelProductionRatio.Visible = checkBoxRatio.Checked;
            productionRatio.Visible = checkBoxRatio.Checked;
        }

        private void radioTotalPattern_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Checked == false)
                return;

            UpdateProductionInfo();
        }

        private void radioEachPattern_CheckedChanged(object sender, System.EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            if (radioButton.Checked == false)
                return;

            UpdateProductionInfo();
        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            if (productionRatio.Visible == true && string.IsNullOrEmpty(productionRatio.Text) == false)
            {
                string[] splitString = productionRatio.Text.Split(' ');

                if (splitString.Length < 1)
                    return;

                float ratio;
                bool result = float.TryParse(splitString[0], out ratio);

                if (result == true)
                {
                    if (ratio > 10)
                    {
                        if (productionRatio.Appearance.BackColor == Color.Transparent)
                            productionRatio.Appearance.BackColor = Colors.Alarm;
                        else
                            productionRatio.Appearance.BackColor = Color.Transparent;
                    }
                    else if (productionRatio.Appearance.BackColor == Colors.Alarm)
                        productionRatio.Appearance.BackColor = Color.Transparent;
                }
                //productionRatio.Text
            }
        }
    }
}