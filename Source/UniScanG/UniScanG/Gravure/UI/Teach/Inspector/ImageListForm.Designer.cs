namespace UniScanG.Gravure.UI.Teach.Inspector
{
    partial class ImageListForm
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("C:\\");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("D:\\");
            this.listView1 = new System.Windows.Forms.ListView();
            this.buttonRun = new System.Windows.Forms.Button();
            this.useAutoTeach = new System.Windows.Forms.CheckBox();
            this.resultPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.filterPattern = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenResultPath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2});
            this.listView1.Location = new System.Drawing.Point(12, 34);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(776, 301);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(12, 386);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(776, 33);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "RUN";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // useAutoTeach
            // 
            this.useAutoTeach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.useAutoTeach.AutoSize = true;
            this.useAutoTeach.Location = new System.Drawing.Point(12, 368);
            this.useAutoTeach.Name = "useAutoTeach";
            this.useAutoTeach.Size = new System.Drawing.Size(117, 16);
            this.useAutoTeach.TabIndex = 3;
            this.useAutoTeach.Text = "Use Auto-Teach";
            this.useAutoTeach.UseVisualStyleBackColor = true;
            // 
            // resultPath
            // 
            this.resultPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.resultPath.Location = new System.Drawing.Point(119, 341);
            this.resultPath.Name = "resultPath";
            this.resultPath.Size = new System.Drawing.Size(616, 21);
            this.resultPath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 344);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Result Save Path";
            // 
            // filterPattern
            // 
            this.filterPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.filterPattern.Location = new System.Drawing.Point(93, 7);
            this.filterPattern.Name = "filterPattern";
            this.filterPattern.Size = new System.Drawing.Size(695, 21);
            this.filterPattern.TabIndex = 2;
            this.filterPattern.Text = "*.png";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Filter Pattern";
            // 
            // buttonOpenResultPath
            // 
            this.buttonOpenResultPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenResultPath.Location = new System.Drawing.Point(741, 341);
            this.buttonOpenResultPath.Name = "buttonOpenResultPath";
            this.buttonOpenResultPath.Size = new System.Drawing.Size(47, 21);
            this.buttonOpenResultPath.TabIndex = 1;
            this.buttonOpenResultPath.Text = "Open";
            this.buttonOpenResultPath.UseVisualStyleBackColor = true;
            this.buttonOpenResultPath.Click += new System.EventHandler(this.buttonOpenResultPath_Click);
            // 
            // ImageListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 431);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.useAutoTeach);
            this.Controls.Add(this.filterPattern);
            this.Controls.Add(this.resultPath);
            this.Controls.Add(this.buttonOpenResultPath);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.listView1);
            this.Name = "ImageListForm";
            this.Text = "ImageListForm";
            this.Load += new System.EventHandler(this.ImageListForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.CheckBox useAutoTeach;
        private System.Windows.Forms.TextBox resultPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filterPattern;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonOpenResultPath;
    }
}