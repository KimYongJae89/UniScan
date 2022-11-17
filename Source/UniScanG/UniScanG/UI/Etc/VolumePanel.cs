using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;
using System.IO;
using UniScanG.Common.Settings;
using DynMvp.Data;

namespace UniScanG.UI.Etc
{
    public partial class VolumePanel : UserControl, IStatusStripPanel
    {
        // 3회에 1번 동작.
        const int BlinkTimer = 5;

        DriveInfo driveInfo;
        Color baseColor;

        bool blink = false;
        int blinkCount = 0;
        public VolumePanel(DriveInfo driveInfo)
        {
            InitializeComponent();

            this.baseColor = this.BackColor;
            
            this.driveInfo = driveInfo;
            this.textLabel.Text = driveInfo.Name;
        }

        public void StateUpdate()
        {
            if (this.blinkCount % BlinkTimer == 0)
            {
                float ratio = (1.0f - ((float)driveInfo.TotalFreeSpace / (float)driveInfo.TotalSize)) * 100.0f;

                volumeBar.ToolTipText = string.Format("{0:0.00} %", ratio);
                volumeBar.Value = (int)ratio;

                DataCopier dataCopier = SystemManager.Instance().DataManagerList.Find(f => f is DataCopier) as DataCopier;
                if (dataCopier?.BackupDrive?.Name == this.driveInfo.Name)
                {
                    textLabel.ToolTipText = string.Format("{0} / {1}", dataCopier.BackupProduction.Name, dataCopier.BackupProduction.LotNo);
                    blink = !blink;
                }
                else
                {
                    textLabel.ToolTipText = null;
                    blink = false;
                }
                this.BackColor = Color.FromArgb(blink ? 127 : 0, baseColor);
            }
            this.blinkCount = (this.blinkCount + 1) % BlinkTimer;
        }
    }
}