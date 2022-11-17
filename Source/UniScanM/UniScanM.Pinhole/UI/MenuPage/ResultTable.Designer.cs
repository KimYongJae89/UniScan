namespace UniScanM.Pinhole.UI.MenuPage
{
    partial class ResultTable
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sectionCam2 = new System.Windows.Forms.Label();
            this.sectionCam1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelCam1 = new System.Windows.Forms.Label();
            this.labelCam2 = new System.Windows.Forms.Label();
            this.dustSum = new System.Windows.Forms.Label();
            this.dustCam1 = new System.Windows.Forms.Label();
            this.dustCam2 = new System.Windows.Forms.Label();
            this.pinholeCam2 = new System.Windows.Forms.Label();
            this.pinholeCam1 = new System.Windows.Forms.Label();
            this.pinholeSum = new System.Windows.Forms.Label();
            this.totalCam2 = new System.Windows.Forms.Label();
            this.totalCam1 = new System.Windows.Forms.Label();
            this.totalSum = new System.Windows.Forms.Label();
            this.labelSum = new System.Windows.Forms.Label();
            this.labelPinhole = new System.Windows.Forms.Label();
            this.labelDust = new System.Windows.Forms.Label();
            this.labelSection = new System.Windows.Forms.Label();
            this.labelInspect = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.labelInspect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(539, 218);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.sectionCam2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.sectionCam1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelTotal, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCam1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCam2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dustSum, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.dustCam1, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.dustCam2, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.pinholeCam2, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.pinholeCam1, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.pinholeSum, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.totalCam2, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.totalCam1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.totalSum, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelSum, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelPinhole, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDust, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelSection, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Gulim", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 169);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // sectionCam2
            // 
            this.sectionCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionCam2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionCam2.Location = new System.Drawing.Point(110, 126);
            this.sectionCam2.Name = "sectionCam2";
            this.sectionCam2.Size = new System.Drawing.Size(101, 43);
            this.sectionCam2.TabIndex = 19;
            this.sectionCam2.Text = "0";
            this.sectionCam2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sectionCam1
            // 
            this.sectionCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionCam1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sectionCam1.Location = new System.Drawing.Point(110, 84);
            this.sectionCam1.Name = "sectionCam1";
            this.sectionCam1.Size = new System.Drawing.Size(101, 42);
            this.sectionCam1.TabIndex = 18;
            this.sectionCam1.Text = "0";
            this.sectionCam1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.SkyBlue;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Gulim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 42);
            this.label7.TabIndex = 6;
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.BackColor = System.Drawing.Color.SkyBlue;
            this.labelTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotal.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTotal.Location = new System.Drawing.Point(0, 42);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(107, 42);
            this.labelTotal.TabIndex = 3;
            this.labelTotal.Text = "Total";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCam1
            // 
            this.labelCam1.AutoSize = true;
            this.labelCam1.BackColor = System.Drawing.Color.SkyBlue;
            this.labelCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam1.Font = new System.Drawing.Font("Malgun Gothic", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCam1.Location = new System.Drawing.Point(0, 84);
            this.labelCam1.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam1.Name = "labelCam1";
            this.labelCam1.Size = new System.Drawing.Size(107, 42);
            this.labelCam1.TabIndex = 4;
            this.labelCam1.Text = "Cam 1";
            this.labelCam1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCam2
            // 
            this.labelCam2.AutoSize = true;
            this.labelCam2.BackColor = System.Drawing.Color.SkyBlue;
            this.labelCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam2.Font = new System.Drawing.Font("Malgun Gothic", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCam2.Location = new System.Drawing.Point(0, 126);
            this.labelCam2.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam2.Name = "labelCam2";
            this.labelCam2.Size = new System.Drawing.Size(107, 43);
            this.labelCam2.TabIndex = 5;
            this.labelCam2.Text = "Cam 2";
            this.labelCam2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dustSum
            // 
            this.dustSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustSum.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dustSum.Location = new System.Drawing.Point(431, 42);
            this.dustSum.Name = "dustSum";
            this.dustSum.Size = new System.Drawing.Size(105, 42);
            this.dustSum.TabIndex = 9;
            this.dustSum.Text = "0";
            this.dustSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dustCam1
            // 
            this.dustCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustCam1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dustCam1.Location = new System.Drawing.Point(431, 84);
            this.dustCam1.Name = "dustCam1";
            this.dustCam1.Size = new System.Drawing.Size(105, 42);
            this.dustCam1.TabIndex = 12;
            this.dustCam1.Text = "0";
            this.dustCam1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dustCam2
            // 
            this.dustCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustCam2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dustCam2.Location = new System.Drawing.Point(431, 126);
            this.dustCam2.Name = "dustCam2";
            this.dustCam2.Size = new System.Drawing.Size(105, 43);
            this.dustCam2.TabIndex = 15;
            this.dustCam2.Text = "0";
            this.dustCam2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pinholeCam2
            // 
            this.pinholeCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinholeCam2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pinholeCam2.Location = new System.Drawing.Point(324, 126);
            this.pinholeCam2.Name = "pinholeCam2";
            this.pinholeCam2.Size = new System.Drawing.Size(101, 43);
            this.pinholeCam2.TabIndex = 14;
            this.pinholeCam2.Text = "0";
            this.pinholeCam2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pinholeCam1
            // 
            this.pinholeCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinholeCam1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pinholeCam1.Location = new System.Drawing.Point(324, 84);
            this.pinholeCam1.Name = "pinholeCam1";
            this.pinholeCam1.Size = new System.Drawing.Size(101, 42);
            this.pinholeCam1.TabIndex = 11;
            this.pinholeCam1.Text = "0";
            this.pinholeCam1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pinholeSum
            // 
            this.pinholeSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinholeSum.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pinholeSum.Location = new System.Drawing.Point(324, 42);
            this.pinholeSum.Name = "pinholeSum";
            this.pinholeSum.Size = new System.Drawing.Size(101, 42);
            this.pinholeSum.TabIndex = 8;
            this.pinholeSum.Text = "0";
            this.pinholeSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalCam2
            // 
            this.totalCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalCam2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalCam2.Location = new System.Drawing.Point(217, 126);
            this.totalCam2.Name = "totalCam2";
            this.totalCam2.Size = new System.Drawing.Size(101, 43);
            this.totalCam2.TabIndex = 13;
            this.totalCam2.Text = "0";
            this.totalCam2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalCam1
            // 
            this.totalCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalCam1.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalCam1.Location = new System.Drawing.Point(217, 84);
            this.totalCam1.Name = "totalCam1";
            this.totalCam1.Size = new System.Drawing.Size(101, 42);
            this.totalCam1.TabIndex = 10;
            this.totalCam1.Text = "0";
            this.totalCam1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalSum
            // 
            this.totalSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalSum.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalSum.Location = new System.Drawing.Point(217, 42);
            this.totalSum.Name = "totalSum";
            this.totalSum.Size = new System.Drawing.Size(101, 42);
            this.totalSum.TabIndex = 7;
            this.totalSum.Text = "0";
            this.totalSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSum
            // 
            this.labelSum.AutoSize = true;
            this.labelSum.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSum.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSum.Location = new System.Drawing.Point(214, 0);
            this.labelSum.Margin = new System.Windows.Forms.Padding(0);
            this.labelSum.Name = "labelSum";
            this.labelSum.Size = new System.Drawing.Size(107, 42);
            this.labelSum.TabIndex = 0;
            this.labelSum.Text = "Sum";
            this.labelSum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPinhole
            // 
            this.labelPinhole.AutoSize = true;
            this.labelPinhole.BackColor = System.Drawing.Color.SkyBlue;
            this.labelPinhole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinhole.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPinhole.Location = new System.Drawing.Point(321, 0);
            this.labelPinhole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinhole.Name = "labelPinhole";
            this.labelPinhole.Size = new System.Drawing.Size(107, 42);
            this.labelPinhole.TabIndex = 1;
            this.labelPinhole.Text = "Pinhole";
            this.labelPinhole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDust
            // 
            this.labelDust.AutoSize = true;
            this.labelDust.BackColor = System.Drawing.Color.SkyBlue;
            this.labelDust.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDust.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelDust.Location = new System.Drawing.Point(428, 0);
            this.labelDust.Margin = new System.Windows.Forms.Padding(0);
            this.labelDust.Name = "labelDust";
            this.labelDust.Size = new System.Drawing.Size(111, 42);
            this.labelDust.TabIndex = 2;
            this.labelDust.Text = "Dust";
            this.labelDust.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSection
            // 
            this.labelSection.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSection.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelSection.Location = new System.Drawing.Point(107, 0);
            this.labelSection.Margin = new System.Windows.Forms.Padding(0);
            this.labelSection.Name = "labelSection";
            this.labelSection.Size = new System.Drawing.Size(107, 42);
            this.labelSection.TabIndex = 16;
            this.labelSection.Text = "Section";
            this.labelSection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect
            // 
            this.labelInspect.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.labelInspect.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelInspect.Font = new System.Drawing.Font("Malgun Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInspect.Location = new System.Drawing.Point(0, 0);
            this.labelInspect.Name = "labelInspect";
            this.labelInspect.Size = new System.Drawing.Size(539, 49);
            this.labelInspect.TabIndex = 4;
            this.labelInspect.Text = "Total Defect Count";
            this.labelInspect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResultTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "ResultTable";
            this.Size = new System.Drawing.Size(539, 218);
            this.SizeChanged += new System.EventHandler(this.ResultTable_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelInspect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label pinholeCam2;
        private System.Windows.Forms.Label totalCam2;
        private System.Windows.Forms.Label pinholeCam1;
        private System.Windows.Forms.Label totalCam1;
        private System.Windows.Forms.Label pinholeSum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSum;
        private System.Windows.Forms.Label labelPinhole;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label labelCam1;
        private System.Windows.Forms.Label labelCam2;
        private System.Windows.Forms.Label totalSum;
        private System.Windows.Forms.Label sectionCam2;
        private System.Windows.Forms.Label sectionCam1;
        private System.Windows.Forms.Label labelSection;
        private System.Windows.Forms.Label dustSum;
        private System.Windows.Forms.Label dustCam1;
        private System.Windows.Forms.Label dustCam2;
        private System.Windows.Forms.Label labelDust;
    }
}
