namespace UniScanG.Gravure.UI.Setting
{
    partial class SettingPage
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
            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelImageCheckInterval = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelMs = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.buttonCollectLog = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonNavigateGeneral = new System.Windows.Forms.Button();
            this.buttonNavigateComm = new System.Windows.Forms.Button();
            this.buttonNavigateAlarm = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonNavigateGrade = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
            this.panelMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageCheckInterval
            // 
            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
            this.imageCheckInterval.Name = "imageCheckInterval";
            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.imageCheckInterval.TabIndex = 0;
            // 
            // labelImageCheckInterval
            // 
            this.labelImageCheckInterval.AutoSize = true;
            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
            this.labelImageCheckInterval.TabIndex = 0;
            this.labelImageCheckInterval.Text = "Image Check Interval";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(286, 126);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(29, 17);
            this.labelSec.TabIndex = 0;
            this.labelSec.Text = "sec";
            // 
            // labelMs
            // 
            this.labelMs.Location = new System.Drawing.Point(0, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(100, 23);
            this.labelMs.TabIndex = 0;
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.buttonCollectLog);
            this.panelMenu.Controls.Add(this.buttonSave);
            this.panelMenu.Controls.Add(this.buttonNavigateGeneral);
            this.panelMenu.Controls.Add(this.buttonNavigateGrade);
            this.panelMenu.Controls.Add(this.buttonNavigateComm);
            this.panelMenu.Controls.Add(this.buttonNavigateAlarm);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.Location = new System.Drawing.Point(3, 3);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(144, 576);
            this.panelMenu.TabIndex = 0;
            // 
            // buttonCollectLog
            // 
            this.buttonCollectLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCollectLog.Location = new System.Drawing.Point(0, 466);
            this.buttonCollectLog.Margin = new System.Windows.Forms.Padding(5);
            this.buttonCollectLog.Name = "buttonCollectLog";
            this.buttonCollectLog.Size = new System.Drawing.Size(144, 55);
            this.buttonCollectLog.TabIndex = 4;
            this.buttonCollectLog.Text = "Collect Log";
            this.buttonCollectLog.UseVisualStyleBackColor = true;
            this.buttonCollectLog.Click += new System.EventHandler(this.buttonCollectLog_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSave.Location = new System.Drawing.Point(0, 521);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(144, 55);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonNavigateGeneral
            // 
            this.buttonNavigateGeneral.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonNavigateGeneral.Location = new System.Drawing.Point(0, 165);
            this.buttonNavigateGeneral.Margin = new System.Windows.Forms.Padding(5);
            this.buttonNavigateGeneral.Name = "buttonNavigateGeneral";
            this.buttonNavigateGeneral.Size = new System.Drawing.Size(144, 55);
            this.buttonNavigateGeneral.TabIndex = 2;
            this.buttonNavigateGeneral.Text = "General";
            this.buttonNavigateGeneral.UseVisualStyleBackColor = true;
            this.buttonNavigateGeneral.Click += new System.EventHandler(this.buttonNavigate_Click);
            // 
            // buttonNavigateComm
            // 
            this.buttonNavigateComm.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonNavigateComm.Location = new System.Drawing.Point(0, 55);
            this.buttonNavigateComm.Margin = new System.Windows.Forms.Padding(5);
            this.buttonNavigateComm.Name = "buttonNavigateComm";
            this.buttonNavigateComm.Size = new System.Drawing.Size(144, 55);
            this.buttonNavigateComm.TabIndex = 2;
            this.buttonNavigateComm.Text = "Comm";
            this.buttonNavigateComm.UseVisualStyleBackColor = true;
            this.buttonNavigateComm.Click += new System.EventHandler(this.buttonNavigate_Click);
            // 
            // buttonNavigateAlarm
            // 
            this.buttonNavigateAlarm.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonNavigateAlarm.Location = new System.Drawing.Point(0, 0);
            this.buttonNavigateAlarm.Margin = new System.Windows.Forms.Padding(5);
            this.buttonNavigateAlarm.Name = "buttonNavigateAlarm";
            this.buttonNavigateAlarm.Size = new System.Drawing.Size(144, 55);
            this.buttonNavigateAlarm.TabIndex = 2;
            this.buttonNavigateAlarm.Text = "Alarm";
            this.buttonNavigateAlarm.UseVisualStyleBackColor = true;
            this.buttonNavigateAlarm.Click += new System.EventHandler(this.buttonNavigate_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelMenu, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelMain, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1442, 582);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(153, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1286, 576);
            this.panelMain.TabIndex = 1;
            // 
            // buttonNavigateGrade
            // 
            this.buttonNavigateGrade.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonNavigateGrade.Location = new System.Drawing.Point(0, 110);
            this.buttonNavigateGrade.Margin = new System.Windows.Forms.Padding(5);
            this.buttonNavigateGrade.Name = "buttonNavigateGrade";
            this.buttonNavigateGrade.Size = new System.Drawing.Size(144, 55);
            this.buttonNavigateGrade.TabIndex = 5;
            this.buttonNavigateGrade.Text = "Grade";
            this.buttonNavigateGrade.UseVisualStyleBackColor = true;
            this.buttonNavigateGrade.Click += new System.EventHandler(this.buttonNavigate_Click);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(1442, 582);
            this.Load += new System.EventHandler(this.SettingPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
            this.panelMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown imageCheckInterval;
        private System.Windows.Forms.Label labelImageCheckInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Button buttonNavigateGeneral;
        private System.Windows.Forms.Button buttonNavigateComm;
        private System.Windows.Forms.Button buttonNavigateAlarm;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Button buttonCollectLog;
        private System.Windows.Forms.Button buttonNavigateGrade;
    }
}
