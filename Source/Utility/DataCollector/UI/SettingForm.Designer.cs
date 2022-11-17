using Infragistics.Win.Misc;

namespace DataCollector.UI
{
    partial class SettingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.tableLayoutPanelPathSetting = new System.Windows.Forms.TableLayoutPanel();
            this.txtPinHolePath = new System.Windows.Forms.TextBox();
            this.txtRVMSPath = new System.Windows.Forms.TextBox();
            this.txtColorSensorPath = new System.Windows.Forms.TextBox();
            this.txtEDMSPath = new System.Windows.Forms.TextBox();
            this.labelPinHole = new System.Windows.Forms.Label();
            this.labelRVMS = new System.Windows.Forms.Label();
            this.labelColorSensor = new System.Windows.Forms.Label();
            this.labelEDMS = new System.Windows.Forms.Label();
            this.labelMonitor = new System.Windows.Forms.Label();
            this.txtMonitorPath = new System.Windows.Forms.TextBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonOK = new Infragistics.Win.Misc.UltraButton();
            this.buttonCancel = new Infragistics.Win.Misc.UltraButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelPathSetting.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelPathSetting
            // 
            this.tableLayoutPanelPathSetting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelPathSetting.ColumnCount = 2;
            this.tableLayoutPanelPathSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelPathSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelPathSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelPathSetting.Controls.Add(this.txtPinHolePath, 1, 4);
            this.tableLayoutPanelPathSetting.Controls.Add(this.txtRVMSPath, 1, 3);
            this.tableLayoutPanelPathSetting.Controls.Add(this.txtColorSensorPath, 1, 2);
            this.tableLayoutPanelPathSetting.Controls.Add(this.txtEDMSPath, 1, 1);
            this.tableLayoutPanelPathSetting.Controls.Add(this.labelPinHole, 0, 4);
            this.tableLayoutPanelPathSetting.Controls.Add(this.labelRVMS, 0, 3);
            this.tableLayoutPanelPathSetting.Controls.Add(this.labelColorSensor, 0, 2);
            this.tableLayoutPanelPathSetting.Controls.Add(this.labelEDMS, 0, 1);
            this.tableLayoutPanelPathSetting.Controls.Add(this.labelMonitor, 0, 0);
            this.tableLayoutPanelPathSetting.Controls.Add(this.txtMonitorPath, 1, 0);
            this.tableLayoutPanelPathSetting.Location = new System.Drawing.Point(0, 40);
            this.tableLayoutPanelPathSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelPathSetting.Name = "tableLayoutPanelPathSetting";
            this.tableLayoutPanelPathSetting.RowCount = 5;
            this.tableLayoutPanelPathSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPathSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPathSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPathSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPathSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelPathSetting.Size = new System.Drawing.Size(784, 160);
            this.tableLayoutPanelPathSetting.TabIndex = 0;
            // 
            // txtPinHolePath
            // 
            this.txtPinHolePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPinHolePath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPinHolePath.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPinHolePath.Location = new System.Drawing.Point(103, 131);
            this.txtPinHolePath.Name = "txtPinHolePath";
            this.txtPinHolePath.Size = new System.Drawing.Size(678, 26);
            this.txtPinHolePath.TabIndex = 14;
            // 
            // txtRVMSPath
            // 
            this.txtRVMSPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRVMSPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRVMSPath.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRVMSPath.Location = new System.Drawing.Point(103, 99);
            this.txtRVMSPath.Name = "txtRVMSPath";
            this.txtRVMSPath.Size = new System.Drawing.Size(678, 26);
            this.txtRVMSPath.TabIndex = 13;
            // 
            // txtColorSensorPath
            // 
            this.txtColorSensorPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtColorSensorPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtColorSensorPath.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtColorSensorPath.Location = new System.Drawing.Point(103, 67);
            this.txtColorSensorPath.Name = "txtColorSensorPath";
            this.txtColorSensorPath.Size = new System.Drawing.Size(678, 26);
            this.txtColorSensorPath.TabIndex = 12;
            // 
            // txtEDMSPath
            // 
            this.txtEDMSPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEDMSPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEDMSPath.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtEDMSPath.Location = new System.Drawing.Point(103, 35);
            this.txtEDMSPath.Name = "txtEDMSPath";
            this.txtEDMSPath.Size = new System.Drawing.Size(678, 26);
            this.txtEDMSPath.TabIndex = 11;
            // 
            // labelPinHole
            // 
            this.labelPinHole.AutoSize = true;
            this.labelPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinHole.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPinHole.Location = new System.Drawing.Point(0, 128);
            this.labelPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinHole.Name = "labelPinHole";
            this.labelPinHole.Size = new System.Drawing.Size(100, 32);
            this.labelPinHole.TabIndex = 4;
            this.labelPinHole.Text = "PinHole";
            this.labelPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRVMS
            // 
            this.labelRVMS.AutoSize = true;
            this.labelRVMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRVMS.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRVMS.Location = new System.Drawing.Point(0, 96);
            this.labelRVMS.Margin = new System.Windows.Forms.Padding(0);
            this.labelRVMS.Name = "labelRVMS";
            this.labelRVMS.Size = new System.Drawing.Size(100, 32);
            this.labelRVMS.TabIndex = 3;
            this.labelRVMS.Text = "RVMS";
            this.labelRVMS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelColorSensor
            // 
            this.labelColorSensor.AutoSize = true;
            this.labelColorSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelColorSensor.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelColorSensor.Location = new System.Drawing.Point(0, 64);
            this.labelColorSensor.Margin = new System.Windows.Forms.Padding(0);
            this.labelColorSensor.Name = "labelColorSensor";
            this.labelColorSensor.Size = new System.Drawing.Size(100, 32);
            this.labelColorSensor.TabIndex = 2;
            this.labelColorSensor.Text = "ColorSensor";
            this.labelColorSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEDMS
            // 
            this.labelEDMS.AutoSize = true;
            this.labelEDMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEDMS.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEDMS.Location = new System.Drawing.Point(0, 32);
            this.labelEDMS.Margin = new System.Windows.Forms.Padding(0);
            this.labelEDMS.Name = "labelEDMS";
            this.labelEDMS.Size = new System.Drawing.Size(100, 32);
            this.labelEDMS.TabIndex = 1;
            this.labelEDMS.Text = "EDMS";
            this.labelEDMS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMonitor
            // 
            this.labelMonitor.AutoSize = true;
            this.labelMonitor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMonitor.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMonitor.Location = new System.Drawing.Point(0, 0);
            this.labelMonitor.Margin = new System.Windows.Forms.Padding(0);
            this.labelMonitor.Name = "labelMonitor";
            this.labelMonitor.Size = new System.Drawing.Size(100, 32);
            this.labelMonitor.TabIndex = 0;
            this.labelMonitor.Text = "Monitor";
            this.labelMonitor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMonitorPath
            // 
            this.txtMonitorPath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMonitorPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMonitorPath.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMonitorPath.Location = new System.Drawing.Point(103, 3);
            this.txtMonitorPath.Name = "txtMonitorPath";
            this.txtMonitorPath.Size = new System.Drawing.Size(678, 26);
            this.txtMonitorPath.TabIndex = 10;
            // 
            // labelResult
            // 
            this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResult.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelResult.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelResult.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelResult.Location = new System.Drawing.Point(0, 0);
            this.labelResult.Margin = new System.Windows.Forms.Padding(0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(784, 40);
            this.labelResult.TabIndex = 6;
            this.labelResult.Text = "Path";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonOK
            // 
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonOK.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonOK.Location = new System.Drawing.Point(272, 0);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(0, 0, 20, 0);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 33);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCancel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonCancel.Location = new System.Drawing.Point(412, 0);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 33);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.buttonOK, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 33);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 208);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(784, 43);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 251);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.tableLayoutPanelPathSetting);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingForm";
            this.Text = "SettingForm";
            this.Load += new System.EventHandler(this.SettingForm_Load);
            this.tableLayoutPanelPathSetting.ResumeLayout(false);
            this.tableLayoutPanelPathSetting.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelPathSetting;
        private System.Windows.Forms.Label labelRVMS;
        private System.Windows.Forms.Label labelColorSensor;
        private System.Windows.Forms.Label labelEDMS;
        private System.Windows.Forms.Label labelMonitor;
        private System.Windows.Forms.Label labelPinHole;
        private System.Windows.Forms.TextBox txtMonitorPath;
        private System.Windows.Forms.TextBox txtPinHolePath;
        private System.Windows.Forms.TextBox txtRVMSPath;
        private System.Windows.Forms.TextBox txtColorSensorPath;
        private System.Windows.Forms.TextBox txtEDMSPath;
        private System.Windows.Forms.Label labelResult;
        private UltraButton buttonOK;
        private UltraButton buttonCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}