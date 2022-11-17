using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class EuresysBoardListForm : Form
    {
        int requiredNumCamera;
        public int RequiredNumCamera
        {
            set { requiredNumCamera = value; }
        }

        CameraConfiguration cameraConfiguration;
        public CameraConfiguration CameraConfiguration
        {
            get { return cameraConfiguration; }
            set { cameraConfiguration = value; }
        }

        public EuresysBoardListForm()
        {
            InitializeComponent();

            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void EuresysBoardListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
           
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                if (cameraInfo is CameraInfoMultiCam)
                {
                    CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)cameraInfo;

                    int id = cameraInfoGrid.Rows.Add(
                        cameraInfoMultiCam.BoardType.ToString(),
                        cameraInfoMultiCam.BoardId,
                        cameraInfoMultiCam.ConnectorId,
                        cameraInfoMultiCam.CameraType.ToString(),
                        cameraInfoMultiCam.SurfaceNum.ToString(),
                        cameraInfoMultiCam.Height.ToString(),
                        cameraInfoMultiCam.UseNativeBuffering.ToString(),
                        "Edit");
                    cameraInfoGrid.Rows[id].Tag = cameraInfoMultiCam;
                }
            }

            while (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null && row.Cells[4].Value != null && row.Cells[5].Value != null)
                {
                    //CameraInfoMultiCam cameraInfoMultiCam = new CameraInfoMultiCam(
                    //                (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), row.Cells[0].Value.ToString()), 
                    //                uint.Parse(row.Cells[1].Value.ToString()),
                    //                uint.Parse(row.Cells[2].Value.ToString()), 
                    //                (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString()), 
                    //                uint.Parse(row.Cells[4].Value.ToString()),
                    //                uint.Parse(row.Cells[5].Value.ToString()));
                    CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)row.Tag;
                    cameraInfoMultiCam.Index = row.Index;
                    cameraInfoMultiCam.UseNativeBuffering = bool.Parse(row.Cells[6].Value.ToString()); 
                    cameraConfiguration.SetCameraInfo(cameraInfoMultiCam);
                }
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = cameraInfoGrid.Columns.Count - 1;
            if (e.ColumnIndex != id)
                return;

            CameraInfoMultiCam cameraInfoGenTL = (CameraInfoMultiCam)cameraInfoGrid.Rows[e.RowIndex].Tag;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfoGenTL;
            propertyGrid.Dock = DockStyle.Fill;

            Form form = new Form();
            form.Size = new System.Drawing.Size(512, 512);
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
