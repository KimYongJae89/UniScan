namespace UniScanM.UI
{
    partial class ModelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelForm));
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupFundamental = new System.Windows.Forms.GroupBox();
            this.paste = new System.Windows.Forms.TextBox();
            this.labelPaste = new System.Windows.Forms.Label();
            this.thickness = new System.Windows.Forms.TextBox();
            this.labelThickness = new System.Windows.Forms.Label();
            this.labelScreenModel = new System.Windows.Forms.Label();
            this.modelName = new System.Windows.Forms.TextBox();
            this.groupSelection = new System.Windows.Forms.GroupBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.registrant = new System.Windows.Forms.TextBox();
            this.labelRegistrant = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupFundamental.SuspendLayout();
            this.groupSelection.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
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
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(353, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 419);
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
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 419);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 450);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(354, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(354, 31);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // groupFundamental
            // 
            this.groupFundamental.Controls.Add(this.paste);
            this.groupFundamental.Controls.Add(this.labelPaste);
            this.groupFundamental.Controls.Add(this.thickness);
            this.groupFundamental.Controls.Add(this.labelThickness);
            this.groupFundamental.Controls.Add(this.labelScreenModel);
            this.groupFundamental.Controls.Add(this.modelName);
            this.groupFundamental.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupFundamental.Font = new System.Drawing.Font("Malgun Gothic", 14F);
            this.groupFundamental.Location = new System.Drawing.Point(1, 31);
            this.groupFundamental.Name = "groupFundamental";
            this.groupFundamental.Size = new System.Drawing.Size(352, 156);
            this.groupFundamental.TabIndex = 0;
            this.groupFundamental.TabStop = false;
            this.groupFundamental.Text = "Fundamental Item";
            // 
            // paste
            // 
            this.paste.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paste.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.paste.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paste.Location = new System.Drawing.Point(178, 114);
            this.paste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.paste.Name = "paste";
            this.paste.Size = new System.Drawing.Size(161, 27);
            this.paste.TabIndex = 0;
            // 
            // labelPaste
            // 
            this.labelPaste.BackColor = System.Drawing.Color.Navy;
            this.labelPaste.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPaste.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.labelPaste.ForeColor = System.Drawing.Color.White;
            this.labelPaste.Location = new System.Drawing.Point(10, 114);
            this.labelPaste.Margin = new System.Windows.Forms.Padding(0);
            this.labelPaste.Name = "labelPaste";
            this.labelPaste.Size = new System.Drawing.Size(164, 27);
            this.labelPaste.TabIndex = 0;
            this.labelPaste.Text = "Paste";
            this.labelPaste.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // thickness
            // 
            this.thickness.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thickness.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.thickness.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.thickness.Location = new System.Drawing.Point(178, 75);
            this.thickness.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.thickness.Name = "thickness";
            this.thickness.Size = new System.Drawing.Size(161, 27);
            this.thickness.TabIndex = 0;
            // 
            // labelThickness
            // 
            this.labelThickness.BackColor = System.Drawing.Color.Navy;
            this.labelThickness.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelThickness.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.labelThickness.ForeColor = System.Drawing.Color.White;
            this.labelThickness.Location = new System.Drawing.Point(10, 75);
            this.labelThickness.Margin = new System.Windows.Forms.Padding(0);
            this.labelThickness.Name = "labelThickness";
            this.labelThickness.Size = new System.Drawing.Size(164, 27);
            this.labelThickness.TabIndex = 0;
            this.labelThickness.Text = "Thickness (um)";
            this.labelThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScreenModel
            // 
            this.labelScreenModel.BackColor = System.Drawing.Color.Navy;
            this.labelScreenModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelScreenModel.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.labelScreenModel.ForeColor = System.Drawing.Color.White;
            this.labelScreenModel.Location = new System.Drawing.Point(10, 39);
            this.labelScreenModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelScreenModel.Name = "labelScreenModel";
            this.labelScreenModel.Size = new System.Drawing.Size(164, 27);
            this.labelScreenModel.TabIndex = 0;
            this.labelScreenModel.Text = "Name";
            this.labelScreenModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelName
            // 
            this.modelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.modelName.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.modelName.Location = new System.Drawing.Point(178, 39);
            this.modelName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(161, 27);
            this.modelName.TabIndex = 0;
            // 
            // groupSelection
            // 
            this.groupSelection.Controls.Add(this.labelDescription);
            this.groupSelection.Controls.Add(this.description);
            this.groupSelection.Controls.Add(this.registrant);
            this.groupSelection.Controls.Add(this.labelRegistrant);
            this.groupSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupSelection.Font = new System.Drawing.Font("Malgun Gothic", 14F);
            this.groupSelection.Location = new System.Drawing.Point(1, 187);
            this.groupSelection.Name = "groupSelection";
            this.groupSelection.Size = new System.Drawing.Size(352, 207);
            this.groupSelection.TabIndex = 0;
            this.groupSelection.TabStop = false;
            this.groupSelection.Text = "Selection Item";
            // 
            // labelDescription
            // 
            this.labelDescription.BackColor = System.Drawing.Color.Navy;
            this.labelDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDescription.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.labelDescription.ForeColor = System.Drawing.Color.White;
            this.labelDescription.Location = new System.Drawing.Point(10, 65);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(164, 27);
            this.labelDescription.TabIndex = 0;
            this.labelDescription.Text = "Description";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.description.Location = new System.Drawing.Point(178, 65);
            this.description.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(161, 134);
            this.description.TabIndex = 0;
            // 
            // registrant
            // 
            this.registrant.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.registrant.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registrant.Location = new System.Drawing.Point(178, 28);
            this.registrant.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.registrant.Name = "registrant";
            this.registrant.Size = new System.Drawing.Size(161, 27);
            this.registrant.TabIndex = 0;
            // 
            // labelRegistrant
            // 
            this.labelRegistrant.BackColor = System.Drawing.Color.Navy;
            this.labelRegistrant.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelRegistrant.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.labelRegistrant.ForeColor = System.Drawing.Color.White;
            this.labelRegistrant.Location = new System.Drawing.Point(10, 28);
            this.labelRegistrant.Margin = new System.Windows.Forms.Padding(0);
            this.labelRegistrant.Name = "labelRegistrant";
            this.labelRegistrant.Size = new System.Drawing.Size(164, 27);
            this.labelRegistrant.TabIndex = 0;
            this.labelRegistrant.Text = "Registrant";
            this.labelRegistrant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.btnCancel.Location = new System.Drawing.Point(178, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 38);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnOK.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.btnOK.Location = new System.Drawing.Point(66, 5);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 37);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // buttonPanel
            // 
            this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPanel.Controls.Add(this.btnOK);
            this.buttonPanel.Controls.Add(this.btnCancel);
            this.buttonPanel.Location = new System.Drawing.Point(1, 399);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(352, 50);
            this.buttonPanel.TabIndex = 0;
            // 
            // ModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 451);
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.groupSelection);
            this.Controls.Add(this.groupFundamental);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Malgun Gothic", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "\\";
            this.Load += new System.EventHandler(this.ModelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupFundamental.ResumeLayout(false);
            this.groupFundamental.PerformLayout();
            this.groupSelection.ResumeLayout(false);
            this.groupSelection.PerformLayout();
            this.buttonPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.GroupBox groupFundamental;
        private System.Windows.Forms.TextBox paste;
        private System.Windows.Forms.Label labelPaste;
        private System.Windows.Forms.TextBox thickness;
        private System.Windows.Forms.Label labelThickness;
        private System.Windows.Forms.Label labelScreenModel;
        private System.Windows.Forms.TextBox modelName;
        private System.Windows.Forms.GroupBox groupSelection;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.TextBox registrant;
        private System.Windows.Forms.Label labelRegistrant;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}