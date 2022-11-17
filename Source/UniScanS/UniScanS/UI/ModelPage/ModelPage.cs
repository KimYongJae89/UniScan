using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniScan.Common;
using UniScan.Common.UI;
using UniScan.Common.Util;
using UniScanG.Data.Model;

namespace UniScanG.UI.ModelPage
{
    internal partial class ModelPage : UserControl, UniEye.Base.UI.IMainTabPage, IModelListener, IMultiLanguageSupport
    {
        bool onRefreshModelList = false;
        
        int cellLastModifiedDateIndex = 6;

        public ModelPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabIndex = 0;
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        private void ModelManagePage_Load(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ModelForm newModelForm = new ModelForm();
            newModelForm.ModelFormType = ModelFormType.New;
            newModelForm.TopMost = true;
            if (newModelForm.ShowDialog() == DialogResult.OK)
                SystemManager.Instance().ExchangeOperator.NewModel(newModelForm.ModelDescription);

            RefreshModelList();
        }

        private void buttonSelect_Click(object sender, EventArgs e)
          {
            if (modelList.SelectedRows.Count == 0)
                return;
            
            LogHelper.Info(LoggerType.Operation, "Select model.");

            ModelDescriptionG modelDescription = (ModelDescriptionG)modelList.SelectedRows[0].Tag;

            SystemManager.Instance().ExchangeOperator.SelectModel(modelDescription);

            if (SystemManager.Instance().UiController != null)
            {
                Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
                if (curModel.IsTaught() == true)
                    SystemManager.Instance().UiController.ChangeTab(MainTabKey.Inspect.ToString());
                else
                    SystemManager.Instance().UiController.ChangeTab(MainTabKey.Teach.ToString());
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in modelList.SelectedRows)
            {
                ModelDescriptionG md = (ModelDescriptionG)dataGridViewRow.Tag;
                
                DialogResult dialogResult = DialogResult.No;
                dialogResult = MessageForm.Show(ParentForm, String.Format("Do you want to delete the selected model {0}?", md.Name), "Delete Model", DynMvp.UI.Touch.MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SystemManager.Instance().ExchangeOperator.DeleteModel(md);
                    RefreshModelList();
                    break;
                }
            }
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
            foreach (ModelDescriptionG modelDescription in SystemManager.Instance().ModelManager)
            {
                if (string.IsNullOrEmpty(searchText) || modelDescription.Name.Contains(searchText))
                {
                    string lastModifiedStr = modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    bool trained = SystemManager.Instance().ExchangeOperator.ModelTrained(modelDescription);
                    
                    int index = modelList.Rows.Add(modelDescription.Name, modelDescription.Thickness, modelDescription.Paste, trained, 
                        modelDescription.Registrant, modelDescription.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"), 
                        lastModifiedStr, modelDescription.Description);
                    if (trained == true)
                        modelList.Rows[index].Cells[3].Style.BackColor = Colors.Trained;
                    else
                        modelList.Rows[index].Cells[3].Style.BackColor = Colors.Untrained;
                    modelList.Rows[index].Tag = modelDescription;
                    
                    totalModelNum++;
                }
            }

            total.Text = totalModelNum.ToString();

            modelList.Sort(modelList.Columns[cellLastModifiedDateIndex], System.ComponentModel.ListSortDirection.Descending);

            onRefreshModelList = false;

            if (modelList.Rows.Count > 0)
                SelectionChanged();
            else
                ClearModelImagePanel();
        }

        public void ClearModelImagePanel()
        {
            camImage.Image = null;
            camImage.Invalidate(false);
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visible)
        {
            if (visible == true)
            {
                findModel.Text = "";
                RefreshModelList();
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

        private void buttonTeach_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count == 0)
                return;

            LogHelper.Info(LoggerType.Operation, "Select model.");

            ModelDescriptionG modelDescription = (ModelDescriptionG)modelList.SelectedRows[0].Tag;
            SystemManager.Instance().ExchangeOperator.SelectModel(modelDescription);

            if (SystemManager.Instance().UiController != null)
                SystemManager.Instance().UiController.ChangeTab(MainTabKey.Teach.ToString());
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            if (onRefreshModelList == true)
                return;

            SelectionChanged();
        }

        private void ButtonEnable()
        {
            buttonSelect.Enabled = true;
            buttonTeach.Enabled = true;
            buttonDelete.Enabled = true;
        }

        private void ButtonDisable()
        {
            buttonSelect.Enabled = false;
            buttonTeach.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void SelectionChanged()
        {
            if (modelList.SelectedRows.Count == 0)
            {
                ButtonDisable();
                return;
            }

            ButtonEnable();
            
            if (modelList.SelectedRows[0].Tag == null)
                return;

            ModelDescriptionG selectedMd = (ModelDescriptionG)modelList.SelectedRows[0].Tag;

            foreach (ModelDescriptionG md in SystemManager.Instance().ModelManager)
            {
                if (md.Name == selectedMd.Name && md.Thickness == selectedMd.Thickness && md.Paste == selectedMd.Paste)
                {
                    Bitmap image =  SystemManager.Instance().ModelManager.GetPreviewImage(md);
                    camImage.Image = image;
                }
            }
        }
        
        public void ModelTeachDone()
        {
            RefreshModelList();
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }
        public void ModelChanged() { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            //StringManager.UpdateString(this.GetType().FullName, labelImage);

            //StringManager.UpdateString(this.GetType().FullName, labelModelList);
            //StringManager.UpdateString(this.GetType().FullName, labelTotal);

            //StringManager.UpdateString(this.GetType().FullName, buttonSelect);
            //StringManager.UpdateString(this.GetType().FullName, buttonTeach);
            //StringManager.UpdateString(this.GetType().FullName, buttonNew);
            //StringManager.UpdateString(this.GetType().FullName, buttonDelete);

            //for(int i=0; i<modelList.ColumnCount; i++)
            //{
            //    modelList.Columns[i].HeaderText = StringManager.GetString(this.GetType().FullName, modelList.Columns[i].HeaderText);
            //}
        }
    }
}
