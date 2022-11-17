namespace UniScanX.MPAlignment.Algo.UI
{
    partial class MPParamControl
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
            this.label_MPAlgorithm = new System.Windows.Forms.Label();
            this.nudThreshod_Y1st = new System.Windows.Forms.NumericUpDown();
            this.nudThreshod_X1st = new System.Windows.Forms.NumericUpDown();
            this.pnlBottomMenu = new System.Windows.Forms.Panel();
            this.checkPreview = new System.Windows.Forms.CheckBox();
            this.buttonZoomFit = new System.Windows.Forms.Button();
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.panel_Profile_Vertical = new System.Windows.Forms.Panel();
            this.panel_Profile_Horizontal = new System.Windows.Forms.Panel();
            this.pnlProbeImage = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudThreshod_Y2nd = new System.Windows.Forms.NumericUpDown();
            this.nudThreshod_X2nd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox_Force = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y1st)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X1st)).BeginInit();
            this.pnlBottomMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y2nd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X2nd)).BeginInit();
            this.SuspendLayout();
            // 
            // label_MPAlgorithm
            // 
            this.label_MPAlgorithm.AutoSize = true;
            this.label_MPAlgorithm.Font = new System.Drawing.Font("맑은 고딕", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_MPAlgorithm.Location = new System.Drawing.Point(1049, 6);
            this.label_MPAlgorithm.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label_MPAlgorithm.Name = "label_MPAlgorithm";
            this.label_MPAlgorithm.Size = new System.Drawing.Size(262, 50);
            this.label_MPAlgorithm.TabIndex = 0;
            this.label_MPAlgorithm.Text = "MP Alignment";
            // 
            // nudThreshod_Y1st
            // 
            this.nudThreshod_Y1st.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_Y1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_Y1st.DecimalPlaces = 1;
            this.nudThreshod_Y1st.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_Y1st.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_Y1st.Location = new System.Drawing.Point(1269, 259);
            this.nudThreshod_Y1st.Margin = new System.Windows.Forms.Padding(14, 18, 14, 18);
            this.nudThreshod_Y1st.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_Y1st.Name = "nudThreshod_Y1st";
            this.nudThreshod_Y1st.Size = new System.Drawing.Size(146, 50);
            this.nudThreshod_Y1st.TabIndex = 2;
            this.nudThreshod_Y1st.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_Y1st.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudThreshod_Y1st.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // nudThreshod_X1st
            // 
            this.nudThreshod_X1st.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_X1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_X1st.DecimalPlaces = 1;
            this.nudThreshod_X1st.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_X1st.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_X1st.Location = new System.Drawing.Point(1269, 193);
            this.nudThreshod_X1st.Margin = new System.Windows.Forms.Padding(14, 18, 14, 18);
            this.nudThreshod_X1st.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_X1st.Name = "nudThreshod_X1st";
            this.nudThreshod_X1st.Size = new System.Drawing.Size(146, 50);
            this.nudThreshod_X1st.TabIndex = 1;
            this.nudThreshod_X1st.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_X1st.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudThreshod_X1st.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // pnlBottomMenu
            // 
            this.pnlBottomMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBottomMenu.Controls.Add(this.checkPreview);
            this.pnlBottomMenu.Controls.Add(this.buttonZoomFit);
            this.pnlBottomMenu.Controls.Add(this.buttonZoomIn);
            this.pnlBottomMenu.Controls.Add(this.buttonZoomOut);
            this.pnlBottomMenu.Location = new System.Drawing.Point(850, 743);
            this.pnlBottomMenu.Margin = new System.Windows.Forms.Padding(6);
            this.pnlBottomMenu.Name = "pnlBottomMenu";
            this.pnlBottomMenu.Size = new System.Drawing.Size(515, 92);
            this.pnlBottomMenu.TabIndex = 153;
            // 
            // checkPreview
            // 
            this.checkPreview.AutoSize = true;
            this.checkPreview.Checked = true;
            this.checkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPreview.Location = new System.Drawing.Point(363, 23);
            this.checkPreview.Margin = new System.Windows.Forms.Padding(6);
            this.checkPreview.Name = "checkPreview";
            this.checkPreview.Size = new System.Drawing.Size(163, 49);
            this.checkPreview.TabIndex = 152;
            this.checkPreview.Text = "Preview";
            this.checkPreview.UseVisualStyleBackColor = true;
            // 
            // buttonZoomFit
            // 
            this.buttonZoomFit.FlatAppearance.BorderSize = 0;
            this.buttonZoomFit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomFit.Image = global::UniScanX.MPAlignment.Properties.Resources.zoomfit_48;
            this.buttonZoomFit.Location = new System.Drawing.Point(224, -2);
            this.buttonZoomFit.Margin = new System.Windows.Forms.Padding(6);
            this.buttonZoomFit.Name = "buttonZoomFit";
            this.buttonZoomFit.Size = new System.Drawing.Size(100, 100);
            this.buttonZoomFit.TabIndex = 151;
            this.buttonZoomFit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomFit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomFit.UseVisualStyleBackColor = true;
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.FlatAppearance.BorderSize = 0;
            this.buttonZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomIn.Image = global::UniScanX.MPAlignment.Properties.Resources.zoom_48;
            this.buttonZoomIn.Location = new System.Drawing.Point(4, -2);
            this.buttonZoomIn.Margin = new System.Windows.Forms.Padding(6);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(96, 96);
            this.buttonZoomIn.TabIndex = 151;
            this.buttonZoomIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.FlatAppearance.BorderSize = 0;
            this.buttonZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonZoomOut.Image = global::UniScanX.MPAlignment.Properties.Resources.zoomout_48;
            this.buttonZoomOut.Location = new System.Drawing.Point(112, -2);
            this.buttonZoomOut.Margin = new System.Windows.Forms.Padding(6);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(100, 100);
            this.buttonZoomOut.TabIndex = 151;
            this.buttonZoomOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonZoomOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            // 
            // panel_Profile_Vertical
            // 
            this.panel_Profile_Vertical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Profile_Vertical.Location = new System.Drawing.Point(800, 6);
            this.panel_Profile_Vertical.Margin = new System.Windows.Forms.Padding(6);
            this.panel_Profile_Vertical.Name = "panel_Profile_Vertical";
            this.panel_Profile_Vertical.Size = new System.Drawing.Size(213, 671);
            this.panel_Profile_Vertical.TabIndex = 155;
            // 
            // panel_Profile_Horizontal
            // 
            this.panel_Profile_Horizontal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_Profile_Horizontal.Location = new System.Drawing.Point(6, 689);
            this.panel_Profile_Horizontal.Margin = new System.Windows.Forms.Padding(6);
            this.panel_Profile_Horizontal.Name = "panel_Profile_Horizontal";
            this.panel_Profile_Horizontal.Size = new System.Drawing.Size(782, 186);
            this.panel_Profile_Horizontal.TabIndex = 156;
            // 
            // pnlProbeImage
            // 
            this.pnlProbeImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProbeImage.Location = new System.Drawing.Point(6, 6);
            this.pnlProbeImage.Margin = new System.Windows.Forms.Padding(6);
            this.pnlProbeImage.Name = "pnlProbeImage";
            this.pnlProbeImage.Size = new System.Drawing.Size(782, 671);
            this.pnlProbeImage.TabIndex = 157;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1099, 144);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Line";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1050, 437);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(196, 45);
            this.label2.TabIndex = 0;
            this.label2.Text = "Second Line";
            // 
            // nudThreshod_Y2nd
            // 
            this.nudThreshod_Y2nd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_Y2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_Y2nd.DecimalPlaces = 1;
            this.nudThreshod_Y2nd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_Y2nd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_Y2nd.Location = new System.Drawing.Point(1269, 549);
            this.nudThreshod_Y2nd.Margin = new System.Windows.Forms.Padding(14, 18, 14, 18);
            this.nudThreshod_Y2nd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_Y2nd.Name = "nudThreshod_Y2nd";
            this.nudThreshod_Y2nd.Size = new System.Drawing.Size(146, 50);
            this.nudThreshod_Y2nd.TabIndex = 4;
            this.nudThreshod_Y2nd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_Y2nd.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudThreshod_Y2nd.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // nudThreshod_X2nd
            // 
            this.nudThreshod_X2nd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_X2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_X2nd.DecimalPlaces = 1;
            this.nudThreshod_X2nd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_X2nd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_X2nd.Location = new System.Drawing.Point(1269, 483);
            this.nudThreshod_X2nd.Margin = new System.Windows.Forms.Padding(14, 18, 14, 18);
            this.nudThreshod_X2nd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_X2nd.Name = "nudThreshod_X2nd";
            this.nudThreshod_X2nd.Size = new System.Drawing.Size(146, 50);
            this.nudThreshod_X2nd.TabIndex = 3;
            this.nudThreshod_X2nd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_X2nd.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudThreshod_X2nd.ValueChanged += new System.EventHandler(this.NumericUpDown_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(1109, 491);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 37);
            this.label3.TabIndex = 0;
            this.label3.Text = "horizontal";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(1141, 557);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 37);
            this.label4.TabIndex = 0;
            this.label4.Text = "Vertical";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(1141, 267);
            this.label9.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 37);
            this.label9.TabIndex = 160;
            this.label9.Text = "Vertical";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(1109, 201);
            this.label10.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(140, 37);
            this.label10.TabIndex = 161;
            this.label10.Text = "horizontal";
            // 
            // checkBox_Force
            // 
            this.checkBox_Force.AutoSize = true;
            this.checkBox_Force.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_Force.Location = new System.Drawing.Point(1101, 370);
            this.checkBox_Force.Name = "checkBox_Force";
            this.checkBox_Force.Size = new System.Drawing.Size(197, 49);
            this.checkBox_Force.TabIndex = 162;
            this.checkBox_Force.Text = "2nd Force";
            this.checkBox_Force.UseVisualStyleBackColor = true;
            // 
            // MPParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.checkBox_Force);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel_Profile_Vertical);
            this.Controls.Add(this.panel_Profile_Horizontal);
            this.Controls.Add(this.pnlProbeImage);
            this.Controls.Add(this.nudThreshod_X2nd);
            this.Controls.Add(this.nudThreshod_Y2nd);
            this.Controls.Add(this.nudThreshod_X1st);
            this.Controls.Add(this.nudThreshod_Y1st);
            this.Controls.Add(this.pnlBottomMenu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_MPAlgorithm);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "MPParamControl";
            this.Size = new System.Drawing.Size(1513, 881);
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y1st)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X1st)).EndInit();
            this.pnlBottomMenu.ResumeLayout(false);
            this.pnlBottomMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y2nd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X2nd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_MPAlgorithm;
        private System.Windows.Forms.NumericUpDown nudThreshod_Y1st;
        private System.Windows.Forms.NumericUpDown nudThreshod_X1st;
        private System.Windows.Forms.Panel pnlBottomMenu;
        private System.Windows.Forms.CheckBox checkPreview;
        private System.Windows.Forms.Button buttonZoomFit;
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Panel panel_Profile_Vertical;
        private System.Windows.Forms.Panel panel_Profile_Horizontal;
        private System.Windows.Forms.Panel pnlProbeImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudThreshod_Y2nd;
        private System.Windows.Forms.NumericUpDown nudThreshod_X2nd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox_Force;
    }
}