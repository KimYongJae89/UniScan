using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DynMvp.Authentication;
using UniScanG.Data;
using DynMvp.Base;
using UniScanG.Data.Model;
using UniEye.Base.UI;

namespace UniScanG.Data.UI
{
    internal partial class ModelForm : Form, IMultiLanguageSupport
    {
        string initModelName;
        internal string InitModelName
        {
            get { return initModelName; }
            set { initModelName = value; }
        }

        ModelDescription modelDescription = null;
        public ModelDescription ModelDescription
        {
            get { return modelDescription; }
            set { modelDescription = value; }
        }

        ModelFormType modelFormType;
        public ModelFormType ModelFormType
        {
            get { return modelFormType; }
            set { modelFormType = value; }
        }
        
        public ModelForm()
        {
            InitializeComponent();
            
            StringManager.AddListener(this);
        }

        private void ModelForm_Load(object sender, EventArgs e)
        {
            if (modelDescription == null)
            {
                Text = StringManager.GetString(this.GetType().FullName, "New Model");

                modelName.Text = initModelName;
                registrant.Text = UserHandler.Instance().CurrentUser.Id;
            }
            else
            {
                if (modelFormType == ModelFormType.Edit)
                {
                    Text = StringManager.GetString(this.GetType().FullName, "Edit Model");

                    groupFundamental.Visible = false;
                }
                else
                {
                    Text = StringManager.GetString(this.GetType().FullName, "Copy Model");
                }

                SetModelData();
            }
        }

        private void SetModelData()
        {
            modelName.Text = modelDescription.Name;
            thickness.Value = (decimal)modelDescription.Thickness;
            paste.Text = modelDescription.Paste.ToString();
            registrant.Text = modelDescription.Registrant;
            description.Text = modelDescription.Description;
        }

        private void GetModelData()
        {
            modelDescription.Name = modelName.Text;
            modelDescription.Thickness = (float)thickness.Value;
            modelDescription.Paste = paste.Text == null ? "" : paste.Text;
            modelDescription.Registrant = registrant.Text;
            modelDescription.Description = description.Text;
        }

        private void EditModelData()
        {
            modelDescription.Paste = paste.Text;
            modelDescription.Registrant = registrant.Text;
            modelDescription.Description = description.Text;

            SystemManager.Instance().ModelManager.SaveModelDescription(modelDescription);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (modelFormType)
            {
                case ModelFormType.New:
                    if (!ControlValidate(this.modelName.Text, @"[A-Z0-9-_]"))
                    //if (string.IsNullOrEmpty(this.modelName.Text))
                    {
                        this.modelName.Select(0, modelName.Text.Length);
                        errorProvider.SetError(this.modelName, "Invalid Model Name");
                        return;
                    }

                    if (!ControlValidate(this.thickness.Text, @"[0-9.]"))
                    //if (String.IsNullOrEmpty(thickness.Text) || float.Parse(thickness.Text) <= 0)
                    {
                        this.thickness.Select(0, modelName.Text.Length);
                        errorProvider.SetError(thickness, "Invalid Thickness");
                        return;
                    }

                    if (!ControlValidate(this.paste.Text, @"[A-Z0-9]"))
                    //if (String.IsNullOrEmpty(paste.Text))
                    {
                        this.paste.Select(0, modelName.Text.Length);
                        errorProvider.SetError(paste, "Invalid Paste");
                        return;
                    }

                    modelDescription = (ModelDescription)SystemManager.Instance().ModelManager.CreateModelDescription();
                    GetModelData();
                    if (SystemManager.Instance().ModelManager.ModelDescriptionList.Contains(modelDescription))
                    //if(SystemManager.Instance().ModelManager.ModelDescriptionList.Exists(f=>f.Name == modelDescription.Name))
                    {
                        errorProvider.SetError(this.modelName, "Duplicated Model Name");
                        return;
                    }
                    break;

                case ModelFormType.Edit:
                    EditModelData();
                    break;
            }
            
            DialogResult = DialogResult.OK;
            
            Close();
        }

        private int GetModelCount()
        {
            return 0;
        }


        private bool ControlValidate(string text, string regex)
        {
            if (string.IsNullOrEmpty(text))
                return false;

            Regex rgx = new Regex(regex);
            MatchCollection matches = rgx.Matches(text);
            return !(matches.Count != text.Length);
        }

        private void modelName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ControlValidate(this.modelName.Text, @"[A-Z0-9-_]"))
                errorProvider.SetError(this.modelName, "Invalid Model Name");
            else
                errorProvider.Clear();
        }

        private void thickness_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ControlValidate(this.thickness.Text, @"[0-9.]"))
                errorProvider.SetError(thickness, "Invalid Thickness");
            else
                errorProvider.Clear();
        }

        private void paste_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!ControlValidate(this.paste.Text, @"[A-Z0-9]"))
                errorProvider.SetError(paste, "Invalid Paste");
            else
                errorProvider.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
