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

namespace UniScanS.Screen.UI.Teach.Monitor
{
    public partial class MasterSettingForm : Form
    {
        public MasterSettingForm()
        {
            InitializeComponent();

            UpdateAlgorithmSetting();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SetAlgorithmSetting();

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            foreach (InspectorObj inspector in server.GetInspectorList())
            {
                AlgorithmSetting.Instance().Save(inspector.Info.Path);
            }


            server.AlgorithmSettingChange();

            AlgorithmSetting.Instance().Save();
            Close();
        }

        private void SetAlgorithmSetting()
        {
            AlgorithmSetting.Instance().SheetAttackMinSize = (int)sheetAttackMinSize.Value;
            AlgorithmSetting.Instance().PoleMinSize = (int)poleMinSize.Value;
            AlgorithmSetting.Instance().DielectricMinSize = (int)dielectricMinSize.Value;
            AlgorithmSetting.Instance().PinHoleMinSize = (int)pinHoleMinSize.Value;

            AlgorithmSetting.Instance().PoleLowerWeight = (int)poleRecommendLowerThWeight.Value;
            AlgorithmSetting.Instance().PoleUpperWeight = (int)poleRecommendUpperThWeight.Value;
            AlgorithmSetting.Instance().DielectricLowerWeight = (int)dielectricRecommendLowerThWeight.Value;
            AlgorithmSetting.Instance().DielectricUpperWeight = (int)dielectricRecommendUpperThWeight.Value;

            AlgorithmSetting.Instance().MaxDefectNum = (int)maxDefectNum.Value;
            
            AlgorithmSetting.Instance().XPixelCal = (float)xPixelCal.Value;
            AlgorithmSetting.Instance().YPixelCal = (float)yPixelCal.Value;

            AlgorithmSetting.Instance().DielectricCompactness = (float)dielectricClassification.Value;
            AlgorithmSetting.Instance().PoleCompactness = (float)poleClassification.Value;

            AlgorithmSetting.Instance().DielectricElongation = (float)dielectricElongation.Value;
            AlgorithmSetting.Instance().PoleElongation = (float)poleElongation.Value;

            AlgorithmSetting.Instance().DefectDistance = (float)distance.Value;
        }

        private void UpdateAlgorithmSetting()
        {
            sheetAttackMinSize.Value = AlgorithmSetting.Instance().SheetAttackMinSize;
            poleMinSize.Value = AlgorithmSetting.Instance().PoleMinSize;
            dielectricMinSize.Value = AlgorithmSetting.Instance().DielectricMinSize;
            pinHoleMinSize.Value = AlgorithmSetting.Instance().PinHoleMinSize;

            poleRecommendLowerThWeight.Value = AlgorithmSetting.Instance().PoleLowerWeight;
            poleRecommendUpperThWeight.Value = AlgorithmSetting.Instance().PoleUpperWeight;
            dielectricRecommendLowerThWeight.Value = AlgorithmSetting.Instance().DielectricLowerWeight;
            dielectricRecommendUpperThWeight.Value = AlgorithmSetting.Instance().DielectricUpperWeight;

            maxDefectNum.Value = AlgorithmSetting.Instance().MaxDefectNum;

            xPixelCal.Value = (decimal)AlgorithmSetting.Instance().XPixelCal;
            yPixelCal.Value = (decimal)AlgorithmSetting.Instance().YPixelCal;

            poleClassification.Value = (decimal)AlgorithmSetting.Instance().PoleCompactness;
            dielectricClassification.Value = (decimal)AlgorithmSetting.Instance().DielectricCompactness;

            poleElongation.Value = (decimal)AlgorithmSetting.Instance().PoleElongation;
            dielectricElongation.Value = (decimal)AlgorithmSetting.Instance().DielectricElongation;

            distance.Value = (decimal)AlgorithmSetting.Instance().DefectDistance;

            trackBarPoleClassification.Value = (int)(AlgorithmSetting.Instance().PoleCompactness * 10.0f);
            trackBarDielectricClassification.Value = (int)(AlgorithmSetting.Instance().DielectricCompactness * 10.0f);
            trackBarPoleElongation.Value = (int)(AlgorithmSetting.Instance().PoleElongation * 10.0f);
            trackBarDielectricElongation.Value = (int)(AlgorithmSetting.Instance().DielectricElongation * 10.0f);
        }

        private void poleClassification_ValueChanged(object sender, EventArgs e)
        {
            trackBarPoleClassification.Value = (int)((float)poleClassification.Value * 10.0f);
        }

        private void dielectricClassification_ValueChanged(object sender, EventArgs e)
        {
            trackBarDielectricClassification.Value = (int)((float)dielectricClassification.Value * 10.0f);
        }

        private void poleElongation_ValueChanged(object sender, EventArgs e)
        {
            trackBarPoleElongation.Value = (int)((float)poleElongation.Value * 10.0f);
        }

        private void dielectricElongation_ValueChanged(object sender, EventArgs e)
        {
            trackBarDielectricElongation.Value = (int)((float)dielectricElongation.Value * 10.0f);
        }

        private void trackBarPoleClassification_Scroll(object sender, EventArgs e)
        {
            poleClassification.Value = (decimal)(trackBarPoleClassification.Value / 10.0f);
        }

        private void trackBarDielectricClassification_Scroll(object sender, EventArgs e)
        {
            dielectricClassification.Value = (decimal)(trackBarDielectricClassification.Value / 10.0f);
        }

        private void trackBarPoleElongation_Scroll(object sender, EventArgs e)
        {
            poleElongation.Value = (decimal)(trackBarPoleElongation.Value / 10.0f);
        }

        private void trackBarDielectricElongation_Scroll(object sender, EventArgs e)
        {
            dielectricElongation.Value = (decimal)(trackBarDielectricElongation.Value / 10.0f);
        }

        private void distance_ValueChanged(object sender, EventArgs e)
        {
            //AlgorithmSetting.Instance().DefectDistance = 
        }
    }
}
