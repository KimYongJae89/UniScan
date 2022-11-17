using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniScan.UI
{
    public partial class ChartSettingForm : Form
    {
        // 패널종류추가
        ResultPanelInfo resultPanelInfo;
        ProfilePanel.Info profilePanelInfo;
        TrendPanel.Info trendPanelInfo;
        OverlayPanel.Info overlayPanelInfo;

        public ChartSettingForm(ResultPanelInfo resultPanelInfo)
        {
            InitializeComponent();

            this.resultPanelInfo = resultPanelInfo;
        }

        // 패널종류추가
        private void ChartSettingForm_Load(object sender, EventArgs e)
        {
            switch (resultPanelInfo.PanelType)
            {
                case PanelType.Profile:
                    {
                        profilePanelInfo = (ProfilePanel.Info)resultPanelInfo;

                        max.Value = (decimal)profilePanelInfo.ChartSetting.MaxValue;
                        min.Value = (decimal)profilePanelInfo.ChartSetting.MinValue;
                        standard.Value = (decimal)profilePanelInfo.ChartSetting.StandardValue;
                        start.Value = (decimal)profilePanelInfo.ChartSetting.StartPos;
                        end.Value = (decimal)profilePanelInfo.ChartSetting.EndPos;
                        validStart.Value = (decimal)profilePanelInfo.ChartSetting.ValidStartPos;
                        validEnd.Value = (decimal)profilePanelInfo.ChartSetting.ValidEndPos;
                        upperError.Value = (decimal)profilePanelInfo.ChartSetting.ValueUpperError;
                        upperWarning.Value = (decimal)profilePanelInfo.ChartSetting.ValueUpperWarning;
                        lowerWarning.Value = (decimal)profilePanelInfo.ChartSetting.ValueLowerWarning;
                        lowerError.Value = (decimal)profilePanelInfo.ChartSetting.ValueLowerError;
                        xPitch.Value = (decimal)profilePanelInfo.ChartSetting.XPitch;
                        yPitch.Value = (decimal)profilePanelInfo.ChartSetting.YPitch;
                    }
                    break;

                case PanelType.Trend:
                    {
                        trendPanelInfo = (TrendPanel.Info)resultPanelInfo;

                        max.Value = (decimal)trendPanelInfo.ChartSetting.MaxValue;
                        min.Value = (decimal)trendPanelInfo.ChartSetting.MinValue;
                        standard.Value = (decimal)trendPanelInfo.ChartSetting.StandardValue;
                        start.Value = (decimal)trendPanelInfo.ChartSetting.StartPos;
                        end.Value = (decimal)trendPanelInfo.ChartSetting.EndPos;
                        //validStart.Value = (decimal)trendPanelInfo.ChartSetting.ValidStartPos;
                        //validEnd.Value = (decimal)trendPanelInfo.ChartSetting.ValidEndPos;
                        upperError.Value = (decimal)trendPanelInfo.ChartSetting.ValueUpperError;
                        upperWarning.Value = (decimal)trendPanelInfo.ChartSetting.ValueUpperWarning;
                        lowerWarning.Value = (decimal)trendPanelInfo.ChartSetting.ValueLowerWarning;
                        lowerError.Value = (decimal)trendPanelInfo.ChartSetting.ValueLowerError;
                        xPitch.Value = (decimal)trendPanelInfo.ChartSetting.XPitch;
                        yPitch.Value = (decimal)trendPanelInfo.ChartSetting.YPitch;

                        validStart.Visible = false;
                        validEnd.Visible = false;                        

                        labelValidStart.Visible = false;
                        labelValidEnd.Visible = false;
                    }
                    break;

                case PanelType.Overlay:
                    {
                        overlayPanelInfo = (OverlayPanel.Info)resultPanelInfo;

                        max.Value = (decimal)overlayPanelInfo.ChartSetting.MaxValue;
                        min.Value = (decimal)overlayPanelInfo.ChartSetting.MinValue;
                        standard.Value = (decimal)overlayPanelInfo.ChartSetting.StandardValue;
                        start.Value = (decimal)overlayPanelInfo.ChartSetting.StartPos;
                        end.Value = (decimal)overlayPanelInfo.ChartSetting.EndPos;
                        validStart.Value = (decimal)overlayPanelInfo.ChartSetting.ValidStartPos;
                        validEnd.Value = (decimal)overlayPanelInfo.ChartSetting.ValidEndPos;
                        upperError.Value = (decimal)overlayPanelInfo.ChartSetting.ValueUpperError;
                        upperWarning.Value = (decimal)overlayPanelInfo.ChartSetting.ValueUpperWarning;
                        lowerWarning.Value = (decimal)overlayPanelInfo.ChartSetting.ValueLowerWarning;
                        lowerError.Value = (decimal)overlayPanelInfo.ChartSetting.ValueLowerError;
                        xPitch.Value = (decimal)overlayPanelInfo.ChartSetting.XPitch;
                        yPitch.Value = (decimal)overlayPanelInfo.ChartSetting.YPitch;

                        overlayCount.Value = overlayPanelInfo.OverlayCount;

                        overlayCount.Visible = true;

                        labelOverlayCount.Visible = true;
                    }
                    break;
            }

            xPitch.Visible = false;
            yPitch.Visible = false;

            labelXPtich.Visible = false;
            labelYPitch.Visible = false;
        }

        // 패널종류추가
        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (resultPanelInfo.PanelType)
            {
                case PanelType.Profile:
                    {
                        profilePanelInfo.ChartSetting.MaxValue = Convert.ToSingle(max.Value);
                        profilePanelInfo.ChartSetting.MinValue = Convert.ToSingle(min.Value);
                        profilePanelInfo.ChartSetting.StandardValue = Convert.ToSingle(standard.Value);
                        profilePanelInfo.ChartSetting.StartPos = Convert.ToSingle(start.Value);
                        profilePanelInfo.ChartSetting.ValidStartPos = Convert.ToSingle(validStart.Value);
                        profilePanelInfo.ChartSetting.ValidEndPos = Convert.ToSingle(validEnd.Value);
                        profilePanelInfo.ChartSetting.EndPos = Convert.ToSingle(end.Value);
                        profilePanelInfo.ChartSetting.ValueUpperError = Convert.ToSingle(upperError.Value);
                        profilePanelInfo.ChartSetting.ValueUpperWarning = Convert.ToSingle(upperWarning.Value);
                        profilePanelInfo.ChartSetting.ValueLowerError = Convert.ToSingle(lowerError.Value);
                        profilePanelInfo.ChartSetting.ValueLowerWarning = Convert.ToSingle(lowerWarning.Value);
                        profilePanelInfo.ChartSetting.XPitch = Convert.ToSingle(xPitch.Value);
                        profilePanelInfo.ChartSetting.YPitch = Convert.ToSingle(yPitch.Value);
                    }
                    break;

                case PanelType.Trend:
                    {
                        trendPanelInfo.ChartSetting.MaxValue = Convert.ToSingle(max.Value);
                        trendPanelInfo.ChartSetting.MinValue = Convert.ToSingle(min.Value);
                        trendPanelInfo.ChartSetting.StandardValue = Convert.ToSingle(standard.Value);
                        trendPanelInfo.ChartSetting.StartPos = Convert.ToSingle(start.Value);
                        trendPanelInfo.ChartSetting.ValidStartPos = Convert.ToSingle(validStart.Value);
                        trendPanelInfo.ChartSetting.ValidEndPos = Convert.ToSingle(validEnd.Value);
                        trendPanelInfo.ChartSetting.EndPos = Convert.ToSingle(end.Value);
                        trendPanelInfo.ChartSetting.ValueUpperError = Convert.ToSingle(upperError.Value);
                        trendPanelInfo.ChartSetting.ValueUpperWarning = Convert.ToSingle(upperWarning.Value);
                        trendPanelInfo.ChartSetting.ValueLowerError = Convert.ToSingle(lowerError.Value);
                        trendPanelInfo.ChartSetting.ValueLowerWarning = Convert.ToSingle(lowerWarning.Value);
                        trendPanelInfo.ChartSetting.XPitch = Convert.ToSingle(xPitch.Value);
                        trendPanelInfo.ChartSetting.YPitch = Convert.ToSingle(yPitch.Value);
                    }
                    break;

                case PanelType.Overlay:
                    {
                        overlayPanelInfo.ChartSetting.MaxValue = Convert.ToSingle(max.Value);
                        overlayPanelInfo.ChartSetting.MinValue = Convert.ToSingle(min.Value);
                        overlayPanelInfo.ChartSetting.StandardValue = Convert.ToSingle(standard.Value);
                        overlayPanelInfo.ChartSetting.StartPos = Convert.ToSingle(start.Value);
                        overlayPanelInfo.ChartSetting.ValidStartPos = Convert.ToSingle(validStart.Value);
                        overlayPanelInfo.ChartSetting.ValidEndPos = Convert.ToSingle(validEnd.Value);
                        overlayPanelInfo.ChartSetting.EndPos = Convert.ToSingle(end.Value);
                        overlayPanelInfo.ChartSetting.ValueUpperError = Convert.ToSingle(upperError.Value);
                        overlayPanelInfo.ChartSetting.ValueUpperWarning = Convert.ToSingle(upperWarning.Value);
                        overlayPanelInfo.ChartSetting.ValueLowerError = Convert.ToSingle(lowerError.Value);
                        overlayPanelInfo.ChartSetting.ValueLowerWarning = Convert.ToSingle(lowerWarning.Value);
                        overlayPanelInfo.ChartSetting.XPitch = Convert.ToSingle(xPitch.Value);
                        overlayPanelInfo.ChartSetting.YPitch = Convert.ToSingle(yPitch.Value);

                        overlayPanelInfo.OverlayCount = Convert.ToInt32(overlayCount.Value);
                    }
                    break;
            }

            DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void start_ValueChanged(object sender, EventArgs e)
        {
            if (start.Value > validStart.Value)
            {
                MessageForm.Show(null, "시작 값은 유효 시작 값보다 클 수 없습니다.");
                start.Value = validStart.Value;
            }
        }

        private void validStart_ValueChanged(object sender, EventArgs e)
        {
            if (start.Value > validStart.Value)
            {
                MessageForm.Show(null, "유효 시작 값은 시작 값보다 작을 수 없습니다.");
                validStart.Value = start.Value;
            }
        }

        private void validEnd_ValueChanged(object sender, EventArgs e)
        {
            if (end.Value < validEnd.Value)
            {
                MessageForm.Show(null, "유효 종료 값은 종료 값보다 클 수 없습니다.");
                validEnd.Value = end.Value;
            }
        }

        private void end_ValueChanged(object sender, EventArgs e)
        {
            if (end.Value < validEnd.Value)
            {
                MessageForm.Show(null, "종료 값은 유효 종료 값보다 작을 수 없습니다.");
                end.Value = validEnd.Value;
            }
        }

        

        private void upperError_ValueChanged(object sender, EventArgs e)
        {
            if (upperError.Value < upperWarning.Value)
            {
                MessageForm.Show(null, "에러 값은 경고 값보다 작을 수 없습니다.");
                upperError.Value = upperWarning.Value;
            }
        }

        private void upperWarning_ValueChanged(object sender, EventArgs e)
        {
            if (upperError.Value < upperWarning.Value)
            {
                MessageForm.Show(null, "경고 값은 에러 값보다 클 수 없습니다.");
                upperWarning.Value = upperError.Value;
            }
        }

        private void lowerWarning_ValueChanged(object sender, EventArgs e)
        {
            if (lowerError.Value > lowerWarning.Value)
            {
                MessageForm.Show(null, "경고 값은 에러 값보다 작을 수 없습니다.");
                lowerWarning.Value = lowerError.Value;
            }
        }

        private void lowerError_ValueChanged(object sender, EventArgs e)
        {
            if (lowerError.Value > lowerWarning.Value)
            {
                MessageForm.Show(null, "에러 값은 경고 값보다 클 수 없습니다.");
                lowerError.Value = lowerWarning.Value;
            }
        }
    }
}
