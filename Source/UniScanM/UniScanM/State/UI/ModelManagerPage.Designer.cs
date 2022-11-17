namespace UniScanM.UI
{
    partial class ModelManagerPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelManagerPage));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.tableLayoutPanelModelList = new System.Windows.Forms.TableLayoutPanel();
            this.labelModelList = new Infragistics.Win.Misc.UltraLabel();
            this.findModel = new System.Windows.Forms.TextBox();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLastModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new Infragistics.Win.Misc.UltraLabel();
            this.labelTotal = new Infragistics.Win.Misc.UltraLabel();
            this.buttonApply = new Infragistics.Win.Misc.UltraButton();
            this.columnTaught = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelParam = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.panelParamTop = new System.Windows.Forms.Panel();
            this.labelParam = new Infragistics.Win.Misc.UltraLabel();
            this.panelSideButton = new System.Windows.Forms.Panel();
            this.buttonDelete = new Infragistics.Win.Misc.UltraButton();
            this.buttonNew = new Infragistics.Win.Misc.UltraButton();
            this.buttonSelect = new Infragistics.Win.Misc.UltraButton();
            this.tableLayoutPanelModelList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.panelParam.SuspendLayout();
            this.panelParamTop.SuspendLayout();
            this.panelSideButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelModelList
            // 
            resources.ApplyResources(this.tableLayoutPanelModelList, "tableLayoutPanelModelList");
            this.tableLayoutPanelModelList.Controls.Add(this.labelModelList, 0, 0);
            this.tableLayoutPanelModelList.Controls.Add(this.findModel, 2, 1);
            this.tableLayoutPanelModelList.Controls.Add(this.modelList, 0, 2);
            this.tableLayoutPanelModelList.Controls.Add(this.total, 1, 1);
            this.tableLayoutPanelModelList.Controls.Add(this.labelTotal, 0, 1);
            this.tableLayoutPanelModelList.Name = "tableLayoutPanelModelList";
            // 
            // labelModelList
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance1, "appearance1");
            this.labelModelList.Appearance = appearance1;
            this.tableLayoutPanelModelList.SetColumnSpan(this.labelModelList, 3);
            resources.ApplyResources(this.labelModelList, "labelModelList");
            this.labelModelList.Name = "labelModelList";
            // 
            // findModel
            // 
            this.findModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.findModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.findModel, "findModel");
            this.findModel.Name = "findModel";
            this.findModel.TextChanged += new System.EventHandler(this.findModel_TextChanged);
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeRows = false;
            this.modelList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.modelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.modelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnRegistrationDate,
            this.columnLastModifiedDate,
            this.columnComment});
            this.tableLayoutPanelModelList.SetColumnSpan(this.modelList, 3);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 14F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.modelList.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.modelList, "modelList");
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.ReadOnly = true;
            this.modelList.RowHeadersVisible = false;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.SelectionChanged += new System.EventHandler(this.modelList_SelectionChanged);
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.columnName, "columnName");
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // columnRegistrationDate
            // 
            this.columnRegistrationDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnRegistrationDate, "columnRegistrationDate");
            this.columnRegistrationDate.Name = "columnRegistrationDate";
            this.columnRegistrationDate.ReadOnly = true;
            // 
            // columnLastModifiedDate
            // 
            this.columnLastModifiedDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnLastModifiedDate, "columnLastModifiedDate");
            this.columnLastModifiedDate.Name = "columnLastModifiedDate";
            this.columnLastModifiedDate.ReadOnly = true;
            // 
            // columnComment
            // 
            this.columnComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnComment, "columnComment");
            this.columnComment.Name = "columnComment";
            this.columnComment.ReadOnly = true;
            // 
            // total
            // 
            appearance2.FontData.Name = resources.GetString("resource.Name1");
            appearance2.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints1")));
            appearance2.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance2, "appearance2");
            this.total.Appearance = appearance2;
            resources.ApplyResources(this.total, "total");
            this.total.Name = "total";
            // 
            // labelTotal
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance3.FontData.Name = resources.GetString("resource.Name2");
            appearance3.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints2")));
            appearance3.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance3, "appearance3");
            this.labelTotal.Appearance = appearance3;
            resources.ApplyResources(this.labelTotal, "labelTotal");
            this.labelTotal.Name = "labelTotal";
            // 
            // buttonApply
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.FontData.BoldAsString = resources.GetString("resource.BoldAsString");
            appearance4.FontData.Name = resources.GetString("resource.Name3");
            appearance4.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints3")));
            this.buttonApply.Appearance = appearance4;
            this.buttonApply.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            resources.ApplyResources(this.buttonApply, "buttonApply");
            this.buttonApply.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // columnTaught
            // 
            this.columnTaught.Name = "columnTaught";
            // 
            // panelParam
            // 
            this.panelParam.Controls.Add(this.propertyGrid);
            this.panelParam.Controls.Add(this.panelParamTop);
            resources.ApplyResources(this.panelParam, "panelParam");
            this.panelParam.Name = "panelParam";
            // 
            // propertyGrid
            // 
            resources.ApplyResources(this.propertyGrid, "propertyGrid");
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Name = "propertyGrid";
            // 
            // panelParamTop
            // 
            this.panelParamTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelParamTop.Controls.Add(this.labelParam);
            this.panelParamTop.Controls.Add(this.buttonApply);
            resources.ApplyResources(this.panelParamTop, "panelParamTop");
            this.panelParamTop.Name = "panelParamTop";
            // 
            // labelParam
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance5.FontData.Name = resources.GetString("resource.Name4");
            appearance5.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints4")));
            appearance5.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance5, "appearance5");
            this.labelParam.Appearance = appearance5;
            resources.ApplyResources(this.labelParam, "labelParam");
            this.labelParam.Name = "labelParam";
            // 
            // panelSideButton
            // 
            this.panelSideButton.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSideButton.Controls.Add(this.buttonDelete);
            this.panelSideButton.Controls.Add(this.buttonNew);
            this.panelSideButton.Controls.Add(this.buttonSelect);
            resources.ApplyResources(this.panelSideButton, "panelSideButton");
            this.panelSideButton.Name = "panelSideButton";
            // 
            // buttonDelete
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance6.FontData.BoldAsString = resources.GetString("resource.BoldAsString1");
            appearance6.FontData.Name = resources.GetString("resource.Name5");
            appearance6.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints5")));
            appearance6.Image = global::UniScanM.Properties.Resources.delete_model;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance6.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonDelete.Appearance = appearance6;
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.FontData.BoldAsString = resources.GetString("resource.BoldAsString2");
            appearance7.FontData.Name = resources.GetString("resource.Name6");
            appearance7.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints6")));
            appearance7.Image = global::UniScanM.Properties.Resources.new_model;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonNew.Appearance = appearance7;
            resources.ApplyResources(this.buttonNew, "buttonNew");
            this.buttonNew.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonSelect
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString3");
            appearance8.FontData.Name = resources.GetString("resource.Name7");
            appearance8.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints7")));
            appearance8.Image = global::UniScanM.Properties.Resources.select_model;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance8.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonSelect.Appearance = appearance8;
            resources.ApplyResources(this.buttonSelect, "buttonSelect");
            this.buttonSelect.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // ModelManagerPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelModelList);
            this.Controls.Add(this.panelParam);
            this.Controls.Add(this.panelSideButton);
            this.Name = "ModelManagerPage";
            this.Load += new System.EventHandler(this.ModelManagerPage_Load);
            this.tableLayoutPanelModelList.ResumeLayout(false);
            this.tableLayoutPanelModelList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.panelParam.ResumeLayout(false);
            this.panelParamTop.ResumeLayout(false);
            this.panelSideButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraButton buttonApply;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTaught;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelModelList;
        private Infragistics.Win.Misc.UltraLabel labelModelList;
        private Infragistics.Win.Misc.UltraLabel labelTotal;
        private System.Windows.Forms.TextBox findModel;
        private Infragistics.Win.Misc.UltraLabel total;
        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Panel panelSideButton;
        private Infragistics.Win.Misc.UltraButton buttonDelete;
        private Infragistics.Win.Misc.UltraButton buttonNew;
        private Infragistics.Win.Misc.UltraButton buttonSelect;
        private System.Windows.Forms.Panel panelParamTop;
        private Infragistics.Win.Misc.UltraLabel labelParam;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLastModifiedDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnComment;
    }
}
