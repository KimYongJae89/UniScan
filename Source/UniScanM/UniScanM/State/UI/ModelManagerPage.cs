using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;
using Infragistics.Win;
using Infragistics.Win.Misc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using UniScan.Common;
//using UniScan.Common.UI;
//using UniScan.Common.Util;
//using UniScanG.Data.Model;
//using UniScanG.Data.UI;
using UniEye.Base.UI;
using UniScanM;
using UniScanM.Data;
//using UniScanM.Data;

namespace UniScanM.UI
{
    public partial class ModelManagerPage : UserControl, UniEye.Base.UI.IMainTabPage, IModelManagerPage, IMultiLanguageSupport//, IModelListener
    {
        bool onRefreshModelList = false;
        CanvasPanel canvasPanel = null;

        Model selectedModel = null;

        //int cellLastModifiedDateIndex = 6;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ModelManagerPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabIndex = 0;

            this.canvasPanel = new CanvasPanel(true);
            this.canvasPanel.Dock = DockStyle.Fill;
            panelParam.Controls.Add(canvasPanel);

            //SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        private void ModelManagerPage_Load(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ModelForm newModelForm = new ModelForm();
            newModelForm.ModelFormType = ModelFormType.New;
            newModelForm.TopMost = true;
            if (newModelForm.ShowDialog() == DialogResult.OK)
                NewModel(newModelForm.ModelDescription);

            RefreshModelList();
        }

        public bool NewModel(ModelDescription modelDescription)
        {
            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;

            if (modelManager.IsModelExist(modelDescription.Name, modelDescription.Paste) == true)
                return false;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();
            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);

