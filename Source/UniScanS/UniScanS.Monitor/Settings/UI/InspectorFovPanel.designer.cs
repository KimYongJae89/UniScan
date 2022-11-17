namespace UniScanS.Common.Settings.Monitor.UI
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
            this.labelHeight = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetX = new System.Windows.Forms.Label();
            this.offsetX = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetY = new System.Windows.Forms.Label();
            this.offsetY = new System.Windows.Forms.NumericUpDown();
            this.labelFov = new System.Windows.Forms.Label();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.panelImage = new System.Windows.Forms.Panel();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetY)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutMain.Controls.Add(this.labelImage, 0, 0);
            this.layoutMain.Controls.Add(this.labelHeight, 1, 4);
            this.layoutMain.Controls.Add(this.height, 2, 4);
            this.layoutMain.Controls.Add(this.labelWidth, 1, 3);
            this.layoutMain.Controls.Add(this.width, 2, 3);
            this.layoutMain.Controls.Add(this.labelOffsetX, 1, 1);
            this.layoutMain.Controls.Add(this.offsetX, 2, 1);
            this.layoutMain.Controls.Add(this.labelOffsetY, 1, 2);
            this.layoutMain.Controls.Add(this.offsetY, 2, 2);
            this.layoutMain.Controls.Add(this.buttonLoadImage, 1, 6);
            this.layoutMain.Controls.Add(this.panelImage, 0, 1);
            this.layoutMain.Controls.Add(this.labelFov, 1, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 7;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
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
            this.labelImage.Size = new System.Drawing.Size(226, 30);
            this.labelImage.TabIndex = 14;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeight.Location = new System.Drawing.Point(228, 131);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(80, 32);
            this.labelHeight.TabIndex = 4;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // height
            // 
            this.height.DecimalPlaces = 2;
            this.height.Dock = System.Windows.Forms.DockStyle.Fill;
            this.height.Location = new System.Drawing.Point(309, 131);
            this.height.Margin = new System.Windows.Forms.Padding(0);
            this.height.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(120, 32);
            this.height.TabIndex = 5;
            this.height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.height.ValueChanged += new System.EventHandler(this.height_ValueChanged);
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWidth.Location = new System.Drawing.Point(228, 98);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(80, 32);
            this.labelWidth.TabIndex = 2;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // width
            // 
            this.width.DecimalPlaces = 2;
            this.width.Dock = System.Windows.Forms.DockStyle.Fill;
            this.width.Location = new System.Drawing.Point(309, 98);
            this.width.Margin = new System.Windows.Forms.Padding(0);
            this.width.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(120, 32);
            this.width.TabIndex = 3;
            this.width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.width.ValueChanged += new System.EventHandler(this.width_ValueChanged);
            // 
            // labelOffsetX
            // 
            this.labelOffsetX.AutoSize = true;
            this.labelOffsetX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOffsetX.Location = new System.Drawing.Point(228, 32);
            this.labelOffsetX.Margin = new System.Windows.Forms.Padding(0);
            this.labelOffsetX.Name = "labelOffsetX";
            this.labelOffsetX.Size = new System.Drawing.Size(80, 32);
            this.labelOffsetX.TabIndex = 6;
            this.labelOffsetX.Text = "X";
            this.labelOffsetX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // offsetX
            // 
            this.offsetX.DecimalPlaces = 2;
            this.offsetX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offsetX.Location = new System.Drawing.Point(309, 32);
            this.offsetX.Margin = new System.Windows.Forms.Padding(0);
            this.offsetX.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.offsetX.Name = "offsetX";
            this.offsetX.Size = new System.Drawing.Size(120, 32);
            this.offsetX.TabIndex = 7;
            this.offsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.offsetX.ValueChanged += new System.EventHandler(this.offsetX_ValueChanged);
            // 
            // labelOffsetY
            // 
            this.labelOffsetY.AutoSize = true;
            this.labelOffsetY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOffsetY.Location = new System.Drawing.Point(228, 65);
            this.labelOffsetY.Margin = new System.Windows.Forms.Padding(0);
            this.labelOffsetY.Name = "labelOffsetY";
            this.labelOffsetY.Size = new System.Drawing.Size(80, 32);
            this.labelOffsetY.TabIndex = 8;
            this.labelOffsetY.Text = "Y";
            this.labelOffsetY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // offsetY
            // 
            this.offsetY.DecimalPlaces = 2;
            this.offsetY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offsetY.Location = new System.Drawing.Point(309, 65);
            this.offsetY.Margin = new System.Windows.Forms.Padding(0);
            this.offsetY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.offsetY.Name = "offsetY";
            this.offsetY.Size = new System.Drawing.Size(120, 32);
            this.offsetY.TabIndex = 9;
            this.offsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.offsetY.ValueChanged += new System.EventHandler(this.offsetY_ValueChanged);
            // 
            // labelFov
            // 
            this.labelFov.AutoSize = true;
            this.labelFov.BackColor = System.Drawing.Color.Navy;
            this.layoutMain.SetColumnSpan(this.labelFov, 2);
            this.labelFov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFov.ForeColor = System.Drawing.Color.White;
            this.labelFov.Location = new System.Drawing.Point(228, 1);
            this.labelFov.Margin = new System.Windows.Forms.Padding(0);
            this.labelFov.Name = "labelFov";
            this.labelFov.Size = new System.Drawing.Size(201, 30);
            this.labelFov.TabIndex = 10;
            this.labelFov.Text = "FOV";
            this.labelFov.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.BackColor = System.Drawing.Color.White;
            this.layoutMain.SetColumnSpan(this.buttonLoadImage, 2);
            this.buttonLoadImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoadImage.Location = new System.Drawing.Point(228, 371);
            this.buttonLoadImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(201, 40);
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
            this.layoutMain.SetRowSpan(this.panelImage, 6);
            this.panelImage.Size = new System.Drawing.Size(226, 379);
            this.panelImage.TabIndex = 15;
            // 
            // InspectorFovPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "InspectorFovPanel";
            this.Size = new System.Drawing.Size(430, 412);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.Label labelOffsetX;
        private System.Windows.Forms.NumericUpDown offsetX;
        private System.Windows.Forms.Label labelOffsetY;
        private System.Windows.Forms.NumericUpDown offsetY;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Label labelFov;
        private System.Windows.Forms.Panel panelImage;
    }
}
