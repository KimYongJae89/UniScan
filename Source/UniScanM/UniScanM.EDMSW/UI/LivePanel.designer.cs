namespace UniScanM.EDMS.UI
{
    partial class LivePanel
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
            this.txtAvg = new System.Windows.Forms.Label();
            this.txtMin = new System.Windows.Forms.Label();
            this.txtMax = new System.Windows.Forms.Label();
            this.labelAvg = new System.Windows.Forms.Label();
            this.labelMin2 = new System.Windows.Forms.Label();
            this.labelMax2 = new System.Windows.Forms.Label();
            this.labelPanel = new System.Windows.Forms.TableLayoutPanel();
            this.txtVar = new System.Windows.Forms.Label();
            this.labelCur = new System.Windows.Forms.Label();
            this.txtCur = new System.Windows.Forms.Label();
            this.labelDiff = new System.Windows.Forms.Label();
            this.txtMinMax = new System.Windows.Forms.Label();
            this.labelVar = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.lineChart = new LiveCharts.WinForms.CartesianChart();
            this.button1 = new System.Windows.Forms.Button();
            this.labelPanel.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAvg
            // 
            this.txtAvg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAvg.Location = new System.Drawing.Point(472, 1);
            this.txtAvg.Margin = new System.Windows.Forms.Padding(0);
            this.txtAvg.Name = "txtAvg";
            this.txtAvg.Size = new System.Drawing.Size(84, 23);
            this.txtAvg.TabIndex = 22;
            this.txtAvg.Text = "0.000";
            this.txtAvg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMin
            // 
            this.txtMin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMin.Location = new System.Drawing.Point(144, 1);
            this.txtMin.Margin = new System.Windows.Forms.Padding(0);
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(81, 23);
            this.txtMin.TabIndex = 24;
            this.txtMin.Text = "0.000";
            this.txtMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMax
            // 
            this.txtMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMax.Location = new System.Drawing.Point(308, 1);
            this.txtMax.Margin = new System.Windows.Forms.Padding(0);
            this.txtMax.Name = "txtMax";
            this.txtMax.Size = new System.Drawing.Size(81, 23);
            this.txtMax.TabIndex = 26;
            this.txtMax.Text = "0.000";
            this.txtMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAvg
            // 
            this.labelAvg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAvg.Location = new System.Drawing.Point(390, 1);
            this.labelAvg.Margin = new System.Windows.Forms.Padding(0);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new System.Drawing.Size(81, 23);
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
            this.labelMin2.Size = new System.Drawing.Size(81, 23);
            this.labelMin2.TabIndex = 32;
            this.labelMin2.Text = "Min";
            this.labelMin2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax2
            // 
            this.labelMax2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax2.Location = new System.Drawing.Point(226, 1);
            this.labelMax2.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax2.Name = "labelMax2";
            this.labelMax2.Size = new System.Drawing.Size(81, 23);
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
            this.labelPanel.Controls.Add(this.txtVar, 4, 1);
            this.labelPanel.Controls.Add(this.labelMin2, 1, 0);
            this.labelPanel.Controls.Add(this.txtAvg, 6, 0);
            this.labelPanel.Controls.Add(this.txtMin, 2, 0);
            this.labelPanel.Controls.Add(this.labelAvg, 5, 0);
            this.labelPanel.Controls.Add(this.txtMax, 4, 0);
            this.labelPanel.Controls.Add(this.labelMax2, 3, 0);
            this.labelPanel.Controls.Add(this.labelCur, 1, 1);
            this.labelPanel.Controls.Add(this.txtCur, 2, 1);
            this.labelPanel.Controls.Add(this.labelDiff, 5, 1);
            this.labelPanel.Controls.Add(this.txtMinMax, 6, 1);
            this.labelPanel.Controls.Add(this.labelVar, 3, 1);
            this.labelPanel.Controls.Add(this.labelUnit, 0, 0);
            this.labelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPanel.Location = new System.Drawing.Point(0, 40);
            this.labelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.labelPanel.Name = "labelPanel";
            this.labelPanel.RowCount = 2;
            this.labelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.labelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.labelPanel.Size = new System.Drawing.Size(557, 50);
            this.labelPanel.TabIndex = 0;
            // 
            // txtVar
            // 
            this.txtVar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVar.Location = new System.Drawing.Point(308, 25);
            this.txtVar.Margin = new System.Windows.Forms.Padding(0);
            this.txtVar.Name = "txtVar";
            this.txtVar.Size = new System.Drawing.Size(81, 24);
            this.txtVar.TabIndex = 42;
            this.txtVar.Text = "0.000";
            this.txtVar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCur
            // 
            this.labelCur.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCur.Location = new System.Drawing.Point(62, 25);
            this.labelCur.Margin = new System.Windows.Forms.Padding(0);
            this.labelCur.Name = "labelCur";
            this.labelCur.Size = new System.Drawing.Size(81, 24);
            this.labelCur.TabIndex = 38;
            this.labelCur.Text = "Cur";
            this.labelCur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCur
            // 
            this.txtCur.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCur.Location = new System.Drawing.Point(144, 25);
            this.txtCur.Margin = new System.Windows.Forms.Padding(0);
            this.txtCur.Name = "txtCur";
            this.txtCur.Size = new System.Drawing.Size(81, 24);
            this.txtCur.TabIndex = 36;
            this.txtCur.Text = "0.000";
            this.txtCur.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDiff
            // 
            this.labelDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDiff.Location = new System.Drawing.Point(390, 25);
            this.labelDiff.Margin = new System.Windows.Forms.Padding(0);
            this.labelDiff.Name = "labelDiff";
            this.labelDiff.Size = new System.Drawing.Size(81, 24);
            this.labelDiff.TabIndex = 39;
            this.labelDiff.Text = "Diff";
            this.labelDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMinMax
            // 
            this.txtMinMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMinMax.Location = new System.Drawing.Point(472, 25);
            this.txtMinMax.Margin = new System.Windows.Forms.Padding(0);
            this.txtMinMax.Name = "txtMinMax";
            this.txtMinMax.Size = new System.Drawing.Size(84, 24);
            this.txtMinMax.TabIndex = 35;
            this.txtMinMax.Text = "0.000";
            this.txtMinMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelVar
            // 
            this.labelVar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVar.Location = new System.Drawing.Point(226, 25);
            this.labelVar.Margin = new System.Windows.Forms.Padding(0);
            this.labelVar.Name = "labelVar";
            this.labelVar.Size = new System.Drawing.Size(81, 24);
            this.labelVar.TabIndex = 40;
            this.labelVar.Text = "Var";
            this.labelVar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUnit
            // 
            this.labelUnit.BackColor = System.Drawing.Color.AliceBlue;
            this.labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnit.Location = new System.Drawing.Point(1, 1);
            this.labelUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelUnit.Name = "labelUnit";
            this.labelPanel.SetRowSpan(this.labelUnit, 2);
            this.labelUnit.Size = new System.Drawing.Size(60, 48);
            this.labelUnit.TabIndex = 31;
            this.labelUnit.Text = "[mm]";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.panel1, 0, 0);
            this.layoutMain.Controls.Add(this.labelPanel, 0, 1);
            this.layoutMain.Controls.Add(this.lineChart, 0, 2);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(557, 343);
            this.layoutMain.TabIndex = 42;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(557, 40);
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
            this.labelTitle.Size = new System.Drawing.Size(557, 40);
            this.labelTitle.TabIndex = 36;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lineChart
            // 
            this.lineChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineChart.Enabled = false;
            this.lineChart.Location = new System.Drawing.Point(3, 93);
            this.lineChart.Name = "lineChart";
            this.lineChart.Size = new System.Drawing.Size(551, 247);
            this.lineChart.TabIndex = 44;
            this.lineChart.Text = "cartesianChart1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(430, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LivePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.layoutMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LivePanel";
            this.Size = new System.Drawing.Size(557, 343);
            this.labelPanel.ResumeLayout(false);
            this.layoutMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label txtVar;
        private System.Windows.Forms.Label labelCur;
        private System.Windows.Forms.Label txtCur;
        private System.Windows.Forms.Label labelDiff;
        private System.Windows.Forms.Label txtMinMax;
        private System.Windows.Forms.Label labelVar;
        private System.Windows.Forms.Panel panel1;
        private LiveCharts.WinForms.CartesianChart lineChart;
        private System.Windows.Forms.Button button1;
    }
}
