namespace UniScanG.Module.Controller.Settings.Monitor.UI
{
    partial class MonitorSystemSettingPanel
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
            this.InspectorInfoGridView = new System.Windows.Forms.DataGridView();
            this.columnUse = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnUserPw = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vncViewerPath = new System.Windows.Forms.TextBox();
            this.labelVncViewerPath = new System.Windows.Forms.Label();
            this.buttonFov = new System.Windows.Forms.Button();
            this.useTestbedStage = new System.Windows.Forms.CheckBox();
            this.useStickerSensor = new System.Windows.Forms.CheckBox();
            this.laserNone = new System.Windows.Forms.RadioButton();
            this.laserUse = new System.Windows.Forms.RadioButton();
            this.laserUseVirtual = new System.Windows.Forms.RadioButton();
            this.groupBoxLaser = new System.Windows.Forms.GroupBox();
            this.useExtStopImg = new System.Windows.Forms.CheckBox();
            this.useExtMargin = new System.Windows.Forms.CheckBox();
            this.useExtTransform = new System.Windows.Forms.CheckBox();
            this.groupBoxExtFunction = new System.Windows.Forms.GroupBox();
            this.useExtObserve = new System.Windows.Forms.CheckBox();
            this.enableImPowCon = new System.Windows.Forms.CheckBox();
            this.groupBoxSystem = new System.Windows.Forms.GroupBox();
            this.useExtSticker = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.InspectorInfoGridView)).BeginInit();
            this.groupBoxLaser.SuspendLayout();
            this.groupBoxExtFunction.SuspendLayout();
            this.groupBoxSystem.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectorInfoGridView
            // 
            this.InspectorInfoGridView.AllowUserToResizeRows = false;
            this.InspectorInfoGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.InspectorInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InspectorInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnUse,
            this.columnCam,
            this.columnClient,
            this.columnPath,
            this.columnUserId,
            this.columnUserPw});
            this.InspectorInfoGridView.Location = new System.Drawing.Point(3, 176);
            this.InspectorInfoGridView.Name = "InspectorInfoGridView";
            this.InspectorInfoGridView.RowTemplate.Height = 23;
            this.InspectorInfoGridView.Size = new System.Drawing.Size(499, 122);
            this.InspectorInfoGridView.TabIndex = 0;
            // 
            // columnUse
            // 
            this.columnUse.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnUse.HeaderText = "Use";
            this.columnUse.Name = "columnUse";
            this.columnUse.Width = 33;
            // 
            // columnCam
            // 
            this.columnCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCam.HeaderText = "Cam";
            this.columnCam.Name = "columnCam";
            this.columnCam.Width = 57;
            // 
            // columnClient
            // 
            this.columnClient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnClient.HeaderText = "Client";
            this.columnClient.Name = "columnClient";
            this.columnClient.Width = 62;
            // 
            // columnPath
            // 
            this.columnPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnPath.HeaderText = "Path";
            this.columnPath.Name = "columnPath";
            // 
            // columnUserId
            // 
            this.columnUserId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnUserId.HeaderText = "ID";
            this.columnUserId.Name = "columnUserId";
            this.columnUserId.Width = 41;
            // 
            // columnUserPw
            // 
            this.columnUserPw.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnUserPw.HeaderText = "Password";
            this.columnUserPw.Name = "columnUserPw";
            this.columnUserPw.Width = 87;
            // 
            // vncViewerPath
            // 
            this.vncViewerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vncViewerPath.Location = new System.Drawing.Point(132, 7);
            this.vncViewerPath.Name = "vncViewerPath";
            this.vncViewerPath.Size = new System.Drawing.Size(354, 21);
            this.vncViewerPath.TabIndex = 0;
            // 
            // labelVncViewerPath
            // 
            this.labelVncViewerPath.AutoSize = true;
            this.labelVncViewerPath.Location = new System.Drawing.Point(16, 11);
            this.labelVncViewerPath.Name = "labelVncViewerPath";
            this.labelVncViewerPath.Size = new System.Drawing.Size(103, 12);
            this.labelVncViewerPath.TabIndex = 0;
            this.labelVncViewerPath.Text = "VNC Viewer Path";
            // 
            // buttonFov
            // 
            this.buttonFov.Location = new System.Drawing.Point(426, 51);
            this.buttonFov.Name = "buttonFov";
            this.buttonFov.Size = new System.Drawing.Size(61, 39);
            this.buttonFov.TabIndex = 2;
            this.buttonFov.Text = "FOV";
            this.buttonFov.UseVisualStyleBackColor = true;
            this.buttonFov.Click += new System.EventHandler(this.buttonFov_Click);
            // 
            // useTestbedStage
            // 
            this.useTestbedStage.AutoSize = true;
            this.useTestbedStage.Location = new System.Drawing.Point(15, 27);
            this.useTestbedStage.Name = "useTestbedStage";
            this.useTestbedStage.Size = new System.Drawing.Size(132, 16);
            this.useTestbedStage.TabIndex = 11;
            this.useTestbedStage.Text = "Use Testbed Stage";
            this.useTestbedStage.UseVisualStyleBackColor = true;
            // 
            // useStickerSensor
            // 
            this.useStickerSensor.AutoSize = true;
            this.useStickerSensor.Location = new System.Drawing.Point(15, 55);
            this.useStickerSensor.Name = "useStickerSensor";
            this.useStickerSensor.Size = new System.Drawing.Size(132, 16);
            this.useStickerSensor.TabIndex = 11;
            this.useStickerSensor.Text = "Use Sticker Sensor";
            this.useStickerSensor.UseVisualStyleBackColor = true;
            // 
            // laserNone
            // 
            this.laserNone.AutoSize = true;
            this.laserNone.Location = new System.Drawing.Point(8, 20);
            this.laserNone.Name = "laserNone";
            this.laserNone.Size = new System.Drawing.Size(53, 16);
            this.laserNone.TabIndex = 12;
            this.laserNone.TabStop = true;
            this.laserNone.Text = "None";
            this.laserNone.UseVisualStyleBackColor = true;
            // 
            // laserUse
            // 
            this.laserUse.AutoSize = true;
            this.laserUse.Location = new System.Drawing.Point(81, 20);
            this.laserUse.Name = "laserUse";
            this.laserUse.Size = new System.Drawing.Size(45, 16);
            this.laserUse.TabIndex = 12;
            this.laserUse.TabStop = true;
            this.laserUse.Text = "Use";
            this.laserUse.UseVisualStyleBackColor = true;
            // 
            // laserUseVirtual
            // 
            this.laserUseVirtual.AutoSize = true;
            this.laserUseVirtual.Location = new System.Drawing.Point(142, 19);
            this.laserUseVirtual.Name = "laserUseVirtual";
            this.laserUseVirtual.Size = new System.Drawing.Size(58, 16);
            this.laserUseVirtual.TabIndex = 12;
            this.laserUseVirtual.TabStop = true;
            this.laserUseVirtual.Text = "Virtual";
            this.laserUseVirtual.UseVisualStyleBackColor = true;
            // 
            // groupBoxLaser
            // 
            this.groupBoxLaser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxLaser.Controls.Add(this.laserNone);
            this.groupBoxLaser.Controls.Add(this.laserUse);
            this.groupBoxLaser.Controls.Add(this.laserUseVirtual);
            this.groupBoxLaser.Location = new System.Drawing.Point(208, 44);
            this.groupBoxLaser.Name = "groupBoxLaser";
            this.groupBoxLaser.Size = new System.Drawing.Size(212, 46);
            this.groupBoxLaser.TabIndex = 14;
            this.groupBoxLaser.TabStop = false;
            this.groupBoxLaser.Text = "Laser";
            // 
            // useExtStopImg
            // 
            this.useExtStopImg.AutoSize = true;
            this.useExtStopImg.Location = new System.Drawing.Point(189, 20);
            this.useExtStopImg.Name = "useExtStopImg";
            this.useExtStopImg.Size = new System.Drawing.Size(84, 16);
            this.useExtStopImg.TabIndex = 15;
            this.useExtStopImg.Text = "StopImage";
            this.useExtStopImg.UseVisualStyleBackColor = true;
            // 
            // useExtMargin
            // 
            this.useExtMargin.AutoSize = true;
            this.useExtMargin.Location = new System.Drawing.Point(8, 47);
            this.useExtMargin.Name = "useExtMargin";
            this.useExtMargin.Size = new System.Drawing.Size(63, 16);
            this.useExtMargin.TabIndex = 15;
            this.useExtMargin.Text = "Margin";
            this.useExtMargin.UseVisualStyleBackColor = true;
            // 
            // useExtTransform
            // 
            this.useExtTransform.AutoSize = true;
            this.useExtTransform.Location = new System.Drawing.Point(95, 47);
            this.useExtTransform.Name = "useExtTransform";
            this.useExtTransform.Size = new System.Drawing.Size(82, 16);
            this.useExtTransform.TabIndex = 15;
            this.useExtTransform.Text = "Transform";
            this.useExtTransform.UseVisualStyleBackColor = true;
            // 
            // groupBoxExtFunction
            // 
            this.groupBoxExtFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxExtFunction.Controls.Add(this.useExtSticker);
            this.groupBoxExtFunction.Controls.Add(this.useExtObserve);
            this.groupBoxExtFunction.Controls.Add(this.useExtStopImg);
            this.groupBoxExtFunction.Controls.Add(this.useExtTransform);
            this.groupBoxExtFunction.Controls.Add(this.useExtMargin);
            this.groupBoxExtFunction.Location = new System.Drawing.Point(208, 96);
            this.groupBoxExtFunction.Name = "groupBoxExtFunction";
            this.groupBoxExtFunction.Size = new System.Drawing.Size(279, 69);
            this.groupBoxExtFunction.TabIndex = 16;
            this.groupBoxExtFunction.TabStop = false;
            this.groupBoxExtFunction.Text = "Extend Function";
            // 
            // useExtObserve
            // 
            this.useExtObserve.AutoSize = true;
            this.useExtObserve.Location = new System.Drawing.Point(95, 20);
            this.useExtObserve.Name = "useExtObserve";
            this.useExtObserve.Size = new System.Drawing.Size(71, 16);
            this.useExtObserve.TabIndex = 15;
            this.useExtObserve.Text = "Observe";
            this.useExtObserve.UseVisualStyleBackColor = true;
            // 
            // enableImPowCon
            // 
            this.enableImPowCon.AutoSize = true;
            this.enableImPowCon.Location = new System.Drawing.Point(15, 83);
            this.enableImPowCon.Name = "enableImPowCon";
            this.enableImPowCon.Size = new System.Drawing.Size(129, 16);
            this.enableImPowCon.TabIndex = 11;
            this.enableImPowCon.Text = "IMs Power Control";
            this.enableImPowCon.UseVisualStyleBackColor = true;
            // 
            // groupBoxSystem
            // 
            this.groupBoxSystem.Controls.Add(this.useTestbedStage);
            this.groupBoxSystem.Controls.Add(this.useStickerSensor);
            this.groupBoxSystem.Controls.Add(this.enableImPowCon);
            this.groupBoxSystem.Location = new System.Drawing.Point(10, 44);
            this.groupBoxSystem.Name = "groupBoxSystem";
            this.groupBoxSystem.Size = new System.Drawing.Size(182, 121);
            this.groupBoxSystem.TabIndex = 17;
            this.groupBoxSystem.TabStop = false;
            this.groupBoxSystem.Text = "System";
            // 
            // useExtSticker
            // 
            this.useExtSticker.AutoSize = true;
            this.useExtSticker.Location = new System.Drawing.Point(8, 20);
            this.useExtSticker.Name = "useExtSticker";
            this.useExtSticker.Size = new System.Drawing.Size(62, 16);
            this.useExtSticker.TabIndex = 15;
            this.useExtSticker.Text = "Sticker";
            this.useExtSticker.UseVisualStyleBackColor = true;
            // 
            // MonitorSystemSettingPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBoxSystem);
            this.Controls.Add(this.groupBoxExtFunction);
            this.Controls.Add(this.groupBoxLaser);
            this.Controls.Add(this.buttonFov);
            this.Controls.Add(this.InspectorInfoGridView);
            this.Controls.Add(this.vncViewerPath);
            this.Controls.Add(this.labelVncViewerPath);
            this.Name = "MonitorSystemSettingPanel";
            this.Size = new System.Drawing.Size(505, 301);
            this.Load += new System.EventHandler(this.MonitorSystemSettingPanel_Load);
            this.SizeChanged += new System.EventHandler(this.MonitorSystemSettingPanel_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.InspectorInfoGridView)).EndInit();
            this.groupBoxLaser.ResumeLayout(false);
            this.groupBoxLaser.PerformLayout();
            this.groupBoxExtFunction.ResumeLayout(false);
            this.groupBoxExtFunction.PerformLayout();
            this.groupBoxSystem.ResumeLayout(false);
            this.groupBoxSystem.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView InspectorInfoGridView;
        private System.Windows.Forms.TextBox vncViewerPath;
        private System.Windows.Forms.Label labelVncViewerPath;
        private System.Windows.Forms.Button buttonFov;
        private System.Windows.Forms.CheckBox useTestbedStage;
        private System.Windows.Forms.CheckBox useStickerSensor;
        private System.Windows.Forms.RadioButton laserNone;
        private System.Windows.Forms.RadioButton laserUse;
        private System.Windows.Forms.RadioButton laserUseVirtual;
        private System.Windows.Forms.GroupBox groupBoxLaser;
        private System.Windows.Forms.CheckBox useExtStopImg;
        private System.Windows.Forms.CheckBox useExtMargin;
        private System.Windows.Forms.CheckBox useExtTransform;
        private System.Windows.Forms.GroupBox groupBoxExtFunction;
        private System.Windows.Forms.CheckBox useExtObserve;
        private System.Windows.Forms.CheckBox enableImPowCon;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnUserPw;
        private System.Windows.Forms.GroupBox groupBoxSystem;
        private System.Windows.Forms.CheckBox useExtSticker;
    }
}
