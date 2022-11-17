namespace UniScanG.Module.Controller.Device.Stage
{
    partial class TestbedStageControlButton
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
            this.buttonMachine = new Infragistics.Win.Misc.UltraButton();
            this.SuspendLayout();
            // 
            // buttonMachine
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = UniScanG.Module.Controller.Properties.Resources.machine32;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextVAlignAsString = "Bottom";
            this.buttonMachine.Appearance = appearance1;
            this.buttonMachine.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMachine.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonMachine.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonMachine.Location = new System.Drawing.Point(0, 0);
            this.buttonMachine.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMachine.Name = "buttonMachine";
            this.buttonMachine.Size = new System.Drawing.Size(80, 80);
            this.buttonMachine.TabIndex = 145;
            this.buttonMachine.Text = "Machine";
            this.buttonMachine.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonMachine.Click += new System.EventHandler(this.buttonMachine_Click);
            // 
            // MachineSettingButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.buttonMachine);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.Name = "MachineSettingButton";
            this.Size = new System.Drawing.Size(80, 80);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton buttonMachine;
    }
}
