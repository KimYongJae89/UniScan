namespace UniScanM.Pinhole.UI.MenuPanel
{
    partial class SheetDisplayPanel
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
            this.vsbDefectMap = new System.Windows.Forms.VScrollBar();
            this.panelDefectMap = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.nudSheetLength = new System.Windows.Forms.NumericUpDown();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.chkZoomSheet = new System.Windows.Forms.CheckBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSheetLength)).BeginInit();
            this.SuspendLayout();
            // 
            // vsbDefectMap
            // 
            this.vsbDefectMap.Dock = System.Windows.Forms.DockStyle.Right;
            this.vsbDefectMap.Location = new System.Drawing.Point(461, 33);
            this.vsbDefectMap.Maximum = 100000;
            this.vsbDefectMap.Name = "vsbDefectMap";
            this.vsbDefectMap.Size = new System.Drawing.Size(19, 418);
            this.vsbDefectMap.TabIndex = 0;
            this.vsbDefectMap.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vsbDefectMap_Scroll);
            // 
            // panelDefectMap
            // 
            this.panelDefectMap.BackColor = System.Drawing.Color.Transparent;
            this.panelDefectMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefectMap.Location = new System.Drawing.Point(0, 33);
            this.panelDefectMap.Name = "panelDefectMap";
            this.panelDefectMap.Size = new System.Drawing.Size(461, 418);
            this.panelDefectMap.TabIndex = 1;
            this.panelDefectMap.SizeChanged += new System.EventHandler(this.panelDefectMap_SizeChanged);
            this.panelDefectMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDefectMap_Paint);
            this.panelDefectMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelDefectMap_MouseClick);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.nudSheetLength);
            this.panelTop.Controls.Add(this.chkPreview);
            this.panelTop.Controls.Add(this.chkZoomSheet);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(480, 33);
            this.panelTop.TabIndex = 1;
            // 
            // nudSheetLength
            // 
            this.nudSheetLength.Location = new System.Drawing.Point(121, 5);
            this.nudSheetLength.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudSheetLength.Name = "nudSheetLength";
            this.nudSheetLength.Size = new System.Drawing.Size(72, 21);
            this.nudSheetLength.TabIndex = 1;
            this.nudSheetLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSheetLength.ValueChanged += new System.EventHandler(this.nudSheetLength_ValueChanged);
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(368, 9);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(69, 16);
            this.chkPreview.TabIndex = 0;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // chkZoomSheet
            // 
            this.chkZoomSheet.AutoSize = true;
            this.chkZoomSheet.Checked = true;
            this.chkZoomSheet.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZoomSheet.Location = new System.Drawing.Point(8, 8);
            this.chkZoomSheet.Name = "chkZoomSheet";
            this.chkZoomSheet.Size = new System.Drawing.Size(93, 16);
            this.chkZoomSheet.TabIndex = 0;
            this.chkZoomSheet.Text = "Zoom Sheet";
            this.chkZoomSheet.UseVisualStyleBackColor = true;
            this.chkZoomSheet.CheckedChanged += new System.EventHandler(this.chkZoomSheet_CheckedChanged);
            // 
            // SheetDisplayPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelDefectMap);
            this.Controls.Add(this.vsbDefectMap);
            this.Controls.Add(this.panelTop);
            this.DoubleBuffered = true;
            this.Name = "SheetDisplayPanel";
            this.Size = new System.Drawing.Size(480, 451);
            this.Load += new System.EventHandler(this.SheetDisplayPanel_Load);
            this.SizeChanged += new System.EventHandler(this.SheetDisplayPanel_SizeChanged);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSheetLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar vsbDefectMap;
        private System.Windows.Forms.Panel panelDefectMap;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.NumericUpDown nudSheetLength;
        private System.Windows.Forms.CheckBox chkZoomSheet;
        private System.Windows.Forms.CheckBox chkPreview;
    }
}
