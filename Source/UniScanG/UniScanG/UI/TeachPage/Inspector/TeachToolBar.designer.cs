namespace UniScanG.UI.TeachPage.Inspector
{
    partial class TeachToolBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TeachToolBar));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.buttonGrab = new System.Windows.Forms.ToolStripButton();
            this.buttonAutoTeach = new System.Windows.Forms.ToolStripButton();
            this.buttonInspect = new System.Windows.Forms.ToolStripButton();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonSave = new System.Windows.Forms.ToolStripButton();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonLoadImage = new System.Windows.Forms.ToolStripButton();
            this.buttonExportData = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripMain.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.toolStripMain.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonGrab,
            this.buttonAutoTeach,
            this.buttonInspect,
            this.separator1,
            this.buttonSave,
            this.separator2,
            this.buttonLoadImage,
            this.buttonExportData});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.toolStripMain.Size = new System.Drawing.Size(1068, 80);
            this.toolStripMain.TabIndex = 5;
            // 
            // buttonGrab
            // 
            this.buttonGrab.AutoSize = false;
            this.buttonGrab.Image = global::UniScanG.Properties.Resources.process_shot_32;
            this.buttonGrab.Name = "buttonGrab";
            this.buttonGrab.Size = new System.Drawing.Size(120, 77);
            this.buttonGrab.Text = "Image Grab";
            this.buttonGrab.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonGrab.ToolTipText = "Grab";
            this.buttonGrab.Click += new System.EventHandler(this.buttonGrab_Click);
            // 
            // buttonAutoTeach
            // 
            this.buttonAutoTeach.AutoSize = false;
            this.buttonAutoTeach.Image = global::UniScanG.Properties.Resources.Teac;
            this.buttonAutoTeach.Name = "buttonAutoTeach";
            this.buttonAutoTeach.Size = new System.Drawing.Size(120, 77);
            this.buttonAutoTeach.Text = "Auto Teach";
            this.buttonAutoTeach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonAutoTeach.Click += new System.EventHandler(this.buttonAutoTeach_Click);
            // 
            // buttonInspect
            // 
            this.buttonInspect.AutoSize = false;
            this.buttonInspect.Image = ((System.Drawing.Image)(resources.GetObject("buttonInspect.Image")));
            this.buttonInspect.Name = "buttonInspect";
            this.buttonInspect.Size = new System.Drawing.Size(120, 77);
            this.buttonInspect.Text = "Inspection";
            this.buttonInspect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonInspect.ToolTipText = "Inspect";
            this.buttonInspect.Click += new System.EventHandler(this.buttonInspect_Click);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(6, 80);
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = false;
            this.buttonSave.Image = global::UniScanG.Properties.Resources.save32;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 77);
            this.buttonSave.Text = "Save Model";
            this.buttonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(6, 80);
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.AutoSize = false;
            this.buttonLoadImage.Image = global::UniScanG.Properties.Resources.picture_folder_32;
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(120, 77);
            this.buttonLoadImage.Text = "Load Image";
            this.buttonLoadImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonLoadImage.ToolTipText = "Select Image Folder";
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // buttonExportData
            // 
            this.buttonExportData.AutoSize = false;
            this.buttonExportData.Image = global::UniScanG.Properties.Resources.export;
            this.buttonExportData.Name = "buttonExportData";
            this.buttonExportData.Size = new System.Drawing.Size(120, 77);
            this.buttonExportData.Text = "Export Data";
            this.buttonExportData.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.buttonExportData.Click += new System.EventHandler(this.buttonExportData_Click);
            // 
            // TeachToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.toolStripMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TeachToolBar";
            this.Size = new System.Drawing.Size(1068, 80);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton buttonGrab;
        private System.Windows.Forms.ToolStripButton buttonAutoTeach;
        private System.Windows.Forms.ToolStripButton buttonInspect;
        private System.Windows.Forms.ToolStripSeparator separator1;
        private System.Windows.Forms.ToolStripButton buttonSave;
        private System.Windows.Forms.ToolStripSeparator separator2;
        private System.Windows.Forms.ToolStripButton buttonLoadImage;
        private System.Windows.Forms.ToolStripButton buttonExportData;
    }
}
