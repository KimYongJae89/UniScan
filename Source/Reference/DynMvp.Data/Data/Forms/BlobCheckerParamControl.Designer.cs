namespace DynMvp.Data.Forms
{
    partial class BlobCheckeParamControl
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
            this.searchRangeHeight = new System.Windows.Forms.NumericUpDown();
            this.labelSize = new System.Windows.Forms.Label();
            this.searchRangeWidth = new System.Windows.Forms.NumericUpDown();
            this.labelW = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.offsetRangeX = new System.Windows.Forms.NumericUpDown();
            this.labelOffsetRange = new System.Windows.Forms.Label();
            this.labelPixel = new System.Windows.Forms.Label();
            this.areaUpperPct = new System.Windows.Forms.NumericUpDown();
            this.areaLowerPct = new System.Windows.Forms.NumericUpDown();
            this.labelArea = new System.Windows.Forms.Label();
            this.labelTilda = new System.Windows.Forms.Label();
            this.useFiducialProbe = new System.Windows.Forms.CheckBox();
            this.useWholeImage = new System.Windows.Forms.CheckBox();
            this.darkBlob = new System.Windows.Forms.CheckBox();
            this.panelSub1 = new System.Windows.Forms.Panel();
            this.panelFiducial = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.offsetRangeY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetRangeX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaUpperPct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaLowerPct)).BeginInit();
            this.panelSub1.SuspendLayout();
            this.panelFiducial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offsetRangeY)).BeginInit();
            this.SuspendLayout();
            // 
            // searchRangeHeight
            // 
            this.searchRangeHeight.Location = new System.Drawing.Point(253, 7);
            this.searchRangeHeight.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeHeight.Name = "searchRangeHeight";
            this.searchRangeHeight.Size = new System.Drawing.Size(59, 27);
            this.searchRangeHeight.TabIndex = 37;
            this.searchRangeHeight.ValueChanged += new System.EventHandler(this.searchRangeHeight_ValueChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(7, 9);
            this.labelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(102, 20);
            this.labelSize.TabIndex = 33;
            this.labelSize.Text = "Search Range";
            // 
            // searchRangeWidth
            // 
            this.searchRangeWidth.Location = new System.Drawing.Point(161, 7);
            this.searchRangeWidth.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeWidth.Name = "searchRangeWidth";
            this.searchRangeWidth.Size = new System.Drawing.Size(59, 27);
            this.searchRangeWidth.TabIndex = 35;
            this.searchRangeWidth.ValueChanged += new System.EventHandler(this.searchRangeWidth_ValueChanged);
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(129, 10);
            this.labelW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(23, 20);
            this.labelW.TabIndex = 34;
            this.labelW.Text = "W";
            this.labelW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(228, 9);
            this.labelH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(20, 20);
            this.labelH.TabIndex = 36;
            this.labelH.Text = "H";
            this.labelH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 43);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 19;
            this.label1.Text = "piexel";
            // 
            // offsetRangeX
            // 
            this.offsetRangeX.Location = new System.Drawing.Point(168, 41);
            this.offsetRangeX.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.offsetRangeX.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.offsetRangeX.Name = "offsetRangeX";
            this.offsetRangeX.Size = new System.Drawing.Size(71, 27);
            this.offsetRangeX.TabIndex = 18;
            this.offsetRangeX.ValueChanged += new System.EventHandler(this.offsetRangeX_ValueChanged);
            // 
            // labelOffsetRange
            // 
            this.labelOffsetRange.AutoSize = true;
            this.labelOffsetRange.Location = new System.Drawing.Point(10, 43);
            this.labelOffsetRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOffsetRange.Name = "labelOffsetRange";
            this.labelOffsetRange.Size = new System.Drawing.Size(98, 20);
            this.labelOffsetRange.TabIndex = 17;
            this.labelOffsetRange.Text = "Offset Range";
            // 
            // labelPixel
            // 
            this.labelPixel.AutoSize = true;
            this.labelPixel.Location = new System.Drawing.Point(250, 11);
            this.labelPixel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPixel.Name = "labelPixel";
            this.labelPixel.Size = new System.Drawing.Size(49, 20);
            this.labelPixel.TabIndex = 16;
            this.labelPixel.Text = "piexel";
            // 
            // areaUpperPct
            // 
            this.areaUpperPct.Location = new System.Drawing.Point(168, 8);
            this.areaUpperPct.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.areaUpperPct.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.areaUpperPct.Name = "areaUpperPct";
            this.areaUpperPct.Size = new System.Drawing.Size(71, 27);
            this.areaUpperPct.TabIndex = 15;
            this.areaUpperPct.ValueChanged += new System.EventHandler(this.areaUpperPct_ValueChanged);
            // 
            // areaLowerPct
            // 
            this.areaLowerPct.Location = new System.Drawing.Point(64, 8);
            this.areaLowerPct.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.areaLowerPct.Maximum = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.areaLowerPct.Name = "areaLowerPct";
            this.areaLowerPct.Size = new System.Drawing.Size(71, 27);
            this.areaLowerPct.TabIndex = 10;
            this.areaLowerPct.ValueChanged += new System.EventHandler(this.areaLowerPct_ValueChanged);
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Location = new System.Drawing.Point(10, 10);
            this.labelArea.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(40, 20);
            this.labelArea.TabIndex = 4;
            this.labelArea.Text = "Area";
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(142, 10);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(20, 20);
            this.labelTilda.TabIndex = 14;
            this.labelTilda.Text = "~";
            // 
            // useFiducialProbe
            // 
            this.useFiducialProbe.AutoSize = true;
            this.useFiducialProbe.Location = new System.Drawing.Point(3, 198);
            this.useFiducialProbe.Name = "useFiducialProbe";
            this.useFiducialProbe.Size = new System.Drawing.Size(155, 24);
            this.useFiducialProbe.TabIndex = 41;
            this.useFiducialProbe.Text = "Use Fiducial Probe";
            this.useFiducialProbe.UseVisualStyleBackColor = true;
            this.useFiducialProbe.CheckedChanged += new System.EventHandler(this.useFiducialProbe_CheckedChanged);
            // 
            // useWholeImage
            // 
            this.useWholeImage.AutoSize = true;
            this.useWholeImage.Location = new System.Drawing.Point(3, 176);
            this.useWholeImage.Name = "useWholeImage";
            this.useWholeImage.Size = new System.Drawing.Size(149, 24);
            this.useWholeImage.TabIndex = 43;
            this.useWholeImage.Text = "Use Whole Image";
            this.useWholeImage.UseVisualStyleBackColor = true;
            this.useWholeImage.CheckedChanged += new System.EventHandler(this.useWholeImage_CheckedChanged);
            // 
            // darkBlob
            // 
            this.darkBlob.AutoSize = true;
            this.darkBlob.Location = new System.Drawing.Point(3, 155);
            this.darkBlob.Name = "darkBlob";
            this.darkBlob.Size = new System.Drawing.Size(96, 24);
            this.darkBlob.TabIndex = 44;
            this.darkBlob.Text = "Dark Blob";
            this.darkBlob.UseVisualStyleBackColor = true;
            this.darkBlob.CheckedChanged += new System.EventHandler(this.darkBlob_CheckedChanged);
            // 
            // panelSub1
            // 
            this.panelSub1.Controls.Add(this.panelFiducial);
            this.panelSub1.Controls.Add(this.useFiducialProbe);
            this.panelSub1.Controls.Add(this.label2);
            this.panelSub1.Controls.Add(this.label1);
            this.panelSub1.Controls.Add(this.darkBlob);
            this.panelSub1.Controls.Add(this.labelArea);
            this.panelSub1.Controls.Add(this.offsetRangeY);
            this.panelSub1.Controls.Add(this.offsetRangeX);
            this.panelSub1.Controls.Add(this.useWholeImage);
            this.panelSub1.Controls.Add(this.labelTilda);
            this.panelSub1.Controls.Add(this.areaUpperPct);
            this.panelSub1.Controls.Add(this.labelOffsetRange);
            this.panelSub1.Controls.Add(this.labelPixel);
            this.panelSub1.Controls.Add(this.areaLowerPct);
            this.panelSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSub1.Location = new System.Drawing.Point(0, 0);
            this.panelSub1.Name = "panelSub1";
            this.panelSub1.Size = new System.Drawing.Size(325, 234);
            this.panelSub1.TabIndex = 45;
            // 
            // panelFiducial
            // 
            this.panelFiducial.Controls.Add(this.searchRangeHeight);
            this.panelFiducial.Controls.Add(this.labelSize);
            this.panelFiducial.Controls.Add(this.labelH);
            this.panelFiducial.Controls.Add(this.searchRangeWidth);
            this.panelFiducial.Controls.Add(this.labelW);
            this.panelFiducial.Location = new System.Drawing.Point(0, 109);
            this.panelFiducial.Name = "panelFiducial";
            this.panelFiducial.Size = new System.Drawing.Size(325, 40);
            this.panelFiducial.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 20);
            this.label2.TabIndex = 19;
            this.label2.Text = "piexel";
            // 
            // offsetRangeY
            // 
            this.offsetRangeY.Location = new System.Drawing.Point(168, 71);
            this.offsetRangeY.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.offsetRangeY.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.offsetRangeY.Name = "offsetRangeY";
            this.offsetRangeY.Size = new System.Drawing.Size(71, 27);
            this.offsetRangeY.TabIndex = 18;
            this.offsetRangeY.ValueChanged += new System.EventHandler(this.offsetRangeY_ValueChanged);
            // 
            // BlobCheckeParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSub1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BlobCheckeParamControl";
            this.Size = new System.Drawing.Size(325, 411);
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.offsetRangeX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaUpperPct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.areaLowerPct)).EndInit();
            this.panelSub1.ResumeLayout(false);
            this.panelSub1.PerformLayout();
            this.panelFiducial.ResumeLayout(false);
            this.panelFiducial.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offsetRangeY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown searchRangeHeight;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.NumericUpDown searchRangeWidth;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label labelPixel;
        private System.Windows.Forms.NumericUpDown areaUpperPct;
        private System.Windows.Forms.NumericUpDown areaLowerPct;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown offsetRangeX;
        private System.Windows.Forms.Label labelOffsetRange;
        private System.Windows.Forms.CheckBox useFiducialProbe;
        private System.Windows.Forms.CheckBox useWholeImage;
        private System.Windows.Forms.CheckBox darkBlob;
        private System.Windows.Forms.Panel panelSub1;
        private System.Windows.Forms.Panel panelFiducial;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown offsetRangeY;
    }
}
