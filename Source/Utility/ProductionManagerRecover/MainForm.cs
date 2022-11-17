using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Screen.Vision.Detector;

namespace ProductionManagerRecover
{
    public partial class MainForm : Form
    {
        ProductionManagerG productionManager = new ProductionManagerG("");

        public MainForm()
        {
            InitializeComponent();


            dataGridView1.RowHeadersVisible = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void buttonLoadFromXml_Click(object sender, EventArgs e)
        {
            LoadFromXml(true);
        }

        private void buttonAppendFromXml_Click(object sender, EventArgs e)
        {
            LoadFromXml(false);

        }

        private void UpdateGrid()
        {
            dataGridView1.DataSource = productionManager.List.ConvertAll<ProductionG>(f=>f as ProductionG);
        }

        public void LoadFromXml(bool clear)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML files(*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    if (clear)
                        productionManager.Clear();
                    string fullFileName = dlg.FileName;
                    string path = Path.GetDirectoryName(fullFileName);
                    string name = Path.GetFileName(fullFileName);
                    productionManager.Load(path, name);
                });
            }

            UpdateGrid();
        }

        private void buttonLoadFromResult_Click(object sender, EventArgs e)
        {
            LoadFromResult(true);
        }

        private void buttonAppendFromResult_Click(object sender, EventArgs e)
        {
            LoadFromResult(false);
        }

        enum ESTEP { Result, Date, Model, Thickness, Paste, Lot, Sheet }
        public void LoadFromResult(bool clear)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (clear)
                    productionManager.Clear();

                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    DirectoryInfo resultDi = new DirectoryInfo(dlg.SelectedPath);
                    Find(resultDi, (int)ESTEP.Result);
                });
            }
            UpdateGrid();
        }

        private void Find(DirectoryInfo directoryInfo, int step)
        {
            if(step == (int)ESTEP.Sheet)
            {
                Finale(directoryInfo);
            }

            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            foreach (DirectoryInfo subDirectoryInfo in subDirectoryInfos)
                Find(subDirectoryInfo, step + 1);
        }

        private void Finale(DirectoryInfo directoryInfo)
        {
            int sheetNo;
            bool ok = int.TryParse(directoryInfo.Name, out sheetNo);
            if (ok == false)
                return;

            string csvFile = Path.Combine(directoryInfo.FullName, string.Format("{0}.csv", SheetInspector.TypeName));
            if (File.Exists(csvFile) == false)
                return;

            List<string> infoList = new List<string>();
            DirectoryInfo tempDirectoryInfo = directoryInfo;
            for (int i = 0; i < 5; i++)
            {
                infoList.Add(tempDirectoryInfo.Name);
                tempDirectoryInfo = tempDirectoryInfo.Parent;
            }

            string modelName;
            string lotNo;
            float thickness;
            string paste;

            try
            {
                modelName = infoList[4];
                lotNo = infoList[1];
                thickness = float.Parse(infoList[3]);
                paste = infoList[2];
            }
            catch(FormatException)
            { return; }

            ProductionG production = (ProductionG)this.productionManager.List.Find(f =>
            {
                ProductionG pg = (ProductionG)f;
                return pg.Name == modelName &&
                pg.LotNo == lotNo &&
                pg.Thickness == thickness &&
                pg.Paste == paste;
            });

            if (production == null)
            {
                production = new ProductionG(modelName, lotNo, thickness, paste, 0);
                this.productionManager.List.Add(production);
            }

            MergeSheetResult mergeSheetResult = new MergeSheetResult(sheetNo, directoryInfo.FullName, true);
            production.Update(mergeSheetResult);

            FileInfo fi = new FileInfo(csvFile);
            DateTime dateTime = fi.LastWriteTime;
            int indicate = DateTime.Compare(production.StartTime, dateTime);
            production.StartTime = indicate > 0 ? dateTime : production.StartTime;
            production.LastUpdateTime = indicate < 0 ? dateTime : production.StartTime;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.productionManager.Clear();
            UpdateGrid();
        }

        private void buttonSaveToXml_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML files(*.xml)|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    string fullFileName = dlg.FileName;
                    string path = Path.GetDirectoryName(fullFileName);
                    string name = Path.GetFileName(fullFileName);
                    productionManager.Save(path, name);
                });
            }
        }
    }
}
