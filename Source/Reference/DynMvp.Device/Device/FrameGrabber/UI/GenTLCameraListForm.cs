using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.IO;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class GenTLCameraListForm : Form
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

        public GenTLCameraListForm()
        {
            InitializeComponent();

            columnDirectionType.Items.AddRange(Enum.GetNames(typeof(EScanDirectionType)));
            columnClientType.Items.AddRange(Enum.GetNames(typeof(EClientType)));
            buttonOK.Text = StringManager.GetString(this.GetType().FullName, buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName, buttonCancel.Text);
        }

        private void GenTLCameraListForm_Load(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            cameraInfoGrid.Rows.Clear();
            for (int i = 0; i < cameraConfiguration.CameraInfos.Length; i++)
            {
                CameraInfo cameraInfo = cameraConfiguration.CameraInfos[i];
                if (cameraInfo is CameraInfoGenTL)
                {
                    CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;
                    int idx = cameraInfoGrid.Rows.Add("Edit", cameraInfoGenTL.Index, cameraInfoGenTL.Width, cameraInfoGenTL.Height, cameraInfoGenTL.FrameNum,
                        cameraInfoGenTL.OffsetX, cameraInfoGenTL.ClientType.ToString(), cameraInfoGenTL.DirectionType.ToString(), cameraInfoGenTL.UseMilBuffer, 
                        cameraInfoGenTL.BinningVertical, cameraInfoGenTL.VirtualImagePath);
                    cameraInfoGrid.Rows[idx].Tag = cameraInfoGenTL;
                    cameraInfoGrid.Rows[idx].ReadOnly = true;

                    //DataGridViewButtonCell dataGridViewButtonCell = cameraInfoGrid.Rows[idx].Cells[10] as DataGridViewButtonCell;
                    //dataGridViewButtonCell.
                }
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ApplyData() == false)
            {
                MessageForm.Show(null, "Plaease, Check the values");
                return;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private bool ApplyData()
        {
            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.IsNewRow == true)
                    continue;

                CameraInfoGenTL cameraInfoGenTL = row.Tag as CameraInfoGenTL;
                if (cameraInfoGenTL == null)
                    continue;

                bool ok = true;
                int width, height, frameNo;
                uint scanLength, offsetX;
                bool useMilBuf, binningVertical;
                EClientType clientType = EClientType.Master;
                EScanDirectionType directionType = EScanDirectionType.Forward;

                ok &= int.TryParse((string)row.Cells[2].Value.ToString(), out width);
                ok &= int.TryParse((string)row.Cells[3].Value.ToString(), out height);
                ok &= int.TryParse((string)row.Cells[4].Value.ToString(), out frameNo);
                ok &= uint.TryParse((string)row.Cells[5].Value.ToString(), out offsetX);
                ok &= Enum.TryParse<EClientType>((string)row.Cells[6].Value.ToString(), out clientType);
                ok &= Enum.TryParse<EScanDirectionType>((string)row.Cells[7].Value.ToString(), out directionType);
                ok &= bool.TryParse((string)row.Cells[8].Value.ToString(), out useMilBuf);
                ok &= bool.TryParse((string)row.Cells[9].Value.ToString(), out binningVertical);
                
                if (ok == false)
                    return false;

                cameraInfoGenTL.Index = row.Index;
                cameraInfoGenTL.Width = width;
                cameraInfoGenTL.Height = height;
                cameraInfoGenTL.FrameNum = frameNo;
                cameraInfoGenTL.OffsetX = offsetX;
                cameraInfoGenTL.ClientType = clientType;
                cameraInfoGenTL.DirectionType = directionType;
                cameraInfoGenTL.UseMilBuffer = useMilBuf;
                cameraInfoGenTL.BinningVertical = binningVertical;
                cameraInfoGenTL.VirtualImagePath = row.Cells[10].Value?.ToString();

                cameraConfiguration.SetCameraInfo(cameraInfoGenTL);
            }

            
            return true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cameraInfoGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 0)
                return;

            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfoGrid.Rows[e.RowIndex].Tag;

            PropertyGrid propertyGrid = new PropertyGrid();
            propertyGrid.SelectedObject = cameraInfoGenTL;
            propertyGrid.Dock = DockStyle.Fill;

            Form form = new Form();
            form.Size = new System.Drawing.Size(480, 960);
            form.Controls.Add(propertyGrid);
            form.ShowDialog();
            UpdateData();
        }
    }
}
