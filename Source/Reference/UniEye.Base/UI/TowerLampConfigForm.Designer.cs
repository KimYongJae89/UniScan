namespace UniEye.Base.UI
{
    partial class TowerLampConfigForm
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
            this.towerLampStateView = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GreenOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GreenBlink = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.YellowOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.YellowBlink = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RedOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RedBlink = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BuzzerOn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BuzzerBlink = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.towerLampStateView)).BeginInit();
            this.SuspendLayout();
            // 
            // towerLampStateView
            // 
            this.towerLampStateView.AllowUserToAddRows = false;
            this.towerLampStateView.AllowUserToDeleteRows = false;
            this.towerLampStateView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.towerLampStateView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.GreenOn,
            this.GreenBlink,
            this.YellowOn,
            this.YellowBlink,
            this.RedOn,
            this.RedBlink,
            this.BuzzerOn,
            this.BuzzerBlink});
            this.towerLampStateView.Dock = System.Windows.Forms.DockStyle.Top;
            this.towerLampStateView.Location = new System.Drawing.Point(0, 0);
            this.towerLampStateView.Name = "towerLampStateView";
            this.towerLampStateView.RowHeadersVisible = false;
            this.towerLampStateView.RowTemplate.Height = 23;
            this.towerLampStateView.Size = new System.Drawing.Size(707, 171);
            this.towerLampStateView.TabIndex = 0;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // GreenOn
            // 
            this.GreenOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GreenOn.HeaderText = "Green On";
            this.GreenOn.Name = "GreenOn";
            this.GreenOn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GreenOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // GreenBlink
            // 
            this.GreenBlink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.GreenBlink.HeaderText = "Green Blink";
            this.GreenBlink.Name = "GreenBlink";
            this.GreenBlink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GreenBlink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // YellowOn
            // 
            this.YellowOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.YellowOn.HeaderText = "Yellow On";
            this.YellowOn.Name = "YellowOn";
            this.YellowOn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.YellowOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // YellowBlink
            // 
            this.YellowBlink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.YellowBlink.HeaderText = "Yellow Blink";
            this.YellowBlink.Name = "YellowBlink";
            this.YellowBlink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.YellowBlink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // RedOn
            // 
            this.RedOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RedOn.HeaderText = "Red On";
            this.RedOn.Name = "RedOn";
            this.RedOn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.RedOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // RedBlink
            // 
            this.RedBlink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RedBlink.HeaderText = "Red Blink";
            this.RedBlink.Name = "RedBlink";
            // 
            // BuzzerOn
            // 
            this.BuzzerOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BuzzerOn.HeaderText = "Buzzer On";
            this.BuzzerOn.Name = "BuzzerOn";
            this.BuzzerOn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BuzzerOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // BuzzerBlink
            // 
            this.BuzzerBlink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BuzzerBlink.HeaderText = "Buzzer Blink";
            this.BuzzerBlink.Name = "BuzzerBlink";
            this.BuzzerBlink.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BuzzerBlink.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(257, 188);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(93, 32);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(356, 188);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(93, 32);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // TowerLampConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 232);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.towerLampStateView);
            this.Name = "TowerLampConfigForm";
            this.Text = "TowerLampConfigForm";
            this.Load += new System.EventHandler(this.TowerLampConfigForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.towerLampStateView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView towerLampStateView;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewCheckBoxColumn GreenOn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn GreenBlink;
        private System.Windows.Forms.DataGridViewCheckBoxColumn YellowOn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn YellowBlink;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RedOn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn RedBlink;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuzzerOn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn BuzzerBlink;
    }
}