namespace UniScanM.Pinhole.UI
{
    partial class InspectionPanelRight
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
            this.process1 = new System.Diagnostics.Process();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.groupModelSettings = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.cmbNGType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupCam2 = new System.Windows.Forms.GroupBox();
            this.labelEdgeThreshold2 = new System.Windows.Forms.Label();
            this.numEdgeThreshold2 = new System.Windows.Forms.NumericUpDown();
            this.labelDefectThreshold2 = new System.Windows.Forms.Label();
            this.numDefectThreshold2 = new System.Windows.Forms.NumericUpDown();
            this.groupCam1 = new System.Windows.Forms.GroupBox();
            this.labelEdgeThreshold1 = new System.Windows.Forms.Label();
            this.labelDefectThreshold1 = new System.Windows.Forms.Label();
            this.numEdgeThreshold1 = new System.Windows.Forms.NumericUpDown();
            this.numDefectThreshold1 = new System.Windows.Forms.NumericUpDown();
            this.buttonApply = new System.Windows.Forms.Button();
            this.numSheetLenght = new System.Windows.Forms.NumericUpDown();
            this.NGType = new System.Windows.Forms.Label();
            this.labelSheetLength = new System.Windows.Forms.Label();
            this.panelParamInfo = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelLight = new System.Windows.Forms.Label();
            this.labelPercent = new System.Windows.Forms.Label();
            this.labelRealValue = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPassWidth = new System.Windows.Forms.Label();
            this.labelPassHeight = new System.Windows.Forms.Label();
            this.labelPassSize = new System.Windows.Forms.Label();
            this.labelPassSizeWidth = new System.Windows.Forms.Label();
            this.labelPassSizeHeight = new System.Windows.Forms.Label();
            this.panelDefectMap = new System.Windows.Forms.Panel();
            this.checkOnTune = new System.Windows.Forms.CheckBox();
            this.panelStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupModelSettings)).BeginInit();
            this.groupModelSettings.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            this.groupCam2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeThreshold2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefectThreshold2)).BeginInit();
            this.groupCam1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeThreshold1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefectThreshold1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSheetLenght)).BeginInit();
            this.panelParamInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // process1
            // 
            this.process1.StartInfo.Domain = "";
            this.process1.StartInfo.LoadUserProfile = false;
            this.process1.StartInfo.Password = null;
            this.process1.StartInfo.StandardErrorEncoding = null;
            this.process1.StartInfo.StandardOutputEncoding = null;
            this.process1.StartInfo.UserName = "";
            this.process1.SynchronizingObject = this;
            this.process1.Exited += new System.EventHandler(this.process1_Exited);
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.groupModelSettings);
            this.panelStatus.Controls.Add(this.panelParamInfo);
            this.panelStatus.Controls.Add(this.panelDefectMap);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStatus.Location = new System.Drawing.Point(0, 0);
            this.panelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(631, 1068);
            this.panelStatus.TabIndex = 2;
            this.panelStatus.VisibleChanged += new System.EventHandler(this.panelStatus_VisibleChanged);
            // 
            // groupModelSettings
            // 
            this.groupModelSettings.ContentPadding.Top = 30;
            this.groupModelSettings.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.groupModelSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupModelSettings.Enabled = false;
            this.groupModelSettings.ExpandedSize = new System.Drawing.Size(631, 730);
            this.groupModelSettings.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupModelSettings.Location = new System.Drawing.Point(0, 338);
            this.groupModelSettings.Margin = new System.Windows.Forms.Padding(6, 0, 6, 6);
            this.groupModelSettings.Name = "groupModelSettings";
            this.groupModelSettings.Size = new System.Drawing.Size(631, 730);
            this.groupModelSettings.TabIndex = 7;
            this.groupModelSettings.Text = "Model Settings";
            this.groupModelSettings.ExpandedStateChanging += new System.ComponentModel.CancelEventHandler(this.groupModelSettings_ExpandedStateChanging);
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.cmbNGType);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label3);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label2);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.groupCam2);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.groupCam1);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.buttonApply);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.numSheetLenght);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.NGType);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.labelSheetLength);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(3, 62);
            this.ultraExpandableGroupBoxPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(625, 665);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // cmbNGType
            // 
            this.cmbNGType.FormattingEnabled = true;
            this.cmbNGType.Items.AddRange(new object[] {
            "Any",
            "Bright",
            "Dark"});
            this.cmbNGType.Location = new System.Drawing.Point(338, 70);
            this.cmbNGType.Name = "cmbNGType";
            this.cmbNGType.Size = new System.Drawing.Size(139, 34);
            this.cmbNGType.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, -32);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(178, 26);
            this.label3.TabIndex = 15;
            this.label3.Text = "Skip length :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(488, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 26);
            this.label2.TabIndex = 14;
            this.label2.Text = "mm";
            // 
            // groupCam2
            // 
            this.groupCam2.Controls.Add(this.labelEdgeThreshold2);
            this.groupCam2.Controls.Add(this.numEdgeThreshold2);
            this.groupCam2.Controls.Add(this.labelDefectThreshold2);
            this.groupCam2.Controls.Add(this.numDefectThreshold2);
            this.groupCam2.Location = new System.Drawing.Point(13, 313);
            this.groupCam2.Margin = new System.Windows.Forms.Padding(6);
            this.groupCam2.Name = "groupCam2";
            this.groupCam2.Padding = new System.Windows.Forms.Padding(6);
            this.groupCam2.Size = new System.Drawing.Size(479, 188);
            this.groupCam2.TabIndex = 12;
            this.groupCam2.TabStop = false;
            this.groupCam2.Text = "Camera 2 Parameter";
            // 
            // labelEdgeThreshold2
            // 
            this.labelEdgeThreshold2.AutoSize = true;
            this.labelEdgeThreshold2.Location = new System.Drawing.Point(11, 62);
            this.labelEdgeThreshold2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEdgeThreshold2.Name = "labelEdgeThreshold2";
            this.labelEdgeThreshold2.Size = new System.Drawing.Size(219, 26);
            this.labelEdgeThreshold2.TabIndex = 11;
            this.labelEdgeThreshold2.Text = "Edge Threshold";
            // 
            // numEdgeThreshold2
            // 
            this.numEdgeThreshold2.Location = new System.Drawing.Point(319, 58);
            this.numEdgeThreshold2.Margin = new System.Windows.Forms.Padding(6);
            this.numEdgeThreshold2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numEdgeThreshold2.Name = "numEdgeThreshold2";
            this.numEdgeThreshold2.Size = new System.Drawing.Size(139, 37);
            this.numEdgeThreshold2.TabIndex = 12;
            this.numEdgeThreshold2.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // labelDefectThreshold2
            // 
            this.labelDefectThreshold2.AutoSize = true;
            this.labelDefectThreshold2.Location = new System.Drawing.Point(11, 120);
            this.labelDefectThreshold2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDefectThreshold2.Name = "labelDefectThreshold2";
            this.labelDefectThreshold2.Size = new System.Drawing.Size(239, 26);
            this.labelDefectThreshold2.TabIndex = 2;
            this.labelDefectThreshold2.Text = "Defect Threshold";
            // 
            // numDefectThreshold2
            // 
            this.numDefectThreshold2.Location = new System.Drawing.Point(319, 116);
            this.numDefectThreshold2.Margin = new System.Windows.Forms.Padding(6);
            this.numDefectThreshold2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDefectThreshold2.Name = "numDefectThreshold2";
            this.numDefectThreshold2.Size = new System.Drawing.Size(139, 37);
            this.numDefectThreshold2.TabIndex = 9;
            this.numDefectThreshold2.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // groupCam1
            // 
            this.groupCam1.Controls.Add(this.labelEdgeThreshold1);
            this.groupCam1.Controls.Add(this.labelDefectThreshold1);
            this.groupCam1.Controls.Add(this.numEdgeThreshold1);
            this.groupCam1.Controls.Add(this.numDefectThreshold1);
            this.groupCam1.Location = new System.Drawing.Point(13, 113);
            this.groupCam1.Margin = new System.Windows.Forms.Padding(6);
            this.groupCam1.Name = "groupCam1";
            this.groupCam1.Padding = new System.Windows.Forms.Padding(6);
            this.groupCam1.Size = new System.Drawing.Size(479, 188);
            this.groupCam1.TabIndex = 11;
            this.groupCam1.TabStop = false;
            this.groupCam1.Text = "Camera 1 Parameter";
            // 
            // labelEdgeThreshold1
            // 
            this.labelEdgeThreshold1.AutoSize = true;
            this.labelEdgeThreshold1.Location = new System.Drawing.Point(11, 72);
            this.labelEdgeThreshold1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEdgeThreshold1.Name = "labelEdgeThreshold1";
            this.labelEdgeThreshold1.Size = new System.Drawing.Size(219, 26);
            this.labelEdgeThreshold1.TabIndex = 13;
            this.labelEdgeThreshold1.Text = "Edge Threshold";
            // 
            // labelDefectThreshold1
            // 
            this.labelDefectThreshold1.AutoSize = true;
            this.labelDefectThreshold1.Location = new System.Drawing.Point(11, 128);
            this.labelDefectThreshold1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDefectThreshold1.Name = "labelDefectThreshold1";
            this.labelDefectThreshold1.Size = new System.Drawing.Size(239, 26);
            this.labelDefectThreshold1.TabIndex = 2;
            this.labelDefectThreshold1.Text = "Defect Threshold";
            // 
            // numEdgeThreshold1
            // 
            this.numEdgeThreshold1.Location = new System.Drawing.Point(319, 68);
            this.numEdgeThreshold1.Margin = new System.Windows.Forms.Padding(6);
            this.numEdgeThreshold1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numEdgeThreshold1.Name = "numEdgeThreshold1";
            this.numEdgeThreshold1.Size = new System.Drawing.Size(139, 37);
            this.numEdgeThreshold1.TabIndex = 14;
            this.numEdgeThreshold1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numDefectThreshold1
            // 
            this.numDefectThreshold1.Location = new System.Drawing.Point(319, 124);
            this.numDefectThreshold1.Margin = new System.Windows.Forms.Padding(6);
            this.numDefectThreshold1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numDefectThreshold1.Name = "numDefectThreshold1";
            this.numDefectThreshold1.Size = new System.Drawing.Size(139, 37);
            this.numDefectThreshold1.TabIndex = 9;
            this.numDefectThreshold1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(19, 539);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(6);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(479, 72);
            this.buttonApply.TabIndex = 0;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // numSheetLenght
            // 
            this.numSheetLenght.DecimalPlaces = 1;
            this.numSheetLenght.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSheetLenght.Location = new System.Drawing.Point(338, 20);
            this.numSheetLenght.Margin = new System.Windows.Forms.Padding(6);
            this.numSheetLenght.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSheetLenght.Name = "numSheetLenght";
            this.numSheetLenght.Size = new System.Drawing.Size(139, 37);
            this.numSheetLenght.TabIndex = 6;
            this.numSheetLenght.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // NGType
            // 
            this.NGType.AutoSize = true;
            this.NGType.Location = new System.Drawing.Point(14, 70);
            this.NGType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.NGType.Name = "NGType";
            this.NGType.Size = new System.Drawing.Size(145, 26);
            this.NGType.TabIndex = 1;
            this.NGType.Text = "NG Type :";
            // 
            // labelSheetLength
            // 
            this.labelSheetLength.AutoSize = true;
            this.labelSheetLength.Location = new System.Drawing.Point(13, 22);
            this.labelSheetLength.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelSheetLength.Name = "labelSheetLength";
            this.labelSheetLength.Size = new System.Drawing.Size(178, 26);
            this.labelSheetLength.TabIndex = 1;
            this.labelSheetLength.Text = "Skip length :";
            // 
            // panelParamInfo
            // 
            this.panelParamInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelParamInfo.Controls.Add(this.panel2);
            this.panelParamInfo.Controls.Add(this.panel1);
            this.panelParamInfo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panelParamInfo.Location = new System.Drawing.Point(6, 392);
            this.panelParamInfo.Margin = new System.Windows.Forms.Padding(6);
            this.panelParamInfo.Name = "panelParamInfo";
            this.panelParamInfo.Size = new System.Drawing.Size(626, 312);
            this.panelParamInfo.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Linen;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.labelLight);
            this.panel2.Controls.Add(this.labelPercent);
            this.panel2.Controls.Add(this.labelRealValue);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel2.Location = new System.Drawing.Point(0, 88);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(626, 88);
            this.panel2.TabIndex = 75;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(310, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 45);
            this.label1.TabIndex = 72;
            this.label1.Text = "%";
            // 
            // labelLight
            // 
            this.labelLight.AutoSize = true;
            this.labelLight.Location = new System.Drawing.Point(6, 22);
            this.labelLight.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLight.Name = "labelLight";
            this.labelLight.Size = new System.Drawing.Size(96, 45);
            this.labelLight.TabIndex = 2;
            this.labelLight.Text = "Light";
            // 
            // labelPercent
            // 
            this.labelPercent.AutoSize = true;
            this.labelPercent.Location = new System.Drawing.Point(247, 22);
            this.labelPercent.Margin = new System.Windows.Forms.Padding(0);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(39, 45);
            this.labelPercent.TabIndex = 68;
            this.labelPercent.Text = "5";
            // 
            // labelRealValue
            // 
            this.labelRealValue.AutoSize = true;
            this.labelRealValue.Location = new System.Drawing.Point(355, 22);
            this.labelRealValue.Margin = new System.Windows.Forms.Padding(0);
            this.labelRealValue.Name = "labelRealValue";
            this.labelRealValue.Size = new System.Drawing.Size(80, 45);
            this.labelRealValue.TabIndex = 71;
            this.labelRealValue.Text = "(20)";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Linen;
            this.panel1.Controls.Add(this.labelPassWidth);
            this.panel1.Controls.Add(this.labelPassHeight);
            this.panel1.Controls.Add(this.labelPassSize);
            this.panel1.Controls.Add(this.labelPassSizeWidth);
            this.panel1.Controls.Add(this.labelPassSizeHeight);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 88);
            this.panel1.TabIndex = 74;
            // 
            // labelPassWidth
            // 
            this.labelPassWidth.AutoSize = true;
            this.labelPassWidth.Location = new System.Drawing.Point(295, 22);
            this.labelPassWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassWidth.Name = "labelPassWidth";
            this.labelPassWidth.Size = new System.Drawing.Size(66, 45);
            this.labelPassWidth.TabIndex = 72;
            this.labelPassWidth.Text = "3.0";
            // 
            // labelPassHeight
            // 
            this.labelPassHeight.AutoSize = true;
            this.labelPassHeight.Location = new System.Drawing.Point(518, 22);
            this.labelPassHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassHeight.Name = "labelPassHeight";
            this.labelPassHeight.Size = new System.Drawing.Size(66, 45);
            this.labelPassHeight.TabIndex = 73;
            this.labelPassHeight.Text = "3.0";
            // 
            // labelPassSize
            // 
            this.labelPassSize.AutoSize = true;
            this.labelPassSize.Location = new System.Drawing.Point(0, 22);
            this.labelPassSize.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelPassSize.Name = "labelPassSize";
            this.labelPassSize.Size = new System.Drawing.Size(174, 45);
            this.labelPassSize.TabIndex = 2;
            this.labelPassSize.Text = "Pass size :";
            // 
            // labelPassSizeWidth
            // 
            this.labelPassSizeWidth.AutoSize = true;
            this.labelPassSizeWidth.Location = new System.Drawing.Point(247, 22);
            this.labelPassSizeWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassSizeWidth.Name = "labelPassSizeWidth";
            this.labelPassSizeWidth.Size = new System.Drawing.Size(52, 45);
            this.labelPassSizeWidth.TabIndex = 68;
            this.labelPassSizeWidth.Text = "W";
            // 
            // labelPassSizeHeight
            // 
            this.labelPassSizeHeight.AutoSize = true;
            this.labelPassSizeHeight.Location = new System.Drawing.Point(477, 22);
            this.labelPassSizeHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelPassSizeHeight.Name = "labelPassSizeHeight";
            this.labelPassSizeHeight.Size = new System.Drawing.Size(45, 45);
            this.labelPassSizeHeight.TabIndex = 71;
            this.labelPassSizeHeight.Text = "H";
            // 
            // panelDefectMap
            // 
            this.panelDefectMap.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDefectMap.Location = new System.Drawing.Point(0, 0);
            this.panelDefectMap.Margin = new System.Windows.Forms.Padding(0);
            this.panelDefectMap.Name = "panelDefectMap";
            this.panelDefectMap.Size = new System.Drawing.Size(631, 338);
            this.panelDefectMap.TabIndex = 3;
            // 
            // checkOnTune
            // 
            this.checkOnTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkOnTune.BackColor = System.Drawing.Color.Gray;
            this.checkOnTune.Checked = true;
            this.checkOnTune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnTune.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkOnTune.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.checkOnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkOnTune.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkOnTune.Location = new System.Drawing.Point(0, 1068);
            this.checkOnTune.Margin = new System.Windows.Forms.Padding(6);
            this.checkOnTune.Name = "checkOnTune";
            this.checkOnTune.Size = new System.Drawing.Size(631, 100);
            this.checkOnTune.TabIndex = 9;
            this.checkOnTune.Text = "Comm Open/Close";
            this.checkOnTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkOnTune.UseVisualStyleBackColor = false;
            this.checkOnTune.CheckedChanged += new System.EventHandler(this.checkOnTune_CheckedChanged);
            // 
            // InspectionPanelRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.checkOnTune);
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Name = "InspectionPanelRight";
            this.Size = new System.Drawing.Size(631, 1168);
            this.panelStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupModelSettings)).EndInit();
            this.groupModelSettings.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            this.groupCam2.ResumeLayout(false);
            this.groupCam2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeThreshold2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefectThreshold2)).EndInit();
            this.groupCam1.ResumeLayout(false);
            this.groupCam1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEdgeThreshold1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefectThreshold1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSheetLenght)).EndInit();
            this.panelParamInfo.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.Panel panelStatus;
        private Infragistics.Win.Misc.UltraExpandableGroupBox groupModelSettings;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupCam2;
        private System.Windows.Forms.Label labelEdgeThreshold2;
        private System.Windows.Forms.NumericUpDown numEdgeThreshold2;
        private System.Windows.Forms.Label labelDefectThreshold2;
        private System.Windows.Forms.NumericUpDown numDefectThreshold2;
        private System.Windows.Forms.GroupBox groupCam1;
        private System.Windows.Forms.Label labelEdgeThreshold1;
        private System.Windows.Forms.Label labelDefectThreshold1;
        private System.Windows.Forms.NumericUpDown numEdgeThreshold1;
        private System.Windows.Forms.NumericUpDown numDefectThreshold1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.NumericUpDown numSheetLenght;
        private System.Windows.Forms.Label labelSheetLength;
        private System.Windows.Forms.Panel panelParamInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelLight;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.Label labelRealValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelPassWidth;
        private System.Windows.Forms.Label labelPassHeight;
        private System.Windows.Forms.Label labelPassSize;
        private System.Windows.Forms.Label labelPassSizeWidth;
        private System.Windows.Forms.Label labelPassSizeHeight;
        private System.Windows.Forms.Panel panelDefectMap;
        private System.Windows.Forms.CheckBox checkOnTune;
        private System.Windows.Forms.ComboBox cmbNGType;
        private System.Windows.Forms.Label NGType;
    }
}
