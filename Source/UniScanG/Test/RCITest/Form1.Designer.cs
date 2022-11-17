namespace RCITest
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonLoadSrc = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonLoadDst = new System.Windows.Forms.Button();
            this.buttonRev1 = new System.Windows.Forms.Button();
            this.buttonResultDebugSave = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAnalyzeSrc = new System.Windows.Forms.Button();
            this.buttonTrainDebugSave = new System.Windows.Forms.Button();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonConsole = new System.Windows.Forms.Button();
            this.buttonBatch = new System.Windows.Forms.Button();
            this.buttonTrainSave = new System.Windows.Forms.Button();
            this.buttonTrainLoad = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.propertyGridDebug = new System.Windows.Forms.PropertyGrid();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLoadSrc
            // 
            this.buttonLoadSrc.AllowDrop = true;
            this.buttonLoadSrc.Location = new System.Drawing.Point(3, 3);
            this.buttonLoadSrc.Name = "buttonLoadSrc";
            this.buttonLoadSrc.Size = new System.Drawing.Size(82, 41);
            this.buttonLoadSrc.TabIndex = 1;
            this.buttonLoadSrc.Text = "Load";
            this.buttonLoadSrc.UseVisualStyleBackColor = true;
            this.buttonLoadSrc.Click += new System.EventHandler(this.buttonLoad_Click);
            this.buttonLoadSrc.DragDrop += new System.Windows.Forms.DragEventHandler(this.buttonLoad_DragDrop);
            this.buttonLoadSrc.DragEnter += new System.Windows.Forms.DragEventHandler(this.buttonLoad_DragEnter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1139, 529);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.buttonLoadDst);
            this.flowLayoutPanel2.Controls.Add(this.buttonRev1);
            this.flowLayoutPanel2.Controls.Add(this.buttonResultDebugSave);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(287, 479);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(278, 47);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // buttonLoadDst
            // 
            this.buttonLoadDst.AllowDrop = true;
            this.buttonLoadDst.Location = new System.Drawing.Point(3, 3);
            this.buttonLoadDst.Name = "buttonLoadDst";
            this.buttonLoadDst.Size = new System.Drawing.Size(82, 41);
            this.buttonLoadDst.TabIndex = 1;
            this.buttonLoadDst.Text = "Load";
            this.buttonLoadDst.UseVisualStyleBackColor = true;
            this.buttonLoadDst.Click += new System.EventHandler(this.buttonLoad_Click);
            this.buttonLoadDst.DragDrop += new System.Windows.Forms.DragEventHandler(this.buttonLoad_DragDrop);
            this.buttonLoadDst.DragEnter += new System.Windows.Forms.DragEventHandler(this.buttonLoad_DragEnter);
            // 
            // buttonRev1
            // 
            this.buttonRev1.Location = new System.Drawing.Point(91, 3);
            this.buttonRev1.Name = "buttonRev1";
            this.buttonRev1.Size = new System.Drawing.Size(82, 41);
            this.buttonRev1.TabIndex = 1;
            this.buttonRev1.Text = "Compare";
            this.buttonRev1.UseVisualStyleBackColor = true;
            this.buttonRev1.Click += new System.EventHandler(this.buttonRev1_Click);
            // 
            // buttonResultDebugSave
            // 
            this.buttonResultDebugSave.Location = new System.Drawing.Point(179, 3);
            this.buttonResultDebugSave.Name = "buttonResultDebugSave";
            this.buttonResultDebugSave.Size = new System.Drawing.Size(82, 41);
            this.buttonResultDebugSave.TabIndex = 1;
            this.buttonResultDebugSave.Text = "Save Debug";
            this.buttonResultDebugSave.UseVisualStyleBackColor = true;
            this.buttonResultDebugSave.Click += new System.EventHandler(this.buttonResultDebugSave_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.buttonLoadSrc);
            this.flowLayoutPanel1.Controls.Add(this.buttonAnalyzeSrc);
            this.flowLayoutPanel1.Controls.Add(this.buttonTrainDebugSave);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 479);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(278, 47);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // buttonAnalyzeSrc
            // 
            this.buttonAnalyzeSrc.Location = new System.Drawing.Point(91, 3);
            this.buttonAnalyzeSrc.Name = "buttonAnalyzeSrc";
            this.buttonAnalyzeSrc.Size = new System.Drawing.Size(82, 41);
            this.buttonAnalyzeSrc.TabIndex = 1;
            this.buttonAnalyzeSrc.Text = "Teach";
            this.buttonAnalyzeSrc.UseVisualStyleBackColor = true;
            this.buttonAnalyzeSrc.Click += new System.EventHandler(this.buttonAnalyze_Click);
            // 
            // buttonTrainDebugSave
            // 
            this.buttonTrainDebugSave.Location = new System.Drawing.Point(179, 3);
            this.buttonTrainDebugSave.Name = "buttonTrainDebugSave";
            this.buttonTrainDebugSave.Size = new System.Drawing.Size(82, 41);
            this.buttonTrainDebugSave.TabIndex = 1;
            this.buttonTrainDebugSave.Text = "Save Debug";
            this.buttonTrainDebugSave.UseVisualStyleBackColor = true;
            this.buttonTrainDebugSave.Click += new System.EventHandler(this.buttonTrainDebugSave_Click);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.buttonConsole);
            this.flowLayoutPanel4.Controls.Add(this.buttonBatch);
            this.flowLayoutPanel4.Controls.Add(this.buttonTrainSave);
            this.flowLayoutPanel4.Controls.Add(this.buttonTrainLoad);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(571, 479);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(565, 47);
            this.flowLayoutPanel4.TabIndex = 4;
            // 
            // buttonConsole
            // 
            this.buttonConsole.Location = new System.Drawing.Point(3, 3);
            this.buttonConsole.Name = "buttonConsole";
            this.buttonConsole.Size = new System.Drawing.Size(82, 41);
            this.buttonConsole.TabIndex = 1;
            this.buttonConsole.Text = "Console";
            this.buttonConsole.UseVisualStyleBackColor = true;
            this.buttonConsole.Click += new System.EventHandler(this.buttonConsole_Click);
            // 
            // buttonBatch
            // 
            this.buttonBatch.Location = new System.Drawing.Point(91, 3);
            this.buttonBatch.Name = "buttonBatch";
            this.buttonBatch.Size = new System.Drawing.Size(82, 41);
            this.buttonBatch.TabIndex = 1;
            this.buttonBatch.Text = "Batch";
            this.buttonBatch.UseVisualStyleBackColor = true;
            this.buttonBatch.Click += new System.EventHandler(this.buttonBatch_Click);
            // 
            // buttonTrainSave
            // 
            this.buttonTrainSave.Location = new System.Drawing.Point(179, 3);
            this.buttonTrainSave.Name = "buttonTrainSave";
            this.buttonTrainSave.Size = new System.Drawing.Size(82, 41);
            this.buttonTrainSave.TabIndex = 1;
            this.buttonTrainSave.Text = "Save";
            this.buttonTrainSave.UseVisualStyleBackColor = true;
            this.buttonTrainSave.Click += new System.EventHandler(this.buttonTrainSave_Click);
            // 
            // buttonTrainLoad
            // 
            this.buttonTrainLoad.Location = new System.Drawing.Point(267, 3);
            this.buttonTrainLoad.Name = "buttonTrainLoad";
            this.buttonTrainLoad.Size = new System.Drawing.Size(82, 41);
            this.buttonTrainLoad.TabIndex = 1;
            this.buttonTrainLoad.Text = "Load";
            this.buttonTrainLoad.UseVisualStyleBackColor = true;
            this.buttonTrainLoad.Click += new System.EventHandler(this.buttonTrainLoad_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.propertyGridDebug, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.listView1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(571, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(565, 470);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // propertyGridDebug
            // 
            this.propertyGridDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridDebug.HelpVisible = false;
            this.propertyGridDebug.Location = new System.Drawing.Point(3, 3);
            this.propertyGridDebug.Name = "propertyGridDebug";
            this.propertyGridDebug.Size = new System.Drawing.Size(559, 229);
            this.propertyGridDebug.TabIndex = 5;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 238);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(559, 229);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripSplitButton1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 529);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1139, 22);
            this.statusStrip1.TabIndex = 3;
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
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(121, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 551);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonLoadSrc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonLoadDst;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button buttonAnalyzeSrc;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button buttonTrainSave;
        private System.Windows.Forms.Button buttonBatch;
        private System.Windows.Forms.Button buttonTrainLoad;
        private System.Windows.Forms.Button buttonRev1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button buttonConsole;
        private System.Windows.Forms.PropertyGrid propertyGridDebug;
        private System.Windows.Forms.Button buttonResultDebugSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button buttonTrainDebugSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

