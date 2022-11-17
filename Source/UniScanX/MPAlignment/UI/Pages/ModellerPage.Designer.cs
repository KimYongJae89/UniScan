using System;
using System.Windows.Forms;

namespace UniScanX.MPAlignment.UI.Pages
{
    partial class ModellerPage
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.addFiducialButton = new System.Windows.Forms.Button();
            this.nudExposureTime = new System.Windows.Forms.NumericUpDown();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.labelMs = new System.Windows.Forms.Label();
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnPasteTarget = new System.Windows.Forms.Button();
            this.labelExposure = new System.Windows.Forms.Label();
            this.btnCopyTarget = new System.Windows.Forms.Button();
            this.btnDeleteTarget = new System.Windows.Forms.Button();
            this.btnAddTarget = new System.Windows.Forms.Button();
            this.btnInspect = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelModelMode = new System.Windows.Forms.Label();
            this.panelParameter = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.labelProbePos = new System.Windows.Forms.Label();
            this.panelTarget = new System.Windows.Forms.Panel();
            this.pnlFov = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.buttonZoomFit = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.pnlTarget = new System.Windows.Forms.Panel();
            this.pnlInspectionResult = new System.Windows.Forms.Panel();
            this.pnlTargetSelector = new System.Windows.Forms.Panel();
            this.targetSelector = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTryResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLastResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbTargetType = new System.Windows.Forms.ComboBox();
            this.pnlShowAllResult = new System.Windows.Forms.Panel();
            this.ckbShowAllResult = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnScan = new System.Windows.Forms.Button();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposureTime)).BeginInit();
            this.panelParameter.SuspendLayout();
            this.panelTarget.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlTarget.SuspendLayout();
            this.pnlTargetSelector.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetSelector)).BeginInit();
            this.pnlShowAllResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.panelTop.Controls.Add(this.addFiducialButton);
            this.panelTop.Controls.Add(this.nudExposureTime);
            this.panelTop.Controls.Add(this.btnScan);
            this.panelTop.Controls.Add(this.btnLoadImage);
            this.panelTop.Controls.Add(this.labelMs);
            this.panelTop.Controls.Add(this.btnGrab);
            this.panelTop.Controls.Add(this.btnPasteTarget);
            this.panelTop.Controls.Add(this.labelExposure);
            this.panelTop.Controls.Add(this.btnCopyTarget);
            this.panelTop.Controls.Add(this.btnDeleteTarget);
            this.panelTop.Controls.Add(this.btnAddTarget);
            this.panelTop.Controls.Add(this.btnInspect);
            this.panelTop.Controls.Add(this.btnSave);
            this.panelTop.Controls.Add(this.labelModelMode);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(5);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1571, 86);
            this.panelTop.TabIndex = 107;
            // 
            // addFiducialButton
            // 
            this.addFiducialButton.BackColor = System.Drawing.Color.DodgerBlue;
            this.addFiducialButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addFiducialButton.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F);
            this.addFiducialButton.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.addFiducialButton.Image = global::UniScanX.MPAlignment.Properties.Resources.addfid_48;
            this.addFiducialButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.addFiducialButton.Location = new System.Drawing.Point(161, 8);
            this.addFiducialButton.Margin = new System.Windows.Forms.Padding(5);
            this.addFiducialButton.Name = "addFiducialButton";
            this.addFiducialButton.Size = new System.Drawing.Size(64, 72);
            this.addFiducialButton.TabIndex = 147;
            this.addFiducialButton.Text = "Add";
            this.addFiducialButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.addFiducialButton.UseVisualStyleBackColor = false;
            this.addFiducialButton.Click += new System.EventHandler(this.addFiducialButton_Click);
            // 
            // nudExposureTime
            // 
            this.nudExposureTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudExposureTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudExposureTime.ForeColor = System.Drawing.SystemColors.Window;
            this.nudExposureTime.Location = new System.Drawing.Point(679, 42);
            this.nudExposureTime.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.nudExposureTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudExposureTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudExposureTime.Name = "nudExposureTime";
            this.nudExposureTime.Size = new System.Drawing.Size(71, 25);
            this.nudExposureTime.TabIndex = 166;
            this.nudExposureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudExposureTime.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.BackColor = System.Drawing.Color.SlateBlue;
            this.btnLoadImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadImage.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadImage.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLoadImage.Image = global::UniScanX.MPAlignment.Properties.Resources.pic_48;
            this.btnLoadImage.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnLoadImage.Location = new System.Drawing.Point(459, 8);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(64, 72);
            this.btnLoadImage.TabIndex = 165;
            this.btnLoadImage.Text = "Load";
            this.btnLoadImage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadImage.UseVisualStyleBackColor = false;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // labelMs
            // 
            this.labelMs.AutoSize = true;
            this.labelMs.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelMs.Location = new System.Drawing.Point(758, 43);
            this.labelMs.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(31, 21);
            this.labelMs.TabIndex = 150;
            this.labelMs.Text = "ms";
            // 
            // btnGrab
            // 
            this.btnGrab.BackColor = System.Drawing.Color.SlateBlue;
            this.btnGrab.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrab.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrab.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnGrab.Image = global::UniScanX.MPAlignment.Properties.Resources.grab_481;
            this.btnGrab.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnGrab.Location = new System.Drawing.Point(599, 8);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(64, 72);
            this.btnGrab.TabIndex = 164;
            this.btnGrab.Text = "Grab";
            this.btnGrab.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGrab.UseVisualStyleBackColor = false;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Clicked);
            // 
            // btnPasteTarget
            // 
            this.btnPasteTarget.BackColor = System.Drawing.Color.SlateBlue;
            this.btnPasteTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPasteTarget.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPasteTarget.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnPasteTarget.Image = global::UniScanX.MPAlignment.Properties.Resources.paste_48;
            this.btnPasteTarget.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPasteTarget.Location = new System.Drawing.Point(374, 8);
            this.btnPasteTarget.Name = "btnPasteTarget";
            this.btnPasteTarget.Size = new System.Drawing.Size(64, 72);
            this.btnPasteTarget.TabIndex = 163;
            this.btnPasteTarget.Text = "Paste";
            this.btnPasteTarget.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPasteTarget.UseVisualStyleBackColor = false;
            this.btnPasteTarget.Click += new System.EventHandler(this.btnPasteTarget_Click);
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelExposure.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposure.Location = new System.Drawing.Point(675, 14);
            this.labelExposure.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(70, 20);
            this.labelExposure.TabIndex = 151;
            this.labelExposure.Text = "Exposure";
            // 
            // btnCopyTarget
            // 
            this.btnCopyTarget.BackColor = System.Drawing.Color.SlateBlue;
            this.btnCopyTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyTarget.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyTarget.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCopyTarget.Image = global::UniScanX.MPAlignment.Properties.Resources.copy_48;
            this.btnCopyTarget.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCopyTarget.Location = new System.Drawing.Point(303, 8);
            this.btnCopyTarget.Name = "btnCopyTarget";
            this.btnCopyTarget.Size = new System.Drawing.Size(64, 72);
            this.btnCopyTarget.TabIndex = 162;
            this.btnCopyTarget.Text = "Copy";
            this.btnCopyTarget.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCopyTarget.UseVisualStyleBackColor = false;
            this.btnCopyTarget.Click += new System.EventHandler(this.btnCopyTarget_Click);
            // 
            // btnDeleteTarget
            // 
            this.btnDeleteTarget.BackColor = System.Drawing.Color.SlateBlue;
            this.btnDeleteTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTarget.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteTarget.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDeleteTarget.Image = global::UniScanX.MPAlignment.Properties.Resources.delete_48;
            this.btnDeleteTarget.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeleteTarget.Location = new System.Drawing.Point(232, 8);
            this.btnDeleteTarget.Name = "btnDeleteTarget";
            this.btnDeleteTarget.Size = new System.Drawing.Size(64, 72);
            this.btnDeleteTarget.TabIndex = 161;
            this.btnDeleteTarget.Text = "Delete";
            this.btnDeleteTarget.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteTarget.UseVisualStyleBackColor = false;
            this.btnDeleteTarget.Click += new System.EventHandler(this.btnDeleteTarget_Click);
            // 
            // btnAddTarget
            // 
            this.btnAddTarget.BackColor = System.Drawing.Color.SlateBlue;
            this.btnAddTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTarget.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTarget.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAddTarget.Image = global::UniScanX.MPAlignment.Properties.Resources.add_48;
            this.btnAddTarget.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddTarget.Location = new System.Drawing.Point(90, 8);
            this.btnAddTarget.Name = "btnAddTarget";
            this.btnAddTarget.Size = new System.Drawing.Size(64, 72);
            this.btnAddTarget.TabIndex = 160;
            this.btnAddTarget.Text = "Add";
            this.btnAddTarget.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTarget.UseVisualStyleBackColor = false;
            this.btnAddTarget.Click += new System.EventHandler(this.btnAddTarget_Click);
            // 
            // btnInspect
            // 
            this.btnInspect.BackColor = System.Drawing.Color.SlateBlue;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInspect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnInspect.Image = global::UniScanX.MPAlignment.Properties.Resources.insepct_48;
            this.btnInspect.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnInspect.Location = new System.Drawing.Point(6, 7);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(64, 72);
            this.btnInspect.TabIndex = 159;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Image = global::UniScanX.MPAlignment.Properties.Resources.save_481;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(1327, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(64, 72);
            this.btnSave.TabIndex = 158;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // labelModelMode
            // 
            this.labelModelMode.AutoSize = true;
            this.labelModelMode.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelModelMode.Location = new System.Drawing.Point(5, 0);
            this.labelModelMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelModelMode.Name = "labelModelMode";
            this.labelModelMode.Size = new System.Drawing.Size(0, 21);
            this.labelModelMode.TabIndex = 153;
            // 
            // panelParameter
            // 
            this.panelParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelParameter.Controls.Add(this.lblMessage);
            this.panelParameter.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelParameter.Location = new System.Drawing.Point(710, 86);
            this.panelParameter.Margin = new System.Windows.Forms.Padding(5);
            this.panelParameter.Name = "panelParameter";
            this.panelParameter.Size = new System.Drawing.Size(861, 850);
            this.panelParameter.TabIndex = 115;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblMessage.Location = new System.Drawing.Point(235, 289);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(163, 21);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Select target please.";
            // 
            // labelProbePos
            // 
            this.labelProbePos.BackColor = System.Drawing.Color.Navy;
            this.labelProbePos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelProbePos.ForeColor = System.Drawing.Color.White;
            this.labelProbePos.Location = new System.Drawing.Point(187, 317);
            this.labelProbePos.Margin = new System.Windows.Forms.Padding(0);
            this.labelProbePos.Name = "labelProbePos";
            this.labelProbePos.Size = new System.Drawing.Size(98, 40);
            this.labelProbePos.TabIndex = 129;
            this.labelProbePos.Text = "Pos";
            this.labelProbePos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTarget
            // 
            this.panelTarget.Controls.Add(this.pnlFov);
            this.panelTarget.Controls.Add(this.pnlBottom);
            this.panelTarget.Controls.Add(this.pnlTarget);
            this.panelTarget.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTarget.Location = new System.Drawing.Point(0, 86);
            this.panelTarget.Margin = new System.Windows.Forms.Padding(5);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(710, 850);
            this.panelTarget.TabIndex = 116;
            // 
            // pnlFov
            // 
            this.pnlFov.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlFov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFov.Location = new System.Drawing.Point(0, 40);
            this.pnlFov.Name = "pnlFov";
            this.pnlFov.Size = new System.Drawing.Size(710, 521);
            this.pnlFov.TabIndex = 122;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottom.Controls.Add(this.buttonZoomFit);
            this.pnlBottom.Controls.Add(this.buttonZoomIn);
            this.pnlBottom.Controls.Add(this.buttonZoomOut);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottom.Location = new System.Drawing.Point(0, 0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(710, 40);
            this.pnlBottom.TabIndex = 154;
            // 
            // buttonZoomFit
            // 
            this.buttonZoomFit.FlatAppearance.BorderSize = 0;
            this.buttonZoomFit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomFit.Image = global::UniScanX.MPAlignment.Properties.Resources.zoomfit_48;
            this.buttonZoomFit.Location = new System.Drawing.Point(79, 1);
            this.buttonZoomFit.Name = "buttonZoomFit";
            this.buttonZoomFit.Size = new System.Drawing.Size(32, 32);
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
            this.buttonZoomIn.Image = global::UniScanX.MPAlignment.Properties.Resources.zoom_48;
            this.buttonZoomIn.Location = new System.Drawing.Point(3, 1);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(32, 32);
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
            this.buttonZoomOut.Image = global::UniScanX.MPAlignment.Properties.Resources.zoomout_48;
            this.buttonZoomOut.Location = new System.Drawing.Point(41, 1);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(32, 32);
            this.buttonZoomOut.TabIndex = 151;
            this.buttonZoomOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // pnlTarget
            // 
            this.pnlTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlTarget.Controls.Add(this.pnlInspectionResult);
            this.pnlTarget.Controls.Add(this.pnlTargetSelector);
            this.pnlTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlTarget.Location = new System.Drawing.Point(0, 561);
            this.pnlTarget.Name = "pnlTarget";
            this.pnlTarget.Size = new System.Drawing.Size(710, 289);
            this.pnlTarget.TabIndex = 123;
            // 
            // pnlInspectionResult
            // 
            this.pnlInspectionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInspectionResult.Location = new System.Drawing.Point(200, 0);
            this.pnlInspectionResult.Name = "pnlInspectionResult";
            this.pnlInspectionResult.Size = new System.Drawing.Size(510, 289);
            this.pnlInspectionResult.TabIndex = 11;
            // 
            // pnlTargetSelector
            // 
            this.pnlTargetSelector.Controls.Add(this.targetSelector);
            this.pnlTargetSelector.Controls.Add(this.cmbTargetType);
            this.pnlTargetSelector.Controls.Add(this.pnlShowAllResult);
            this.pnlTargetSelector.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTargetSelector.Location = new System.Drawing.Point(0, 0);
            this.pnlTargetSelector.Name = "pnlTargetSelector";
            this.pnlTargetSelector.Size = new System.Drawing.Size(200, 289);
            this.pnlTargetSelector.TabIndex = 12;
            // 
            // targetSelector
            // 
            this.targetSelector.AllowUserToAddRows = false;
            this.targetSelector.AllowUserToDeleteRows = false;
            this.targetSelector.AllowUserToResizeColumns = false;
            this.targetSelector.AllowUserToResizeRows = false;
            this.targetSelector.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.targetSelector.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.targetSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.targetSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnTryResult,
            this.ColumnLastResult});
            this.targetSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetSelector.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.targetSelector.Location = new System.Drawing.Point(0, 58);
            this.targetSelector.Margin = new System.Windows.Forms.Padding(5);
            this.targetSelector.MultiSelect = false;
            this.targetSelector.Name = "targetSelector";
            this.targetSelector.ReadOnly = true;
            this.targetSelector.RowHeadersVisible = false;
            this.targetSelector.RowTemplate.Height = 23;
            this.targetSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.targetSelector.ShowEditingIcon = false;
            this.targetSelector.Size = new System.Drawing.Size(200, 231);
            this.targetSelector.TabIndex = 10;
            this.targetSelector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.targetSelector_CellClick);
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnTryResult
            // 
            this.ColumnTryResult.HeaderText = "T";
            this.ColumnTryResult.Name = "ColumnTryResult";
            this.ColumnTryResult.ReadOnly = true;
            this.ColumnTryResult.Width = 30;
            // 
            // ColumnLastResult
            // 
            this.ColumnLastResult.HeaderText = "L";
            this.ColumnLastResult.Name = "ColumnLastResult";
            this.ColumnLastResult.ReadOnly = true;
            this.ColumnLastResult.Width = 30;
            // 
            // cmbTargetType
            // 
            this.cmbTargetType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetType.FormattingEnabled = true;
            this.cmbTargetType.Location = new System.Drawing.Point(0, 29);
            this.cmbTargetType.Name = "cmbTargetType";
            this.cmbTargetType.Size = new System.Drawing.Size(200, 29);
            this.cmbTargetType.TabIndex = 11;
            this.cmbTargetType.SelectedIndexChanged += new System.EventHandler(this.cmbTargetType_SelectedIndexChanged);
            // 
            // pnlShowAllResult
            // 
            this.pnlShowAllResult.Controls.Add(this.ckbShowAllResult);
            this.pnlShowAllResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlShowAllResult.Location = new System.Drawing.Point(0, 0);
            this.pnlShowAllResult.Name = "pnlShowAllResult";
            this.pnlShowAllResult.Size = new System.Drawing.Size(200, 29);
            this.pnlShowAllResult.TabIndex = 12;
            this.pnlShowAllResult.Visible = false;
            // 
            // ckbShowAllResult
            // 
            this.ckbShowAllResult.AutoSize = true;
            this.ckbShowAllResult.ForeColor = System.Drawing.Color.White;
            this.ckbShowAllResult.Location = new System.Drawing.Point(6, 3);
            this.ckbShowAllResult.Name = "ckbShowAllResult";
            this.ckbShowAllResult.Size = new System.Drawing.Size(145, 25);
            this.ckbShowAllResult.TabIndex = 0;
            this.ckbShowAllResult.Text = "Show All Result";
            this.ckbShowAllResult.UseVisualStyleBackColor = true;
            this.ckbShowAllResult.CheckedChanged += new System.EventHandler(this.ckbShowAllResult_CheckedChanged);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnScan
            // 
            this.btnScan.BackColor = System.Drawing.Color.SlateBlue;
            this.btnScan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScan.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnScan.Image = global::UniScanX.MPAlignment.Properties.Resources.pic_48;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnScan.Location = new System.Drawing.Point(529, 8);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(64, 72);
            this.btnScan.TabIndex = 165;
            this.btnScan.Text = "Scan";
            this.btnScan.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnScan.UseVisualStyleBackColor = false;
            this.btnScan.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // ModellerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.panelTarget);
            this.Controls.Add(this.panelParameter);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ModellerPage";
            this.Size = new System.Drawing.Size(1571, 936);
            this.Load += new System.EventHandler(this.Modeller_Load);
            this.VisibleChanged += new System.EventHandler(this.ModellerPage_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Modeller_KeyDown);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposureTime)).EndInit();
            this.panelParameter.ResumeLayout(false);
            this.panelParameter.PerformLayout();
            this.panelTarget.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlTarget.ResumeLayout(false);
            this.pnlTargetSelector.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.targetSelector)).EndInit();
            this.pnlShowAllResult.ResumeLayout(false);
            this.pnlShowAllResult.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelParameter;
        private System.Windows.Forms.Label labelProbePos;
        private System.Windows.Forms.Panel panelTarget;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelModelMode;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.DataGridView targetSelector;
        private System.Windows.Forms.Button addFiducialButton;
        private System.Windows.Forms.Button btnLoadImage;
        private System.Windows.Forms.Button btnGrab;
        private System.Windows.Forms.Button btnPasteTarget;
        private System.Windows.Forms.Button btnCopyTarget;
        private System.Windows.Forms.Button btnDeleteTarget;
        private System.Windows.Forms.Button btnAddTarget;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlFov;
        private System.Windows.Forms.Panel pnlTarget;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.NumericUpDown nudExposureTime;
        private System.Windows.Forms.Panel pnlInspectionResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTryResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLastResult;
        private Panel pnlTargetSelector;
        private ComboBox cmbTargetType;
        private Panel pnlBottom;
        private Button buttonZoomFit;
        private Button buttonZoomIn;
        private Button buttonZoomOut;
        private Panel pnlShowAllResult;
        private CheckBox ckbShowAllResult;
        private Timer timer1;
        private Button btnScan;
    }
}