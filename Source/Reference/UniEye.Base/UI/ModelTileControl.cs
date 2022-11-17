using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using DynMvp.Data.UI;
using Infragistics.Win.Misc;
using DynMvp.Base;
using DynMvp.UI.Touch;
using DynMvp.Data;
using DynMvp.Authentication;

namespace UniEye.Base.UI
{
    public delegate void ModelSelected(ModelDescription modelDescription);
    public delegate void CloseModelDelegate();

    public partial class ModelTileControl : UserControl, IMainTabPage
    {
        ModelManager modelManager;
        public ModelManager ModelManager
        {
            set { modelManager = value; }
        }

        UltraButton selectedModelButton;

        Infragistics.Win.Appearance appearanceModelButton = new Infragistics.Win.Appearance();

        public ModelSelected ModelSelected;
        public CloseModelDelegate CloseModel;

        public ModelTileControl()
        {
            InitializeComponent();

            buttonNewModel.Text = StringManager.GetString(this.GetType().FullName, buttonNewModel.Text);
            labelCategory.Text = StringManager.GetString(this.GetType().FullName, labelCategory.Text);
            buttonRefresh.Text = StringManager.GetString(this.GetType().FullName, buttonRefresh.Text);
            labelFind.Text = StringManager.GetString(this.GetType().FullName, labelFind.Text);
        }

        private void ModelTileControl_Load(object sender, EventArgs e)
        {
            modelManager = SystemManager.Instance().ModelManager;

            appearanceModelButton.BackColor = System.Drawing.Color.LemonChiffon;
            appearanceModelButton.FontData.Name = "NanumGothic";
            appearanceModelButton.FontData.SizeInPoints = 12F;
            appearanceModelButton.ForeColor = System.Drawing.Color.Black;
            appearanceModelButton.TextHAlignAsString = "Left";
            appearanceModelButton.TextVAlignAsString = "Bottom";

            buttonNewModel.Size = new System.Drawing.Size(150, 150);

            RefreshList();
        }

        private void RefreshList()
        { 
            if (modelManager == null || modelManager.Count() == 0)
                return;

            modelManager.Refresh();

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add(StringManager.GetString(this.GetType().FullName, "All"));

            foreach (string category in modelManager.CategoryList)
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;
        }

        private void AddModelButton(ModelDescription modelDescription)
        {
            UltraButton modelButton = new UltraButton();

            modelButton.Appearance = appearanceModelButton;
            modelButton.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            modelButton.ImageSize = new System.Drawing.Size(116, 116);
            modelButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            modelButton.Location = new System.Drawing.Point(3, 3);
            modelButton.Name = "buttonModel";
            modelButton.Size = new System.Drawing.Size(150, 150);
            modelButton.TabIndex = 1;
            modelButton.Tag = modelDescription;
            modelButton.Text = modelDescription.Name;
            modelButton.UseAppStyling = false;
            modelButton.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            modelButton.Click += modelButton_DoubleClick;
            modelButton.MouseUp += modelButton_MouseUp;
            modelButton.PressAndHoldGesture += modelButton_PressAndHoldGesture;
            panelModelList.Controls.Add(modelButton);
        }

        private void modelButton_DoubleClick(object sender, EventArgs e)
        {
            if (ModelSelected != null)
            {
                selectedModelButton = (UltraButton)sender;
                ModelSelected((ModelDescription)selectedModelButton.Tag);
            }
        }

        void modelButton_PressAndHoldGesture(object sender, Infragistics.Win.Touch.PressAndHoldGestureEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void modelButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Size menuSize = modelMenu.MenuSettings.Size;
                selectedModelButton = (UltraButton)sender;
                Point point = selectedModelButton.PointToScreen(
                        new Point(selectedModelButton.Width - menuSize.Width / 2, selectedModelButton.Height - menuSize.Height / 2));
                modelMenu.Show(ParentForm, point);
            }
        }

        private void buttonNewModel_Click(object sender, EventArgs e)
        {
            CreateNewModel("");
        }

        private void CreateNewModel(string modelName)
        {
            ModelForm newModelForm = new ModelForm();
            newModelForm.InitModelName = modelName;
            newModelForm.ModelManager = modelManager;
            newModelForm.ModelFormType = ModelFormType.New;
            if (newModelForm.ShowDialog(this) == DialogResult.OK)
            {
                ModelDescription md = newModelForm.ModelDescription;
                modelManager.AddModel(md);
                AddModelButton(md);
            }
        }

