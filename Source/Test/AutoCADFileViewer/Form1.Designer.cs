namespace AutoCADFileViewer
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
            this.pictureBoxCADImage = new System.Windows.Forms.PictureBox();
            this.labelSaveInfo = new System.Windows.Forms.Label();
            this.groupBoxLayerInformation = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.labelScale = new System.Windows.Forms.Label();
            this.textBoxScale = new System.Windows.Forms.TextBox();
            this.labelRect = new System.Windows.Forms.Label();
            this.buttonCadFileLoad = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCADImage)).BeginInit();
            this.groupBoxLayerInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxCADImage
            // 
            this.pictureBoxCADImage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxCADImage.Location = new System.Drawing.Point(217, 12);
            this.pictureBoxCADImage.Name = "pictureBoxCADImage";
            this.pictureBoxCADImage.Size = new System.Drawing.Size(786, 940);
            this.pictureBoxCADImage.TabIndex = 1;
            this.pictureBoxCADImage.TabStop = false;
            // 
            // labelSaveInfo
            // 
            this.labelSaveInfo.AutoSize = true;
            this.labelSaveInfo.Location = new System.Drawing.Point(8, 71);
            this.labelSaveInfo.Name = "labelSaveInfo";
            this.labelSaveInfo.Size = new System.Drawing.Size(48, 12);
            this.labelSaveInfo.TabIndex = 3;
            this.labelSaveInfo.Text = "Content";
            // 
            // groupBoxLayerInformation
            // 
            this.groupBoxLayerInformation.Controls.Add(this.buttonSave);
            this.groupBoxLayerInformation.Controls.Add(this.label1);
            this.groupBoxLayerInformation.Controls.Add(this.labelScale);
            this.groupBoxLayerInformation.Controls.Add(this.labelSaveInfo);
            this.groupBoxLayerInformation.Controls.Add(this.textBoxScale);
            this.groupBoxLayerInformation.Location = new System.Drawing.Point(12, 173);
            this.groupBoxLayerInformation.Name = "groupBoxLayerInformation";
            this.groupBoxLayerInformation.Size = new System.Drawing.Size(183, 274);
            this.groupBoxLayerInformation.TabIndex = 4;
            this.groupBoxLayerInformation.TabStop = false;
            this.groupBoxLayerInformation.Text = "Image Save Option";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(9, 148);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(155, 44);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Scale:";
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(120, 40);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(57, 12);
            this.labelScale.TabIndex = 4;
            this.labelScale.Text = "Pixel/um";
            // 
            // textBoxScale
            // 
            this.textBoxScale.Location = new System.Drawing.Point(50, 36);
            this.textBoxScale.Name = "textBoxScale";
            this.textBoxScale.Size = new System.Drawing.Size(67, 21);
            this.textBoxScale.TabIndex = 4;
            this.textBoxScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxScale.TextChanged += new System.EventHandler(this.textBoxScale_TextChanged);
            // 
            // labelRect
            // 
            this.labelRect.AutoSize = true;
            this.labelRect.Location = new System.Drawing.Point(20, 91);
            this.labelRect.Name = "labelRect";
            this.labelRect.Size = new System.Drawing.Size(65, 12);
            this.labelRect.TabIndex = 3;
            this.labelRect.Text = "ImageSize";
            // 
            // buttonCadFileLoad
            // 
            this.buttonCadFileLoad.Location = new System.Drawing.Point(15, 12);
            this.buttonCadFileLoad.Name = "buttonCadFileLoad";
            this.buttonCadFileLoad.Size = new System.Drawing.Size(180, 65);
            this.buttonCadFileLoad.TabIndex = 5;
            this.buttonCadFileLoad.Text = "CAD File Load";
            this.buttonCadFileLoad.UseVisualStyleBackColor = true;
            this.buttonCadFileLoad.Click += new System.EventHandler(this.buttonCadFileLoad_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 961);
            this.Controls.Add(this.buttonCadFileLoad);
            this.Controls.Add(this.groupBoxLayerInformation);
            this.Controls.Add(this.pictureBoxCADImage);
            this.Controls.Add(this.labelRect);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCADImage)).EndInit();
            this.groupBoxLayerInformation.ResumeLayout(false);
            this.groupBoxLayerInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBoxCADImage;
        private System.Windows.Forms.Label labelSaveInfo;
        private System.Windows.Forms.GroupBox groupBoxLayerInformation;
        private System.Windows.Forms.TextBox textBoxScale;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Label labelRect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCadFileLoad;
        private System.Windows.Forms.Button buttonSave;
    }
}

