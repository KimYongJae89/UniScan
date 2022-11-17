namespace UniAoi.A.UI.InspectParamControl
{
    partial class TargetParamControl
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.probeSelector = new System.Windows.Forms.DataGridView();
            this.ColumnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelCommonParam = new System.Windows.Forms.Panel();
            this.cmbTargetType = new System.Windows.Forms.ComboBox();
            this.btnPasteProbe = new System.Windows.Forms.Button();
            this.panelTopLeft = new System.Windows.Forms.Panel();
            this.pnlTargetImage = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.checkPreview = new System.Windows.Forms.CheckBox();
            this.buttonZoomFit = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.btnDeletProbe = new System.Windows.Forms.Button();
            this.btnCopyProbe = new System.Windows.Forms.Button();
            this.comboInspectionLogicType = new System.Windows.Forms.ComboBox();
            this.btnAddProbe = new System.Windows.Forms.Button();
            this.labelLogic = new System.Windows.Forms.Label();
            this.useInspection = new System.Windows.Forms.CheckBox();
            this.targetName = new System.Windows.Forms.TextBox();
            this.lblAngle = new System.Windows.Forms.Label();
            this.targetId = new System.Windows.Forms.Label();
            this.labelAngle = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblTargetId = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.btnSyncTarget = new System.Windows.Forms.Button();
            this.panelParam = new System.Windows.Forms.Panel();
            this.contextMenuStripAlgorithmType = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.patternMatchingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.binaryCounterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.simpleColorCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blobCheckerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.probeSelector)).BeginInit();
            this.panelCommonParam.SuspendLayout();
            this.panelTopLeft.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.contextMenuStripAlgorithmType.SuspendLayout();
            this.SuspendLayout();
            // 
            // probeSelector
            // 
            this.probeSelector.AllowUserToAddRows = false;
            this.probeSelector.AllowUserToDeleteRows = false;
            this.probeSelector.AllowUserToResizeColumns = false;
            this.probeSelector.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.probeSelector.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.probeSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.probeSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnID,
            this.ColumnType});
            this.probeSelector.Location = new System.Drawing.Point(519, 269);
            this.probeSelector.MultiSelect = false;
            this.probeSelector.Name = "probeSelector";
            this.probeSelector.ReadOnly = true;
            this.probeSelector.RowHeadersVisible = false;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            this.probeSelector.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.probeSelector.RowTemplate.Height = 30;
            this.probeSelector.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.probeSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.probeSelector.Size = new System.Drawing.Size(266, 108);
            this.probeSelector.TabIndex = 8;
            this.probeSelector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.probeList_CellClick);
            // 
            // ColumnID
            // 
            this.ColumnID.HeaderText = "ID";
            this.ColumnID.Name = "ColumnID";
            this.ColumnID.ReadOnly = true;
            this.ColumnID.Width = 80;
            // 
            // ColumnType
            // 
            this.ColumnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnType.HeaderText = "Type";
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(-2, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // panelCommonParam
            // 
            this.panelCommonParam.Controls.Add(this.cmbTargetType);
            this.panelCommonParam.Controls.Add(this.btnPasteProbe);
            this.panelCommonParam.Controls.Add(this.panelTopLeft);
            this.panelCommonParam.Controls.Add(this.btnDeletProbe);
            this.panelCommonParam.Controls.Add(this.btnCopyProbe);
            this.panelCommonParam.Controls.Add(this.comboInspectionLogicType);
            this.panelCommonParam.Controls.Add(this.btnAddProbe);
            this.panelCommonParam.Controls.Add(this.labelLogic);
            this.panelCommonParam.Controls.Add(this.useInspection);
            this.panelCommonParam.Controls.Add(this.targetName);
            this.panelCommonParam.Controls.Add(this.lblAngle);
            this.panelCommonParam.Controls.Add(this.targetId);
            this.panelCommonParam.Controls.Add(this.labelAngle);
            this.panelCommonParam.Controls.Add(this.lblType);
            this.panelCommonParam.Controls.Add(this.lblTargetId);
            this.panelCommonParam.Controls.Add(this.labelName);
            this.panelCommonParam.Controls.Add(this.probeSelector);
            this.panelCommonParam.Controls.Add(this.btnSyncTarget);
            this.panelCommonParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCommonParam.Location = new System.Drawing.Point(0, 0);
            this.panelCommonParam.Name = "panelCommonParam";
            this.panelCommonParam.Size = new System.Drawing.Size(806, 447);
            this.panelCommonParam.TabIndex = 0;
            // 
            // cmbTargetType
            // 
            this.cmbTargetType.DropDownHeight = 300;
            this.cmbTargetType.FormattingEnabled = true;
            this.cmbTargetType.IntegralHeight = false;
            this.cmbTargetType.Location = new System.Drawing.Point(586, 75);
            this.cmbTargetType.Name = "cmbTargetType";
            this.cmbTargetType.Size = new System.Drawing.Size(199, 25);
            this.cmbTargetType.TabIndex = 160;
            this.cmbTargetType.SelectedValueChanged += new System.EventHandler(this.cmbTargetType_SelectedValueChanged);
            this.cmbTargetType.TextChanged += new System.EventHandler(this.cmbTargetType_TextChanged);
            // 
            // btnPasteProbe
            // 
            this.btnPasteProbe.AccessibleName = "Inspect";
            this.btnPasteProbe.FlatAppearance.BorderSize = 0;
            this.btnPasteProbe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPasteProbe.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPasteProbe.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPasteProbe.Image = global::UniAoi.A.Properties.Resources.paste_48;
            this.btnPasteProbe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPasteProbe.Location = new System.Drawing.Point(653, 213);
            this.btnPasteProbe.Margin = new System.Windows.Forms.Padding(0);
            this.btnPasteProbe.Name = "btnPasteProbe";
            this.btnPasteProbe.Size = new System.Drawing.Size(114, 50);
            this.btnPasteProbe.TabIndex = 159;
            this.btnPasteProbe.Text = "Paste";
            this.btnPasteProbe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPasteProbe.UseVisualStyleBackColor = true;
            this.btnPasteProbe.Click += new System.EventHandler(this.pasteProbeButton_Click);
            // 
            // panelTopLeft
            // 
            this.panelTopLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelTopLeft.Controls.Add(this.pnlTargetImage);
            this.panelTopLeft.Controls.Add(this.pnlBottom);
            this.panelTopLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTopLeft.Location = new System.Drawing.Point(0, 0);
            this.panelTopLeft.Name = "panelTopLeft";
            this.panelTopLeft.Size = new System.Drawing.Size(499, 447);
            this.panelTopLeft.TabIndex = 153;
            // 
            // pnlTargetImage
            // 
            this.pnlTargetImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTargetImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTargetImage.Location = new System.Drawing.Point(0, 0);
            this.pnlTargetImage.Name = "pnlTargetImage";
            this.pnlTargetImage.Size = new System.Drawing.Size(497, 391);
            this.pnlTargetImage.TabIndex = 152;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.checkPreview);
            this.pnlBottom.Controls.Add(this.buttonZoomFit);
            this.pnlBottom.Controls.Add(this.buttonZoomIn);
            this.pnlBottom.Controls.Add(this.buttonZoomOut);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 391);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(497, 54);
            this.pnlBottom.TabIndex = 153;
            // 
            // checkPreview
            // 
            this.checkPreview.AutoSize = true;
            this.checkPreview.Checked = true;
            this.checkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPreview.Location = new System.Drawing.Point(416, 9);
            this.checkPreview.Name = "checkPreview";
            this.checkPreview.Size = new System.Drawing.Size(72, 21);
            this.checkPreview.TabIndex = 152;
            this.checkPreview.Text = "Preview";
            this.checkPreview.UseVisualStyleBackColor = true;
            this.checkPreview.CheckedChanged += new System.EventHandler(this.checkPreview_CheckedChanged);
            // 
            // buttonZoomFit
            // 
            this.buttonZoomFit.FlatAppearance.BorderSize = 0;
            this.buttonZoomFit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomFit.Image = global::UniAoi.A.Properties.Resources.zoomfit_48;
            this.buttonZoomFit.Location = new System.Drawing.Point(112, -1);
            this.buttonZoomFit.Name = "buttonZoomFit";
            this.buttonZoomFit.Size = new System.Drawing.Size(50, 50);
            this.buttonZoomFit.TabIndex = 151;
            this.buttonZoomFit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomFit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomFit.UseVisualStyleBackColor = true;
            this.buttonZoomFit.Click += new System.EventHandler(this.buttonZoomFit_Click);
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.FlatAppearance.BorderSize = 0;
            this.buttonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomIn.Image = global::UniAoi.A.Properties.Resources.zoom_48;
            this.buttonZoomIn.Location = new System.Drawing.Point(2, -1);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(48, 48);
            this.buttonZoomIn.TabIndex = 151;
            this.buttonZoomIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.FlatAppearance.BorderSize = 0;
            this.buttonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomOut.Image = global::UniAoi.A.Properties.Resources.zoomout_48;
            this.buttonZoomOut.Location = new System.Drawing.Point(56, -1);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(50, 50);
            this.buttonZoomOut.TabIndex = 151;
            this.buttonZoomOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // btnDeletProbe
            // 
            this.btnDeletProbe.AccessibleName = "Inspect";
            this.btnDeletProbe.FlatAppearance.BorderSize = 0;
            this.btnDeletProbe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletProbe.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeletProbe.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeletProbe.Image = global::UniAoi.A.Properties.Resources.delete_48;
            this.btnDeletProbe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeletProbe.Location = new System.Drawing.Point(653, 163);
            this.btnDeletProbe.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeletProbe.Name = "btnDeletProbe";
            this.btnDeletProbe.Size = new System.Drawing.Size(114, 50);
            this.btnDeletProbe.TabIndex = 158;
            this.btnDeletProbe.Text = "Delete";
            this.btnDeletProbe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeletProbe.UseVisualStyleBackColor = true;
            this.btnDeletProbe.Click += new System.EventHandler(this.deleteProbeButton_Click);
            // 
            // btnCopyProbe
            // 
            this.btnCopyProbe.AccessibleName = "Inspect";
            this.btnCopyProbe.FlatAppearance.BorderSize = 0;
            this.btnCopyProbe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyProbe.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCopyProbe.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCopyProbe.Image = global::UniAoi.A.Properties.Resources.copy_48;
            this.btnCopyProbe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCopyProbe.Location = new System.Drawing.Point(525, 213);
            this.btnCopyProbe.Margin = new System.Windows.Forms.Padding(0);
            this.btnCopyProbe.Name = "btnCopyProbe";
            this.btnCopyProbe.Size = new System.Drawing.Size(114, 50);
            this.btnCopyProbe.TabIndex = 156;
            this.btnCopyProbe.Text = "Copy";
            this.btnCopyProbe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCopyProbe.UseVisualStyleBackColor = true;
            this.btnCopyProbe.Click += new System.EventHandler(this.copyProbeButton_Click);
            // 
            // comboInspectionLogicType
            // 
            this.comboInspectionLogicType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboInspectionLogicType.FormattingEnabled = true;
            this.comboInspectionLogicType.Items.AddRange(new object[] {
            "AND",
            "OR"});
            this.comboInspectionLogicType.Location = new System.Drawing.Point(706, 381);
            this.comboInspectionLogicType.Name = "comboInspectionLogicType";
            this.comboInspectionLogicType.Size = new System.Drawing.Size(79, 25);
            this.comboInspectionLogicType.TabIndex = 150;
            this.comboInspectionLogicType.SelectedIndexChanged += new System.EventHandler(this.comboLogic_SelectedIndexChanged);
            // 
            // btnAddProbe
            // 
            this.btnAddProbe.AccessibleName = "Inspect";
            this.btnAddProbe.FlatAppearance.BorderSize = 0;
            this.btnAddProbe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddProbe.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddProbe.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAddProbe.Image = global::UniAoi.A.Properties.Resources.add_48;
            this.btnAddProbe.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddProbe.Location = new System.Drawing.Point(525, 163);
            this.btnAddProbe.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddProbe.Name = "btnAddProbe";
            this.btnAddProbe.Size = new System.Drawing.Size(114, 51);
            this.btnAddProbe.TabIndex = 157;
            this.btnAddProbe.Text = "New";
            this.btnAddProbe.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddProbe.UseVisualStyleBackColor = true;
            this.btnAddProbe.Click += new System.EventHandler(this.addProbeButton_Click);
            // 
            // labelLogic
            // 
            this.labelLogic.AutoSize = true;
            this.labelLogic.Location = new System.Drawing.Point(524, 384);
            this.labelLogic.Name = "labelLogic";
            this.labelLogic.Size = new System.Drawing.Size(106, 17);
            this.labelLogic.TabIndex = 149;
            this.labelLogic.Text = "Inspection Logic";
            // 
            // useInspection
            // 
            this.useInspection.AutoSize = true;
            this.useInspection.Location = new System.Drawing.Point(527, 144);
            this.useInspection.Name = "useInspection";
            this.useInspection.Size = new System.Drawing.Size(116, 21);
            this.useInspection.TabIndex = 147;
            this.useInspection.Text = "Use Inspection";
            this.useInspection.UseVisualStyleBackColor = true;
            this.useInspection.CheckedChanged += new System.EventHandler(this.useInspection_CheckedChanged);
            // 
            // targetName
            // 
            this.targetName.Location = new System.Drawing.Point(586, 41);
            this.targetName.Name = "targetName";
            this.targetName.Size = new System.Drawing.Size(199, 25);
            this.targetName.TabIndex = 2;
            this.targetName.TextChanged += new System.EventHandler(this.targetName_TextChanged);
            this.targetName.Enter += new System.EventHandler(this.TextBox_Enter);
            this.targetName.Leave += new System.EventHandler(this.TextBox_Leave);
            // 
            // lblAngle
            // 
            this.lblAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblAngle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAngle.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblAngle.Location = new System.Drawing.Point(586, 110);
            this.lblAngle.Margin = new System.Windows.Forms.Padding(0);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(57, 25);
            this.lblAngle.TabIndex = 0;
            this.lblAngle.Text = "ID";
            this.lblAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // targetId
            // 
            this.targetId.BackColor = System.Drawing.Color.Transparent;
            this.targetId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.targetId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.targetId.Location = new System.Drawing.Point(586, 9);
            this.targetId.Margin = new System.Windows.Forms.Padding(0);
            this.targetId.Name = "targetId";
            this.targetId.Size = new System.Drawing.Size(199, 25);
            this.targetId.TabIndex = 0;
            this.targetId.Text = "ID";
            this.targetId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(526, 116);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.Size = new System.Drawing.Size(43, 17);
            this.labelAngle.TabIndex = 1;
            this.labelAngle.Text = "Angle";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(526, 80);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(36, 17);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Type";
            // 
            // lblTargetId
            // 
            this.lblTargetId.AutoSize = true;
            this.lblTargetId.Location = new System.Drawing.Point(526, 15);
            this.lblTargetId.Name = "lblTargetId";
            this.lblTargetId.Size = new System.Drawing.Size(21, 17);
            this.lblTargetId.TabIndex = 1;
            this.lblTargetId.Text = "ID";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(526, 47);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(43, 17);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // btnSyncTarget
            // 
            this.btnSyncTarget.AccessibleName = "Inspect";
            this.btnSyncTarget.FlatAppearance.BorderSize = 0;
            this.btnSyncTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSyncTarget.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSyncTarget.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSyncTarget.Image = global::UniAoi.A.Properties.Resources.paste_48;
            this.btnSyncTarget.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSyncTarget.Location = new System.Drawing.Point(653, 113);
            this.btnSyncTarget.Margin = new System.Windows.Forms.Padding(0);
            this.btnSyncTarget.Name = "btnSyncTarget";
            this.btnSyncTarget.Size = new System.Drawing.Size(88, 50);
            this.btnSyncTarget.TabIndex = 159;
            this.btnSyncTarget.Text = "Sync";
            this.btnSyncTarget.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSyncTarget.UseVisualStyleBackColor = true;
            this.btnSyncTarget.Click += new System.EventHandler(this.btnSyncTarget_Click);
            // 
            // panelParam
            // 
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(0, 447);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(806, 474);
            this.panelParam.TabIndex = 1;
            // 
            // contextMenuStripAlgorithmType
            // 
            this.contextMenuStripAlgorithmType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.contextMenuStripAlgorithmType.DropShadowEnabled = false;
            this.contextMenuStripAlgorithmType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contextMenuStripAlgorithmType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.patternMatchingToolStripMenuItem,
            this.binaryCounterToolStripMenuItem,
            this.brightnessCheckerToolStripMenuItem,
            this.simpleColorCheckerToolStripMenuItem,
            this.blobCheckerToolStripMenuItem});
            this.contextMenuStripAlgorithmType.Name = "contextMenuStripAlgorithmType";
            this.contextMenuStripAlgorithmType.Size = new System.Drawing.Size(218, 124);
            // 
            // patternMatchingToolStripMenuItem
            // 
            this.patternMatchingToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.patternMatchingToolStripMenuItem.Name = "patternMatchingToolStripMenuItem";
            this.patternMatchingToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.patternMatchingToolStripMenuItem.Text = "Pattern Matching";
            this.patternMatchingToolStripMenuItem.Click += new System.EventHandler(this.patternMatchingToolStripMenuItem_Click);
            // 
            // binaryCounterToolStripMenuItem
            // 
            this.binaryCounterToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.binaryCounterToolStripMenuItem.Name = "binaryCounterToolStripMenuItem";
            this.binaryCounterToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.binaryCounterToolStripMenuItem.Text = "Binary Counter";
            this.binaryCounterToolStripMenuItem.Click += new System.EventHandler(this.binaryCounterToolStripMenuItem_Click);
            // 
            // brightnessCheckerToolStripMenuItem
            // 
            this.brightnessCheckerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.brightnessCheckerToolStripMenuItem.Name = "brightnessCheckerToolStripMenuItem";
            this.brightnessCheckerToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.brightnessCheckerToolStripMenuItem.Text = "Brightness Checker";
            this.brightnessCheckerToolStripMenuItem.Click += new System.EventHandler(this.brightnessCheckerToolStripMenuItem_Click);
            // 
            // simpleColorCheckerToolStripMenuItem
            // 
            this.simpleColorCheckerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.simpleColorCheckerToolStripMenuItem.Name = "simpleColorCheckerToolStripMenuItem";
            this.simpleColorCheckerToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.simpleColorCheckerToolStripMenuItem.Text = "Color checker";
            this.simpleColorCheckerToolStripMenuItem.Click += new System.EventHandler(this.simpleColorCheckerToolStripMenuItem_Click);
            // 
            // blobCheckerToolStripMenuItem
            // 
            this.blobCheckerToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.blobCheckerToolStripMenuItem.Name = "blobCheckerToolStripMenuItem";
            this.blobCheckerToolStripMenuItem.Size = new System.Drawing.Size(217, 24);
            this.blobCheckerToolStripMenuItem.Text = "Blob checker";
            this.blobCheckerToolStripMenuItem.Click += new System.EventHandler(this.blobCheckerToolStripMenuItem_Click);
            // 
            // TargetParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.panelParam);
            this.Controls.Add(this.panelCommonParam);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Name = "TargetParamControl";
            this.Size = new System.Drawing.Size(806, 921);
            this.Load += new System.EventHandler(this.TargetParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.probeSelector)).EndInit();
            this.panelCommonParam.ResumeLayout(false);
            this.panelCommonParam.PerformLayout();
            this.panelTopLeft.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.contextMenuStripAlgorithmType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView probeSelector;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelCommonParam;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAlgorithmType;
        private System.Windows.Forms.ToolStripMenuItem patternMatchingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem binaryCounterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem brightnessCheckerToolStripMenuItem;
        private System.Windows.Forms.Label targetId;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox targetName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.CheckBox useInspection;
        private System.Windows.Forms.Label labelLogic;
        private System.Windows.Forms.ComboBox comboInspectionLogicType;
        private System.Windows.Forms.ToolStripMenuItem simpleColorCheckerToolStripMenuItem;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomFit;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Panel pnlTargetImage;
        private System.Windows.Forms.Panel panelTopLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnPasteProbe;
        private System.Windows.Forms.Button btnDeletProbe;
        private System.Windows.Forms.Button btnCopyProbe;
        private System.Windows.Forms.Button btnAddProbe;
        private System.Windows.Forms.ToolStripMenuItem blobCheckerToolStripMenuItem;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbTargetType;
        private System.Windows.Forms.Label lblTargetId;
        private System.Windows.Forms.Button btnSyncTarget;
        private System.Windows.Forms.CheckBox checkPreview;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Label labelAngle;
    }
}