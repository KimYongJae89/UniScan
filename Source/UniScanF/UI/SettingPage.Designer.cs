namespace UniScan.UI
{
    partial class SettingPage
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.tabPageMachine = new System.Windows.Forms.TabPage();
            this.tabPageCamera = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPageChart = new System.Windows.Forms.TabPage();
            this.ultraGridPanel = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraButtonSave = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDelete = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonDownChange = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonUpChange = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonAdd = new Infragistics.Win.Misc.UltraButton();
            this.comboPageSelect = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.tabPageModel = new System.Windows.Forms.TabPage();
            this.ugbPET = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonPETSave = new Infragistics.Win.Misc.UltraButton();
            this.listBoxPET = new System.Windows.Forms.ListBox();
            this.buttonPETDelete = new Infragistics.Win.Misc.UltraButton();
            this.buttonPETAdd = new Infragistics.Win.Misc.UltraButton();
            this.utePET = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ugbPowder = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonPowderSave = new Infragistics.Win.Misc.UltraButton();
            this.buttonPowderDelete = new Infragistics.Win.Misc.UltraButton();
            this.buttonPowderAdd = new Infragistics.Win.Misc.UltraButton();
            this.listBoxPowder = new System.Windows.Forms.ListBox();
            this.utePowder = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraGridBagLayoutManager1 = new Infragistics.Win.Misc.UltraGridBagLayoutManager(this.components);
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.tabControlMain.SuspendLayout();
            this.tabPageCamera.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboPageSelect)).BeginInit();
            this.tabPageModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugbPET)).BeginInit();
            this.ugbPET.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utePET)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbPowder)).BeginInit();
            this.ugbPowder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utePowder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageGeneral);
            this.tabControlMain.Controls.Add(this.tabPageData);
            this.tabControlMain.Controls.Add(this.tabPageMachine);
            this.tabControlMain.Controls.Add(this.tabPageCamera);
            this.tabControlMain.Controls.Add(this.tabPageChart);
            this.tabControlMain.Controls.Add(this.tabPageModel);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(788, 633);
            this.tabControlMain.TabIndex = 159;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 30);
            this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageGeneral.Size = new System.Drawing.Size(780, 599);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageData
            // 
            this.tabPageData.Location = new System.Drawing.Point(4, 30);
            this.tabPageData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageData.Size = new System.Drawing.Size(780, 599);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // tabPageMachine
            // 
            this.tabPageMachine.Location = new System.Drawing.Point(4, 30);
            this.tabPageMachine.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageMachine.Name = "tabPageMachine";
            this.tabPageMachine.Size = new System.Drawing.Size(780, 599);
            this.tabPageMachine.TabIndex = 2;
            this.tabPageMachine.Text = "Machine";
            this.tabPageMachine.UseVisualStyleBackColor = true;
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.Controls.Add(this.groupBox1);
            this.tabPageCamera.Location = new System.Drawing.Point(4, 30);
            this.tabPageCamera.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCamera.Size = new System.Drawing.Size(780, 599);
            this.tabPageCamera.TabIndex = 3;
            this.tabPageCamera.Text = "Camera";
            this.tabPageCamera.UseVisualStyleBackColor = true;
            this.tabPageCamera.Click += new System.EventHandler(this.tabPageCamera_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(7, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 155);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calibration";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "0.000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Resolution [um/px]";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(231, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Setup";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPageChart
            // 
            this.tabPageChart.Controls.Add(this.ultraGridPanel);
            this.tabPageChart.Controls.Add(this.ultraButtonSave);
            this.tabPageChart.Controls.Add(this.ultraButtonDelete);
            this.tabPageChart.Controls.Add(this.ultraButtonDownChange);
            this.tabPageChart.Controls.Add(this.ultraButtonUpChange);
            this.tabPageChart.Controls.Add(this.ultraButtonAdd);
            this.tabPageChart.Controls.Add(this.comboPageSelect);
            this.tabPageChart.Location = new System.Drawing.Point(4, 30);
            this.tabPageChart.Name = "tabPageChart";
            this.tabPageChart.Size = new System.Drawing.Size(780, 599);
            this.tabPageChart.TabIndex = 4;
            this.tabPageChart.Text = "Chart";
            this.tabPageChart.UseVisualStyleBackColor = true;
            // 
            // ultraGridPanel
            // 
            this.ultraGridPanel.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.ultraGridPanel.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridPanel.DisplayLayout.GroupByBox.Hidden = true;
            this.ultraGridPanel.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
            this.ultraGridPanel.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.None;
            this.ultraGridPanel.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.ultraGridPanel.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            this.ultraGridPanel.DisplayLayout.Override.FixedRowIndicator = Infragistics.Win.UltraWinGrid.FixedRowIndicator.None;
            this.ultraGridPanel.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.SeparateElement;
            this.ultraGridPanel.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGridPanel.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridPanel.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.ultraGridPanel.DisplayLayout.ShowDeleteRowsPrompt = false;
            this.ultraGridPanel.Location = new System.Drawing.Point(13, 53);
            this.ultraGridPanel.Name = "ultraGridPanel";
            this.ultraGridPanel.Size = new System.Drawing.Size(604, 150);
            this.ultraGridPanel.TabIndex = 19;
            this.ultraGridPanel.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGridPanel_CellChange);
            // 
            // ultraButtonSave
            // 
            appearance1.Image = global::UniScan.Properties.Resources.save32;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonSave.Appearance = appearance1;
            this.ultraButtonSave.Location = new System.Drawing.Point(586, 16);
            this.ultraButtonSave.Name = "ultraButtonSave";
            this.ultraButtonSave.Size = new System.Drawing.Size(31, 31);
            this.ultraButtonSave.TabIndex = 18;
            this.ultraButtonSave.Click += new System.EventHandler(this.ultraButtonSave_Click);
            // 
            // ultraButtonDelete
            // 
            appearance2.Image = global::UniScan.Properties.Resources.delete_32;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonDelete.Appearance = appearance2;
            this.ultraButtonDelete.Location = new System.Drawing.Point(549, 16);
            this.ultraButtonDelete.Name = "ultraButtonDelete";
            this.ultraButtonDelete.Size = new System.Drawing.Size(31, 31);
            this.ultraButtonDelete.TabIndex = 18;
            this.ultraButtonDelete.Click += new System.EventHandler(this.ultraButtonDelete_Click);
            // 
            // ultraButtonDownChange
            // 
            appearance3.Image = global::UniScan.Properties.Resources.arrow_down;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonDownChange.Appearance = appearance3;
            this.ultraButtonDownChange.Location = new System.Drawing.Point(623, 133);
            this.ultraButtonDownChange.Name = "ultraButtonDownChange";
            this.ultraButtonDownChange.Size = new System.Drawing.Size(31, 70);
            this.ultraButtonDownChange.TabIndex = 18;
            this.ultraButtonDownChange.Click += new System.EventHandler(this.ultraButtonDownChange_Click);
            // 
            // ultraButtonUpChange
            // 
            appearance4.Image = global::UniScan.Properties.Resources.arrow_up;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonUpChange.Appearance = appearance4;
            this.ultraButtonUpChange.Location = new System.Drawing.Point(623, 53);
            this.ultraButtonUpChange.Name = "ultraButtonUpChange";
            this.ultraButtonUpChange.Size = new System.Drawing.Size(31, 70);
            this.ultraButtonUpChange.TabIndex = 18;
            this.ultraButtonUpChange.Click += new System.EventHandler(this.ultraButtonUpChange_Click);
            // 
            // ultraButtonAdd
            // 
            appearance5.Image = global::UniScan.Properties.Resources.add_32;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonAdd.Appearance = appearance5;
            this.ultraButtonAdd.Location = new System.Drawing.Point(512, 16);
            this.ultraButtonAdd.Name = "ultraButtonAdd";
            this.ultraButtonAdd.Size = new System.Drawing.Size(31, 31);
            this.ultraButtonAdd.TabIndex = 18;
            this.ultraButtonAdd.Click += new System.EventHandler(this.ultraButtonAdd_Click);
            // 
            // comboPageSelect
            // 
            this.comboPageSelect.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.comboPageSelect.Location = new System.Drawing.Point(13, 16);
            this.comboPageSelect.Name = "comboPageSelect";
            this.comboPageSelect.Size = new System.Drawing.Size(493, 31);
            this.comboPageSelect.TabIndex = 1;
            this.comboPageSelect.ValueChanged += new System.EventHandler(this.comboPageSelect_ValueChanged);
            // 
            // tabPageModel
            // 
            this.tabPageModel.Controls.Add(this.ugbPET);
            this.tabPageModel.Controls.Add(this.ugbPowder);
            this.tabPageModel.Location = new System.Drawing.Point(4, 30);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Size = new System.Drawing.Size(780, 599);
            this.tabPageModel.TabIndex = 5;
            this.tabPageModel.Text = "Model";
            this.tabPageModel.UseVisualStyleBackColor = true;
            // 
            // ugbPET
            // 
            this.ugbPET.Controls.Add(this.buttonPETSave);
            this.ugbPET.Controls.Add(this.listBoxPET);
            this.ugbPET.Controls.Add(this.buttonPETDelete);
            this.ugbPET.Controls.Add(this.buttonPETAdd);
            this.ugbPET.Controls.Add(this.utePET);
            this.ugbPET.Location = new System.Drawing.Point(234, 15);
            this.ugbPET.Name = "ugbPET";
            this.ugbPET.Size = new System.Drawing.Size(213, 290);
            this.ugbPET.TabIndex = 1;
            this.ugbPET.Text = "PET Type";
            // 
            // buttonPETSave
            // 
            appearance6.Image = global::UniScan.Properties.Resources.save32;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPETSave.Appearance = appearance6;
            this.buttonPETSave.Location = new System.Drawing.Point(172, 111);
            this.buttonPETSave.Name = "buttonPETSave";
            this.buttonPETSave.Size = new System.Drawing.Size(35, 35);
            this.buttonPETSave.TabIndex = 24;
            this.buttonPETSave.Click += new System.EventHandler(this.buttonPETSave_Click);
            // 
            // listBoxPET
            // 
            this.listBoxPET.FormattingEnabled = true;
            this.listBoxPET.ItemHeight = 21;
            this.listBoxPET.Location = new System.Drawing.Point(6, 70);
            this.listBoxPET.Name = "listBoxPET";
            this.listBoxPET.Size = new System.Drawing.Size(160, 214);
            this.listBoxPET.TabIndex = 28;
            // 
            // buttonPETDelete
            // 
            appearance7.Image = global::UniScan.Properties.Resources.delete_32;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPETDelete.Appearance = appearance7;
            this.buttonPETDelete.Location = new System.Drawing.Point(172, 70);
            this.buttonPETDelete.Name = "buttonPETDelete";
            this.buttonPETDelete.Size = new System.Drawing.Size(35, 35);
            this.buttonPETDelete.TabIndex = 25;
            this.buttonPETDelete.Click += new System.EventHandler(this.buttonPETDelete_Click);
            // 
            // buttonPETAdd
            // 
            appearance8.Image = global::UniScan.Properties.Resources.add_32;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPETAdd.Appearance = appearance8;
            this.buttonPETAdd.Location = new System.Drawing.Point(172, 29);
            this.buttonPETAdd.Name = "buttonPETAdd";
            this.buttonPETAdd.Size = new System.Drawing.Size(35, 35);
            this.buttonPETAdd.TabIndex = 26;
            this.buttonPETAdd.Click += new System.EventHandler(this.buttonPETAdd_Click);
            // 
            // utePET
            // 
            this.utePET.AutoSize = false;
            this.utePET.Location = new System.Drawing.Point(6, 29);
            this.utePET.Name = "utePET";
            this.utePET.Size = new System.Drawing.Size(160, 35);
            this.utePET.TabIndex = 27;
            // 
            // ugbPowder
            // 
            this.ugbPowder.Controls.Add(this.buttonPowderSave);
            this.ugbPowder.Controls.Add(this.buttonPowderDelete);
            this.ugbPowder.Controls.Add(this.buttonPowderAdd);
            this.ugbPowder.Controls.Add(this.listBoxPowder);
            this.ugbPowder.Controls.Add(this.utePowder);
            this.ugbPowder.Location = new System.Drawing.Point(15, 15);
            this.ugbPowder.Name = "ugbPowder";
            this.ugbPowder.Size = new System.Drawing.Size(213, 290);
            this.ugbPowder.TabIndex = 0;
            this.ugbPowder.Text = "Powder Type";
            // 
            // buttonPowderSave
            // 
            appearance9.Image = global::UniScan.Properties.Resources.save32;
            appearance9.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance9.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPowderSave.Appearance = appearance9;
            this.buttonPowderSave.Location = new System.Drawing.Point(172, 111);
            this.buttonPowderSave.Name = "buttonPowderSave";
            this.buttonPowderSave.Size = new System.Drawing.Size(35, 35);
            this.buttonPowderSave.TabIndex = 24;
            this.buttonPowderSave.Click += new System.EventHandler(this.buttonPowderSave_Click);
            // 
            // buttonPowderDelete
            // 
            appearance10.Image = global::UniScan.Properties.Resources.delete_32;
            appearance10.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance10.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPowderDelete.Appearance = appearance10;
            this.buttonPowderDelete.Location = new System.Drawing.Point(172, 70);
            this.buttonPowderDelete.Name = "buttonPowderDelete";
            this.buttonPowderDelete.Size = new System.Drawing.Size(35, 35);
            this.buttonPowderDelete.TabIndex = 25;
            this.buttonPowderDelete.Click += new System.EventHandler(this.buttonPowderDelete_Click);
            // 
            // buttonPowderAdd
            // 
            appearance11.Image = global::UniScan.Properties.Resources.add_32;
            appearance11.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance11.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonPowderAdd.Appearance = appearance11;
            this.buttonPowderAdd.Location = new System.Drawing.Point(172, 29);
            this.buttonPowderAdd.Name = "buttonPowderAdd";
            this.buttonPowderAdd.Size = new System.Drawing.Size(35, 35);
            this.buttonPowderAdd.TabIndex = 26;
            this.buttonPowderAdd.Click += new System.EventHandler(this.buttonPowderAdd_Click);
            // 
            // listBoxPowder
            // 
            this.listBoxPowder.FormattingEnabled = true;
            this.listBoxPowder.ItemHeight = 21;
            this.listBoxPowder.Location = new System.Drawing.Point(6, 70);
            this.listBoxPowder.Name = "listBoxPowder";
            this.listBoxPowder.Size = new System.Drawing.Size(160, 214);
            this.listBoxPowder.TabIndex = 23;
            // 
            // utePowder
            // 
            this.utePowder.AutoSize = false;
            this.utePowder.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.utePowder.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.utePowder.Location = new System.Drawing.Point(6, 29);
            this.utePowder.Name = "utePowder";
            this.utePowder.Size = new System.Drawing.Size(160, 35);
            this.utePowder.TabIndex = 22;
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(788, 633);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageCamera.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageChart.ResumeLayout(false);
            this.tabPageChart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboPageSelect)).EndInit();
            this.tabPageModel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugbPET)).EndInit();
            this.ugbPET.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utePET)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugbPowder)).EndInit();
            this.ugbPowder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utePowder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGridBagLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.TabPage tabPageMachine;
        private System.Windows.Forms.TabPage tabPageCamera;
        private System.Windows.Forms.TabPage tabPageChart;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor comboPageSelect;
        private Infragistics.Win.Misc.UltraButton ultraButtonAdd;
        private Infragistics.Win.Misc.UltraButton ultraButtonDelete;
        private Infragistics.Win.Misc.UltraGridBagLayoutManager ultraGridBagLayoutManager1;
        private Infragistics.Win.Misc.UltraButton ultraButtonSave;
        private Infragistics.Win.UltraWinGrid.UltraGrid ultraGridPanel;
        private Infragistics.Win.Misc.UltraButton ultraButtonDownChange;
        private Infragistics.Win.Misc.UltraButton ultraButtonUpChange;
        private System.Windows.Forms.TabPage tabPageModel;
        private Infragistics.Win.Misc.UltraGroupBox ugbPET;
        private Infragistics.Win.Misc.UltraGroupBox ugbPowder;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor utePowder;
        private System.Windows.Forms.ListBox listBoxPowder;
        private System.Windows.Forms.ListBox listBoxPET;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor utePET;
        private Infragistics.Win.Misc.UltraButton buttonPETSave;
        private Infragistics.Win.Misc.UltraButton buttonPETDelete;
        private Infragistics.Win.Misc.UltraButton buttonPETAdd;
        private Infragistics.Win.Misc.UltraButton buttonPowderSave;
        private Infragistics.Win.Misc.UltraButton buttonPowderDelete;
        private Infragistics.Win.Misc.UltraButton buttonPowderAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}
