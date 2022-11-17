namespace ConditionEditor.Condition
{
    partial class ConditionEditForm
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
            this.labelInputType = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.labelOutputType = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelInputType
            // 
            this.labelInputType.AutoSize = true;
            this.labelInputType.Location = new System.Drawing.Point(14, 28);
            this.labelInputType.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelInputType.Name = "labelInputType";
            this.labelInputType.Size = new System.Drawing.Size(104, 25);
            this.labelInputType.TabIndex = 0;
            this.labelInputType.Text = "Input Type";
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Location = new System.Drawing.Point(14, 75);
            this.labelOperator.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(90, 25);
            this.labelOperator.TabIndex = 1;
            this.labelOperator.Text = "Operator";
            // 
            // labelOutputType
            // 
            this.labelOutputType.AutoSize = true;
            this.labelOutputType.Location = new System.Drawing.Point(14, 122);
            this.labelOutputType.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelOutputType.Name = "labelOutputType";
            this.labelOutputType.Size = new System.Drawing.Size(121, 25);
            this.labelOutputType.TabIndex = 1;
            this.labelOutputType.Text = "Output Type";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(146, 25);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(134, 33);
            this.comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(146, 72);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(134, 33);
            this.comboBox2.TabIndex = 2;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(146, 119);
            this.comboBox3.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(134, 33);
            this.comboBox3.TabIndex = 2;
            // 
            // ConditionEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 170);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelOutputType);
            this.Controls.Add(this.labelOperator);
            this.Controls.Add(this.labelInputType);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "ConditionEditForm";
            this.Text = "ConditionEditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInputType;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.Label labelOutputType;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
    }
}