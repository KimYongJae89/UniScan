namespace DynMvp.Devices.UI
{
    partial class LightConfigForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.LightConfigForm_Fill_Panel = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelNumLightUnit = new System.Windows.Forms.Label();
            this.labelTimeoutUnit = new System.Windows.Forms.Label();
            this.labelWaitResponceTimeout = new System.Windows.Forms.Label();
            this.labelNumLight = new System.Windows.Forms.Label();
            this.checkResponceTimeout = new System.Windows.Forms.NumericUpDown();
            this.numLight = new System.Windows.Forms.NumericUpDown();
            this.comboLightControllerVender = new System.Windows.Forms.ComboBox();
            this.buttonTestLightController = new System.Windows.Forms.Button();
            this.buttonAdvance = new System.Windows.Forms.Button();
            this.buttonEditLightCtrlPort = new System.Windows.Forms.Button();
            this.labelSerialPortInfo = new System.Windows.Forms.Label();
            this.useSerialLightCtrl = new System.Windows.Forms.RadioButton();
            this.useIoLightCtrl = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.LightConfigForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkResponceTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLight)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(203, 213);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(104, 36);
            this.buttonCancel.TabIndex = 154;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(97, 213);
            this.buttonOk.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(104, 36);
            this.buttonOk.TabIndex = 155;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(441, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 293);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(441, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 262);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(440, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 262);
            // 
            // LightConfigForm_Fill_Panel
            // 
            this.LightConfigForm_Fill_Panel.Controls.Add(this.txtName);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelName);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelNumLightUnit);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelTimeoutUnit);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelWaitResponceTimeout);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelNumLight);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.checkResponceTimeout);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.numLight);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.comboLightControllerVender);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.buttonTestLightController);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.buttonAdvance);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.buttonEditLightCtrlPort);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.labelSerialPortInfo);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.useSerialLightCtrl);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.useIoLightCtrl);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.buttonCancel);
            this.LightConfigForm_Fill_Panel.Controls.Add(this.buttonOk);
            this.LightConfigForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.LightConfigForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LightConfigForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.LightConfigForm_Fill_Panel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LightConfigForm_Fill_Panel.Name = "LightConfigForm_Fill_Panel";
            this.LightConfigForm_Fill_Panel.Size = new System.Drawing.Size(439, 262);
            this.LightConfigForm_Fill_Panel.TabIndex = 164;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(99, 9);
            this.txtName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(329, 24);
            this.txtName.TabIndex = 176;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(14, 13);
            this.labelName.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(48, 18);
            this.labelName.TabIndex = 175;
            this.labelName.Text = "Name";
            // 
            // labelNumLightUnit
            // 
            this.labelNumLightUnit.AutoSize = true;
            this.labelNumLightUnit.Location = new System.Drawing.Point(200, 42);
            this.labelNumLightUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumLightUnit.Name = "labelNumLightUnit";
            this.labelNumLightUnit.Size = new System.Drawing.Size(35, 18);
            this.labelNumLightUnit.TabIndex = 160;
            this.labelNumLightUnit.Text = "[EA]";
            // 
            // labelTimeoutUnit
            // 
            this.labelTimeoutUnit.AutoSize = true;
            this.labelTimeoutUnit.Location = new System.Drawing.Point(391, 44);
            this.labelTimeoutUnit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTimeoutUnit.Name = "labelTimeoutUnit";
            this.labelTimeoutUnit.Size = new System.Drawing.Size(37, 18);
            this.labelTimeoutUnit.TabIndex = 160;
            this.labelTimeoutUnit.Text = "[ms]";
            // 
            // labelWaitResponceTimeout
            // 
            this.labelWaitResponceTimeout.AutoSize = true;
            this.labelWaitResponceTimeout.Location = new System.Drawing.Point(247, 44);
            this.labelWaitResponceTimeout.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelWaitResponceTimeout.Name = "labelWaitResponceTimeout";
            this.labelWaitResponceTimeout.Size = new System.Drawing.Size(62, 18);
            this.labelWaitResponceTimeout.TabIndex = 160;
            this.labelWaitResponceTimeout.Text = "Timeout";
            // 
            // labelNumLight
            // 
            this.labelNumLight.AutoSize = true;
            this.labelNumLight.Location = new System.Drawing.Point(14, 43);
            this.labelNumLight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNumLight.Name = "labelNumLight";
            this.labelNumLight.Size = new System.Drawing.Size(75, 18);
            this.labelNumLight.TabIndex = 160;
            this.labelNumLight.Text = "Num Light";
            // 
            // checkResponceTimeout
            // 
            this.checkResponceTimeout.Location = new System.Drawing.Point(311, 41);
            this.checkResponceTimeout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkResponceTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.checkResponceTimeout.Name = "checkResponceTimeout";
            this.checkResponceTimeout.Size = new System.Drawing.Size(78, 24);
            this.checkResponceTimeout.TabIndex = 161;
            this.checkResponceTimeout.ValueChanged += new System.EventHandler(this.checkResponceTimeout_ValueChanged);
            // 
            // numLight
            // 
            this.numLight.Location = new System.Drawing.Point(99, 40);
            this.numLight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numLight.Name = "numLight";
            this.numLight.Size = new System.Drawing.Size(100, 24);
            this.numLight.TabIndex = 161;
            this.numLight.ValueChanged += new System.EventHandler(this.numLight_ValueChanged);
            // 
            // comboLightControllerVender
            // 
            this.comboLightControllerVender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLightControllerVender.FormattingEnabled = true;
            this.comboLightControllerVender.Items.AddRange(new object[] {
            "Iovis",
            "Movis"});
            this.comboLightControllerVender.Location = new System.Drawing.Point(29, 173);
            this.comboLightControllerVender.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboLightControllerVender.Name = "comboLightControllerVender";
            this.comboLightControllerVender.Size = new System.Drawing.Size(310, 26);
            this.comboLightControllerVender.TabIndex = 159;
            this.comboLightControllerVender.SelectedIndexChanged += new System.EventHandler(this.comboLightControllerVender_SelectedIndexChanged);
            // 
            // buttonTestLightController
            // 
            this.buttonTestLightController.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonTestLightController.Location = new System.Drawing.Point(340, 213);
            this.buttonTestLightController.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTestLightController.Name = "buttonTestLightController";
            this.buttonTestLightController.Size = new System.Drawing.Size(87, 36);
            this.buttonTestLightController.TabIndex = 158;
            this.buttonTestLightController.Text = "Test";
            this.buttonTestLightController.UseVisualStyleBackColor = true;
            this.buttonTestLightController.Click += new System.EventHandler(this.buttonTestLightController_Click);
            // 
            // buttonAdvance
            // 
            this.buttonAdvance.Location = new System.Drawing.Point(347, 173);
            this.buttonAdvance.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonAdvance.Name = "buttonAdvance";
            this.buttonAdvance.Size = new System.Drawing.Size(80, 26);
            this.buttonAdvance.TabIndex = 158;
            this.buttonAdvance.Text = "Advanced";
            this.buttonAdvance.UseVisualStyleBackColor = true;
            this.buttonAdvance.Click += new System.EventHandler(this.buttonAdvance_Click);
            // 
            // buttonEditLightCtrlPort
            // 
            this.buttonEditLightCtrlPort.Location = new System.Drawing.Point(347, 136);
            this.buttonEditLightCtrlPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEditLightCtrlPort.Name = "buttonEditLightCtrlPort";
            this.buttonEditLightCtrlPort.Size = new System.Drawing.Size(80, 32);
            this.buttonEditLightCtrlPort.TabIndex = 158;
            this.buttonEditLightCtrlPort.Text = "Edit";
            this.buttonEditLightCtrlPort.UseVisualStyleBackColor = true;
            this.buttonEditLightCtrlPort.Click += new System.EventHandler(this.buttonEditLightCtrlPort_Click);
            // 
            // labelSerialPortInfo
            // 
            this.labelSerialPortInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSerialPortInfo.Location = new System.Drawing.Point(29, 136);
            this.labelSerialPortInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSerialPortInfo.Name = "labelSerialPortInfo";
            this.labelSerialPortInfo.Size = new System.Drawing.Size(310, 32);
            this.labelSerialPortInfo.TabIndex = 157;
            this.labelSerialPortInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // useSerialLightCtrl
            // 
            this.useSerialLightCtrl.AutoSize = true;
            this.useSerialLightCtrl.Location = new System.Drawing.Point(14, 109);
            this.useSerialLightCtrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.useSerialLightCtrl.Name = "useSerialLightCtrl";
            this.useSerialLightCtrl.Size = new System.Drawing.Size(167, 22);
            this.useSerialLightCtrl.TabIndex = 156;
            this.useSerialLightCtrl.TabStop = true;
            this.useSerialLightCtrl.Text = "Serial Light Controller";
            this.useSerialLightCtrl.UseVisualStyleBackColor = true;
            this.useSerialLightCtrl.CheckedChanged += new System.EventHandler(this.useSerialLightCtrl_CheckedChanged);
            // 
            // useIoLightCtrl
            // 
            this.useIoLightCtrl.AutoSize = true;
            this.useIoLightCtrl.Location = new System.Drawing.Point(14, 77);
            this.useIoLightCtrl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.useIoLightCtrl.Name = "useIoLightCtrl";
            this.useIoLightCtrl.Size = new System.Drawing.Size(149, 22);
            this.useIoLightCtrl.TabIndex = 156;
            this.useIoLightCtrl.TabStop = true;
            this.useIoLightCtrl.Text = "I/O Light Controller";
            this.useIoLightCtrl.UseVisualStyleBackColor = true;
            this.useIoLightCtrl.CheckedChanged += new System.EventHandler(this.useIoLightCtrl_CheckedChanged);
            // 
            // LightConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 294);
            this.Controls.Add(this.LightConfigForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "LightConfigForm";
            this.Text = "Config Light Controller";
            this.Load += new System.EventHandler(this.LightConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.LightConfigForm_Fill_Panel.ResumeLayout(false);
            this.LightConfigForm_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkResponceTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private System.Windows.Forms.Panel LightConfigForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.RadioButton useIoLightCtrl;
        private System.Windows.Forms.RadioButton useSerialLightCtrl;
        private System.Windows.Forms.Label labelSerialPortInfo;
        private System.Windows.Forms.Button buttonEditLightCtrlPort;
        private System.Windows.Forms.ComboBox comboLightControllerVender;
        private System.Windows.Forms.Button buttonTestLightController;
        private System.Windows.Forms.Label labelNumLight;
        private System.Windows.Forms.NumericUpDown numLight;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelTimeoutUnit;
        private System.Windows.Forms.Label labelWaitResponceTimeout;
        private System.Windows.Forms.NumericUpDown checkResponceTimeout;
        private System.Windows.Forms.Label labelNumLightUnit;
        private System.Windows.Forms.Button buttonAdvance;
    }
}