            /* 아래처럼 되야하는거 아닌가? ms
            Data.Model model = (Data.Model)modelManager.CreateModel();
            model.ModelDescription = modelDescription;
            model.ModelPath = Path.Combine(modelManager.GetModelPath(modelDescription.Name));
            modelManager.SaveModel(model);
            modelManager.SaveModelDescription(model);
            modelManager.Refresh();
            */
            return true;
        }


        public bool SelectModel(ModelDescription modelDescription)
        {
            if (SystemManager.Instance().LoadModel(modelDescription) == false)
                return false;

            Model curModel = SystemManager.Instance().CurrentModel as Model;

            SystemManager.Instance().MainForm?.OnModelChanged();
            SystemManager.Instance().MainForm.PageChange(SystemManager.Instance().MainForm.InspectPage);
            //SystemManager.Instance().MainForm.PageChange(SystemManager.Instance().MainForm.MonitoringPage);
            //if (curModel.IsTaught())
            //    SystemManager.Instance().MainForm.PageChange(SystemManager.Instance().MainForm.MonitoringPage);
            //else
            //    SystemManager.Instance().MainForm.PageChange(SystemManager.Instance().MainForm.ModellerPage);
            return true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in modelList.SelectedRows)
            {
                ModelDescription md = (ModelDescription)dataGridViewRow.Tag;

                if (md.Name == SystemManager.Instance().CurrentModel?.Name)
                {
                    MessageForm.Show(null, StringManager.GetString(this.GetType().FullName, "This Model Is Current Model. Can't delete this model"));
                    return;
                }

                DialogResult dialogResult = DialogResult.No;
                dialogResult = MessageForm.Show(ParentForm, String.Format(
                    StringManager.GetString(this.GetType().FullName, "Do you want to delete the selected model") + "{0}?", md.Name), StringManager.GetString(this.GetType().FullName, "Delete Model"), DynMvp.UI.Touch.MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DeleteModel(md);
                    RefreshModelList();
                    break;
                }
            }
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            Model model = (Model)SystemManager.Instance().CurrentModel;
            if (model != null && model.ModelDescription == modelDescription)
            {
                //model.Release();
                model = null;
            }

            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;
            modelManager.DeleteModel(modelDescription);
        }

        delegate void RefreshModelListDelegate(string searchText);
        private void RefreshModelList(string searchText = "")
        {
            if (InvokeRequired)
            {
                Invoke(new RefreshModelListDelegate(RefreshModelList), searchText);
                return;
            }

            onRefreshModelList = true;

            modelList.Rows.Clear();

            int totalModelNum = 0;
            foreach (ModelDescription modelDescription in SystemManager.Instance().ModelManager)
            {
                if (string.IsNullOrEmpty(searchText) || modelDescription.Name.Contains(searchText))
                {
                    string lastModifiedStr = modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");

                    int index = modelList.Rows.Add(modelDescription.Name, modelDescription.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"), modelDescription.Description);

                    modelList.Rows[index].Tag = modelDescription;

                    totalModelNum++;
                }
            }

            total.Text = totalModelNum.ToString();

            modelList.Sort(modelList.Columns[0], System.ComponentModel.ListSortDirection.Descending);

            onRefreshModelList = false;

            if (modelList.Rows.Count > 0)
                SelectionChanged();
        }

        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visible)
        {
            if (visible == true)
            {
                findModel.Text = "";
                RefreshModelList();

                Model model = SystemManager.Instance().CurrentModel;
                if (model != null)
                {
                    foreach (DataGridViewRow row in modelList.Rows)
                    {
                        if (row.Cells[0].Value.ToString().Equals(model.Name))
                        {
                            modelList.Rows[row.Index].Selected = true;
                        }
                    }
                }
            }
        }

        private void modelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectionChanged();
        }

        private void findModel_TextChanged(object sender, EventArgs e)
        {
            RefreshModelList(findModel.Text);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count == 0)
                return;

            LogHelper.Info(LoggerType.Operation, "Select model.");

            ModelDescription modelDescription = (ModelDescription)modelList.SelectedRows[0].Tag;


            UniEye.Base.UI.ModelForm modelForm = new UniEye.Base.UI.ModelForm();
            modelForm.Text = StringManager.GetString(this.GetType().FullName, "Edit Model");
            modelForm.ModelFormType = ModelFormType.Edit;
            modelForm.ModelDescription = modelDescription;
            modelForm.TopMost = true;
            if (modelForm.ShowDialog() == DialogResult.OK)
            {
                //NewModel(modelForm.ModelDescription);
                RefreshModelList();
            }

            //SelectModel(modelDescription);

            //if (SystemManager.Instance().UiController != null)
            //    SystemManager.Instance().UiController.ChangeTab(MainTabKey.Teach.ToString());
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            if (onRefreshModelList == true)
                return;

            SelectionChanged();
        }

        private void ButtonEnable()
        {
            buttonApply.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void ButtonDisable()
        {
            buttonApply.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void SelectionChanged()
        {
            if (modelList.SelectedRows.Count == 0)
            {
                ButtonDisable();
                selectedModel = null;
                return;
            }

            ButtonEnable();

            //if (modelList.SelectedRows[0].Tag == null || modelList.SelectedRows[0].Tag is ModelDescription == false)
            //    return;

            //ModelDescription md = modelList.SelectedRows[0].Tag as ModelDescription;
            //UniScanM.Data.Model model = (UniScanM.Data.Model)SystemManager.Instance().ModelManager.LoadModel(md, null);
            //selectedModel = model;

            //propertyGrid.SelectedObject = model.InspectParam;
        }

        public void ModelTeachDone()
        {
            RefreshModelList();
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;

            Color buttonBackColor = visibleFlag ? Color.CornflowerBlue : Color.Transparent;
            if (this.showHideControl != null)
                ((UltraButton)this.showHideControl).Appearance.BackColor = buttonBackColor;

            TabPageVisibleChanged(visibleFlag);
        }
        public void ModelChanged() { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (selectedModel == null)
                return;
            SystemManager.Instance().ModelManager.SaveModel(selectedModel);
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            //InspectRunner inspectRunner = SystemManager.Instance().InspectRunner as InspectRunner;
            //if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
            //{
            //    MessageForm.Show(null, "Cannot Access on Auto Mode.");
            //    return;
            //}

            //if (UniEye.Base.Data.SystemState.Instance().OpState != UniEye.Base.Data.OpState.Idle)
            //{
            //    MessageForm.Show(null, "Please, Stop the Inspect.");
            //    return;
            //}

            if (modelList.SelectedRows.Count == 0)
                return;

            LogHelper.Info(LoggerType.Operation, "Select model.");

            ModelDescription modelDescription = modelList.SelectedRows[0].Tag as ModelDescription;

            SelectModel(modelDescription);
        }
    }
}
