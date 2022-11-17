namespace UniScanG.UI.Model
{
    partial class ModelPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelPage));
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
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.layoutImageView = new System.Windows.Forms.TableLayoutPanel();
            this.imageView = new System.Windows.Forms.TableLayoutPanel();
            this.camImage = new System.Windows.Forms.PictureBox();
            this.labelImage = new Infragistics.Win.Misc.UltraLabel();
            this.layoutModelList = new System.Windows.Forms.TableLayoutPanel();
            this.findModel = new System.Windows.Forms.TextBox();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPaste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTrained = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnRegistrant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRegistrationDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLastModifiedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new Infragistics.Win.Misc.UltraLabel();
            this.labelModelList = new Infragistics.Win.Misc.UltraLabel();
            this.labelTotal = new Infragistics.Win.Misc.UltraLabel();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonSelect = new Infragistics.Win.Misc.UltraButton();
            this.buttonTeach = new Infragistics.Win.Misc.UltraButton();
            this.buttonNew = new Infragistics.Win.Misc.UltraButton();
            this.buttonDelete = new Infragistics.Win.Misc.UltraButton();
            this.columnTaught = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            this.layoutImageView.SuspendLayout();
            this.imageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.camImage)).BeginInit();
            this.layoutModelList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainContainer
            // 
            this.mainContainer.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.mainContainer, "mainContainer");
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.layoutImageView);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.layoutModelList);
            resources.ApplyResources(this.mainContainer.Panel2, "mainContainer.Panel2");
            // 
            // layoutImageView
            // 
            resources.ApplyResources(this.layoutImageView, "layoutImageView");
            this.layoutImageView.Controls.Add(this.imageView, 0, 0);
            this.layoutImageView.Controls.Add(this.labelImage, 0, 0);
            this.layoutImageView.Name = "layoutImageView";
            // 
            // imageView
            // 
            this.imageView.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.imageView, "imageView");
            this.imageView.Controls.Add(this.camImage, 0, 0);
            this.imageView.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.imageView.Name = "imageView";
            // 
            // camImage
            // 
            resources.ApplyResources(this.camImage, "camImage");
            this.camImage.Name = "camImage";
            this.camImage.TabStop = false;
            // 
            // labelImage
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance1.FontData.Name = resources.GetString("resource.Name");
            appearance1.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints")));
            appearance1.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance1, "appearance1");
            this.labelImage.Appearance = appearance1;
            resources.ApplyResources(this.labelImage, "labelImage");
            this.labelImage.Name = "labelImage";
            // 
            // layoutModelList
            // 
            resources.ApplyResources(this.layoutModelList, "layoutModelList");
            this.layoutModelList.Controls.Add(this.findModel, 2, 1);
            this.layoutModelList.Controls.Add(this.modelList, 0, 2);
            this.layoutModelList.Controls.Add(this.total, 1, 1);
            this.layoutModelList.Controls.Add(this.labelModelList, 0, 0);
            this.layoutModelList.Controls.Add(this.labelTotal, 0, 1);
            this.layoutModelList.Name = "layoutModelList";
            // 
            // findModel
            // 
            this.findModel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.findModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.findModel, "findModel");
            this.findModel.Name = "findModel";
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
            this.columnThickness,
            this.columnPaste,
            this.columnTrained,
            this.columnRegistrant,
            this.columnRegistrationDate,
            this.columnLastModifiedDate});
            this.layoutModelList.SetColumnSpan(this.modelList, 3);
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
            // columnThickness
            // 
            this.columnThickness.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnThickness, "columnThickness");
            this.columnThickness.Name = "columnThickness";
            this.columnThickness.ReadOnly = true;
            // 
            // columnPaste
            // 
            this.columnPaste.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnPaste, "columnPaste");
            this.columnPaste.Name = "columnPaste";
            this.columnPaste.ReadOnly = true;
            // 
            // columnTrained
            // 
            this.columnTrained.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnTrained.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.columnTrained, "columnTrained");
            this.columnTrained.Name = "columnTrained";
            this.columnTrained.ReadOnly = true;
            this.columnTrained.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnTrained.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // columnRegistrant
            // 
            this.columnRegistrant.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.columnRegistrant, "columnRegistrant");
            this.columnRegistrant.Name = "columnRegistrant";
            this.columnRegistrant.ReadOnly = true;
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
            // labelModelList
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance3.FontData.Name = resources.GetString("resource.Name2");
            appearance3.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints2")));
            appearance3.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance3, "appearance3");
            this.labelModelList.Appearance = appearance3;
            this.layoutModelList.SetColumnSpan(this.labelModelList, 3);
            resources.ApplyResources(this.labelModelList, "labelModelList");
            this.labelModelList.Name = "labelModelList";
            // 
            // labelTotal
            // 
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance4.FontData.Name = resources.GetString("resource.Name3");
            appearance4.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints3")));
            appearance4.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(appearance4, "appearance4");
            this.labelTotal.Appearance = appearance4;
            resources.ApplyResources(this.labelTotal, "labelTotal");
            this.labelTotal.Name = "labelTotal";
            // 
            // menuPanel
            // 
            resources.ApplyResources(this.menuPanel, "menuPanel");
            this.menuPanel.Controls.Add(this.buttonSelect);
            this.menuPanel.Controls.Add(this.buttonTeach);
            this.menuPanel.Controls.Add(this.buttonNew);
            this.menuPanel.Controls.Add(this.buttonDelete);
            this.menuPanel.Name = "menuPanel";
            // 
            // buttonSelect
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.FontData.BoldAsString = resources.GetString("resource.BoldAsString");
            appearance5.FontData.Name = resources.GetString("resource.Name4");
            appearance5.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints4")));
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Top;
            resources.ApplyResources(appearance5, "appearance5");
            this.buttonSelect.Appearance = appearance5;
            this.buttonSelect.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonSelect.ImageSize = new System.Drawing.Size(45, 45);
            resources.ApplyResources(this.buttonSelect, "buttonSelect");
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // buttonTeach
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.FontData.BoldAsString = resources.GetString("resource.BoldAsString1");
            appearance6.FontData.Name = resources.GetString("resource.Name5");
            appearance6.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints5")));
            appearance6.Image = global::UniScanG.Properties.Resources.Teach_black;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            resources.ApplyResources(appearance6, "appearance6");
            this.buttonTeach.Appearance = appearance6;
            this.buttonTeach.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonTeach.ImageSize = new System.Drawing.Size(45, 45);
            resources.ApplyResources(this.buttonTeach, "buttonTeach");
            this.buttonTeach.Name = "buttonTeach";
            this.buttonTeach.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonTeach.Click += new System.EventHandler(this.buttonTeach_Click);
            // 
            // buttonNew
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.FontData.BoldAsString = resources.GetString("resource.BoldAsString2");
            appearance7.FontData.Name = resources.GetString("resource.Name6");
            appearance7.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints6")));
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Top;
            resources.ApplyResources(appearance7, "appearance7");
            this.buttonNew.Appearance = appearance7;
            this.buttonNew.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonNew.ImageSize = new System.Drawing.Size(45, 45);
            resources.ApplyResources(this.buttonNew, "buttonNew");
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonDelete
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.FontData.BoldAsString = resources.GetString("resource.BoldAsString3");
            appearance8.FontData.Name = resources.GetString("resource.Name7");
            appearance8.FontData.SizeInPoints = ((float)(resources.GetObject("resource.SizeInPoints7")));
            appearance8.Image = global::UniScanG.Properties.Resources.delete_model;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Top;
            resources.ApplyResources(appearance8, "appearance8");
            this.buttonDelete.Appearance = appearance8;
            this.buttonDelete.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonDelete.ImageSize = new System.Drawing.Size(45, 45);
            resources.ApplyResources(this.buttonDelete, "buttonDelete");
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // columnTaught
            // 
            this.columnTaught.Name = "columnTaught";
            // 
            // ModelPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.menuPanel);
            this.Name = "ModelPage";
            this.Load += new System.EventHandler(this.ModelManagePage_Load);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.layoutImageView.ResumeLayout(false);
            this.imageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.camImage)).EndInit();
            this.layoutModelList.ResumeLayout(false);
            this.layoutModelList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private Infragistics.Win.Misc.UltraButton buttonSelect;
        private Infragistics.Win.Misc.UltraButton buttonNew;
        private Infragistics.Win.Misc.UltraButton buttonDelete;
        private Infragistics.Win.Misc.UltraButton buttonTeach;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTaught;
        private System.Windows.Forms.TableLayoutPanel layoutImageView;
        private System.Windows.Forms.TableLayoutPanel imageView;
        private Infragistics.Win.Misc.UltraLabel labelImage;
        private System.Windows.Forms.TableLayoutPanel layoutModelList;
        private Infragistics.Win.Misc.UltraLabel labelModelList;
        private Infragistics.Win.Misc.UltraLabel labelTotal;
        private System.Windows.Forms.TextBox findModel;
        private Infragistics.Win.Misc.UltraLabel total;
        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.PictureBox camImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPaste;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnTrained;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrant;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRegistrationDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLastModifiedDate;
    }
}
