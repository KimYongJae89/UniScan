namespace UniScanM.Gloss.UI
{
    partial class ScanWidthSettingForm
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.groupModelWidth = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonModelWidthDelete = new Infragistics.Win.Misc.UltraButton();
            this.buttonModelWidthAdd = new Infragistics.Win.Misc.UltraButton();
            this.validPositionList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOK = new Infragistics.Win.Misc.UltraButton();
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCancel = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupModelWidth)).BeginInit();
            this.groupModelWidth.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.validPositionList)).BeginInit();
            this.tableMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupModelWidth
            // 
            this.tableMain.SetColumnSpan(this.groupModelWidth, 2);
            this.groupModelWidth.Controls.Add(this.buttonModelWidthDelete);
            this.groupModelWidth.Controls.Add(this.buttonModelWidthAdd);
            this.groupModelWidth.Controls.Add(this.validPositionList);
            this.groupModelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupModelWidth.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupModelWidth.Location = new System.Drawing.Point(3, 3);
            this.groupModelWidth.Name = "groupModelWidth";
            this.groupModelWidth.Size = new System.Drawing.Size(673, 277);
            this.groupModelWidth.TabIndex = 2;
            this.groupModelWidth.Text = "Model Width";
            // 
            // buttonModelWidthDelete
            // 
            this.buttonModelWidthDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance3.Image = global::UniScanM.Gloss.Properties.Resources.delete_32;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonModelWidthDelete.Appearance = appearance3;
            this.buttonModelWidthDelete.Location = new System.Drawing.Point(632, 70);
            this.buttonModelWidthDelete.Name = "buttonModelWidthDelete";
            this.buttonModelWidthDelete.Size = new System.Drawing.Size(35, 35);
            this.buttonModelWidthDelete.TabIndex = 54;
            this.buttonModelWidthDelete.TabStop = false;
            this.buttonModelWidthDelete.Click += new System.EventHandler(this.buttonModelWidthDelete_Click);
            // 
            // buttonModelWidthAdd
            // 
            this.buttonModelWidthAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            appearance4.Image = global::UniScanM.Gloss.Properties.Resources.add_32;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonModelWidthAdd.Appearance = appearance4;
            this.buttonModelWidthAdd.Location = new System.Drawing.Point(632, 29);
            this.buttonModelWidthAdd.Name = "buttonModelWidthAdd";
            this.buttonModelWidthAdd.Size = new System.Drawing.Size(35, 35);
            this.buttonModelWidthAdd.TabIndex = 55;
            this.buttonModelWidthAdd.TabStop = false;
            this.buttonModelWidthAdd.Click += new System.EventHandler(this.buttonModelWidthAdd_Click);
            // 
            // validPositionList
            // 
            this.validPositionList.AllowUserToAddRows = false;
            this.validPositionList.AllowUserToDeleteRows = false;
            this.validPositionList.AllowUserToResizeColumns = false;
            this.validPositionList.AllowUserToResizeRows = false;
            this.validPositionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.validPositionList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.validPositionList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.validPositionList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.validPositionList.ColumnHeadersHeight = 30;
            this.validPositionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.validPositionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.ColumnStart,
            this.ColumnEnd,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.validPositionList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.validPositionList.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.validPositionList.Location = new System.Drawing.Point(6, 29);
            this.validPositionList.MultiSelect = false;
            this.validPositionList.Name = "validPositionList";
            this.validPositionList.RowHeadersVisible = false;
            this.validPositionList.RowTemplate.Height = 23;
            this.validPositionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.validPositionList.Size = new System.Drawing.Size(620, 244);
            this.validPositionList.TabIndex = 52;
            this.validPositionList.TabStop = false;
            this.validPositionList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.validPositionList_CellValueChanged);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "Width";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.Width = 75;
            // 
            // ColumnStart
            // 
            this.ColumnStart.HeaderText = "Start";
            this.ColumnStart.Name = "ColumnStart";
            // 
            // ColumnEnd
            // 
            this.ColumnEnd.HeaderText = "End";
            this.ColumnEnd.Name = "ColumnEnd";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Valid Start";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Valid End";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // buttonOK
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 12F;
            this.buttonOK.Appearance = appearance2;
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonOK.Location = new System.Drawing.Point(3, 286);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(333, 34);
            this.buttonOK.TabIndex = 13;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 2;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Controls.Add(this.buttonCancel, 1, 1);
            this.tableMain.Controls.Add(this.groupModelWidth, 0, 0);
            this.tableMain.Controls.Add(this.buttonOK, 0, 1);
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 2;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableMain.Size = new System.Drawing.Size(679, 323);
            this.tableMain.TabIndex = 14;
            // 
            // buttonCancel
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 12F;
            this.buttonCancel.Appearance = appearance1;
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonCancel.Location = new System.Drawing.Point(342, 286);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(334, 34);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // ScanWidthSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 323);
            this.Controls.Add(this.tableMain);
            this.Name = "ScanWidthSettingForm";
            this.Text = "ScanWidth Setting Form";
            ((System.ComponentModel.ISupportInitialize)(this.groupModelWidth)).EndInit();
            this.groupModelWidth.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.validPositionList)).EndInit();
            this.tableMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox groupModelWidth;
        private Infragistics.Win.Misc.UltraButton buttonModelWidthDelete;
        private Infragistics.Win.Misc.UltraButton buttonModelWidthAdd;
        private System.Windows.Forms.DataGridView validPositionList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private Infragistics.Win.Misc.UltraButton buttonCancel;
        private Infragistics.Win.Misc.UltraButton buttonOK;
    }
}
