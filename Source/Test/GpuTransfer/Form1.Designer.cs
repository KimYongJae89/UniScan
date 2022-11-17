namespace GpuTransfer
{
    partial class Form1
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
            this.buttonMil = new System.Windows.Forms.Button();
            this.buttonEmgu = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonLogClear = new System.Windows.Forms.Button();
            this.buttonLogSave = new System.Windows.Forms.Button();
            this.buttonCuCudas = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonMil
            // 
            this.buttonMil.Location = new System.Drawing.Point(282, 76);
            this.buttonMil.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonMil.Name = "buttonMil";
            this.buttonMil.Size = new System.Drawing.Size(128, 54);
            this.buttonMil.TabIndex = 0;
            this.buttonMil.Text = "MIL";
            this.buttonMil.UseVisualStyleBackColor = true;
            this.buttonMil.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEmgu
            // 
            this.buttonEmgu.Location = new System.Drawing.Point(282, 12);
            this.buttonEmgu.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonEmgu.Name = "buttonEmgu";
            this.buttonEmgu.Size = new System.Drawing.Size(128, 54);
            this.buttonEmgu.TabIndex = 0;
            this.buttonEmgu.Text = "EMGU CV";
            this.buttonEmgu.UseVisualStyleBackColor = true;
            this.buttonEmgu.Click += new System.EventHandler(this.button_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(12, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(263, 249);
            this.propertyGrid1.TabIndex = 2;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 267);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(905, 197);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SizeChanged += new System.EventHandler(this.listView1_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            this.columnHeader1.Width = 130;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Message";
            this.columnHeader2.Width = 500;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Elapsed[ms]";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Elapsed[ms]/GB";
            this.columnHeader4.Width = 130;
            // 
            // buttonLogClear
            // 
            this.buttonLogClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogClear.Location = new System.Drawing.Point(788, 207);
            this.buttonLogClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonLogClear.Name = "buttonLogClear";
            this.buttonLogClear.Size = new System.Drawing.Size(128, 54);
            this.buttonLogClear.TabIndex = 0;
            this.buttonLogClear.Text = "Clear Log";
            this.buttonLogClear.UseVisualStyleBackColor = true;
            this.buttonLogClear.Click += new System.EventHandler(this.buttonLogClear_Click);
            // 
            // buttonLogSave
            // 
            this.buttonLogSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogSave.Location = new System.Drawing.Point(652, 207);
            this.buttonLogSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonLogSave.Name = "buttonLogSave";
            this.buttonLogSave.Size = new System.Drawing.Size(128, 54);
            this.buttonLogSave.TabIndex = 0;
            this.buttonLogSave.Text = "Save Log";
            this.buttonLogSave.UseVisualStyleBackColor = true;
            this.buttonLogSave.Click += new System.EventHandler(this.buttonLogSave_Click);
            // 
            // buttonCuCudas
            // 
            this.buttonCuCudas.Location = new System.Drawing.Point(282, 140);
            this.buttonCuCudas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCuCudas.Name = "buttonCuCudas";
            this.buttonCuCudas.Size = new System.Drawing.Size(128, 54);
            this.buttonCuCudas.TabIndex = 0;
            this.buttonCuCudas.Text = "cuCUDAs";
            this.buttonCuCudas.UseVisualStyleBackColor = true;
            this.buttonCuCudas.Click += new System.EventHandler(this.button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 476);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonEmgu);
            this.Controls.Add(this.buttonLogSave);
            this.Controls.Add(this.buttonLogClear);
            this.Controls.Add(this.buttonCuCudas);
            this.Controls.Add(this.buttonMil);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonMil;
        private System.Windows.Forms.Button buttonEmgu;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button buttonLogClear;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button buttonLogSave;
        private System.Windows.Forms.Button buttonCuCudas;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}

