using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class ScanWidthSettingForm : Form, IMultiLanguageSupport
    {
        #region 생성자
        public ScanWidthSettingForm(List<GlossScanWidth> glossScanWidthList)
        {
            InitializeComponent();

            foreach (var glossScanWidth in glossScanWidthList)
                GlossScanWidthList.Add(glossScanWidth.Clone());

            IsLoaded = true;
            RefreshValidList();
            StringManager.AddListener(this);
        }
        #endregion

        #region 이벤트
        private void buttonOK_Click(object sender, EventArgs e)
        {
            GlossSettings.Instance().GlossScanWidthList = GlossScanWidthList;
            GlossSettings.Instance().Save();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonModelWidthAdd_Click(object sender, EventArgs e)
        {
            GlossScanWidthList.Add(new GlossScanWidth("", 0, 0, 0, 0));

            RefreshValidList();
        }

        private void buttonModelWidthDelete_Click(object sender, EventArgs e)
        {
            if (validPositionList.SelectedCells[0].RowIndex > -1)
            {
                string modelName = validPositionList.SelectedCells[0].Value.ToString();

                DialogResult dialogResult = DialogResult.No;
                dialogResult = MessageForm.Show(ParentForm, StringManager.GetString(this.GetType().FullName, "Do you want to delete the selected model") + String.Format(" [{0}]?", modelName), "Delete Model", MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    GlossScanWidthList.RemoveAt(validPositionList.SelectedCells[0].RowIndex);

                    RefreshValidList();
                }
            }
        }

        private void validPositionList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool isOk = true;

            if (IsLoaded)
            {
                IsLoaded = false;
                return;
            }

            if (validPositionList.SelectedCells.Count == 0)
                return;

            if (e.ColumnIndex == 0)
            {
                foreach (GlossScanWidth validPosition in GlossScanWidthList)
                {
                    if (validPosition.Name == validPositionList.SelectedCells[0].Value.ToString())
                    {
                        isOk = false;
                        MessageBox.Show("해당 Width 가 이미 존재합니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        IsLoaded = true;
                        validPositionList.SelectedCells[0].Value = "";
                        return;
                    }
                }
            }
            else
            {
                float test;
                if (!float.TryParse(validPositionList.SelectedCells[0].Value.ToString(), out test))
                {
                    isOk = false;
                    MessageBox.Show("입력하신 숫자가 잘못되었습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsLoaded = true;
                    validPositionList.SelectedCells[0].Value = 0;
                    return;
                }

            }

            if (isOk && e.RowIndex != -1)
            {
                switch (e.ColumnIndex)
                {
                    case 0:
                        GlossScanWidthList[e.RowIndex].Name = validPositionList.SelectedCells[0].Value.ToString();
                        break;

                    case 1:
                        GlossScanWidthList[e.RowIndex].Start = Convert.ToSingle(validPositionList.SelectedCells[0].Value);
                        break;

                    case 2:
                        GlossScanWidthList[e.RowIndex].End = Convert.ToSingle(validPositionList.SelectedCells[0].Value);
                        break;

                    case 3:
                        GlossScanWidthList[e.RowIndex].ValidStart = Convert.ToSingle(validPositionList.SelectedCells[0].Value);
                        break;

                    case 4:
                        GlossScanWidthList[e.RowIndex].ValidEnd = Convert.ToSingle(validPositionList.SelectedCells[0].Value);
                        break;
                }
                RefreshValidList();

                DataGridView dataGridView = sender as DataGridView;
                dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
            }
        }
        #endregion

        #region 인터페이스
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
        #endregion

        #region 속성
        private List<GlossScanWidth> GlossScanWidthList { get; set; } = new List<GlossScanWidth>();
        private bool IsLoaded { get; set; } = false;
        #endregion

        #region 메서드
        public void RefreshValidList()
        {
            GlossScanWidthList.Sort((x, y) => x.Name.CompareTo(y.Name));

            validPositionList.Rows.Clear();

            for (int i = 0; i < GlossScanWidthList.Count; i++)
                validPositionList.Rows.Add(GlossScanWidthList[i].Name.ToString(), GlossScanWidthList[i].Start.ToString(), GlossScanWidthList[i].End.ToString(), GlossScanWidthList[i].ValidStart, GlossScanWidthList[i].ValidEnd);
        }
        #endregion
    }
}
