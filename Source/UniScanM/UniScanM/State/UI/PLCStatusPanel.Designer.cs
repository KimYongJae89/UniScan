namespace UniScanM.UI
{
    partial class PLCStatusPanel
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.statusPLC = new System.Windows.Forms.StatusStrip();
            this.labelConnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelPinhole = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelRVMS = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelColorSensor = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelEDMS = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelCEDMS = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStopImageOn = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelGloss = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelRewinderCut = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelSPSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelPVSpeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelPVPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelNull = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelModel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelLot = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelWorker = new System.Windows.Forms.ToolStripStatusLabel();
            this.checkTimer = new System.Windows.Forms.Timer(this.components);
            this.statusPLC.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusPLC
            // 
            this.statusPLC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelConnected,
            this.labelPinhole,
            this.labelRVMS,
            this.labelColorSensor,
            this.labelEDMS,
            this.labelCEDMS,
            this.labelStopImageOn,
            this.labelGloss,
            this.labelRewinderCut,
            this.labelSPSpeed,
            this.labelPVSpeed,
            this.labelPVPos,
            this.labelNull,
            this.labelModel,
            this.labelLot,
            this.labelWorker});
            this.statusPLC.Location = new System.Drawing.Point(0, 1);
            this.statusPLC.Name = "statusPLC";
            this.statusPLC.Size = new System.Drawing.Size(1451, 24);
            this.statusPLC.TabIndex = 0;
            this.statusPLC.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip_ItemClicked);
            // 
            // labelConnected
            // 
            this.labelConnected.AutoSize = false;
            this.labelConnected.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelConnected.Name = "labelConnected";
            this.labelConnected.Size = new System.Drawing.Size(90, 19);
            this.labelConnected.Text = "Connected";
            // 
            // labelPinhole
            // 
            this.labelPinhole.AutoSize = false;
            this.labelPinhole.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelPinhole.Name = "labelPinhole";
            this.labelPinhole.Size = new System.Drawing.Size(90, 19);
            this.labelPinhole.Text = "Pinhole";
            // 
            // labelRVMS
            // 
            this.labelRVMS.AutoSize = false;
            this.labelRVMS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelRVMS.Name = "labelRVMS";
            this.labelRVMS.Size = new System.Drawing.Size(90, 19);
            this.labelRVMS.Text = "RVMS";
            // 
            // labelColorSensor
            // 
            this.labelColorSensor.AutoSize = false;
            this.labelColorSensor.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelColorSensor.Name = "labelColorSensor";
            this.labelColorSensor.Size = new System.Drawing.Size(90, 19);
            this.labelColorSensor.Text = "Color Sensor";
            // 
            // labelEDMS
            // 
            this.labelEDMS.AutoSize = false;
            this.labelEDMS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelEDMS.Name = "labelEDMS";
            this.labelEDMS.Size = new System.Drawing.Size(90, 19);
            this.labelEDMS.Text = "EDMS";
            // 
            // labelCEDMS
            // 
            this.labelCEDMS.AutoSize = false;
            this.labelCEDMS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelCEDMS.Name = "labelCEDMS";
            this.labelCEDMS.Size = new System.Drawing.Size(90, 19);
            this.labelCEDMS.Text = "C-EDMS";
            // 
            // labelStopImageOn
            // 
            this.labelStopImageOn.AutoSize = false;
            this.labelStopImageOn.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelStopImageOn.Name = "labelStopImageOn";
            this.labelStopImageOn.Size = new System.Drawing.Size(90, 19);
            this.labelStopImageOn.Text = "Stop Image";
            // 
            // labelGloss
            // 
            this.labelGloss.AutoSize = false;
            this.labelGloss.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelGloss.Name = "labelGloss";
            this.labelGloss.Size = new System.Drawing.Size(90, 19);
            this.labelGloss.Text = "Gloss";
            // 
            // labelRewinderCut
            // 
            this.labelRewinderCut.AutoSize = false;
            this.labelRewinderCut.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelRewinderCut.Name = "labelRewinderCut";
            this.labelRewinderCut.Size = new System.Drawing.Size(90, 19);
            this.labelRewinderCut.Text = "Rewinder Cut";
            // 
            // labelSPSpeed
            // 
            this.labelSPSpeed.AutoSize = false;
            this.labelSPSpeed.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelSPSpeed.Name = "labelSPSpeed";
            this.labelSPSpeed.Size = new System.Drawing.Size(130, 19);
            this.labelSPSpeed.Text = "SP Spd :  100 m/min";
            // 
            // labelPVSpeed
            // 
            this.labelPVSpeed.AutoSize = false;
            this.labelPVSpeed.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelPVSpeed.Name = "labelPVSpeed";
            this.labelPVSpeed.Size = new System.Drawing.Size(130, 19);
            this.labelPVSpeed.Text = "PV Spd : 100 m/min";
            // 
            // labelPVPos
            // 
            this.labelPVPos.AutoSize = false;
            this.labelPVPos.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelPVPos.Name = "labelPVPos";
            this.labelPVPos.Size = new System.Drawing.Size(130, 19);
            this.labelPVPos.Text = "PV Pos : 10000 m";
            // 
            // labelNull
            // 
            this.labelNull.Name = "labelNull";
            this.labelNull.Size = new System.Drawing.Size(63, 19);
            this.labelNull.Spring = true;
            // 
            // labelModel
            // 
            this.labelModel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(52, 19);
            this.labelModel.Text = "MODEL";
            this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLot
            // 
            this.labelLot.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(32, 19);
            this.labelLot.Text = "LOT";
            this.labelLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelWorker
            // 
            this.labelWorker.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelWorker.Name = "labelWorker";
            this.labelWorker.Size = new System.Drawing.Size(58, 19);
            this.labelWorker.Text = "WORKER";
            this.labelWorker.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkTimer
            // 
            this.checkTimer.Enabled = true;
            this.checkTimer.Interval = 500;
            this.checkTimer.Tick += new System.EventHandler(this.checkTimer_Tick);
            // 
            // PLCStatusPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusPLC);
            this.Name = "PLCStatusPanel";
            this.Size = new System.Drawing.Size(1451, 25);
            this.Load += new System.EventHandler(this.PLCStatusPanel_Load);
            this.statusPLC.ResumeLayout(false);
            this.statusPLC.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusPLC;
        private System.Windows.Forms.ToolStripStatusLabel labelConnected;
        private System.Windows.Forms.ToolStripStatusLabel labelStopImageOn;
        private System.Windows.Forms.ToolStripStatusLabel labelColorSensor;
        private System.Windows.Forms.ToolStripStatusLabel labelEDMS;
        private System.Windows.Forms.ToolStripStatusLabel labelPinhole;
        private System.Windows.Forms.ToolStripStatusLabel labelRVMS;
        private System.Windows.Forms.ToolStripStatusLabel labelSPSpeed;
        private System.Windows.Forms.ToolStripStatusLabel labelPVSpeed;
        private System.Windows.Forms.ToolStripStatusLabel labelPVPos;
        private System.Windows.Forms.ToolStripStatusLabel labelRewinderCut;
        private System.Windows.Forms.Timer checkTimer;
        private System.Windows.Forms.ToolStripStatusLabel labelModel;
        private System.Windows.Forms.ToolStripStatusLabel labelLot;
        private System.Windows.Forms.ToolStripStatusLabel labelWorker;
        private System.Windows.Forms.ToolStripStatusLabel labelNull;
        private System.Windows.Forms.ToolStripStatusLabel labelCEDMS;
        private System.Windows.Forms.ToolStripStatusLabel labelGloss;
    }
}
