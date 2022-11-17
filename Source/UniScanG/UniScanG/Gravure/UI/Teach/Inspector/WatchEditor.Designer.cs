namespace UniScanG.Gravure.UI.Teach.Inspector
{
    partial class WatchEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelImage = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonAdd = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabelSelCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelType = new System.Windows.Forms.ToolStripLabel();
            this.toolStripType = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelId = new System.Windows.Forms.ToolStripLabel();
            this.toolStripId = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabelName = new System.Windows.Forms.ToolStripLabel();
            this.toolStripName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonReset = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonApply = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 60);
            this.panelImage.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(1406, 560);
            this.panelImage.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Malgun Gothic", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonAdd,
            this.toolStripButtonRemove,
            this.toolStripButtonAll,
            this.toolStripLabelSelCount,
            this.toolStripSeparator1,
            this.toolStripLabelType,
            this.toolStripType,
            this.toolStripLabelId,
            this.toolStripId,
            this.toolStripLabelName,
            this.toolStripName,
            this.toolStripSeparator2,
            this.toolStripButtonReset,
            this.toolStripButtonApply});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1406, 60);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButtonAdd
            // 
            this.toolStripSplitButtonAdd.Image = global::UniScanG.Properties.Resources.add_32;
            this.toolStripSplitButtonAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSplitButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonAdd.Margin = new System.Windows.Forms.Padding(5, 1, 5, 2);
            this.toolStripSplitButtonAdd.Name = "toolStripSplitButtonAdd";
            this.toolStripSplitButtonAdd.Size = new System.Drawing.Size(57, 57);
            this.toolStripSplitButtonAdd.Text = "Add";
            this.toolStripSplitButtonAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripSplitButtonAdd.ButtonClick += new System.EventHandler(this.toolStripSplitButtonAdd_ButtonClick);
            // 
            // toolStripButtonRemove
            // 
            this.toolStripButtonRemove.Image = global::UniScanG.Properties.Resources.delete_32;
            this.toolStripButtonRemove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemove.Margin = new System.Windows.Forms.Padding(5, 1, 5, 2);
            this.toolStripButtonRemove.Name = "toolStripButtonRemove";
            this.toolStripButtonRemove.Size = new System.Drawing.Size(74, 57);
            this.toolStripButtonRemove.Text = "Remove";
            this.toolStripButtonRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
            // 
            // toolStripButtonAll
            // 
            this.toolStripButtonAll.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripButtonAll.Image = global::UniScanG.Properties.Resources.test_32;
            this.toolStripButtonAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAll.Name = "toolStripButtonAll";
            this.toolStripButtonAll.Size = new System.Drawing.Size(84, 57);
            this.toolStripButtonAll.Text = "All Select";
            this.toolStripButtonAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonAll.Click += new System.EventHandler(this.toolStripButtonAll_Click);
            // 
            // toolStripLabelSelCount
            // 
            this.toolStripLabelSelCount.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripLabelSelCount.Name = "toolStripLabelSelCount";
            this.toolStripLabelSelCount.Size = new System.Drawing.Size(128, 57);
            this.toolStripLabelSelCount.Text = "0 Item Selected";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 60);
            // 
            // toolStripLabelType
            // 
            this.toolStripLabelType.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripLabelType.Name = "toolStripLabelType";
            this.toolStripLabelType.Size = new System.Drawing.Size(46, 57);
            this.toolStripLabelType.Text = "Type";
            // 
            // toolStripType
            // 
            this.toolStripType.Enabled = false;
            this.toolStripType.Name = "toolStripType";
            this.toolStripType.Size = new System.Drawing.Size(100, 60);
            // 
            // toolStripLabelId
            // 
            this.toolStripLabelId.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripLabelId.Name = "toolStripLabelId";
            this.toolStripLabelId.Size = new System.Drawing.Size(25, 57);
            this.toolStripLabelId.Text = "ID";
            // 
            // toolStripId
            // 
            this.toolStripId.Enabled = false;
            this.toolStripId.Margin = new System.Windows.Forms.Padding(1, 0, 5, 0);
            this.toolStripId.Name = "toolStripId";
            this.toolStripId.Size = new System.Drawing.Size(100, 60);
            this.toolStripId.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripLabelName
            // 
            this.toolStripLabelName.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolStripLabelName.Name = "toolStripLabelName";
            this.toolStripLabelName.Size = new System.Drawing.Size(53, 57);
            this.toolStripLabelName.Text = "Name";
            // 
            // toolStripName
            // 
            this.toolStripName.Margin = new System.Windows.Forms.Padding(1, 0, 5, 0);
            this.toolStripName.Name = "toolStripName";
            this.toolStripName.Size = new System.Drawing.Size(100, 60);
            this.toolStripName.TextChanged += new System.EventHandler(this.toolStripName_TextChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 60);
            // 
            // toolStripButtonReset
            // 
            this.toolStripButtonReset.Image = global::UniScanG.Properties.Resources.Reset32;
            this.toolStripButtonReset.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonReset.Margin = new System.Windows.Forms.Padding(5, 1, 5, 2);
            this.toolStripButtonReset.Name = "toolStripButtonReset";
            this.toolStripButtonReset.Size = new System.Drawing.Size(55, 57);
            this.toolStripButtonReset.Text = "Reset";
            this.toolStripButtonReset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonReset.Click += new System.EventHandler(this.toolStripButtonReset_Click);
            // 
            // toolStripButtonApply
            // 
            this.toolStripButtonApply.Image = global::UniScanG.Properties.Resources.save32;
            this.toolStripButtonApply.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonApply.Margin = new System.Windows.Forms.Padding(5, 1, 5, 2);
            this.toolStripButtonApply.Name = "toolStripButtonApply";
            this.toolStripButtonApply.Size = new System.Drawing.Size(57, 57);
            this.toolStripButtonApply.Text = "Apply";
            this.toolStripButtonApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonApply.Click += new System.EventHandler(this.toolStripButtonApply_Click);
            // 
            // WatchEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "WatchEditor";
            this.Size = new System.Drawing.Size(1406, 620);
            this.SizeChanged += new System.EventHandler(this.RegionEditor_SizeChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelId;
        private System.Windows.Forms.ToolStripTextBox toolStripId;
        private System.Windows.Forms.ToolStripLabel toolStripLabelName;
        private System.Windows.Forms.ToolStripTextBox toolStripName;
        private System.Windows.Forms.ToolStripLabel toolStripLabelType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButtonReset;
        private System.Windows.Forms.ToolStripButton toolStripButtonApply;
        private System.Windows.Forms.ToolStripButton toolStripButtonAll;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSelCount;
        private System.Windows.Forms.ToolStripTextBox toolStripType;
    }
}
