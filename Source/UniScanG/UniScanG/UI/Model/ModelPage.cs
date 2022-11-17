using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UniScanG.Common;
using UniScanG.Common.UI;
using UniScanG.Common.Util;
using UniScanG.Data.Model;
using UniScanG.Data.UI;
using UniEye.Base.UI;
using DynMvp.Authentication;
using DynMvp.Vision;
using System.Threading;
using UniEye.Base.Data;
using System.Threading.Tasks;

namespace UniScanG.UI.Model
{
    internal partial class ModelPage : UserControl, UniEye.Base.UI.IMainTabPage, IModelListener, IMultiLanguageSupport, IUserHandlerListener
    {
        int cellLastModifiedDateIndex = 6;
        List<ModelDescription> loadingMD = new List<ModelDescription>();

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ModelPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabIndex = 0;
            UserHandler.Instance().AddListener(this);
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        private void ModelManagePage_Load(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            Data.UI.ModelForm newModelForm = new Data.UI.ModelForm();
            newModelForm.ModelFormType = ModelFormType.New;

            MachineIF.MachineIfData machineIfData = ((Gravure.Device.DeviceControllerG)SystemManager.Instance().DeviceController).MachineIfMonitor?.MachineIfData;
            newModelForm.InitModelName = machineIfData?.GET_MODEL;

            if (newModelForm.ShowDialog(this) == DialogResult.OK)
            {
                //SystemManager.Instance().ModelManager.SaveModelDescription(newModelForm.ModelDescription);
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() =>
                {
                    AlgorithmPool.Instance().BuildAlgorithmPool();
                    SystemManager.Instance().ExchangeOperator.NewModel(newModelForm.ModelDescription);
                    RefreshModelList();
                    buttonTeach_Click(newModelForm.ModelDescription);
                });
            }
        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            ModelSelect();
        }

        private void ModelSelect()
        {
            if (modelList.SelectedRows.Count != 1)
                return;

            if (SystemState.Instance().OpState != OpState.Idle)
            {
                MessageForm.Show(this.ParentForm, StringManager.GetString("Please, Stop the inspection."));
                return;
            }

            LogHelper.Info(LoggerType.Operation, "Select model.");

            ModelDescription modelDescription = (ModelDescription)modelList.SelectedRows[0].Tag;

            bool good = SystemManager.Instance().ExchangeOperator.SelectModel(modelDescription);
            if (!good)
                return;

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
            List<ModelDescription> selectedModelDescriptionList = new List<ModelDescription>();
            foreach (DataGridViewRow dataGridViewRow in modelList.SelectedRows)
                selectedModelDescriptionList.Add((ModelDescription)dataGridViewRow.Tag);

            if (selectedModelDescriptionList.Count == 0)
                return;

            //if (selectedModelDescriptionList.Exists(f => f.IsDefaultModel))
            //{
            //    MessageForm.Show(ParentForm, StringManager.GetString("Can not delete [Default] model."), "Delete Model", DynMvp.UI.Touch.MessageFormType.OK);
            //    return;
            //}

            ModelDescription curModelDescription = SystemManager.Instance().CurrentModel?.ModelDescription;
            if (selectedModelDescriptionList.Contains(curModelDescription) && SystemState.Instance().IsInspectOrWait)
            {
                MessageForm.Show(ParentForm, StringManager.GetString("Can not delete current model."), "Delete Model", DynMvp.UI.Touch.MessageFormType.OK);
                return;
            }

            string message;
            if (selectedModelDescriptionList.Count == 1)
                message = string.Format(StringManager.GetString("Do you want to delete the selected model [{0}]?"), selectedModelDescriptionList[0].Name);
            else
                message = string.Format(StringManager.GetString("Do you want to delete {0} Models?"), selectedModelDescriptionList.Count);

            DialogResult dialogResult = MessageForm.Show(ParentForm, message, "Delete Model", DynMvp.UI.Touch.MessageFormType.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                selectedModelDescriptionList.ForEach(f =>
                {
                    // 클릭하면 Task로 미리보기 이미지 불러옴 -> 불러오는 중 삭제하면 bmp파일 삭제 오류 발생함.
                    bool isLoading;
                    do
                    {
                        lock (this.loadingMD)
                            isLoading = this.loadingMD.Contains(f);

                        if (isLoading)
                            Thread.Sleep(100);

                    } while (isLoading);

                    if (f.Equals(curModelDescription))
                        SystemManager.Instance().ExchangeOperator.CloseModel();
                    SystemManager.Instance().ExchangeOperator.DeleteModel(f);
                });
            }

            RefreshModelList();
        }

