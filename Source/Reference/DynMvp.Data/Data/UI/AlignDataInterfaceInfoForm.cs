using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Data;
using DynMvp.Base;

namespace DynMvp.Data.UI
{
    public partial class AlignDataInterfaceInfoForm : Form
    {
        AlignDataInterfaceInfo alignDataInterfaceInfo;
        public AlignDataInterfaceInfo AlignDataInterfaceInfo
        {
            get { return alignDataInterfaceInfo;  }
            set { alignDataInterfaceInfo = value; }
        }

        public AlignDataInterfaceInfoForm()
        {
            InitializeComponent();

            groupBoxAlignDataInterfaceInfo.Text = StringManager.GetString(this.GetType().FullName,groupBoxAlignDataInterfaceInfo.Text);
            labelOffsetXAddress1.Text = StringManager.GetString(this.GetType().FullName,labelOffsetXAddress1.Text);
            labelOffsetYAddress1.Text = StringManager.GetString(this.GetType().FullName,labelOffsetYAddress1.Text);
            labelAngleAddress.Text = StringManager.GetString(this.GetType().FullName,labelAngleAddress.Text);
            labelOffsetXAddress2.Text = StringManager.GetString(this.GetType().FullName,labelOffsetXAddress2.Text);
            labelOffsetYAddress2.Text = StringManager.GetString(this.GetType().FullName,labelOffsetYAddress2.Text);
            label1.Text = StringManager.GetString(this.GetType().FullName,label1.Text);
            label2.Text = StringManager.GetString(this.GetType().FullName,label2.Text);
            label3.Text = StringManager.GetString(this.GetType().FullName,label3.Text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            alignDataInterfaceInfo.OffsetXAddress1 = Convert.ToInt32(offsetXAddress1.Text);
            alignDataInterfaceInfo.OffsetYAddress1 = Convert.ToInt32(offsetYAddress1.Text);
            alignDataInterfaceInfo.AngleAddress = Convert.ToInt32(angleAddress.Text);
            alignDataInterfaceInfo.OffsetXAddress2 = Convert.ToInt32(offsetXAddress2.Text);
            alignDataInterfaceInfo.OffsetYAddress2 = Convert.ToInt32(offsetYAddress2.Text);
            alignDataInterfaceInfo.XAxisCalibration = Convert.ToSingle(xAxisCalibration.Text);
            alignDataInterfaceInfo.YAxisCalibration = Convert.ToSingle(yAxisCalibration.Text);
            alignDataInterfaceInfo.RAxisCalibration = Convert.ToSingle(rAxisCalibration.Text);
        }

        private void AlignDataInterfaceInfoForm_Load(object sender, EventArgs e)
        {
            offsetXAddress1.Text = alignDataInterfaceInfo.OffsetXAddress1.ToString();
            offsetYAddress1.Text = alignDataInterfaceInfo.OffsetYAddress1.ToString();
            angleAddress.Text = alignDataInterfaceInfo.AngleAddress.ToString();
            offsetXAddress2.Text = alignDataInterfaceInfo.OffsetXAddress2.ToString();
            offsetYAddress2.Text = alignDataInterfaceInfo.OffsetYAddress2.ToString();
            xAxisCalibration.Text = alignDataInterfaceInfo.XAxisCalibration.ToString();
            yAxisCalibration.Text = alignDataInterfaceInfo.YAxisCalibration.ToString();
            rAxisCalibration.Text = alignDataInterfaceInfo.RAxisCalibration.ToString();
        }
    }
}
