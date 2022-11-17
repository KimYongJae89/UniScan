namespace UniScanG.UI.Etc
{
    partial class ModelPanel
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
            this.layoutModelName = new System.Windows.Forms.TableLayoutPanel();
            this.modelName = new System.Windows.Forms.Label();
            this.labelModelName = new System.Windows.Forms.Label();
            this.layoutModelName.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutModelName
            // 
            this.layoutModelName.ColumnCount = 1;
            this.layoutModelName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutModelName.Controls.Add(this.modelName, 0, 1);
            this.layoutModelName.Controls.Add(this.labelModelName, 0, 0);
            this.layoutModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutModelName.Location = new System.Drawing.Point(10, 10);
            this.layoutModelName.Margin = new System.Windows.Forms.Padding(20);
            this.layoutModelName.Name = "layoutModelName";
            this.layoutModelName.RowCount = 2;
            this.layoutModelName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutModelName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutModelName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutModelName.Size = new System.Drawing.Size(267, 65);
            this.layoutModelName.TabIndex = 2;
            // 
            // modelName
            // 
            this.modelName.AutoSize = true;
            this.modelName.BackColor = System.Drawing.Color.DarkGray;
            this.modelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelName.Location = new System.Drawing.Point(0, 32);
            this.modelName.Margin = new System.Windows.Forms.Padding(0);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(267, 33);
            this.modelName.TabIndex = 0;
            this.modelName.Text = "Empty";
            this.modelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModelName
            // 
            this.labelModelName.AutoSize = true;
            this.labelModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModelName.Location = new System.Drawing.Point(3, 0);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(261, 32);
            this.labelModelName.TabIndex = 0;
            this.labelModelName.Text = "Model";
            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ModelPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.layoutModelName);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ModelPanel";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(287, 85);
            this.layoutModelName.ResumeLayout(false);
            this.layoutModelName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutModelName;
        private System.Windows.Forms.Label modelName;
        private System.Windows.Forms.Label labelModelName;
    }
}
