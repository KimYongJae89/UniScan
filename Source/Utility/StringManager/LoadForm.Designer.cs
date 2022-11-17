namespace StringManager
{
    partial class LoadForm
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
            this.lblRef = new System.Windows.Forms.Label();
            this.txtRef = new System.Windows.Forms.TextBox();
            this.btnBrowseRef = new System.Windows.Forms.Button();
            this.lblComp = new System.Windows.Forms.Label();
            this.txtComp = new System.Windows.Forms.TextBox();
            this.btnBrowseComp = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblRef
            // 
            this.lblRef.Location = new System.Drawing.Point(12, 14);
            this.lblRef.Name = "lblRef";
            this.lblRef.Size = new System.Drawing.Size(64, 21);
            this.lblRef.TabIndex = 0;
            this.lblRef.Text = "Reference";
            this.lblRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRef
            // 
            this.txtRef.Location = new System.Drawing.Point(82, 14);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(252, 21);
            this.txtRef.TabIndex = 1;
            // 
            // btnBrowseRef
            // 
            this.btnBrowseRef.Location = new System.Drawing.Point(340, 14);
            this.btnBrowseRef.Name = "btnBrowseRef";
            this.btnBrowseRef.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseRef.TabIndex = 2;
            this.btnBrowseRef.Text = "...";
            this.btnBrowseRef.UseVisualStyleBackColor = true;
            this.btnBrowseRef.Click += new System.EventHandler(this.btnBrowseRef_Click);
            // 
            // lblComp
            // 
            this.lblComp.Location = new System.Drawing.Point(12, 47);
            this.lblComp.Name = "lblComp";
            this.lblComp.Size = new System.Drawing.Size(64, 21);
            this.lblComp.TabIndex = 0;
            this.lblComp.Text = "Compare";
            this.lblComp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtComp
            // 
            this.txtComp.Location = new System.Drawing.Point(82, 47);
            this.txtComp.Name = "txtComp";
            this.txtComp.Size = new System.Drawing.Size(252, 21);
            this.txtComp.TabIndex = 1;
            // 
            // btnBrowseComp
            // 
            this.btnBrowseComp.Location = new System.Drawing.Point(340, 46);
            this.btnBrowseComp.Name = "btnBrowseComp";
            this.btnBrowseComp.Size = new System.Drawing.Size(28, 23);
            this.btnBrowseComp.TabIndex = 2;
            this.btnBrowseComp.Text = "...";
            this.btnBrowseComp.UseVisualStyleBackColor = true;
            this.btnBrowseComp.Click += new System.EventHandler(this.btnBrowseComp_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(106, 79);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(217, 79);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 110);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnBrowseComp);
            this.Controls.Add(this.btnBrowseRef);
            this.Controls.Add(this.txtComp);
            this.Controls.Add(this.txtRef);
            this.Controls.Add(this.lblComp);
            this.Controls.Add(this.lblRef);
            this.Name = "LoadForm";
            this.Text = "LoadForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRef;
        private System.Windows.Forms.TextBox txtRef;
        private System.Windows.Forms.Button btnBrowseRef;
        private System.Windows.Forms.Label lblComp;
        private System.Windows.Forms.TextBox txtComp;
        private System.Windows.Forms.Button btnBrowseComp;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}