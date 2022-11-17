namespace DataAnalyzer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonExport = new System.Windows.Forms.Button();
            this.subResultCnt = new System.Windows.Forms.TextBox();
            this.labelSubResultCnt = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPagePosition = new System.Windows.Forms.TabPage();
            this.treeViewPosition = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageSheet = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.labelDefectWidth = new System.Windows.Forms.Label();
            this.labelDefectHeight = new System.Windows.Forms.Label();
            this.defectWidth = new System.Windows.Forms.TextBox();
            this.defectHeight = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPagePosition.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabPageSheet.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.77551F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.22449F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.709678F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.29032F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(911, 620);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 44);
            this.panel1.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(7, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(515, 32);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(525, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonExport);
            this.panel2.Controls.Add(this.subResultCnt);
            this.panel2.Controls.Add(this.labelSubResultCnt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(583, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(325, 48);
            this.panel2.TabIndex = 2;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(240, 7);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(5);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(80, 32);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // subResultCnt
            // 
            this.subResultCnt.Enabled = false;
            this.subResultCnt.Location = new System.Drawing.Point(80, 7);
            this.subResultCnt.Name = "subResultCnt";
            this.subResultCnt.Size = new System.Drawing.Size(68, 32);
            this.subResultCnt.TabIndex = 2;
            // 
            // labelSubResultCnt
            // 
            this.labelSubResultCnt.AutoSize = true;
            this.labelSubResultCnt.Location = new System.Drawing.Point(10, 11);
            this.labelSubResultCnt.Name = "labelSubResultCnt";
            this.labelSubResultCnt.Size = new System.Drawing.Size(64, 25);
            this.labelSubResultCnt.TabIndex = 0;
            this.labelSubResultCnt.Text = "Count";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tabControl1);
            this.panel3.Controls.Add(this.tableLayoutPanel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(583, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(325, 560);
            this.panel3.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPagePosition);
            this.tabControl1.Controls.Add(this.tabPageSheet);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 308);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPagePosition
            // 
            this.tabPagePosition.Controls.Add(this.treeViewPosition);
            this.tabPagePosition.Location = new System.Drawing.Point(4, 34);
            this.tabPagePosition.Name = "tabPagePosition";
            this.tabPagePosition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePosition.Size = new System.Drawing.Size(317, 270);
            this.tabPagePosition.TabIndex = 0;
            this.tabPagePosition.Text = "Position";
            this.tabPagePosition.UseVisualStyleBackColor = true;
            // 
            // treeViewPosition
            // 
            this.treeViewPosition.CheckBoxes = true;
            this.treeViewPosition.ContextMenuStrip = this.contextMenuStrip1;
            this.treeViewPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPosition.Location = new System.Drawing.Point(3, 3);
            this.treeViewPosition.Name = "treeViewPosition";
            this.treeViewPosition.Size = new System.Drawing.Size(311, 264);
            this.treeViewPosition.TabIndex = 0;
            this.treeViewPosition.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeViewPosition.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deselectAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.deselectAllToolStripMenuItem.Text = "Deselect All";
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.deselectAllToolStripMenuItem_Click);
            // 
            // tabPageSheet
            // 
            this.tabPageSheet.Controls.Add(this.listBox1);
            this.tabPageSheet.Location = new System.Drawing.Point(4, 34);
            this.tabPageSheet.Name = "tabPageSheet";
            this.tabPageSheet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSheet.Size = new System.Drawing.Size(317, 270);
            this.tabPageSheet.TabIndex = 1;
            this.tabPageSheet.Text = "Sheet";
            this.tabPageSheet.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDefectWidth, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelDefectHeight, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.defectWidth, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.defectHeight, 3, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 308);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(325, 252);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // panel5
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.panel5, 4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(319, 214);
            this.panel5.TabIndex = 2;
            // 
            // labelDefectWidth
            // 
            this.labelDefectWidth.AutoSize = true;
            this.labelDefectWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectWidth.Location = new System.Drawing.Point(0, 220);
            this.labelDefectWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectWidth.Name = "labelDefectWidth";
            this.labelDefectWidth.Size = new System.Drawing.Size(32, 32);
            this.labelDefectWidth.TabIndex = 3;
            this.labelDefectWidth.Text = "W";
            this.labelDefectWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefectHeight
            // 
            this.labelDefectHeight.AutoSize = true;
            this.labelDefectHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectHeight.Location = new System.Drawing.Point(162, 220);
            this.labelDefectHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectHeight.Name = "labelDefectHeight";
            this.labelDefectHeight.Size = new System.Drawing.Size(32, 32);
            this.labelDefectHeight.TabIndex = 4;
            this.labelDefectHeight.Text = "H";
            this.labelDefectHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectWidth
            // 
            this.defectWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectWidth.Enabled = false;
            this.defectWidth.Location = new System.Drawing.Point(32, 220);
            this.defectWidth.Margin = new System.Windows.Forms.Padding(0);
            this.defectWidth.Name = "defectWidth";
            this.defectWidth.Size = new System.Drawing.Size(130, 32);
            this.defectWidth.TabIndex = 5;
            // 
            // defectHeight
            // 
            this.defectHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectHeight.Enabled = false;
            this.defectHeight.Location = new System.Drawing.Point(194, 220);
            this.defectHeight.Margin = new System.Windows.Forms.Padding(0);
            this.defectHeight.Name = "defectHeight";
            this.defectHeight.Size = new System.Drawing.Size(131, 32);
            this.defectHeight.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 57);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(574, 560);
            this.panel4.TabIndex = 4;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(311, 264);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 620);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "Form1";
            this.Text = "Gravure Result Data Analizer - UniEye";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPagePosition.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabPageSheet.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TreeView treeViewPosition;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox subResultCnt;
        private System.Windows.Forms.Label labelSubResultCnt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelDefectWidth;
        private System.Windows.Forms.Label labelDefectHeight;
        private System.Windows.Forms.TextBox defectWidth;
        private System.Windows.Forms.TextBox defectHeight;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPagePosition;
        private System.Windows.Forms.TabPage tabPageSheet;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

