namespace UniScanM.Pinhole.UI
{
    partial class InspectionPanelLeft
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
            this.components = new System.ComponentModel.Container();
            this.process1 = new System.Diagnostics.Process();
            this.layoutLastImage = new System.Windows.Forms.TableLayoutPanel();
            this.layoutCurrentImage = new System.Windows.Forms.TableLayoutPanel();
            this.panelCam1 = new System.Windows.Forms.Panel();
            this.panelCam2 = new System.Windows.Forms.Panel();
            this.imageTable = new System.Windows.Forms.TableLayoutPanel();
            this.panelCam = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer_GUIRevison = new System.Windows.Forms.Timer(this.components);
            this.layoutCurrentImage.SuspendLayout();
            this.imageTable.SuspendLayout();
            this.panelCam.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
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
            // layoutLastImage
            // 
            this.layoutLastImage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutLastImage.ColumnCount = 2;
            this.layoutLastImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutLastImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutLastImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLastImage.Location = new System.Drawing.Point(0, 270);
            this.layoutLastImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLastImage.Name = "layoutLastImage";
            this.layoutLastImage.RowCount = 1;
            this.layoutLastImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutLastImage.Size = new System.Drawing.Size(992, 308);
            this.layoutLastImage.TabIndex = 1;
            // 
            // layoutCurrentImage
            // 
            this.layoutCurrentImage.ColumnCount = 2;
            this.layoutCurrentImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCurrentImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCurrentImage.Controls.Add(this.panelCam1, 0, 0);
            this.layoutCurrentImage.Controls.Add(this.panelCam2, 1, 0);
            this.layoutCurrentImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCurrentImage.Location = new System.Drawing.Point(0, 0);
            this.layoutCurrentImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutCurrentImage.Name = "layoutCurrentImage";
            this.layoutCurrentImage.RowCount = 1;
            this.layoutCurrentImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCurrentImage.Size = new System.Drawing.Size(992, 210);
            this.layoutCurrentImage.TabIndex = 0;
            // 
            // panelCam1
            // 
            this.panelCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCam1.Location = new System.Drawing.Point(0, 0);
            this.panelCam1.Margin = new System.Windows.Forms.Padding(0);
            this.panelCam1.Name = "panelCam1";
            this.panelCam1.Size = new System.Drawing.Size(496, 210);
            this.panelCam1.TabIndex = 1;
            // 
            // panelCam2
            // 
            this.panelCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCam2.Location = new System.Drawing.Point(496, 0);
            this.panelCam2.Margin = new System.Windows.Forms.Padding(0);
            this.panelCam2.Name = "panelCam2";
            this.panelCam2.Size = new System.Drawing.Size(496, 210);
            this.panelCam2.TabIndex = 0;
            // 
            // imageTable
            // 
            this.imageTable.ColumnCount = 1;
            this.imageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.imageTable.Controls.Add(this.layoutCurrentImage, 0, 0);
            this.imageTable.Controls.Add(this.layoutLastImage, 0, 2);
            this.imageTable.Controls.Add(this.panelCam, 0, 1);
            this.imageTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageTable.Location = new System.Drawing.Point(0, 0);
            this.imageTable.Margin = new System.Windows.Forms.Padding(0);
            this.imageTable.Name = "imageTable";
            this.imageTable.RowCount = 3;
            this.imageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.63604F));
            this.imageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.imageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.36396F));
            this.imageTable.Size = new System.Drawing.Size(992, 578);
            this.imageTable.TabIndex = 2;
            // 
            // panelCam
            // 
            this.panelCam.Controls.Add(this.tableLayoutPanel2);
            this.panelCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCam.Location = new System.Drawing.Point(0, 210);
            this.panelCam.Margin = new System.Windows.Forms.Padding(0);
            this.panelCam.Name = "panelCam";
            this.panelCam.Size = new System.Drawing.Size(992, 60);
            this.panelCam.TabIndex = 9;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(992, 60);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(496, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(496, 60);
            this.label2.TabIndex = 6;
            this.label2.Text = "Camera 2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(496, 60);
            this.label1.TabIndex = 5;
            this.label1.Text = "Camera 1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_GUIRevison
            // 
            this.timer_GUIRevison.Tick += new System.EventHandler(this.timer_GUIRevison_Tick);
            // 
            // InspectionPanelLeft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.imageTable);
            this.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.Name = "InspectionPanelLeft";
            this.Size = new System.Drawing.Size(992, 578);
            this.layoutCurrentImage.ResumeLayout(false);
            this.imageTable.ResumeLayout(false);
            this.panelCam.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Diagnostics.Process process1;
        private System.Windows.Forms.TableLayoutPanel layoutCurrentImage;
        private System.Windows.Forms.TableLayoutPanel layoutLastImage;
        private System.Windows.Forms.TableLayoutPanel imageTable;
        private System.Windows.Forms.Panel panelCam;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelCam1;
        private System.Windows.Forms.Panel panelCam2;
        private System.Windows.Forms.Timer timer_GUIRevison;
    }
}
