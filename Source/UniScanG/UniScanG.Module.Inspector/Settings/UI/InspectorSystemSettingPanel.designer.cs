namespace UniScanG.Module.Inspector.Settings.Inspector.UI
{
    partial class InspectorSystemSettingPanel
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
            this.labelClientIndex = new System.Windows.Forms.Label();
            this.clientIndex = new System.Windows.Forms.NumericUpDown();
            this.labelCamIndex = new System.Windows.Forms.Label();
            this.camIndex = new System.Windows.Forms.NumericUpDown();
            this.calculatorVersion = new System.Windows.Forms.ComboBox();
            this.labelCalculator = new System.Windows.Forms.Label();
            this.labelTrainer = new System.Windows.Forms.Label();
            this.trainerVersion = new System.Windows.Forms.ComboBox();
            this.groupBoxAlgorithmVersion = new System.Windows.Forms.GroupBox();
            this.detectorVersion = new System.Windows.Forms.ComboBox();
            this.sheetFindVersion = new System.Windows.Forms.ComboBox();
            this.labelDetectorVersion = new System.Windows.Forms.Label();
            this.labelSheetFind = new System.Windows.Forms.Label();
            this.groupBoxInspectorInfo = new System.Windows.Forms.GroupBox();
            this.ipAddress = new System.Windows.Forms.Label();
            this.labelIpAddress = new System.Windows.Forms.Label();
            this.groupBoxExtFunction = new System.Windows.Forms.GroupBox();
            this.useExtSticker = new System.Windows.Forms.CheckBox();
            this.useExtObserve = new System.Windows.Forms.CheckBox();
            this.useExtStopImg = new System.Windows.Forms.CheckBox();
            this.useExtTransform = new System.Windows.Forms.CheckBox();
            this.useExtMargin = new System.Windows.Forms.CheckBox();
            this.splitExactPattern = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).BeginInit();
            this.groupBoxAlgorithmVersion.SuspendLayout();
            this.groupBoxInspectorInfo.SuspendLayout();
            this.groupBoxExtFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelClientIndex
            // 
            this.labelClientIndex.AutoSize = true;
            this.labelClientIndex.Location = new System.Drawing.Point(8, 62);
            this.labelClientIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelClientIndex.Name = "labelClientIndex";
            this.labelClientIndex.Size = new System.Drawing.Size(72, 12);
            this.labelClientIndex.TabIndex = 0;
            this.labelClientIndex.Text = "Client Index";
            // 
            // clientIndex
            // 
            this.clientIndex.Location = new System.Drawing.Point(103, 58);
            this.clientIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.clientIndex.Name = "clientIndex";
            this.clientIndex.Size = new System.Drawing.Size(78, 21);
            this.clientIndex.TabIndex = 3;
            this.clientIndex.ValueChanged += new System.EventHandler(this.InspectorIndex_ValueChanged);
            // 
            // labelCamIndex
            // 
            this.labelCamIndex.AutoSize = true;
            this.labelCamIndex.Location = new System.Drawing.Point(8, 33);
            this.labelCamIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelCamIndex.Name = "labelCamIndex";
            this.labelCamIndex.Size = new System.Drawing.Size(67, 12);
            this.labelCamIndex.TabIndex = 0;
            this.labelCamIndex.Text = "Cam Index";
            // 
            // camIndex
            // 
            this.camIndex.Location = new System.Drawing.Point(103, 29);
            this.camIndex.Name = "camIndex";
            this.camIndex.Size = new System.Drawing.Size(78, 21);
            this.camIndex.TabIndex = 0;
            this.camIndex.ValueChanged += new System.EventHandler(this.InspectorIndex_ValueChanged);
            // 
            // calculatorVersion
            // 
            this.calculatorVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.calculatorVersion.FormattingEnabled = true;
            this.calculatorVersion.Location = new System.Drawing.Point(81, 51);
            this.calculatorVersion.Name = "calculatorVersion";
            this.calculatorVersion.Size = new System.Drawing.Size(121, 20);
            this.calculatorVersion.TabIndex = 11;
            // 
            // labelCalculator
            // 
            this.labelCalculator.AutoSize = true;
            this.labelCalculator.Location = new System.Drawing.Point(14, 55);
            this.labelCalculator.Margin = new System.Windows.Forms.Padding(0);
            this.labelCalculator.Name = "labelCalculator";
            this.labelCalculator.Size = new System.Drawing.Size(62, 12);
            this.labelCalculator.TabIndex = 0;
            this.labelCalculator.Text = "Calculator";
            // 
            // labelTrainer
            // 
            this.labelTrainer.AutoSize = true;
            this.labelTrainer.Location = new System.Drawing.Point(14, 111);
            this.labelTrainer.Margin = new System.Windows.Forms.Padding(0);
            this.labelTrainer.Name = "labelTrainer";
            this.labelTrainer.Size = new System.Drawing.Size(45, 12);
            this.labelTrainer.TabIndex = 0;
            this.labelTrainer.Text = "Trainer";
            // 
            // trainerVersion
            // 
            this.trainerVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.trainerVersion.FormattingEnabled = true;
            this.trainerVersion.Location = new System.Drawing.Point(81, 107);
            this.trainerVersion.Name = "trainerVersion";
            this.trainerVersion.Size = new System.Drawing.Size(121, 20);
            this.trainerVersion.TabIndex = 11;
            // 
            // groupBoxAlgorithmVersion
            // 
            this.groupBoxAlgorithmVersion.Controls.Add(this.detectorVersion);
            this.groupBoxAlgorithmVersion.Controls.Add(this.sheetFindVersion);
            this.groupBoxAlgorithmVersion.Controls.Add(this.calculatorVersion);
            this.groupBoxAlgorithmVersion.Controls.Add(this.labelDetectorVersion);
            this.groupBoxAlgorithmVersion.Controls.Add(this.labelSheetFind);
            this.groupBoxAlgorithmVersion.Controls.Add(this.trainerVersion);
            this.groupBoxAlgorithmVersion.Controls.Add(this.labelCalculator);
            this.groupBoxAlgorithmVersion.Controls.Add(this.labelTrainer);
            this.groupBoxAlgorithmVersion.Location = new System.Drawing.Point(234, 12);
            this.groupBoxAlgorithmVersion.Name = "groupBoxAlgorithmVersion";
            this.groupBoxAlgorithmVersion.Size = new System.Drawing.Size(219, 133);
            this.groupBoxAlgorithmVersion.TabIndex = 12;
            this.groupBoxAlgorithmVersion.TabStop = false;
            this.groupBoxAlgorithmVersion.Text = "Algorithm Version";
            // 
            // detectorVersion
            // 
            this.detectorVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.detectorVersion.FormattingEnabled = true;
            this.detectorVersion.Location = new System.Drawing.Point(81, 79);
            this.detectorVersion.Name = "detectorVersion";
            this.detectorVersion.Size = new System.Drawing.Size(121, 20);
            this.detectorVersion.TabIndex = 11;
            // 
            // sheetFindVersion
            // 
            this.sheetFindVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sheetFindVersion.FormattingEnabled = true;
            this.sheetFindVersion.Location = new System.Drawing.Point(81, 25);
            this.sheetFindVersion.Name = "sheetFindVersion";
            this.sheetFindVersion.Size = new System.Drawing.Size(121, 20);
            this.sheetFindVersion.TabIndex = 11;
            // 
            // labelDetectorVersion
            // 
            this.labelDetectorVersion.AutoSize = true;
            this.labelDetectorVersion.Location = new System.Drawing.Point(14, 83);
            this.labelDetectorVersion.Margin = new System.Windows.Forms.Padding(0);
            this.labelDetectorVersion.Name = "labelDetectorVersion";
            this.labelDetectorVersion.Size = new System.Drawing.Size(51, 12);
            this.labelDetectorVersion.TabIndex = 0;
            this.labelDetectorVersion.Text = "Detector";
            // 
            // labelSheetFind
            // 
            this.labelSheetFind.AutoSize = true;
            this.labelSheetFind.Location = new System.Drawing.Point(14, 29);
            this.labelSheetFind.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetFind.Name = "labelSheetFind";
            this.labelSheetFind.Size = new System.Drawing.Size(61, 12);
            this.labelSheetFind.TabIndex = 0;
            this.labelSheetFind.Text = "SheetFind";
            // 
            // groupBoxInspectorInfo
            // 
            this.groupBoxInspectorInfo.Controls.Add(this.splitExactPattern);
            this.groupBoxInspectorInfo.Controls.Add(this.labelCamIndex);
            this.groupBoxInspectorInfo.Controls.Add(this.ipAddress);
            this.groupBoxInspectorInfo.Controls.Add(this.labelIpAddress);
            this.groupBoxInspectorInfo.Controls.Add(this.labelClientIndex);
            this.groupBoxInspectorInfo.Controls.Add(this.clientIndex);
            this.groupBoxInspectorInfo.Controls.Add(this.camIndex);
            this.groupBoxInspectorInfo.Location = new System.Drawing.Point(16, 12);
            this.groupBoxInspectorInfo.Name = "groupBoxInspectorInfo";
            this.groupBoxInspectorInfo.Size = new System.Drawing.Size(199, 182);
            this.groupBoxInspectorInfo.TabIndex = 13;
            this.groupBoxInspectorInfo.TabStop = false;
            this.groupBoxInspectorInfo.Text = "Inspector Info";
            // 
            // ipAddress
            // 
            this.ipAddress.AutoSize = true;
            this.ipAddress.Location = new System.Drawing.Point(101, 93);
            this.ipAddress.Margin = new System.Windows.Forms.Padding(0);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(89, 12);
            this.ipAddress.TabIndex = 0;
            this.ipAddress.Text = "000.000.000.000";
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.AutoSize = true;
            this.labelIpAddress.Location = new System.Drawing.Point(8, 93);
            this.labelIpAddress.Margin = new System.Windows.Forms.Padding(0);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(67, 12);
            this.labelIpAddress.TabIndex = 0;
            this.labelIpAddress.Text = "IP Address";
            // 
            // groupBoxExtFunction
            // 
            this.groupBoxExtFunction.Controls.Add(this.useExtSticker);
            this.groupBoxExtFunction.Controls.Add(this.useExtObserve);
            this.groupBoxExtFunction.Controls.Add(this.useExtStopImg);
            this.groupBoxExtFunction.Controls.Add(this.useExtTransform);
            this.groupBoxExtFunction.Controls.Add(this.useExtMargin);
            this.groupBoxExtFunction.Location = new System.Drawing.Point(234, 151);
            this.groupBoxExtFunction.Name = "groupBoxExtFunction";
            this.groupBoxExtFunction.Size = new System.Drawing.Size(219, 134);
            this.groupBoxExtFunction.TabIndex = 17;
            this.groupBoxExtFunction.TabStop = false;
            this.groupBoxExtFunction.Text = "Extend Function";
            // 
            // useExtSticker
            // 
            this.useExtSticker.AutoSize = true;
            this.useExtSticker.Location = new System.Drawing.Point(6, 20);
            this.useExtSticker.Name = "useExtSticker";
            this.useExtSticker.Size = new System.Drawing.Size(62, 16);
            this.useExtSticker.TabIndex = 15;
            this.useExtSticker.Text = "Sticker";
            this.useExtSticker.UseVisualStyleBackColor = true;
            // 
            // useExtObserve
            // 
            this.useExtObserve.AutoSize = true;
            this.useExtObserve.Location = new System.Drawing.Point(6, 41);
            this.useExtObserve.Name = "useExtObserve";
            this.useExtObserve.Size = new System.Drawing.Size(71, 16);
            this.useExtObserve.TabIndex = 15;
            this.useExtObserve.Text = "Observe";
            this.useExtObserve.UseVisualStyleBackColor = true;
            // 
            // useExtStopImg
            // 
            this.useExtStopImg.AutoSize = true;
            this.useExtStopImg.Location = new System.Drawing.Point(6, 63);
            this.useExtStopImg.Name = "useExtStopImg";
            this.useExtStopImg.Size = new System.Drawing.Size(84, 16);
            this.useExtStopImg.TabIndex = 15;
            this.useExtStopImg.Text = "StopImage";
            this.useExtStopImg.UseVisualStyleBackColor = true;
            // 
            // useExtTransform
            // 
            this.useExtTransform.AutoSize = true;
            this.useExtTransform.Location = new System.Drawing.Point(6, 107);
            this.useExtTransform.Name = "useExtTransform";
            this.useExtTransform.Size = new System.Drawing.Size(82, 16);
            this.useExtTransform.TabIndex = 15;
            this.useExtTransform.Text = "Transform";
            this.useExtTransform.UseVisualStyleBackColor = true;
            // 
            // useExtMargin
            // 
            this.useExtMargin.AutoSize = true;
            this.useExtMargin.Location = new System.Drawing.Point(6, 85);
            this.useExtMargin.Name = "useExtMargin";
            this.useExtMargin.Size = new System.Drawing.Size(63, 16);
            this.useExtMargin.TabIndex = 15;
            this.useExtMargin.Text = "Margin";
            this.useExtMargin.UseVisualStyleBackColor = true;
            // 
            // splitExactPattern
            // 
            this.splitExactPattern.AutoSize = true;
            this.splitExactPattern.Location = new System.Drawing.Point(10, 117);
            this.splitExactPattern.Name = "splitExactPattern";
            this.splitExactPattern.Size = new System.Drawing.Size(127, 16);
            this.splitExactPattern.TabIndex = 18;
            this.splitExactPattern.Text = "Split Exact Pattern";
            this.splitExactPattern.UseVisualStyleBackColor = true;
            // 
            // InspectorSystemSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxExtFunction);
            this.Controls.Add(this.groupBoxInspectorInfo);
            this.Controls.Add(this.groupBoxAlgorithmVersion);
            this.Name = "InspectorSystemSettingPanel";
            this.Size = new System.Drawing.Size(469, 314);
            this.Load += new System.EventHandler(this.InspectorSystemSettingPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).EndInit();
            this.groupBoxAlgorithmVersion.ResumeLayout(false);
            this.groupBoxAlgorithmVersion.PerformLayout();
            this.groupBoxInspectorInfo.ResumeLayout(false);
            this.groupBoxInspectorInfo.PerformLayout();
            this.groupBoxExtFunction.ResumeLayout(false);
            this.groupBoxExtFunction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelClientIndex;
        private System.Windows.Forms.NumericUpDown clientIndex;
        private System.Windows.Forms.Label labelCamIndex;
        private System.Windows.Forms.NumericUpDown camIndex;
        private System.Windows.Forms.ComboBox calculatorVersion;
        private System.Windows.Forms.Label labelCalculator;
        private System.Windows.Forms.Label labelTrainer;
        private System.Windows.Forms.ComboBox trainerVersion;
        private System.Windows.Forms.GroupBox groupBoxAlgorithmVersion;
        private System.Windows.Forms.GroupBox groupBoxInspectorInfo;
        private System.Windows.Forms.Label ipAddress;
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.GroupBox groupBoxExtFunction;
        private System.Windows.Forms.CheckBox useExtStopImg;
        private System.Windows.Forms.CheckBox useExtTransform;
        private System.Windows.Forms.CheckBox useExtMargin;
        private System.Windows.Forms.ComboBox detectorVersion;
        private System.Windows.Forms.Label labelDetectorVersion;
        private System.Windows.Forms.CheckBox useExtObserve;
        private System.Windows.Forms.CheckBox useExtSticker;
        private System.Windows.Forms.ComboBox sheetFindVersion;
        private System.Windows.Forms.Label labelSheetFind;
        private System.Windows.Forms.CheckBox splitExactPattern;
    }
}
