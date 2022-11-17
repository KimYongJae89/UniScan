using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using DynMvp.Data;


namespace UniScanX.MPAlignment.UI.Components
{
    public delegate void ModelCopiedDelegate(ModelDescription desc);

    public partial class ProductModelCard : UserControl
    {

        [Category("Text"), Description("모델명")]
        public string ModelName
        {
            get { return this.lblModelName.Text; }
            set { this.lblModelName.Text = value; }
        }

        [Category("Text"), Description("마지막 수정된 날짜")]
        public string LastUpdatedTime
        {
            get { return this.lblLastUpdateTime.Text; }
            set { this.lblLastUpdateTime.Text = value; }
        }

        //[Category("Text"), Description("보드 사이즈 ")]
        //public string BoardSize
        //{
        //    get { return this.lblBoardSize.Text; }
        //    set { this.lblBoardSize.Text = value; }
        //}

        public ModelDescription CurModelDescription { get; set;}
        public ModelCopiedDelegate ModelModified;
        public ModelCopiedDelegate ModelCopied;
        public ModelCopiedDelegate ModelDeleted;
        public ModelCopiedDelegate ModelSelected;
        public ModelCopiedDelegate ModelCaptured;


        public ProductModelCard(ModelDescription modelDesc)
        {
            InitializeComponent();
            Updatedata(modelDesc);
            //LastUpdatedTime =
            //BoardSize = string.Format($"{currentModelDescription.BoardSize.Width} X  {currentModelDescription.BoardSize.Height}");
        }

        private void Updatedata(ModelDescription modelDesc)
        {
            this.CurModelDescription = modelDesc;
            ModelName = CurModelDescription.Name;
            var msc = modelDesc as MPAlignment.Data.ModelDescription;
            lblSize.Text = msc.PatternSize.ToString();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var editModelForm = new ModelInfoForm((MPAlignment.Data.ModelDescription)CurModelDescription);
            if (editModelForm.ShowDialog() == DialogResult.OK)
            {
                Updatedata(CurModelDescription);
                ModelModified?.Invoke((DynMvp.Data.ModelDescription)CurModelDescription);
            }
            
        }
        private void btnCopyModel_Click(object sender, EventArgs e)
        {
            var form = new ModelInfoForm((MPAlignment.Data.ModelDescription)CurModelDescription, true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                ModelCopied?.Invoke((DynMvp.Data.ModelDescription)form.NewModelDesc);
                return;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var messageForm = new NoticeMessageForm("Caution", $"Do you want to delete this [{this.CurModelDescription.Name}]?");
            if(messageForm.ShowDialog() == DialogResult.OK)
            {
                ModelDeleted?.Invoke(CurModelDescription);
            }
        }

        void This_MouseHover(object sender, EventArgs e)
        {

        }

        void This_MouseLeave(object sender, EventArgs e)
        {

        }


        private void pnlMain_MouseEnter(object sender, EventArgs e)
        {
            pnlMain.BackColor = Color.DarkGreen;
        }

        private void pnlMain_MouseLeave(object sender, EventArgs e)
        {
            pnlMain.BackColor = Color.DimGray; // pnlMain.BackColor = Color.DarkGreen;
        }

        //double click
        private void ProductModelSelected(object sender, MouseEventArgs e)
        {
            ModelSelected?.Invoke(CurModelDescription);
           //SystemManager.Instance().ModelManager.LoadModel(CurModelDescription, null); //메인 페이지에서 처리
        }
        //click
        private void ProductModelCaptured(object sender, MouseEventArgs e)
        {
            ModelCaptured?.Invoke(CurModelDescription);
            //SystemManager.Instance().ModelManager.LoadCapturedModel(curModelDescription, null);//메인 페이지에서 처리
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
    //        Model model =SystemManager.Instance().ModelManager.LoadModel((ModelDescription)curModelDescription);
        }


    }
}
