namespace DynMvp.Data.UI
{
    partial class TargetGridView
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.targetViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // targetViewPanel
            // 
            this.targetViewPanel.ColumnCount = 2;
            this.targetViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.targetViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.targetViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.targetViewPanel.Location = new System.Drawing.Point(0, 0);
            this.targetViewPanel.Name = "targetViewPanel";
            this.targetViewPanel.RowCount = 2;
            this.targetViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.targetViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.targetViewPanel.Size = new System.Drawing.Size(633, 564);
            this.targetViewPanel.TabIndex = 0;
            // 
            // TargetGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetViewPanel);
            this.Name = "TargetGridView";
            this.Size = new System.Drawing.Size(633, 564);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel targetViewPanel;

    }
}
