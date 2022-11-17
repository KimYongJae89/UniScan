namespace UniScanG.Module.Controller.Settings.Monitor.UI
{
    partial class InspectorFovPanel
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
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.labelLeft = new System.Windows.Forms.Label();
            this.left = new System.Windows.Forms.NumericUpDown();
            this.labelTop = new System.Windows.Forms.Label();
            this.top = new System.Windows.Forms.NumericUpDown();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.panelImage = new System.Windows.Forms.Panel();
            this.labelFov = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelRight = new System.Windows.Forms.Label();
            this.labelBottom = new System.Windows.Forms.Label();
            this.right = new System.Windows.Forms.NumericUpDown();
            this.bottom = new System.Windows.Forms.NumericUpDown();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.left)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.right)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottom)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 124F));
            this.layoutMain.Controls.Add(this.labelImage, 0, 0);
            this.layoutMain.Controls.Add(this.labelLeft, 1, 1);
            this.layoutMain.Controls.Add(this.left, 2, 1);
            this.layoutMain.Controls.Add(this.labelTop, 1, 2);
            this.layoutMain.Controls.Add(this.top, 2, 2);
            this.layoutMain.Controls.Add(this.buttonLoadImage, 1, 8);
            this.layoutMain.Controls.Add(this.panelImage, 0, 1);
            this.layoutMain.Controls.Add(this.labelFov, 1, 0);
            this.layoutMain.Controls.Add(this.height, 2, 6);
            this.layoutMain.Controls.Add(this.labelHeight, 1, 6);
            this.layoutMain.Controls.Add(this.width, 2, 5);
            this.layoutMain.Controls.Add(this.labelWidth, 1, 5);
            this.layoutMain.Controls.Add(this.labelRight, 1, 3);
            this.layoutMain.Controls.Add(this.labelBottom, 1, 4);
            this.layoutMain.Controls.Add(this.right, 2, 3);
            this.layoutMain.Controls.Add(this.bottom, 2, 4);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 9;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.Size = new System.Drawing.Size(430, 412);
            this.layoutMain.TabIndex = 0;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.Navy;
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.ForeColor = System.Drawing.Color.White;
            this.labelImage.Location = new System.Drawing.Point(1, 1);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(222, 30);
            this.labelImage.TabIndex = 14;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLeft
            // 
            this.labelLeft.AutoSize = true;
            this.labelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLeft.Location = new System.Drawing.Point(224, 32);
            this.labelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(80, 32);
            this.labelLeft.TabIndex = 6;
            this.labelLeft.Text = "Left";
            this.labelLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // left
            // 
            this.left.Dock = System.Windows.Forms.DockStyle.Fill;
            this.left.Location = new System.Drawing.Point(305, 32);
            this.left.Margin = new System.Windows.Forms.Padding(0);
            this.left.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(124, 32);
            this.left.TabIndex = 7;
            this.left.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.left.ValueChanged += new System.EventHandler(this.fov_ValueChanged);
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTop.Location = new System.Drawing.Point(224, 65);
            this.labelTop.Margin = new System.Windows.Forms.Padding(0);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(80, 32);
            this.labelTop.TabIndex = 8;
            this.labelTop.Text = "Top";
            this.labelTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // top
            // 
            this.top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.top.Location = new System.Drawing.Point(305, 65);
            this.top.Margin = new System.Windows.Forms.Padding(0);
            this.top.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(124, 32);
            this.top.TabIndex = 9;
            this.top.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.top.ValueChanged += new System.EventHandler(this.fov_ValueChanged);
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.BackColor = System.Drawing.Color.White;
            this.layoutMain.SetColumnSpan(this.buttonLoadImage, 2);
            this.buttonLoadImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadImage.Location = new System.Drawing.Point(224, 371);
            this.buttonLoadImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(205, 40);
            this.buttonLoadImage.TabIndex = 0;
            this.buttonLoadImage.Text = "Load";
            this.buttonLoadImage.UseVisualStyleBackColor = false;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(1, 32);
            this.panelImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelImage.Name = "panelImage";
            this.layoutMain.SetRowSpan(this.panelImage, 8);
            this.panelImage.Size = new System.Drawing.Size(222, 379);
            this.panelImage.TabIndex = 15;
            // 
            // labelFov
            // 
            this.labelFov.AutoSize = true;
            this.labelFov.BackColor = System.Drawing.Color.Navy;
            this.layoutMain.SetColumnSpan(this.labelFov, 2);
            this.labelFov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFov.ForeColor = System.Drawing.Color.White;
            this.labelFov.Location = new System.Drawing.Point(224, 1);
            this.labelFov.Margin = new System.Windows.Forms.Padding(0);
            this.labelFov.Name = "labelFov";
            this.labelFov.Size = new System.Drawing.Size(205, 30);
            this.labelFov.TabIndex = 10;
            this.labelFov.Text = "FOV";
            this.labelFov.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // height
            // 
            this.height.Dock = System.Windows.Forms.DockStyle.Fill;
            this.height.Enabled = false;
            this.height.Location = new System.Drawing.Point(305, 197);
            this.height.Margin = new System.Windows.Forms.Padding(0);
            this.height.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(124, 32);
            this.height.TabIndex = 5;
            this.height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeight.Location = new System.Drawing.Point(224, 197);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(80, 32);
            this.labelHeight.TabIndex = 4;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // width
            // 
            this.width.Dock = System.Windows.Forms.DockStyle.Fill;
            this.width.Enabled = false;
            this.width.Location = new System.Drawing.Point(305, 164);
            this.width.Margin = new System.Windows.Forms.Padding(0);
            this.width.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(124, 32);
            this.width.TabIndex = 3;
            this.width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWidth.Location = new System.Drawing.Point(224, 164);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(80, 32);
            this.labelWidth.TabIndex = 2;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRight
            // 
            this.labelRight.AutoSize = true;
            this.labelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRight.Location = new System.Drawing.Point(224, 98);
            this.labelRight.Margin = new System.Windows.Forms.Padding(0);
            this.labelRight.Name = "labelRight";
            this.labelRight.Size = new System.Drawing.Size(80, 32);
            this.labelRight.TabIndex = 2;
            this.labelRight.Text = "Right";
            this.labelRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBottom
            // 
            this.labelBottom.AutoSize = true;
            this.labelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBottom.Location = new System.Drawing.Point(224, 131);
            this.labelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(80, 32);
            this.labelBottom.TabIndex = 2;
            this.labelBottom.Text = "Bottom";
            this.labelBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // right
            // 
            this.right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.right.Location = new System.Drawing.Point(305, 98);
            this.right.Margin = new System.Windows.Forms.Padding(0);
            this.right.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(124, 32);
            this.right.TabIndex = 9;
            this.right.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.right.ValueChanged += new System.EventHandler(this.fov_ValueChanged);
            // 
            // bottom
            // 
            this.bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottom.Location = new System.Drawing.Point(305, 131);
            this.bottom.Margin = new System.Windows.Forms.Padding(0);
            this.bottom.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.bottom.Name = "bottom";
            this.bottom.Size = new System.Drawing.Size(124, 32);
            this.bottom.TabIndex = 9;
            this.bottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bottom.ValueChanged += new System.EventHandler(this.fov_ValueChanged);
            // 
            // InspectorFovPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "InspectorFovPanel";
            this.Size = new System.Drawing.Size(430, 412);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.left)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.right)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.Label labelLeft;
        private System.Windows.Forms.NumericUpDown left;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.NumericUpDown top;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Label labelFov;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Label labelRight;
        private System.Windows.Forms.Label labelBottom;
        private System.Windows.Forms.NumericUpDown right;
        private System.Windows.Forms.NumericUpDown bottom;
    }
}
