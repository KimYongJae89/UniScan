namespace UniScanG.UI.Etc
{
    partial class UserPanel
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
            this.layoutUser = new System.Windows.Forms.TableLayoutPanel();
            this.userName = new System.Windows.Forms.Label();
            this.userPictureBox = new System.Windows.Forms.PictureBox();
            this.layoutUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutUser
            // 
            this.layoutUser.ColumnCount = 2;
            this.layoutUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutUser.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutUser.Controls.Add(this.userName, 1, 0);
            this.layoutUser.Controls.Add(this.userPictureBox, 0, 0);
            this.layoutUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutUser.Location = new System.Drawing.Point(10, 10);
            this.layoutUser.Margin = new System.Windows.Forms.Padding(0);
            this.layoutUser.Name = "layoutUser";
            this.layoutUser.RowCount = 1;
            this.layoutUser.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutUser.Size = new System.Drawing.Size(161, 46);
            this.layoutUser.TabIndex = 2;
            // 
            // userName
            // 
            this.userName.BackColor = System.Drawing.Color.Transparent;
            this.userName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userName.Location = new System.Drawing.Point(50, 0);
            this.userName.Margin = new System.Windows.Forms.Padding(0);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(111, 46);
            this.userName.TabIndex = 0;
            this.userName.Text = "Operator";
            this.userName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.userName.Click += new System.EventHandler(this.userName_Click);
            // 
            // userPictureBox
            // 
            this.userPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.userPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userPictureBox.ErrorImage = global::UniScanG.Properties.Resources.user;
            this.userPictureBox.Image = global::UniScanG.Properties.Resources.user;
            this.userPictureBox.Location = new System.Drawing.Point(0, 0);
            this.userPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.userPictureBox.Name = "userPictureBox";
            this.userPictureBox.Size = new System.Drawing.Size(50, 46);
            this.userPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.userPictureBox.TabIndex = 0;
            this.userPictureBox.TabStop = false;
            // 
            // UserPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.layoutUser);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserPanel";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(181, 66);
            this.layoutUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.userPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutUser;
        private System.Windows.Forms.Label userName;
        private System.Windows.Forms.PictureBox userPictureBox;
    }
}
