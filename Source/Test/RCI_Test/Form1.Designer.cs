namespace WCI_Test
{
    partial class Form1
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupMaster = new System.Windows.Forms.GroupBox();
            this.groupImage = new System.Windows.Forms.GroupBox();
            this.groupParam = new System.Windows.Forms.GroupBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.groupResult = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMasterImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.startInspectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveResultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupParam.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupMaster
            // 
            this.groupMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupMaster.Location = new System.Drawing.Point(3, 3);
            this.groupMaster.Name = "groupMaster";
            this.groupMaster.Size = new System.Drawing.Size(194, 422);
            this.groupMaster.TabIndex = 0;
            this.groupMaster.TabStop = false;
            this.groupMaster.Text = "Master";
            // 
            // groupImage
            // 
            this.groupImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupImage.Location = new System.Drawing.Point(203, 3);
            this.groupImage.Name = "groupImage";
            this.groupImage.Size = new System.Drawing.Size(194, 422);
            this.groupImage.TabIndex = 0;
            this.groupImage.TabStop = false;
            this.groupImage.Text = "Image";
            // 
            // groupParam
            // 
            this.groupParam.Controls.Add(this.propertyGrid1);
            this.groupParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupParam.Location = new System.Drawing.Point(403, 3);
            this.groupParam.Name = "groupParam";
            this.groupParam.Size = new System.Drawing.Size(194, 422);
            this.groupParam.TabIndex = 0;
            this.groupParam.TabStop = false;
            this.groupParam.Text = "Param";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 17);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(188, 402);
            this.propertyGrid1.TabIndex = 0;
            // 
            // groupResult
            // 
            this.groupResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupResult.Location = new System.Drawing.Point(603, 3);
            this.groupResult.Name = "groupResult";
            this.groupResult.Size = new System.Drawing.Size(194, 422);
            this.groupResult.TabIndex = 0;
            this.groupResult.TabStop = false;
            this.groupResult.Text = "Result";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.groupResult, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupMaster, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupImage, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupParam, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 428F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 428);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripSplitButton});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(121, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(451, 17);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripSplitButton
            // 
            this.toolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImageToolStripMenuItem,
            this.loadMasterImageToolStripMenuItem,
            this.clearResultToolStripMenuItem,
            this.toolStripSeparator1,
            this.startInspectToolStripMenuItem,
            this.saveResultToolStripMenuItem});
            this.toolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton.Image")));
            this.toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton.Name = "toolStripSplitButton";
            this.toolStripSplitButton.Size = new System.Drawing.Size(80, 20);
            this.toolStripSplitButton.Text = "Command";
            this.toolStripSplitButton.ButtonClick += new System.EventHandler(this.toolStripMenuItem_ButtonClick);
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadImageToolStripMenuItem.Text = "Load Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
            // 
            // loadMasterImageToolStripMenuItem
            // 
            this.loadMasterImageToolStripMenuItem.Name = "loadMasterImageToolStripMenuItem";
            this.loadMasterImageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.loadMasterImageToolStripMenuItem.Text = "Load Master Image";
            this.loadMasterImageToolStripMenuItem.Click += new System.EventHandler(this.loadMasterImageToolStripMenuItem_Click);
            // 
            // clearResultToolStripMenuItem
            // 
            this.clearResultToolStripMenuItem.Name = "clearResultToolStripMenuItem";
            this.clearResultToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clearResultToolStripMenuItem.Text = "Clear Result";
            this.clearResultToolStripMenuItem.Click += new System.EventHandler(this.clearResultToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // startInspectToolStripMenuItem
            // 
            this.startInspectToolStripMenuItem.Name = "startInspectToolStripMenuItem";
            this.startInspectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startInspectToolStripMenuItem.Text = "Start Inspect";
            this.startInspectToolStripMenuItem.Click += new System.EventHandler(this.startInspectToolStripMenuItem_Click);
            // 
            // saveResultToolStripMenuItem
            // 
            this.saveResultToolStripMenuItem.Name = "saveResultToolStripMenuItem";
            this.saveResultToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveResultToolStripMenuItem.Text = "Save Result";
            this.saveResultToolStripMenuItem.Click += new System.EventHandler(this.saveResultToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupParam.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupMaster;
        private System.Windows.Forms.GroupBox groupImage;
        private System.Windows.Forms.GroupBox groupParam;
        private System.Windows.Forms.GroupBox groupResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem loadMasterImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearResultToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem startInspectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveResultToolStripMenuItem;
    }
}

