using Infragistics.Win.Misc;

namespace DataCollector
{
    partial class MainForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dataCollectionTimer = new System.Windows.Forms.Timer(this.components);
            this.ultraSpreadsheet1 = new Infragistics.Win.UltraWinSpreadsheet.UltraSpreadsheet();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelResult = new System.Windows.Forms.Label();
            this.buttonSetting = new Infragistics.Win.Misc.UltraButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraSpreadsheet1)).BeginInit();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataCollectionTimer
            // 
            this.dataCollectionTimer.Interval = 1000;
            this.dataCollectionTimer.Tick += new System.EventHandler(this.dataCollectionTimer_Tick);
            // 
            // ultraSpreadsheet1
            // 
            this.ultraSpreadsheet1.AllowAddWorksheet = false;
            this.ultraSpreadsheet1.AllowDeleteWorksheet = false;
            this.ultraSpreadsheet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraSpreadsheet1.IsEnterKeyNavigationEnabled = false;
            this.ultraSpreadsheet1.IsUndoEnabled = false;
            this.ultraSpreadsheet1.Location = new System.Drawing.Point(0, 60);
            this.ultraSpreadsheet1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraSpreadsheet1.Name = "ultraSpreadsheet1";
            this.ultraSpreadsheet1.Size = new System.Drawing.Size(850, 383);
            this.ultraSpreadsheet1.TabIndex = 6;
            this.ultraSpreadsheet1.Text = "ultraSpreadsheet1";
            this.ultraSpreadsheet1.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDI;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.ultraSpreadsheet1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(850, 443);
            this.tableLayoutPanel.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.labelResult, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSetting, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(850, 60);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Font = new System.Drawing.Font("맑은 고딕", 35F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelResult.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelResult.Location = new System.Drawing.Point(0, 0);
            this.labelResult.Margin = new System.Windows.Forms.Padding(0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(820, 60);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "RESULT";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSetting
            // 
            appearance1.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.Image = global::DataCollector.Properties.Resources.Setting;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonSetting.Appearance = appearance1;
            this.buttonSetting.Location = new System.Drawing.Point(820, 0);
            this.buttonSetting.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(30, 30);
            this.buttonSetting.TabIndex = 6;
            this.buttonSetting.Click += new System.EventHandler(this.buttonSetting_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 443);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DataCollector";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraSpreadsheet1)).EndInit();
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer dataCollectionTimer;
        private Infragistics.Win.UltraWinSpreadsheet.UltraSpreadsheet ultraSpreadsheet1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelResult;
        private UltraButton buttonSetting;
    }
}

