using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DynMvp.Authentication;
using DynMvp.Base;
using UniEye.Base.UI;
using UniScanM.Data;

namespace UniScanM.UI
{
    public partial class ModelForm : Form, IMultiLanguageSupport
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
            }
            else
            {
                if (modelFormType == ModelFormType.Edit)
                {
                    Text = StringManager.GetString(this.GetType().FullName, "Edit Model");

                    //groupFundamental.Visible = false;
                    modelName.Enabled = false;
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
            paste.Text = modelDescription.Paste.ToString();
            registrant.Text = modelDescription.Registrant;
            description.Text = modelDescription.Description;
        }

        private void GetModelData()
        {
            modelDescription.Name = modelName.Text;
            modelDescription.Paste = paste.Text;
            modelDescription.Registrant = registrant.Text;
            modelDescription.Description = description.Text;

            SystemManager.Instance().ModelManager.SaveModelDescription(modelDescription);
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
            string modelName = null;

            switch (modelFormType)
            {
                case ModelFormType.New:
                    if (string.IsNullOrEmpty(this.modelName.Text))
                    {
                        errorProvider.SetError(this.modelName, "Invalid Model Name");
                        return;
                    }

                    if (String.IsNullOrEmpty(thickness.Text))
                    {
                        errorProvider.SetError(thickness, "Invalid Thickness");
                        return;
                    }

                    if (float.Parse(thickness.Text) <= 0)
                    {
                        errorProvider.SetError(thickness, "Invalid Thickness");
                        return;
                    }

                    if (String.IsNullOrEmpty(paste.Text))
                    {
                        errorProvider.SetError(paste, "Invalid Paste");
                        return;
                    }
                    
                    modelDescription = (ModelDescription)SystemManager.Instance().ModelManager.CreateModelDescription();
                    GetModelData();
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

        private void screenModel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex rgx = new Regex(@"[A-Z0-9-]");
            MatchCollection matches = rgx.Matches(modelName.Text);
            //e.Cancel = ();
            e.Cancel = matches.Count != modelName.Text.Length; 
            if (e.Cancel == true)
            {
                modelName.Select(0, modelName.Text.Length);

                errorProvider.SetError(modelName, "Invalid Model Name");
            }
            else
            {
                errorProvider.Clear();
            }
        }

        private void thickness_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex rgx = new Regex(@"[0-9.]");
            MatchCollection matches = rgx.Matches(thickness.Text);
            e.Cancel = matches.Count != thickness.Text.Length;
            if (e.Cancel == true)
            {
                thickness.Select(0, thickness.Text.Length);
                errorProvider.SetError(thickness, "Invalid Thickness");
            }
            else
            {
                errorProvider.Clear();
            }
        }
        
        private void paste_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex rgx = new Regex(@"[A-Z0-9]");
            MatchCollection matches = rgx.Matches(paste.Text);
            e.Cancel = matches.Count != paste.Text.Length;
            if (e.Cancel == true)
            {
                paste.Select(0, paste.Text.Length);
                errorProvider.SetError(paste, "Invalid Paste");
            }
            else
            {
                errorProvider.Clear();
            }
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
