using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using UniScanM.Pinhole.UI.MenuPage;
using System.IO;
using UniScanM.Pinhole.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;

namespace UniScanM.Pinhole.UI.MenuPage
{
    public partial class ReportDefectList : UserControl, IMultiLanguageSupport
    {
        public ReportDefectList()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            dataGridDetail.RowTemplate.Height = (dataGridDetail.Height - dataGridDetail.ColumnHeadersHeight) / 4;
            StringManager.AddListener(this);
        }
        
        public void AddDefect(string path, List<DefectInfo> defectInfoList)
        {
            if (defectInfoList == null)
                return;

            //UiHelper.SuspendDrawing(dataGridDetail);
            dataGridDetail.SuspendLayout();

            float pixelResolution = Settings.PinholeSettings.Instance().PixelResolution;

            foreach (DefectInfo df in defectInfoList)
            {
                string dfImagePath = Path.Combine(path, df.Path);
                df.ClipImage = (Bitmap)ImageHelper.LoadImage(dfImagePath);
                string pvPos = string.Format("{0}m", df.PvPos);
                if (string.IsNullOrEmpty(pvPos))
                    pvPos = "0m";


                int posX = (int) Math.Round(df.PixelPosition.X * pixelResolution / 1000.0);
                if (df.CameraIndex == 1) posX += 240;
                int posY = (int) Math.Round(df.PixelPosition.Y * pixelResolution / 1000.0);
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridDetail,
                    df.CameraIndex, 
                    df.DefectType, 
                   posX, 
                   posY, 
                    df.BoundingRect.Width * pixelResolution, 
                    df.BoundingRect.Height * pixelResolution, 
                    pvPos, 
                    df.ClipImage);

                newRow.Tag = dfImagePath;
                newRow.Height = 50;
            
                dataGridDetail.Rows.Add(newRow);
            }
            this.dataGridDetail.AutoResizeColumns();
            //UiHelper.ResumeDrawing(dataGridDetail);
            dataGridDetail.ResumeLayout();
        }

        public void Clear()
        {
            dataGridDetail.Rows.Clear();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void dataGridDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridDetail.SelectedRows.Count == 0)
                return;

            string defectImageFile = (string)dataGridDetail.SelectedRows[0].Tag;
            if (File.Exists(defectImageFile))
                System.Diagnostics.Process.Start(defectImageFile);
            else
                MessageForm.Show(null, string.Format(StringManager.GetString("Can not Fouund Image. [{0}]"), defectImageFile));
        }
    }
}
