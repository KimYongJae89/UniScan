namespace ImageConverter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonAddFile = new System.Windows.Forms.Button();
            this.buttonAddFolder = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupConvertTo = new System.Windows.Forms.GroupBox();
            this.labelThumbnailSizeUnit = new System.Windows.Forms.Label();
            this.thumbnailSize = new System.Windows.Forms.NumericUpDown();
            this.checkThumbnail = new System.Windows.Forms.CheckBox();
            this.radioJPG = new System.Windows.Forms.RadioButton();
            this.radioBMP = new System.Windows.Forms.RadioButton();
            this.radioPNG = new System.Windows.Forms.RadioButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupConvertTo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailSize)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAddFile
            // 
            resources.ApplyResources(this.buttonAddFile, "buttonAddFile");
            this.buttonAddFile.ImageList = this.imageList1;
            this.buttonAddFile.Name = "buttonAddFile";
            this.buttonAddFile.UseVisualStyleBackColor = true;
            this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
            // 
            // buttonAddFolder
            // 
            resources.ApplyResources(this.buttonAddFolder, "buttonAddFolder");
            this.buttonAddFolder.ImageList = this.imageList1;
            this.buttonAddFolder.Name = "buttonAddFolder";
            this.buttonAddFolder.UseVisualStyleBackColor = true;
            this.buttonAddFolder.Click += new System.EventHandler(this.buttonAddFolder_Click);
            // 
            // buttonStart
            // 
            resources.ApplyResources(this.buttonStart, "buttonStart");
            this.buttonStart.ImageList = this.imageList1;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupConvertTo
            // 
            this.groupConvertTo.Controls.Add(this.labelThumbnailSizeUnit);
            this.groupConvertTo.Controls.Add(this.thumbnailSize);
            this.groupConvertTo.Controls.Add(this.checkThumbnail);
            this.groupConvertTo.Controls.Add(this.radioJPG);
            this.groupConvertTo.Controls.Add(this.radioBMP);
            this.groupConvertTo.Controls.Add(this.radioPNG);
            resources.ApplyResources(this.groupConvertTo, "groupConvertTo");
            this.groupConvertTo.Name = "groupConvertTo";
            this.groupConvertTo.TabStop = false;
            // 
            // labelThumbnailSizeUnit
            // 
            resources.ApplyResources(this.labelThumbnailSizeUnit, "labelThumbnailSizeUnit");
            this.labelThumbnailSizeUnit.Name = "labelThumbnailSizeUnit";
            // 
            // thumbnailSize
            // 
            resources.ApplyResources(this.thumbnailSize, "thumbnailSize");
            this.thumbnailSize.Maximum = new decimal(new int[] {
            12800,
            0,
            0,
            0});
            this.thumbnailSize.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.thumbnailSize.Name = "thumbnailSize";
            this.thumbnailSize.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // checkThumbnail
            // 
            resources.ApplyResources(this.checkThumbnail, "checkThumbnail");
            this.checkThumbnail.Checked = true;
            this.checkThumbnail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkThumbnail.Name = "checkThumbnail";
            this.checkThumbnail.UseVisualStyleBackColor = true;
            this.checkThumbnail.CheckedChanged += new System.EventHandler(this.checkThumbnail_CheckedChanged);
            // 
            // radioJPG
            // 
            resources.ApplyResources(this.radioJPG, "radioJPG");
            this.radioJPG.Name = "radioJPG";
            this.radioJPG.UseVisualStyleBackColor = true;
            this.radioJPG.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioBMP
            // 
            resources.ApplyResources(this.radioBMP, "radioBMP");
            this.radioBMP.Name = "radioBMP";
            this.radioBMP.UseVisualStyleBackColor = true;
            this.radioBMP.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // radioPNG
            // 
            resources.ApplyResources(this.radioPNG, "radioPNG");
            this.radioPNG.Checked = true;
            this.radioPNG.Name = "radioPNG";
            this.radioPNG.TabStop = true;
            this.radioPNG.UseVisualStyleBackColor = true;
            this.radioPNG.CheckedChanged += new System.EventHandler(this.radio_CheckedChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowDrop = true;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "File.png");
            this.imageList1.Images.SetKeyName(1, "Folder.png");
            this.imageList1.Images.SetKeyName(2, "Start.png");
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupConvertTo);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonAddFolder);
            this.Controls.Add(this.buttonAddFile);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupConvertTo.ResumeLayout(false);
            this.groupConvertTo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailSize)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddFile;
        private System.Windows.Forms.Button buttonAddFolder;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupConvertTo;
        private System.Windows.Forms.CheckBox checkThumbnail;
        private System.Windows.Forms.RadioButton radioJPG;
        private System.Windows.Forms.RadioButton radioBMP;
        private System.Windows.Forms.RadioButton radioPNG;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelThumbnailSizeUnit;
        private System.Windows.Forms.NumericUpDown thumbnailSize;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

