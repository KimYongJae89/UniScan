namespace UniScanS.Data.UI
{
    partial class AlarmForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.labelTitle = new System.Windows.Forms.Label();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.sheetList = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.layoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetTotal = new System.Windows.Forms.Label();
            this.labelSheetNG = new System.Windows.Forms.Label();
            this.ng = new System.Windows.Forms.Label();
            this.total = new System.Windows.Forms.Label();
            this.ratio = new System.Windows.Forms.Label();
            this.labelSheetRatio = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.layoutAlarmType = new System.Windows.Forms.TableLayoutPanel();
            this.type = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelDefect = new System.Windows.Forms.Label();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.columnCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.panelCanvas = new System.Windows.Forms.Panel();
            this.labelSheet = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).BeginInit();
            this.layoutInfo.SuspendLayout();
            this.layoutAlarmType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.StandardWithRibbon;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            this.ultraFormManager.UseAppStyling = false;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1291, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.UseAppStyling = false;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 858);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1291, 1);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.UseAppStyling = false;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 828);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.UseAppStyling = false;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1290, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 828);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.UseAppStyling = false;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.Red;
            this.layoutMain.SetColumnSpan(this.labelTitle, 3);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Image = global::UniScanS.Properties.Resources.alert;
            this.labelTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.Location = new System.Drawing.Point(1, 1);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(1287, 100);
            this.labelTitle.TabIndex = 181;
            this.labelTitle.Text = "Alarm";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.labelImage, 2, 2);
            this.layoutMain.Controls.Add(this.sheetList, 0, 5);
            this.layoutMain.Controls.Add(this.layoutInfo, 0, 3);
            this.layoutMain.Controls.Add(this.labelInfo, 0, 2);
            this.layoutMain.Controls.Add(this.layoutAlarmType, 0, 1);
            this.layoutMain.Controls.Add(this.labelTitle, 0, 0);
            this.layoutMain.Controls.Add(this.labelDefect, 1, 2);
            this.layoutMain.Controls.Add(this.defectList, 1, 3);
            this.layoutMain.Controls.Add(this.panelCanvas, 2, 3);
            this.layoutMain.Controls.Add(this.labelSheet, 0, 4);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.layoutMain.Location = new System.Drawing.Point(1, 30);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 6;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1289, 828);
            this.layoutMain.TabIndex = 191;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Location = new System.Drawing.Point(653, 153);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(635, 40);
            this.labelImage.TabIndex = 185;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetList
            // 
            this.sheetList.AllowUserToAddRows = false;
            this.sheetList.AllowUserToDeleteRows = false;
            this.sheetList.AllowUserToResizeColumns = false;
            this.sheetList.AllowUserToResizeRows = false;
            this.sheetList.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sheetList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sheetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnQty});
            this.sheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetList.Location = new System.Drawing.Point(1, 336);
            this.sheetList.Margin = new System.Windows.Forms.Padding(0);
            this.sheetList.MultiSelect = false;
            this.sheetList.Name = "sheetList";
            this.sheetList.ReadOnly = true;
            this.sheetList.RowHeadersVisible = false;
            this.sheetList.RowTemplate.Height = 23;
            this.sheetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetList.Size = new System.Drawing.Size(200, 491);
            this.sheetList.TabIndex = 183;
            this.sheetList.SelectionChanged += new System.EventHandler(this.sheetList_SelectionChanged);
            this.sheetList.Click += new System.EventHandler(this.sheetList_Click);
            // 
            // columnIndex
            // 
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 80;
            // 
            // columnQty
            // 
            this.columnQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            this.columnQty.ReadOnly = true;
            // 
            // layoutInfo
            // 
            this.layoutInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutInfo.ColumnCount = 2;
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.Controls.Add(this.labelSheetTotal, 0, 1);
            this.layoutInfo.Controls.Add(this.labelSheetNG, 0, 2);
            this.layoutInfo.Controls.Add(this.ng, 1, 2);
            this.layoutInfo.Controls.Add(this.total, 1, 1);
            this.layoutInfo.Controls.Add(this.ratio, 1, 3);
            this.layoutInfo.Controls.Add(this.labelSheetRatio, 0, 3);
            this.layoutInfo.Controls.Add(this.labelUnit, 1, 0);
            this.layoutInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInfo.Location = new System.Drawing.Point(1, 194);
            this.layoutInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInfo.Name = "layoutInfo";
            this.layoutInfo.RowCount = 4;
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutInfo.Size = new System.Drawing.Size(200, 100);
            this.layoutInfo.TabIndex = 192;
            // 
            // labelSheetTotal
            // 
            this.labelSheetTotal.AutoSize = true;
            this.labelSheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetTotal.Location = new System.Drawing.Point(2, 26);
            this.labelSheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetTotal.Name = "labelSheetTotal";
            this.labelSheetTotal.Size = new System.Drawing.Size(67, 22);
            this.labelSheetTotal.TabIndex = 1;
            this.labelSheetTotal.Text = "Total";
            this.labelSheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetNG
            // 
            this.labelSheetNG.AutoSize = true;
            this.labelSheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetNG.Location = new System.Drawing.Point(2, 50);
            this.labelSheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetNG.Name = "labelSheetNG";
            this.labelSheetNG.Size = new System.Drawing.Size(67, 22);
            this.labelSheetNG.TabIndex = 3;
            this.labelSheetNG.Text = "NG";
            this.labelSheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ng
            // 
            this.ng.AutoSize = true;
            this.ng.BackColor = System.Drawing.Color.White;
            this.ng.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ng.Location = new System.Drawing.Point(71, 50);
            this.ng.Margin = new System.Windows.Forms.Padding(0);
            this.ng.Name = "ng";
            this.ng.Size = new System.Drawing.Size(127, 22);
            this.ng.TabIndex = 4;
            this.ng.Text = "0";
            this.ng.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // total
            // 
            this.total.AutoSize = true;
            this.total.BackColor = System.Drawing.Color.White;
            this.total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.total.Location = new System.Drawing.Point(71, 26);
            this.total.Margin = new System.Windows.Forms.Padding(0);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(127, 22);
            this.total.TabIndex = 6;
            this.total.Text = "0";
            this.total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ratio
            // 
            this.ratio.AutoSize = true;
            this.ratio.BackColor = System.Drawing.Color.White;
            this.ratio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratio.Location = new System.Drawing.Point(71, 74);
            this.ratio.Margin = new System.Windows.Forms.Padding(0);
            this.ratio.Name = "ratio";
            this.ratio.Size = new System.Drawing.Size(127, 24);
            this.ratio.TabIndex = 2;
            this.ratio.Text = "0";
            this.ratio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetRatio
            // 
            this.labelSheetRatio.AutoSize = true;
            this.labelSheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetRatio.Location = new System.Drawing.Point(2, 74);
            this.labelSheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetRatio.Name = "labelSheetRatio";
            this.labelSheetRatio.Size = new System.Drawing.Size(67, 24);
            this.labelSheetRatio.TabIndex = 0;
            this.labelSheetRatio.Text = "Ratio";
            this.labelSheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.labelUnit.ForeColor = System.Drawing.Color.Red;
            this.labelUnit.Location = new System.Drawing.Point(74, 2);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(121, 22);
            this.labelUnit.TabIndex = 68;
            this.labelUnit.Text = "Unit : Print";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Location = new System.Drawing.Point(1, 153);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(200, 40);
            this.labelInfo.TabIndex = 191;
            this.labelInfo.Text = "Info";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutAlarmType
            // 
            this.layoutAlarmType.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutAlarmType.ColumnCount = 2;
            this.layoutMain.SetColumnSpan(this.layoutAlarmType, 3);
            this.layoutAlarmType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.26025F));
            this.layoutAlarmType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.73975F));
            this.layoutAlarmType.Controls.Add(this.type, 1, 0);
            this.layoutAlarmType.Controls.Add(this.labelType, 0, 0);
            this.layoutAlarmType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutAlarmType.Location = new System.Drawing.Point(1, 102);
            this.layoutAlarmType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutAlarmType.Name = "layoutAlarmType";
            this.layoutAlarmType.RowCount = 1;
            this.layoutAlarmType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutAlarmType.Size = new System.Drawing.Size(1287, 50);
            this.layoutAlarmType.TabIndex = 189;
            // 
            // type
            // 
            this.type.AutoSize = true;
            this.type.BackColor = System.Drawing.Color.White;
            this.type.Dock = System.Windows.Forms.DockStyle.Fill;
            this.type.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.type.Location = new System.Drawing.Point(300, 1);
            this.type.Margin = new System.Windows.Forms.Padding(0);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(986, 48);
            this.type.TabIndex = 191;
            this.type.Text = "type";
            this.type.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelType.Location = new System.Drawing.Point(1, 1);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(298, 48);
            this.labelType.TabIndex = 190;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefect
            // 
            this.labelDefect.AutoSize = true;
            this.labelDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefect.Location = new System.Drawing.Point(202, 153);
            this.labelDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(450, 40);
            this.labelDefect.TabIndex = 188;
            this.labelDefect.Text = "Defect";
            this.labelDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeColumns = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.defectList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCam,
            this.columnType,
            this.columnInfo,
            this.columnImage});
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.Location = new System.Drawing.Point(202, 194);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.MultiSelect = false;
            this.defectList.Name = "defectList";
            this.defectList.ReadOnly = true;
            this.defectList.RowHeadersVisible = false;
            this.layoutMain.SetRowSpan(this.defectList, 3);
            this.defectList.RowTemplate.Height = 23;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(450, 633);
            this.defectList.TabIndex = 190;
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            this.defectList.Click += new System.EventHandler(this.defectList_Click);
            // 
            // columnCam
            // 
            this.columnCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnCam.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnCam.HeaderText = "Cam";
            this.columnCam.Name = "columnCam";
            this.columnCam.ReadOnly = true;
            this.columnCam.Width = 69;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnType.DefaultCellStyle = dataGridViewCellStyle4;
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            this.columnType.Width = 72;
            // 
            // columnInfo
            // 
            this.columnInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.columnInfo.DefaultCellStyle = dataGridViewCellStyle5;
            this.columnInfo.HeaderText = "Info";
            this.columnInfo.Name = "columnInfo";
            this.columnInfo.ReadOnly = true;
            this.columnInfo.Width = 66;
            // 
            // columnImage
            // 
            this.columnImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnImage.HeaderText = "Image";
            this.columnImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.columnImage.Name = "columnImage";
            this.columnImage.ReadOnly = true;
            // 
            // panelCanvas
            // 
            this.panelCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCanvas.Location = new System.Drawing.Point(653, 194);
            this.panelCanvas.Margin = new System.Windows.Forms.Padding(0);
            this.panelCanvas.Name = "panelCanvas";
            this.layoutMain.SetRowSpan(this.panelCanvas, 3);
            this.panelCanvas.Size = new System.Drawing.Size(635, 633);
            this.panelCanvas.TabIndex = 184;
            // 
            // labelSheet
            // 
            this.labelSheet.AutoSize = true;
            this.labelSheet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheet.Location = new System.Drawing.Point(1, 295);
            this.labelSheet.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheet.Name = "labelSheet";
            this.labelSheet.Size = new System.Drawing.Size(200, 40);
            this.labelSheet.TabIndex = 187;
            this.labelSheet.Text = "Sheet";
            this.labelSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1291, 859);
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlarmForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).EndInit();
            this.layoutInfo.ResumeLayout(false);
            this.layoutInfo.PerformLayout();
            this.layoutAlarmType.ResumeLayout(false);
            this.layoutAlarmType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTitle;
        public System.Windows.Forms.DataGridView sheetList;
        private System.Windows.Forms.Panel panelCanvas;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Label labelSheet;
        private System.Windows.Forms.Label labelDefect;
        private System.Windows.Forms.TableLayoutPanel layoutAlarmType;
        private System.Windows.Forms.Label type;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInfo;
        private System.Windows.Forms.DataGridViewImageColumn columnImage;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TableLayoutPanel layoutInfo;
        private System.Windows.Forms.Label labelSheetTotal;
        private System.Windows.Forms.Label labelSheetNG;
        private System.Windows.Forms.Label total;
        private System.Windows.Forms.Label ratio;
        private System.Windows.Forms.Label labelSheetRatio;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label ng;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
    }
}