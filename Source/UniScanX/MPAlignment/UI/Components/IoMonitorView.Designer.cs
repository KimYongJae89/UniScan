
namespace UniScanX.MPAlignment.UI.Components
{
    partial class IoMonitorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IoMonitorView));
            this.themeForm1 = new ReaLTaiizor.ThemeForm();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.dgvOutPorts = new System.Windows.Forms.DataGridView();
            this.ColumnInputNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInputValue = new System.Windows.Forms.DataGridViewImageColumn();
            this.dgvInPorts = new System.Windows.Forms.DataGridView();
            this.ColumnOutputNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlConv = new System.Windows.Forms.Panel();
            this.btnStopFlush = new ReaLTaiizor.HopeButton();
            this.btnStartFlush = new ReaLTaiizor.HopeButton();
            this.btnEjectPcb = new ReaLTaiizor.HopeButton();
            this.btnStopReceive = new ReaLTaiizor.HopeButton();
            this.btnReceiveBackOnce = new ReaLTaiizor.HopeButton();
            this.btnReceiveOnce = new ReaLTaiizor.HopeButton();
            this.btnReceiveBackMulti = new ReaLTaiizor.HopeButton();
            this.btnReceiveMulti = new ReaLTaiizor.HopeButton();
            this.themeForm1.SuspendLayout();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutPorts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInPorts)).BeginInit();
            this.pnlConv.SuspendLayout();
            this.SuspendLayout();
            // 
            // themeForm1
            // 
            this.themeForm1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.themeForm1.Controls.Add(this.tlpMain);
            this.themeForm1.Controls.Add(this.btnClose);
            this.themeForm1.Controls.Add(this.pnlConv);
            this.themeForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themeForm1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.themeForm1.Image = ((System.Drawing.Image)(resources.GetObject("themeForm1.Image")));
            this.themeForm1.Location = new System.Drawing.Point(0, 0);
            this.themeForm1.Name = "themeForm1";
            this.themeForm1.Padding = new System.Windows.Forms.Padding(10, 70, 10, 9);
            this.themeForm1.RoundCorners = true;
            this.themeForm1.Sizable = true;
            this.themeForm1.Size = new System.Drawing.Size(919, 670);
            this.themeForm1.SmartBounds = true;
            this.themeForm1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.themeForm1.TabIndex = 0;
            this.themeForm1.Text = "I/O Monitor";
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.dgvOutPorts, 0, 0);
            this.tlpMain.Controls.Add(this.dgvInPorts, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(10, 70);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 1;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Size = new System.Drawing.Size(899, 498);
            this.tlpMain.TabIndex = 2;
            // 
            // dgvOutPorts
            // 
            this.dgvOutPorts.AllowUserToAddRows = false;
            this.dgvOutPorts.AllowUserToDeleteRows = false;
            this.dgvOutPorts.AllowUserToResizeColumns = false;
            this.dgvOutPorts.AllowUserToResizeRows = false;
            this.dgvOutPorts.ColumnHeadersHeight = 40;
            this.dgvOutPorts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnInputNo,
            this.ColumnInputName,
            this.ColumnInputValue});
            this.dgvOutPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutPorts.Location = new System.Drawing.Point(453, 5);
            this.dgvOutPorts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvOutPorts.MultiSelect = false;
            this.dgvOutPorts.Name = "dgvOutPorts";
            this.dgvOutPorts.ReadOnly = true;
            this.dgvOutPorts.RowHeadersVisible = false;
            this.dgvOutPorts.RowTemplate.Height = 23;
            this.dgvOutPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOutPorts.Size = new System.Drawing.Size(442, 488);
            this.dgvOutPorts.TabIndex = 1;
            this.dgvOutPorts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutPorts_CellClick);
            // 
            // ColumnInputNo
            // 
            this.ColumnInputNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnInputNo.HeaderText = "No";
            this.ColumnInputNo.Name = "ColumnInputNo";
            this.ColumnInputNo.ReadOnly = true;
            this.ColumnInputNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnInputNo.Width = 40;
            // 
            // ColumnInputName
            // 
            this.ColumnInputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnInputName.FillWeight = 155.8074F;
            this.ColumnInputName.HeaderText = "Name";
            this.ColumnInputName.Name = "ColumnInputName";
            this.ColumnInputName.ReadOnly = true;
            this.ColumnInputName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnInputValue
            // 
            this.ColumnInputValue.FillWeight = 44.19263F;
            this.ColumnInputValue.HeaderText = "Value";
            this.ColumnInputValue.Name = "ColumnInputValue";
            this.ColumnInputValue.ReadOnly = true;
            this.ColumnInputValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnInputValue.Width = 78;
            // 
            // dgvInPorts
            // 
            this.dgvInPorts.AllowUserToAddRows = false;
            this.dgvInPorts.AllowUserToDeleteRows = false;
            this.dgvInPorts.AllowUserToResizeColumns = false;
            this.dgvInPorts.AllowUserToResizeRows = false;
            this.dgvInPorts.ColumnHeadersHeight = 40;
            this.dgvInPorts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnOutputNo,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvInPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInPorts.Location = new System.Drawing.Point(4, 5);
            this.dgvInPorts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dgvInPorts.MultiSelect = false;
            this.dgvInPorts.Name = "dgvInPorts";
            this.dgvInPorts.ReadOnly = true;
            this.dgvInPorts.RowHeadersVisible = false;
            this.dgvInPorts.RowTemplate.Height = 23;
            this.dgvInPorts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInPorts.Size = new System.Drawing.Size(441, 488);
            this.dgvInPorts.TabIndex = 2;
            // 
            // ColumnOutputNo
            // 
            this.ColumnOutputNo.HeaderText = "No";
            this.ColumnOutputNo.Name = "ColumnOutputNo";
            this.ColumnOutputNo.ReadOnly = true;
            this.ColumnOutputNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnOutputNo.Width = 40;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.Width = 78;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(871, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(32, 32);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlConv
            // 
            this.pnlConv.Controls.Add(this.btnStopFlush);
            this.pnlConv.Controls.Add(this.btnStartFlush);
            this.pnlConv.Controls.Add(this.btnEjectPcb);
            this.pnlConv.Controls.Add(this.btnStopReceive);
            this.pnlConv.Controls.Add(this.btnReceiveBackOnce);
            this.pnlConv.Controls.Add(this.btnReceiveOnce);
            this.pnlConv.Controls.Add(this.btnReceiveBackMulti);
            this.pnlConv.Controls.Add(this.btnReceiveMulti);
            this.pnlConv.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlConv.Location = new System.Drawing.Point(10, 568);
            this.pnlConv.Name = "pnlConv";
            this.pnlConv.Size = new System.Drawing.Size(899, 93);
            this.pnlConv.TabIndex = 3;
            // 
            // btnStopFlush
            // 
            this.btnStopFlush.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnStopFlush.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnStopFlush.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopFlush.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnStopFlush.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnStopFlush.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnStopFlush.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnStopFlush.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnStopFlush.Location = new System.Drawing.Point(780, 50);
            this.btnStopFlush.Name = "btnStopFlush";
            this.btnStopFlush.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnStopFlush.Size = new System.Drawing.Size(100, 40);
            this.btnStopFlush.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnStopFlush.TabIndex = 7;
            this.btnStopFlush.Text = "Stop Flush";
            this.btnStopFlush.TextColor = System.Drawing.Color.White;
            this.btnStopFlush.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnStopFlush.Click += new System.EventHandler(this.btnStopFlush_Click);
            // 
            // btnStartFlush
            // 
            this.btnStartFlush.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnStartFlush.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnStartFlush.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStartFlush.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnStartFlush.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnStartFlush.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnStartFlush.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnStartFlush.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnStartFlush.Location = new System.Drawing.Point(780, 6);
            this.btnStartFlush.Name = "btnStartFlush";
            this.btnStartFlush.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnStartFlush.Size = new System.Drawing.Size(100, 40);
            this.btnStartFlush.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnStartFlush.TabIndex = 7;
            this.btnStartFlush.Text = "Start Flush";
            this.btnStartFlush.TextColor = System.Drawing.Color.White;
            this.btnStartFlush.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnStartFlush.Click += new System.EventHandler(this.btnStartFlush_Click);
            // 
            // btnEjectPcb
            // 
            this.btnEjectPcb.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnEjectPcb.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnEjectPcb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEjectPcb.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnEjectPcb.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnEjectPcb.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnEjectPcb.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnEjectPcb.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnEjectPcb.Location = new System.Drawing.Point(674, 26);
            this.btnEjectPcb.Name = "btnEjectPcb";
            this.btnEjectPcb.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnEjectPcb.Size = new System.Drawing.Size(100, 40);
            this.btnEjectPcb.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnEjectPcb.TabIndex = 6;
            this.btnEjectPcb.Text = "Eject PCB";
            this.btnEjectPcb.TextColor = System.Drawing.Color.White;
            this.btnEjectPcb.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnEjectPcb.Click += new System.EventHandler(this.btnEjectPcb_Click);
            // 
            // btnStopReceive
            // 
            this.btnStopReceive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnStopReceive.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnStopReceive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStopReceive.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnStopReceive.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnStopReceive.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnStopReceive.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnStopReceive.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnStopReceive.Location = new System.Drawing.Point(548, 26);
            this.btnStopReceive.Name = "btnStopReceive";
            this.btnStopReceive.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnStopReceive.Size = new System.Drawing.Size(120, 40);
            this.btnStopReceive.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnStopReceive.TabIndex = 5;
            this.btnStopReceive.Text = "Stop Receive";
            this.btnStopReceive.TextColor = System.Drawing.Color.White;
            this.btnStopReceive.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnStopReceive.Click += new System.EventHandler(this.btnStopReceive_Click);
            // 
            // btnReceiveBackOnce
            // 
            this.btnReceiveBackOnce.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnReceiveBackOnce.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnReceiveBackOnce.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceiveBackOnce.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnReceiveBackOnce.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnReceiveBackOnce.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnReceiveBackOnce.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnReceiveBackOnce.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnReceiveBackOnce.Location = new System.Drawing.Point(257, 26);
            this.btnReceiveBackOnce.Name = "btnReceiveBackOnce";
            this.btnReceiveBackOnce.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnReceiveBackOnce.Size = new System.Drawing.Size(120, 40);
            this.btnReceiveBackOnce.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnReceiveBackOnce.TabIndex = 4;
            this.btnReceiveBackOnce.Text = "Receive Back";
            this.btnReceiveBackOnce.TextColor = System.Drawing.Color.White;
            this.btnReceiveBackOnce.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReceiveBackOnce.Click += new System.EventHandler(this.btnReceiveBackOnce_Click);
            // 
            // btnReceiveOnce
            // 
            this.btnReceiveOnce.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnReceiveOnce.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnReceiveOnce.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceiveOnce.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnReceiveOnce.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnReceiveOnce.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnReceiveOnce.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnReceiveOnce.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnReceiveOnce.Location = new System.Drawing.Point(16, 26);
            this.btnReceiveOnce.Name = "btnReceiveOnce";
            this.btnReceiveOnce.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnReceiveOnce.Size = new System.Drawing.Size(102, 40);
            this.btnReceiveOnce.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnReceiveOnce.TabIndex = 2;
            this.btnReceiveOnce.Text = "Receive";
            this.btnReceiveOnce.TextColor = System.Drawing.Color.White;
            this.btnReceiveOnce.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReceiveOnce.Click += new System.EventHandler(this.btnReceiveOnce_Click);
            // 
            // btnReceiveBackMulti
            // 
            this.btnReceiveBackMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnReceiveBackMulti.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnReceiveBackMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceiveBackMulti.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnReceiveBackMulti.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnReceiveBackMulti.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnReceiveBackMulti.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnReceiveBackMulti.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnReceiveBackMulti.Location = new System.Drawing.Point(383, 26);
            this.btnReceiveBackMulti.Name = "btnReceiveBackMulti";
            this.btnReceiveBackMulti.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnReceiveBackMulti.Size = new System.Drawing.Size(159, 40);
            this.btnReceiveBackMulti.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnReceiveBackMulti.TabIndex = 1;
            this.btnReceiveBackMulti.Text = "Receive Back Multi";
            this.btnReceiveBackMulti.TextColor = System.Drawing.Color.White;
            this.btnReceiveBackMulti.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReceiveBackMulti.Click += new System.EventHandler(this.btnReceiveBackMulti_Click);
            // 
            // btnReceiveMulti
            // 
            this.btnReceiveMulti.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnReceiveMulti.ButtonType = ReaLTaiizor.HopeButtonType.Primary;
            this.btnReceiveMulti.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReceiveMulti.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnReceiveMulti.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnReceiveMulti.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnReceiveMulti.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.btnReceiveMulti.InfoColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(147)))), ((int)(((byte)(153)))));
            this.btnReceiveMulti.Location = new System.Drawing.Point(124, 26);
            this.btnReceiveMulti.Name = "btnReceiveMulti";
            this.btnReceiveMulti.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.btnReceiveMulti.Size = new System.Drawing.Size(127, 40);
            this.btnReceiveMulti.SuccessColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(194)))), ((int)(((byte)(58)))));
            this.btnReceiveMulti.TabIndex = 1;
            this.btnReceiveMulti.Text = "Receive Multi";
            this.btnReceiveMulti.TextColor = System.Drawing.Color.White;
            this.btnReceiveMulti.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(162)))), ((int)(((byte)(60)))));
            this.btnReceiveMulti.Click += new System.EventHandler(this.btnReceiveMulti_Click);
            // 
            // IoMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 670);
            this.Controls.Add(this.themeForm1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 61);
            this.Name = "IoMonitorView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "I/O Monitor";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.Load += new System.EventHandler(this.IoMonitorView_Load);
            this.themeForm1.ResumeLayout(false);
            this.tlpMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutPorts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInPorts)).EndInit();
            this.pnlConv.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.ThemeForm themeForm1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.DataGridView dgvOutPorts;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInputNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInputName;
        private System.Windows.Forms.DataGridViewImageColumn ColumnInputValue;
        private System.Windows.Forms.DataGridView dgvInPorts;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnOutputNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Panel pnlConv;
        private ReaLTaiizor.HopeButton btnReceiveMulti;
        private ReaLTaiizor.HopeButton btnReceiveOnce;
        private ReaLTaiizor.HopeButton btnReceiveBackOnce;
        private ReaLTaiizor.HopeButton btnStopReceive;
        private ReaLTaiizor.HopeButton btnEjectPcb;
        private ReaLTaiizor.HopeButton btnReceiveBackMulti;
        private ReaLTaiizor.HopeButton btnStopFlush;
        private ReaLTaiizor.HopeButton btnStartFlush;
    }
}