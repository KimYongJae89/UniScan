using ZedGraph;
namespace UniScanX.MPAlignment.Algo
{
    partial class TestAlgoForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button_LoadImage = new System.Windows.Forms.Button();
            this.button_Inspect = new System.Windows.Forms.Button();
            this.zedGraphControl_X = new ZedGraph.ZedGraphControl();
            this.zedGraphControl_Y = new ZedGraph.ZedGraphControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_Capture = new System.Windows.Forms.Button();
            this.button_ChangeImage = new System.Windows.Forms.Button();
            this.button_ZoomFit = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox_Force = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nudThreshod_X2nd = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudThreshod_X1st = new System.Windows.Forms.NumericUpDown();
            this.nudThreshod_Y2nd = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nud_Margin = new System.Windows.Forms.NumericUpDown();
            this.nudHysterisisY = new System.Windows.Forms.NumericUpDown();
            this.nudHysterisisX = new System.Windows.Forms.NumericUpDown();
            this.nudThreshod_Y1st = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel_Canvas = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button_PairLoad = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X2nd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X1st)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y2nd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Margin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysterisisY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysterisisX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y1st)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_LoadImage
            // 
            this.button_LoadImage.Location = new System.Drawing.Point(6, 6);
            this.button_LoadImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_LoadImage.Name = "button_LoadImage";
            this.button_LoadImage.Size = new System.Drawing.Size(82, 38);
            this.button_LoadImage.TabIndex = 0;
            this.button_LoadImage.Text = "Load";
            this.button_LoadImage.UseVisualStyleBackColor = true;
            this.button_LoadImage.Click += new System.EventHandler(this.button_LoadImage_Click);
            // 
            // button_Inspect
            // 
            this.button_Inspect.Location = new System.Drawing.Point(6, 49);
            this.button_Inspect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Inspect.Name = "button_Inspect";
            this.button_Inspect.Size = new System.Drawing.Size(82, 38);
            this.button_Inspect.TabIndex = 1;
            this.button_Inspect.Text = "Inspect";
            this.button_Inspect.UseVisualStyleBackColor = true;
            this.button_Inspect.Click += new System.EventHandler(this.button_Inspect_Click);
            // 
            // zedGraphControl_X
            // 
            this.zedGraphControl_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl_X.Location = new System.Drawing.Point(522, 393);
            this.zedGraphControl_X.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.zedGraphControl_X.Name = "zedGraphControl_X";
            this.zedGraphControl_X.ScrollGrace = 0D;
            this.zedGraphControl_X.ScrollMaxX = 0D;
            this.zedGraphControl_X.ScrollMaxY = 0D;
            this.zedGraphControl_X.ScrollMaxY2 = 0D;
            this.zedGraphControl_X.ScrollMinX = 0D;
            this.zedGraphControl_X.ScrollMinY = 0D;
            this.zedGraphControl_X.ScrollMinY2 = 0D;
            this.zedGraphControl_X.Size = new System.Drawing.Size(510, 134);
            this.zedGraphControl_X.TabIndex = 2;
            this.zedGraphControl_X.UseExtendedPrintDialog = true;
            // 
            // zedGraphControl_Y
            // 
            this.zedGraphControl_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl_Y.Location = new System.Drawing.Point(522, 253);
            this.zedGraphControl_Y.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.zedGraphControl_Y.Name = "zedGraphControl_Y";
            this.zedGraphControl_Y.ScrollGrace = 0D;
            this.zedGraphControl_Y.ScrollMaxX = 0D;
            this.zedGraphControl_Y.ScrollMaxY = 0D;
            this.zedGraphControl_Y.ScrollMaxY2 = 0D;
            this.zedGraphControl_Y.ScrollMinX = 0D;
            this.zedGraphControl_Y.ScrollMinY = 0D;
            this.zedGraphControl_Y.ScrollMinY2 = 0D;
            this.zedGraphControl_Y.Size = new System.Drawing.Size(510, 134);
            this.zedGraphControl_Y.TabIndex = 2;
            this.zedGraphControl_Y.UseExtendedPrintDialog = true;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.button_Capture);
            this.panel1.Controls.Add(this.button_ChangeImage);
            this.panel1.Controls.Add(this.button_ZoomFit);
            this.panel1.Controls.Add(this.button_Clear);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.checkBox_Force);
            this.panel1.Controls.Add(this.button_Inspect);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nudThreshod_X2nd);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.nudThreshod_X1st);
            this.panel1.Controls.Add(this.nudThreshod_Y2nd);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.button_PairLoad);
            this.panel1.Controls.Add(this.button_LoadImage);
            this.panel1.Controls.Add(this.nud_Margin);
            this.panel1.Controls.Add(this.nudHysterisisY);
            this.panel1.Controls.Add(this.nudHysterisisX);
            this.panel1.Controls.Add(this.nudThreshod_Y1st);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1032, 246);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(483, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 238);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 178;
            this.pictureBox1.TabStop = false;
            // 
            // button_Capture
            // 
            this.button_Capture.Location = new System.Drawing.Point(4, 90);
            this.button_Capture.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Capture.Name = "button_Capture";
            this.button_Capture.Size = new System.Drawing.Size(82, 38);
            this.button_Capture.TabIndex = 177;
            this.button_Capture.Text = "capture";
            this.button_Capture.UseVisualStyleBackColor = true;
            this.button_Capture.Click += new System.EventHandler(this.button_Capture_Click);
            // 
            // button_ChangeImage
            // 
            this.button_ChangeImage.Location = new System.Drawing.Point(88, 174);
            this.button_ChangeImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_ChangeImage.Name = "button_ChangeImage";
            this.button_ChangeImage.Size = new System.Drawing.Size(82, 38);
            this.button_ChangeImage.TabIndex = 176;
            this.button_ChangeImage.Text = "ChageImage";
            this.button_ChangeImage.UseVisualStyleBackColor = true;
            this.button_ChangeImage.Click += new System.EventHandler(this.button_ChangeImage_Click);
            // 
            // button_ZoomFit
            // 
            this.button_ZoomFit.Location = new System.Drawing.Point(3, 174);
            this.button_ZoomFit.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_ZoomFit.Name = "button_ZoomFit";
            this.button_ZoomFit.Size = new System.Drawing.Size(82, 38);
            this.button_ZoomFit.TabIndex = 176;
            this.button_ZoomFit.Text = "Fit";
            this.button_ZoomFit.UseVisualStyleBackColor = true;
            this.button_ZoomFit.Click += new System.EventHandler(this.button_ZoomFit_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Location = new System.Drawing.Point(3, 132);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(82, 38);
            this.button_Clear.TabIndex = 175;
            this.button_Clear.Text = "clear";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // textBox1
            // 
            this.textBox1.AcceptsTab = true;
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox1.Location = new System.Drawing.Point(771, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox1.MaxLength = 327670;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(261, 248);
            this.textBox1.TabIndex = 174;
            // 
            // checkBox_Force
            // 
            this.checkBox_Force.AutoSize = true;
            this.checkBox_Force.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_Force.Location = new System.Drawing.Point(118, 18);
            this.checkBox_Force.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_Force.Name = "checkBox_Force";
            this.checkBox_Force.Size = new System.Drawing.Size(80, 16);
            this.checkBox_Force.TabIndex = 173;
            this.checkBox_Force.Text = "2nd Force";
            this.checkBox_Force.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 12);
            this.label2.TabIndex = 165;
            this.label2.Text = "Second Line";
            // 
            // nudThreshod_X2nd
            // 
            this.nudThreshod_X2nd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_X2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_X2nd.DecimalPlaces = 1;
            this.nudThreshod_X2nd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_X2nd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_X2nd.Location = new System.Drawing.Point(294, 38);
            this.nudThreshod_X2nd.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudThreshod_X2nd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_X2nd.Name = "nudThreshod_X2nd";
            this.nudThreshod_X2nd.Size = new System.Drawing.Size(79, 21);
            this.nudThreshod_X2nd.TabIndex = 169;
            this.nudThreshod_X2nd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_X2nd.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 166;
            this.label1.Text = "First Line";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(132, 38);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 15);
            this.label10.TabIndex = 172;
            this.label10.Text = "horizontal";
            // 
            // nudThreshod_X1st
            // 
            this.nudThreshod_X1st.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_X1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_X1st.DecimalPlaces = 1;
            this.nudThreshod_X1st.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_X1st.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_X1st.Location = new System.Drawing.Point(210, 38);
            this.nudThreshod_X1st.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudThreshod_X1st.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_X1st.Name = "nudThreshod_X1st";
            this.nudThreshod_X1st.Size = new System.Drawing.Size(79, 21);
            this.nudThreshod_X1st.TabIndex = 167;
            this.nudThreshod_X1st.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_X1st.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // nudThreshod_Y2nd
            // 
            this.nudThreshod_Y2nd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_Y2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_Y2nd.DecimalPlaces = 1;
            this.nudThreshod_Y2nd.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_Y2nd.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_Y2nd.Location = new System.Drawing.Point(294, 62);
            this.nudThreshod_Y2nd.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudThreshod_Y2nd.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_Y2nd.Name = "nudThreshod_Y2nd";
            this.nudThreshod_Y2nd.Size = new System.Drawing.Size(79, 21);
            this.nudThreshod_Y2nd.TabIndex = 170;
            this.nudThreshod_Y2nd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_Y2nd.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(104, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 171;
            this.label3.Text = "Estimate Margin";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(394, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 171;
            this.label4.Text = "Hysterisis";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(149, 60);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 15);
            this.label9.TabIndex = 171;
            this.label9.Text = "Vertical";
            // 
            // nud_Margin
            // 
            this.nud_Margin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nud_Margin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nud_Margin.DecimalPlaces = 1;
            this.nud_Margin.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nud_Margin.Location = new System.Drawing.Point(209, 97);
            this.nud_Margin.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nud_Margin.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_Margin.Name = "nud_Margin";
            this.nud_Margin.Size = new System.Drawing.Size(79, 21);
            this.nud_Margin.TabIndex = 168;
            this.nud_Margin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nud_Margin.Value = new decimal(new int[] {
            250,
            0,
            0,
            0});
            // 
            // nudHysterisisY
            // 
            this.nudHysterisisY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudHysterisisY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHysterisisY.DecimalPlaces = 1;
            this.nudHysterisisY.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudHysterisisY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudHysterisisY.Location = new System.Drawing.Point(383, 62);
            this.nudHysterisisY.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudHysterisisY.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHysterisisY.Name = "nudHysterisisY";
            this.nudHysterisisY.Size = new System.Drawing.Size(79, 21);
            this.nudHysterisisY.TabIndex = 168;
            this.nudHysterisisY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHysterisisY.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // nudHysterisisX
            // 
            this.nudHysterisisX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudHysterisisX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudHysterisisX.DecimalPlaces = 1;
            this.nudHysterisisX.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudHysterisisX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudHysterisisX.Location = new System.Drawing.Point(383, 38);
            this.nudHysterisisX.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudHysterisisX.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHysterisisX.Name = "nudHysterisisX";
            this.nudHysterisisX.Size = new System.Drawing.Size(79, 21);
            this.nudHysterisisX.TabIndex = 168;
            this.nudHysterisisX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudHysterisisX.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // nudThreshod_Y1st
            // 
            this.nudThreshod_Y1st.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudThreshod_Y1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nudThreshod_Y1st.DecimalPlaces = 1;
            this.nudThreshod_Y1st.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.nudThreshod_Y1st.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThreshod_Y1st.Location = new System.Drawing.Point(210, 62);
            this.nudThreshod_Y1st.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.nudThreshod_Y1st.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshod_Y1st.Name = "nudThreshod_Y1st";
            this.nudThreshod_Y1st.Size = new System.Drawing.Size(79, 21);
            this.nudThreshod_Y1st.TabIndex = 168;
            this.nudThreshod_Y1st.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudThreshod_Y1st.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControl_Y, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControl_X, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel_Canvas, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1036, 530);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel_Canvas
            // 
            this.panel_Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Canvas.Location = new System.Drawing.Point(2, 252);
            this.panel_Canvas.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel_Canvas.Name = "panel_Canvas";
            this.tableLayoutPanel1.SetRowSpan(this.panel_Canvas, 2);
            this.panel_Canvas.Size = new System.Drawing.Size(514, 276);
            this.panel_Canvas.TabIndex = 4;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // button_PairLoad
            // 
            this.button_PairLoad.Location = new System.Drawing.Point(357, 174);
            this.button_PairLoad.Margin = new System.Windows.Forms.Padding(2);
            this.button_PairLoad.Name = "button_PairLoad";
            this.button_PairLoad.Size = new System.Drawing.Size(82, 38);
            this.button_PairLoad.TabIndex = 0;
            this.button_PairLoad.Text = "PairLoad";
            this.button_PairLoad.UseVisualStyleBackColor = true;
            this.button_PairLoad.Click += new System.EventHandler(this.button_PairLoad_Click);
            // 
            // TestAlgoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 530);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "TestAlgoForm";
            this.Text = "TestAlgoForm";
            this.Load += new System.EventHandler(this.TestAlgoForm_Load);
            this.SizeChanged += new System.EventHandler(this.TestAlgoForm_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X2nd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_X1st)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y2nd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Margin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysterisisY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysterisisX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshod_Y1st)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_LoadImage;
        private System.Windows.Forms.Button button_Inspect;
        private ZedGraph.ZedGraphControl zedGraphControl_X;
        private ZedGraph.ZedGraphControl zedGraphControl_Y;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel_Canvas;
        private System.Windows.Forms.CheckBox checkBox_Force;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudThreshod_X2nd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudThreshod_X1st;
        private System.Windows.Forms.NumericUpDown nudThreshod_Y2nd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudThreshod_Y1st;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_Clear;
        private System.Windows.Forms.Button button_ZoomFit;
        private System.Windows.Forms.Button button_Capture;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nud_Margin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudHysterisisY;
        private System.Windows.Forms.NumericUpDown nudHysterisisX;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button button_ChangeImage;
        private System.Windows.Forms.Button button_PairLoad;
    }
}