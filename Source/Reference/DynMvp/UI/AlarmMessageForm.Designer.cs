namespace DynMvp.UI
{
    partial class AlarmMessageForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SettingPage_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.errorSection = new System.Windows.Forms.Label();
            this.errorSubSection = new System.Windows.Forms.Label();
            this.messsage = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.errorLevel = new System.Windows.Forms.Label();
            this.labelErrorLevel = new System.Windows.Forms.Label();
            this.errorCode = new System.Windows.Forms.Label();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelErrorName = new System.Windows.Forms.Label();
            this.errorName = new System.Windows.Forms.Label();
            this.labelErrorSection = new System.Windows.Forms.Label();
            this.labelErrorTime = new System.Windows.Forms.Label();
            this.errorTime = new System.Windows.Forms.Label();
            this.imageIcon = new System.Windows.Forms.PictureBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonNextError = new System.Windows.Forms.Button();
            this.buttonPrevError = new System.Windows.Forms.Button();
            this.buttonAlarmOff = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelAlarm = new System.Windows.Forms.Label();
            this.checkSaveTargetImage = new System.Windows.Forms.CheckBox();
            this.errorCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.SettingPage_Fill_Panel.ClientArea.SuspendLayout();
            this.SettingPage_Fill_Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingPage_Fill_Panel
            // 
            // 
            // SettingPage_Fill_Panel.ClientArea
            // 
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panelBottom);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panelTop);
            this.SettingPage_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SettingPage_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingPage_Fill_Panel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SettingPage_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.SettingPage_Fill_Panel.Name = "SettingPage_Fill_Panel";
            this.SettingPage_Fill_Panel.Size = new System.Drawing.Size(625, 393);
            this.SettingPage_Fill_Panel.TabIndex = 161;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.imageIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 291);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.messsage, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelMessage, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.errorLevel, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorLevel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.errorCode, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelCode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorName, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.errorName, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorSection, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorTime, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.errorTime, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(93, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(532, 291);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.errorSection, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.errorSubSection, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(140, 84);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(392, 42);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // errorSection
            // 
            this.errorSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorSection.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorSection.Location = new System.Drawing.Point(2, 2);
            this.errorSection.Margin = new System.Windows.Forms.Padding(2);
            this.errorSection.Name = "errorSection";
            this.errorSection.Size = new System.Drawing.Size(192, 38);
            this.errorSection.TabIndex = 2;
            this.errorSection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorSubSection
            // 
            this.errorSubSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorSubSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorSubSection.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorSubSection.Location = new System.Drawing.Point(198, 2);
            this.errorSubSection.Margin = new System.Windows.Forms.Padding(2);
            this.errorSubSection.Name = "errorSubSection";
            this.errorSubSection.Size = new System.Drawing.Size(192, 38);
            this.errorSubSection.TabIndex = 2;
            this.errorSubSection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // messsage
            // 
            this.messsage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.messsage, 3);
            this.messsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messsage.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.messsage.Location = new System.Drawing.Point(142, 170);
            this.messsage.Margin = new System.Windows.Forms.Padding(2);
            this.messsage.Name = "messsage";
            this.messsage.Size = new System.Drawing.Size(388, 119);
            this.messsage.TabIndex = 2;
            this.messsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMessage.Location = new System.Drawing.Point(2, 170);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(2);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(136, 119);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorLevel
            // 
            this.errorLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorLevel.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorLevel.Location = new System.Drawing.Point(408, 2);
            this.errorLevel.Margin = new System.Windows.Forms.Padding(2);
            this.errorLevel.Name = "errorLevel";
            this.errorLevel.Size = new System.Drawing.Size(122, 38);
            this.errorLevel.TabIndex = 2;
            this.errorLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErrorLevel
            // 
            this.labelErrorLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelErrorLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelErrorLevel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorLevel.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErrorLevel.Location = new System.Drawing.Point(268, 2);
            this.labelErrorLevel.Margin = new System.Windows.Forms.Padding(2);
            this.labelErrorLevel.Name = "labelErrorLevel";
            this.labelErrorLevel.Size = new System.Drawing.Size(136, 38);
            this.labelErrorLevel.TabIndex = 2;
            this.labelErrorLevel.Text = "Error Level";
            this.labelErrorLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorCode
            // 
            this.errorCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorCode.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorCode.Location = new System.Drawing.Point(142, 2);
            this.errorCode.Margin = new System.Windows.Forms.Padding(2);
            this.errorCode.Name = "errorCode";
            this.errorCode.Size = new System.Drawing.Size(122, 38);
            this.errorCode.TabIndex = 2;
            this.errorCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCode
            // 
            this.labelCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCode.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCode.Location = new System.Drawing.Point(2, 2);
            this.labelCode.Margin = new System.Windows.Forms.Padding(2);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(136, 38);
            this.labelCode.TabIndex = 2;
            this.labelCode.Text = "Code";
            this.labelCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErrorName
            // 
            this.labelErrorName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelErrorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelErrorName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorName.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErrorName.Location = new System.Drawing.Point(2, 128);
            this.labelErrorName.Margin = new System.Windows.Forms.Padding(2);
            this.labelErrorName.Name = "labelErrorName";
            this.labelErrorName.Size = new System.Drawing.Size(136, 38);
            this.labelErrorName.TabIndex = 2;
            this.labelErrorName.Text = "Name";
            this.labelErrorName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorName
            // 
            this.errorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.errorName, 3);
            this.errorName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorName.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorName.Location = new System.Drawing.Point(142, 128);
            this.errorName.Margin = new System.Windows.Forms.Padding(2);
            this.errorName.Name = "errorName";
            this.errorName.Size = new System.Drawing.Size(388, 38);
            this.errorName.TabIndex = 2;
            this.errorName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErrorSection
            // 
            this.labelErrorSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelErrorSection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelErrorSection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorSection.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErrorSection.Location = new System.Drawing.Point(2, 86);
            this.labelErrorSection.Margin = new System.Windows.Forms.Padding(2);
            this.labelErrorSection.Name = "labelErrorSection";
            this.labelErrorSection.Size = new System.Drawing.Size(136, 38);
            this.labelErrorSection.TabIndex = 2;
            this.labelErrorSection.Text = "Section";
            this.labelErrorSection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelErrorTime
            // 
            this.labelErrorTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelErrorTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelErrorTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorTime.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErrorTime.Location = new System.Drawing.Point(2, 44);
            this.labelErrorTime.Margin = new System.Windows.Forms.Padding(2);
            this.labelErrorTime.Name = "labelErrorTime";
            this.labelErrorTime.Size = new System.Drawing.Size(136, 38);
            this.labelErrorTime.TabIndex = 2;
            this.labelErrorTime.Text = "Time";
            this.labelErrorTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // errorTime
            // 
            this.errorTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.errorTime, 3);
            this.errorTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorTime.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorTime.Location = new System.Drawing.Point(142, 44);
            this.errorTime.Margin = new System.Windows.Forms.Padding(2);
            this.errorTime.Name = "errorTime";
            this.errorTime.Size = new System.Drawing.Size(388, 38);
            this.errorTime.TabIndex = 2;
            this.errorTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageIcon
            // 
            this.imageIcon.BackColor = System.Drawing.Color.Transparent;
            this.imageIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.imageIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageIcon.Location = new System.Drawing.Point(0, 0);
            this.imageIcon.Name = "imageIcon";
            this.imageIcon.Size = new System.Drawing.Size(93, 291);
            this.imageIcon.TabIndex = 0;
            this.imageIcon.TabStop = false;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelBottom.Controls.Add(this.buttonStop);
            this.panelBottom.Controls.Add(this.buttonReset);
            this.panelBottom.Controls.Add(this.buttonNextError);
            this.panelBottom.Controls.Add(this.buttonPrevError);
            this.panelBottom.Controls.Add(this.buttonAlarmOff);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 339);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(625, 54);
            this.panelBottom.TabIndex = 2;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(424, 6);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(95, 38);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(523, 6);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(95, 38);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonNextError
            // 
            this.buttonNextError.Location = new System.Drawing.Point(231, 6);
            this.buttonNextError.Name = "buttonNextError";
            this.buttonNextError.Size = new System.Drawing.Size(95, 38);
            this.buttonNextError.TabIndex = 0;
            this.buttonNextError.Text = "Next";
            this.buttonNextError.UseVisualStyleBackColor = true;
            this.buttonNextError.Click += new System.EventHandler(this.buttonNextError_Click);
            // 
            // buttonPrevError
            // 
            this.buttonPrevError.Location = new System.Drawing.Point(134, 6);
            this.buttonPrevError.Name = "buttonPrevError";
            this.buttonPrevError.Size = new System.Drawing.Size(95, 38);
            this.buttonPrevError.TabIndex = 0;
            this.buttonPrevError.Text = "Prev";
            this.buttonPrevError.UseVisualStyleBackColor = true;
            this.buttonPrevError.Click += new System.EventHandler(this.buttonPrevError_Click);
            // 
            // buttonAlarmOff
            // 
            this.buttonAlarmOff.Location = new System.Drawing.Point(12, 6);
            this.buttonAlarmOff.Name = "buttonAlarmOff";
            this.buttonAlarmOff.Size = new System.Drawing.Size(120, 38);
            this.buttonAlarmOff.TabIndex = 0;
            this.buttonAlarmOff.Text = "Alarm Off";
            this.buttonAlarmOff.UseVisualStyleBackColor = true;
            this.buttonAlarmOff.Click += new System.EventHandler(this.buttonAlarmOff_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelTop.Controls.Add(this.labelAlarm);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(625, 48);
            this.panelTop.TabIndex = 0;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseMove);
            // 
            // labelAlarm
            // 
            this.labelAlarm.AutoSize = true;
            this.labelAlarm.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAlarm.ForeColor = System.Drawing.Color.White;
            this.labelAlarm.Location = new System.Drawing.Point(12, 9);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(72, 30);
            this.labelAlarm.TabIndex = 0;
            this.labelAlarm.Text = "Alarm";
            // 
            // checkSaveTargetImage
            // 
            this.checkSaveTargetImage.AutoSize = true;
            this.checkSaveTargetImage.Location = new System.Drawing.Point(15, 188);
            this.checkSaveTargetImage.Name = "checkSaveTargetImage";
            this.checkSaveTargetImage.Size = new System.Drawing.Size(157, 24);
            this.checkSaveTargetImage.TabIndex = 160;
            this.checkSaveTargetImage.Text = "Save target image";
            this.checkSaveTargetImage.UseVisualStyleBackColor = true;
            // 
            // errorCheckTimer
            // 
            this.errorCheckTimer.Tick += new System.EventHandler(this.errorCheckTimer_Tick);
            // 
            // AlarmMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 393);
            this.Controls.Add(this.SettingPage_Fill_Panel);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmMessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AlarmMessageForm_Load);
            this.SettingPage_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SettingPage_Fill_Panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageIcon)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraPanel SettingPage_Fill_Panel;
        private System.Windows.Forms.CheckBox checkSaveTargetImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox imageIcon;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonAlarmOff;
        private System.Windows.Forms.Label messsage;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Label errorCode;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label errorLevel;
        private System.Windows.Forms.Label labelErrorLevel;
        private System.Windows.Forms.Button buttonPrevError;
        private System.Windows.Forms.Button buttonNextError;
        private System.Windows.Forms.Timer errorCheckTimer;
        private System.Windows.Forms.Label labelAlarm;
        private System.Windows.Forms.Label errorName;
        private System.Windows.Forms.Label labelErrorName;
        private System.Windows.Forms.Label errorSection;
        private System.Windows.Forms.Label labelErrorSection;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelErrorTime;
        private System.Windows.Forms.Label errorTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label errorSubSection;
    }
}