        private void modelMenu_ToolClick(object sender, Infragistics.Win.UltraWinRadialMenu.RadialMenuToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "Edit":
                    EditModelDescription();
                    break;
                case "Delete":
                    DeleteModelDescription();
                    break;
                case "Copy":
                    CopyModel();
                    break;
                case "Close":
                    CloseModel?.Invoke();
                    break;
                case "ExportFormat":
                    Model curModel = SystemManager.Instance().CurrentModel;
                    if (curModel != null && (curModel.Name == selectedModelButton.Text))
                    {
                        OutputFormatForm form = new OutputFormatForm();
                        form.Model = curModel;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            modelManager.SaveModelDescription((ModelDescription)curModel.ModelDescription);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please, select the model first.");
                    }
                    break;
            }

            modelMenu.Hide();
        }

        private void CopyModel()
        {
            ModelForm copyModelForm = new ModelForm();
            copyModelForm.ModelFormType = ModelFormType.Copy;
            copyModelForm.ModelManager = modelManager;
            copyModelForm.ModelDescription = (ModelDescription)selectedModelButton.Tag;
           
            if (copyModelForm.ShowDialog(this) == DialogResult.OK)
            {
                ModelDescription copyMd = copyModelForm.ModelDescription;

                modelManager.AddModel(copyMd);
                AddModelButton(copyMd);

                modelManager.CopyModelData(copyModelForm.ModelDescription, copyMd);
            }
        }

        private void EditModelDescription()
        {
            ModelForm editModelForm = new ModelForm();
            editModelForm.ModelFormType = ModelFormType.Edit;
            editModelForm.ModelManager = modelManager;
            editModelForm.ModelDescription = (ModelDescription)selectedModelButton.Tag;
            if (editModelForm.ShowDialog(this) == DialogResult.OK)
            {
                modelManager.EditModel(editModelForm.ModelDescription);
            }
        }

        private void DeleteModelDescription()
        {
            ModelDescription md = (ModelDescription)selectedModelButton.Tag;
            if (MessageBox.Show(this, String.Format("Do you want to delete the model[{0}]?", md.Name), "Delete Model", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                modelManager.DeleteModel(md);

                panelModelList.Controls.Remove(selectedModelButton);
                selectedModelButton = null;
            }
        }

        private void panelModelList_MouseUp(object sender, MouseEventArgs e)
        {
            modelMenu.Hide();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelModelList.Controls.Clear();
            panelModelList.Controls.Add(buttonNewModel);

            foreach (ModelDescription modelDescription in modelManager)
            {
                if (cmbCategory.Text == StringManager.GetString(this.GetType().FullName, "All") || cmbCategory.Text == modelDescription.Category)
                    AddModelButton(modelDescription);
            }
        }

        private void buttonCloseMode_Click(object sender, EventArgs e)
        {
            CloseModel?.Invoke();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            panelModelList.Controls.Clear();
            panelModelList.Controls.Add(buttonNewModel);

            foreach (ModelDescription modelDescription in modelManager)
            {
                if (searchModelName.Text == "" || searchModelName.Text == modelDescription.Name)
                    AddModelButton(modelDescription);
            }

            if (panelModelList.Controls.Count == 1)
            {
                if (MessageForm.Show(ParentForm, String.Format("Do you want to create the model [{0}]?", searchModelName.Text), MessageFormType.YesNo) == DialogResult.Yes)
                {
                    CreateNewModel(searchModelName.Text);
                }
            }
        }

        private void searchModelName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                panelModelList.Controls.Clear();
                panelModelList.Controls.Add(buttonNewModel);

                ModelDescription lastModelDescription = null;
                foreach (ModelDescription modelDescription in modelManager)
                {
                    if (searchModelName.Text == "" || searchModelName.Text == modelDescription.Name)
                    {
                        AddModelButton(modelDescription);
                        lastModelDescription = modelDescription;
                    }
                }

                if (panelModelList.Controls.Count == 1)
                {
                    if (MessageForm.Show(ParentForm, String.Format("Do you want to create the model [{0}]?", searchModelName.Text), MessageFormType.YesNo) == DialogResult.Yes)
                    {
                        CreateNewModel(searchModelName.Text);
                    }
                }
                else if (panelModelList.Controls.Count == 2)
                {
                    if (ModelSelected != null && lastModelDescription != null)
                    {
                        ModelSelected(lastModelDescription);
                    }
                }
            }
        }

        public void EnableControls(UserType user)
        {

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
