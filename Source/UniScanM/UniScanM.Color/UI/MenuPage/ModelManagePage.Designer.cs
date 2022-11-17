

namespace UniScanM.ColorSens.UI.MenuPage
{
    partial class ModelManagePage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelManagePage));
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.totalModel = new Infragistics.Win.Misc.UltraLabel();
            this.findModel = new System.Windows.Forms.TextBox();
            this.labelModelList = new Infragistics.Win.Misc.UltraLabel();
            this.buttonSelect = new Infragistics.Win.Misc.UltraButton();
            this.buttonNew = new Infragistics.Win.Misc.UltraButton();
            this.buttonDelete = new Infragistics.Win.Misc.UltraButton();
            this.buttonTeach = new Infragistics.Win.Misc.UltraButton();
            this.panel_ModelProperty = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid_Model = new System.Windows.Forms.PropertyGrid();
            this.panel_Buttons = new System.Windows.Forms.Panel();
            this.button_Reload = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.columnEtc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLastModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnScreenModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.panelModelList = new System.Windows.Forms.Panel();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel_ModelProperty.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_Buttons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.panelModelList.SuspendLayout();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // totalModel
            // 
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.totalModel.Appearance = appearance1;
            this.totalModel.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.totalModel.Font = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.totalModel.Location = new System.Drawing.Point(3, 53);
            this.totalModel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.totalModel.Name = "totalModel";
            this.totalModel.Size = new System.Drawing.Size(118, 43);
            this.totalModel.TabIndex = 52;
            this.totalModel.Text = "Total : 100";
            // 
            // findModel
            // 
            this.findModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.findModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.findModel.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.findModel.Location = new System.Drawing.Point(124, 53);
            this.findModel.Name = "findModel";
            this.findModel.Size = new System.Drawing.Size(670, 43);
            this.findModel.TabIndex = 50;
            // 
            // labelModelList
            // 
            this.labelModelList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance2.FontData.Name = "Malgun Gothic";
            appearance2.FontData.SizeInPoints = 22F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.labelModelList.Appearance = appearance2;
            this.labelModelList.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelModelList.Font = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelModelList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModelList.Location = new System.Drawing.Point(3, 2);
            this.labelModelList.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelModelList.Name = "labelModelList";
            this.labelModelList.Size = new System.Drawing.Size(791, 45);
            this.labelModelList.TabIndex = 51;
            this.labelModelList.Text = "Model List";
            // 
            // buttonSelect
            // 
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Malgun Gothic";
            appearance3.FontData.SizeInPoints = 16F;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance3.TextVAlignAsString = "Bottom";
            this.buttonSelect.Appearance = appearance3;
            this.buttonSelect.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonSelect.Location = new System.Drawing.Point(3, 6);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(100, 100);
            this.buttonSelect.TabIndex = 27;
            this.buttonSelect.Text = "Select";
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonNew
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 16F;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance4.TextVAlignAsString = "Bottom";
            this.buttonNew.Appearance = appearance4;
            this.buttonNew.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonNew.Location = new System.Drawing.Point(3, 112);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(100, 100);
            this.buttonNew.TabIndex = 26;
            this.buttonNew.Text = "New";
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonDelete
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Malgun Gothic";
            appearance5.FontData.SizeInPoints = 16F;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance5.TextVAlignAsString = "Bottom";
            this.buttonDelete.Appearance = appearance5;
            this.buttonDelete.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonDelete.Location = new System.Drawing.Point(3, 218);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(100, 100);
            this.buttonDelete.TabIndex = 26;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonTeach
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 16F;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance6.TextVAlignAsString = "Bottom";
            this.buttonTeach.Appearance = appearance6;
            this.buttonTeach.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonTeach.Location = new System.Drawing.Point(3, 324);
            this.buttonTeach.Name = "buttonTeach";
            this.buttonTeach.Size = new System.Drawing.Size(100, 100);
            this.buttonTeach.TabIndex = 28;
            this.buttonTeach.Text = "Teach";
            this.buttonTeach.Click += new System.EventHandler(this.buttonTeach_Click);
            // 
            // panel_ModelProperty
            // 
            this.panel_ModelProperty.Controls.Add(this.panel2);
            this.panel_ModelProperty.Controls.Add(this.panel1);
            this.panel_ModelProperty.Controls.Add(this.panel_Buttons);
            this.panel_ModelProperty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_ModelProperty.Location = new System.Drawing.Point(909, 3);
            this.panel_ModelProperty.Name = "panel_ModelProperty";
            this.panel_ModelProperty.Size = new System.Drawing.Size(621, 655);
            this.panel_ModelProperty.TabIndex = 56;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(311, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(310, 555);
            this.panel2.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid_Model);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 555);
            this.panel1.TabIndex = 4;
            // 
            // propertyGrid_Model
            // 
            this.propertyGrid_Model.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid_Model.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid_Model.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid_Model.Name = "propertyGrid_Model";
            this.propertyGrid_Model.Size = new System.Drawing.Size(311, 555);
            this.propertyGrid_Model.TabIndex = 1;
            // 
            // panel_Buttons
            // 
            this.panel_Buttons.Controls.Add(this.button_Reload);
            this.panel_Buttons.Controls.Add(this.button_Save);
            this.panel_Buttons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Buttons.Location = new System.Drawing.Point(0, 555);
            this.panel_Buttons.Name = "panel_Buttons";
            this.panel_Buttons.Size = new System.Drawing.Size(621, 100);
            this.panel_Buttons.TabIndex = 3;
            // 
            // button_Reload
            // 
            this.button_Reload.Location = new System.Drawing.Point(6, 25);
            this.button_Reload.Name = "button_Reload";
            this.button_Reload.Size = new System.Drawing.Size(118, 46);
            this.button_Reload.TabIndex = 2;
            this.button_Reload.Text = "Reset";
            this.button_Reload.UseVisualStyleBackColor = true;
            this.button_Reload.Click += new System.EventHandler(this.button_Reload_Click);
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(130, 25);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(118, 46);
            this.button_Save.TabIndex = 2;
            this.button_Save.Text = "적용";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // columnEtc
            // 
            this.columnEtc.HeaderText = "Etc.";
            this.columnEtc.Name = "columnEtc";
            this.columnEtc.Width = 150;
            // 
            // columnLastModifiedDate
            // 
            this.columnLastModifiedDate.HeaderText = "Last Modified";
            this.columnLastModifiedDate.Name = "columnLastModifiedDate";
            this.columnLastModifiedDate.Width = 150;
            // 
            // columnRegistrationDate
            // 
            this.columnRegistrationDate.HeaderText = "Registration Date";
            this.columnRegistrationDate.Name = "columnRegistrationDate";
            this.columnRegistrationDate.Width = 150;
            // 
            // columnRegistrant
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnRegistrant.DefaultCellStyle = dataGridViewCellStyle1;
            this.columnRegistrant.HeaderText = "Registrant";
            this.columnRegistrant.Name = "columnRegistrant";
            this.columnRegistrant.Width = 150;
            // 
            // columnScreenModel
            // 
            this.columnScreenModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnScreenModel.HeaderText = "Model";
            this.columnScreenModel.Name = "columnScreenModel";
            // 
            // columnNo
            // 
            this.columnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Width = 50;
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeColumns = false;
            this.modelList.AllowUserToResizeRows = false;
            this.modelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.modelList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnScreenModel,
            this.columnRegistrant,
            this.columnRegistrationDate,
            this.columnLastModifiedDate,
            this.columnEtc});
            this.modelList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.modelList.Location = new System.Drawing.Point(3, 99);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.RowHeadersVisible = false;
            this.modelList.RowTemplate.Height = 23;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.Size = new System.Drawing.Size(791, 550);
            this.modelList.TabIndex = 50;
            // 
            // panelModelList
            // 
            this.panelModelList.Controls.Add(this.totalModel);
            this.panelModelList.Controls.Add(this.findModel);
            this.panelModelList.Controls.Add(this.labelModelList);
            this.panelModelList.Controls.Add(this.modelList);
            this.panelModelList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelModelList.Location = new System.Drawing.Point(111, 3);
            this.panelModelList.Name = "panelModelList";
            this.panelModelList.Size = new System.Drawing.Size(798, 655);
            this.panelModelList.TabIndex = 55;
            // 
            // menuPanel
            // 
            this.menuPanel.Controls.Add(this.buttonSelect);
            this.menuPanel.Controls.Add(this.buttonNew);
            this.menuPanel.Controls.Add(this.buttonDelete);
            this.menuPanel.Controls.Add(this.buttonTeach);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuPanel.Location = new System.Drawing.Point(3, 3);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.menuPanel.Size = new System.Drawing.Size(108, 655);
            this.menuPanel.TabIndex = 54;
            // 
            // ModelManagePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_ModelProperty);
            this.Controls.Add(this.panelModelList);
            this.Controls.Add(this.menuPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModelManagePage";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(1533, 661);
            this.Load += new System.EventHandler(this.ModelManagePage_Load);
            this.panel_ModelProperty.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel_Buttons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.panelModelList.ResumeLayout(false);
            this.panelModelList.PerformLayout();
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel totalModel;
        private System.Windows.Forms.TextBox findModel;
        private Infragistics.Win.Misc.UltraLabel labelModelList;
        private Infragistics.Win.Misc.UltraButton buttonSelect;
        private Infragistics.Win.Misc.UltraButton buttonNew;
        private Infragistics.Win.Misc.UltraButton buttonDelete;
        private Infragistics.Win.Misc.UltraButton buttonTeach;
        private System.Windows.Forms.Panel panel_ModelProperty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnEtc;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLastModifiedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrant;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnScreenModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.Panel panelModelList;
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private System.Windows.Forms.PropertyGrid propertyGrid_Model;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Panel panel_Buttons;
        private System.Windows.Forms.Button button_Reload;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}
