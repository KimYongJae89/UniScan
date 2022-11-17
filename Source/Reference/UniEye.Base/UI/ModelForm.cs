using System;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Base;

namespace UniEye.Base.UI
{
    public enum ModelFormType
    {
        New, Edit, Copy
    }

    public interface ModelFormExtraProperty
    {
        void Initialize(ModelDescription modelDescription);
        void GetModelData(ModelDescription modelDescription);
    }

    public partial class ModelForm : Form, IMultiLanguageSupport
    {
        string initModelName;
        internal string InitModelName
        {
            get { return initModelName; }
            set { initModelName = value; }
        }

        ModelManager modelManager;
        internal ModelManager ModelManager
        {
            get { return modelManager; }
            set { modelManager = value; }
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

        ModelFormExtraProperty extraProperty = null;

        public ModelForm()
        {
            InitializeComponent();

            label1.Text = StringManager.GetString(this.GetType().FullName,label1.Text);

            // language change
            //labelModelName.Text = StringManager.GetString(this.GetType().FullName,labelModelName.Text);
            //labelDescription.Text = StringManager.GetString(this.GetType().FullName,labelDescription.Text);
            //labelProductName.Text = StringManager.GetString(this.GetType().FullName,labelProductName.Text);
            //labelItemCode.Text = StringManager.GetString(this.GetType().FullName,labelItemCode.Text);
            //btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            //btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);

            //SystemType systemType = Settings.Instance().Operation.SystemType;

            //switch(systemType)
            //{
            //    case SystemType.FPCBAlignChecker:
            //    case SystemType.FPCBAlignChecker2:
            //        {
            //            ModelPropertyFpcbAlignCheckPanel modelPropertyFpcbAlignCheckPanel = new ModelPropertyFpcbAlignCheckPanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyFpcbAlignCheckPanel);

            //            modelPropertyFpcbAlignCheckPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyFpcbAlignCheckPanel.Name = "modelPropertyFpcbAlignCheckPanel";
            //            modelPropertyFpcbAlignCheckPanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyFpcbAlignCheckPanel.TabIndex = 147;

            //            extraProperty = modelPropertyFpcbAlignCheckPanel;
            //        }

            //        break;
            //    case SystemType.FPCBAligner:
            //        {
            //            ModelPropertyFpcbAlignerPanel modelPropertyFpcbAlignerPanel = new ModelPropertyFpcbAlignerPanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyFpcbAlignerPanel);

            //            modelPropertyFpcbAlignerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyFpcbAlignerPanel.Name = "modelPropertyFpcbAlignerPanel";
            //            modelPropertyFpcbAlignerPanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyFpcbAlignerPanel.TabIndex = 147;

            //            extraProperty = modelPropertyFpcbAlignerPanel;
            //        }

            //        break;

            //    case SystemType.MaskInspector:
            //        {
            //            ModelPropertyMaskInspector modelPropertyMaskInspector = new ModelPropertyMaskInspector();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyMaskInspector);

            //            modelPropertyMaskInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyMaskInspector.Name = "modelPropertyMaskInspector";
            //            modelPropertyMaskInspector.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyMaskInspector.TabIndex = 147;

            //            extraProperty = modelPropertyMaskInspector;
            //        }

            //        break;
            //    case SystemType.BoxBarcode:
            //        {
            //            ModelPropertyBoxBarcodePanel modelPropertyBoxBarcodePanel = new ModelPropertyBoxBarcodePanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyBoxBarcodePanel);

            //            modelPropertyBoxBarcodePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyBoxBarcodePanel.Name = "modelPropertyBoxBarcodePanel";
            //            modelPropertyBoxBarcodePanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyBoxBarcodePanel.TabIndex = 147;

            //            extraProperty = modelPropertyBoxBarcodePanel;
            //        }
            //        break;
            //    case SystemType.ShampooBarcode:
            //        {
            //            ModelPropertyShampooBarcodePanel modelPropertyShampooBarcodePanel = new ModelPropertyShampooBarcodePanel();

            //            this.extraModelPropertyPanel.Controls.Add(modelPropertyShampooBarcodePanel);

            //            modelPropertyShampooBarcodePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            //            modelPropertyShampooBarcodePanel.Name = "modelPropertyBoxBarcodePanel";
            //            modelPropertyShampooBarcodePanel.Size = new System.Drawing.Size(404, 250);
            //            modelPropertyShampooBarcodePanel.TabIndex = 147;

            //            extraProperty = modelPropertyShampooBarcodePanel;
            //        }
            //        break;

            //}

            StringManager.AddListener(this);
        }

        private void ModelForm_Load(object sender, EventArgs e)
        {
            modelManager = SystemManager.Instance().ModelManager;

            foreach (string category in modelManager.CategoryList)
                cmbCategory.Items.Add(category);

            if (modelDescription == null)
            {
                Text = StringManager.GetString(this.GetType().FullName, "New Model");

                modelName.Text = initModelName;
//                int count = modelManager.NewModelExistCount(name);
            }
            else
            {
                if (modelFormType == ModelFormType.Edit)
                {
                    Text = StringManager.GetString(this.GetType().FullName, "Edit Model");
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
            cmbCategory.Text = modelDescription.Category;
            productName.Text = modelDescription.ProductName;
            itemCode.Text = modelDescription.ProductCode;
            description.Text = modelDescription.Description;

            if (extraProperty != null)
                extraProperty.Initialize(modelDescription);
        }

        private void GetModelData()
        {
            modelDescription.ProductCode = itemCode.Text;
            modelDescription.Name = modelName.Text;
            modelDescription.Category = cmbCategory.Text;
            modelDescription.ProductName = productName.Text;
            modelDescription.Description = description.Text;

            
            if (extraProperty != null)
                extraProperty.GetModelData(modelDescription);
            ModelManager.SaveModelDescription(modelDescription);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(modelName.Text))
                return;

            if (modelDescription == null || modelFormType == ModelFormType.Copy)
            {
                //if (modelManager.IsModelExist(modelName.Text))
                {
                    MessageBox.Show(this, StringManager.GetString(this.GetType().FullName, "The model name is exist. Please, use other model name."), "Error", MessageBoxButtons.OK);
                    return;
                }

                modelDescription = modelManager.CreateModelDescription();

                //SystemType systemType = Settings.Instance().Operation.SystemType;

                //switch (systemType)
                //{
                //    case SystemType.FPCBAlignChecker:
                //    case SystemType.FPCBAlignChecker2:
                //        modelDescription = new FpcbAlignCheckModelDescription();
                //        break;
                //    case SystemType.FPCBAligner:
                //        modelDescription = new FpcbAlignerModelDescription();
                //        break;
                //    case SystemType.BoxBarcode:
                //        modelDescription = new BoxBarcodeModelDescription();
                //        break;
                //    case SystemType.ShampooBarcode:
                //        ModelDescription = new ShampooBarcodeModelDescription();
                //        break;
                //    case SystemType.MaskInspector:
                //        ModelDescription = new MaskInspectorModelDescription();
                //        break;
                //    default:
                //        modelDescription = new UniEyeModelDescription();
                //        break;
                //}
            }

            DialogResult = DialogResult.OK;

            GetModelData();
            
            Close();
        }
        private int GetModelCount()
        {
            return 0;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
