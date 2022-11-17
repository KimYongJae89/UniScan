namespace UniScanG.Module.Controller.Device.Laser
{
    partial class HanbitLaserControlForm
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
            this.cmEmergency = new System.Windows.Forms.Button();
            this.cmReset = new System.Windows.Forms.Button();
            this.laserReady = new System.Windows.Forms.Button();
            this.laserError = new System.Windows.Forms.Button();
            this.laserOutofMeanderRange = new System.Windows.Forms.Button();
            this.doneCountContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.doneCountContextMenuStripMenuItemClear = new System.Windows.Forms.ToolStripMenuItem();
            this.cmState = new System.Windows.Forms.GroupBox();
            this.cmFreeze = new System.Windows.Forms.Button();
            this.cmLotClear = new System.Windows.Forms.Button();
            this.cmAlive = new System.Windows.Forms.Button();
            this.cmMark = new System.Windows.Forms.Button();
            this.cmRun = new System.Windows.Forms.Button();
            this.cmUseLocal = new System.Windows.Forms.Button();
            this.labelCounterOver = new System.Windows.Forms.Label();
            this.counterClear = new System.Windows.Forms.Button();
            this.laserAlive = new System.Windows.Forms.Button();
            this.laserDone = new System.Windows.Forms.Button();
            this.laserLotClearDone = new System.Windows.Forms.Button();
            this.laserState = new System.Windows.Forms.GroupBox();
            this.laserMarkGood = new System.Windows.Forms.Button();
            this.laserDecelMarkFault = new System.Windows.Forms.Button();
            this.printerState = new System.Windows.Forms.GroupBox();
            this.printerUseRemote = new System.Windows.Forms.Button();
            this.printerAblationAll = new System.Windows.Forms.Button();
            this.visionState = new System.Windows.Forms.GroupBox();
            this.visionNg11 = new System.Windows.Forms.Button();
            this.visionNg10 = new System.Windows.Forms.Button();
            this.visionNg01 = new System.Windows.Forms.Button();
            this.visionNg00 = new System.Windows.Forms.Button();
            this.counterOver = new System.Windows.Forms.NumericUpDown();
            this.labelCounterNg = new System.Windows.Forms.Label();
            this.counterReq = new System.Windows.Forms.NumericUpDown();
            this.labelCounterDone = new System.Windows.Forms.Label();
            this.counterDone = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.counter = new System.Windows.Forms.GroupBox();
            this.labelCounterGood = new System.Windows.Forms.Label();
            this.counterGood = new System.Windows.Forms.NumericUpDown();
            this.systemStartStop = new System.Windows.Forms.Button();
            this.cmIgnoreTimeMs = new System.Windows.Forms.NumericUpDown();
            this.labelIgnoreTime = new System.Windows.Forms.Label();
            this.doneCountContextMenuStrip.SuspendLayout();
            this.cmState.SuspendLayout();
            this.laserState.SuspendLayout();
            this.printerState.SuspendLayout();
            this.visionState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.counterOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.counterReq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.counterDone)).BeginInit();
            this.counter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.counterGood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmIgnoreTimeMs)).BeginInit();
            this.SuspendLayout();
            // 
            // cmEmergency
            // 
            this.cmEmergency.Location = new System.Drawing.Point(155, 61);
            this.cmEmergency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmEmergency.Name = "cmEmergency";
            this.cmEmergency.Size = new System.Drawing.Size(141, 60);
            this.cmEmergency.TabIndex = 0;
            this.cmEmergency.Text = "Emergency";
            this.cmEmergency.UseVisualStyleBackColor = true;
            this.cmEmergency.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmReset
            // 
            this.cmReset.Location = new System.Drawing.Point(302, 61);
            this.cmReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmReset.Name = "cmReset";
            this.cmReset.Size = new System.Drawing.Size(141, 60);
            this.cmReset.TabIndex = 0;
            this.cmReset.Text = "Reset";
            this.cmReset.UseVisualStyleBackColor = true;
            this.cmReset.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserReady
            // 
            this.laserReady.Location = new System.Drawing.Point(155, 30);
            this.laserReady.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserReady.Name = "laserReady";
            this.laserReady.Size = new System.Drawing.Size(141, 50);
            this.laserReady.TabIndex = 0;
            this.laserReady.Text = "Ready";
            this.laserReady.UseVisualStyleBackColor = true;
            this.laserReady.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserError
            // 
            this.laserError.Location = new System.Drawing.Point(157, 90);
            this.laserError.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserError.Name = "laserError";
            this.laserError.Size = new System.Drawing.Size(141, 50);
            this.laserError.TabIndex = 0;
            this.laserError.Text = "Error";
            this.laserError.UseVisualStyleBackColor = true;
            this.laserError.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserOutofMeanderRange
            // 
            this.laserOutofMeanderRange.Location = new System.Drawing.Point(302, 90);
            this.laserOutofMeanderRange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserOutofMeanderRange.Name = "laserOutofMeanderRange";
            this.laserOutofMeanderRange.Size = new System.Drawing.Size(141, 50);
            this.laserOutofMeanderRange.TabIndex = 0;
            this.laserOutofMeanderRange.Text = "Out of Meander Range";
            this.laserOutofMeanderRange.UseVisualStyleBackColor = true;
            this.laserOutofMeanderRange.Click += new System.EventHandler(this.Set_Click);
            // 
            // doneCountContextMenuStrip
            // 
            this.doneCountContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doneCountContextMenuStripMenuItemClear});
            this.doneCountContextMenuStrip.Name = "doneCountContextMenuStrip";
            this.doneCountContextMenuStrip.Size = new System.Drawing.Size(102, 26);
            // 
            // doneCountContextMenuStripMenuItemClear
            // 
            this.doneCountContextMenuStripMenuItemClear.Name = "doneCountContextMenuStripMenuItemClear";
            this.doneCountContextMenuStripMenuItemClear.Size = new System.Drawing.Size(101, 22);
            this.doneCountContextMenuStripMenuItemClear.Text = "Clear";
            this.doneCountContextMenuStripMenuItemClear.Click += new System.EventHandler(this.doneCountContextMenuStripMenuItemClear_Click);
            // 
            // cmState
            // 
            this.cmState.Controls.Add(this.cmFreeze);
            this.cmState.Controls.Add(this.cmLotClear);
            this.cmState.Controls.Add(this.cmAlive);
            this.cmState.Controls.Add(this.cmEmergency);
            this.cmState.Controls.Add(this.cmMark);
            this.cmState.Controls.Add(this.cmReset);
            this.cmState.Controls.Add(this.labelIgnoreTime);
            this.cmState.Controls.Add(this.cmRun);
            this.cmState.Controls.Add(this.cmIgnoreTimeMs);
            this.cmState.Location = new System.Drawing.Point(12, 14);
            this.cmState.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmState.Name = "cmState";
            this.cmState.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmState.Size = new System.Drawing.Size(603, 206);
            this.cmState.TabIndex = 1;
            this.cmState.TabStop = false;
            this.cmState.Text = "CM State";
            // 
            // cmFreeze
            // 
            this.cmFreeze.Location = new System.Drawing.Point(157, 131);
            this.cmFreeze.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmFreeze.Name = "cmFreeze";
            this.cmFreeze.Size = new System.Drawing.Size(141, 60);
            this.cmFreeze.TabIndex = 0;
            this.cmFreeze.Text = "Freeze";
            this.cmFreeze.UseVisualStyleBackColor = true;
            this.cmFreeze.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmLotClear
            // 
            this.cmLotClear.Location = new System.Drawing.Point(8, 131);
            this.cmLotClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmLotClear.Name = "cmLotClear";
            this.cmLotClear.Size = new System.Drawing.Size(141, 60);
            this.cmLotClear.TabIndex = 0;
            this.cmLotClear.Text = "Lot Clear";
            this.cmLotClear.UseVisualStyleBackColor = true;
            this.cmLotClear.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmAlive
            // 
            this.cmAlive.Location = new System.Drawing.Point(8, 61);
            this.cmAlive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmAlive.Name = "cmAlive";
            this.cmAlive.Size = new System.Drawing.Size(141, 60);
            this.cmAlive.TabIndex = 0;
            this.cmAlive.Text = "Alive";
            this.cmAlive.UseVisualStyleBackColor = true;
            this.cmAlive.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmMark
            // 
            this.cmMark.Location = new System.Drawing.Point(450, 61);
            this.cmMark.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmMark.Name = "cmMark";
            this.cmMark.Size = new System.Drawing.Size(141, 60);
            this.cmMark.TabIndex = 0;
            this.cmMark.Text = "Mark";
            this.cmMark.UseVisualStyleBackColor = true;
            this.cmMark.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmRun
            // 
            this.cmRun.Location = new System.Drawing.Point(450, 131);
            this.cmRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmRun.Name = "cmRun";
            this.cmRun.Size = new System.Drawing.Size(141, 60);
            this.cmRun.TabIndex = 0;
            this.cmRun.Text = "Run";
            this.cmRun.UseVisualStyleBackColor = true;
            this.cmRun.Click += new System.EventHandler(this.Set_Click);
            // 
            // cmUseLocal
            // 
            this.cmUseLocal.Location = new System.Drawing.Point(777, 404);
            this.cmUseLocal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmUseLocal.Name = "cmUseLocal";
            this.cmUseLocal.Size = new System.Drawing.Size(141, 60);
            this.cmUseLocal.TabIndex = 0;
            this.cmUseLocal.Text = "Use";
            this.cmUseLocal.UseVisualStyleBackColor = true;
            this.cmUseLocal.Click += new System.EventHandler(this.Set_Click);
            // 
            // labelCounterOver
            // 
            this.labelCounterOver.Location = new System.Drawing.Point(223, 37);
            this.labelCounterOver.Name = "labelCounterOver";
            this.labelCounterOver.Size = new System.Drawing.Size(69, 21);
            this.labelCounterOver.TabIndex = 1;
            this.labelCounterOver.Text = "Over";
            this.labelCounterOver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // counterClear
            // 
            this.counterClear.Location = new System.Drawing.Point(230, 121);
            this.counterClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.counterClear.Name = "counterClear";
            this.counterClear.Size = new System.Drawing.Size(63, 33);
            this.counterClear.TabIndex = 0;
            this.counterClear.Text = "Clear";
            this.counterClear.UseVisualStyleBackColor = true;
            this.counterClear.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserAlive
            // 
            this.laserAlive.Location = new System.Drawing.Point(8, 30);
            this.laserAlive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserAlive.Name = "laserAlive";
            this.laserAlive.Size = new System.Drawing.Size(141, 50);
            this.laserAlive.TabIndex = 0;
            this.laserAlive.Text = "Alive";
            this.laserAlive.UseVisualStyleBackColor = true;
            this.laserAlive.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserDone
            // 
            this.laserDone.Location = new System.Drawing.Point(302, 30);
            this.laserDone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserDone.Name = "laserDone";
            this.laserDone.Size = new System.Drawing.Size(141, 50);
            this.laserDone.TabIndex = 0;
            this.laserDone.Text = "Mark Done";
            this.laserDone.UseVisualStyleBackColor = true;
            this.laserDone.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserLotClearDone
            // 
            this.laserLotClearDone.Location = new System.Drawing.Point(8, 92);
            this.laserLotClearDone.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserLotClearDone.Name = "laserLotClearDone";
            this.laserLotClearDone.Size = new System.Drawing.Size(141, 50);
            this.laserLotClearDone.TabIndex = 0;
            this.laserLotClearDone.Text = "Lot Clear Done";
            this.laserLotClearDone.UseVisualStyleBackColor = true;
            this.laserLotClearDone.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserState
            // 
            this.laserState.Controls.Add(this.laserAlive);
            this.laserState.Controls.Add(this.laserError);
            this.laserState.Controls.Add(this.laserMarkGood);
            this.laserState.Controls.Add(this.laserOutofMeanderRange);
            this.laserState.Controls.Add(this.laserDecelMarkFault);
            this.laserState.Controls.Add(this.laserDone);
            this.laserState.Controls.Add(this.laserLotClearDone);
            this.laserState.Controls.Add(this.laserReady);
            this.laserState.Location = new System.Drawing.Point(12, 228);
            this.laserState.Name = "laserState";
            this.laserState.Size = new System.Drawing.Size(603, 150);
            this.laserState.TabIndex = 2;
            this.laserState.TabStop = false;
            this.laserState.Text = "Laser State";
            // 
            // laserMarkGood
            // 
            this.laserMarkGood.Location = new System.Drawing.Point(451, 30);
            this.laserMarkGood.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserMarkGood.Name = "laserMarkGood";
            this.laserMarkGood.Size = new System.Drawing.Size(141, 50);
            this.laserMarkGood.TabIndex = 0;
            this.laserMarkGood.Text = "Mark Good";
            this.laserMarkGood.UseVisualStyleBackColor = true;
            this.laserMarkGood.Click += new System.EventHandler(this.Set_Click);
            // 
            // laserDecelMarkFault
            // 
            this.laserDecelMarkFault.Location = new System.Drawing.Point(450, 90);
            this.laserDecelMarkFault.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserDecelMarkFault.Name = "laserDecelMarkFault";
            this.laserDecelMarkFault.Size = new System.Drawing.Size(141, 50);
            this.laserDecelMarkFault.TabIndex = 0;
            this.laserDecelMarkFault.Text = "Decel Mark Fault";
            this.laserDecelMarkFault.UseVisualStyleBackColor = true;
            this.laserDecelMarkFault.Click += new System.EventHandler(this.Set_Click);
            // 
            // printerState
            // 
            this.printerState.Controls.Add(this.printerUseRemote);
            this.printerState.Controls.Add(this.printerAblationAll);
            this.printerState.Location = new System.Drawing.Point(12, 384);
            this.printerState.Name = "printerState";
            this.printerState.Size = new System.Drawing.Size(603, 90);
            this.printerState.TabIndex = 2;
            this.printerState.TabStop = false;
            this.printerState.Text = "Printer";
            // 
            // printerUseRemote
            // 
            this.printerUseRemote.Location = new System.Drawing.Point(8, 30);
            this.printerUseRemote.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.printerUseRemote.Name = "printerUseRemote";
            this.printerUseRemote.Size = new System.Drawing.Size(141, 50);
            this.printerUseRemote.TabIndex = 0;
            this.printerUseRemote.Text = "Use";
            this.printerUseRemote.UseVisualStyleBackColor = true;
            this.printerUseRemote.Click += new System.EventHandler(this.Set_Click);
            // 
            // printerAblationAll
            // 
            this.printerAblationAll.Location = new System.Drawing.Point(155, 30);
            this.printerAblationAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.printerAblationAll.Name = "printerAblationAll";
            this.printerAblationAll.Size = new System.Drawing.Size(141, 50);
            this.printerAblationAll.TabIndex = 0;
            this.printerAblationAll.Text = "Ablation All";
            this.printerAblationAll.UseVisualStyleBackColor = true;
            this.printerAblationAll.Click += new System.EventHandler(this.Set_Click);
            // 
            // visionState
            // 
            this.visionState.Controls.Add(this.visionNg11);
            this.visionState.Controls.Add(this.visionNg10);
            this.visionState.Controls.Add(this.visionNg01);
            this.visionState.Controls.Add(this.visionNg00);
            this.visionState.Location = new System.Drawing.Point(621, 228);
            this.visionState.Name = "visionState";
            this.visionState.Size = new System.Drawing.Size(309, 150);
            this.visionState.TabIndex = 3;
            this.visionState.TabStop = false;
            this.visionState.Text = "Vision State";
            // 
            // visionNg11
            // 
            this.visionNg11.Location = new System.Drawing.Point(156, 90);
            this.visionNg11.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.visionNg11.Name = "visionNg11";
            this.visionNg11.Size = new System.Drawing.Size(141, 50);
            this.visionNg11.TabIndex = 0;
            this.visionNg11.Text = "VNG11";
            this.visionNg11.UseVisualStyleBackColor = true;
            this.visionNg11.Click += new System.EventHandler(this.Set_Click);
            // 
            // visionNg10
            // 
            this.visionNg10.Location = new System.Drawing.Point(7, 90);
            this.visionNg10.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.visionNg10.Name = "visionNg10";
            this.visionNg10.Size = new System.Drawing.Size(141, 50);
            this.visionNg10.TabIndex = 0;
            this.visionNg10.Text = "VNG10";
            this.visionNg10.UseVisualStyleBackColor = true;
            this.visionNg10.Click += new System.EventHandler(this.Set_Click);
            // 
            // visionNg01
            // 
            this.visionNg01.Location = new System.Drawing.Point(156, 30);
            this.visionNg01.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.visionNg01.Name = "visionNg01";
            this.visionNg01.Size = new System.Drawing.Size(141, 50);
            this.visionNg01.TabIndex = 0;
            this.visionNg01.Text = "VNG01";
            this.visionNg01.UseVisualStyleBackColor = true;
            this.visionNg01.Click += new System.EventHandler(this.Set_Click);
            // 
            // visionNg00
            // 
            this.visionNg00.Location = new System.Drawing.Point(7, 30);
            this.visionNg00.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.visionNg00.Name = "visionNg00";
            this.visionNg00.Size = new System.Drawing.Size(141, 50);
            this.visionNg00.TabIndex = 0;
            this.visionNg00.Text = "VNG00";
            this.visionNg00.UseVisualStyleBackColor = true;
            this.visionNg00.Click += new System.EventHandler(this.Set_Click);
            // 
            // counterOver
            // 
            this.counterOver.Location = new System.Drawing.Point(223, 61);
            this.counterOver.Name = "counterOver";
            this.counterOver.Size = new System.Drawing.Size(69, 29);
            this.counterOver.TabIndex = 2;
            this.counterOver.ValueChanged += new System.EventHandler(this.counter_ValueChanged);
            // 
            // labelCounterNg
            // 
            this.labelCounterNg.Location = new System.Drawing.Point(117, 37);
            this.labelCounterNg.Name = "labelCounterNg";
            this.labelCounterNg.Size = new System.Drawing.Size(69, 21);
            this.labelCounterNg.TabIndex = 1;
            this.labelCounterNg.Text = "Request";
            this.labelCounterNg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // counterReq
            // 
            this.counterReq.Location = new System.Drawing.Point(117, 61);
            this.counterReq.Name = "counterReq";
            this.counterReq.Size = new System.Drawing.Size(69, 29);
            this.counterReq.TabIndex = 2;
            this.counterReq.ValueChanged += new System.EventHandler(this.counter_ValueChanged);
            // 
            // labelCounterDone
            // 
            this.labelCounterDone.Location = new System.Drawing.Point(7, 37);
            this.labelCounterDone.Name = "labelCounterDone";
            this.labelCounterDone.Size = new System.Drawing.Size(73, 21);
            this.labelCounterDone.TabIndex = 1;
            this.labelCounterDone.Text = "Done";
            this.labelCounterDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // counterDone
            // 
            this.counterDone.Location = new System.Drawing.Point(7, 61);
            this.counterDone.Name = "counterDone";
            this.counterDone.Size = new System.Drawing.Size(73, 29);
            this.counterDone.TabIndex = 2;
            this.counterDone.ValueChanged += new System.EventHandler(this.counter_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 21);
            this.label4.TabIndex = 3;
            this.label4.Text = "=";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(194, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 21);
            this.label5.TabIndex = 3;
            this.label5.Text = "+";
            // 
            // counter
            // 
            this.counter.Controls.Add(this.label5);
            this.counter.Controls.Add(this.counterDone);
            this.counter.Controls.Add(this.labelCounterDone);
            this.counter.Controls.Add(this.label4);
            this.counter.Controls.Add(this.counterOver);
            this.counter.Controls.Add(this.labelCounterOver);
            this.counter.Controls.Add(this.labelCounterGood);
            this.counter.Controls.Add(this.labelCounterNg);
            this.counter.Controls.Add(this.counterClear);
            this.counter.Controls.Add(this.counterGood);
            this.counter.Controls.Add(this.counterReq);
            this.counter.Location = new System.Drawing.Point(621, 14);
            this.counter.Name = "counter";
            this.counter.Size = new System.Drawing.Size(309, 206);
            this.counter.TabIndex = 4;
            this.counter.TabStop = false;
            this.counter.Text = "Info";
            // 
            // labelCounterGood
            // 
            this.labelCounterGood.Location = new System.Drawing.Point(7, 107);
            this.labelCounterGood.Name = "labelCounterGood";
            this.labelCounterGood.Size = new System.Drawing.Size(73, 21);
            this.labelCounterGood.TabIndex = 1;
            this.labelCounterGood.Text = "Good";
            this.labelCounterGood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // counterGood
            // 
            this.counterGood.Location = new System.Drawing.Point(7, 131);
            this.counterGood.Name = "counterGood";
            this.counterGood.Size = new System.Drawing.Size(73, 29);
            this.counterGood.TabIndex = 2;
            this.counterGood.ValueChanged += new System.EventHandler(this.counter_ValueChanged);
            // 
            // systemStartStop
            // 
            this.systemStartStop.Location = new System.Drawing.Point(628, 404);
            this.systemStartStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.systemStartStop.Name = "systemStartStop";
            this.systemStartStop.Size = new System.Drawing.Size(141, 60);
            this.systemStartStop.TabIndex = 0;
            this.systemStartStop.Text = "Start / Stop";
            this.systemStartStop.UseVisualStyleBackColor = true;
            this.systemStartStop.Click += new System.EventHandler(this.systemStartStop_Click);
            // 
            // cmIgnoreTimeMs
            // 
            this.cmIgnoreTimeMs.Location = new System.Drawing.Point(150, 27);
            this.cmIgnoreTimeMs.Name = "cmIgnoreTimeMs";
            this.cmIgnoreTimeMs.Size = new System.Drawing.Size(69, 29);
            this.cmIgnoreTimeMs.TabIndex = 2;
            this.cmIgnoreTimeMs.ValueChanged += new System.EventHandler(this.counter_ValueChanged);
            // 
            // labelIgnoreTime
            // 
            this.labelIgnoreTime.AutoSize = true;
            this.labelIgnoreTime.Location = new System.Drawing.Point(7, 29);
            this.labelIgnoreTime.Name = "labelIgnoreTime";
            this.labelIgnoreTime.Size = new System.Drawing.Size(137, 21);
            this.labelIgnoreTime.TabIndex = 1;
            this.labelIgnoreTime.Text = "Ignore Time [ms]";
            this.labelIgnoreTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HanbitLaserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(940, 482);
            this.Controls.Add(this.cmUseLocal);
            this.Controls.Add(this.counter);
            this.Controls.Add(this.visionState);
            this.Controls.Add(this.printerState);
            this.Controls.Add(this.laserState);
            this.Controls.Add(this.cmState);
            this.Controls.Add(this.systemStartStop);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HanbitLaserControlForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "HanbitLaserControlForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HanbitLaserControlForm_FormClosing);
            this.Load += new System.EventHandler(this.HanbitLaserControlForm_Load);
            this.doneCountContextMenuStrip.ResumeLayout(false);
            this.cmState.ResumeLayout(false);
            this.cmState.PerformLayout();
            this.laserState.ResumeLayout(false);
            this.printerState.ResumeLayout(false);
            this.visionState.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.counterOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.counterReq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.counterDone)).EndInit();
            this.counter.ResumeLayout(false);
            this.counter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.counterGood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmIgnoreTimeMs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmEmergency;
        private System.Windows.Forms.Button cmReset;
        private System.Windows.Forms.Button laserReady;
        private System.Windows.Forms.Button laserError;
        private System.Windows.Forms.Button laserOutofMeanderRange;
        private System.Windows.Forms.GroupBox cmState;
        private System.Windows.Forms.Button laserAlive;
        private System.Windows.Forms.Button cmRun;
        private System.Windows.Forms.Button cmMark;
        private System.Windows.Forms.ContextMenuStrip doneCountContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem doneCountContextMenuStripMenuItemClear;
        private System.Windows.Forms.Button cmLotClear;
        private System.Windows.Forms.Button cmFreeze;
        private System.Windows.Forms.Button cmAlive;
        private System.Windows.Forms.Button laserLotClearDone;
        private System.Windows.Forms.Button laserDone;
        private System.Windows.Forms.GroupBox laserState;
        private System.Windows.Forms.Label labelCounterOver;
        private System.Windows.Forms.Button cmUseLocal;
        private System.Windows.Forms.GroupBox printerState;
        private System.Windows.Forms.Button printerUseRemote;
        private System.Windows.Forms.Button printerAblationAll;
        private System.Windows.Forms.Button counterClear;
        private System.Windows.Forms.Button laserMarkGood;
        private System.Windows.Forms.Button laserDecelMarkFault;
        private System.Windows.Forms.GroupBox visionState;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown counterDone;
        private System.Windows.Forms.NumericUpDown counterReq;
        private System.Windows.Forms.Label labelCounterDone;
        private System.Windows.Forms.NumericUpDown counterOver;
        private System.Windows.Forms.Label labelCounterNg;
        private System.Windows.Forms.Button visionNg11;
        private System.Windows.Forms.Button visionNg10;
        private System.Windows.Forms.Button visionNg01;
        private System.Windows.Forms.Button visionNg00;
        private System.Windows.Forms.GroupBox counter;
        private System.Windows.Forms.Button systemStartStop;
        private System.Windows.Forms.Label labelCounterGood;
        private System.Windows.Forms.NumericUpDown counterGood;
        private System.Windows.Forms.Label labelIgnoreTime;
        private System.Windows.Forms.NumericUpDown cmIgnoreTimeMs;
    }
}