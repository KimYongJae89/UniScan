namespace UniScanG.UI.Etc
{
    partial class VncCamButton
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
            this.buttonCam = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // buttonCam
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanG.Properties.Resources.Scanner1;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextVAlignAsString = "Bottom";
            this.buttonCam.Appearance = appearance1;
            this.buttonCam.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCam.Font = new System.Drawing.Font("Malgun Gothic", 11F, System.Drawing.FontStyle.Bold);
            this.buttonCam.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonCam.Location = new System.Drawing.Point(0, 0);
            this.buttonCam.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCam.Name = "buttonCam";
            this.buttonCam.Size = new System.Drawing.Size(80, 80);
            this.buttonCam.TabIndex = 0;
            this.buttonCam.Text = "Cam #";
            this.buttonCam.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonCam.Click += new System.EventHandler(this.buttonCam_Click);
            // 
            // VncCamButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.buttonCam);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Name = "VncCamButton";
            this.Size = new System.Drawing.Size(80, 80);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton buttonCam;
    }
}
