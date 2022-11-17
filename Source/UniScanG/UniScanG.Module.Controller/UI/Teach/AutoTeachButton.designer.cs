namespace UniScanG.Module.Controller.UI.Teach
{
    partial class AutoTeachButton
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoTeachButton));
            this.button = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // button
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextVAlignAsString = "Bottom";
            this.button.Appearance = appearance1;
            this.button.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button.ImageSize = new System.Drawing.Size(45, 45);
            this.button.Location = new System.Drawing.Point(0, 0);
            this.button.Margin = new System.Windows.Forms.Padding(0);
            this.button.Name = "button";
            this.button.Size = new System.Drawing.Size(80, 80);
            this.button.TabIndex = 145;
            this.button.Text = "A.Teach";
            this.button.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.button.Click += new System.EventHandler(this.button_Click);
            // 
            // AutoTeachButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.button);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Name = "AutoTeachButton";
            this.Size = new System.Drawing.Size(80, 80);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton button;
    }
}
