namespace UniScanG.UI.TeachPage.Inspector
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
            this.layoutTeachPanel = new System.Windows.Forms.TableLayoutPanel();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.paramPanel = new System.Windows.Forms.Panel();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.toolbarPanel = new System.Windows.Forms.Panel();
            this.layoutTeachPanel.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutTeachPanel
            // 
            this.layoutTeachPanel.ColumnCount = 2;
            this.layoutTeachPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.layoutTeachPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.layoutTeachPanel.Controls.Add(this.imagePanel, 0, 0);
            this.layoutTeachPanel.Controls.Add(this.paramPanel, 1, 0);
            this.layoutTeachPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTeachPanel.Location = new System.Drawing.Point(0, 80);
            this.layoutTeachPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTeachPanel.Name = "layoutTeachPanel";
            this.layoutTeachPanel.RowCount = 1;
            this.layoutTeachPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTeachPanel.Size = new System.Drawing.Size(680, 426);
            this.layoutTeachPanel.TabIndex = 0;
            // 
            // imagePanel
            // 
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(374, 426);
            this.imagePanel.TabIndex = 0;
            // 
            // paramPanel
            // 
            this.paramPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramPanel.Location = new System.Drawing.Point(374, 0);
            this.paramPanel.Margin = new System.Windows.Forms.Padding(0);
            this.paramPanel.Name = "paramPanel";
            this.paramPanel.Size = new System.Drawing.Size(306, 426);
            this.paramPanel.TabIndex = 2;
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.layoutTeachPanel, 0, 1);
            this.layoutMain.Controls.Add(this.toolbarPanel, 0, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(680, 506);
            this.layoutMain.TabIndex = 1;
            // 
            // toolbarPanel
            // 
            this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarPanel.Location = new System.Drawing.Point(0, 0);
            this.toolbarPanel.Margin = new System.Windows.Forms.Padding(0);
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Size = new System.Drawing.Size(680, 80);
            this.toolbarPanel.TabIndex = 1;
            // 
            // TeachPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TeachPage";
            this.Size = new System.Drawing.Size(680, 506);
            this.layoutTeachPanel.ResumeLayout(false);
            this.layoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutTeachPanel;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.Panel paramPanel;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Panel toolbarPanel;
    }
}
