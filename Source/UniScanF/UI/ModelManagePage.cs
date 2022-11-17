using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using UniEye.Base.UI;
using DynMvp.Data;
using UniEye.Base;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;
using DynMvp.UI.Touch;
using System.Xml;
using DynMvp.Authentication;

namespace UniScan.UI
{
    public partial class ModelManagePage : UserControl, IMainTabPage
    {
        bool modelSelected = false;
        public bool ModelSelected
        {
            get { return modelSelected; }
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ModelManagePage()
        {
            InitializeComponent();
        }

        private void ModelManagePage_Load(object sender, EventArgs e)
        {
            SystemManager.Instance().ModelManager.Refresh();
            RefreshModelList();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            ModelForm newModelForm = new ModelForm();
            newModelForm.ModelFormType = ModelFormType.New;
            if (newModelForm.ShowDialog(this) == DialogResult.OK)
            {
                NewModel(newModelForm.ModelDescription);
                SelectionChanged();
            }
        }

        delegate void NewModelDelegate(ModelDescription modelDescription);

        public void NewModel(ModelDescription modelDescription)
        {
            if (InvokeRequired)
            {
                Invoke(new NewModelDelegate(NewModel), modelDescription);
                return;
            }

            modelDescription.RegistrationDate = DateTime.Now;

            Model curModel = (Model)SystemManager.Instance().CurrentModel;
            curModel?.CloseModel();

            SystemManager.Instance().ModelManager.CloseModel();
            string modelName = modelDescription.Name;

            SystemManager.Instance().ModelManager.AddModel(modelDescription);
            //SystemManager.Instance().LoadModel(modelName);

            modelSelected = false;

            RefreshModelList();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            ModelForm copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.New;

            if (modelList.SelectedRows.Count != 0)
            {
                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

                SystemManager.Instance().ModelManager.CloseModel();
                //if (SystemManager.Instance().LoadModel(modelName) == false)
                //    return;

                UniScan.Data.Model curModel = (UniScan.Data.Model)SystemManager.Instance().CurrentModel;

                copyModelForm.ModelDescription = curModel.ModelDescription.Clone();

                if (copyModelForm.ShowDialog(this) == DialogResult.OK)
                {
                    SystemManager.Instance().ModelManager.CloseModel();
                    //SystemManager.Instance().ModelManager.DeleteModel(modelName);

                    copyModelForm.ModelDescription.LastModifiedDate = DateTime.Now;
                    SystemManager.Instance().ModelManager.AddModel(copyModelForm.ModelDescription);

                    RefreshModelList();
                }
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            ModelForm copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.New;

            if (modelList.SelectedRows.Count != 0)
            {
                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

                SystemManager.Instance().ModelManager.CloseModel();
                //if (SystemManager.Instance().LoadModel(modelName) == false)
                //    return;

                UniScan.Data.Model curModel = (UniScan.Data.Model)SystemManager.Instance().CurrentModel;

                copyModelForm.ModelDescription = curModel.ModelDescription.Clone();

                if (copyModelForm.ShowDialog(this) == DialogResult.OK)
                {
                    copyModelForm.ModelDescription.RegistrationDate = DateTime.Now;
                    copyModelForm.ModelDescription.LastModifiedDate = DateTime.Now;
                    SystemManager.Instance().ModelManager.AddModel(copyModelForm.ModelDescription);

                    RefreshModelList();
                }
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count != 0)
            {
                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

                SelectModel(modelName, false);
            }
        }

        delegate void SelectModelDelegate(string modelName, bool remoteCall);
        public void SelectModel(string modelName, bool remoteCall)
        {
            if (InvokeRequired)
            {
                Invoke(new SelectModelDelegate(SelectModel), modelName, remoteCall);
                return;
            }

            SystemManager.Instance().ModelManager.CloseModel();

            //if (SystemManager.Instance().LoadModel(modelName) == false)
            //    return;

            UniScan.Data.Model curModel = (UniScan.Data.Model)SystemManager.Instance().CurrentModel;

            modelSelected = true;

            UniScan.UI.MainForm mainForm = (UniScan.UI.MainForm)SystemManager.Instance().MainForm;

            string[] modelNameText = modelName.Split('_');

            if (modelNameText.Count() > 3)
            {
                mainForm.modelName.Text = String.Format("{0}:{1}\n{2}:{3}", modelNameText[0], modelNameText[1], modelNameText[2], modelNameText[3]);
            }
            else
            {
                mainForm.modelName.Text = modelName;
            }

            //if (curModel?.IsTaught() == true)

            ((MainForm)SystemManager.Instance().MainForm).UpdateMainTab(true);
            ((MainForm)SystemManager.Instance().MainForm).TabChange("Inspect");
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridViewRow in modelList.SelectedRows)
            {
                string modelName = ((ModelDescription)dataGridViewRow.Tag).Name;

                DialogResult dialogResult = DialogResult.No;
                dialogResult = DynMvp.UI.Touch.MessageForm.Show(ParentForm, String.Format("Do you want to delete the selected model {0}?", modelName), "Delete Model", DynMvp.UI.Touch.MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DeleteModel(modelName);
                    SelectionChanged();
                    break;
                }
            }
        }

        delegate void DeleteModelDelegate(string modelName);

        public void DeleteModel(string modelName)
        {
            if (InvokeRequired)
            {
                Invoke(new DeleteModelDelegate(DeleteModel), modelName);
                return;
            }

            UniScan.Data.Model curModel = (UniScan.Data.Model)SystemManager.Instance().CurrentModel;
            if (curModel != null)
            {
                if (curModel.Name == modelName)
                {
                    SystemManager.Instance().ModelManager.CloseModel();
                }
            }

            //SystemManager.Instance().ModelManager.DeleteModel(modelName);

            RefreshModelList();

            ((MainForm)SystemManager.Instance().MainForm).UpdateMainTab(false);
        }

        private void RefreshModelList(string searchText = "")
        {
            modelList.Rows.Clear();

            int total = 0;

            int index = 0;
            bool taught = false;
            string lastModifiedStr = "";
            string taughtStr = "X";
            bool isCType = false;

            foreach (ModelDescription modelDescription in SystemManager.Instance().ModelManager)
            {
                if (searchText == "" || modelDescription.Name.Contains(searchText))
                {
                    //DirectoryInfo directoryInfo = new DirectoryInfo(SystemManager.Instance().ModelManager.GetModelPath(modelDescription.Name));

                    //FileInfo[] fileInfo = directoryInfo.GetFiles("*.txt", SearchOption.TopDirectoryOnly);

                    //if (fileInfo.Count() >= 4)
                    //    taughtStr = "O";
                    //else
                    //    taughtStr = "X";

                    //lastModifiedStr = modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");

                    //int curIndex = modelList.Rows.Add(index + 1, modelDescription.Name, taughtStr, modelDescription.Registrant,
                    //        modelDescription.RegistrationDate.ToString("yyyy-MM-dd HH:mm:ss"), lastModifiedStr, modelDescription.Description);
                    //modelList.Rows[index].Tag = modelDescription;

                    //if (taught)
                    //    modelList.Rows[curIndex].Cells[2].Style.BackColor = Color.LightGreen;
                    //else
                    //    modelList.Rows[curIndex].Cells[2].Style.BackColor = Color.Red;

                    //index++;
                }
            }

            totalModel.Text = String.Format("Total : {0}", total);

            modelList.Sort(modelList.Columns[4], System.ComponentModel.ListSortDirection.Descending);

            for (int i = 0; i < modelList.RowCount; i++)
            {
                modelList.Rows[i].Cells[0].Value = i + 1;
            }

            if (modelList.Rows.Count > 0)
                modelList.Rows[0].Selected = true;
        }

        public void EnableControls(UserType user)
        {

        }

        public void TabPageVisibleChanged(bool visible)
        {
            if (visible == true)
            {
                RefreshModelList();
            }
        }

        private void modelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectionChanged();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefreshModelList(findModel.Text);
        }

        private void findModel_TextChanged(object sender, EventArgs e)
        {
            modelList.Rows.Clear();
            RefreshModelList(findModel.Text);
        }

        private void buttonTeach_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count != 0)
            {
                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

                SelectModel(modelName, false);
                ((MainForm)SystemManager.Instance().MainForm).TabChange("Teach");
            }
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged();
        }

        private void SelectionChanged()
        {
            if (modelList.SelectedRows.Count != 0)
            {
                if (modelList.SelectedRows[0].Tag == null)
                    return;

                string modelName = ((ModelDescription)modelList.SelectedRows[0].Tag).Name;

                foreach (ModelDescription modelDescription in SystemManager.Instance().ModelManager)
                {
                    if (modelDescription.Name == modelName)
                    {
                        // Do something for display
                    }
                }
            }
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }
    }
}
