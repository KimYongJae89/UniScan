namespace minsongCV
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_open = new System.Windows.Forms.Button();
            this.button_Binary = new System.Windows.Forms.Button();
            this.button_Median = new System.Windows.Forms.Button();
            this.button_Erode = new System.Windows.Forms.Button();
            this.button_Dilate = new System.Windows.Forms.Button();
            this.button_Average = new System.Windows.Forms.Button();
            this.button_AND = new System.Windows.Forms.Button();
            this.panel_menu = new System.Windows.Forms.Panel();
            this.button_ImgThreshold = new System.Windows.Forms.Button();
            this.tbThreshold = new System.Windows.Forms.TextBox();
            this.button_Subtraction = new System.Windows.Forms.Button();
            this.button_ZoomOut = new System.Windows.Forms.Button();
            this.button_ZoomIn = new System.Windows.Forms.Button();
            this.button_PatMat = new System.Windows.Forms.Button();
            this.button_MakeReferenceImage = new System.Windows.Forms.Button();
            this.button_OpenFixed = new System.Windows.Forms.Button();
            this.button_Threshold = new System.Windows.Forms.Button();
            this.button_Remove_edge = new System.Windows.Forms.Button();
            this.button_Test = new System.Windows.Forms.Button();
            this.button_Blob = new System.Windows.Forms.Button();
            this.button_Edge = new System.Windows.Forms.Button();
            this.button_Mutiplication = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_TDI = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_menu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Green;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(6, 106);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(789, 998);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(9, 4);
            this.button_open.Margin = new System.Windows.Forms.Padding(6);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(108, 60);
            this.button_open.TabIndex = 1;
            this.button_open.Text = "1. Open";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_Binary
            // 
            this.button_Binary.Location = new System.Drawing.Point(232, 8);
            this.button_Binary.Margin = new System.Windows.Forms.Padding(6);
            this.button_Binary.Name = "button_Binary";
            this.button_Binary.Size = new System.Drawing.Size(147, 56);
            this.button_Binary.TabIndex = 2;
            this.button_Binary.Text = "Binary";
            this.button_Binary.UseVisualStyleBackColor = true;
            this.button_Binary.Click += new System.EventHandler(this.button_Binary_Click);
            // 
            // button_Median
            // 
            this.button_Median.Location = new System.Drawing.Point(390, 8);
            this.button_Median.Margin = new System.Windows.Forms.Padding(6);
            this.button_Median.Name = "button_Median";
            this.button_Median.Size = new System.Drawing.Size(147, 56);
            this.button_Median.TabIndex = 3;
            this.button_Median.Text = "Median";
            this.button_Median.UseVisualStyleBackColor = true;
            this.button_Median.Click += new System.EventHandler(this.button_Median_Click);
            // 
            // button_Erode
            // 
            this.button_Erode.Location = new System.Drawing.Point(548, 8);
            this.button_Erode.Margin = new System.Windows.Forms.Padding(6);
            this.button_Erode.Name = "button_Erode";
            this.button_Erode.Size = new System.Drawing.Size(147, 56);
            this.button_Erode.TabIndex = 4;
            this.button_Erode.Text = "erode";
            this.button_Erode.UseVisualStyleBackColor = true;
            this.button_Erode.Click += new System.EventHandler(this.button_Erode_Click);
            // 
            // button_Dilate
            // 
            this.button_Dilate.Location = new System.Drawing.Point(706, 8);
            this.button_Dilate.Margin = new System.Windows.Forms.Padding(6);
            this.button_Dilate.Name = "button_Dilate";
            this.button_Dilate.Size = new System.Drawing.Size(147, 56);
            this.button_Dilate.TabIndex = 5;
            this.button_Dilate.Text = "dilate";
            this.button_Dilate.UseVisualStyleBackColor = true;
            this.button_Dilate.Click += new System.EventHandler(this.button_Dilate_Click);
            // 
            // button_Average
            // 
            this.button_Average.Location = new System.Drawing.Point(864, 8);
            this.button_Average.Margin = new System.Windows.Forms.Padding(6);
            this.button_Average.Name = "button_Average";
            this.button_Average.Size = new System.Drawing.Size(147, 56);
            this.button_Average.TabIndex = 6;
            this.button_Average.Text = "Average";
            this.button_Average.UseVisualStyleBackColor = true;
            this.button_Average.Click += new System.EventHandler(this.button_Average_Click);
            // 
            // button_AND
            // 
            this.button_AND.Location = new System.Drawing.Point(1021, 8);
            this.button_AND.Margin = new System.Windows.Forms.Padding(6);
            this.button_AND.Name = "button_AND";
            this.button_AND.Size = new System.Drawing.Size(93, 56);
            this.button_AND.TabIndex = 7;
            this.button_AND.Text = "AND";
            this.button_AND.UseVisualStyleBackColor = true;
            this.button_AND.Click += new System.EventHandler(this.button_AND_Click);
            // 
            // panel_menu
            // 
            this.panel_menu.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tableLayoutPanel1.SetColumnSpan(this.panel_menu, 2);
            this.panel_menu.Controls.Add(this.button_ImgThreshold);
            this.panel_menu.Controls.Add(this.tbThreshold);
            this.panel_menu.Controls.Add(this.button_Subtraction);
            this.panel_menu.Controls.Add(this.button_ZoomOut);
            this.panel_menu.Controls.Add(this.button_ZoomIn);
            this.panel_menu.Controls.Add(this.button_PatMat);
            this.panel_menu.Controls.Add(this.button_MakeReferenceImage);
            this.panel_menu.Controls.Add(this.button_OpenFixed);
            this.panel_menu.Controls.Add(this.button_Threshold);
            this.panel_menu.Controls.Add(this.button_Remove_edge);
            this.panel_menu.Controls.Add(this.button_TDI);
            this.panel_menu.Controls.Add(this.button_Test);
            this.panel_menu.Controls.Add(this.button_Blob);
            this.panel_menu.Controls.Add(this.button_Edge);
            this.panel_menu.Controls.Add(this.button_Mutiplication);
            this.panel_menu.Controls.Add(this.button_Binary);
            this.panel_menu.Controls.Add(this.button_AND);
            this.panel_menu.Controls.Add(this.button_open);
            this.panel_menu.Controls.Add(this.button_Average);
            this.panel_menu.Controls.Add(this.button_Median);
            this.panel_menu.Controls.Add(this.button_Dilate);
            this.panel_menu.Controls.Add(this.button_Erode);
            this.panel_menu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_menu.Location = new System.Drawing.Point(6, 6);
            this.panel_menu.Margin = new System.Windows.Forms.Padding(6);
            this.panel_menu.Name = "panel_menu";
            this.panel_menu.Size = new System.Drawing.Size(2659, 88);
            this.panel_menu.TabIndex = 8;
            // 
            // button_ImgThreshold
            // 
            this.button_ImgThreshold.Location = new System.Drawing.Point(2536, 16);
            this.button_ImgThreshold.Name = "button_ImgThreshold";
            this.button_ImgThreshold.Size = new System.Drawing.Size(92, 47);
            this.button_ImgThreshold.TabIndex = 11;
            this.button_ImgThreshold.Text = "ImgTh";
            this.button_ImgThreshold.UseVisualStyleBackColor = true;
            this.button_ImgThreshold.Click += new System.EventHandler(this.button_ImgThreshold_Click);
            // 
            // tbThreshold
            // 
            this.tbThreshold.Location = new System.Drawing.Point(2009, 16);
            this.tbThreshold.Margin = new System.Windows.Forms.Padding(6);
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(73, 35);
            this.tbThreshold.TabIndex = 10;
            this.tbThreshold.Text = "20";
            this.tbThreshold.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInterval_KeyPress);
            // 
            // button_Subtraction
            // 
            this.button_Subtraction.Location = new System.Drawing.Point(1753, 16);
            this.button_Subtraction.Margin = new System.Windows.Forms.Padding(6);
            this.button_Subtraction.Name = "button_Subtraction";
            this.button_Subtraction.Size = new System.Drawing.Size(119, 48);
            this.button_Subtraction.TabIndex = 9;
            this.button_Subtraction.Text = "Subtraction";
            this.button_Subtraction.UseVisualStyleBackColor = true;
            this.button_Subtraction.Click += new System.EventHandler(this.button_Subtraction_Click);
            // 
            // button_ZoomOut
            // 
            this.button_ZoomOut.Location = new System.Drawing.Point(2375, 48);
            this.button_ZoomOut.Margin = new System.Windows.Forms.Padding(6);
            this.button_ZoomOut.Name = "button_ZoomOut";
            this.button_ZoomOut.Size = new System.Drawing.Size(111, 40);
            this.button_ZoomOut.TabIndex = 8;
            this.button_ZoomOut.Text = "zoom-";
            this.button_ZoomOut.UseVisualStyleBackColor = true;
            this.button_ZoomOut.Click += new System.EventHandler(this.button_ZoomOut_Click);
            // 
            // button_ZoomIn
            // 
            this.button_ZoomIn.Location = new System.Drawing.Point(2375, 4);
            this.button_ZoomIn.Margin = new System.Windows.Forms.Padding(6);
            this.button_ZoomIn.Name = "button_ZoomIn";
            this.button_ZoomIn.Size = new System.Drawing.Size(111, 40);
            this.button_ZoomIn.TabIndex = 8;
            this.button_ZoomIn.Text = "zoom+";
            this.button_ZoomIn.UseVisualStyleBackColor = true;
            this.button_ZoomIn.Click += new System.EventHandler(this.button_ZoomIn_Click);
            // 
            // button_PatMat
            // 
            this.button_PatMat.Location = new System.Drawing.Point(1631, 18);
            this.button_PatMat.Margin = new System.Windows.Forms.Padding(6);
            this.button_PatMat.Name = "button_PatMat";
            this.button_PatMat.Size = new System.Drawing.Size(111, 46);
            this.button_PatMat.TabIndex = 8;
            this.button_PatMat.Text = "PatMat";
            this.button_PatMat.UseVisualStyleBackColor = true;
            this.button_PatMat.Click += new System.EventHandler(this.button_PatMat_Click);
            // 
            // button_MakeReferenceImage
            // 
            this.button_MakeReferenceImage.Location = new System.Drawing.Point(1475, 14);
            this.button_MakeReferenceImage.Margin = new System.Windows.Forms.Padding(6);
            this.button_MakeReferenceImage.Name = "button_MakeReferenceImage";
            this.button_MakeReferenceImage.Size = new System.Drawing.Size(145, 50);
            this.button_MakeReferenceImage.TabIndex = 8;
            this.button_MakeReferenceImage.Text = "Make Ref";
            this.button_MakeReferenceImage.UseVisualStyleBackColor = true;
            this.button_MakeReferenceImage.Click += new System.EventHandler(this.button_MakeReferenceImage_Click);
            // 
            // button_OpenFixed
            // 
            this.button_OpenFixed.Location = new System.Drawing.Point(117, 8);
            this.button_OpenFixed.Margin = new System.Windows.Forms.Padding(6);
            this.button_OpenFixed.Name = "button_OpenFixed";
            this.button_OpenFixed.Size = new System.Drawing.Size(104, 56);
            this.button_OpenFixed.TabIndex = 8;
            this.button_OpenFixed.Text = "fixload";
            this.button_OpenFixed.UseVisualStyleBackColor = true;
            this.button_OpenFixed.Click += new System.EventHandler(this.button_OpenFixed_Click);
            // 
            // button_Threshold
            // 
            this.button_Threshold.Location = new System.Drawing.Point(1883, 14);
            this.button_Threshold.Margin = new System.Windows.Forms.Padding(6);
            this.button_Threshold.Name = "button_Threshold";
            this.button_Threshold.Size = new System.Drawing.Size(111, 50);
            this.button_Threshold.TabIndex = 8;
            this.button_Threshold.Text = "Threshold";
            this.button_Threshold.UseVisualStyleBackColor = true;
            this.button_Threshold.Click += new System.EventHandler(this.button_Threshold_Click);
            // 
            // button_Remove_edge
            // 
            this.button_Remove_edge.Location = new System.Drawing.Point(2086, 24);
            this.button_Remove_edge.Margin = new System.Windows.Forms.Padding(6);
            this.button_Remove_edge.Name = "button_Remove_edge";
            this.button_Remove_edge.Size = new System.Drawing.Size(111, 40);
            this.button_Remove_edge.TabIndex = 8;
            this.button_Remove_edge.Text = "Rem Edge";
            this.button_Remove_edge.UseVisualStyleBackColor = true;
            this.button_Remove_edge.Click += new System.EventHandler(this.button_Remove_edge_Click);
            // 
            // button_Test
            // 
            this.button_Test.Location = new System.Drawing.Point(2225, 10);
            this.button_Test.Margin = new System.Windows.Forms.Padding(6);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(111, 40);
            this.button_Test.TabIndex = 8;
            this.button_Test.Text = "Test";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // button_Blob
            // 
            this.button_Blob.Location = new System.Drawing.Point(1352, 10);
            this.button_Blob.Margin = new System.Windows.Forms.Padding(6);
            this.button_Blob.Name = "button_Blob";
            this.button_Blob.Size = new System.Drawing.Size(111, 54);
            this.button_Blob.TabIndex = 8;
            this.button_Blob.Text = "blob";
            this.button_Blob.UseVisualStyleBackColor = true;
            this.button_Blob.Click += new System.EventHandler(this.button_Blob_Click);
            // 
            // button_Edge
            // 
            this.button_Edge.Location = new System.Drawing.Point(1229, 10);
            this.button_Edge.Margin = new System.Windows.Forms.Padding(6);
            this.button_Edge.Name = "button_Edge";
            this.button_Edge.Size = new System.Drawing.Size(111, 54);
            this.button_Edge.TabIndex = 8;
            this.button_Edge.Text = "Edge";
            this.button_Edge.UseVisualStyleBackColor = true;
            this.button_Edge.Click += new System.EventHandler(this.button_Edge_Click);
            // 
            // button_Mutiplication
            // 
            this.button_Mutiplication.Location = new System.Drawing.Point(1125, 8);
            this.button_Mutiplication.Margin = new System.Windows.Forms.Padding(6);
            this.button_Mutiplication.Name = "button_Mutiplication";
            this.button_Mutiplication.Size = new System.Drawing.Size(93, 56);
            this.button_Mutiplication.TabIndex = 8;
            this.button_Mutiplication.Text = "Mul";
            this.button_Mutiplication.UseVisualStyleBackColor = true;
            this.button_Mutiplication.Click += new System.EventHandler(this.button_Mutiplication_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel_menu, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(2671, 1110);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.OrangeRed;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(807, 106);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1858, 998);
            this.panel1.TabIndex = 10;
            // 
            // button_TDI
            // 
            this.button_TDI.Location = new System.Drawing.Point(2234, 48);
            this.button_TDI.Margin = new System.Windows.Forms.Padding(6);
            this.button_TDI.Name = "button_TDI";
            this.button_TDI.Size = new System.Drawing.Size(111, 40);
            this.button_TDI.TabIndex = 8;
            this.button_TDI.Text = "TDI";
            this.button_TDI.UseVisualStyleBackColor = true;
            this.button_TDI.Click += new System.EventHandler(this.button_TDI_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2671, 1110);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_menu.ResumeLayout(false);
            this.panel_menu.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_Binary;
        private System.Windows.Forms.Button button_Median;
        private System.Windows.Forms.Button button_Erode;
        private System.Windows.Forms.Button button_Dilate;
        private System.Windows.Forms.Button button_Average;
        private System.Windows.Forms.Button button_AND;
        private System.Windows.Forms.Panel panel_menu;
        private System.Windows.Forms.Button button_ZoomOut;
        private System.Windows.Forms.Button button_ZoomIn;
        private System.Windows.Forms.Button button_PatMat;
        private System.Windows.Forms.Button button_MakeReferenceImage;
        private System.Windows.Forms.Button button_OpenFixed;
        private System.Windows.Forms.Button button_Test;
        private System.Windows.Forms.Button button_Blob;
        private System.Windows.Forms.Button button_Edge;
        private System.Windows.Forms.Button button_Mutiplication;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_Subtraction;
        private System.Windows.Forms.Button button_Threshold;
        private System.Windows.Forms.Button button_Remove_edge;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Button button_ImgThreshold;
        private System.Windows.Forms.Button button_TDI;
    }
}

