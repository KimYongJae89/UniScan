using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class MatroxBoardListForm : Form
    {
        CameraConfiguration cameraConfiguration;
        public CameraConfiguration CameraConfiguration
        {
            get { return cameraConfiguration; }
            set { cameraConfiguration = value; }
        }

        public MatroxBoardListForm()
        {
            InitializeComponent();

            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName, buttonCancel.Text);

            foreach (var item in Enum.GetValues(typeof(MilSystemType)))
                this.columnSystemType.Items.Add(item);

            foreach (var item in Enum.GetValues(typeof(CameraType)))
                this.columnCameraType.Items.Add(item);

            foreach (var item in Enum.GetValues(typeof(EClientType)))
                this.columnClientType.Items.Add(item);
        }

        private void MatroxBoardListForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < cameraConfiguration.RequiredCameras; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(cameraInfoGrid);

                CameraInfoMil cameraInfoMil = cameraConfiguration.CameraInfos.ElementAtOrDefault(i) as CameraInfoMil;
                if (cameraInfoMil == null)
                {
                    cameraInfoMil = new CameraInfoMil();
                    cameraConfiguration.CameraInfos[i] = cameraInfoMil;
                }

                row.Cells[0].Value = cameraInfoMil.SystemType;
                row.Cells[1].Value = cameraInfoMil.SystemNum;
                row.Cells[2].Value = cameraInfoMil.DigitizerNum;
                row.Cells[3].Value = cameraInfoMil.CameraType;
                row.Cells[4].Value = cameraInfoMil.ClientType;
                row.Cells[5].Value = cameraInfoMil.DcfFilePath;
                row.Tag = cameraInfoMil;

                cameraInfoGrid.Rows.Add(row);
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cameraConfiguration.RequiredCameras; i++)
            {
                DataGridViewRow row = cameraInfoGrid.Rows[i];
                CameraInfoMil cameraInfoMil = row.Tag as CameraInfoMil;

                cameraInfoMil.SystemType = (MilSystemType)row.Cells[0].Value;
                cameraInfoMil.SystemNum = (uint)row.Cells[1].Value;
                cameraInfoMil.DigitizerNum = (uint)row.Cells[2].Value;
                cameraInfoMil.CameraType = (CameraType)row.Cells[3].Value;
                cameraInfoMil.ClientType = (EClientType)row.Cells[4].Value;
                cameraInfoMil.DcfFilePath = (string)row.Cells[5].Value;
            }
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
