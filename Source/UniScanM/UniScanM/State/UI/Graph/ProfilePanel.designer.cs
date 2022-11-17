﻿namespace UniScanM.UI.Graph
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.txtAvg = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.Label();
            this.txtMax = new System.Windows.Forms.Label();
            this.labelAvg = new System.Windows.Forms.Label();
            this.labelMin2 = new System.Windows.Forms.Label();
            this.labelMax2 = new System.Windows.Forms.Label();
            this.labelPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtStd = new System.Windows.Forms.Label();
            this.labelCur = new System.Windows.Forms.Label();
            this.txtCur = new System.Windows.Forms.Label();
            this.labelDiff = new System.Windows.Forms.Label();
            this.txtDiff = new System.Windows.Forms.Label();
            this.labelStd = new System.Windows.Forms.Label();
            this.labelMM = new System.Windows.Forms.Label();
            this.ultraButtonZoomReset = new Infragistics.Win.Misc.UltraButton();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelPanel.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAvg
            // 
            this.txtAvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAvg.Location = new System.Drawing.Point(482, 1);
            this.txtAvg.Margin = new System.Windows.Forms.Padding(0);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(86, 23);
            this.txtAvg.TabIndex = 22;
            this.txtAvg.Text = "0.000";
            this.txtAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMin
            // 
            this.txtMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMin.Location = new System.Drawing.Point(146, 1);
            this.txtMin.Margin = new System.Windows.Forms.Padding(0);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(83, 23);
            this.txtMin.TabIndex = 24;
            this.txtMin.Text = "0.000";
            this.txtMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMax
            // 
            this.txtMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMax.Location = new System.Drawing.Point(314, 1);
            this.txtMax.Margin = new System.Windows.Forms.Padding(0);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(83, 23);
            this.txtMax.TabIndex = 26;
            this.txtMax.Text = "0.000";
            this.txtMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAvg
            // 
            this.labelAvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAvg.Location = new System.Drawing.Point(398, 1);
            this.labelAvg.Margin = new System.Windows.Forms.Padding(0);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new System.Drawing.Size(83, 23);
            this.labelAvg.TabIndex = 28;
            this.labelAvg.Text = "Avg";
            this.labelAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMin2
            // 
            this.labelMin2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin2.Location = new System.Drawing.Point(62, 1);
            this.labelMin2.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin2.Name = "labelMin2";
            this.labelMin2.Size = new System.Drawing.Size(83, 23);
            this.labelMin2.TabIndex = 32;
            this.labelMin2.Text = "Min";
            this.labelMin2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax2
            // 
            this.labelMax2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax2.Location = new System.Drawing.Point(230, 1);
            this.labelMax2.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax2.Name = "labelMax2";
            this.labelMax2.Size = new System.Drawing.Size(83, 23);
            this.labelMax2.TabIndex = 33;
            this.labelMax2.Text = "Max";
            this.labelMax2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPanel
            // 
            this.labelPanel.BackColor = System.Drawing.Color.AliceBlue;
            this.labelPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.labelPanel.ColumnCount = 7;
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.labelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.labelPanel.Controls.Add(this.txtStd, 4, 1);
            this.labelPanel.Controls.Add(this.labelMin2, 1, 0);
            this.labelPanel.Controls.Add(this.txtAvg, 6, 0);
            this.labelPanel.Controls.Add(this.txtMin, 2, 0);
            this.labelPanel.Controls.Add(this.labelAvg, 5, 0);
            this.labelPanel.Controls.Add(this.txtMax, 4, 0);
            this.labelPanel.Controls.Add(this.labelMax2, 3, 0);
            this.labelPanel.Controls.Add(this.labelCur, 1, 1);
            this.labelPanel.Controls.Add(this.txtCur, 2, 1);
            this.labelPanel.Controls.Add(this.labelDiff, 5, 1);
            this.labelPanel.Controls.Add(this.txtDiff, 6, 1);
            this.labelPanel.Controls.Add(this.labelStd, 3, 1);
            this.labelPanel.Controls.Add(this.labelMM, 0, 0);
            this.labelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanel.Location = new System.Drawing.Point(0, 40);
            this.labelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.labelPanel.Name = "labelPanel";
            this.labelPanel.RowCount = 2;
            this.labelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.labelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.labelPanel.Size = new System.Drawing.Size(569, 50);
            this.labelPanel.TabIndex = 0;
            // 
            // txtStd
            // 
            this.txtStd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtStd.Location = new System.Drawing.Point(314, 25);
            this.txtStd.Margin = new System.Windows.Forms.Padding(0);
            this.txtStd.Name = "txtStd";
            this.txtStd.Size = new System.Drawing.Size(83, 24);
            this.txtStd.TabIndex = 42;
            this.txtStd.Text = "0.000";
            this.txtStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCur
            // 
            this.labelCur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCur.Location = new System.Drawing.Point(62, 25);
            this.labelCur.Margin = new System.Windows.Forms.Padding(0);
            this.labelCur.Name = "labelCur";
            this.labelCur.Size = new System.Drawing.Size(83, 24);
            this.labelCur.TabIndex = 38;
            this.labelCur.Text = "Cur";
            this.labelCur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCur
            // 
            this.txtCur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCur.Location = new System.Drawing.Point(146, 25);
            this.txtCur.Margin = new System.Windows.Forms.Padding(0);
            this.txtCur.Name = "txtCur";
            this.txtCur.Size = new System.Drawing.Size(83, 24);
            this.txtCur.TabIndex = 36;
            this.txtCur.Text = "0.000";
            this.txtCur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDiff
            // 
            this.labelDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDiff.Location = new System.Drawing.Point(398, 25);
            this.labelDiff.Margin = new System.Windows.Forms.Padding(0);
            this.labelDiff.Name = "labelDiff";
            this.labelDiff.Size = new System.Drawing.Size(83, 24);
            this.labelDiff.TabIndex = 39;
            this.labelDiff.Text = "Diff";
            this.labelDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDiff
            // 
            this.txtDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiff.Location = new System.Drawing.Point(482, 25);
            this.txtDiff.Margin = new System.Windows.Forms.Padding(0);
            this.txtDiff.Name = "txtDiff";
            this.txtDiff.Size = new System.Drawing.Size(86, 24);
            this.txtDiff.TabIndex = 35;
            this.txtDiff.Text = "0.000";
            this.txtDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStd
            // 
            this.labelStd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStd.Location = new System.Drawing.Point(230, 25);
            this.labelStd.Margin = new System.Windows.Forms.Padding(0);
            this.labelStd.Name = "labelStd";
            this.labelStd.Size = new System.Drawing.Size(83, 24);
            this.labelStd.TabIndex = 40;
            this.labelStd.Text = "Std";
            this.labelStd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMM
            // 
            this.labelMM.BackColor = System.Drawing.Color.AliceBlue;
            this.labelMM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMM.Location = new System.Drawing.Point(1, 1);
            this.labelMM.Margin = new System.Windows.Forms.Padding(0);
            this.labelMM.Name = "labelMM";
            this.labelPanel.SetRowSpan(this.labelMM, 2);
            this.labelMM.Size = new System.Drawing.Size(60, 48);
            this.labelMM.TabIndex = 31;
            this.labelMM.Text = "[mm]";
            this.labelMM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ultraButtonZoomReset
            // 
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.ultraButtonZoomReset.Appearance = appearance1;
            this.ultraButtonZoomReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraButtonZoomReset.ImageSize = new System.Drawing.Size(20, 20);
            this.ultraButtonZoomReset.Location = new System.Drawing.Point(519, 0);
            this.ultraButtonZoomReset.Margin = new System.Windows.Forms.Padding(0);
            this.ultraButtonZoomReset.Name = "ultraButtonZoomReset";
            this.ultraButtonZoomReset.Size = new System.Drawing.Size(50, 40);
            this.ultraButtonZoomReset.TabIndex = 41;
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.panel1, 0, 0);
            this.layoutMain.Controls.Add(this.labelPanel, 0, 1);
            this.layoutMain.Controls.Add(this.chart, 0, 2);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(569, 339);
            this.layoutMain.TabIndex = 42;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ultraButtonZoomReset);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(569, 40);
            this.panel1.TabIndex = 43;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.AliceBlue;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(569, 40);
            this.labelTitle.TabIndex = 36;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(3, 93);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(563, 243);
            this.chart.TabIndex = 44;
            this.chart.Text = "chart1";
            // 
            // ProfilePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.layoutMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ProfilePanel";
            this.Size = new System.Drawing.Size(569, 339);
            this.labelPanel.ResumeLayout(false);
            this.layoutMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label txtAvg;
        private System.Windows.Forms.Label txtMin;
        private System.Windows.Forms.Label txtMax;
        private System.Windows.Forms.Label labelAvg;
        private System.Windows.Forms.Label labelMin2;
        private System.Windows.Forms.Label labelMax2;
        private System.Windows.Forms.TableLayoutPanel labelPanel;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelMM;
        private System.Windows.Forms.Label txtStd;
        private System.Windows.Forms.Label labelCur;
        private System.Windows.Forms.Label txtCur;
        private System.Windows.Forms.Label labelDiff;
        private System.Windows.Forms.Label txtDiff;
        private System.Windows.Forms.Label labelStd;
        private Infragistics.Win.Misc.UltraButton ultraButtonZoomReset;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
    }
}
