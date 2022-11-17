using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using UniScanS.Screen.Vision;
using UniScanS.Common;
using UniScanS.Common.Data;

namespace UniScanS.Data.UI
{
    public partial class AlarmCheckerForm : Form
    {
        AlarmChecker curAlarmChecker;

        public AlarmChecker CurAlarmChecker { get => curAlarmChecker; }

        public AlarmCheckerForm(AlarmChecker alarmChecker = null)
        {
            InitializeComponent();

            alarmType.Items.AddRange(Enum.GetNames(typeof(AlarmType)));
            alarmType.SelectedIndex = 0;
            if (alarmChecker != null)
            {
                curAlarmChecker = alarmChecker;
                UpdateData();

                alarmType.Enabled = false;
            }
        }

        private void UpdateData()
        {
            if (curAlarmChecker == null)
                return;

            switch (curAlarmChecker.AlarmIOType)
            {
                case AlarmIOType.Alarm:
                    radioAlarm.Checked = true;
                    break;
                case AlarmIOType.NG:
                    radioNG.Checked = true;
                    break;
            }
            
            switch (curAlarmChecker.AlarmType)
            {
                case AlarmType.CheckPoint:
                    alarmType.SelectedIndex = 0;
                    UpdateCheckPointData((CheckPointAlarmChecker)curAlarmChecker);
                    break;
                case AlarmType.Recent:
                    alarmType.SelectedIndex = 1;
                    UpdateRecentData((RecentAlarmChecker)curAlarmChecker);
                    break;
                case AlarmType.SamePoint:
                    alarmType.SelectedIndex = 2;
                    UpdateSamePointData((SamePointAlarmChecker)curAlarmChecker);
                    break;
            }
        }

        private void UpdateCheckPointData(CheckPointAlarmChecker alarmChecker)
        {
            checkPointIndex.Value = alarmChecker.CheckIndex;
            checkPointRatio.Value = (decimal)(alarmChecker.Ratio * 100.0f);
        }

        private void UpdateRecentData(RecentAlarmChecker alarmChecker)
        {
            recentNum.Value = alarmChecker.RecentNum;
            recentRatio.Value = (decimal)(alarmChecker.Ratio * 100.0f);

            recentSheetAttack.Checked = alarmChecker.UseSheetAttack;
            recentPole.Checked = alarmChecker.UsePole;
            recentDielectric.Checked = alarmChecker.UseDielectric;
            recentPinHole.Checked = alarmChecker.UsePinHole;
            recentShape.Checked = alarmChecker.UseShape;
        }

        private void UpdateSamePointData(SamePointAlarmChecker alarmChecker)
        {
            samePointNum.Value = alarmChecker.SameNum;

            samePointSheetAttack.Checked = alarmChecker.UseSheetAttack;
            samePointPole.Checked = alarmChecker.UsePole;
            samePointDielectric.Checked = alarmChecker.UseDielectric;
            samePointPinHole.Checked = alarmChecker.UsePinHole;
            samePointShape.Checked = alarmChecker.UseShape;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            AlarmType type = (AlarmType)Enum.Parse(typeof(AlarmType), alarmType.SelectedItem.ToString());

            if (curAlarmChecker == null)
            {
                switch (type)
                {
                    case AlarmType.CheckPoint:
                        curAlarmChecker = new CheckPointAlarmChecker();
                        break;
                    case AlarmType.Recent:
                        curAlarmChecker = new RecentAlarmChecker();
                        break;
                    case AlarmType.SamePoint:
                        curAlarmChecker = new SamePointAlarmChecker();
                        break;
                }
            }

            if (radioAlarm.Checked == true)
                curAlarmChecker.AlarmIOType = AlarmIOType.Alarm;
            else
                curAlarmChecker.AlarmIOType = AlarmIOType.NG;

            switch (type)
            {
                case AlarmType.CheckPoint:
                    CheckPointAlarmChecker checkPointAlarmChecker = (CheckPointAlarmChecker)curAlarmChecker;
                    checkPointAlarmChecker.Ratio = (float)checkPointRatio.Value / 100.0f;
                    checkPointAlarmChecker.CheckIndex = (int)checkPointIndex.Value;
                    break;
                case AlarmType.Recent:
                    RecentAlarmChecker recentAlarmChecker = (RecentAlarmChecker)curAlarmChecker;
                    recentAlarmChecker.Ratio = (float)recentRatio.Value / 100.0f;
                    recentAlarmChecker.RecentNum = (int)recentNum.Value;
                    recentAlarmChecker.UseSheetAttack = recentSheetAttack.Checked;
                    recentAlarmChecker.UsePole = recentPole.Checked;
                    recentAlarmChecker.UseDielectric = recentDielectric.Checked;
                    recentAlarmChecker.UsePinHole = recentPinHole.Checked;
                    recentAlarmChecker.UseShape = recentShape.Checked;
                    break;
                case AlarmType.SamePoint:
                    SamePointAlarmChecker samePointAlarmChecker = (SamePointAlarmChecker)curAlarmChecker;
                    samePointAlarmChecker.SameNum = (int)samePointNum.Value;
                    samePointAlarmChecker.UseSheetAttack = samePointSheetAttack.Checked;
                    samePointAlarmChecker.UsePole = samePointPole.Checked;
                    samePointAlarmChecker.UseDielectric = samePointDielectric.Checked;
                    samePointAlarmChecker.UsePinHole = samePointPinHole.Checked;
                    samePointAlarmChecker.UseShape = samePointShape.Checked;
                    break;
            }

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void alarmType_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlarmType type = (AlarmType)Enum.Parse(typeof(AlarmType), alarmType.SelectedItem.ToString());
            switch (type)
            {
                case AlarmType.CheckPoint:
                    layoutRecent.Visible = false;
                    layoutSamePoint.Visible = false;
                    layoutCheckPoint.Visible = true;
                    break;
                case AlarmType.Recent:
                    layoutCheckPoint.Visible = false;
                    layoutSamePoint.Visible = false;
                    layoutRecent.Visible = true;
                    break;
                case AlarmType.SamePoint:
                    layoutCheckPoint.Visible = false;
                    layoutRecent.Visible = false;
                    layoutSamePoint.Visible = true;
                    break;
            }
        }
    }
}