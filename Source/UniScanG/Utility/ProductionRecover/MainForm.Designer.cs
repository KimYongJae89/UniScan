namespace ProductionRecover
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupSearchTarget = new System.Windows.Forms.GroupBox();
            this.startDeepScan = new System.Windows.Forms.Button();
            this.startSearch = new System.Windows.Forms.Button();
            this.browseXml = new System.Windows.Forms.Button();
            this.xmlPath = new System.Windows.Forms.TextBox();
            this.labelXmlPathName = new System.Windows.Forms.Label();
            this.groupSearchResult = new System.Windows.Forms.GroupBox();
            this.foundGridView = new System.Windows.Forms.DataGridView();
            this.columnValid = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNoprint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPinhole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSpread = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSheetattack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCoating = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSticker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnKill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSpec1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSpec2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.saveResult = new System.Windows.Forms.Button();
            this.clearResult = new System.Windows.Forms.Button();
            this.labelScanCount = new System.Windows.Forms.Label();
            this.groupSearchTarget.SuspendLayout();
            this.groupSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.foundGridView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupSearchTarget
            // 
            resources.ApplyResources(this.groupSearchTarget, "groupSearchTarget");
            this.groupSearchTarget.Controls.Add(this.startDeepScan);
            this.groupSearchTarget.Controls.Add(this.startSearch);
            this.groupSearchTarget.Controls.Add(this.browseXml);
            this.groupSearchTarget.Controls.Add(this.xmlPath);
            this.groupSearchTarget.Controls.Add(this.labelXmlPathName);
            this.groupSearchTarget.Name = "groupSearchTarget";
            this.groupSearchTarget.TabStop = false;
            // 
            // startDeepScan
            // 
            resources.ApplyResources(this.startDeepScan, "startDeepScan");
            this.startDeepScan.Name = "startDeepScan";
            this.startDeepScan.UseVisualStyleBackColor = true;
            this.startDeepScan.Click += new System.EventHandler(this.startDeepScan_Click);
            // 
            // startSearch
            // 
            resources.ApplyResources(this.startSearch, "startSearch");
            this.startSearch.Name = "startSearch";
            this.startSearch.UseVisualStyleBackColor = true;
            this.startSearch.Click += new System.EventHandler(this.startSearch_Click);
            // 
            // browseXml
            // 
            resources.ApplyResources(this.browseXml, "browseXml");
            this.browseXml.Name = "browseXml";
            this.browseXml.UseVisualStyleBackColor = true;
            this.browseXml.Click += new System.EventHandler(this.browseXml_Click);
            // 
            // xmlPath
            // 
            resources.ApplyResources(this.xmlPath, "xmlPath");
            this.xmlPath.Name = "xmlPath";
            // 
            // labelXmlPathName
            // 
            resources.ApplyResources(this.labelXmlPathName, "labelXmlPathName");
            this.labelXmlPathName.Name = "labelXmlPathName";
            // 
            // groupSearchResult
            // 
            resources.ApplyResources(this.groupSearchResult, "groupSearchResult");
            this.groupSearchResult.Controls.Add(this.foundGridView);
            this.groupSearchResult.Controls.Add(this.labelScanCount);
            this.groupSearchResult.Name = "groupSearchResult";
            this.groupSearchResult.TabStop = false;
            // 
            // foundGridView
            // 
            this.foundGridView.AllowUserToAddRows = false;
            this.foundGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            resources.ApplyResources(this.foundGridView, "foundGridView");
            this.foundGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.foundGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnValid,
            this.columnDate,
            this.columnModel,
            this.columnLot,
            this.columnTotal,
            this.columnNoprint,
            this.columnPinhole,
            this.columnSpread,
            this.columnSheetattack,
            this.columnCoating,
            this.columnSticker,
            this.columnNg,
            this.columnKill,
            this.columnSpec1,
            this.columnSpec2,
            this.columnSpeed});
            this.foundGridView.ContextMenuStrip = this.contextMenuStrip1;
            this.foundGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.foundGridView.Name = "foundGridView";
            this.foundGridView.RowTemplate.Height = 23;
            this.foundGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.foundGridView.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.foundGridView_RowsRemoved);
            // 
            // columnValid
            // 
            resources.ApplyResources(this.columnValid, "columnValid");
            this.columnValid.Name = "columnValid";
            this.columnValid.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnValid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // columnDate
            // 
            resources.ApplyResources(this.columnDate, "columnDate");
            this.columnDate.Name = "columnDate";
            // 
            // columnModel
            // 
            resources.ApplyResources(this.columnModel, "columnModel");
            this.columnModel.Name = "columnModel";
            // 
            // columnLot
            // 
            resources.ApplyResources(this.columnLot, "columnLot");
            this.columnLot.Name = "columnLot";
            // 
            // columnTotal
            // 
            resources.ApplyResources(this.columnTotal, "columnTotal");
            this.columnTotal.Name = "columnTotal";
            // 
            // columnNoprint
            // 
            resources.ApplyResources(this.columnNoprint, "columnNoprint");
            this.columnNoprint.Name = "columnNoprint";
            // 
            // columnPinhole
            // 
            resources.ApplyResources(this.columnPinhole, "columnPinhole");
            this.columnPinhole.Name = "columnPinhole";
            // 
            // columnSpread
            // 
            resources.ApplyResources(this.columnSpread, "columnSpread");
            this.columnSpread.Name = "columnSpread";
            // 
            // columnSheetattack
            // 
            resources.ApplyResources(this.columnSheetattack, "columnSheetattack");
            this.columnSheetattack.Name = "columnSheetattack";
            // 
            // columnCoating
            // 
            resources.ApplyResources(this.columnCoating, "columnCoating");
            this.columnCoating.Name = "columnCoating";
            // 
            // columnSticker
            // 
            resources.ApplyResources(this.columnSticker, "columnSticker");
            this.columnSticker.Name = "columnSticker";
            // 
            // columnNg
            // 
            resources.ApplyResources(this.columnNg, "columnNg");
            this.columnNg.Name = "columnNg";
            // 
            // columnKill
            // 
            resources.ApplyResources(this.columnKill, "columnKill");
            this.columnKill.Name = "columnKill";
            // 
            // columnSpec1
            // 
            resources.ApplyResources(this.columnSpec1, "columnSpec1");
            this.columnSpec1.Name = "columnSpec1";
            // 
            // columnSpec2
            // 
            resources.ApplyResources(this.columnSpec2, "columnSpec2");
            this.columnSpec2.Name = "columnSpec2";
            // 
            // columnSpeed
            // 
            resources.ApplyResources(this.columnSpeed, "columnSpeed");
            this.columnSpeed.Name = "columnSpeed";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // ToolStripMenuItemDelete
            // 
            this.ToolStripMenuItemDelete.Name = "ToolStripMenuItemDelete";
            resources.ApplyResources(this.ToolStripMenuItemDelete, "ToolStripMenuItemDelete");
            this.ToolStripMenuItemDelete.Click += new System.EventHandler(this.ToolStripMenuItemDelete_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            resources.ApplyResources(this.toolStripProgressBar1, "toolStripProgressBar1");
            // 
            // toolStripStatusLabel1
            // 
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            // 
            // saveResult
            // 
            resources.ApplyResources(this.saveResult, "saveResult");
            this.saveResult.Name = "saveResult";
            this.saveResult.UseVisualStyleBackColor = true;
            this.saveResult.Click += new System.EventHandler(this.saveResult_Click);
            // 
            // clearResult
            // 
            resources.ApplyResources(this.clearResult, "clearResult");
            this.clearResult.Name = "clearResult";
            this.clearResult.UseVisualStyleBackColor = true;
            this.clearResult.Click += new System.EventHandler(this.clearResult_Click);
            // 
            // labelScanCount
            // 
            resources.ApplyResources(this.labelScanCount, "labelScanCount");
            this.labelScanCount.Name = "labelScanCount";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.clearResult);
            this.Controls.Add(this.saveResult);
            this.Controls.Add(this.groupSearchResult);
            this.Controls.Add(this.groupSearchTarget);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupSearchTarget.ResumeLayout(false);
            this.groupSearchTarget.PerformLayout();
            this.groupSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.foundGridView)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupSearchTarget;
        private System.Windows.Forms.Button startSearch;
        private System.Windows.Forms.Button browseXml;
        private System.Windows.Forms.TextBox xmlPath;
        private System.Windows.Forms.Label labelXmlPathName;
        private System.Windows.Forms.GroupBox groupSearchResult;
        private System.Windows.Forms.DataGridView foundGridView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Button saveResult;
        private System.Windows.Forms.Button clearResult;
        private System.Windows.Forms.Button startDeepScan;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLot;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNoprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPinhole;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSpread;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSheetattack;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCoating;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSticker;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNg;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnKill;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSpec1;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSpec2;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSpeed;
        private System.Windows.Forms.Label labelScanCount;
    }
}

