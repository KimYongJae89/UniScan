namespace UniScanM.Gloss.UI
{
    partial class InspectionPanelRight
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
            this.layoutPatternView = new System.Windows.Forms.TableLayoutPanel();
            this.labelState = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBarZeroing = new System.Windows.Forms.ProgressBar();
            this.tableScanWidth = new System.Windows.Forms.TableLayoutPanel();
            this.labelLotNo = new System.Windows.Forms.Label();
            this.buttonCalibration = new System.Windows.Forms.Button();
            this.labelScanWidth = new System.Windows.Forms.Label();
            this.comboScanWidth = new System.Windows.Forms.ComboBox();
            this.buttonSafety = new System.Windows.Forms.Button();
            this.textLotNo = new System.Windows.Forms.TextBox();
            this.labelCalParam = new System.Windows.Forms.Label();
            this.comboCalParam = new System.Windows.Forms.ComboBox();
            this.layoutPatternView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableScanWidth.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPatternView
            // 
            this.layoutPatternView.ColumnCount = 1;
            this.layoutPatternView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPatternView.Controls.Add(this.labelState, 0, 0);
            this.layoutPatternView.Controls.Add(this.panel1, 0, 1);
            this.layoutPatternView.Controls.Add(this.tableScanWidth, 0, 2);
            this.layoutPatternView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPatternView.Location = new System.Drawing.Point(0, 0);
            this.layoutPatternView.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPatternView.Name = "layoutPatternView";
            this.layoutPatternView.RowCount = 3;
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutPatternView.Size = new System.Drawing.Size(499, 431);
            this.layoutPatternView.TabIndex = 30;
            // 
            // labelState
            // 
            this.labelState.BackColor = System.Drawing.Color.Black;
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("맑은 고딕", 26F, System.Drawing.FontStyle.Bold);
            this.labelState.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelState.Location = new System.Drawing.Point(0, 0);
            this.labelState.Margin = new System.Windows.Forms.Padding(0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(499, 50);
            this.labelState.TabIndex = 44;
            this.labelState.Text = "State";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBarZeroing);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 40);
            this.panel1.TabIndex = 0;
            // 
            // progressBarZeroing
            // 
            this.progressBarZeroing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarZeroing.Location = new System.Drawing.Point(0, 0);
            this.progressBarZeroing.Margin = new System.Windows.Forms.Padding(0);
            this.progressBarZeroing.Name = "progressBarZeroing";
            this.progressBarZeroing.Size = new System.Drawing.Size(499, 40);
            this.progressBarZeroing.TabIndex = 45;
            // 
            // tableScanWidth
            // 
            this.tableScanWidth.ColumnCount = 2;
            this.tableScanWidth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableScanWidth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableScanWidth.Controls.Add(this.comboCalParam, 1, 2);
            this.tableScanWidth.Controls.Add(this.labelCalParam, 0, 2);
            this.tableScanWidth.Controls.Add(this.labelLotNo, 0, 0);
            this.tableScanWidth.Controls.Add(this.buttonCalibration, 1, 3);
            this.tableScanWidth.Controls.Add(this.labelScanWidth, 0, 1);
            this.tableScanWidth.Controls.Add(this.comboScanWidth, 1, 1);
            this.tableScanWidth.Controls.Add(this.buttonSafety, 0, 3);
            this.tableScanWidth.Controls.Add(this.textLotNo, 1, 0);
            this.tableScanWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableScanWidth.Location = new System.Drawing.Point(0, 90);
            this.tableScanWidth.Margin = new System.Windows.Forms.Padding(0);
            this.tableScanWidth.Name = "tableScanWidth";
            this.tableScanWidth.RowCount = 5;
            this.tableScanWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableScanWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableScanWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableScanWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableScanWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableScanWidth.Size = new System.Drawing.Size(499, 341);
            this.tableScanWidth.TabIndex = 45;
            // 
            // labelLotNo
            // 
            this.labelLotNo.AutoSize = true;
            this.labelLotNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLotNo.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLotNo.Location = new System.Drawing.Point(5, 5);
            this.labelLotNo.Margin = new System.Windows.Forms.Padding(5);
            this.labelLotNo.Name = "labelLotNo";
            this.labelLotNo.Size = new System.Drawing.Size(239, 35);
            this.labelLotNo.TabIndex = 4;
            this.labelLotNo.Text = "Lot No";
            this.labelLotNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCalibration
            // 
            this.buttonCalibration.Location = new System.Drawing.Point(252, 138);
            this.buttonCalibration.Name = "buttonCalibration";
            this.buttonCalibration.Size = new System.Drawing.Size(243, 39);
            this.buttonCalibration.TabIndex = 3;
            this.buttonCalibration.Text = "Calibration Position";
            this.buttonCalibration.UseVisualStyleBackColor = true;
            this.buttonCalibration.Visible = false;
            this.buttonCalibration.Click += new System.EventHandler(this.buttonCalibration_Click);
            // 
            // labelScanWidth
            // 
            this.labelScanWidth.AutoSize = true;
            this.labelScanWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScanWidth.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelScanWidth.Location = new System.Drawing.Point(5, 50);
            this.labelScanWidth.Margin = new System.Windows.Forms.Padding(5);
            this.labelScanWidth.Name = "labelScanWidth";
            this.labelScanWidth.Size = new System.Drawing.Size(239, 35);
            this.labelScanWidth.TabIndex = 0;
            this.labelScanWidth.Text = "Scan Width";
            this.labelScanWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboScanWidth
            // 
            this.comboScanWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboScanWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScanWidth.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboScanWidth.FormattingEnabled = true;
            this.comboScanWidth.Location = new System.Drawing.Point(254, 50);
            this.comboScanWidth.Margin = new System.Windows.Forms.Padding(5);
            this.comboScanWidth.Name = "comboScanWidth";
            this.comboScanWidth.Size = new System.Drawing.Size(240, 38);
            this.comboScanWidth.TabIndex = 1;
            this.comboScanWidth.SelectedIndexChanged += new System.EventHandler(this.comboScanWidth_SelectedIndexChanged);
            // 
            // buttonSafety
            // 
            this.buttonSafety.Location = new System.Drawing.Point(3, 138);
            this.buttonSafety.Name = "buttonSafety";
            this.buttonSafety.Size = new System.Drawing.Size(243, 39);
            this.buttonSafety.TabIndex = 2;
            this.buttonSafety.Text = "Safety Position";
            this.buttonSafety.UseVisualStyleBackColor = true;
            this.buttonSafety.Visible = false;
            this.buttonSafety.Click += new System.EventHandler(this.buttonSafety_Click);
            // 
            // textLotNo
            // 
            this.textLotNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textLotNo.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textLotNo.Location = new System.Drawing.Point(254, 5);
            this.textLotNo.Margin = new System.Windows.Forms.Padding(5);
            this.textLotNo.Name = "textLotNo";
            this.textLotNo.Size = new System.Drawing.Size(240, 35);
            this.textLotNo.TabIndex = 5;
            this.textLotNo.TextChanged += new System.EventHandler(this.textLotNo_TextChanged);
            // 
            // labelCalParam
            // 
            this.labelCalParam.AutoSize = true;
            this.labelCalParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCalParam.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCalParam.Location = new System.Drawing.Point(5, 95);
            this.labelCalParam.Margin = new System.Windows.Forms.Padding(5);
            this.labelCalParam.Name = "labelCalParam";
            this.labelCalParam.Size = new System.Drawing.Size(239, 35);
            this.labelCalParam.TabIndex = 6;
            this.labelCalParam.Text = "Cal Param";
            this.labelCalParam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboCalParam
            // 
            this.comboCalParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboCalParam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCalParam.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboCalParam.FormattingEnabled = true;
            this.comboCalParam.Location = new System.Drawing.Point(254, 95);
            this.comboCalParam.Margin = new System.Windows.Forms.Padding(5);
            this.comboCalParam.Name = "comboCalParam";
            this.comboCalParam.Size = new System.Drawing.Size(240, 38);
            this.comboCalParam.TabIndex = 7;
            this.comboCalParam.SelectedIndexChanged += new System.EventHandler(this.comboCalParam_SelectedIndexChanged);
            // 
            // InspectionPanelRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPatternView);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "InspectionPanelRight";
            this.Size = new System.Drawing.Size(499, 431);
            this.layoutPatternView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableScanWidth.ResumeLayout(false);
            this.tableScanWidth.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel layoutPatternView;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ProgressBar progressBarZeroing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableScanWidth;
        private System.Windows.Forms.Label labelScanWidth;
        private System.Windows.Forms.ComboBox comboScanWidth;
        private System.Windows.Forms.Button buttonCalibration;
        private System.Windows.Forms.Button buttonSafety;
        private System.Windows.Forms.Label labelLotNo;
        private System.Windows.Forms.TextBox textLotNo;
        private System.Windows.Forms.ComboBox comboCalParam;
        private System.Windows.Forms.Label labelCalParam;
    }
}
