using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.Teach.Inspector;
using UniScanG.Data;
using DynMvp.Base;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision;

namespace UniScanG.Gravure.UI.Teach.Inspector
{

    public delegate void OnDefectTpyeSelectChangedDelegate(DefectType[] defectTypes);
    public interface IDefectTypeFilter
    {
        void SetDefectTpyeSelectChanged(OnDefectTpyeSelectChangedDelegate onDefectTpyeSelectChanged);
    }

    public partial class DefectTypeFilterPanel : UserControl, IDefectTypeFilter, IMultiLanguageSupport
    {
        public event OnDefectTpyeSelectChangedDelegate OnDefectTpyeSelectChanged;

        class TypeSelectContainer
        {
            public DefectType Key { get; set; }
            public bool Value { get; set; }
            public CheckBox CheckBox { get; set; }

            public TypeSelectContainer(DefectType key, bool value, CheckBox checkBox)
            {
                this.Key = key;
                this.Value = value;
                this.CheckBox = checkBox;
            }

            public void Toggle()
            {
                this.Value = !this.Value;
            }
        }

        TypeSelectContainer totalCheckBox;
        List<TypeSelectContainer> checkedValueList = new List<TypeSelectContainer>();
        CheckBox[] checkBoxes = new CheckBox[0];

        public DefectTypeFilterPanel(int rowCount)
        {
            InitializeComponent();
            
            bool simpleReport = UniScanG.Common.Settings.SystemTypeSettings.Instance().ShowSimpleReportLotList;

            List<DefectType> typeList = Enum.GetValues(typeof(DefectType)).Cast<DefectType>().ToList();
            typeList.Remove(DefectType.Unknown);

            if (simpleReport || !AlgorithmSetting.Instance().UseExtSticker)
                typeList.Remove(DefectType.Sticker);

            if (simpleReport || !AlgorithmSetting.Instance().UseExtMargin)
                typeList.Remove(DefectType.Margin);

            if (simpleReport || !AlgorithmSetting.Instance().UseExtTransfrom)
                typeList.Remove(DefectType.Transform);

            this.layoutTotal.RowCount = rowCount;
            this.layoutTotal.ColumnCount = rowCount == 1 ? typeList.Count : typeList.Count / rowCount + 1;

            for (int i = 0; i < this.layoutTotal.ColumnCount; i++)
            {
                ColumnStyle columnStyle = new ColumnStyle(SizeType.Percent, 100.0f / this.layoutTotal.ColumnCount);
                if (this.layoutTotal.ColumnStyles.Count > i)
                    this.layoutTotal.ColumnStyles[i] = columnStyle;
                else
                    this.layoutTotal.ColumnStyles.Add(columnStyle);
            }

            for (int i = 0; i < this.layoutTotal.RowCount; i++)
            {
                RowStyle rowStyle = new RowStyle(SizeType.Percent, 100.0f / this.layoutTotal.RowCount);
                if (this.layoutTotal.RowStyles.Count > i)
                    this.layoutTotal.RowStyles[i] = rowStyle;
                else
                    this.layoutTotal.RowStyles.Add(rowStyle);
            }

            int addCount = 0;
            List<CheckBox> checkBoxList = new List<CheckBox>();
            foreach (DefectType value in typeList)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.AutoCheck = false;
                checkBox.Margin = new Padding(0);
                checkBox.Text = StringManager.GetString(typeof(DefectType).FullName, value.ToString());
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.FlatAppearance.CheckedBackColor = Color.CornflowerBlue;
                checkBox.FlatAppearance.BorderSize = 0;
                checkBox.Appearance = Appearance.Button;
                checkBox.CheckAlign = ContentAlignment.MiddleCenter;
                checkBox.TextAlign = ContentAlignment.MiddleCenter;
                checkBox.Dock = DockStyle.Fill;
                checkBox.Checked = true;
                checkBox.Paint += CheckBox_Paint;
                checkBox.CheckedChanged += CheckBox_CheckedChanged;
                checkBox.Tag = value;

                int rowIdx, colIdx, rowSpan;
                if (value == DefectType.Total)
                {
                    this.totalCheckBox = new TypeSelectContainer(value, true, checkBox);
                    rowIdx = colIdx = 0;
                    rowSpan = this.layoutTotal.RowCount;
                }
                else
                {
                    rowIdx = (this.layoutTotal.Controls.Count - 1) / (this.layoutTotal.ColumnCount - 1);
                    colIdx = (this.layoutTotal.Controls.Count - 1) % (this.layoutTotal.ColumnCount - 1) + 1;
                    rowSpan = 1;

                    checkedValueList.Add(new TypeSelectContainer(value, true, checkBox));
                }

                checkBox.MouseDown += CheckBox_MouseDown;

                checkBoxList.Add(checkBox);
                this.layoutTotal.Controls.Add(checkBox, colIdx, rowIdx);
                addCount++;
                this.layoutTotal.SetRowSpan(checkBox, rowSpan);
            }
            this.checkBoxes = checkBoxList.ToArray();
            UpdateCheckBoxColor();