        bool onRefreshModelList;
        private void RefreshModelList()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(RefreshModelList));
                return;
            }

            this.onRefreshModelList = true;

            string searchText = findModel.Text;
            ClearModelImagePanel();
            modelList.Rows.Clear();

            DataGridViewRow selectRow = null;
            Data.Model.Model currentModel = SystemManager.Instance().CurrentModel;
            List<ModelDescription> modelDescriptionList = SystemManager.Instance().ModelManager.ModelDescriptionList.ConvertAll(f => (ModelDescription)f);
            if (!UserHandler.Instance().CurrentUser.IsMasterAccount)
                modelDescriptionList.RemoveAll(f => f.IsDefaultModel);

            lock (modelDescriptionList)
            {
                total.Text = modelDescriptionList.Count.ToString();
                foreach (ModelDescription modelDescription in modelDescriptionList)
                {
                    if (string.IsNullOrEmpty(searchText) || modelDescription.Name.Contains(searchText))
                    {
                        string lastModifiedStr = modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(modelList);
                        FillRow(row, modelDescription);
                        modelList.Rows.Add(row);

                        if (modelDescription.Equals(currentModel?.ModelDescription))
                            selectRow = row;
                    }
                }
            }
            modelList.Sort(modelList.Columns[cellLastModifiedDateIndex], System.ComponentModel.ListSortDirection.Descending);
            modelList.ClearSelection();

            if (selectRow != null)
                selectRow.Selected = true;


            this.onRefreshModelList = false;
        }

        private void FillRow(DataGridViewRow row, ModelDescription modelDescription)
        {
            row.Cells[0].Value = modelDescription.Name;
            row.Cells[1].Value = modelDescription.Thickness;
            row.Cells[2].Value = modelDescription.Paste;
            row.Cells[3].Value = modelDescription.IsTrained;
            row.Cells[4].Value = modelDescription.Registrant;
            row.Cells[5].Value = modelDescription.RegistrationDate;
            row.Cells[6].Value = modelDescription.LastModifiedDate;
            //row.Cells[7].Value = modelDescription.Description;

            row.Cells[3].Style.BackColor = modelDescription.IsTrained ? Colors.Trained : Colors.Untrained;

            row.Tag = modelDescription;
        }

        public void ClearModelImagePanel()
        {
            camImage.Image = null;
            camImage.Invalidate(false);
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
            }
        }

        private void modelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectionChanged();
        }

        private void findModel_TextChanged(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        private void buttonTeach_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count == 0)
                return;

            ModelDescription modelDescription = (ModelDescription)modelList.SelectedRows[0].Tag;
            buttonTeach_Click(modelDescription);
        }

        private void buttonTeach_Click(ModelDescription modelDescription)
        {
            if (SystemState.Instance().OpState != OpState.Idle)
            {
                MessageForm.Show(this.ParentForm, StringManager.GetString("Please, Stop the inspection."), "UniEye");
                return;
            }

            LogHelper.Info(LoggerType.Operation, "Select model.");

            bool good = SystemManager.Instance().ExchangeOperator.SelectModel(modelDescription);
            if (!good)
                return;

            if (SystemManager.Instance().UiController != null)
                SystemManager.Instance().UiController.ChangeTab(MainTabKey.Teach.ToString());
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            if (onRefreshModelList)
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

        CancellationTokenSource cancellationTokenSource;
        private void ButtonState(bool select, bool teach, bool delete)
        {
            buttonSelect.Enabled = false;
            buttonTeach.Enabled = false;
            buttonDelete.Enabled = false;
        }

        private void SelectionChanged()
        {
            if (modelList.SelectedRows.Count == 0)
            {
                buttonSelect.Enabled = buttonTeach.Enabled = buttonDelete.Enabled = false;
            }
            else if (modelList.SelectedRows.Count > 1)
            {
                buttonSelect.Enabled = buttonTeach.Enabled = false;
                buttonDelete.Enabled = true;
                ClearModelImagePanel();
            }
            else
            {
                buttonSelect.Enabled = buttonTeach.Enabled = buttonDelete.Enabled = true;

                if (modelList.SelectedRows[0].Tag == null)
                    return;

                ModelDescription selectedMd = (ModelDescription)modelList.SelectedRows[0].Tag;
                List<DynMvp.Data.ModelDescription> modelDescriptionList = SystemManager.Instance().ModelManager.ModelDescriptionList;
                lock (modelDescriptionList)
                {
                    foreach (ModelDescription md in modelDescriptionList)
                    {
                        if (selectedMd.Equals(md))
                        {
                            cancellationTokenSource?.Cancel();
                            cancellationTokenSource = new CancellationTokenSource();
                            System.Threading.Tasks.Task.Run(new Action(() => UpdateModelImagePanel(md, cancellationTokenSource.Token)));
                        }
                    }
                }
            }
        }

        private void UpdateModelImagePanel(ModelDescription md, CancellationToken cancellationToken)
        {
            lock (this.loadingMD)
                this.loadingMD.Add(md);

            UpdateModelImagePanel(UniScanG.Properties.Resources.InProgress);
            Bitmap image = (SystemManager.Instance().ModelManager as Common.Data.ModelManager)?.GetPreviewImage(md, "");

            if (!cancellationToken.IsCancellationRequested)
                UpdateModelImagePanel(image);

            lock (this.loadingMD)
                this.loadingMD.Remove(md);
        }

        delegate void UpdateImageDelegate(Image image);
        private void UpdateModelImagePanel(Image image)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateImageDelegate(UpdateModelImagePanel), image);
                return;
            }

            camImage.Image = image;
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag)
            {
                SystemManager.Instance().ModelManager.Refresh();
                RefreshModelList();
            }
        }

        public void ModelChanged() { }

        public void ModelTeachDone(int camId) { RefreshModelList(); }

        public void ModelRefreshed() { RefreshModelList(); }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void modelList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ModelSelect();
        }

        public void UserChanged()
        {
            modelList.ContextMenuStrip = UserHandler.Instance().CurrentUser.IsMasterAccount ? this.contextMenuStrip1 : null;
            RefreshModelList();
        }

        private void openExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (modelList.SelectedRows.Count != 1)
                return;

            ModelDescription modelDescription = (ModelDescription)modelList.SelectedRows[0].Tag;
            string path = SystemManager.Instance().ModelManager.GetModelPath(modelDescription);
            if (System.IO.Directory.Exists(path))
                System.Diagnostics.Process.Start(path);
        }
    }
}
