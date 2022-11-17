namespace MLCCS.Operation.UI.Monitor
{
    partial class MonitoringPanel
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
            this.layoutMonitoring = new System.Windows.Forms.TableLayoutPanel();
            this.layoutMenu = new System.Windows.Forms.FlowLayoutPanel();
            this.grabImage = new System.Windows.Forms.Button();
            this.defect = new System.Windows.Forms.Button();
            this.cam = new System.Windows.Forms.Button();
            this.panelView = new System.Windows.Forms.Panel();
            this.layoutMonitoring.SuspendLayout();
            this.layoutMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMonitoring
            // 
            this.layoutMonitoring.ColumnCount = 2;
            this.layoutMonitoring.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
            this.layoutMonitoring.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMonitoring.Controls.Add(this.layoutMenu, 0, 0);
            this.layoutMonitoring.Controls.Add(this.panelView, 1, 0);
            this.layoutMonitoring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMonitoring.Location = new System.Drawing.Point(0, 0);
            this.layoutMonitoring.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMonitoring.Name = "layoutMonitoring";
            this.layoutMonitoring.RowCount = 1;
            this.layoutMonitoring.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMonitoring.Size = new System.Drawing.Size(1243, 771);
            this.layoutMonitoring.TabIndex = 0;
            // 
            // layoutMenu
            // 
            this.layoutMenu.Controls.Add(this.grabImage);
            this.layoutMenu.Controls.Add(this.defect);
            this.layoutMenu.Controls.Add(this.cam);
            this.layoutMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMenu.Location = new System.Drawing.Point(0, 0);
            this.layoutMenu.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMenu.Name = "layoutMenu";
            this.layoutMenu.Size = new System.Drawing.Size(134, 771);
            this.layoutMenu.TabIndex = 1;
            // 
            // grabImage
            // 
            this.grabImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grabImage.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.grabImage.Location = new System.Drawing.Point(3, 3);
            this.grabImage.Name = "grabImage";
            this.grabImage.Size = new System.Drawing.Size(128, 74);
            this.grabImage.TabIndex = 0;
            this.grabImage.Text = "Grab";
            this.grabImage.UseVisualStyleBackColor = true;
            // 
            // defect
            // 
            this.defect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defect.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.defect.Location = new System.Drawing.Point(3, 83);
            this.defect.Name = "defect";
            this.defect.Size = new System.Drawing.Size(128, 74);
            this.defect.TabIndex = 1;
            this.defect.Text = "Defect";
            this.defect.UseVisualStyleBackColor = true;
            this.defect.Click += new System.EventHandler(this.defect_Click);
            // 
            // cam
            // 
            this.cam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cam.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cam.Location = new System.Drawing.Point(3, 163);
            this.cam.Name = "cam";
            this.cam.Size = new System.Drawing.Size(128, 74);
            this.cam.TabIndex = 2;
            this.cam.Text = "Cam";
            this.cam.UseVisualStyleBackColor = true;
            // 
            // panelView
            // 
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(134, 0);
            this.panelView.Margin = new System.Windows.Forms.Padding(0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(1109, 771);
            this.panelView.TabIndex = 2;
            // 
            // MonitoringPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutMonitoring);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MonitoringPanel";
            this.Size = new System.Drawing.Size(1243, 771);
            this.layoutMonitoring.ResumeLayout(false);
            this.layoutMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel layoutMonitoring;
        private System.Windows.Forms.FlowLayoutPanel layoutMenu;
        private System.Windows.Forms.Button grabImage;
        private System.Windows.Forms.Button defect;
        private System.Windows.Forms.Button cam;
        private System.Windows.Forms.Panel panelView;
    }
}