            StringManager.AddListener(this);
            ColorTable.OnColorTableUpdated += UpdateCheckBoxColor;
        }

        private void UpdateCheckBoxColor()
        {
            foreach (Control control in this.layoutTotal.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox checkBox = (CheckBox)control;
                    UpdateCheckBoxColor(checkBox, (DefectType)checkBox.Tag);
                    //Data.ColorTable.UpdateControlColor(checkBox, (DefectType)checkBox.Tag);
                }
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null)
                return;

            DefectType defectType = (DefectType)checkBox.Tag;
            UpdateCheckBoxColor(checkBox, defectType);
        }

        private void UpdateCheckBoxColor(CheckBox checkBox, DefectType defectType)
        {
            Color checkedColor = ColorTable.GetColor(defectType);
            Color bgColor = SystemColors.Control;
            Color fgColor = ColorTable.GetBgColor(checkBox.Checked ? checkedColor : bgColor);

            checkBox.FlatAppearance.CheckedBackColor = checkedColor;
            checkBox.BackColor = bgColor;
            checkBox.ForeColor = fgColor;
        }

        private void CheckBox_Paint(object sender, PaintEventArgs e)
        {
  

            //Brush bgBbrush = new SolidBrush(bgColor);
            //Brush fgBbrush = new SolidBrush(ColorTable.GetBgColor(bgColor));

            //Rectangle rectangle = checkBox.DisplayRectangle;
            //e.Graphics.FillRectangle(bgBbrush, rectangle);
            //e.Graphics.DrawString("dd", checkBox.Font, fgBbrush, rectangle);
        }

        public void SetDefectTpyeSelectChanged(OnDefectTpyeSelectChangedDelegate onDefectTpyeSelectChanged)
        {
            OnDefectTpyeSelectChanged = onDefectTpyeSelectChanged;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            Array.ForEach(this.checkBoxes, f =>
            {
                f.Text = StringManager.GetString(typeof(DefectType).FullName, f.Tag.ToString());
            });

        }


        private void CheckBox_MouseDown(object sender, MouseEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            DefectType defectType = (DefectType)checkBox.Tag;
            MouseButtons mouseButtons = e.Button;

            if (defectType == DefectType.Total)
            {
                this.totalCheckBox.Toggle();
                checkedValueList.ForEach(f => f.Value = this.totalCheckBox.Value);
            }
            else
            {
                switch (mouseButtons)
                {
                    case MouseButtons.Left:
                        // 왼쪽 클릭: 누른 버튼만 활성화
                        checkedValueList.ForEach(f => f.Value = (f.Key == defectType) ? true : false);
                        break;

                    case MouseButtons.Right:
                        // 오른쪽 클릭: 누른 버튼을 토글
                        checkedValueList.Find(f => f.Key == defectType).Toggle();
                        break;
                }

                this.totalCheckBox.Value = checkedValueList.TrueForAll(f => f.Value);
            }

            UpdateControl();

            DefectType[] defectTypes = GetSelected();
            OnDefectTpyeSelectChanged?.Invoke(defectTypes);
        }

        private void UpdateControl()
        {
            this.totalCheckBox.CheckBox.Checked = this.totalCheckBox.Value;
            checkedValueList.ForEach(f => f.CheckBox.Checked = f.Value);
        }

        public DefectType[] GetSelected()
        {
            DefectType[] defectTypes = this.checkedValueList.FindAll(f => f.Value).Select(f => f.Key).ToArray();
            return defectTypes;
        }
    }
}
