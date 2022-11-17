namespace UniScanM.CGInspector.UI
{
    partial class TeachPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeachPage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonGrab = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonAddGlass = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonInspection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButtonGrab,
            this.toolStripButtonLoad,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.toolStripButtonAddGlass,
            this.toolStripButtonAddPrint,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.toolStripButtonInspection,
            this.toolStripSeparator3,
            this.toolStripLabel4,
            this.toolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(102, 554);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(99, 15);
            this.toolStripLabel1.Text = "Getting Image";
            // 
            // toolStripButtonGrab
            // 
            this.toolStripButtonGrab.Image = global::UniScanM.CGInspector.Properties.Resources.process_shot_32;
            this.toolStripButtonGrab.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrab.Name = "toolStripButtonGrab";
            this.toolStripButtonGrab.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonGrab.Text = "Grab";
            this.toolStripButtonGrab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButtonGrab.Click += new System.EventHandler(this.toolStripButtonGrab_Click);
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = global::UniScanM.CGInspector.Properties.Resources.picture_folder_32;
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonLoad.Text = "Load";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(99, 6);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(99, 15);
            this.toolStripLabel2.Text = "Teaching";
            // 
            // toolStripButtonAddGlass
            // 
            this.toolStripButtonAddGlass.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddGlass.Image")));
            this.toolStripButtonAddGlass.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonAddGlass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddGlass.Name = "toolStripButtonAddGlass";
            this.toolStripButtonAddGlass.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonAddGlass.Text = "Glass";
            this.toolStripButtonAddGlass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButtonAddGlass.Click += new System.EventHandler(this.toolStripButtonAddGlass_Click);
            // 
            // toolStripButtonAddPrint
            // 
            this.toolStripButtonAddPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPrint.Image")));
            this.toolStripButtonAddPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonAddPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPrint.Name = "toolStripButtonAddPrint";
            this.toolStripButtonAddPrint.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonAddPrint.Text = "Printing";
            this.toolStripButtonAddPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButtonAddPrint.Click += new System.EventHandler(this.toolStripButtonAddPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(99, 6);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(99, 15);
            this.toolStripLabel3.Text = "Test";
            // 
            // toolStripButtonInspection
            // 
            this.toolStripButtonInspection.Image = global::UniScanM.CGInspector.Properties.Resources.test_32;
            this.toolStripButtonInspection.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButtonInspection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInspection.Name = "toolStripButtonInspection";
            this.toolStripButtonInspection.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonInspection.Text = "Inspect";
            this.toolStripButtonInspection.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripButtonInspection.Click += new System.EventHandler(this.toolStripButtonInspection_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(99, 6);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(99, 15);
            this.toolStripLabel4.Text = "Save";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::UniScanM.CGInspector.Properties.Resources.save_32;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(99, 52);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(102, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.2852F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.7148F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1017, 554);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(705, 548);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(714, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 400);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(714, 409);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 142);
            this.panel3.TabIndex = 2;
            // 
            // TeachPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "TeachPage";
            this.Size = new System.Drawing.Size(1119, 554);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrab;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddGlass;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonInspection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}
