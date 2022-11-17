using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniEye.Base;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;
using DynMvp.UI.Touch;
using UniEye.Base.UI.CameraCalibration;
using DynMvp.Authentication;

namespace UniScan.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage
    {
        private DataTable dataTable;
        public DataTable DataTable
        {
            get { return dataTable; }
            set { dataTable = value; }
        }

        private List<PageInfo> pageInfoList;
        public List<PageInfo> PageInfoList
        {
            get { return pageInfoList; }
            set { pageInfoList = value; }
        }

        List<string> powderList = new List<string>();
        public List<string> PowderList
        {
            get { return powderList; }
            set { powderList = value; }
        }

        List<string> petList = new List<string>();
        public List<string> PetList
        {
            get { return petList; }
            set { petList = value; }
        }

        int pageIndex;

        public List<PageInfo> Clone(List<PageInfo> list)
        {
            List<PageInfo> pageInfoList = new List<PageInfo>();

            for (int i = 0; i < list.Count; i++)
            {
                PageInfo pageInfo = list[i].Clone();

                pageInfoList.Add(pageInfo);
            }

            return pageInfoList;
        }

        public List<string> Clone(List<string> list)
        {
            List<string> dataList = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                string data = list[i].Clone().ToString();

                dataList.Add(data);
            }

            return dataList;
        }

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();

            tabControlMain.SelectedIndex = 4;

            pageInfoList = Clone(SystemSettings.Instance().PageInfoList);

            InitPage();

            UpdatePage();
        }

        public void InitPage()
        {
            comboPageSelect.Items.Add("Page 1");
            comboPageSelect.Items.Add("Page 2");
            comboPageSelect.Items.Add("Page 3");
            comboPageSelect.Items.Add("Page 4");
            comboPageSelect.Items.Add("Page 5");



            DataTable = new DataTable();

            DataTable.Columns.Add("No");
            DataTable.Columns.Add("Type");
            DataTable.Columns.Add("Value");

            ultraGridPanel.DataSource = DataTable;
            


            ValueList typeValueList = ultraGridPanel.DisplayLayout.ValueLists.Add("TypeList");

            typeValueList.ValueListItems.Add("Profile", "Profile");
            typeValueList.ValueListItems.Add("Trend", "Trend");

            UltraGridColumn columnType = ultraGridPanel.DisplayLayout.Bands[0].Columns["Type"];

            UltraComboEditor UltraComboTypeEditor = new UltraComboEditor();

            // 패널종류추가
            UltraComboTypeEditor.DropDownStyle = DropDownStyle.DropDownList;
            UltraComboTypeEditor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            UltraComboTypeEditor.Items.Add(PanelType.Profile);
            UltraComboTypeEditor.Items.Add(PanelType.Trend);
            UltraComboTypeEditor.Items.Add(PanelType.Overlay);

            columnType.EditorComponent = UltraComboTypeEditor;
            columnType.ValueList = typeValueList;



            ValueList chartValueList = ultraGridPanel.DisplayLayout.ValueLists.Add("ChartValueList");

            chartValueList.ValueListItems.Add("Sheet", "Sheet");
            chartValueList.ValueListItems.Add("PET", "PET");

            UltraGridColumn columnValue = ultraGridPanel.DisplayLayout.Bands[0].Columns["Value"];

            UltraComboEditor UltraComboValueEditor = new UltraComboEditor();

            UltraComboValueEditor.DropDownStyle = DropDownStyle.DropDownList;
            UltraComboValueEditor.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            UltraComboValueEditor.Items.Add(ValueType.Sheet);
            UltraComboValueEditor.Items.Add(ValueType.PET);

            columnValue.EditorComponent = UltraComboValueEditor;
            columnValue.ValueList = chartValueList;



            ultraGridPanel.DisplayLayout.Bands[0].Columns["No"].CellActivation = Activation.NoEdit;
            ultraGridPanel.DisplayLayout.Bands[0].Columns["Type"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
            ultraGridPanel.DisplayLayout.Bands[0].Columns["Value"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
        }

        private void UpdatePage()
        {
            comboPageSelect.SelectedIndex = 0;
        }

        public void SaveSettings()
        {
            SystemSettings.Instance().Save();
        }

        public void LoadSettings()
        {
            SystemSettings.Instance().Load();

            pageInfoList = Clone(SystemSettings.Instance().PageInfoList);

            powderList = Clone(SystemSettings.Instance().PowderList);
            petList = Clone(SystemSettings.Instance().PetList);
            RefreshList();
        }

        public void GridNumReset()
        {
            for (int i = 0; i < ultraGridPanel.Rows.Count; i++)
            {
                ultraGridPanel.Rows[i].Cells[0].Value = i + 1;
            }
        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == false)
                SaveSettings();
            else
                LoadSettings();
        }

        // Chart
        private void comboPageSelect_ValueChanged(object sender, EventArgs e)
        {
            pageIndex = comboPageSelect.SelectedIndex;

            dataTable.Rows.Clear();

            if (pageIndex >= pageInfoList.Count)
                return;

            PageInfo pageInfo = pageInfoList[pageIndex];

            for (int i = 0; i < pageInfo.NumPanel; i++)
            {
                DataRow dataRow = DataTable.NewRow();

                dataRow["No"] = DataTable.Rows.Count + 1;
                dataRow["Type"] = pageInfo.ResultPanelInfoList[i].PanelType;
                dataRow["Value"] = pageInfo.ResultPanelInfoList[i].ValueType;

                DataTable.Rows.Add(dataRow);
            }
        }

        private void ultraButtonAdd_Click(object sender, EventArgs e)
        {
            if (pageInfoList.Count < pageIndex)
            {
                MessageForm.Show(null, "사용하지 않은 이전 페이지가 존재합니다.\n이전 페이지를 사용하지 않고 다음페이지를 사용할 수 없습니다.");
                return;
            }

            if (dataTable.Rows.Count < 4)
            {
                DataRow dataRow = DataTable.NewRow();

                dataRow["No"] = DataTable.Rows.Count + 1;
                dataRow["Type"] = PanelType.Profile;
                dataRow["Value"] = ValueType.Sheet;

                DataTable.Rows.Add(dataRow);
            }
            else
            {
                MessageForm.Show(null, "차트는 한 페이지에 4개를 초과할 수 없습니다.");
                return;
            }

            if (pageInfoList.Count < pageIndex + 1)
                pageInfoList.Add(new PageInfo());

            pageInfoList[pageIndex].ResultPanelInfoList.Add(new ProfilePanel.Info());
        }

        private void ultraButtonDelete_Click(object sender, EventArgs e)
        {
            if (ultraGridPanel.Selected.Cells.Count == 0)
                return;

            int selIndex = ultraGridPanel.Selected.Cells[0].Row.Index;

            pageInfoList[pageIndex].ResultPanelInfoList.RemoveAt(selIndex);

            if (pageInfoList[pageIndex].ResultPanelInfoList.Count == 0)
                pageInfoList.RemoveAt(pageIndex);

            DataTable.Rows[selIndex].Delete();

            GridNumReset();
        }

        private void ultraButtonSave_Click(object sender, EventArgs e)
        {
            SystemSettings.Instance().PageInfoList = pageInfoList;
        }

        private void ultraGridPanel_CellChange(object sender, CellEventArgs e)
        {
            switch (e.Cell.Column.Index)
            {
                case 1:
                    {
                        pageInfoList[pageIndex].ResultPanelInfoList.RemoveAt(e.Cell.Row.Index);

                        // 패널종류추가
                        switch ((PanelType)Enum.Parse(typeof(PanelType), e.Cell.Text))
                        {
                            case PanelType.Profile:
                                {
                                    pageInfoList[pageIndex].ResultPanelInfoList.Insert(e.Cell.Row.Index, new ProfilePanel.Info());
                                }
                                break;

                            case PanelType.Trend:
                                {
                                    pageInfoList[pageIndex].ResultPanelInfoList.Insert(e.Cell.Row.Index, new TrendPanel.Info());
                                }
                                break;

                            case PanelType.Overlay:
                                {
                                    pageInfoList[pageIndex].ResultPanelInfoList.Insert(e.Cell.Row.Index, new OverlayPanel.Info());
                                }
                                break;
                        }

                        pageInfoList[pageIndex].ResultPanelInfoList[e.Cell.Row.Index].ValueType = (ValueType)Enum.Parse(typeof(ValueType), e.Cell.Row.Cells[2].Value.ToString());
                    }
                    break;
                case 2:
                    {
                        pageInfoList[pageIndex].ResultPanelInfoList[e.Cell.Row.Index].ValueType = (ValueType)Enum.Parse(typeof(ValueType), e.Cell.Text);
                    }
                    break;
            }
        }

        private void ultraButtonUpChange_Click(object sender, EventArgs e)
        {
            if (ultraGridPanel.Selected.Cells.Count == 0)
                return;

            int selIndex = ultraGridPanel.Selected.Cells[0].Row.Index;

            if (selIndex == 0)
                return;

            ResultPanelInfo resultPanelInfo = pageInfoList[pageIndex].ResultPanelInfoList[selIndex].Clone();

            pageInfoList[pageIndex].ResultPanelInfoList.RemoveAt(selIndex);
            pageInfoList[pageIndex].ResultPanelInfoList.Insert(selIndex - 1, resultPanelInfo);

            DataRow dataRow = DataTable.NewRow();
            for (int i = 0; i < DataTable.Rows[selIndex].ItemArray.Count(); i++)
            {
                dataRow[i] = DataTable.Rows[selIndex][i];
            }

            DataTable.Rows[selIndex].Delete();
            DataTable.Rows.InsertAt(dataRow, selIndex - 1);

            GridNumReset();
        }

        private void ultraButtonDownChange_Click(object sender, EventArgs e)
        {
            if (ultraGridPanel.Selected.Cells.Count == 0)
                return;

            int selIndex = ultraGridPanel.Selected.Cells[0].Row.Index;

            if (selIndex + 1 >= ultraGridPanel.Rows.Count)
                return;

            ResultPanelInfo resultPanelInfo = pageInfoList[pageIndex].ResultPanelInfoList[selIndex].Clone();

            pageInfoList[pageIndex].ResultPanelInfoList.RemoveAt(selIndex);
            pageInfoList[pageIndex].ResultPanelInfoList.Insert(selIndex + 1, resultPanelInfo);

            DataRow dataRow = DataTable.NewRow();
            for (int i = 0; i < DataTable.Rows[selIndex].ItemArray.Count(); i++)
            {
                dataRow[i] = DataTable.Rows[selIndex][i];
            }

            DataTable.Rows[selIndex].Delete();
            DataTable.Rows.InsertAt(dataRow, selIndex + 1);

            GridNumReset();
        }

        // Model
        public void RefreshList()
        {
            listBoxPowder.Items.Clear();
            PowderList.Sort();
            petList.Sort();

            for (int i = 0; i < PowderList.Count; i++)
                listBoxPowder.Items.Add(PowderList[i]);

            listBoxPET.Items.Clear();

            for (int i = 0; i < petList.Count; i++)
                listBoxPET.Items.Add(petList[i]);
        }


        private void buttonPowderAdd_Click(object sender, EventArgs e)
        {
            if (utePowder.Text != null && utePowder.Text != "")
            {
                bool isNew = true;

                foreach (string Powder in PowderList)
                {
                    if (Powder == utePowder.Text)
                        isNew = false;
                }

                if (isNew)
                    PowderList.Add(utePowder.Text);
                else
                    MessageBox.Show("해당 Powder 가 이미 존재합니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshList();
        }

        private void buttonPowderDelete_Click(object sender, EventArgs e)
        {
            if (listBoxPowder.SelectedIndex > -1)
            {
                PowderList.RemoveAt(listBoxPowder.SelectedIndex);
            }

            RefreshList();
        }

        private void buttonPowderSave_Click(object sender, EventArgs e)
        {
            SystemSettings.Instance().PowderList = PowderList;
        }

        private void buttonPETAdd_Click(object sender, EventArgs e)
        {
            if (utePET.Text != null && utePET.Text != "")
            {
                bool isNew = true;

                foreach (string pet in petList)
                {
                    if (pet == utePET.Text)
                        isNew = false;
                }

                if (isNew)
                    petList.Add(utePET.Text);
                else
                    MessageBox.Show("해당 PET 가 이미 존재합니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshList();
        }

        private void buttonPETDelete_Click(object sender, EventArgs e)
        {
            if (listBoxPET.SelectedIndex > -1)
            {
                petList.RemoveAt(listBoxPET.SelectedIndex);
            }

            RefreshList();
        }

        private void buttonPETSave_Click(object sender, EventArgs e)
        {
            SystemSettings.Instance().PetList = petList;
        }

        // no use
        public void EnableControls(UserType user)
        {

        }

        public void Initialize()
        {

        }

        private void tabPageCamera_Click(object sender, EventArgs e)
        {
            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count == 0)
            {
                label2.Text = "";
                return;
            }

            label2.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height.ToString("0.000");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CameraCalibrationForm form = new CameraCalibrationForm();
            form.Initialize();
            form.ShowDialog();
            label2.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height.ToString("0.000");
        }

        public void UpdateControl(string item, object value)
        {
            
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            
        }
    }
}
