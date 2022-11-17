namespace PadMonitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel_Top = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dataGridView_LEFT = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_RIGHT = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView_CENTER = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel_Right = new System.Windows.Forms.TableLayoutPanel();
            this.button_PASS = new System.Windows.Forms.Button();
            this.button_Scan = new System.Windows.Forms.Button();
            this.button_NG = new System.Windows.Forms.Button();
            this.button_STOP = new System.Windows.Forms.Button();
            this.tableLayoutPanel_Left = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelLHeader = new System.Windows.Forms.TableLayoutPanel();
            this.panelClock = new System.Windows.Forms.Panel();
            this.panelCompanyLogo = new System.Windows.Forms.Panel();
            this.panelMHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelInfoHeader = new System.Windows.Forms.Panel();
            this.panel_Top.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_LEFT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RIGHT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CENTER)).BeginInit();
            this.tableLayoutPanel_Right.SuspendLayout();
            this.tableLayoutPanel_Left.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.tableLayoutPanelLHeader.SuspendLayout();
            this.panelMHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_Top
            // 
            this.panel_Top.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel_Top.Controls.Add(this.tableLayoutPanel1);
            this.panel_Top.Controls.Add(this.tableLayoutPanel_Right);
            this.panel_Top.Controls.Add(this.tableLayoutPanel_Left);
            this.panel_Top.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Top.Location = new System.Drawing.Point(0, 353);
            this.panel_Top.Name = "panel_Top";
            this.panel_Top.Size = new System.Drawing.Size(2225, 427);
            this.panel_Top.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.label11, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_LEFT, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_RIGHT, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView_CENTER, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(627, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1145, 427);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(765, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(377, 42);
            this.label11.TabIndex = 7;
            this.label11.Text = "RIGHT";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(384, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(375, 42);
            this.label10.TabIndex = 7;
            this.label10.Text = "CENTER";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView_LEFT
            // 
            this.dataGridView_LEFT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_LEFT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView_LEFT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_LEFT.Location = new System.Drawing.Point(3, 45);
            this.dataGridView_LEFT.MultiSelect = false;
            this.dataGridView_LEFT.Name = "dataGridView_LEFT";
            this.dataGridView_LEFT.ReadOnly = true;
            this.dataGridView_LEFT.RowHeadersVisible = false;
            this.dataGridView_LEFT.RowTemplate.Height = 37;
            this.dataGridView_LEFT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_LEFT.Size = new System.Drawing.Size(375, 379);
            this.dataGridView_LEFT.TabIndex = 5;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "NO";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "POS";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 180;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "SIZE";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 180;
            // 
            // dataGridView_RIGHT
            // 
            this.dataGridView_RIGHT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_RIGHT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridView_RIGHT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_RIGHT.Location = new System.Drawing.Point(765, 45);
            this.dataGridView_RIGHT.MultiSelect = false;
            this.dataGridView_RIGHT.Name = "dataGridView_RIGHT";
            this.dataGridView_RIGHT.ReadOnly = true;
            this.dataGridView_RIGHT.RowHeadersVisible = false;
            this.dataGridView_RIGHT.RowTemplate.Height = 37;
            this.dataGridView_RIGHT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_RIGHT.Size = new System.Drawing.Size(377, 379);
            this.dataGridView_RIGHT.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "NO";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "POS";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 180;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "SIZE";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 180;
            // 
            // dataGridView_CENTER
            // 
            this.dataGridView_CENTER.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_CENTER.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dataGridView_CENTER.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_CENTER.Location = new System.Drawing.Point(384, 45);
            this.dataGridView_CENTER.MultiSelect = false;
            this.dataGridView_CENTER.Name = "dataGridView_CENTER";
            this.dataGridView_CENTER.ReadOnly = true;
            this.dataGridView_CENTER.RowHeadersVisible = false;
            this.dataGridView_CENTER.RowTemplate.Height = 37;
            this.dataGridView_CENTER.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_CENTER.Size = new System.Drawing.Size(375, 379);
            this.dataGridView_CENTER.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "NO";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "POS";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 180;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "SIZE";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 180;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(375, 42);
            this.label9.TabIndex = 6;
            this.label9.Text = "LEFT";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel_Right
            // 
            this.tableLayoutPanel_Right.ColumnCount = 2;
            this.tableLayoutPanel_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_Right.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel_Right.Controls.Add(this.button_PASS, 0, 0);
            this.tableLayoutPanel_Right.Controls.Add(this.button_Scan, 1, 0);
            this.tableLayoutPanel_Right.Controls.Add(this.button_NG, 0, 1);
            this.tableLayoutPanel_Right.Controls.Add(this.button_STOP, 1, 1);
            this.tableLayoutPanel_Right.Dock = System.Windows.Forms.DockStyle.Right;
            this.tableLayoutPanel_Right.Location = new System.Drawing.Point(1772, 0);
            this.tableLayoutPanel_Right.Name = "tableLayoutPanel_Right";
            this.tableLayoutPanel_Right.RowCount = 3;
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel_Right.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_Right.Size = new System.Drawing.Size(453, 427);
            this.tableLayoutPanel_Right.TabIndex = 4;
            // 
            // button_PASS
            // 
            this.button_PASS.BackColor = System.Drawing.Color.Lime;
            this.button_PASS.Font = new System.Drawing.Font("휴먼둥근헤드라인", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_PASS.ForeColor = System.Drawing.Color.White;
            this.button_PASS.Location = new System.Drawing.Point(3, 3);
            this.button_PASS.Name = "button_PASS";
            this.button_PASS.Size = new System.Drawing.Size(250, 171);
            this.button_PASS.TabIndex = 1;
            this.button_PASS.Text = "PASS";
            this.button_PASS.UseVisualStyleBackColor = false;
            // 
            // button_Scan
            // 
            this.button_Scan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_Scan.BackgroundImage = global::PadMonitor.Properties.Resources.Start;
            this.button_Scan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Scan.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Scan.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_Scan.Location = new System.Drawing.Point(259, 3);
            this.button_Scan.Name = "button_Scan";
            this.button_Scan.Size = new System.Drawing.Size(174, 171);
            this.button_Scan.TabIndex = 0;
            this.button_Scan.Text = "Scan";
            this.button_Scan.UseVisualStyleBackColor = true;
            this.button_Scan.Click += new System.EventHandler(this.button_Scan_Click);
            // 
            // button_NG
            // 
            this.button_NG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.button_NG.Font = new System.Drawing.Font("휴먼둥근헤드라인", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_NG.ForeColor = System.Drawing.Color.White;
            this.button_NG.Location = new System.Drawing.Point(3, 180);
            this.button_NG.Name = "button_NG";
            this.button_NG.Size = new System.Drawing.Size(250, 171);
            this.button_NG.TabIndex = 1;
            this.button_NG.Text = "NG";
            this.button_NG.UseVisualStyleBackColor = false;
            // 
            // button_STOP
            // 
            this.button_STOP.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_STOP.BackgroundImage = global::PadMonitor.Properties.Resources.Stop;
            this.button_STOP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_STOP.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_STOP.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_STOP.Location = new System.Drawing.Point(259, 180);
            this.button_STOP.Name = "button_STOP";
            this.button_STOP.Size = new System.Drawing.Size(174, 171);
            this.button_STOP.TabIndex = 0;
            this.button_STOP.Text = "Stop";
            this.button_STOP.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel_Left
            // 
            this.tableLayoutPanel_Left.ColumnCount = 2;
            this.tableLayoutPanel_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Left.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel_Left.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel_Left.Controls.Add(this.label8, 1, 2);
            this.tableLayoutPanel_Left.Controls.Add(this.label7, 1, 1);
            this.tableLayoutPanel_Left.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel_Left.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel_Left.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel_Left.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel_Left.Controls.Add(this.label6, 1, 3);
            this.tableLayoutPanel_Left.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanel_Left.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_Left.Name = "tableLayoutPanel_Left";
            this.tableLayoutPanel_Left.RowCount = 4;
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel_Left.Size = new System.Drawing.Size(627, 427);
            this.tableLayoutPanel_Left.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.ForestGreen;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(316, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(308, 106);
            this.label5.TabIndex = 5;
            this.label5.Text = "Status";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Lime;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(316, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(308, 106);
            this.label8.TabIndex = 7;
            this.label8.Text = "123";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.DodgerBlue;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(316, 106);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(308, 106);
            this.label7.TabIndex = 6;
            this.label7.Text = "123";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Green;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 106);
            this.label4.TabIndex = 2;
            this.label4.Text = "Status";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Red;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(307, 109);
            this.label3.TabIndex = 2;
            this.label3.Text = "NG";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.DodgerBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 106);
            this.label1.TabIndex = 2;
            this.label1.Text = "TOTAL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Lime;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 106);
            this.label2.TabIndex = 2;
            this.label2.Text = "PASS";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Red;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 28.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(316, 318);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(308, 109);
            this.label6.TabIndex = 5;
            this.label6.Text = "123";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHeader
            // 
            this.panelHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelHeader.BackgroundImage")));
            this.panelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelHeader.Controls.Add(this.tableLayoutPanelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(2225, 76);
            this.panelHeader.TabIndex = 1;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelHeader.ColumnCount = 3;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.Controls.Add(this.tableLayoutPanelLHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelMHeader, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelInfoHeader, 2, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(2225, 76);
            this.tableLayoutPanelHeader.TabIndex = 2;
            // 
            // tableLayoutPanelLHeader
            // 
            this.tableLayoutPanelLHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanelLHeader.ColumnCount = 2;
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelLHeader.Controls.Add(this.panelClock, 0, 0);
            this.tableLayoutPanelLHeader.Controls.Add(this.panelCompanyLogo, 0, 0);
            this.tableLayoutPanelLHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLHeader.Name = "tableLayoutPanelLHeader";
            this.tableLayoutPanelLHeader.RowCount = 1;
            this.tableLayoutPanelLHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLHeader.Size = new System.Drawing.Size(741, 76);
            this.tableLayoutPanelLHeader.TabIndex = 0;
            // 
            // panelClock
            // 
            this.panelClock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelClock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClock.Location = new System.Drawing.Point(444, 0);
            this.panelClock.Margin = new System.Windows.Forms.Padding(0);
            this.panelClock.Name = "panelClock";
            this.panelClock.Size = new System.Drawing.Size(297, 76);
            this.panelClock.TabIndex = 2;
            // 
            // panelCompanyLogo
            // 
            this.panelCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCompanyLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCompanyLogo.Location = new System.Drawing.Point(0, 0);
            this.panelCompanyLogo.Margin = new System.Windows.Forms.Padding(0);
            this.panelCompanyLogo.Name = "panelCompanyLogo";
            this.panelCompanyLogo.Size = new System.Drawing.Size(444, 76);
            this.panelCompanyLogo.TabIndex = 1;
            // 
            // panelMHeader
            // 
            this.panelMHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMHeader.Controls.Add(this.labelTitle);
            this.panelMHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMHeader.Location = new System.Drawing.Point(741, 0);
            this.panelMHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelMHeader.Name = "panelMHeader";
            this.panelMHeader.Size = new System.Drawing.Size(741, 76);
            this.panelMHeader.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 13.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelTitle.Image = ((System.Drawing.Image)(resources.GetObject("labelTitle.Image")));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(741, 76);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "TAB Monitor";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInfoHeader
            // 
            this.panelInfoHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoHeader.Location = new System.Drawing.Point(1482, 0);
            this.panelInfoHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfoHeader.Name = "panelInfoHeader";
            this.panelInfoHeader.Size = new System.Drawing.Size(743, 76);
            this.panelInfoHeader.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(2225, 780);
            this.Controls.Add(this.panel_Top);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PAD Monitor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.panel_Top.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_LEFT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RIGHT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_CENTER)).EndInit();
            this.tableLayoutPanel_Right.ResumeLayout(false);
            this.tableLayoutPanel_Left.ResumeLayout(false);
            this.tableLayoutPanel_Left.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelLHeader.ResumeLayout(false);
            this.panelMHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLHeader;
        private System.Windows.Forms.Panel panelClock;
        private System.Windows.Forms.Panel panelCompanyLogo;
        private System.Windows.Forms.Panel panelMHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelInfoHeader;
        private System.Windows.Forms.Panel panel_Top;
        private System.Windows.Forms.Button button_Scan;
        private System.Windows.Forms.Button button_STOP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Left;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_NG;
        private System.Windows.Forms.Button button_PASS;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_Right;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dataGridView_LEFT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridView_RIGHT;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridView dataGridView_CENTER;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
    }
}

