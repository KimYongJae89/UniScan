using System;

namespace UniEye.Base.UI
{
    partial class LivePage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LivePage));
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
            this.cameraViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.viewContainer = new System.Windows.Forms.Panel();
            this.measureMode = new System.Windows.Forms.CheckBox();
            this.clearMeasure = new System.Windows.Forms.Button();
            this.zoomIn = new System.Windows.Forms.Button();
            this.zoomOut = new System.Windows.Forms.Button();
            this.zoomStep = new System.Windows.Forms.NumericUpDown();
            this.labelStep = new System.Windows.Forms.Label();
            this.txtExposure = new System.Windows.Forms.TextBox();
            this.labelMs = new System.Windows.Forms.Label();
            this.labelExposure = new System.Windows.Forms.Label();
            this.buttonZoomFit = new System.Windows.Forms.Button();
            this.viewContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoomStep)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(90)))), ((int)(((byte)(50)))));
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStart.Appearance = appearance1;
            this.buttonStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless;
            this.buttonStart.ImageSize = new System.Drawing.Size(70, 90);
            this.buttonStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonStart.Location = new System.Drawing.Point(3, 4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(119, 124);
            this.buttonStart.TabIndex = 11;
            this.buttonStart.Tag = "Start";
            this.buttonStart.UseAppStyling = false;
            this.buttonStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cameraViewPanel
            // 
            this.cameraViewPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cameraViewPanel.ColumnCount = 2;
            this.cameraViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraViewPanel.Location = new System.Drawing.Point(0, 0);
            this.cameraViewPanel.Name = "cameraViewPanel";
            this.cameraViewPanel.RowCount = 2;
            this.cameraViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.cameraViewPanel.Size = new System.Drawing.Size(581, 514);
            this.cameraViewPanel.TabIndex = 17;
            // 
            // viewContainer
            // 
            this.viewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewContainer.Controls.Add(this.cameraViewPanel);
            this.viewContainer.Location = new System.Drawing.Point(127, 3);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(581, 514);
            this.viewContainer.TabIndex = 20;
            // 
            // measureMode
            // 
            this.measureMode.AutoSize = true;
            this.measureMode.Location = new System.Drawing.Point(3, 134);
            this.measureMode.Name = "measureMode";
            this.measureMode.Size = new System.Drawing.Size(110, 16);
            this.measureMode.TabIndex = 21;
            this.measureMode.Text = "Measure Mode";
            this.measureMode.UseVisualStyleBackColor = true;
            this.measureMode.CheckedChanged += new System.EventHandler(this.measureMode_CheckedChanged);
            // 
            // clearMeasure
            // 
            this.clearMeasure.Location = new System.Drawing.Point(3, 156);
            this.clearMeasure.Name = "clearMeasure";
            this.clearMeasure.Size = new System.Drawing.Size(119, 31);
            this.clearMeasure.TabIndex = 22;
            this.clearMeasure.Text = "Clear Measure";
            this.clearMeasure.UseVisualStyleBackColor = true;
            this.clearMeasure.Click += new System.EventHandler(this.clearMeasure_Click);
            // 
            // zoomIn
            // 
            this.zoomIn.Location = new System.Drawing.Point(3, 193);
            this.zoomIn.Name = "zoomIn";
            this.zoomIn.Size = new System.Drawing.Size(119, 31);
            this.zoomIn.TabIndex = 22;
            this.zoomIn.Text = "Zoom In";
            this.zoomIn.UseVisualStyleBackColor = true;
            this.zoomIn.Click += new System.EventHandler(this.zoomIn_Click);
            // 
            // zoomOut
            // 
            this.zoomOut.Location = new System.Drawing.Point(3, 229);
            this.zoomOut.Name = "zoomOut";
            this.zoomOut.Size = new System.Drawing.Size(119, 31);
            this.zoomOut.TabIndex = 22;
            this.zoomOut.Text = "Zoom Out";
            this.zoomOut.UseVisualStyleBackColor = true;
            this.zoomOut.Click += new System.EventHandler(this.zoomOut_Click);
            // 
            // zoomStep
            // 
            this.zoomStep.Location = new System.Drawing.Point(63, 311);
            this.zoomStep.Name = "zoomStep";
            this.zoomStep.Size = new System.Drawing.Size(58, 21);
            this.zoomStep.TabIndex = 23;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(3, 316);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(30, 12);
            this.labelStep.TabIndex = 24;
            this.labelStep.Text = "Step";
            // 
            // txtExposure
            // 
            this.txtExposure.Location = new System.Drawing.Point(5, 356);
            this.txtExposure.Name = "txtExposure";
            this.txtExposure.Size = new System.Drawing.Size(86, 21);
            this.txtExposure.TabIndex = 25;
            this.txtExposure.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtExposure_KeyDown);
            // 
            // labelMs
            // 
            this.labelMs.AutoSize = true;
            this.labelMs.Font = new System.Drawing.Font("굴림", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMs.Location = new System.Drawing.Point(97, 366);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(21, 11);
            this.labelMs.TabIndex = 26;
            this.labelMs.Text = "ms";
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.Location = new System.Drawing.Point(3, 341);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(92, 12);
            this.labelExposure.TabIndex = 27;
            this.labelExposure.Text = "Exposure Time";
            // 
            // buttonZoomFit
            // 
            this.buttonZoomFit.Location = new System.Drawing.Point(3, 266);
            this.buttonZoomFit.Name = "buttonZoomFit";
            this.buttonZoomFit.Size = new System.Drawing.Size(119, 31);
            this.buttonZoomFit.TabIndex = 28;
            this.buttonZoomFit.Text = "Zoom Fit";
            this.buttonZoomFit.UseVisualStyleBackColor = true;
            this.buttonZoomFit.Click += new System.EventHandler(this.buttonZoomFit_Click);
            // 
            // LivePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonZoomFit);
            this.Controls.Add(this.labelExposure);
            this.Controls.Add(this.labelMs);
            this.Controls.Add(this.txtExposure);
            this.Controls.Add(this.labelStep);
            this.Controls.Add(this.zoomStep);
            this.Controls.Add(this.zoomOut);
            this.Controls.Add(this.zoomIn);
            this.Controls.Add(this.clearMeasure);
            this.Controls.Add(this.measureMode);
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.buttonStart);
            this.Name = "LivePage";
            this.Size = new System.Drawing.Size(712, 521);
            this.VisibleChanged += new System.EventHandler(this.LivePage_VisibleChanged);
            this.viewContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.zoomStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.Misc.UltraButton buttonStart;
        private System.Windows.Forms.TableLayoutPanel cameraViewPanel;
        private System.Windows.Forms.Panel viewContainer;
        private System.Windows.Forms.CheckBox measureMode;
        private System.Windows.Forms.Button clearMeasure;
        private System.Windows.Forms.Button zoomIn;
        private System.Windows.Forms.Button zoomOut;
        private System.Windows.Forms.NumericUpDown zoomStep;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.TextBox txtExposure;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.Button buttonZoomFit;
    }
}
