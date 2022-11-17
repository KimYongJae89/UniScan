namespace DynMvp.Devices.FrameGrabber.UI
{
    partial class MatroxBoardListForm
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.cameraInfoGrid = new System.Windows.Forms.DataGridView();
            this.columnSystemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnSystemNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDigitizerNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCameraType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnClientType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnDcfName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(650, 217);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 33);
            this.buttonCancel.TabIndex = 166;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(542, 217);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 33);
            this.buttonOK.TabIndex = 167;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // cameraInfoGrid
            // 
            this.cameraInfoGrid.AllowUserToAddRows = false;
            this.cameraInfoGrid.AllowUserToDeleteRows = false;
            this.cameraInfoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cameraInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cameraInfoGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSystemType,
            this.columnSystemNum,
            this.columnDigitizerNum,
            this.columnCameraType,
            this.columnClientType,
            this.columnDcfName});
            this.cameraInfoGrid.Location = new System.Drawing.Point(13, 13);
            this.cameraInfoGrid.Margin = new System.Windows.Forms.Padding(4);
            this.cameraInfoGrid.Name = "cameraInfoGrid";
            this.cameraInfoGrid.RowHeadersVisible = false;
            this.cameraInfoGrid.RowTemplate.Height = 23;
            this.cameraInfoGrid.Size = new System.Drawing.Size(744, 199);
            this.cameraInfoGrid.TabIndex = 165;
            // 
            // columnSystemType
            // 
            this.columnSystemType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSystemType.HeaderText = "SystemType";
            this.columnSystemType.Name = "columnSystemType";
            this.columnSystemType.Width = 96;
            // 
            // columnSystemNum
            // 
            this.columnSystemNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSystemNum.HeaderText = "SystemNum";
            this.columnSystemNum.Name = "columnSystemNum";
            this.columnSystemNum.Width = 115;
            // 
            // columnDigitizerNum
            // 
            this.columnDigitizerNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDigitizerNum.HeaderText = "DigitizerNum";
            this.columnDigitizerNum.Name = "columnDigitizerNum";
            this.columnDigitizerNum.Width = 118;
            // 
            // columnCameraType
            // 
            this.columnCameraType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCameraType.HeaderText = "CameraType";
            this.columnCameraType.Name = "columnCameraType";
            this.columnCameraType.Width = 99;
            // 
            // columnClientType
            // 
            this.columnClientType.HeaderText = "ClientType";
            this.columnClientType.Name = "columnClientType";
            // 
            // columnDcfName
            // 
            this.columnDcfName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDcfName.HeaderText = "DcfName";
            this.columnDcfName.Name = "columnDcfName";
            // 
            // MatroxBoardListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 263);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.cameraInfoGrid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MatroxBoardListForm";
            this.Text = "Matrox Board List";
            this.Load += new System.EventHandler(this.MatroxBoardListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cameraInfoGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.DataGridView cameraInfoGrid;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnSystemType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSystemNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDigitizerNum;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnCameraType;
        private System.Windows.Forms.DataGridViewComboBoxColumn columnClientType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDcfName;
    }
}