namespace UniScan.UI
{
    partial class ProfilePanel
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
            this.profileChart = new Infragistics.Win.DataVisualization.UltraDataChart();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSV = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.txtSv = new System.Windows.Forms.Label();
            this.txtMax = new System.Windows.Forms.Label();
            this.labelAvg = new System.Windows.Forms.Label();
            this.txtAvg = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.txtRange = new System.Windows.Forms.Label();
            this.btnPanelSetting = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChartSetting = new System.Windows.Forms.Button();
            this.labelUpperError = new System.Windows.Forms.Label();
            this.labelLowerError = new System.Windows.Forms.Label();
            this.labelUpperWarning = new System.Windows.Forms.Label();
            this.labelLowerWarning = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.profileChart)).BeginInit();
            this.SuspendLayout();
            // 
            // profileChart
            // 
            this.profileChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profileChart.BackColor = System.Drawing.Color.White;
            this.profileChart.CrosshairPoint = new Infragistics.Win.DataVisualization.Point(double.NaN, double.NaN);
            this.profileChart.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.profileChart.Location = new System.Drawing.Point(155, 33);
            this.profileChart.Name = "profileChart";
            this.profileChart.PlotAreaBackground = new Infragistics.Win.DataVisualization.SolidColorBrush(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0))))));
            this.profileChart.PreviewRect = new Infragistics.Win.DataVisualization.Rectangle(double.PositiveInfinity, double.PositiveInfinity, double.NegativeInfinity, double.NegativeInfinity);
            this.profileChart.Size = new System.Drawing.Size(696, 254);
            this.profileChart.TabIndex = 0;
            this.profileChart.Text = "ultraDataChart1";
            this.profileChart.TitleFontSize = 12D;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.CornflowerBlue;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.Location = new System.Drawing.Point(3, 3);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(118, 34);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSV
            // 
            this.labelSV.BackColor = System.Drawing.Color.Transparent;
            this.labelSV.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSV.Location = new System.Drawing.Point(620, 3);
            this.labelSV.Name = "labelSV";
            this.labelSV.Size = new System.Drawing.Size(46, 27);
            this.labelSV.TabIndex = 3;
            this.labelSV.Text = "SV";
            this.labelSV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMax
            // 
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMax.Location = new System.Drawing.Point(248, 3);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(46, 27);
            this.labelMax.TabIndex = 3;
            this.labelMax.Text = "Max";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSv
            // 
            this.txtSv.BackColor = System.Drawing.Color.Transparent;
            this.txtSv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSv.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtSv.Location = new System.Drawing.Point(667, 3);
            this.txtSv.Name = "txtSv";
            this.txtSv.Size = new System.Drawing.Size(71, 27);
            this.txtSv.TabIndex = 3;
            this.txtSv.Text = "0.000";
            this.txtSv.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMax
            // 
            this.txtMax.BackColor = System.Drawing.Color.Transparent;
            this.txtMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMax.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMax.Location = new System.Drawing.Point(295, 3);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(71, 27);
            this.txtMax.TabIndex = 3;
            this.txtMax.Text = "0.000";
            this.txtMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAvg
            // 
            this.labelAvg.BackColor = System.Drawing.Color.Transparent;
            this.labelAvg.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAvg.Location = new System.Drawing.Point(372, 3);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new System.Drawing.Size(46, 27);
            this.labelAvg.TabIndex = 3;
            this.labelAvg.Text = "Avg";
            this.labelAvg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAvg
            // 
            this.txtAvg.BackColor = System.Drawing.Color.Transparent;
            this.txtAvg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAvg.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtAvg.Location = new System.Drawing.Point(419, 3);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(71, 27);
            this.txtAvg.TabIndex = 3;
            this.txtAvg.Text = "0.000";
            this.txtAvg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMin
            // 
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMin.Location = new System.Drawing.Point(124, 3);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(46, 27);
            this.labelMin.TabIndex = 3;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMin
            // 
            this.txtMin.BackColor = System.Drawing.Color.Transparent;
            this.txtMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMin.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMin.Location = new System.Drawing.Point(171, 3);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(71, 27);
            this.txtMin.TabIndex = 3;
            this.txtMin.Text = "0.000";
            this.txtMin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRange
            // 
            this.labelRange.BackColor = System.Drawing.Color.Transparent;
            this.labelRange.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRange.Location = new System.Drawing.Point(496, 3);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(46, 27);
            this.labelRange.TabIndex = 3;
            this.labelRange.Text = "R";
            this.labelRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRange
            // 
            this.txtRange.BackColor = System.Drawing.Color.Transparent;
            this.txtRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRange.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtRange.Location = new System.Drawing.Point(543, 3);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(71, 27);
            this.txtRange.TabIndex = 3;
            this.txtRange.Text = "0.000";
            this.txtRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnPanelSetting
            // 
            this.btnPanelSetting.Location = new System.Drawing.Point(2, 40);
            this.btnPanelSetting.Name = "btnPanelSetting";
            this.btnPanelSetting.Size = new System.Drawing.Size(119, 32);
            this.btnPanelSetting.TabIndex = 4;
            this.btnPanelSetting.Text = "Panel Setting";
            this.btnPanelSetting.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(802, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 27);
            this.label1.TabIndex = 3;
            this.label1.Text = "(um)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnChartSetting
            // 
            this.btnChartSetting.Location = new System.Drawing.Point(2, 74);
            this.btnChartSetting.Name = "btnChartSetting";
            this.btnChartSetting.Size = new System.Drawing.Size(119, 32);
            this.btnChartSetting.TabIndex = 4;
            this.btnChartSetting.Text = "Chart Setting";
            this.btnChartSetting.UseVisualStyleBackColor = true;
            this.btnChartSetting.Click += new System.EventHandler(this.btnChartSetting_Click);
            // 
            // labelUpperError
            // 
            this.labelUpperError.AutoSize = true;
            this.labelUpperError.BackColor = System.Drawing.Color.Transparent;
            this.labelUpperError.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelUpperError.Location = new System.Drawing.Point(124, 204);
            this.labelUpperError.Name = "labelUpperError";
            this.labelUpperError.Size = new System.Drawing.Size(31, 15);
            this.labelUpperError.TabIndex = 3;
            this.labelUpperError.Text = "0.00";
            this.labelUpperError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLowerError
            // 
            this.labelLowerError.AutoSize = true;
            this.labelLowerError.BackColor = System.Drawing.Color.Transparent;
            this.labelLowerError.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLowerError.Location = new System.Drawing.Point(124, 264);
            this.labelLowerError.Name = "labelLowerError";
            this.labelLowerError.Size = new System.Drawing.Size(31, 15);
            this.labelLowerError.TabIndex = 3;
            this.labelLowerError.Text = "0.00";
            this.labelLowerError.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUpperWarning
            // 
            this.labelUpperWarning.AutoSize = true;
            this.labelUpperWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelUpperWarning.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelUpperWarning.Location = new System.Drawing.Point(124, 224);
            this.labelUpperWarning.Name = "labelUpperWarning";
            this.labelUpperWarning.Size = new System.Drawing.Size(31, 15);
            this.labelUpperWarning.TabIndex = 3;
            this.labelUpperWarning.Text = "0.00";
            this.labelUpperWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLowerWarning
            // 
            this.labelLowerWarning.AutoSize = true;
            this.labelLowerWarning.BackColor = System.Drawing.Color.Transparent;
            this.labelLowerWarning.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLowerWarning.Location = new System.Drawing.Point(124, 244);
            this.labelLowerWarning.Name = "labelLowerWarning";
            this.labelLowerWarning.Size = new System.Drawing.Size(31, 15);
            this.labelLowerWarning.TabIndex = 3;
            this.labelLowerWarning.Text = "0.00";
            this.labelLowerWarning.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ProfilePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnChartSetting);
            this.Controls.Add(this.btnPanelSetting);
            this.Controls.Add(this.txtRange);
            this.Controls.Add(this.txtAvg);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.labelRange);
            this.Controls.Add(this.txtMax);
            this.Controls.Add(this.labelAvg);
            this.Controls.Add(this.labelLowerWarning);
            this.Controls.Add(this.labelUpperWarning);
            this.Controls.Add(this.labelLowerError);
            this.Controls.Add(this.labelUpperError);
            this.Controls.Add(this.labelMin);
            this.Controls.Add(this.txtSv);
            this.Controls.Add(this.labelMax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSV);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.profileChart);
            this.Name = "ProfilePanel";
            this.Size = new System.Drawing.Size(851, 287);
            ((System.ComponentModel.ISupportInitialize)(this.profileChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.DataVisualization.UltraDataChart profileChart;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelSV;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label txtSv;
        private System.Windows.Forms.Label txtMax;
        private System.Windows.Forms.Label labelAvg;
        private System.Windows.Forms.Label txtAvg;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label txtMin;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.Label txtRange;
        private System.Windows.Forms.Button btnPanelSetting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnChartSetting;
        private System.Windows.Forms.Label labelUpperError;
        private System.Windows.Forms.Label labelLowerError;
        private System.Windows.Forms.Label labelUpperWarning;
        private System.Windows.Forms.Label labelLowerWarning;
    }
}
